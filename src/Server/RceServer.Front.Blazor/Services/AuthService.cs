using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RceServer.Core.Services;
using RceServer.Front.Blazor.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RceServer.Front.Blazor.Services
{
	public class AuthService
	{
		private readonly IConfiguration _configuration;
		private readonly IHttpClientService _httpClientService;

		public event Func<Task> OnChanged;
		public bool UserAuthenticated { get; set; }

		public AuthService(IConfiguration configuration, IHttpClientService httpClientService)
		{
			_configuration = configuration;
			_httpClientService = httpClientService;
		}

		public async Task<string> LogIn(LogInModel logInModel)
		{
			try
			{
				var response = await _httpClientService.Post($@"{_configuration["Domain"]}/api/auth/login",
					new StringContent(JsonConvert.SerializeObject(new Dictionary<string, string>
					{
						{ "email", logInModel.Email },
						{ "password", logInModel.Password },
					}), Encoding.UTF8, "application/json"));
				UserAuthenticated = response.IsSuccessStatusCode;

				return response.IsSuccessStatusCode
					? string.Empty
					: await response.Content.ReadAsStringAsync();
			}
			catch (Exception e)
			{
				UserAuthenticated = false;
				return e.Message;
			}
			finally
			{
				OnChanged?.Invoke();
			}
		}

		public async Task<string> Register(RegisterModel registerModel)
		{
			try
			{
				return "no";
			}
			catch (Exception e)
			{
				return e.Message;
			}
			finally
			{
				OnChanged?.Invoke();
			}
		}
	}
}
