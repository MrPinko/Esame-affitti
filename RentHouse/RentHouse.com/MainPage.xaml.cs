using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace RentHouse.com
{
	public partial class MainPage : ContentPage
	{
		private List<string> houseImageList = new List<string>();
		private List<Pin> appartamentiList = new List<Pin>();
		private List<Pin> attrazionipinList = new List<Pin>();
		private List<string> appartamenti_ImmaginiList = new List<string>();
		private Location location;

		ObservableCollection<AppartamentiPosizione> appartamentiJson;
		ObservableCollection<Review> reviewJson;
		ObservableCollection<AppartamentiImmagini> appartamentiImmaginiJson;

		private bool bottomBarUp = false, isExpanded = false;
		string userEmail;
		int cityID;

		public MainPage(string userEmail) { }

		public MainPage()
		{
			InitializeComponent();
			BindingContext = this;
			NavigationPage.SetHasNavigationBar(this, false);
			customMap();

			getRequestForAppartamenti();

			getrequestForRecensioni();

			getRequestForAppartamentiImmagini();

			foreach (AppartamentiPosizione item in appartamentiJson)
			{
				double latWithDot = toDouble(item.lat);
				double longWithDot = toDouble(item.@long);

				appartamentiList.Add(
					new Pin
					{
						Type = PinType.Place,
						Label = item.nomeAppartamento,
						Address = item.via + " " + item.numeroC,
						Position = new Position(Convert.ToDouble(item.lat), Convert.ToDouble(item.@long))
					});


			}

			this.userEmail = userEmail;

			//aggiugno i pin alla mappa 
			foreach (Pin pin in appartamentiList)
			{
				Console.WriteLine(pin.Label);
				Console.WriteLine(pin.Position.Latitude);
				Console.WriteLine(pin.Position.Longitude);

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
				carousel.IsSwipeEnabled = false;
			}
			Console.WriteLine("tapped at " + e.Point.Latitude + "," + e.Point.Longitude);
		}

		//evento click sui pin della mappa
		private void map_PinClicked(object sender, PinClickedEventArgs e)
		{
			if (e.Pin.Type == PinType.Place)
			{
				//caricamento delle immagini

				appartamenti_ImmaginiList.Clear();

				foreach (AppartamentiImmagini ai in appartamentiImmaginiJson)
				{
					if (ai.nome.ToLower().Equals(e.Pin.Label.ToLower()))
					{
						appartamenti_ImmaginiList.Add(ai.url);
					}
				}

				AppartamentiImmaginiFromUrl appartamentiImmagini = new AppartamentiImmaginiFromUrl(appartamenti_ImmaginiList[0], appartamenti_ImmaginiList[1], appartamenti_ImmaginiList[2]);
				carousel.BindingContext = appartamentiImmagini;

				if (bottomBarUp)
				{
					bottomBar.TranslateTo(0, 0, 300);
					bottomBarUp = false;
				}
				else             //la barra sotto non è visibile
				{
					bottomBar.TranslateTo(0, -bottomBar.Height / 3, 350);
					houseName.Text = e.Pin.Label.ToUpper();

					carousel.HeightRequest = bottomBar.Height / 3;
					carousel.WidthRequest = bottomBar.Width;

					//caricamento delle review
					foreach (Review re in reviewJson)
					{
						if (re.nome.ToLower().Equals(e.Pin.Label.ToLower()))
						{
							posizioneStar.Text = re.avg_posizione.Substring(0,1);
							qpStar.Text = re.avg_qualita_prezzo.Substring(0, 1);
							servizioStar.Text = re.avg_servizio.Substring(0, 1);

							break;
						}
					}

					foreach (AppartamentiPosizione item in appartamentiJson)
					{
						nomeProprietario.Text = item.nomeProprietario.ToLower();
						Cognomeproprietario.Text = item.cognomeProprietario.ToLower();
						iban.Text = item.iban.ToUpper();
					}


						bottomBarUp = true;
				}
			}
		}

		private void expandBottomBar(object sender, SwipedEventArgs e)       //massima altezza
		{
			bottomBar.TranslateTo(0, -bottomBar.Height, 350);
			isExpanded = true;
			carousel.IsSwipeEnabled = true;
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

			//houseImage.IsSwipeEnabled = false;

		}

		private bool isUserPopUpVisible = false;
		private void userImage_tapped(object sender, EventArgs e)
		{
			if (!isUserPopUpVisible)
			{
				userPopUp.IsVisible = true;
				userPopUp.IsEnabled = true;

				isUserPopUpVisible = true;
			}
			else
			{
				userPopUp.IsVisible = false;
				userPopUp.IsEnabled = false;

				isUserPopUpVisible = false;
			}
		}

		private void logoutButton_Tapped(object sender, EventArgs e)
		{
			Navigation.PushAsync(new loginUser());
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

			var tr = JsonConvert.DeserializeObject<List<Review>>(myXMLstring);
			//After deserializing , we store our data in the List called ObservableCollection
			reviewJson = new ObservableCollection<Review>(tr);
		}

		private void getRequestForAppartamentiImmagini()
		{
			var url = "http://rosafedericoesame.altervista.org/index.php/user/appartamentiImmagini";
			var myXMLstring = "";
			Task task = new Task(() =>
			{
				myXMLstring = AccessTheWebAsync(url).Result;
			});
			task.Start();
			task.Wait();
			Console.WriteLine(myXMLstring);

			var tr = JsonConvert.DeserializeObject<List<AppartamentiImmagini>>(myXMLstring);
			//After deserializing , we store our data in the List called ObservableCollection
			appartamentiImmaginiJson = new ObservableCollection<AppartamentiImmagini>(tr);
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
				if (appartamentiJson[i].nomeAppartamento.Equals(s))
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

		#region double formattati bene grazie pietro
		private static double toDouble(string s)      //ovviamente va usata la virgola e non il punto per formattare un Double ...
		{
			double d = 0;
			bool isNegative = false;
			//leggo la parte prima del punto
			int i = 0;
			int decimalIndex = 0;
			StringBuilder b = new StringBuilder("");

			if (s[0] == '-')
			{
				isNegative = true;
				i++;
			}

			for (; i < s.Length; i++)
			{
				if (s[i] == '.')
				{
					decimalIndex = i;
					i++;
					break;
				}
				d *= 10;
				d += toInt(s[i]);
			}
			for (; i < s.Length; i++)
			{
				if (s[i] == 'E' || s[i] == 'e')
				{
					i++;
					break;
				}
				d += toInt(s[i]) * Math.Pow(10, -(i - decimalIndex));
			}
			for (; i < s.Length; i++)
			{
				b.Append(s[i]);
			}
			if (b.ToString() != "")
			{
				int n = Convert.ToInt32(b.ToString());
				d *= Math.Pow(10, n);
			}
			if (isNegative)
				d = -d;

			return d;
		}

		private static int toInt(char c)
		{
			return (Convert.ToInt32(c) - 48);
		}
		#endregion


	}
}

#region classi per json

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
	public string nomeAppartamento { get; set; }
	public string piano { get; set; }
	public string superficie { get; set; }
	public string costo { get; set; }
	public string lat { get; set; }
	public string @long { get; set; }
	public string via { get; set; }
	public string numeroC { get; set; }
	public string nomeProprietario { get; set; }
	public string cognomeProprietario { get; set; }
	public string provider { get; set; }
	public string nome { get; set; }
	public string iban { get; set; }
}

public class Review
{
	public string nome { get; set; }
	public string avg_posizione { get; set; }
	public string avg_qualita_prezzo { get; set; }
	public string avg_servizio { get; set; }
}

public class AttrazioniCordEImmagini
{
	public string nome { get; set; }
	public string lat { get; set; }
	public string @long { get; set; }
	public string url { get; set; }
}

public class AppartamentiImmagini
{
	public string nome { get; set; }
	public string url { get; set; }
}
#endregion

public class CarouselModel
{
	public CarouselModel(string imagestr)
	{
		Image = imagestr;
	}
	private string _image;

	public string Image
	{
		get { return _image; }
		set { _image = value; }
	}
}

public class AppartamentiImmaginiFromUrl
{
	public class StringData
	{
		public string uri { get; set; }
	}

	public List<StringData> uris { get; set; }

	public void clearUrl()
	{
		//uris.Clear();
	}

	public AppartamentiImmaginiFromUrl(string url1, string url2, string url3)
	{
		uris = new List<StringData>() {
			new StringData() { uri = url1 },
			new StringData() { uri = url2 },
			new StringData() { uri = url3 },
		};
	}
}