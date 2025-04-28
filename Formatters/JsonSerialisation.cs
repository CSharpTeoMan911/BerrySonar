using Newtonsoft.Json;

namespace BerrySonar
{
    class JsonSerialisation
    {
        public static string? SerializeToJson<ObjectType>(ObjectType? data)
        {
            try
            {
                if (data != null)
                {
                    using (StringWriter st_writer = new StringWriter())
                    {
                        using (JsonWriter reader = new JsonTextWriter(st_writer))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.Serialize(reader, data);
                            return st_writer.ToString();
                        }
                    }
                }
            }
            catch { }

            return null;
        }

        public static ObjectType? DeserializeFromJson<ObjectType>(string? data)
        {
            try
            {
                if (data != null)
                {
                    using (StringReader st_reader = new StringReader(data))
                    {
                        using (JsonReader reader = new JsonTextReader(st_reader))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            return serializer.Deserialize<ObjectType>(reader);
                        }
                    }
                }

            }
            catch { }

            return (ObjectType?)(object?)null;
        }
    }
}