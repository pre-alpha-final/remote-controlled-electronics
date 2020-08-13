using System;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using RceServer.Core.Services;
using RceServer.Core.Services.Implementation;
using RceServer.Data;
using RceServer.Data.Identity;
using RceServer.Data.Identity.Models;
using RceServer.Domain.Services;
using RceServer.Front.Blazor.Infrastructure;
using RceServer.Front.Blazor.Services;

namespace RceServer.Front.Blazor
{
	public class Startup
	{
		private const int SignalRKeepAlive = 10;
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddRazorPages();
			services.AddServerSideBlazor();
			services.AddControllersWithViews().AddNewtonsoftJson(); ;
			services.AddRazorPages().AddNewtonsoftJson();

			services.AddSingleton<IHttpClientService, HttpClientService>();
			services.AddSingleton<IAzureKicker, AzureKicker>();
			services.AddSingleton<IMaintenanceService, MaintenanceService>();
			services.AddTransient<IEmailSender, EmailSender>();
			services.AddTransient<IServerService, ServerService>();
			services.AddTransient<IWorkerService, WorkerService>();
			services.AddTransient<IMessageRepository, MessageRepository>();
			services.AddSingleton<WeatherForecastService>();
			services.AddScoped<AuthService>();

			services.AddDbContext<UsersDbContext>();
			services.AddIdentity<IdentityUser, IdentityRole>()
				.AddEntityFrameworkStores<UsersDbContext>()
				.AddDefaultTokenProviders();

			services.AddSingleton<IMongoClient>(e =>
			{
				var configuration = e.GetService<IConfiguration>();
				return new MongoClient(configuration.GetConnectionString("RceMessagesDb"));
			});

			services.AddIdentityServer()
				.AddSigningCredential(new SigningCredentials(
					new JsonWebKey(Configuration["IdentityJwk"]),
					SecurityAlgorithms.RsaSha256))
				.AddOperationalStore(options =>
					options.ConfigureDbContext = builder =>
						builder.UseMySql(Configuration.GetConnectionString("UsersDbContext")))
				.AddInMemoryIdentityResources(Config.GetIdentityResources())
				.AddInMemoryApiResources(Config.GetApiResources())
				.AddInMemoryClients(Config.GetClients(Configuration["ClientSecret"]))
				.AddAspNetIdentity<IdentityUser>();
			services.AddTransient<IProfileService, IdentityClaimsProfileService>();
			services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();

			services.AddApplicationInsightsTelemetry(Configuration["InstrumentationKey"]);

			services.AddMvc().AddNewtonsoftJson();

			services
				.AddSignalR(e => e.KeepAliveInterval = TimeSpan.FromSeconds(SignalRKeepAlive))
				.AddAzureSignalR(Configuration.GetConnectionString("SignalR"))
				.AddNewtonsoftJsonProtocol();
			services.AddSingleton<IUserIdProvider, UsernameIdProvider>();

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.Authority = Configuration["Authority"];
				options.Audience = "rceserverapi";
				options.RequireHttpsMetadata = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateAudience = true,
					ValidateIssuer = true,
					ValidateIssuerSigningKey = true,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero,
				};

				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						var accessToken = context.Request.Query["access_token"];

						var path = context.HttpContext.Request.Path;
						if (string.IsNullOrEmpty(accessToken) == false &&
							path.StartsWithSegments("/hubs/rce"))
						{
							context.Token = accessToken;
						}

						return Task.CompletedTask;
					}
				};
			});

			services.AddAuthorization(options =>
			{
				options.AddPolicy("RceServerApiAccess", policy => policy.RequireClaim("rce_server_api_access", "true"));
			});

			services.AddSpaStaticFiles(configuration =>
			{
				configuration.RootPath = "ClientApp/dist";
			});

			services.AddLogging(e =>
			{
				e.AddDebug();
				e.AddAzureWebAppDiagnostics();
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
			IServiceProvider serviceProvider)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseIdentityServer();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
			});

			serviceProvider.GetService<IAzureKicker>().Start();
			serviceProvider.GetService<IMaintenanceService>().Start();
		}
	}
}
