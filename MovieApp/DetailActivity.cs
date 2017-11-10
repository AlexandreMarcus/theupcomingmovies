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
using Newtonsoft.Json;
using Android.Graphics;
using System.Net;

[assembly: UsesPermission(Android.Manifest.Permission.Internet)]
namespace MovieApp
{
    [Activity(NoHistory = true)]
    
    public class DetailActivity : Activity
    {
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

        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            // Create your application here
            SetContentView(Resource.Layout.Detail);
            Button buttonClose = FindViewById<Button>(Resource.Id.button1);

            string endereoDetail = Intent.GetStringExtra("MyData1") ?? "Data not available";
            string texto = Intent.GetStringExtra("MyData2") ?? "Data not available";
            string path = Intent.GetStringExtra("MyData3") ?? "Data not available";

            string imagem = "https://image.tmdb.org/t/p/w500" + path;



            buttonClose.Click += delegate
            {
                StartActivity(typeof(MainActivity));
            };
            
            var text = FindViewById<TextView>(Resource.Id.editText1);
            text.Text = texto;
            var image = FindViewById<ImageView>(Resource.Id.imageView1);
            
            var imageBitmap = GetImageBitmapFromUrl(imagem);
            image.SetImageBitmap(imageBitmap);

        }

        
    }
}