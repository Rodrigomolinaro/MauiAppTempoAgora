using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using System.ComponentModel.Design;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                var entrada = this.FindByName<Entry>("txt_cidade");
                var lbl = this.FindByName<Label>("lbl_res");

                string cidade = entrada?.Text ?? string.Empty;

                if (!string.IsNullOrEmpty(cidade))
                {
                    Tempo? t = await DataService.GetPrevisao(cidade);

                    if (t != null)
                    {
                        string dados_previsao = "";

                        dados_previsao = $"Latitude: {t.lat} \n" +
                                        $"Longitude: {t.lon} \n" +
                                        $"Nascer do Sol: {t.sunrise} \n" +
                                        $"Pôr do Sol: {t.sunset} \n" +
                                        $"Temp Máx: {t.temp_max} \n" +
                                        $"Temp Min: {t.temp_min} \n";

                        if (lbl != null)
                            lbl.Text = dados_previsao;
                    }
                    else
                    {
                        if (lbl != null)
                            lbl.Text = "Sem dados de Previsão";
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

    }
}
