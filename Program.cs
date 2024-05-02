using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using Microsoft.Identity.Web;

namespace GraphAPI_WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

            // Add configuration sources
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            ConfigureServices(builder.Services, (Microsoft.AspNetCore.Builder.WebApplicationBuilder)builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        public static void ConfigureServices(IServiceCollection services, Microsoft.AspNetCore.Builder.WebApplicationBuilder builder)
        {
            services.AddControllersWithViews();

            services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration)
            .EnableTokenAcquisitionToCallDownstreamApi()
            .AddInMemoryTokenCaches()
            .AddMicrosoftGraph();

            services.AddSingleton(provider =>
            {
                var configuration = provider.GetRequiredService<Microsoft.Extensions.Configuration.IConfiguration>();
                string tenantId = configuration.GetValue<string>("AzureAd:TenantId");
                string clientId = configuration.GetValue<string>("AzureAd:ClientId");
                string clientSecret = configuration.GetValue<string>("AzureAd:ClientSecret");

                var clientSecretCred = new ClientSecretCredential(tenantId, clientId, clientSecret);
                return new GraphServiceClient(clientSecretCred);
            });

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
        }
    }
}
