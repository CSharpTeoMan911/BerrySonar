using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Database;

namespace BerrySonar
{
    public class Firebase
    {
        private static FirebaseClient? firebaseClient;

        private static async Task LoadConfig()
        {

        } 


        protected static void InitialiseDatabase(Config config)
        {

            firebaseClient = new FirebaseClient(
                "https://your-database-url.firebaseio.com/",
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Login(config)
                });
        }


        protected static async Task<string> Login(Config config)
        {   await client.CreateUserWithEmailAndPasswordAsync("email", "pwd", "Display Name");
            return "";
        }
    }
}