using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Database;
using Firebase.Database.Query;

namespace BerrySonar
{
    public class Firebase
    {

        private static bool isFirebaseInitialized = false;
        private static int retries = 0;

        private static FirebaseClient? firebaseClient;

        protected static void InitateDatabase(Config config) => firebaseClient = new FirebaseClient(
                                                                    config.FirebaseDatabaseUrl,
                                                                    new FirebaseOptions
                                                                    {
                                                                        AuthTokenAsyncFactory = () => Task.FromResult(config.FirebaseDatabaseSecret)
                                                                    });

        protected static async Task UpdateSonarData(SonarMetadata sonarData)
        {
            try
            {
                if (firebaseClient != null)
                    await firebaseClient.Child("SonarData").PutAsync(JsonSerialisation.SerializeToJson(sonarData), TimeSpan.FromSeconds(10));
                isFirebaseInitialized = true;
                retries = 0;
            }
            catch
            {
                if (isFirebaseInitialized == false)
                {
                    OnError(true);
                }
                else
                {
                    retries++;
                    if (retries > 5)
                    {
                        OnError(false);
                    }
                }
            }
        }


        private static void OnError(bool init_error)
        {
            Console.Clear();
            Console.WriteLine($"\n\nError writing to the database. {(init_error == true 
            ? "Check the Firebase configuration and/or the database permissions" 
            : "Check your internet connection and/or if you reached the Firebase plan limits")}\n\n");
            Environment.Exit(1);
        }
    }
}