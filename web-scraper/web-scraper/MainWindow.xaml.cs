using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            var html = @"http://html-agility-pack.net/";
            DownloadWebPage(html);
        }

        public void DownloadWebPage(string html)
        {
            using (WebClient wc = new WebClient())
            {
                Uri _uri = new Uri(html, UriKind.Absolute);
                wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadedWebHandler);
                wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressHandler);
                wc.DownloadStringAsync(_uri);
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
        public void HtmlParse(string t) {
            var html = new HtmlDocument();
            html.LoadHtml(t);
            txt.AppendText(html.DocumentNode.OuterHtml);    
        }
    }
}
