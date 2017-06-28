using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HackAtHome.SAL;
using HackAtHome.Entities;

namespace HackAtHomeClient
{
    [Activity(Label = "Evidencias")]
    public class Evidencias : Activity
    {
        private ListView lvEvidencias;
        List<Evidence> listEvidencias;
        Complex Data;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.layoutEvidencia);
            var txtNombre = FindViewById<TextView>(Resource.Id.txtNombre);
            var token = Intent.Extras.GetString("token");
            txtNombre.Text = Intent.Extras.GetString("nombre");
            lvEvidencias = FindViewById<ListView>(Resource.Id.lvEvidencias);
            lvEvidencias.ItemClick += (s,ev) =>
            {
                var evidenciaSeleted = listEvidencias[ev.Position];
                Intent intent = new Intent(this,typeof(EvidenciaDetalle));
                intent.PutExtra("IDEvidencia", evidenciaSeleted.EvidenceID);
                intent.PutExtra("tituloEvidencia", evidenciaSeleted.Title);
                intent.PutExtra("statusEvidencia",evidenciaSeleted.Status);
                intent.PutExtra("nombre",txtNombre.Text);
                intent.PutExtra("token",token); 
                StartActivity(intent);
            };
            if (bundle != null)
            {                
                Android.Util.Log.Debug("Log", "Recovered Instance State");
                Toast.MakeText(this, "Not reload Evidencias", ToastLength.Long).Show();
            }
            else
            {
                getEvidencias(token);
                Toast.MakeText(this,"Get Evidencias",ToastLength.Long).Show();
            }
            Data = (Complex)this.FragmentManager.FindFragmentByTag("Data");
            if (Data == null)
            {
                // No ha sido almacenado, agregar el fragmento a la Activity
                Data = new Complex();
                var FragmentTransaction = this.FragmentManager.BeginTransaction();
                FragmentTransaction.Add(Data, "Data");
                FragmentTransaction.Commit();
            }
        }

        public async void getEvidencias(string token)
        {
            var serviceClient = new ServiceClient();
            listEvidencias = await serviceClient.GetEvidencesAsync(token);            
            lvEvidencias.Adapter = new HackAtHome.CustomAdapter.EvidencesAdapter
                (
                    this,
                    listEvidencias,
                    Resource.Layout.ListItem,
                    Resource.Id.txtNombre,
                    Resource.Id.txtEstado
                );            
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);            
        }
    }
}