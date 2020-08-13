using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RceServer.Core.Infrastructure;

namespace RceServer.Core.Services.Implementation
{
	public class HttpClientService : IHttpClientService
	{
		private readonly HttpClient _httpClient;

		public HttpClientService()
		{
			_httpClient = new HttpClient(new RetryHandler(new HttpClientHandler()));
		}

		public Task<HttpResponseMessage> Get(string url)
		{
			if (url.StartsWith("http", StringComparison.InvariantCultureIgnoreCase) == false)
			{
				url = $"https://{url}";
			}

			return _httpClient.GetAsync(new Uri(url));
		}

		public Task<HttpResponseMessage> Post(string url, HttpContent httpContent)
		{
			if (url.StartsWith("http", StringComparison.InvariantCultureIgnoreCase) == false)
			{
				url = $"https://{url}";
			}

			return _httpClient.PostAsync(new Uri(url), httpContent);
		}
	}
}
