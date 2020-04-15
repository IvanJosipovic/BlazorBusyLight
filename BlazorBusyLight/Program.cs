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
                authentication.Authority = "https://login.microsoftonline.com/ce6ec000-1cfa-49c2-a24f-7db63c8a9a52";
                authentication.ClientId = "5c7e0bc4-87f3-4db5-8294-964ff1b43d8b";
                
                //options.ProviderOptions.AdditionalScopesToConsent.Add("https://graph.microsoft.com/Presence.Read");
                //options.ProviderOptions.AdditionalScopesToConsent.Add("https://graph.microsoft.com/User.Read");
            });

            await builder.Build().RunAsync();
        }
    }
}
