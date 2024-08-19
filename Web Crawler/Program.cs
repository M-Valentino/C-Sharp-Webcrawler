using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

class Web_Crawler
{
    static async Task Crawl(string url)
    {
        Uri uri = new Uri(url);
        string baseUrl = $"{uri.Scheme}://{uri.Host}";
        Console.WriteLine($"Base URL: {baseUrl}");
        string webPage = await Fetch(url);
        var anchorMatches = Regex.Matches(webPage, @"<a href=(.*?)>");
        
        string startTag = "href=\"";
        string endTag = "\">";
        foreach (Match match in anchorMatches)
        {
            int startIndex = match.Value.IndexOf(startTag) + startTag.Length;
            int endIndex = match.Value.IndexOf(endTag, startIndex);

            string relativePath = match.Value.Substring(startIndex, endIndex - startIndex);
            if (relativePath.Length > 0 && relativePath[0] == '#')
            {
                return;
            }
            string fullPath = $"{baseUrl}/{relativePath}";
            Console.WriteLine("Found: " + fullPath);
            await Crawl(fullPath);
        }
    } 
    
    static async Task<string> Fetch(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                string htmlContent = await client.GetStringAsync(url);
                return htmlContent;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }
    }
    
    static async Task Main(string[] args)
    {
        string userInput = "";
        while (userInput?.ToLower() != "q")
        {
            Console.WriteLine("Enter the starting URL to crawl from or press 'q' to quit:");
            userInput = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(userInput))
            {
                Console.WriteLine("Input cannot be empty. Please enter a valid URL or 'q' to quit.");
                continue;
            }

            if (userInput.ToLower() == "q")
            {
                return;
            }

            if (Uri.IsWellFormedUriString(userInput, UriKind.Absolute))
            {
                await Crawl(userInput);
            }
            else
            {
                Console.WriteLine("Input is not a valid URL. Please try again.");
            }
        }
    }

}