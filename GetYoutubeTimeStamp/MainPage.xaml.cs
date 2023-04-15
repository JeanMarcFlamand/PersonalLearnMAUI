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
                content = Regex.Replace(content, @"\\[Uu]([0-9A-Fa-f]{4})", m => char.ToString((char)ushort.Parse(m.Groups[1].Value, NumberStyles.AllowHexSpecifier)));
                content = Regex.Unescape(content);
                var contentlenght = content.Length;

                //Replace the content by hardcoded content for test purpose
                content = "</path></svg></span><span><span class=\"ytp-time-current\">0:00</span><span class=\"ytp-time-separator\"> / </span><span class=\"ytp-time-duration\">0:23</span></span><span class=\"ytp-clip-watch-full-video-button-separator\">•</span><span class=\"ytp-clip-watch-full-video-button\">Watch full video</span><button class=\"ytp-live-badge ytp-button\" disabled=\"true\">Live</button></div><div class=\"ytp-chapter-container\" style=\"display: none;\"><button class=\"ytp-chapter-title ytp-button ";

                //First method use HtmlAgilityPack
                //Load the HTML content into an HtmlDocument object
                var doc = new HtmlDocument();
                doc.LoadHtml(content);


                // Find the element with the class "ytp-time-current"
                var element = doc.DocumentNode.SelectSingleNode("//span[@class='ytp-time-current']");

                // Get the value of the element
                string value = element?.InnerText.Trim();

                // Display the value in the label on the page
                valueLabel.Text = value ?? "Element not found";


                
                //Alternative method to get the value
                string startString = "<span class=\"ytp-time-current\">"; //ok
                string endString = "</span>"; //ok

                valueLabel2.Text = GetStringBetween(content, startString, endString);
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