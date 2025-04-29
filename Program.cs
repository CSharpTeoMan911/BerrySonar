namespace BerrySonar
{
    using System.Device.Gpio;
    using System.Timers;


    internal class Program : Firebase
    {

        // Sonar variables
        private static DateTime startTime = DateTime.Now;
        private static float speed = 34300; // Speed of sound in cm/s


        private static float degree = 0;
        private static int step = 0;
        private static int step_counter = 0;
        private static double distance = 0;
        private static bool switch_direction = false;

        // Servo motor GPIO pins [29, 31, 33, 37] (coil1, coil2, coil3, coil4)
        private readonly static int coil1 = 29;
        private readonly static int coil2 = 31;
        private readonly static int coil3 = 33;
        private readonly static int coil4 = 37;


        // Ultrasonic sensor GPIO pins [38, 40] (echo, trigger)
        private readonly static int echo = 38;
        private readonly static int trigger = 40;

        private static bool errorShutdown = false;

        private static GpioController gpioController = new GpioController(PinNumberingScheme.Board);

        static void Main()
        {
            Console.CancelKeyPress += (s, e) => Shutdown();
            AppDomain.CurrentDomain.ProcessExit += (s, e) => Shutdown();
            _ = Operation();
            Console.ReadKey();
        }

        private static async Task Operation()
        {
            try
            {
                Console.WriteLine("Starting Sonar...");
                UninitializePins();

                Config config = await SonarConfiguration.ReadConfig();
                Metadata? metadata = await SonarPositionCache.ReadFile();

                InitateDatabase(config);

                degree = metadata?.degree ?? 0;
                step = metadata?.step ?? 0;
                step_counter = metadata?.step_counter ?? 0;
                switch_direction = metadata?.switch_direction ?? false;

                InitializePins();

                Timer servoTimer = new Timer(10);
                servoTimer.Elapsed += ServoMotorControl;
                servoTimer.Start();

                Timer ultrasonicPulse = new Timer(100);
                ultrasonicPulse.Elapsed += SonarOperation;
                ultrasonicPulse.Start();

                Timer databaseWriter = new Timer(100);
                databaseWriter.Elapsed += UpdatePositionData;
                databaseWriter.Start();

                gpioController.RegisterCallbackForPinValueChangedEvent(echo, PinEventTypes.Falling | PinEventTypes.Rising, ReadUltrasonicSensor);
            }
            catch { }
        }

        private static async void UpdatePositionData(object? sender, ElapsedEventArgs e)
        {
            if (errorShutdown == false)
            {
                errorShutdown = await UpdateSonarData(new SonarMetadata
                {
                    degree = degree,
                    distance = distance
                });
            }
            else
            {
                ((Timer?)sender)?.Stop();
                Console.WriteLine("Error detected. Shutting down...");
                Environment.Exit(0);
            }
        }

        private static void ServoMotorControl(object? sender, ElapsedEventArgs e)
        {
            //Console.WriteLine($"Step: {step}, Step Counter: {step_counter}, Switch Direction: {switch_direction}, Degree: {degree}°");

            if (switch_direction == false)
            {
                ExecuteServoStep(step);
                step++;
                step_counter++;
                degree += 0.17578125f;

                if (step > 3) step = 0; // correct range check
                if (step_counter >= 1024)
                {
                    switch_direction = true;
                }
            }
            else
            {
                ExecuteServoStep(step);
                step--;
                step_counter--;
                degree -= 0.17578125f;

                if (step < 0) step = 3; // reverse range check
                if (step_counter <= 0)
                {
                    switch_direction = false;
                }
            }
        }


        private static void TriggerUltrasonicSensor()
        {
            startTime = DateTime.Now;
            gpioController.Write(trigger, PinValue.High);
            gpioController.Write(trigger, PinValue.Low);
        }

        private static void ReadUltrasonicSensor(object? sender, PinValueChangedEventArgs e)
        {
            if (errorShutdown == false)
            {
                if (e.ChangeType == PinEventTypes.Falling)
                {
                    double duration = (DateTime.Now - startTime).TotalSeconds;
                    distance = duration * speed / 2; // Divide by 2 because the signal travels to the object and back

                    Console.WriteLine($"Distance: {distance} cm");
                }
            }
            else
            {
                Console.WriteLine("Error detected. Shutting down...");

                Environment.Exit(0);
            }
        }


        private static void SonarOperation(object? sender, ElapsedEventArgs e) => TriggerUltrasonicSensor();


        private static void ExecuteServoStep(int step)
        {
            switch (step)
            {
                case 0:
                    gpioController.Write(coil1, PinValue.High);
                    gpioController.Write(coil2, PinValue.High);
                    gpioController.Write(coil3, PinValue.Low);
                    gpioController.Write(coil4, PinValue.Low);
                    break;
                case 1:
                    gpioController.Write(coil1, PinValue.Low);
                    gpioController.Write(coil2, PinValue.High);
                    gpioController.Write(coil3, PinValue.High);
                    gpioController.Write(coil4, PinValue.Low);
                    break;
                case 2:
                    gpioController.Write(coil1, PinValue.Low);
                    gpioController.Write(coil2, PinValue.Low);
                    gpioController.Write(coil3, PinValue.High);
                    gpioController.Write(coil4, PinValue.High);
                    break;
                case 3:
                    gpioController.Write(coil1, PinValue.High);
                    gpioController.Write(coil2, PinValue.Low);
                    gpioController.Write(coil3, PinValue.Low);
                    gpioController.Write(coil4, PinValue.High);
                    break;
            }
        }


        private static void InitializePins()
        {
            try
            {
                // Initialize GPIO pins for servo motor
                gpioController.OpenPin(coil1, PinMode.Output);
                gpioController.OpenPin(coil2, PinMode.Output);
                gpioController.OpenPin(coil3, PinMode.Output);
                gpioController.OpenPin(coil4, PinMode.Output);


                // Initialize GPIO pins for ultrasonic sensor
                gpioController.OpenPin(echo, PinMode.InputPullDown);
                gpioController.OpenPin(trigger, PinMode.Output);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing pins: {ex.Message}");
            }
        }




        private static void UninitializePins()
        {
            try
            {
                // Uninitialize GPIO pins for servo motor
                gpioController.ClosePin(coil1);
                gpioController.ClosePin(coil2);
                gpioController.ClosePin(coil3);
                gpioController.ClosePin(coil4);


                // Uninitialize GPIO pins for ultrasonic sensor
                gpioController.ClosePin(echo);
                gpioController.ClosePin(trigger);

                gpioController.UnregisterCallbackForPinValueChangedEvent(echo, ReadUltrasonicSensor);
            }
            catch { }
        }


        private static async void UpdatePositionCacheFile()
        {
            Metadata metadata = new Metadata
            {
                degree = degree,
                step = step,
                step_counter = step_counter,
                switch_direction = switch_direction
            };

            await SonarPositionCache.CreateFile(metadata);
        }

        private static async void Shutdown()
        {
            UninitializePins();
            gpioController?.Dispose();

            await SonarPositionCache.CreateFile(new Metadata
            {
                degree = degree,
                step = step,
                step_counter = step_counter,
                switch_direction = switch_direction
            });

            errorShutdown = await UpdateSonarData(new SonarMetadata
            {
                degree = degree,
                distance = distance
            });
        }
    }
}