using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorBusyLight.Pages
{
    public partial class Index : IDisposable
    {
        [Inject]
        private ILogger<Index> Logger { get; set; }

        [Inject]
        private IAccessTokenProvider AuthService { get; set; }

        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private Timer Timer;

        private string Color;

        private string Status;

        protected override void OnInitialized()
        {
            Timer?.Dispose();
            Timer = new Timer(async _ => await Update(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        public void Dispose()
        {
            Timer?.Dispose();
        }

        private async Task Update()
        {
            try
            {
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

                if (authState?.User?.Identity?.IsAuthenticated == true)
                {
                    var data = await GetPresence();

                    switch (data.availability)
                    {
                        case "Available":
                            Color = "bg-success";
                            break;
                        case "Busy":
                        case "DoNotDisturb":
                            Color = "bg-danger";
                            break;
                        case "BeRightBack":
                            Color = "bg-light";
                            break;
                        case "Away":
                            Color = "bg-dark text-light";
                            break;
                        case "Offline":
                            Color = "bg-dark text-light";
                            break;
                        default:
                            Color = "";
                            break;
                    }

                    Status = data.availability;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting presence");
            }
        }

        private async Task<PresenceResponse> GetPresence()
        {
            var tokenResult = await AuthService.RequestAccessToken();

            if (tokenResult.TryGetToken(out var token) && token.Value != null)
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.Value);

                return await client.GetFromJsonAsync<PresenceResponse>("https://graph.microsoft.com/beta/me/presence");
            }

            throw new Exception("Unable to get the Token");
        }

        private class PresenceResponse
        {
            public string availability { get; set; }
        }
    }
}
