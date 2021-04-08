using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

using HtmlAgilityPack;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace web_scraper_v2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        //Main scraping function here
        public void BeginScrape(object sender, RoutedEventArgs e)
        {
            txt.Text = "";
            prg.Value = 0;
            var html = @"http://html-agility-pack.net/";
            DownloadWebPage(html);
        }

        public void DownloadWebPage(string html)
        {
            Uri _uri = new Uri(html, UriKind.Absolute);
            using (WebClient wc = new WebClient())
            {
                wc.Dispose();
                wc.DownloadStringAsync(_uri);
                wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressHandler);
                wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadedWebHandler);
            }
        }

        public void DownloadProgressHandler(Object sender, DownloadProgressChangedEventArgs e)
        {
            prg.Value = e.ProgressPercentage;
        }

        public void DownloadedWebHandler(Object sender, DownloadStringCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                string textString = (string)e.Result;
                HtmlParse(textString);
            }
        }
        public void HtmlParse(string t)
        {
            var html = new HtmlDocument();
            html.LoadHtml(t);
            txt.Text = html.DocumentNode.OuterHtml;
        }
    }
}
