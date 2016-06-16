using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HtmlAgilityPack;

namespace GoogleTracker
{
    public static class Query
    {
        private static string googleQueryURL = "http://google.co.uk/search?q=";

        /* will only do one page now TODO fix */
        public static List<Result> GetSearchResults(string query)
        {
            var ret = new List<Result>();
            string sanitised = SanitiseQuery(query);

            WebRequest wr = WebRequest.Create(googleQueryURL + sanitised);
            Stream pageStream = wr.GetResponse().GetResponseStream();

            Debug.Assert(pageStream != null, "pageStream != null");
            Console.WriteLine("Page retrieved");

            var sr = new StreamReader(pageStream);
            string page = sr.ReadToEnd();

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(page);

            SearchInstance si = ExtractSearchInstance(htmlDoc, query);
            Console.WriteLine("SearchInstance created");

            foreach (var result in htmlDoc.DocumentNode.SelectNodes(".//div[@class='g']"))
            {
                var processedResult = ResultFromRaw(result, si);
                if (processedResult != null)
                    ret.Add(processedResult);
            }
            Console.WriteLine("Processed {0} results", ret.Count);
            return ret;
        }

        public static SearchInstance ExtractSearchInstance(HtmlDocument htmlDoc, string query)
        {
            ulong count = 0;
            HtmlNode resultStats = htmlDoc.GetElementbyId("resultStats");
            foreach (string word in resultStats.InnerText.Split())
            {
                if(word.Any(char.IsDigit) && !word.Contains("."))
                {
                    count = Convert.ToUInt64(RemoveCharacter(word, ','));
                    break;
                }
            }
            return new SearchInstance()
            {
                retrieved = DateTime.Now,
                query = query,
                count = count
            };
        }

        public static string RemoveCharacter(string toChange, char toRemove)
        {
            int idx = toChange.IndexOf(toRemove);
            string ret = toChange;
            while (idx != -1)
            {
                ret = ret.Remove(idx, 1);
                idx = ret.IndexOf(toRemove);
            }
            return ret;
        }

        public static Result ResultFromRaw(HtmlNode node, SearchInstance si)
        {
            var rawTitle = node.SelectSingleNode(".//h3[@class='r']");
            var rawUrl = node.SelectSingleNode(".//cite");
            var rawDesc = node.SelectSingleNode(".//span[@class='st']");
            if (rawTitle == null || rawUrl == null || rawDesc == null)
                return null;
            return new Result()
            {
                url = RemoveTags(rawUrl),
                title = RemoveTags(rawTitle),
                description = RemoveTags(rawDesc),
                searchPageNo = 1,
                si = si
            };
        }

        public static string RemoveTags(HtmlNode node)
        {
            string ret = node.InnerText;
            // do things depending on what ret looks like
            return ret;
        }

        public static string SanitiseQuery(string query)
        {
            string ret = query.Replace(' ', '+');
            ret = ret.Replace('_', '+');
            return ret;
        }
    }
}
