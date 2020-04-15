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
                authentication.ClientId = "a77357bb-e91e-4595-b29c-96047da062f0";
                //options.ProviderOptions.AdditionalScopesToConsent.Add("https://graph.microsoft.com/Presence.Read");
                //options.ProviderOptions.AdditionalScopesToConsent.Add("https://graph.microsoft.com/User.Read");
            });

            await builder.Build().RunAsync();
        }
    }
}
