using System;
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
			return _httpClient.GetAsync(new Uri(url));
		}

		public Task<HttpResponseMessage> Post(string url, HttpContent httpContent)
		{
			return _httpClient.PostAsync(new Uri(url), httpContent);
		}
	}
}
