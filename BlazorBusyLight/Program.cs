using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
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
                var authentication = options.ProviderOptions.Authentication;
                authentication.Authority = "https://login.microsoftonline.com/b82075ce-f897-4df8-8624-47b71a1fd251";
                authentication.ClientId = "522dd1fb-4748-4698-8df6-5b14119a9042";
                //options.ProviderOptions.AdditionalScopesToConsent.Add("https://graph.microsoft.com/Presence.Read");
                //options.ProviderOptions.AdditionalScopesToConsent.Add("https://graph.microsoft.com/User.Read");
            });

            await builder.Build().RunAsync();
        }
    }
}
