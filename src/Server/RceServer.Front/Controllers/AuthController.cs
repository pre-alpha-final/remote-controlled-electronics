using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RceServer.Core.Services;
using RceServer.Front.Controllers.Models;

namespace RceServer.Front.Controllers
{
	[Route("api/auth/")]
	public class AuthController : Controller
	{
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IHttpClientService _httpClientService;
		private readonly IConfiguration _configuration;
		private readonly IEmailSender _emailSender;

		public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
			IHttpClientService httpClientService, IConfiguration configuration, IEmailSender emailSender)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_httpClientService = httpClientService;
			_configuration = configuration;
			_emailSender = emailSender;
		}

		//[HttpPost("register")]
		//public async Task<IActionResult> Register([FromBody] RegisterModel model)
		//{
		//	/*
		//	//Dummy user
		//	_userManager.PasswordValidators.Clear();
		//	var user = new IdentityUser { UserName = "username" };
		//	var result = await _userManager.CreateAsync(user, "password");
		//	await _userManager.AddClaimAsync(user, new Claim("RceServerApiAccess", "true"));
		//	*/

		//	if (model.Password != model.Password2)
		//	{
		//		return Ok(new ErrorResponse { Error = "Passwords don't match" });
		//	}

		//	var user = new IdentityUser { UserName = model.Email, Email = model.Email };
		//	var result = await _userManager.CreateAsync(user, model.Password);
		//	await _userManager.AddClaimAsync(user, new Claim("RceServerApiAccess", "true"));
		//	if (result.Succeeded)
		//	{
		//		var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
		//		var callbackUrl = $"https://{_configuration["Domain"]}/auth/emailconfirmation?userId={user.Id}&code={code}";
		//		await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

		//		await _signInManager.SignInAsync(user, false);

		//		return Ok();
		//	}

		//	return BadRequest(new ErrorResponse
		//	{
		//		Error = string.Join(", ", result.Errors.Select(e => e.Description))
		//	});
		//}

		[HttpPost("login")]
		public async Task<IActionResult> LogIn([FromBody] LoginModel model)
		{
			var authority = $"{_configuration["Authority"]}/connect/token";
			var clientSecret = _configuration["ClientSecret"];
			var httpResponseMessage = await _httpClientService.Post(authority,
				new FormUrlEncodedContent(new Dictionary<string, string>
				{
					{ "grant_type", "password" },
					{ "username", model.Login },
					{ "password", model.Password },
					{ "scope", "rceserverapi offline_access" },
					{ "client_id", "rceserver" },
					{ "client_secret", clientSecret },
				}));

			return StatusCode((int)httpResponseMessage.StatusCode,
				await httpResponseMessage.Content.ReadAsStringAsync());
		}

		[HttpPost("refresh")]
		public async Task<IActionResult> Refresh([FromBody] RefreshTokenResponse model)
		{
			var authority = $"{_configuration["Authority"]}/connect/token";
			var clientSecret = _configuration["ClientSecret"];
			var httpResponseMessage = await _httpClientService.Post(authority,
				new FormUrlEncodedContent(new Dictionary<string, string>
				{
					{ "grant_type", "refresh_token" },
					{ "refresh_token", model.RefreshToken },
					{ "scope", "rceserverapi" },
					{ "client_id", "rceserver" },
					{ "client_secret", clientSecret },
				}));

			return StatusCode((int)httpResponseMessage.StatusCode,
				await httpResponseMessage.Content.ReadAsStringAsync());
		}

		[HttpGet("logout")]
		public async Task<IActionResult> LogOut()
		{
			await _signInManager.SignOutAsync();

			return NoContent();
		}

		[HttpGet("checkemail")]
		public async Task<IActionResult> CheckEmail(string userId, string code)
		{
			if (userId == null || code == null)
			{
				return Ok(JsonConvert.SerializeObject(new ErrorResponse
				{
					Error = "Invalid arguments",
				}));
			}

			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return Ok(JsonConvert.SerializeObject(new ErrorResponse
				{
					Error = $"Unable to load user with ID '{userId}'",
				}));
			}

			var result = await _userManager.ConfirmEmailAsync(user, code.Replace(' ', '+'));
			if (result.Succeeded == false)
			{
				return Ok(JsonConvert.SerializeObject(new ErrorResponse
				{
					Error = $"Error confirming email for user with ID '{userId}'",
				}));
			}

			return Ok();
		}

		[HttpPost("forgotpassword")]
		public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
			{
				return Ok(JsonConvert.SerializeObject(new ErrorResponse
				{
					Error = "User does not exist or email unconfirmed"
				}));
			}

			var code = await _userManager.GeneratePasswordResetTokenAsync(user);
			var callbackUrl = $"https://{_configuration["Domain"]}/auth/resetpassword?userId={user.Id}&code={code}";
			await _emailSender.SendResetPasswordAsync(model.Email, callbackUrl);

			return Ok();
		}

		[HttpPost("resetpassword")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
		{
			if (model.Password != model.Password2)
			{
				return Ok(new ErrorResponse { Error = "Passwords don't match" });
			}

			var user = await _userManager.FindByIdAsync(model.UserId);
			if (user == null)
			{
				return Ok(JsonConvert.SerializeObject(new ErrorResponse
				{
					Error = "Invalid user"
				}));
			}

			var result = await _userManager.ResetPasswordAsync(user, model.Code.Replace(' ', '+'), model.Password);
			if (result.Succeeded)
			{
				return Ok();
			}

			return Ok(new ErrorResponse { Error = "Password reset error" });
		}
	}
}
