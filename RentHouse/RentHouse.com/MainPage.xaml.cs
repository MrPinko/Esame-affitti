using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Clipboard;
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
		ObservableCollection<OrdiniEffettuati> ordiniEffettuatiJson;
		ObservableCollection<Servizi> serviziJson;


		private bool bottomBarUp = false, isExpanded = false, isReviewContainer = false;
		private double prezzoAppartamentoSelezionato;

		private string username, MPagamento;

		private Label tempProvvider, tempNome;
		private BoxView boxView;

		public MainPage(string username, string MPagamento)
		{
			InitializeComponent();
			BindingContext = this;

			this.username = username;
			this.MPagamento = MPagamento;

			UserNameLabel.Text = username;

			NavigationPage.SetHasNavigationBar(this, false);
			customMap();

			getRequestForAppartamenti();

			getrequestForRecensioni();

			getRequestForAppartamentiImmagini();

			getRequestForAttrazioniCordEImmagini();

			getDateDisponibili();

			getRequestForCodiceFiscale();

			getRequestForServiziDisponibili();


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
						Position = new Position(latWithDot, longWithDot)
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
						Position = new Position(latWithDot, longWithDot)
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
			if (isReviewContainer)
			{
				reviewContainer.TranslateTo(0, 0, 300);
				isReviewContainer = false;
			}
			Console.WriteLine("tapped at " + e.Point.Latitude + "," + e.Point.Longitude);
		}

		//evento click sui pin della mappa
		private void map_PinClicked(object sender, PinClickedEventArgs e)
		{
			int gridAutoInc = 6;
			if (tempProvvider != null)
			{
				gridSocial.Children.Remove(tempProvvider);
				gridSocial.Children.Remove(tempNome);
				gridSocial.Children.Remove(boxView);
			}
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
						prezzoAppartamentoSelezionato = Convert.ToDouble(obj.costo);
						prezzoAppartamento.Text = "Costo: " + obj.costo + "€";
						pianoAppartamento.Text = "Piano: " + obj.piano + "°";
						superficieAppartamento.Text = "Superficie: " + obj.superficie + "m²";
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
						if (item.nomeAppartamento.ToLower().Equals(e.Pin.Label.ToLower()))
						{
							nomeProprietario.Text = item.nomeProprietario.ToLower();
							Cognomeproprietario.Text = item.cognomeProprietario.ToLower();
							iban.Text = item.iban.ToUpper();

							tempProvvider = new Label
							{
								Text = item.provider,
								TextColor = Color.FromHex("#ee3861")

							};

							tempNome = new Label
							{
								Text = item.nome,
								TextColor = Color.FromHex("#ee3861")
							};

							boxView = new BoxView
							{
								Color = Color.FromHex("#ee3861"),
							};

							gridSocial.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
							gridSocial.RowDefinitions.Add(new RowDefinition { Height = 1 });

							gridSocial.Children.Add(tempProvvider, 0, gridAutoInc);
							gridSocial.Children.Add(tempNome, 2, gridAutoInc);
							gridSocial.Children.Add(boxView, 0, gridAutoInc + 1);
							Grid.SetColumnSpan(boxView, 3);

							gridAutoInc += 2;
						}

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
				getOrdiniEffettuati();
				if (ordiniEffettuatiJson != null)   //ci sono ordini 
				{
					int gridAutoInc = 0;
					int buttonID = 0;
					containerOrdiniCompletati.Children.Clear();

					foreach (OrdiniEffettuati obj in ordiniEffettuatiJson)
					{
						Label TemplabelAppartamento = new Label
						{
							Text = obj.nomeAppartamento,
							HorizontalOptions = LayoutOptions.CenterAndExpand,
							TextColor = Color.FromHex("#ee3861")
						};

						Label TemplabelDataInizio = new Label
						{
							Text = obj.dataInizio,
							HorizontalOptions = LayoutOptions.CenterAndExpand,
							TextColor = Color.FromHex("#ee3861")
						};

						Label tempConnetti = new Label
						{
							Text = "=>",
							HorizontalOptions = LayoutOptions.CenterAndExpand,
							TextColor = Color.FromHex("#ee3861")
						};

						Label TemplabelDataFine = new Label
						{
							Text = obj.dataFine,
							HorizontalOptions = LayoutOptions.CenterAndExpand,
							TextColor = Color.FromHex("#ee3861")
						};

						Label TemplabelTimestamp = new Label
						{
							Text = obj.timestamp,
							HorizontalOptions = LayoutOptions.CenterAndExpand,
							TextColor = Color.FromHex("#ee3861")
						};

						Button tempButtonRemoveOrdine = new Button
						{
							Text = "Remove",
							FontSize = 10,
							ClassId = obj.idUtente_Appartamenti,
							TextColor = Color.White,
							BackgroundColor = Color.Red,
							Scale = 0.8,
							CornerRadius = 10
						};


						System.TimeSpan diff = DateTime.UtcNow.Subtract(Convert.ToDateTime(obj.timestamp));
						if (diff.TotalDays <= 3)
						{
							tempButtonRemoveOrdine.Clicked += TempButtonRemoveOrdine_Clicked;
						}
						else if (Convert.ToDateTime(obj.dataFine) > DateTime.UtcNow.Date)
						{
							tempButtonRemoveOrdine.Text = "review";
							foreach (AppartamentiPosizione app in appartamentiJson)
							{
								if (app.nomeAppartamento.ToLower().Equals(obj.nomeAppartamento))
								{
									tempButtonRemoveOrdine.ClassId = app.idappartamenti;
									break;
								}
							}

							tempButtonRemoveOrdine.BackgroundColor = Color.Green;
							tempButtonRemoveOrdine.Clicked += RecensisciTempButton_Clicked;
						}
						else
						{
							tempButtonRemoveOrdine.BackgroundColor = Color.Gray;
							tempButtonRemoveOrdine.Clicked += DisabledTempButton_Clicked;
						}

						containerOrdiniCompletati.Children.Add(TemplabelAppartamento, 0, gridAutoInc);        //colonna iniziale riga iniziale

						containerOrdiniCompletati.Children.Add(TemplabelDataInizio, 0, gridAutoInc + 1);       //prima riga
						containerOrdiniCompletati.Children.Add(tempConnetti, 1, gridAutoInc + 1);
						containerOrdiniCompletati.Children.Add(TemplabelDataFine, 2, gridAutoInc + 1);
						containerOrdiniCompletati.Children.Add(TemplabelTimestamp, 0, gridAutoInc + 2);            //terza e ultima riga del blocco
						containerOrdiniCompletati.Children.Add(tempButtonRemoveOrdine, 2, gridAutoInc + 2);            //bottone per rimuovere un appartamento

						Grid.SetColumnSpan(TemplabelAppartamento, 3);

						gridAutoInc += 4;           //salto a creare il blocco successivo con uno spazio vuoto tra essi
						buttonID++;
					}
				}

				userPopUp.TranslateTo(-userPopUp.Width, 0, 300);
				bottomBar.TranslateTo(0, 0, 200);

				bottomBarUp = false;
				isUserPopUpVisible = true;
			}
			else
			{
				userPopUp.TranslateTo(0, 0, 300);
				isUserPopUpVisible = false;

				reviewContainer.TranslateTo(0, 0, 300);
				isReviewContainer = false;
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
				if (MPagamento.Equals("carta prepagata"))
				{
					Navigation.PushAsync(new PayForm(prezzoAppartamentoSelezionato, dateDisponibiliJson, codiceFiscaleUtenteJson[0].cf_utente, datePicker.SelectedItem));
				}
				else
				{
					DisplayAlert("bonifico bancario", "si prega di fare un bonifico di almeno " + prezzoAppartamentoSelezionato * 0.6 + 
						" euro al conto IBAN " + iban.Text, "copia");
					CrossClipboard.Current.SetText(iban.Text);
					DependencyService.Get<IMessage>().ShortAlert("iban copiato correttamente");

				}
			}
			else
			{
				DependencyService.Get<IMessage>().ShortAlert("inserire una data valida");
			}
		}

		private void TempButtonRemoveOrdine_Clicked(object sender, EventArgs e)
		{

			Button btn = (Button)sender;
			Console.WriteLine(btn.ClassId);
			deleteBindingUserToDate(Convert.ToInt32(btn.ClassId));

			DependencyService.Get<IMessage>().ShortAlert("rimozione eseguita con successo");


			userPopUp.TranslateTo(0, 0, 300);           //nascondo la barra laterale finita la query cosi da aggiornare
			isUserPopUpVisible = false;

		}

		private void DisabledTempButton_Clicked(object sender, EventArgs e)
		{
			DependencyService.Get<IMessage>().ShortAlert("i 3 giorni per annullare la prenotazione sono scaduti");
		}

		private int reviewAppartamentoTemp;
		private void RecensisciTempButton_Clicked(object sender, EventArgs e)
		{
			reviewAppartamentoTemp = Convert.ToInt32(((Button)sender).ClassId);           //tiene traccia del bottone assegnato ad un appartamento 
			if (!isReviewContainer)
			{
				reviewContainer.TranslateTo(reviewContainer.Width, 0, 300);
				isReviewContainer = true;
			}
			else
			{
				reviewContainer.TranslateTo(0, 0, 300);
				isReviewContainer = false;
			}

		}

		private void ReviewButton_Clicked(object sender, EventArgs e)
		{
			CreateReview();
		}

		private void SearchByService(object sender, EventArgs e)
		{
			appartamentiList.Clear();
			map.Pins.Clear();

			if (searchServices.Text.Length > 0)
			{
				foreach (AppartamentiPosizione item in appartamentiJson)
				{
					foreach (Servizi servizio in serviziJson)
					{
						if (item.idappartamenti.Equals(servizio.idappartamenti) && servizio.servizio.Equals(searchServices.Text))
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
									Position = new Position(latWithDot, longWithDot)
								});
						}
					}
				}
			}
			else
			{
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
							Position = new Position(latWithDot, longWithDot)
						});
				}
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
						Position = new Position(latWithDot, longWithDot)
					});
			}

			foreach (Pin pin in appartamentiList)
			{
				map.Pins.Add(pin);
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

		private void getOrdiniEffettuati()
		{
			var url = "http://rosafedericoesame.altervista.org/index.php/user/ordiniEffettuati/" + codiceFiscaleUtenteJson[0].cf_utente;
			var myXMLstring = "";
			Task task = new Task(() =>
			{
				myXMLstring = AccessTheWebAsync(url).Result;
			});
			task.Start();
			try
			{
				task.Wait();
			}
			catch (Exception)
			{
				if (ordiniEffettuatiJson != null)
					ordiniEffettuatiJson.Clear();
				return;
			}

			Console.WriteLine(myXMLstring);
			var tr = JsonConvert.DeserializeObject<List<OrdiniEffettuati>>(myXMLstring);
			//After deserializing , we store our data in the List called ObservableCollection
			ordiniEffettuatiJson = new ObservableCollection<OrdiniEffettuati>(tr);
		}

		private void getRequestForServiziDisponibili()
		{
			var url = "http://rosafedericoesame.altervista.org/index.php/user/getServizi";
			var myXMLstring = "";
			Task task = new Task(() =>
			{
				myXMLstring = AccessTheWebAsync(url).Result;
			});
			task.Start();
			try
			{
				task.Wait();
			}
			catch (Exception)
			{
				if (ordiniEffettuatiJson != null)
					ordiniEffettuatiJson.Clear();
				return;
			}

			Console.WriteLine(myXMLstring);
			var tr = JsonConvert.DeserializeObject<List<Servizi>>(myXMLstring);
			//After deserializing , we store our data in the List called ObservableCollection
			serviziJson = new ObservableCollection<Servizi>(tr);
		}

		private async void deleteBindingUserToDate(int idTabella)
		{

			var client = new HttpClient();
			Uri uri = new Uri("http://rosafedericoesame.altervista.org/index.php/user/deleteBindingUserToDate");

			string jsonData = "{" +
							"\"idUtente_Appartamenti\" : \"" + idTabella +
							"\"}";


			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await client.PostAsync(uri, content);

			// this result string should be something like: "{"token":"rgh2ghgdsfds"}"
			var result = await response.Content.ReadAsStringAsync();

			Console.WriteLine(result);
		}

		private async void CreateReview()
		{

			var client = new HttpClient();
			Uri uri = new Uri("http://rosafedericoesame.altervista.org/index.php/user/createReview");

			//gli id vengono calcolati nel web service
			string jsonData = "{" +
							"\"posizione\" : \"" + pickerPosizione.SelectedItem +
							"\", \"qualita_prezzo\" : \"" + pickerQualità_Prezzo.SelectedItem +
							"\", \"servizio\" : \"" + pickerServizio.SelectedItem +
							"\", \"timestamp\" : \"" + DateTime.UtcNow.ToString("yyyy-MM-dd HH-mm-ss") +
							"\", \"fk_appartamenti\" : \"" + reviewAppartamentoTemp +
							"\"}";

			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await client.PostAsync(uri, content);

			// this result string should be something like: "{"token":"rgh2ghgdsfds"}"
			var result = await response.Content.ReadAsStringAsync();

			Console.WriteLine(result);

			getrequestForRecensioni();
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

		private void ClosePayContainer(object sender, EventArgs e)
		{

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
	public string idappartamenti { get; set; }
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
	public string idUtente_Appartamenti { get; set; }
	public string nome { get; set; }
	public string dataInizio { get; set; }
	public string dataFine { get; set; }
}

public class CodiceFiscaleUtente
{
	public string cf_utente { get; set; }
}

public class OrdiniEffettuati
{
	public string idUtente_Appartamenti { get; set; }
	public string username { get; set; }
	public string dataInizio { get; set; }
	public string dataFine { get; set; }
	public string timestamp { get; set; }
	public string nomeAppartamento { get; set; }
}

public class Servizi
{
	public string idappartamenti { get; set; }
	public string nome { get; set; }
	public string servizio { get; set; }
}


#endregion

#region classi per xaml
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

#endregion

public interface IMessage
{
	void LongAlert(string message);
	void ShortAlert(string message);
}