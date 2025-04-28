using System.Text;

namespace BerrySonar
{
    public class SonarConfiguration
    {
        public static async Task<Config> ReadConfig()
        {
            Config defaultConfig = new Config();
            string configFilePath = Path.Combine(Environment.CurrentDirectory, "config.json");

            if (File.Exists(configFilePath) == true)
            {
                using(FileStream fs = File.Open(configFilePath, FileMode.Open))
                {
                    byte[] buffer = new byte[fs.Length];
                    await fs.ReadAsync(buffer);

                    string json = Encoding.UTF8.GetString(buffer);
                    Config? config = JsonSerialisation.DeserializeFromJson<Config?>(json);

                    return config == null ? defaultConfig : config;
                }
            }
            else
            {
                await CreateConfigFile();

                Console.Clear();

                Console.WriteLine("\n\nConfig file not found. A new config file has been created.\n\n");
                
                Environment.Exit(0);
            }

            return defaultConfig;
        }


        private static async Task CreateConfigFile()
        {
            Config defaultConfig = new Config();
            string path = Path.Combine(Environment.CurrentDirectory, "config.json");

            Console.WriteLine("Creating config file...");
            Console.WriteLine($"Path: {path}");

            using (FileStream fs = File.Open(path, FileMode.Create))
            {
                byte[] file = Encoding.UTF8.GetBytes(JsonSerialisation.SerializeToJson(defaultConfig, true));
                await fs.WriteAsync(file);
                await fs.FlushAsync();
            }
        }
    }
}