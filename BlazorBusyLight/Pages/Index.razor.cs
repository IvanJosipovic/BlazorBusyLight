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
        [Inject] private ILogger<Index> logger { get; set; }

        [Inject] private IAccessTokenProvider authService { get; set; }

        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private Timer timer;

        private string color;

        private string status;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            var user = authState.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                SetTimer();
            }
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

        public void SetTimer()
        {
            timer?.Dispose();
            timer = new Timer(async _ => await Update(), null, 0, 5000);
        }

        private async Task Update()
        {
            try
            {
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

                var user = authState.User;

                if (user?.Identity?.IsAuthenticated == true)
                {
                    var data = await GetPresence();

                    switch (data.availability)
                    {
                        case "Available":
                            color = "bg-success";
                            break;
                        case "Busy":
                        case "DoNotDisturb":
                            color = "bg-danger";
                            break;
                        case "BeRightBack":
                            color = "bg-light";
                            break;
                        case "Away":
                            color = "bg-dark text-light";
                            break;
                        case "Offline":
                            color = "bg-dark text-light";
                            break;
                        default:
                            color = "";
                            break;
                    }

                    status = data.availability;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting presence");
            }
        }

        private async Task<PresenceResponse> GetPresence()
        {
            var tokenResult = await authService.RequestAccessToken(
                new AccessTokenRequestOptions
                {
                    Scopes = new[] { "https://graph.microsoft.com/User.Read", "https://graph.microsoft.com/Presence.Read" }
                }
            );

            if (tokenResult.TryGetToken(out var token))
            {
                logger.LogInformation("Scopes: " + token?.GrantedScopes.Length);
                logger.LogInformation("Token: " + token?.Value);

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.Value);

                    return await client.GetFromJsonAsync<PresenceResponse>("https://graph.microsoft.com/beta/me/presence");
                }
            }

            throw new Exception("Unable to get Presence");
        }
    }
}
