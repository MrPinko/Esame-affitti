using System;
using System.Net.Http;
using System.Security.Cryptography;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentHouse.com
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class loginUser : ContentPage
	{
		private HttpClient _client;
		private static bool isLoginSuccesseful = false;
		private static string responseString = "";

		public loginUser()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		private void loginConfirm_Tapped(object sender, EventArgs e)
		{
			//checkLoginUser("http://192.168.1.106/Api_Server/index.php/user/loginUser/" + LoginnomeEntry.Text + "/" + MD5Hash(LoginpasswordEntry.Text));      //per localhost su fisso

			if (isAllFilled())
			{
				checkLoginUser("http://rosafedericoesame.altervista.org/index.php/user/loginUser/" + LoginnomeEntry.Text + "/" + MD5Hash(LoginpasswordEntry.Text));           //per altervista


				if (!responseString.Equals("NotFound"))
				{
					Navigation.PushAsync(new MainPage(LoginnomeEntry.Text));
				}
				else
				{
					DisplayAlert("errore", "credenziali sbagliate", "ok");
				}
			}

		}

		public bool isAllFilled()
		{
			if (LoginnomeEntry.Text != null && LoginpasswordEntry != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		public void WebInterface(HttpClient httpClient)
		{
			_client = httpClient;
		}

		static void checkLoginUser(string uri)
		{
			var client = new HttpClient();
			var response = client.GetAsync(uri);

			//l'utente è giusto
			responseString = response.Result.StatusCode.ToString();

			isLoginSuccesseful = true;
			//cose in più (utili nel futuro)
			//string content = await response.Content.ReadAsStringAsync();
			//Items = JsonSerializer.Deserialize<List<TodoItem>>(content, serializerOptions);

		}

		public static string MD5Hash(string _password)
		{
			SHA512 sha512 = new System.Security.Cryptography.SHA512Managed();

			byte[] sha512Bytes = System.Text.Encoding.Default.GetBytes(_password);

			byte[] cryString = sha512.ComputeHash(sha512Bytes);

			string hashedPwd = string.Empty;

			for (int i = 0; i < cryString.Length; i++)
			{
				hashedPwd += cryString[i].ToString("X2");
			}

			return hashedPwd;
		}

		private void gotoRegistrazione(object sender, EventArgs e)
		{
			Navigation.PushAsync(new registerUser());
		}

		protected override bool OnBackButtonPressed()
		{
			System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
			return true;
		}
	}
}