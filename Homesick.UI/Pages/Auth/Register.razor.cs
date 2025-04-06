using Homesick.UI.Models;
using Homesick.UI.Utility;
using MudBlazor;
using System.Text.RegularExpressions;

namespace Homesick.UI.Pages.Auth
{
    public partial class Register
    {
        bool success;
        string[] errors = { };
        MudTextField<string> pwField1;
        MudForm form;
        RegistrationRequestDto registrationRequest = new RegistrationRequestDto();

        private IEnumerable<string> PasswordStrength(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                yield return "Password is required!";
                yield break;
            }
            if (pw.Length < 8)
                yield return "Password must be at least of length 8";
            if (!Regex.IsMatch(pw, @"[A-Z]"))
                yield return "Password must contain at least one capital letter";
            if (!Regex.IsMatch(pw, @"[a-z]"))
                yield return "Password must contain at least one lowercase letter";
            if (!Regex.IsMatch(pw, @"[0-9]"))
                yield return "Password must contain at least one digit";
        }

        private string PasswordMatch(string arg)
        {
            if (pwField1.Value != arg)
                return "Passwords don't match";
            return null;
        }

        public async void RegisterAsync()
        {
            ResponseDto result = await _authService.RegisterAsync(registrationRequest);
            ResponseDto assingRole;

            if (result != null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(registrationRequest.Role))
                {
                    registrationRequest.Role = SD.RoleCustomer;
                }
                assingRole = await _authService.AssignRoleAsync(registrationRequest);
                if (assingRole.IsSuccess)
                {
                    Navigation.NavigateTo("/");
                }
                else
                {
                    Snackbar.Add(assingRole.Message);
                }
            }
            else
            {
                Snackbar.Add(result?.Message);
            }


        }
    }
}
