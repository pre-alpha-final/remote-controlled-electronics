using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RceServer.Core.Infrastructure
{
	public class RetryHandler : DelegatingHandler
	{
		private const int MaxRetries = 2;

		public RetryHandler(HttpMessageHandler innerHandler)
			: base(innerHandler)
		{ }

		protected override async Task<HttpResponseMessage> SendAsync(
			HttpRequestMessage request, CancellationToken cancellationToken)
		{
			HttpResponseMessage response = null;
			for (var i = 0; i < MaxRetries; i++)
			{
				response = await base.SendAsync(request, cancellationToken);
				if (response.IsSuccessStatusCode)
				{
					return response;
				}

				await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
			}

			return response;
		}
	}
}
