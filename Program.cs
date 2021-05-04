using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ImageCrawler
{
    class Program
    {
        private const string snipesLink = "https://www.snipes.es";
        private const string snipesLoader = "https://www.snipes.es/dw/image/v2/BDCB_PRD/on/demandware.static/-/Sites-snse-master-eu/default/d";
        private const string folderTarget = "/Users/PabloMiletich/Downloads/AllFootProcessed";

        static void Main(string[] args)
        {

            var folderPath = "/Users/PabloMiletich/Downloads/AllFoot";

            /*****

HtmlWeb hw = new HtmlWeb();
HtmlDocument doc = GetPage("https://www.snipes.es/c/shoes?sz=1000");
var linkTags = doc.DocumentNode.Descendants("link");
var linkedPages = doc.DocumentNode.Descendants("a")
                                  .Select(a => a.GetAttributeValue("href", null))
                                  .Where(u => !String.IsNullOrEmpty(u) && u.StartsWith("/p"))
                                  .Select(s => $"{snipesLink}{s}");

Console.WriteLine(string.Join(Environment.NewLine, linkedPages));



foreach (var link in linkedPages)
{
    HtmlDocument document = GetPage($"{snipesLink}{link}");

    var imageList = new List<string>();


    var imgs = document.DocumentNode.Descendants("img")
                                .Select(i => i.Attributes["src"]);
    // For every tag in the HTML containing the node img.
    foreach (var node in imgs)
    {
            if (node != null && node.Value.StartsWith(snipesLoader) && node.Value.Contains("_P"))
        {
            var shoeCode = node.Value.Replace(snipesLoader, "");
            imageList.Add(shoeCode.Substring(0, shoeCode.IndexOf("?")));
        }
    }

    foreach (var image in imageList)
    {
        using (WebClient webClient = new WebClient())
        {
            var imageUrl = $"{snipesLoader}{image}?sw=200&sh=200&sm=fit&sfrm=png";
            byte[] data = webClient.DownloadData(imageUrl);

            using (MemoryStream mem = new MemoryStream(data))
            {
                using (var yourImage = Image.FromStream(mem))
                {
                    // If you want it as Png
                    yourImage.Save($"{folderTarget}/{image.Replace("/", "-")}", ImageFormat.Jpeg);
                }
            }
        }

        Task.Delay(1000).Wait();
    }

    Task.Delay(10000).Wait();
}
****/

foreach (var folder in Directory.GetDirectories(folderPath))
{
    string index = null;

    foreach(var file in Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories))
    {
        if (file.Contains("_P3.jpg"))
        {
            index = file.Replace("_P3.jpg", "");
        }
    }

    if (index != null)
    {
        foreach (var file in Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories))
        {
            if (file.Contains(index))
            {
                var name = Path.GetFileName(file);
                var targetPath = Path.Combine(folderTarget, name);

                if (!File.Exists(targetPath))
                    File.Copy(file, targetPath);
            }
        }
    }
}
        }

        private static HtmlDocument GetPage(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            //Set more parameters here...
            //...

            //This is the important part.
            request.CookieContainer = new CookieContainer();
            request.UserAgent = @"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
            request.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var stream = response.GetResponseStream();

            //When you get the response from the website, the cookies will be stored
            //automatically in "_cookies".

            using (var reader = new StreamReader(stream))
            {
                string html = reader.ReadToEnd();
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                return doc;
            }
        }
    }
}
