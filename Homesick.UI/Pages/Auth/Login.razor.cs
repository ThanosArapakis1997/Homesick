using Homesick.UI.Models;
using Homesick.UI.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Homesick.UI.Pages.Auth
{
    public partial class Login
    {
        LoginRequestDto model = new LoginRequestDto();

        public async void LoginAsync()
        {
            ResponseDto responseDto = await _authService.LoginAsync(model);

            if (responseDto != null && responseDto.IsSuccess)
            {
                LoginResponseDto loginResponseDto =
                    JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));

                _sessionStorage.SetItemAsync("jwt", loginResponseDto.Token);

                var authStateProvider = _authenticationStateProvider;
                authStateProvider.NotifyUserAuthentication(loginResponseDto.Token);

                Navigation.NavigateTo("/");
            }
            else
            {
                Snackbar.Add(responseDto?.Message);
            }
        }        
    }
}
