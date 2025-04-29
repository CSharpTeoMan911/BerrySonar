using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Database;
using Firebase.Database.Query;

namespace BerrySonar
{
    public class Firebase
    {
        private static FirebaseClient? firebaseClient;

        protected static void InitateDatabase(Config config) => firebaseClient = new FirebaseClient(
                                                                    config.FirebaseDatabaseUrl,
                                                                    new FirebaseOptions
                                                                    {
                                                                        AuthTokenAsyncFactory = () => Task.FromResult(config.FirebaseDatabaseSecret)
                                                                    });

        protected static async Task<bool> UpdateSonarData(SonarMetadata sonarData)
        {
            try
            {
                if (firebaseClient != null)
                    await firebaseClient.Child("SonarData").PutAsync(JsonSerialisation.SerializeToJson(sonarData), TimeSpan.FromSeconds(2));
                return false;
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("\n\nError writing to the database. Check the Firebase configuration and/or the database permissions.\n\n");
                return true;
            }
        }
    }
}