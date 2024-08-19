using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

class Web_Crawler
{
    private HashSet<string> urlsVisited = new HashSet<string>();
    private Queue<string> urlsToVisit = new Queue<string>();

    public async Task Crawl(string startUrl, int numLinks)
    {
        urlsToVisit.Enqueue(startUrl);
        
        while (urlsToVisit.Count > 0)
        {
            if (urlsToVisit.Count >= numLinks)
            {
                break;
            }
            
            string url = urlsToVisit.Dequeue();
            
            // Check if URL has already been visited
            if (urlsVisited.Contains(url))
            {
                continue;
            }
            
            urlsVisited.Add(url);
            
            Console.WriteLine($"Visiting: {url}");
            string webPage = await Fetch(url);
            if (webPage == null)
            {
                continue;
            }

            Uri baseUri = new Uri(url);
            var anchorMatches = Regex.Matches(webPage, @"<a\s+href\s*=\s*[""'](https?://[^""']+)[""']");

            foreach (Match match in anchorMatches)
            {
                string absoluteUrl = match.Groups[1].Value;
                if (urlsVisited.Contains(absoluteUrl))
                {
                    continue;
                }

                Console.WriteLine("Found: " + absoluteUrl);
                urlsToVisit.Enqueue(absoluteUrl);
            }

            var relativeAnchorMatches = Regex.Matches(webPage, @"<a\s+href\s*=\s*[""'](/[^""']+)[""']");

            foreach (Match match in relativeAnchorMatches)
            {
                string relativeUrl = match.Groups[1].Value;
                string absoluteUrl = new Uri(baseUri, relativeUrl).ToString();
                
                if (urlsVisited.Contains(absoluteUrl))
                {
                    continue;
                }

                Console.WriteLine("Found: " + absoluteUrl);
                urlsToVisit.Enqueue(absoluteUrl);
            }
        }
    }

    public async Task<string> Fetch(string url)
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
        Web_Crawler crawler = new Web_Crawler();
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
                Console.WriteLine("How many links do you want to crawl?");
                string urlInput = userInput;
                userInput = Console.ReadLine();
                int.TryParse(userInput, out int numLinks);
                await crawler.Crawl(urlInput, numLinks);
                Console.WriteLine(crawler.urlsVisited.Count);
            }
            else
            {
                Console.WriteLine("Input is not a valid URL. Please try again.");
            }
        }
    }
}
