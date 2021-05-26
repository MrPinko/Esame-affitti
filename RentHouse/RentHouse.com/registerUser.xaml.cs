using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentHouse.com
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class registerUser : ContentPage
	{
		private List<Comuni> comuniList;
		private int menuInt = 0;

		public registerUser()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);

			LocalJson();

			secondContainer.TranslateTo(1000, 0, 0);
			reviewContainer.TranslateTo(2000, 0, 0);

		}


		//json delle case prese dal database
		private void LocalJson()
		{
			var assembly = typeof(MainPage).GetTypeInfo().Assembly;
			Stream stream = assembly.GetManifestResourceStream("RentHouse.com.comuni.json");

			using (var reader = new System.IO.StreamReader(stream))
			{

				var json = reader.ReadToEnd();
				comuniList = JsonConvert.DeserializeObject<List<Comuni>>(json);
				/*foreach (Comuni obj in comuniList)
				{
					Console.WriteLine(obj.provincia.nome);
				}*/
			}
		}

		private void confirmimagePressed(object sender, EventArgs e)
		{
			moveContainer("forward");
		}

		private void backImagePressed(object sender, EventArgs e)
		{
			moveContainer("backwards");
		}

		string sigla;
		private void comuneDiNascita_Unfocused(object sender, FocusEventArgs e)
		{
			if(comuneDiNascita.Text != null)
			foreach (Comuni obj in comuniList)
			{
				if (comuneDiNascita.Text.ToUpper().Equals(obj.nome.ToUpper()))
				{
					capEntry.Text = obj.cap[0];
					sigla = obj.sigla;
					return;
				}
			}
		}

		public void moveContainer(string dir)
		{
			if (dir.Equals("forward"))
			{
				switch (menuInt)
				{
					case 0:
						firstContainer.TranslateTo(-1000, 0, 500);
						secondContainer.TranslateTo(0, 0, 500);
						reviewContainer.TranslateTo(1000, 0, 500);
						menuInt++;
						break;

					case 1:          //review container
						if (checkAllEntry())
						{
							insertIntoReviewContainer();
							firstContainer.TranslateTo(-2000, 0, 500);
							secondContainer.TranslateTo(-1000, 0, 500);
							reviewContainer.TranslateTo(0, 0, 500);
							menuInt++;
						}
						break;
					case 2:
						postRequest();
							
						Navigation.PushAsync(new MainPage(review_Username.Text));
						break;
				}
			}
			else if (dir.Equals("backwards"))
			{
				switch (menuInt)
				{
					case 2:
						firstContainer.TranslateTo(-1000, 0, 500);
						secondContainer.TranslateTo(0, 0, 500);
						reviewContainer.TranslateTo(1000, 0, 500);
						menuInt--;
						break;

					case 1:
						firstContainer.TranslateTo(0, 0, 500);
						secondContainer.TranslateTo(1000, 0, 500);
						reviewContainer.TranslateTo(2000, 0, 500);
						menuInt--;
						break;
				}
			}

			if (menuInt != 0)
			{
				backButton.IsVisible = true;
				backButton.IsEnabled = true;
			}
			else
			{
				backButton.IsVisible = false;
				backButton.IsEnabled = false;
			}
		}

		private async void postRequest()
		{
			var client = new HttpClient();
			Uri uri = new Uri("http://rosafedericoesame.altervista.org/index.php/user/registerUser");

			string jsonData = "{" +
							"\"username\" : \"" + usernameEntry.Text +
							"\", \"pw\" : \"" + MD5Hash(pwEntry.Text) +
							"\", \"email\": \"" + review_email.Text +
							"\", \"cell\" : \"" + review_cell.Text +
							"\", \"citta\" : \"" + review_citta.Text +
							"\", \"via\" : \"" + review_via.Text +
							"\", \"numero\": \"" + review_numero.Text +
							"\", \"cap\" : \"" + review_cap.Text +
							"\", \"dataN\": \"" + data.Date.ToString("yyyy-MM-dd") +
							"\", \"sesso\": \"" + review_sesso.Text +
							"\", \"cf_utente\" : \"" + codiceFiscale.Text +
							"\", \"nome\" : \"" +review_nome.Text +
							"\", \"cognome\": \"" + review_cognome.Text +
							"\", \"m_pagamento\": \" " + review_MPagamento.Text +
							"\"}";


			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await client.PostAsync(uri, content);

			// this result string should be something like: "{"token":"rgh2ghgdsfds"}"
			var result = await response.Content.ReadAsStringAsync();

			Console.WriteLine(result);
		}

		public bool checkAllEntry()
		{
			if (usernameEntry.Text != null &&
				nomeEntry.Text != null &&
				cognomeEntry.Text.Length != 0 &&
				sessoEntry.SelectedItem != null &&
				cellEntry.Text.Length != 0 &&
				emailEntry.Text.Length != 0 &&
				pwEntry.Text.Length != 0 &&
				MPagamentoEntry.SelectedItem != null &&
				comuneDiNascita.Text.Length != 0 &&
				cittaEntry.Text.Length != 0 &&
				viaEntry.Text.Length != 0 &&
				numeroEntry.Text.Length != 0 &&
				capEntry.Text.Length != 0)
			{
				return true;
			}
			else
			{
				DependencyService.Get<IMessage>().ShortAlert("inserire tutti i dati prima di continuare");
				return false;
			}
		}

		public void insertIntoReviewContainer()
		{
			review_Username.Text = usernameEntry.Text.ToLower();
			review_nome.Text = nomeEntry.Text.ToLower();
			review_cognome.Text = cognomeEntry.Text.ToLower();
			review_sesso.Text = sessoEntry.SelectedItem.ToString().ToLower();
			review_MPagamento.Text = MPagamentoEntry.SelectedItem.ToString().ToLower();
			review_data.Text = data.Date.ToString("dd/MM/yyyy");
			review_cell.Text = cellEntry.Text.ToLower();
			review_email.Text = emailEntry.Text.ToLower();
			review_comuneDiNascita.Text = comuneDiNascita.Text.ToLower();
			review_citta.Text = cittaEntry.Text.ToLower();
			review_via.Text = viaEntry.Text.ToLower();
			review_numero.Text = numeroEntry.Text.ToLower();
			review_cap.Text = capEntry.Text.ToLower();
			codiceFiscale.Text = new WebInterface(nomeEntry.Text.ToString(), cognomeEntry.Text.ToString(), data.Date.ToString("dd/MM/yyyy"), sessoEntry.SelectedItem.ToString(), comuneDiNascita.Text.ToString()).getCF();

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


		private void gotoLogin(object sender, EventArgs e)
		{
			Navigation.PushAsync(new loginUser());
		}

		protected override bool OnBackButtonPressed()
		{
			System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
			return true;
		}

		public class Zona
		{
			public string codice { get; set; }
			public string nome { get; set; }
		}

		public class Regione
		{
			public string codice { get; set; }
			public string nome { get; set; }
		}

		public class Provincia
		{
			public string codice { get; set; }
			public string nome { get; set; }
		}

		public class Comuni
		{
			public string nome { get; set; }
			public string codice { get; set; }
			public Zona zona { get; set; }
			public Regione regione { get; set; }
			public Provincia provincia { get; set; }
			public string sigla { get; set; }
			public string codiceCatastale { get; set; }
			public List<string> cap { get; set; }
			public int popolazione { get; set; }
		}


	}
}
