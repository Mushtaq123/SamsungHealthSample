using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HealthApi
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Label label = new Label
            {
                Text = $"{DateTime.Now:dd-MM-yyyy HH:mm:ss}:Page Initiated",
                FontSize = 16,
                Padding = new Thickness(10),
                TextColor = Color.Black
            };
            myStack.Children.Add(label);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            ISamsungHealthApiService samsungService = DependencyService.Get<ISamsungHealthApiService>();
            samsungService.OnSamsungHealthApiValueChanged += (s, ev) =>
            {
                Label label = new Label
                {
                    Text = $"{DateTime.Now:dd-MM-yyyy HH:mm:ss}:{ev.Count} steps",
                    FontSize = 16,
                    Padding = new Thickness(10),
                    TextColor = Color.Black
                };
                myStack.Children.Add(label);
            };
            samsungService.Connect();
        }

    }
}