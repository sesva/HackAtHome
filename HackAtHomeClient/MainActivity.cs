using Android.App;
using Android.Widget;
using Android.OS;
using HackAtHome.SAL;
using HackAtHome.Entities;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/Logo")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            var ediTexCorreo = FindViewById<EditText>(Resource.Id.ediTexCorreo);
            var ediTextContraseña = FindViewById<EditText>(Resource.Id.ediTexContraseña);
            var btnValidar = FindViewById<Button>(Resource.Id.btnValidar);
            btnValidar.Click += (s, ev) =>
            {
                validar(ediTexCorreo.Text, ediTextContraseña.Text);
            };

        }

        public async void validar(string correoEstudiante, string contraseñaEstudiante)
        { 
            var serviceClient = new ServiceClient();
            ResultInfo resultInfo = await serviceClient.AutenticateAsync(correoEstudiante, contraseñaEstudiante);

            validarMicrosoft(correoEstudiante);

            if ((int)resultInfo.Status == 1)
            {                
                var Intent = new Android.Content.Intent(this, typeof(Evidencias));
                Intent.PutExtra("token", resultInfo.Token);
                Intent.PutExtra("nombre", resultInfo.FullName);
                StartActivity(Intent);
            }
            else
            {
                var AlertDialog = new AlertDialog.Builder(this);
                AlertDialog.SetMessage("Ops, ha ocurrido un problema, revisa tus credenciales.");
                AlertDialog.SetNegativeButton("Aceptar", delegate { });
                AlertDialog.Show();
            }
            
        }

        private async void validarMicrosoft(string correoEstudiante)
        {
            var microsoftServiceClient = new MicrosoftServiceClient();
            var labItem = new LabItem
            {
                Email = correoEstudiante,
                Lab = "Hack@Home",
                DeviceId = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId)
            };
            await microsoftServiceClient.SendEvidence(labItem);
        }
    }
}

