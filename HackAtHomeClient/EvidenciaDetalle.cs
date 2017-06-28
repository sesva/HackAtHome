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
using Android.Graphics;
using System.Net;

namespace HackAtHomeClient
{
    [Activity(Label = "EvidenciaDetalle")]
    public class EvidenciaDetalle : Activity
    {
        private TextView txtContenidoDescrip;
        private ImageView imgEvidencia;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.EvidenciaDetalle);

            var txtNombre = FindViewById<TextView>(Resource.Id.txtNombre);
            var txtTituloEvidencia = FindViewById<TextView>(Resource.Id.txtEvidencia);
            var txtEstado = FindViewById<TextView>(Resource.Id.txtEstado);
            txtContenidoDescrip = FindViewById<TextView>(Resource.Id.txtContenidoDescripcion);
            imgEvidencia = FindViewById<ImageView>(Resource.Id.imgEvidencia);

            txtNombre.Text = Intent.Extras.GetString("nombre");
            txtTituloEvidencia.Text = Intent.Extras.GetString("tituloEvidencia");
            txtEstado.Text = Intent.Extras.GetString("statusEvidencia");
            int idEvidencia = Intent.Extras.GetInt("IDEvidencia");
            var token  = Intent.Extras.GetString("token");
            getEvidenceDetail(token,idEvidencia);
        }

        public async void getEvidenceDetail(string token, int id)
        {
            var serviceClient = new ServiceClient();
            var evidenceDetail = await serviceClient.GetEvidenceByIDAsync(token,id);
            txtContenidoDescrip.Text = evidenceDetail.Description;
            var imageBitmap = GetImageBitmapFromUrl(evidenceDetail.Url);
            imgEvidencia.SetImageBitmap(imageBitmap);
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
    }
}