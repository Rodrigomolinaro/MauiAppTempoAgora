using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;
using System.Net;

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
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // Lógica de sucesso (Lê o JSON e preenche o objeto manualmente)
                    string json = await response.Content.ReadAsStringAsync();
                    var rascunho = JObject.Parse(json);

                    // Inicializa a data base para conversão do Unix Timestamp
                    DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    t = new Tempo()
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
                    };
                }
                // EXERCÍCIO PARTE 2: Verifica se o erro é 404 (Not Found)
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new Exception("cidade_nao_encontrada");
                }
                // Captura qualquer outro erro HTTP (ex: 500, 401)
                else
                {
                    // Como não há mais código abaixo do throw, não gerará erro de compilação
                    throw new Exception($"Erro na requisição: {response.StatusCode}");
                }
            }

            return t;
        }
    }
}