using Android.App;
using Android.Widget;
using Android.OS;
using Entities;
using SAL;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/android")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);


            var email = FindViewById<EditText>(Resource.Id.editTextEmail);
            var password = FindViewById<EditText>(Resource.Id.editTextPassword);
            var btnValidate = FindViewById<Button>(Resource.Id.buttonValidate);

            btnValidate.Click += (object sender, System.EventArgs e) =>
            {
                Validate(email.Text, password.Text);
            };
            SendEvidence(email.Text);
        }

        /// Método para autenticar el correo electrónico y la contraseña
        public async void Validate(string email, string password)
        {
            var ServiceClient = new ServiceClient();
            var resultado = await ServiceClient.AutenticateAsync(email, password);

            if (resultado.Status == Status.Success)
            {
                var ActivityIntent =
                    new Android.Content.Intent(this, typeof(EvidencesActivity));
                    ActivityIntent.PutExtra("Token", resultado.Token);
                    ActivityIntent.PutExtra("FullName", resultado.FullName);
                    StartActivity(ActivityIntent);
            }
            else
            {
                AlertDialog.Builder Builder =
                    new AlertDialog.Builder(this);
                    AlertDialog Alert = Builder.Create();
                    Alert.SetTitle("ERROR");
                    Alert.SetMessage("Datos incorrectos");
                    Alert.SetButton("Ok", (s, ev) => { });
                Alert.Show();
            }
        }

        /// Método para enviar la evidencia a Microsoft
        public async void SendEvidence(string email)
        {
            var MicrosoftEvidence = new LabItem
            {
                Email = email,
                Lab = "@string/ApplicationName",
                DeviceId = Android.Provider.Settings.Secure.GetString(
                    ContentResolver, Android.Provider.Settings.Secure.AndroidId)
            };
            var MicrosoftClient = new MicrosoftServiceClient();
            await MicrosoftClient.SendEvidence(MicrosoftEvidence);
        }
    }
}

