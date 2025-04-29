using System.IO;
using System.Text;

namespace BerrySonar
{
    public class SonarPositionCache
    {
        private readonly static string filePath = $"{Environment.CurrentDirectory}/SonarPositionCache.json";
        public static async Task CreateFile(Metadata metadata)
        {
            try
            {
                using (FileStream fs = File.Open(filePath, FileMode.Create))
                {
                    string? json = JsonSerialisation.SerializeToJson(metadata);
                    
                    if(json != null)
                    {
                       await fs.WriteAsync(Encoding.UTF8.GetBytes(json)); 
                    }
                }
            }
            catch { }
        }

        public static async Task<Metadata?> ReadFile()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (FileStream fs = File.Open(filePath, FileMode.Open))
                    {
                        byte[] buffer = new byte[fs.Length];
                        await fs.ReadAsync(buffer);

                        string json = Encoding.UTF8.GetString(buffer);
                        return JsonSerialisation.DeserializeFromJson<Metadata>(json);
                    }
                }
                else
                {
                    await CreateFile(new Metadata());
                }
            }
            catch { }

            return null;
        }
    }
}