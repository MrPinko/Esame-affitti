using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FFImageLoading.Forms;
using Google.Type;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace RentHouse.com
{
	public partial class MainPage : ContentPage
	{
		object data = null;
		private List<City> cityList;
		private List<Pin> pinList = new List<Pin>();
		private bool bottomBarUp = false, isExpanded = false;
		Location location;
		String startCity = "tokyo";
		string selectedCity = "";


		public MainPage()
		{
			InitializeComponent();
			LocalJson();
			customMap();
			location = new Location();

			//carico i pin
			foreach (City item in cityList)
			{
				pinList.Add(
					new Pin
					{
						Type = PinType.Place,
						Label = item.city,
						Address = item.address,
						Position = new Position(item.Lat, item.Long)
					});

			}


			//aggiugno i pin alla mappa 
			foreach (Pin pin in pinList)
			{
				map.Pins.Add(pin);
			}

			//muovo la telecamera sulla città cercata
			var latitudine = location.GetLocation().Result.Latitude;
			var longitudine = location.GetLocation().Result.Longitude;

			//map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(latitudine,longitudine), Distance.FromMeters(5000)));   //si posiziona sulla posizione corrente

			map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(cityList[findID(startCity)].Lat, cityList[findID(startCity)].Long), Distance.FromMeters(5000)));

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

		//evento click sulla mappa
		private void map_MapClicked(object sender, MapClickedEventArgs e)
		{
			Console.WriteLine("tapped at " + e.Point.Latitude + "," + e.Point.Longitude);
		}

		//evento click sui pin della mappa
		private void map_PinClicked(object sender, PinClickedEventArgs e)
		{
			int cityID = findID(e.Pin.Label.ToString());
			if (bottomBarUp)
			{
				bottomBar.TranslateTo(0, 0, 300);
				bottomBarUp = false;
			}
			else
			{
				bottomBar.TranslateTo(0, -bottomBar.Height / 3, 350);
				houseName.Text = e.Pin.Label.ToUpper();

				houseImage.Source = new Uri(cityList[cityID].image);
				houseImage.HeightRequest = bottomBar.Height / 3;
				houseImage.WidthRequest = bottomBar.Width;

				posizioneStar.Text = cityList[cityID].posizioneStar.ToString();
				qpStar.Text = cityList[cityID].qpStar.ToString();
				servizioStar.Text = cityList[cityID].servizioStar.ToString();


				bottomBarUp = true;
			}
		}
		
		//json delle case prese dal database
		private void LocalJson()
		{
			var assembly = typeof(MainPage).GetTypeInfo().Assembly;
			Stream stream = assembly.GetManifestResourceStream("RentHouse.com.House.json");

			using (var reader = new System.IO.StreamReader(stream))
			{

				var json = reader.ReadToEnd();
				cityList = JsonConvert.DeserializeObject<List<City>>(json);
				foreach (City user in cityList)
				{
					Console.WriteLine(user.city);
				}
			}
		}

		private int findID(String s)
		{
			for(int i = 0; i < cityList.Count; i++)
			{
				if (cityList[i].city.Equals(s))
					return i;
			}
			
			return 0;
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

	}

}

public class City
{
	public string city { get; set; }
	public string image { get; set; }
	public string address { get; set; }
	public double Lat { get; set; }
	public double Long { get; set; }
	public int posizioneStar { get; set; }
	public int qpStar { get; set; }
	public int servizioStar { get; set; }


}
