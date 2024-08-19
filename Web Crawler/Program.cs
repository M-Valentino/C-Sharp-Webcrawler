using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

internal class Web_Crawler
{
    private HashSet<string> _urlsVisited = new HashSet<string>();
    private Queue<string> _topLevelDomainsToVisit = new Queue<string>();
    private HashSet<string> _finalList = new HashSet<string>();

    private Boolean AddTofinalListOrStop(string url, int numLinks)
    {
        if (_finalList.Count >= numLinks)
        {
            return true;
        }
        _finalList.Add(url);
        return false;
    }

    private async Task Crawl(string startUrl, int numLinks)
    {
        _topLevelDomainsToVisit.Enqueue(startUrl);

        while (true)
        {
            string url = _topLevelDomainsToVisit.Dequeue();

            if (_urlsVisited.Contains(url))
            {
                continue;
            }

            _urlsVisited.Add(url);
            AddTofinalListOrStop(startUrl, numLinks);
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
                if (_urlsVisited.Contains(absoluteUrl))
                {
                    continue;
                }
                if (AddTofinalListOrStop(absoluteUrl, numLinks))
                {
                    return;
                }
                Console.WriteLine("Found: " + absoluteUrl);

                _topLevelDomainsToVisit.Enqueue(absoluteUrl);
            }

            var relativeAnchorMatches = Regex.Matches(webPage, @"<a\s+href\s*=\s*[""'](/[^""']+)[""']");

            foreach (Match match in relativeAnchorMatches)
            {
                string relativeUrl = match.Groups[1].Value;
                string absoluteUrl = new Uri(baseUri, relativeUrl).ToString();

                if (_urlsVisited.Contains(absoluteUrl))
                {
                    continue;
                }
                if (AddTofinalListOrStop(absoluteUrl, numLinks))
                {
                    return;
                }
                Console.WriteLine("Found: " + absoluteUrl);

                _topLevelDomainsToVisit.Enqueue(absoluteUrl);
            }
        }
    }

    private async Task<string> Fetch(string url)
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

    public static async Task Main(string[] args)
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
                Console.WriteLine("Saved " + crawler._finalList.Count + " links.");
                foreach (var link in crawler._finalList)
                {
                    Console.WriteLine(link);
                }
                break;
            }
          
            Console.WriteLine("Input is not a valid URL. Please try again.");
        }
    }
}
