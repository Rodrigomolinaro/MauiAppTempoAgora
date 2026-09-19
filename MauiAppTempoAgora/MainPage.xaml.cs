using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // Alterado de 'async Task' para 'async void', que é o padrão correto para eventos no MAUI
        private async void Button_Clicked(object sender, EventArgs e)
        {
            // EXERCÍCIO PARTE 2: Verifica a conexão com a internet antes de fazer a requisição
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Sem Conexão", "Verifique a sua ligação à internet e tente novamente.", "OK");
                return; // Encerra o método aqui para não tentar consultar a API sem internet
            }

            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    // Faz a chamada ao Web Service[cite: 1]
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        // EXERCÍCIO PARTE 1: Expandindo os dados exibidos
                        string dados_previsao = $"Clima: {t.description} \n" +
                                                $"Velocidade do Vento: {t.speed} \n" +
                                                $"Visibilidade: {t.visibility} \n" +
                                                $"Latitude: {t.lat} \n" +
                                                $"Longitude: {t.lon} \n" +
                                                $"Nascer do Sol: {t.sunrise} \n" +
                                                $"Pôr do Sol: {t.sunset} \n" +
                                                $"Temp Máx: {t.temp_max} \n" +
                                                $"Temp Min: {t.temp_min} \n";

                        lbl_res.Text = dados_previsao;
                    }
                    else
                    {
                        lbl_res.Text = "Sem dados de Previsão";
                    }
                }
                else
                {
                    // Alerta amigável se o utilizador clicar no botão com a caixa de texto vazia
                    await DisplayAlert("Aviso", "Por favor, digite o nome de uma cidade.", "OK");
                }
            }
            catch (Exception ex)
            {
                // EXERCÍCIO PARTE 2: Interceta a exceção específica do DataService
                if (ex.Message == "cidade_nao_encontrada")
                {
                    await DisplayAlert("Não Encontrada", "A cidade digitada não existe. Verifique o nome e tente novamente.", "OK");
                }
                else
                {
                    // Exibe qualquer outro erro inesperado
                    await DisplayAlert("Ops", ex.Message, "OK");
                }
            }
        }
    }
}