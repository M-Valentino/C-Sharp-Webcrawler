# C-Sharp-Webcrawler

This repository contains an easy-to-use web crawler terminal program written in c#. Given an entry URL, the program will visit links and save their URLs and page titles. Optionally you can export the crawled links to a CSV using the `--csv` arg. 

## Prerequisites

Before you begin, ensure you have the .NET SDK installed on your machine.
You can download it from the official [.NET website](https://dotnet.microsoft.com/download).

## Running
After cloning the repo:
go inside the `Web Crawler` directory on the terminal and run:
```bash
dotnet build

dotnet run <entry URL> <number of pages to crawl> [--csv]
```


