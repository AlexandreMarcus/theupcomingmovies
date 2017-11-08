using Android.App;
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

namespace MovieApp
{
    [Activity(Label = "The Upcoming Movie App", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static List<string> items = new List<string>();
        public static List<string> itemsIndex = new List<string>();
        string apiKey = "1f54bd990f1cdfb230adb312546d765d";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            CarregarGeneros();
            CarregarFilmes();

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
                    string responseFromServerImage = CallRequest(endereoDetail);
                    var detail = JsonConvert.DeserializeObject<RootObjectDetail>(responseFromServerImage);
                    string textoFilme = item.ToString() + " Overview: " + detail.overview;

                    //Make a toast with the item name just to show it was clicked
                    Toast.MakeText(this, textoFilme, ToastLength.Short).Show();
                }
            }
            catch
            {
                Toast.MakeText(this, "Sorry something went wrong!", ToastLength.Long).Show();
            }
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



                    }
                    catch (Exception ex)
                    {

                    }
                }

            }

            var listAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, items.ToArray());
            list.Adapter = listAdapter;


            //   this.radGridView1.Rows.Add(i.id, i.title, i.overview);

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
}

