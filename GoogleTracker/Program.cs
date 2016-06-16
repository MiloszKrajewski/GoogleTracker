using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace GoogleTracker
{
	public class SearchInstance
	{
		public DateTime Retreived { get; set; }
		public string Query { get; set; }
		public long Count { get; set; }
	}

	public class Result
	{
		public string Url { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int SearchPageNo { get; set; }
		public SearchInstance Instance { get; set; }
	}

	public static class Query
	{
		private const string GoogleQueryUrl = "http://google.co.uk/search?q=";

		public static string SanitiseQuery(string query) { return Regex.Replace(query, "[ _]", "+"); }
		private static long? TryParseLong(string text)
		{
			try { return long.Parse(text); } catch { return null; }
		}

		public static IEnumerable<Result> GetSearchResults(string query)
		{
			var document = new HtmlDocument();
			var request = WebRequest.Create(GoogleQueryUrl + SanitiseQuery(query));
			var response = request.GetResponse();
			document.Load(response.GetResponseStream());
			var instance = ExtractSearchInstance(document, query);
			return document.DocumentNode
				.SelectNodes(".//div[@class='g']")
				.Select(r => ResultFromRaw(r, instance))
				.Where(r => r != null);
		}

		private static SearchInstance ExtractSearchInstance(HtmlDocument document, string query)
		{
			var count = document.GetElementbyId("resultStats").InnerText.Split()
				.Select(word => word.Replace(",", string.Empty))
				.Select(TryParseLong)
				.FirstOrDefault(c => c != null) ?? 0;
			return new SearchInstance {
				Retreived = DateTime.Now,
				Query = query,
				Count = count
			};
		}

		private static Result ResultFromRaw(HtmlNode node, SearchInstance instance)
		{
			var title = node.SelectSingleNode(".//h3[@class='r']");
			var url = node.SelectSingleNode(".//cite");
			var description = node.SelectSingleNode(".//span[@class='st']");
			return (title == null || url == null || description == null)
				? null
				: new Result {
					Url = url.InnerText,
					Title = title.InnerText,
					Description = description.InnerText,
					SearchPageNo = 1,
					Instance = instance
				};
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			var results = Query.GetSearchResults("Flying Colors");
			Console.WriteLine(results.Count());
			Console.ReadLine();
		}
	}
}
