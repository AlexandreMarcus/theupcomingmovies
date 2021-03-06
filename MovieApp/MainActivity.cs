﻿using Android.App;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Android.Media;
using Android.Graphics.Drawables;
using Android.Views;
using System.Threading;
using System;
using Android.Content;
using Android.Graphics;

namespace MovieApp
{
    [Activity(Label = "The Upcoming Movie App", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        IList<Filme> listaFilmes = new List<Filme>();


        public static List<string> items = new List<string>();
        public static List<string> itemsIndex = new List<string>();
        string apiKey = "1f54bd990f1cdfb230adb312546d765d";
        bool first = true;
        //Exemplo imagem: 
        //https://image.tmdb.org/t/p/w500/kqjL17yufvn9OVLyXYpvtyrFfak.jpg

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            if (first == true)
            {
                first = false;
                CarregarGeneros();
                CarregarFilmes();
            }
        }



        private void CarregarGeneros()
        {
            //List of Genres
            string apiKey = "1f54bd990f1cdfb230adb312546d765d";
            string endereco = "https://api.themoviedb.org/3/genre/movie/list?api_key=" + apiKey + "&language=en-US";

            string responseFromServer = CallRequest(endereco);
            var listaGen = JsonConvert.DeserializeObject<RootObject>(responseFromServer);
            foreach (var i in listaGen.genres)
            {
                Genero reg = new Genero();
                reg.genero = i.name;
                reg.idGenero = i.id;
                listaGenero.Add(reg);
            }
        }

        //Image
        public class Backdrop
        {
            public double aspect_ratio { get; set; }
            public string file_path { get; set; }
            public int height { get; set; }
            public string iso_639_1 { get; set; }
            public double vote_average { get; set; }
            public int vote_count { get; set; }
            public int width { get; set; }
        }

        public class Poster
        {
            public double aspect_ratio { get; set; }
            public string file_path { get; set; }
            public int height { get; set; }
            public string iso_639_1 { get; set; }
            public double vote_average { get; set; }
            public int vote_count { get; set; }
            public int width { get; set; }
        }

        public class RootObjectImage
        {
            public int id { get; set; }
            public List<Backdrop> backdrops { get; set; }
            public List<Poster> posters { get; set; }
        }



        //Genre

        public class Genre
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class RootObject
        {
            public List<Genre> genres { get; set; }
        }

        IList<Genero> listaGenero = new List<Genero>();
        public class Movie
        {
            public int vote_count { get; set; }
            public int id { get; set; }
            public bool video { get; set; }
            public double vote_average { get; set; }
            public string title { get; set; }
            public double popularity { get; set; }
            public string poster_path { get; set; }
            public string original_language { get; set; }
            public string original_title { get; set; }
            public List<int> genre_ids { get; set; }
            public string backdrop_path { get; set; }
            public bool adult { get; set; }
            public string overview { get; set; }
            public string release_date { get; set; }
            public string maximum { get; set; }
            public string minimum { get; set; }
            public List<Movie> results { get; set; }
            public int page { get; set; }
            public int total_results { get; set; }
            //public DateTime dates { get; set; }
            public int total_pages { get; set; }
        }
        public class UpcomingMovies
        {
            public virtual int ID { get; set; }
            public virtual string titulo { get; set; }
            public virtual string descricao { get; set; }

            public virtual string data { get; set; }
            public virtual Image poster { get; set; }
        }
        public class GeneroJson
        {
            public virtual int id { get; set; }
            public virtual string name { get; set; }
        }
        public class Genero
        {
            public virtual int idGenero { get; set; }
            public virtual string genero { get; set; }
        }

        public void list_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string end = "";
            string textoFilme = "";
            string url = "";
            try
            {
                //Get our item from the list adapter
                var list = FindViewById<ListView>(Resource.Id.listView1);
                var item = list.GetItemAtPosition(e.Position);
                string id = "0";
                for (int i = 0; i < itemsIndex.Count; i++)
                {
                    try
                    {
                        string[] linhaArray = itemsIndex[i].Split('-');
                        string filme = linhaArray[1].Trim();
                        string filmeClicado = item.ToString().Split('-')[0].Trim();
                        if (filme == filmeClicado)
                        {
                            id = linhaArray[0];
                        }
                    }
                    catch
                    {

                    }
                }

                if (id == "0")
                {
                    Toast.MakeText(this, "Movie not found!", ToastLength.Short).Show();
                }
                else
                {
                    //List of Details                
                    string endereoDetail = "https://api.themoviedb.org/3/movie/" + id + "?api_key=" + apiKey + "&language=en-US";
                    end = endereoDetail;
                    string responseFromServerImage = CallRequest(endereoDetail);
                    var detail = JsonConvert.DeserializeObject<RootObjectDetail>(responseFromServerImage);
                    string textoFilmeTela = item.ToString() + " Overview: " + detail.overview;
                    textoFilme = textoFilmeTela;
                    url = detail.poster_path;

                    //Make a toast with the item name just to show it was clicked
                    Toast.MakeText(this, textoFilmeTela, ToastLength.Short).Show();
                }
            }
            catch
            {
                Toast.MakeText(this, "Sorry something went wrong!", ToastLength.Long).Show();
            }




            var atividade2 = new Intent(this, typeof(DetailActivity));
            atividade2.PutExtra("MyData1", end);
            atividade2.PutExtra("MyData2", textoFilme);
            atividade2.PutExtra("MyData3", url);
            StartActivity(atividade2);






        }



        private void CarregarFilmes()
        {
            string pagina = "1";

            string endereco = "https://api.themoviedb.org/3/movie/upcoming?api_key=" + apiKey + "&language=en-US&page=" + pagina;

            string responseFromServer = CallRequest(endereco);

            var mov = JsonConvert.DeserializeObject<Movie>(responseFromServer);


            //string[] items = new string[20 * 10];

            //var grid = FindViewById<GridView>(Resource.Id.gridView1);
            var list = FindViewById<ListView>(Resource.Id.listView1);
            //Wire up the click event            
            list.ItemClick += list_ItemClick;



            int contador = 0;

            //foreach (var i in mov.results)
            //{
            //    string genero = "";
            //    for (int g = 0; g < i.genre_ids.Count; g++)
            //    {
            //        if (genero.Length > 0)
            //        {
            //            genero += "/";
            //        }
            //        var itemGenero = listaGenero.Where(x => x.idGenero == i.genre_ids[g]).ToList()[0];
            //        genero += itemGenero.genero;
            //    }

            //    //List of Details                
            //    string endereoDetail = "https://api.themoviedb.org/3/movie/" + i.id + "?api_key=" + apiKey + "&language=en-US";
            //    string responseFromServerImage = CallRequest(endereoDetail);
            //    var detail = JsonConvert.DeserializeObject<RootObjectDetail>(responseFromServerImage);
            //    string caminhoImagem = detail.poster_path;

            //    items[contador] = i.title + " - " + caminhoImagem + " - " + genero + " - " + i.release_date;
            //    contador++;

            //}



            //for (int i = 1; i < mov.total_pages - 1; i++)
            contador = 0;
            items.Add("Title" + "   ImagePath" + "    Genre" + "     Release Date");
            for (int i = 1; i < 4; i++)
            {
                pagina = i.ToString();
                endereco = "https://api.themoviedb.org/3/movie/upcoming?api_key=" + apiKey + "&language=en-US&page=" + pagina;
                string retorno = CallRequest(endereco);

                var movies = JsonConvert.DeserializeObject<Movie>(retorno);
                foreach (var m in movies.results)
                {

                    try
                    {
                        contador++;

                        string genero = "";
                        for (int g = 0; g < m.genre_ids.Count; g++)
                        {
                            if (genero.Length > 0)
                            {
                                genero += "/";
                            }
                            var itemGenero = listaGenero.Where(x => x.idGenero == m.genre_ids[g]).ToList()[0];
                            genero += itemGenero.genero;
                        }

                        //List of Details                
                        string endereoDetail = "https://api.themoviedb.org/3/movie/" + m.id + "?api_key=" + apiKey + "&language=en-US";
                        string responseFromServerImage = CallRequest(endereoDetail);
                        var detail = JsonConvert.DeserializeObject<RootObjectDetail>(responseFromServerImage);
                        //string caminhoImagem = detail.poster_path;
                        string caminhoImagem = "?";

                        items.Add(detail.title + " - " + caminhoImagem + " - " + genero + " - " + detail.release_date);
                        itemsIndex.Add(detail.id + "-" + detail.title + " - " + caminhoImagem + " - " + genero + " - " + detail.release_date);

                        Filme reg = new Filme();
                        reg.Descricao = detail.title + " - " + caminhoImagem + " - " + genero + " - " + detail.release_date;
                        reg.Id = detail.id;


                        string im = "https://image.tmdb.org/t/p/w500" + detail.poster_path;


                        Bitmap imageBitmap = null;
                        using (var webClient = new WebClient())
                        {
                            var imageBytes = webClient.DownloadData(im);
                            if (imageBytes != null && imageBytes.Length > 0)
                            {
                                imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                            }
                        }
                        reg.Poster = imageBitmap;

                        FilmesRepositorio.Filmes.Add(reg);

                    }
                    catch (Exception ex)
                    {

                    }
                }

            }

            var listAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, items.ToArray());
            list.Adapter = listAdapter;

        }
        private static string CallRequest(string endereco)
        {
            try
            {
                //Request para a URL. 
                Thread.Sleep(100);
                WebRequest request = WebRequest.Create(endereco);
                request.Credentials = CredentialCache.DefaultCredentials;
                WebResponse response = request.GetResponse();
                string retornoStatus = ((HttpWebResponse)response).StatusDescription;
                System.IO.Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return responseFromServer;
            }
            catch
            {
                return "N";
            }


        }
        public class ProductionCompany
        {
            public string name { get; set; }
            public int id { get; set; }
        }

        public class ProductionCountry
        {
            public string iso_3166_1 { get; set; }
            public string name { get; set; }
        }

        public class SpokenLanguage
        {
            public string iso_639_1 { get; set; }
            public string name { get; set; }
        }

        public class RootObjectDetail
        {
            public bool adult { get; set; }
            public string backdrop_path { get; set; }
            public object belongs_to_collection { get; set; }
            public int budget { get; set; }
            public List<Genre> genres { get; set; }
            public string homepage { get; set; }
            public int id { get; set; }
            public string imdb_id { get; set; }
            public string original_language { get; set; }
            public string original_title { get; set; }
            public string overview { get; set; }
            public double popularity { get; set; }
            public string poster_path { get; set; }
            public List<ProductionCompany> production_companies { get; set; }
            public List<ProductionCountry> production_countries { get; set; }
            public string release_date { get; set; }
            public int revenue { get; set; }
            public int runtime { get; set; }
            public List<SpokenLanguage> spoken_languages { get; set; }
            public string status { get; set; }
            public string tagline { get; set; }
            public string title { get; set; }
            public bool video { get; set; }
            public double vote_average { get; set; }
            public int vote_count { get; set; }
        }

    }


    public class Filme
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public Bitmap Poster { get; set; }

        public override string ToString()
        {
            return Descricao;
        }
    }

    public static class FilmesRepositorio
    {
        public static List<Filme> Filmes { get; private set; }

        static FilmesRepositorio()
        {
            Filmes = new List<Filme>();

            // AddFilmes();


        }

        //private static void AddFilmes()
        //{
        //    foreach (var i in listaFilmes)
        //    {

        //    }
        //    Filmes.Add(new Filme
        //    {
        //        Id = 1,
        //        //Titulo = "A New Hope",
        //        //Diretor = "George Lucas",
        //        //DataLancamento = new DateTime(1977, 05, 25)
        //    });

        //}


    }


    public class FilmeAdapter : BaseAdapter<Filme>
    {
        private readonly Activity context;
        private readonly List<Filme> filmes;

        public FilmeAdapter(Activity context, List<Filme> filmes)
        {
            this.context = context;
            this.filmes = filmes;
        }

        public override Filme this[int position]
        {
            get
            {
                return filmes[position];
            }
        }

        public override int Count
        {
            get
            {
                return filmes.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return filmes[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.Main, parent, false);

            var txtTitulo = view.FindViewById<TextView>(Resource.Id.editText1);
            var image = view.FindViewById<ImageView>(Resource.Id.imageView1);

            txtTitulo.Text = filmes[position].Descricao;
            image.SetImageBitmap(filmes[position].Poster);

            return view;
        }
    }


}

