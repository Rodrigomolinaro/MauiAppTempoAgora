using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "fe7010c3a91d8d8ec8538862e639d48b";

            string url =
                $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&units=metric&appid={chave}";

            using (HttpClient client = new HttpClient())
            {

                HttpResponseMessage resp = await client.GetAsync(url);

                if(resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();
                    var rascunho = JObject.Parse(json);

                    DateTime time = new();
                    DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    t = new()
                    {
                        lon = (double)rascunho["coord"]["lon"],
                        lat = (double)rascunho["coord"]["lat"],
                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp_max = (double)rascunho["main"]["temp_max"],
                        visibility = (int)rascunho["visibility"],
                        speed = (double)rascunho["wind"]["speed"],
                        main = (string)rascunho["weather"][0]["main"],
                        description = (string)rascunho["weather"][0]["description"],
                        sunrise = sunrise.ToString(),
                        sunset = sunset.ToString(),
                    }; //Fecha obj do tempo
                } //Fecha if se o status do servidor foi de sucesso
            } // fecha laço using 



            return t;
        }
    }
}
