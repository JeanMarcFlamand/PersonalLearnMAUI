using HtmlAgilityPack;

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
                
                // Load the HTML content into an HtmlDocument object
                var doc = new HtmlDocument();
                doc.LoadHtml(content);
                

                // Find the element with the class "ytp-time-current"
                var element = doc.DocumentNode.SelectSingleNode("//span[@class='ytp-time-current']");

                // Get the value of the element
                string value = element?.InnerText.Trim();

                // Display the value in the label on the page
                valueLabel.Text = value ?? "Element not found";
            }
        }
    }
}