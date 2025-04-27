namespace BerrySonar
{
    using System.Device.Gpio;
    using System.Timers;

    internal class Program
    {

        // Sonar variables
        private static DateTime startTime = DateTime.Now;
        private static float speed = 34300; // Speed of sound in cm/s


        private static float degree = 0;
        private static int coil = 0;
        private static int step_counter = 0;
        private static bool switch_direction = false;

        // Servo motor GPIO pins [29, 31, 33, 37] (coil1, coil2, coil3, coil4)
        private readonly static int coil1 = 29;
        private readonly static int coil2 = 31;
        private readonly static int coil3 = 33;
        private readonly static int coil4 = 37;


        // Ultrasonic sensor GPIO pins [38, 40] (echo, trigger)
        private readonly static int echo = 38;
        private readonly static int trigger = 40;


        private static GpioController gpioController = new GpioController(PinNumberingScheme.Board);

        static void Main(string[] args)
        {
            try
            {
                InitializePins();
                
                // Attach event handler for echo pin
                gpioController.RegisterCallbackForPinValueChangedEvent(echo, PinEventTypes.Falling | PinEventTypes.Rising, ReadUltrasonicSensor);

                Timer servoTimer = new Timer(10);
                servoTimer.Elapsed += ServoMotorControl;
                servoTimer.Start();

                Timer ultrasonicTimer = new Timer(100);
                ultrasonicTimer.Elapsed += SonarOperation;
                ultrasonicTimer.Start();


                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }
        }

        private static void ServoMotorControl(object? sender, ElapsedEventArgs e)
        {
            //Console.WriteLine($"Coil: {coil}, Step Counter: {step_counter}, Switch Direction: {switch_direction}, Degree: {degree}°");

            if (switch_direction == false)
            {
                if (coil > 4)
                {
                    coil = 0;
                }

                ExecuteServoStep(coil);

                coil++;
                step_counter++;
                degree += 0.17578125f; // 2048 steps => step = 0.17578125° => 0.17578125° * 2048 = 360°

                if (step_counter == 2048)
                {
                    coil = 4;
                    switch_direction = true;
                }
            }
            else
            {
                if (coil == 0)
                {
                    coil = 4;
                }

                ExecuteServoStep(coil);

                coil--;
                step_counter--;

                if (step_counter == 0)
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
            Console.WriteLine($"{e.ChangeType} detected on pin {echo}");
            if (e.ChangeType == PinEventTypes.Falling)
            {
                double duration = (DateTime.Now - startTime).TotalSeconds;
                double distance = duration * speed / 2; // Divide by 2 because the signal travels to the object and back

                Console.WriteLine($"Distance: {distance} cm");
            }
        }


        private static void SonarOperation(object? sender, ElapsedEventArgs e) => TriggerUltrasonicSensor();


        private static void ExecuteServoStep(int coil)
        {
            switch (coil)
            {
                case 0:
                    gpioController.Write(coil1, PinValue.High);
                    gpioController.Write(coil2, PinValue.Low);
                    gpioController.Write(coil3, PinValue.Low);
                    gpioController.Write(coil4, PinValue.Low);
                    break;
                case 1:
                    gpioController.Write(coil1, PinValue.Low);
                    gpioController.Write(coil2, PinValue.High);
                    gpioController.Write(coil3, PinValue.Low);
                    gpioController.Write(coil4, PinValue.Low);
                    break;
                case 2:
                    gpioController.Write(coil1, PinValue.Low);
                    gpioController.Write(coil2, PinValue.Low);
                    gpioController.Write(coil3, PinValue.High);
                    gpioController.Write(coil4, PinValue.Low);
                    break;
                case 3:
                    gpioController.Write(coil1, PinValue.Low);
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

                //gpioController.UnregisterCallbackForPinValueChangedEvent(echo, ReadUltrasonicSensor);
            }
            catch { }
        }


        ~Program()
        {
            UninitializePins();
            gpioController?.Dispose();
        }
    }
}