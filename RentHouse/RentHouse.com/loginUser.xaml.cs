using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentHouse.com
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class loginUser : ContentPage
	{
		ObservableCollection<UsernameLogin> usernameLoginJson;
		private HttpClient _client;
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
				checkLoginUser("http://rosafedericoesame.altervista.org/index.php/user/loginUser/" + LoginEmailEntry.Text + "/" + MD5Hash(LoginpasswordEntry.Text));           //per altervista


				if (!responseString.Equals("NotFound"))
				{
					getRequestForUsername();
					Navigation.PushAsync(new MainPage(usernameLoginJson[0].username));
				}
				else
				{
					DependencyService.Get<IMessage>().ShortAlert("credenziali sbagliate");
				}
			}
			else
			{
				DependencyService.Get<IMessage>().ShortAlert("inserire le credenziali");
			}

		}

		public bool isAllFilled()
		{
			if (LoginEmailEntry.Text != null && LoginpasswordEntry != null)
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

			//cose in più (utili nel futuro)
			//string content = await response.Content.ReadAsStringAsync();
			//Items = JsonSerializer.Deserialize<List<TodoItem>>(content, serializerOptions);

		}

		private void getRequestForUsername()
		{
			var url = "http://rosafedericoesame.altervista.org/index.php/user/loginUser/" + LoginEmailEntry.Text.ToLower();
			var myXMLstring = "";
			Task task = new Task(() =>
			{
				myXMLstring = AccessTheWebAsync(url).Result;
			});
			task.Start();
			task.Wait();
			Console.WriteLine(myXMLstring);

			var tr = JsonConvert.DeserializeObject<List<UsernameLogin>>(myXMLstring);
			//After deserializing , we store our data in the List called ObservableCollection
			usernameLoginJson = new ObservableCollection<UsernameLogin>(tr);
		}

		async Task<String> AccessTheWebAsync(String url)
		{
			HttpClient client = new HttpClient();

			Task<string> getStringTask = client.GetStringAsync(url);

			string urlContents = await getStringTask;

			return urlContents;
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

public class UsernameLogin
{
	public string username { get; set; }
}

