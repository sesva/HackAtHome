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
        Complex dataEvidencias;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

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
            getEvidencias(token);
        }

        public async void getEvidencias(string token)
        {
            dataEvidencias = (Complex)this.FragmentManager.FindFragmentByTag("DataEvidencias");
            if (dataEvidencias == null)
            {
                
                // No ha sido almacenado, agregar el fragmento a la Activity
                dataEvidencias = new Complex();
                var FragmentTransaction = this.FragmentManager.BeginTransaction();
                FragmentTransaction.Add(dataEvidencias, "DataEvidencias");
                FragmentTransaction.Commit();
                var serviceClient = new ServiceClient();
                listEvidencias = await serviceClient.GetEvidencesAsync(token);
                dataEvidencias.Evidences = listEvidencias;
            }
            else
            {
                listEvidencias = dataEvidencias.Evidences;
            }
                       
            lvEvidencias.Adapter = new HackAtHome.CustomAdapter.EvidencesAdapter
                (
                    this,
                    listEvidencias,
                    Resource.Layout.ListItem,
                    Resource.Id.txtNombre,
                    Resource.Id.txtEstado
                );            
        }

    }
}