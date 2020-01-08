using System;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RceServer.Core.Hubs;
using RceServer.Core.Services;
using RceServer.Core.Services.Implementation;
using RceServer.Data;
using RceServer.Data.Identity;
using RceServer.Data.Identity.Models;
using RceServer.Domain.Services;
using RceServer.Front.Infrastructure;

namespace RceServer.Front
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
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services.AddSingleton<IHttpClientService, HttpClientService>();
			services.AddSingleton<IAzureKicker, AzureKicker>();
			services.AddSingleton<IMaintenanceService, MaintenanceService>();
			services.AddTransient<IEmailSender, EmailSender>();
			services.AddTransient<IClientService, ClientServiceMock>();
			services.AddTransient<IMessageRepository, InMemoryMessageRepository>();

			services.AddDbContext<UsersDbContext>();
			services.AddIdentity<IdentityUser, IdentityRole>()
				.AddEntityFrameworkStores<UsersDbContext>()
				.AddDefaultTokenProviders();

			services.AddIdentityServer()
				.AddSigningCredential(new SigningCredentials(
					new JsonWebKey(Configuration["IdentityJwk"]),
					SecurityAlgorithms.RsaSha256Signature))
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

			services.AddMvc();

			services
				.AddSignalR(e => e.KeepAliveInterval = TimeSpan.FromSeconds(SignalRKeepAlive))
				.AddAzureSignalR(Configuration.GetConnectionString("SignalR"));
			services.AddSingleton<IUserIdProvider, UsernameIdProvider>();

			services.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(options =>
				{
					options.Authority = Configuration["Authority"];
					options.Audience = "rceserverapi";
#if !DEBUG
					options.RequireHttpsMetadata = true;
#else
					options.RequireHttpsMetadata = false;
#endif
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

		public void Configure(IApplicationBuilder app, IHostingEnvironment env,
			IAzureKicker azureKicker, IMaintenanceService maintenanceService)
		{
			if (env.IsDevelopment())
			{
				//app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

#if !DEBUG
			app.UseHttpsRedirection();
#endif
			app.UseFileServer();
			app.UseAzureSignalR(routes =>
			{
				routes.MapHub<RceHub>("/rce");
			});
			app.UseSpaStaticFiles();

			app.UseIdentityServer();
			app.UseAuthentication();
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller}/{action=Index}/{id?}");
			});

			app.UseSpa(spa =>
			{
				spa.Options.SourcePath = "ClientApp";

				if (env.IsDevelopment())
				{
					spa.UseAngularCliServer(npmScript: "start");
				}
			});

			azureKicker.Start();
			maintenanceService.Start();
		}
	}
}
