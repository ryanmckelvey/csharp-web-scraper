using System;
using System.Windows;
using System.Net;

//Dependencies
using HtmlAgilityPack;

namespace web_scraper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }

        //Main scraping function here
        public void BeginScrape(object sender, RoutedEventArgs e)
        {
            txt.Document.Blocks.Clear();
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
            txt.AppendText(html.DocumentNode.OuterHtml);
        }
    }
}
