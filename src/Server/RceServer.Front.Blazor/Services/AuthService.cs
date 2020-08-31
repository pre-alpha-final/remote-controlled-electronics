using Microsoft.AspNetCore.Components;
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
		private readonly NavigationManager _navigationManager;

		public event Func<Task> OnChanged;
		private bool _userAuthenticated;
		public bool UserAuthenticated
		{
			get { return _userAuthenticated; }
			set
			{
				_userAuthenticated = value;
				OnChanged?.Invoke();
				if (_userAuthenticated == false)
				{
					_navigationManager.NavigateTo("/auth/login");
				}
			}
		}

		public AuthService(IConfiguration configuration, IHttpClientService httpClientService, NavigationManager navigationManager)
		{
			_configuration = configuration;
			_httpClientService = httpClientService;
			_navigationManager = navigationManager;
		}

		public async Task<string> Register(RegisterModel registerModel)
		{
			try
			{
				throw new NotImplementedException();
			}
			catch (Exception e)
			{
				return e.Message;
			}
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
		}

		public async Task<string> ConfirmEmail(string userId, string code)
		{
			try
			{
				var response = await _httpClientService.Get($@"{_configuration["Domain"]}/api/auth/checkemail?userId={userId}&code={code}");
				if (response.IsSuccessStatusCode == false)
				{
					return response.ReasonPhrase;
				}
				var responseContent = await response.Content.ReadAsStringAsync();

				return responseContent.Contains("error")
					? responseContent
					: string.Empty;
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		public async Task<string> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
		{
			try
			{
				throw new NotImplementedException();
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		public async Task<string> ResetPassword(ResetPasswordModel resetPasswordModel)
		{
			try
			{
				throw new NotImplementedException();
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}
	}
}
