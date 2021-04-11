using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using System.Threading.Tasks;
using System.Net;
using System.ComponentModel;

using HtmlAgilityPack;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace web_scraper_v2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        BackgroundWorker backgroundWorker1 = new BackgroundWorker();
        public MainPage()
        {
            this.InitializeComponent();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(DownloadWebPageAsync);
        }
        //Main scraping function here
        public void BeginScrape(object sender, RoutedEventArgs e)
        {
            txt.Text = "";
            prg.Value = 0;
            cancelBtn.IsEnabled = true;
            if (url.Text != "")
            {
                var html = (url.Text);
                backgroundWorker1.RunWorkerAsync(argument: html);
            }
        }

        public async void DownloadWebPageAsync(object sender, DoWorkEventArgs e)
        {
            
            try
            {
                Uri _uri = new Uri(e.Argument.ToString(), UriKind.Absolute);
                using (WebClient wc = new WebClient())
                {
                    wc.Dispose();
                    wc.DownloadStringAsync(_uri);
                    wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressHandlerAsync);
                    if (backgroundWorker1.CancellationPending != true)
                    {
                        wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadedWebHandler);
                    }
                }
            }
            catch (Exception ex)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    txt.Text = ex.Message;
                });
            }

        }

        public async void CancelScrape(object sender, RoutedEventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    cancelBtn.IsEnabled = false;
                });
                backgroundWorker1.CancelAsync();
            }
        }

        public async void DownloadProgressHandlerAsync(Object sender, DownloadProgressChangedEventArgs e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
             {
                 prg.Value = e.ProgressPercentage;
             });

        }

        public void DownloadedWebHandler(Object sender, DownloadStringCompletedEventArgs e)
        {
            if (!backgroundWorker1.CancellationPending)
            {
                string textString = (string)e.Result;
                HtmlParse(textString);
            }
            else
            {
                HtmlParse("Cancelled");
            }
        }
        public async void HtmlParse(string t)
        {
            var html = new HtmlDocument();
            html.LoadHtml(t);
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                txt.Text = html.DocumentNode.OuterHtml;
            });

        }
    }
}
