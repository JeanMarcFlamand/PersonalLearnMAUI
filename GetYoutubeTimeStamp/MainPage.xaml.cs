using AngleSharp.Io;
using HtmlAgilityPack;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GetYoutubeTimeStamp
{
    public partial class MainPage : ContentPage
    {
        

        public MainPage()
        {
            InitializeComponent();
        }
        
        private async void OnGetValueClicked(object sender, EventArgs e)
        {
            // Create an HTTP client to make the request
            using (var client = new HttpClient())
            {
                // Get the HTML content from the web view
                var content = await webView.EvaluateJavaScriptAsync("document.documentElement.outerHTML");


                // Unescape that damn Unicode Java bull.
                // ref  https://github.com/xamarin/Xamarin.Forms/issues/11648#issuecomment-673196372
                content = Regex.Replace(content, @"\\[Uu]([0-9A-Fa-f]{4})", m => char.ToString((char)ushort.Parse(m.Groups[1].Value, NumberStyles.AllowHexSpecifier)));
                content = Regex.Unescape(content);

                if (content.Equals("\"null\""))
                    content = null;

                else if (content.StartsWith("\"") && content.EndsWith("html>"))
                    content = content.Substring(1, content.Length - 2);

                var contentlenght = content.Length;

                //First method use HtmlAgilityPack
                //Load the HTML content into an HtmlDocument object
                var doc = new HtmlDocument();
                doc.LoadHtml(content);


                // Find the element with the class "ytp-time-current"
                var element = doc.DocumentNode.SelectSingleNode("//span[@class='ytp-time-current']");

                // Get the value of the element
                string value = element?.InnerText.Trim();

                // Display the value in the label on the page
                HtmlAgilityPackValue.Text = value ?? "Element not found";


                
                //Alternative method to get the value
                string startString = "<span class=\\\"ytp-time-current\\\">"; //ok
                string endString = "</span>"; //ok

                GetStringBetweenvalue.Text = GetStringBetween(content, startString, endString);
            }
        }


        public static string GetStringBetween(string strSource, string strStart, string strEnd)
        {
            int startIndex = strSource.IndexOf(strStart);
            if (startIndex == -1)
            {
                return "The first part of the element is not found";
            }
            int endIndex = strSource.IndexOf(strEnd, startIndex + strStart.Length);
            if (endIndex == -1)
            {
                return "The second part of the element is not found"; ;
            }

            return strSource.Substring(startIndex + strStart.Length, endIndex - startIndex - strStart.Length);
        }

       

        
    }
}