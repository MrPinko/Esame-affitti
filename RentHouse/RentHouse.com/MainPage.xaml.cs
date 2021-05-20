using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace RentHouse.com
{
	public partial class MainPage : ContentPage
	{
		private List<Attrazioni> attrazioniList;
		private List<string> houseImageList = new List<string>();
		private List<Pin> AppartamentiList = new List<Pin>();
		private List<Pin> attrazionipinList = new List<Pin>();
		ObservableCollection<AppartamentiPosizione> appartamentiJson;
		ObservableCollection<ReviewAndImage> reviewJson;
		private bool bottomBarUp = false, isExpanded = false;
		Location location;
		string userEmail;
		int cityID;


		public MainPage(string userEmail) { }

		public MainPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			customMap();

			getRequestForAppartamenti();

			getrequestForRecensioni();

			foreach (AppartamentiPosizione item in appartamentiJson)
			{
				AppartamentiList.Add(
					new Pin
					{
						Type = PinType.Place,
						Label = item.nome,
						Address = item.via + " " + item.numeroC,
						Position = new Position(Convert.ToDouble(item.lat), Convert.ToDouble(item.@long))
					});
			}

			this.userEmail = userEmail;

			location = new Location();

			//aggiugno i pin alla mappa 
			foreach (Pin pin in AppartamentiList)
			{
				map.Pins.Add(pin);
			}

			//muovo la telecamera sulla città cercata
			//var latitudine = location.GetLocation().Result.Latitude;
			//var longitudine = location.GetLocation().Result.Longitude;

			//map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(latitudine,longitudine), Distance.FromMeters(5000)));   //si posiziona sulla posizione corrente


		}

		//evento click sulla mappa
		private void map_MapClicked(object sender, MapClickedEventArgs e)
		{
			if (bottomBarUp)
			{
				bottomBar.TranslateTo(0, 0, 300);
				bottomBarUp = false;
			}
			Console.WriteLine("tapped at " + e.Point.Latitude + "," + e.Point.Longitude);
		}

		//evento click sui pin della mappa
		private void map_PinClicked(object sender, PinClickedEventArgs e)
		{

			if (e.Pin.Type == PinType.Place)
			{
				int cityID = findID(e.Pin.Label.ToString());             //cerco l'id del pin selezionato nel json

				//caricamento delle immagini
				foreach (string s in reviewJson[cityID].url)
				{
					houseImageList.Add(s);
				}

				if (bottomBarUp)
				{
					bottomBar.TranslateTo(0, 0, 300);
					bottomBarUp = false;
				}
				else
				{
					bottomBar.TranslateTo(0, -bottomBar.Height / 3, 350);
					houseName.Text = appartamentiJson[cityID].nome.ToUpper();

					houseImage.ItemsSource = houseImageList;
					houseImage.HeightRequest = bottomBar.Height / 3;
					houseImage.WidthRequest = bottomBar.Width;

					//caricamento delle review
					posizioneStar.Text = reviewJson[cityID].avg_posizione;
					qpStar.Text = reviewJson[cityID].avg_qualita_prezzo;
					servizioStar.Text = reviewJson[cityID].avg_servizio;

					bottomBarUp = true;
				}
			}
		}

		

		private void expandBottomBar(object sender, SwipedEventArgs e)
		{
			bottomBar.TranslateTo(0, -bottomBar.Height, 350);
			isExpanded = true;
		}

		private void retriveBottomBar(object sender, SwipedEventArgs e)
		{
			if (isExpanded)
			{
				bottomBar.TranslateTo(0, -bottomBar.Height / 3, 350);
				isExpanded = false;
			}
			else
			{
				bottomBar.TranslateTo(0, 0, 350);
				bottomBarUp = false;
			}
		}


		#region richieste GET

		private void getRequestForAppartamenti()
		{
			var url = "http://rosafedericoesame.altervista.org/index.php/user/appartamenti";
			var myXMLstring = "";
			Task task = new Task(() =>
			{
				myXMLstring = AccessTheWebAsync(url).Result;
			});
			task.Start();
			task.Wait();
			Console.WriteLine(myXMLstring);

			var tr = JsonConvert.DeserializeObject<List<AppartamentiPosizione>>(myXMLstring);
			//After deserializing , we store our data in the List called ObservableCollection
			appartamentiJson = new ObservableCollection<AppartamentiPosizione>(tr);
		}

		private void getrequestForRecensioni()
		{
			var url = "http://rosafedericoesame.altervista.org/index.php/user/review";
			var myXMLstring = "";
			Task task = new Task(() =>
			{
				myXMLstring = AccessTheWebAsync(url).Result;
			});
			task.Start();
			task.Wait();
			Console.WriteLine(myXMLstring);

			var tr = JsonConvert.DeserializeObject<List<ReviewAndImage>>(myXMLstring);
			//After deserializing , we store our data in the List called ObservableCollection
			reviewJson = new ObservableCollection<ReviewAndImage>(tr);
		}

		async Task<String> AccessTheWebAsync(String url)
		{
			HttpClient client = new HttpClient();

			Task<string> getStringTask = client.GetStringAsync(url);

			string urlContents = await getStringTask;

			return urlContents;
		}

		#endregion

		#region findID mappa moddata e onback

		private int findID(String s)
		{
			for (int i = 0; i < appartamentiJson.Count; i++)
			{
				if (appartamentiJson[i].nome.Equals(s))
					return i;
			}

			return 0;
		}

		private void customMap()       //caricare la mappa da map wizard
		{
			var assembly = typeof(MainPage).GetTypeInfo().Assembly;
			Stream stream = assembly.GetManifestResourceStream("RentHouse.com.GoogleMap.json");
			string Json = "";
			using (var reader = new StreamReader(stream))
			{
				Json = reader.ReadToEnd();
			}
			map.MapStyle = MapStyle.FromJson(Json);
		}


		protected override bool OnBackButtonPressed()
		{
			System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
			return true;
		}

		#endregion

	}
}

#region classi per json

public class City
{
	public string name { get; set; }
	public string city { get; set; }
	public List<string> image { get; set; }
	public string address { get; set; }
	public double Lat { get; set; }
	public double Long { get; set; }
	public int posizioneStar { get; set; }
	public int qpStar { get; set; }
	public int servizioStar { get; set; }


}

public class Attrazioni
{
	public string ccomune { get; set; }
	public string cprovincia { get; set; }
	public string cregione { get; set; }
	public string cnome { get; set; }
	public string canno_inserimento { get; set; }
	public DateTime cdata_e_ora_inserimento { get; set; }
	public string cidentificatore_in_openstreetmap { get; set; }
	public string clongitudine { get; set; }
	public string clatitudine { get; set; }
}

public class AppartamentiPosizione
{
	public string nome { get; set; }
	public string piano { get; set; }
	public string superficie { get; set; }
	public string costo { get; set; }
	public string lat { get; set; }
	public string @long { get; set; }
	public string via { get; set; }
	public string numeroC { get; set; }
}

public class ReviewAndImage
{
	public string nome { get; set; }
	public List<string> url { get; set; }
	public string avg_posizione { get; set; }
	public string avg_qualita_prezzo { get; set; }
	public string avg_servizio { get; set; }
}

#endregion


