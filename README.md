# Graph API implemented in Asp.NET Core 

## Project Overview
Making Graph API request to list all the users in a specific tenant. Project is only focused on implementing Graph API inside the Asp.NET Core, and not the UI of the app.

The core of this application is the Program.cs file, which includes all the necessary configuration for the run.

```text
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
```
