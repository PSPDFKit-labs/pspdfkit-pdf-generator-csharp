using System;
using System.IO;
using System.Net;
using RestSharp;

namespace PspdfkitApiDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			var client = new RestClient("https://api.pspdfkit.com/build");

			var request = new RestRequest(Method.POST)
			  .AddHeader("Authorization", "Bearer {YOUR_API_KEY}") // Replace {YOUR_API_KEY} with your API key.")
			  .AddFile("index.html", "index.html")
			  .AddFile("style.css", "style.css")
			  .AddFile("Inter-Regular.ttf", "Inter-Regular.ttf")
			  .AddFile("Inter-Medium.ttf", "Inter-Medium.ttf")
			  .AddFile("Inter-Bold.ttf", "Inter-Bold.ttf")
			  .AddFile("SpaceMono-Regular.ttf", "SpaceMono-Regular.ttf")
			  .AddFile("logo.svg", "logo.svg")
			  .AddParameter("instructions", new JsonObject
			  {
				  ["parts"] = new JsonArray
				{
			new JsonObject
			{
			  ["html"] = "index.html",
			  ["assets"] = new JsonArray
			  {
				"style.css",
				"Inter-Regular.ttf",
				"Inter-Medium.ttf",
				"Inter-Bold.ttf",
				"SpaceMono-Regular.ttf",
				"logo.svg"
			  }
			}
				}
			  }.ToString());

			request.AdvancedResponseWriter = (responseStream, response) =>
			{
				if (response.StatusCode == HttpStatusCode.OK)
				{
					using (responseStream)
					{
						using var outputFileWriter = File.OpenWrite("result.pdf");
						responseStream.CopyTo(outputFileWriter);
					}
				}
				else
				{
					var responseStreamReader = new StreamReader(responseStream);
					Console.Write(responseStreamReader.ReadToEnd());
				}
			};

			client.Execute(request);
		}
	}
}
