using Xamarin.Forms;

namespace RentHouse.com
{
	public partial class App : Application
	{


		public App()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new loginUser());
			//MainPage = new NavigationPage(new MainPage());
			//MainPage = new NavigationPage(new registerUser());     //vera main page
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}

	}
}
