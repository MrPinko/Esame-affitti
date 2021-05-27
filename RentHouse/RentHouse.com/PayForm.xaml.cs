using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FormsControls.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentHouse.com
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PayForm : AnimationPage
	{

		ObservableCollection<DateDisponibili> dateDisponibiliJson;
		string codiceFiscale;
		private double prezzoAppartamentoSelezionato;
		private object datePickerSelectedItem;

		public PayForm(double prezzoAppartamentoSelezionato, ObservableCollection<DateDisponibili> dateDisponibilijson, string codiceFiscale, object datePickerSelectedItem)
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);

			dataScadenza.MinimumDate = DateTime.UtcNow.Date;
			paySlider.Maximum = prezzoAppartamentoSelezionato;
			paySlider.Minimum = prezzoAppartamentoSelezionato * 0.6;
			paySliderLabel.Text = (prezzoAppartamentoSelezionato * 0.6).ToString() + "€";

			this.prezzoAppartamentoSelezionato = prezzoAppartamentoSelezionato;
			this.dateDisponibiliJson = dateDisponibilijson;
			this.codiceFiscale = codiceFiscale;
			this.datePickerSelectedItem = datePickerSelectedItem;
		}

		private void paySlider_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			paySliderLabel.Text = paySlider.Value.ToString() + "€";
		}

		private void closePaymentMethod(object sender, EventArgs e)
		{
			Navigation.PopAsync(true);
		}

		private void BuyButton(object sender, EventArgs e)
		{
			if (cartaNum != null && cvc.Text != null) {
				if (cartaNum.Text.Length == 12)
				{
					bindingUserToDate();
					DependencyService.Get<IMessage>().ShortAlert("prenotazione eseguita con successo");
					Navigation.PopAsync(true);
				}
				else
				{
					DependencyService.Get<IMessage>().ShortAlert("la carta è un numero di 12 cifre");
				}
			}
			else
			{
				DependencyService.Get<IMessage>().ShortAlert("inserire tutti i dati per continuare");
			}
		}

		private async void bindingUserToDate()
		{
			//02-07-2021 fino a 09-07-2021
			int id = 0;
			foreach (DateDisponibili DD in dateDisponibiliJson)              //date picker
			{
				if ((Convert.ToDateTime(DD.dataInizio).ToString("dd-MM-yyyy") + " fino a " + Convert.ToDateTime(DD.dataFine).ToString("dd-MM-yyyy")).Equals(datePickerSelectedItem))      //creo la stringa come è nel picker
				{
					id = Convert.ToInt32(DD.idUtente_Appartamenti);
					break;
				}

			}

			var client = new HttpClient();
			Uri uri = new Uri("http://rosafedericoesame.altervista.org/index.php/user/addUserToAppartamenti");

			string jsonData = "{" +
							"\"fk_utente\" : \"" + codiceFiscale +
							"\", \"idUtente_Appartamenti\" : \"" + id +
							"\", \"timestamp\" : \"" + DateTime.UtcNow.ToString("yyyy-MM-dd HH-mm-ss") +
							"\"}";


			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await client.PostAsync(uri, content);

			// this result string should be something like: "{"token":"rgh2ghgdsfds"}"
			var result = await response.Content.ReadAsStringAsync();

			Console.WriteLine(result);
		}
	}
}