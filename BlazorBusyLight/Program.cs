using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorBusyLight
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddBaseAddressHttpClient();

            builder.Services.AddMsalAuthentication(options =>
            {
                options.ProviderOptions.Authentication.ClientId = "a77357bb-e91e-4595-b29c-96047da062f0";

                options.ProviderOptions.AdditionalScopesToConsent.Add("https://graph.microsoft.com/User.Read");
                options.ProviderOptions.AdditionalScopesToConsent.Add("https://graph.microsoft.com/Presence.Read");

                options.ProviderOptions.DefaultAccessTokenScopes.Add("https://graph.microsoft.com/User.Read");
                options.ProviderOptions.DefaultAccessTokenScopes.Add("https://graph.microsoft.com/Presence.Read");
            });

            await builder.Build().RunAsync();
        }
    }
}
