using System;
using System.Net.Http;
using System.Threading.Tasks;

class Web_Crawler
{
    static async Task Crawl(string url)
    {
        Console.WriteLine(await Fetch(url));
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