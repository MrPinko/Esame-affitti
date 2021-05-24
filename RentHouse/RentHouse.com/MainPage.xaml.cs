﻿using System;
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

		private List<Pin> appartamentiList = new List<Pin>();
		private List<string> appartamenti_ImmaginiList = new List<string>();

		ObservableCollection<AppartamentiPosizione> appartamentiJson;
		ObservableCollection<Review> reviewJson;
		ObservableCollection<AppartamentiImmagini> appartamentiImmaginiJson;
		ObservableCollection<AttrazioniCordEImmagini> attrazioniCordEImmaginiJson;
		ObservableCollection<DateDisponibili> dateDisponibiliJson;
		ObservableCollection<CodiceFiscaleUtente> codiceFiscaleUtenteJson;


		private bool bottomBarUp = false, isExpanded = false;

		public MainPage(string username)
		{
			InitializeComponent();
			BindingContext = this;
			NavigationPage.SetHasNavigationBar(this, false);
			customMap();

			UserNameLabel.Text = username;

			getRequestForAppartamenti();

			getrequestForRecensioni();

			getRequestForAppartamentiImmagini();

			getRequestForAttrazioniCordEImmagini();

			getDateDisponibili();

			getRequestForCodiceFiscale();

			foreach (AppartamentiPosizione item in appartamentiJson)
			{
				double latWithDot = toDouble(item.lat);
				double longWithDot = toDouble(item.@long);

				appartamentiList.Add(
					new Pin
					{
						Type = PinType.Place,
						Label = item.nomeAppartamento,
						Icon = BitmapDescriptorFactory.FromBundle("home"),
						Address = item.via + " " + item.numeroC,
						Position = new Position(Convert.ToDouble(item.lat), Convert.ToDouble(item.@long))
					});
			}


			foreach (AttrazioniCordEImmagini item in attrazioniCordEImmaginiJson)
			{
				double latWithDot = toDouble(item.lat);
				double longWithDot = toDouble(item.@long);

				appartamentiList.Add(
					new Pin
					{
						Type = PinType.SearchResult,
						Label = item.nome,
						Icon = BitmapDescriptorFactory.FromBundle("tourist"),
						//Address = item.via + " " + item.numeroC,
						Position = new Position(Convert.ToDouble(item.lat), Convert.ToDouble(item.@long))
					});
			}

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
				bottomBar.TranslateTo(0, 0, 200);
				bottomBarUp = false;
				carousel.IsSwipeEnabled = false;
			}
			if (isUserPopUpVisible)
			{
				userPopUp.TranslateTo(0, 0, 200);
				isUserPopUpVisible = false;
			}
			Console.WriteLine("tapped at " + e.Point.Latitude + "," + e.Point.Longitude);
		}

		//evento click sui pin della mappa
		private void map_PinClicked(object sender, PinClickedEventArgs e)
		{

			if (e.Pin.Type == PinType.Place)        //popup degli apartamenti
			{
				descrizioneposto.Text = "";
				prenotaGriglia.IsVisible = true;
				prenotaGriglia.IsEnabled = true;
				datiBottomPopUp.IsVisible = true;
				datiBottomPopUp.IsEnabled = true;
				containerDatiAppartamento.IsVisible = true;
				containerDatiAppartamento.IsEnabled = true;

				appartamenti_ImmaginiList.Clear();
				foreach (AppartamentiImmagini ai in appartamentiImmaginiJson)
				{
					if (ai.nome.ToLower().Equals(e.Pin.Label.ToLower()))
					{
						appartamenti_ImmaginiList.Add(ai.url);
					}
				}

				AppartamentiImmaginiFromUrl attrazioniImmagini = new AppartamentiImmaginiFromUrl(appartamenti_ImmaginiList[0], appartamenti_ImmaginiList[1], appartamenti_ImmaginiList[2]);
				carousel.BindingContext = attrazioniImmagini;

				foreach (AppartamentiPosizione obj in appartamentiJson)
				{
					if (obj.nomeAppartamento.ToLower().Equals(e.Pin.Label.ToLower()))
					{
						prezzoAppartamento.Text = "Costo: " + obj.costo;
						pianoAppartamento.Text = "Piano: " + obj.piano;
						superficieAppartamento.Text = "Superficie: " + obj.superficie;
					}
				}

				getDateDisponibili();
				datePicker.Items.Clear();
				foreach (DateDisponibili DD in dateDisponibiliJson)              //date picker
				{
					if (DD.nome.ToLower().Equals(e.Pin.Label.ToLower()))
					{
						datePicker.Items.Add(Convert.ToDateTime(DD.dataInizio).ToString("dd-MM-yyyy") + " fino a " + Convert.ToDateTime(DD.dataFine).ToString("dd-MM-yyyy"));
					}
				}

				if (bottomBarUp)       //abbasso la barra se clicco un altro pin e la barra sotto è visibile    
				{
					bottomBar.TranslateTo(0, 0, 300);
					bottomBarUp = false;
				}
				else             //la barra sotto non è visibile quindi la porto su
				{
					bottomBar.TranslateTo(0, -bottomBar.Height / 3, 350);
					houseName.Text = e.Pin.Label.ToUpper();

					userPopUp.TranslateTo(0, 0, 300);           //nascondo la barra laterale se apro la barra sotto
					isUserPopUpVisible = false;

					carousel.HeightRequest = bottomBar.Height / 3;
					carousel.WidthRequest = bottomBar.Width;

					//caricamento delle review
					foreach (Review re in reviewJson)
					{
						if (re.nome.ToLower().Equals(e.Pin.Label.ToLower()))
						{
							posizioneStar.Text = re.avg_posizione.Substring(0, 1);
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
			else if (e.Pin.Type == PinType.SearchResult)       //apro il popup dele attrazioni turistiche
			{
				datiBottomPopUp.IsVisible = false;
				datiBottomPopUp.IsEnabled = false;
				prenotaGriglia.IsVisible = false;
				prenotaGriglia.IsEnabled = false;
				containerDatiAppartamento.IsVisible = false;
				containerDatiAppartamento.IsEnabled = false;
				foreach (AttrazioniCordEImmagini item in attrazioniCordEImmaginiJson)
				{
					if (e.Pin.Label.ToLower().Equals(item.nome.ToLower()))
					{        //trovo il posto selezionato
						descrizioneposto.Text = item.descrizione.ToLower();
						AttrazioniImmaginiFromUrl attrazioniImmagini = new AttrazioniImmaginiFromUrl(item.url);
						carousel.BindingContext = attrazioniImmagini;
						break;
					}
				}

				if (bottomBarUp)            //abbasso la barra se clicco un altro pin e la barra sotto è visibile   serve per il tocco    
				{
					bottomBar.TranslateTo(0, 0, 200);
					bottomBarUp = false;
				}
				else             //la barra sotto non è visibile quindi la porto su
				{
					userPopUp.TranslateTo(0, 0, 200);
					isUserPopUpVisible = false;

					bottomBar.TranslateTo(0, -bottomBar.Height / 3, 350);
					houseName.Text = e.Pin.Label.ToUpper();

					carousel.HeightRequest = bottomBar.Height / 3;
					carousel.WidthRequest = bottomBar.Width;
				}

				bottomBarUp = true;

			}
		}

		private void expandBottomBar(object sender, SwipedEventArgs e)       //massima altezza     serve per i drag
		{
			bottomBar.TranslateTo(0, -bottomBar.Height, 350);
			isExpanded = true;
			carousel.IsSwipeEnabled = true;
		}

		private void retriveBottomBar(object sender, SwipedEventArgs e)     // 1/3 di altezza o tutto giù serve per i drag 
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

			carousel.IsSwipeEnabled = false;

		}

		private bool isUserPopUpVisible = false;
		private void userImage_tapped(object sender, EventArgs e)         //compare il box utente
		{
			if (!isUserPopUpVisible)      //barra laterale diventa visibile
			{

				containerOrdiniCompletati.Children.Add(new Label
				{
					Text = "test",
					TextColor = Color.Black
				});

				userPopUp.TranslateTo(-userPopUp.Width, 0, 300);
				bottomBar.TranslateTo(0, 0, 200);

				bottomBarUp = false;
				isUserPopUpVisible = true;
			}
			else
			{
				userPopUp.TranslateTo(0, 0, 300);
				isUserPopUpVisible = false;
			}
		}

		private void logoutButton_Tapped(object sender, EventArgs e)
		{
			Navigation.PushAsync(new loginUser());
		}

		private void Riserva_Tapped(object sender, EventArgs e)      //riserva la data
		{
			if (datePicker.SelectedItem != null)
			{
				postRequest();
			}
			else
			{
				DisplayAlert("errore", "inserire una data valida", "ok");
			}
		}

		#region richieste GET
		//auto esplicative
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

		private void getRequestForAttrazioniCordEImmagini()
		{
			var url = "http://rosafedericoesame.altervista.org/index.php/user/attrazioniTuristiche";
			var myXMLstring = "";
			Task task = new Task(() =>
			{
				myXMLstring = AccessTheWebAsync(url).Result;
			});
			task.Start();
			task.Wait();
			Console.WriteLine(myXMLstring);

			var tr = JsonConvert.DeserializeObject<List<AttrazioniCordEImmagini>>(myXMLstring);
			//After deserializing , we store our data in the List called ObservableCollection
			attrazioniCordEImmaginiJson = new ObservableCollection<AttrazioniCordEImmagini>(tr);
		}

		private void getRequestForCodiceFiscale()
		{
			var url = "http://rosafedericoesame.altervista.org/index.php/user/getCF/" + UserNameLabel.Text;
			var myXMLstring = "";
			Task task = new Task(() =>
			{
				myXMLstring = AccessTheWebAsync(url).Result;
			});
			task.Start();
			task.Wait();
			Console.WriteLine(myXMLstring);

			var tr = JsonConvert.DeserializeObject<List<CodiceFiscaleUtente>>(myXMLstring);
			//After deserializing , we store our data in the List called ObservableCollection
			codiceFiscaleUtenteJson = new ObservableCollection<CodiceFiscaleUtente>(tr);
		}

		private void getDateDisponibili()
		{
			var url = "http://rosafedericoesame.altervista.org/index.php/user/dateDisponibili";
			var myXMLstring = "";
			Task task = new Task(() =>
			{
				myXMLstring = AccessTheWebAsync(url).Result;
			});
			task.Start();
			task.Wait();
			Console.WriteLine(myXMLstring);

			var tr = JsonConvert.DeserializeObject<List<DateDisponibili>>(myXMLstring);
			//After deserializing , we store our data in the List called ObservableCollection
			dateDisponibiliJson = new ObservableCollection<DateDisponibili>(tr);
		}

		private async void postRequest()
		{
			//02-07-2021 fino a 09-07-2021
			int id = 0;
			foreach (DateDisponibili DD in dateDisponibiliJson)              //date picker
			{
				if ((Convert.ToDateTime(DD.dataInizio).ToString("dd-MM-yyyy") + " fino a " + Convert.ToDateTime(DD.dataFine).ToString("dd-MM-yyyy")).Equals(datePicker.SelectedItem))      //creo la stringa come è nel picker
				{
					id = Convert.ToInt32(DD.idUtente_Apaprtamenti);
					break;
				}

			}

			var client = new HttpClient();
			Uri uri = new Uri("http://rosafedericoesame.altervista.org/index.php/user/addUserToAppartamenti");

			string jsonData = "{" +
							"\"fk_utente\" : \"" + codiceFiscaleUtenteJson[0].cf_utente +
							"\", \"idUtente_Appartamenti\" : \"" + id +
							"\"}";


			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await client.PostAsync(uri, content);

			// this result string should be something like: "{"token":"rgh2ghgdsfds"}"
			var result = await response.Content.ReadAsStringAsync();

			Console.WriteLine(result);
		}


		async Task<String> AccessTheWebAsync(String url)
		{
			HttpClient client = new HttpClient();

			Task<string> getStringTask = client.GetStringAsync(url);

			string urlContents = await getStringTask;

			return urlContents;
		}

		#endregion

		#region mappa moddata e onback

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
		private static double toDouble(string s)      //ovviamente va usata la virgola e non il punto per formattare un Double
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
	public string descrizione { get; set; }
	public string lat { get; set; }
	public string @long { get; set; }
	public string url { get; set; }
}

public class AppartamentiImmagini
{
	public string nome { get; set; }
	public string url { get; set; }
}

public class DateDisponibili
{
	public string idUtente_Apaprtamenti { get; set; }
	public string nome { get; set; }
	public string dataInizio { get; set; }
	public string dataFine { get; set; }
}

public class CodiceFiscaleUtente
{
	public string cf_utente { get; set; }
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

	public AppartamentiImmaginiFromUrl(string url1, string url2, string url3)
	{
		uris = new List<StringData>() {
			new StringData() { uri = url1 },
			new StringData() { uri = url2 },
			new StringData() { uri = url3 },
		};
	}
}


public class AttrazioniImmaginiFromUrl
{
	public class StringData
	{
		public string uri { get; set; }
	}

	public List<StringData> uris { get; set; }

	public AttrazioniImmaginiFromUrl(string url1)
	{
		uris = new List<StringData>() {
			new StringData() { uri = url1 }
		};
	}
}
