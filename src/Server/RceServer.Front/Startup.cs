#define HTTPS

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RceServer.Front
{
	public class Startup
	{
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
			services.AddTransient<IEmailSender, EmailSender>();

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

			services.AddMvc();

			services.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(options =>
				{
					options.Authority = Configuration["Authority"];
					options.Audience = "rceserverapi";
#if HTTPS
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
			IAzureKicker azureKicker)
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

#if HTTPS
			app.UseHttpsRedirection();
#endif
			app.UseStaticFiles();
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
		}
	}
}
