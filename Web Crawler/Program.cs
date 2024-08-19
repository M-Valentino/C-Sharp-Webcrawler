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
        await Crawl("https://sacred.neocities.org");
    }
}