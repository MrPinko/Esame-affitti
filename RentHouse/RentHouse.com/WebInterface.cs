using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

public class WebInterface
{
    private HttpClient _client;
    string cf;

    public WebInterface(string nome, string cognome, string data, string sesso, string comuneDiNascita)
    {
        _client = new HttpClient();

        var url = "http://webservices.dotnethell.it/codicefiscale.asmx/CalcolaCodiceFiscale?Nome=" + nome +"&Cognome=" + cognome + "&ComuneNascita=" + comuneDiNascita +"&DataNascita=" + data + "&Sesso="+ sesso + "";
        var myXMLstring = "";
        Task task = new Task(() => {
            myXMLstring = AccessTheWebAsync(url).Result;
        });
        task.Start();
        task.Wait();
        string cf = myXMLstring.Substring(myXMLstring.IndexOf('>') + 66, 16);
        Console.WriteLine(cf);
        this.cf = cf;
    }

    async Task<String> AccessTheWebAsync(String url)
    {
        // You need to add a reference to System.Net.Http to declare client.
        HttpClient client = new HttpClient();

        // GetStringAsync returns a Task<string>. That means that when you await the 
        // task you'll get a string (urlContents).
        Task<string> getStringTask = client.GetStringAsync(url);

        // You can do work here that doesn't rely on the string from GetStringAsync.
        //DoIndependentWork();

        // The await operator suspends AccessTheWebAsync. 
        //  - AccessTheWebAsync can't continue until getStringTask is complete. 
        //  - Meanwhile, control returns to the caller of AccessTheWebAsync. 
        //  - Control resumes here when getStringTask is complete.  
        //  - The await operator then retrieves the string result from getStringTask. 
        string urlContents = await getStringTask;

        // The return statement specifies an integer result. 
        // Any methods that are awaiting AccessTheWebAsync retrieve the length value. 
        return urlContents;
    }


    public string getCF()
	{
        return cf;
	}

}
