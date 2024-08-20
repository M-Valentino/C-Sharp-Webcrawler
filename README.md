# C-Sharp-Webcrawler

This repository contains an easy-to-use web crawler terminal program written in c#. Given an entry URL, the program will visit links and save their URLs, page titles, and meta descriptions. Optionally you can export the crawled links to a CSV. 

## Prerequisites

Before you begin, ensure you have the .NET SDK installed on your machine.
You can download it from the official [.NET website](https://dotnet.microsoft.com/download).

## Running
After cloning the repo:
go inside the `Web Crawler` directory on the terminal and run:
```bash
dotnet build

dotnet run <entry URL> <number of pages to crawl> [--csv] [--cd:<number in milliseconds>]
```
### Optional Args
- `--csv` will export to CSV
- `--cd:<number in milliseconds>` controls the crawl delay in milliseconds. For example, `--cd:2000` specifies a crawl delay of two seconds. If this arg isn't used, the crawl delay will be 1000ms.

## Results
If you don't run the crawler with the --csv arg, it will just print the crawled webpages' URL and title to the terminal. If you export to a CSV, you won't get a terminal output.

If a title or description is not found on a webpage, the value will be displayed as `none`.

Below is an example CSV of crawled links with `https://google.com` as the entry URL:

|url                                                    |title                                                           |description                                                                                                                                    |
|-------------------------------------------------------|----------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------|
|https://google.com                                     |Google                                                          |none                                                                                                                                           |
|https://www.google.com/intl/en/about/products?tab=wh   |Browse All of Google&#39;s Products &amp; Services - Google     |Browse a list of Google products designed to help you work and play, stay organized, get answers, keep in touch, grow your business, and more. |
|https://google.com/advanced_search?hl=en&amp;authuser=0|Google Advanced Search                                          |none                                                                                                                                           |
|https://google.com/intl/en/ads/                        |Google Ads - Get Customers and Sell More with Online Advertising|Discover how online advertising with Google Ads can help grow your business. Get customers and sell more with our digital advertising platform.|
|https://google.com/services/                           |none                                                            |none                                                                                                                                           |

