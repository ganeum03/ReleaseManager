using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace NoteApp.UI.Pages
{
    public partial class Index
    {
        private LoginModel loginModel = new LoginModel();
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private HttpClient Http { get; set; }
        [Inject]
        IJSRuntime JSRuntime { get; set; }
        protected async Task LoginToApp()
        {
            var Result = await Http.PostAsJsonAsync("login", loginModel);
            if (Result.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Invalid Login");
            }
            else
            {
                //        var claims = new List<Claim>
                //{
                //    new Claim(ClaimTypes.Name, loginModel.UserId),
                //};

                //        var identity = new ClaimsIdentity(claims, "cookie");
                //        var user = new ClaimsPrincipal(identity);
                //await AuthenticationStateProvider.GetAuthenticationStateAsync();
                NavigationManager.NavigateTo("/homeq");
            }
        }

        private class LoginModel
        {
            [Required(ErrorMessage = "Username is required")]
            public string UserId { get; set; }

            [Required(ErrorMessage = "Password is required")]
            public string Password { get; set; }
        }
    }
}
