using Homesick.UI.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using MudBlazor;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.Tracing;
using System.IO;
using Homesick.UI.Utility;
using System.Xml.Linq;

namespace Homesick.UI.Pages.Listings
{
    public partial class CreateListing
    {
        private int _index;
        private bool _completed;
        private bool _open = false;
        private int progress = 0;
        MudForm form;

        private List<string> _areas = SD.Areas;
        private ListingDto listing = new();
        private HouseDto model = new();
        private MudFileUpload<IReadOnlyList<IBrowserFile>> _fileUpload;
        private List<string> filenames = new List<string>();

        private AuthenticationState authState;
        public int count = 0;
        bool success = false;
        List<string> errors = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            listing.UserId = authState.User.FindFirst("sub").Value;
            listing.Email = authState.User.FindFirst("email").Value;
        }

        private async Task<IEnumerable<string>> Search(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _areas;

            return _areas.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        async Task UploadFiles(InputFileChangeEventArgs e)
        {
            await _fileUpload.ClearAsync();
            var format = "image/png";
            foreach (var image in e.GetMultipleFiles())
            {
                try
                {
                    filenames.Add(image.Name);

                    var resizedImage = await image.RequestImageFileAsync(format, 300, 300);

                    using var stream = image.OpenReadStream();
                    using var memoryStream = new MemoryStream();

                    await stream.CopyToAsync(memoryStream);
                    var buffer = memoryStream.ToArray();

                    var imageData = $"data:{format};base64,{Convert.ToBase64String(buffer)}";

                    Console.WriteLine($"Base64 Hash: {Convert.ToBase64String(buffer).Substring(0, 30)}...");

                    model.Images.Add(new ImageDto {Data = imageData });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing {image.Name}: {ex}");
                }
            }
        }       

        private async Task HandleValidSubmit()
        {
            success =await Validate();

            if (success)
            {
                try
                {
                    listing.House = model;
                    listing.Status = SD.StatusInReview;

                    ResponseDto responseDto = await listingService.CreateListing(listing);
                    if (responseDto?.IsSuccess == true)
                    {
                        Navigation.NavigateTo("/");
                        Snackbar.Add("Listing created successfully!", Severity.Success);
                    }
                    else
                    {
                        Snackbar.Add(responseDto?.Message ?? "Failed to create listing", Severity.Error);
                    }
                }
                catch (Exception ex)
                {
                    Snackbar.Add($"Submission failed: {ex.Message}", Severity.Info);
                }
            }
            else
            {
                foreach(string error in errors)
                {
                    _open = true;
                }
            }
        }

        private async Task<bool> Validate()
        {
            errors.Clear();
            int count = 0;
            if (string.IsNullOrEmpty(model.Area))
            {
                errors.Add("Το πεδίο Περιοχή είναι απαραίτητο");
                count++;
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                errors.Add("Ο τίτλος του σπιτιού είναι απαραίτητος");
                count++;
            }
            if (listing.ListingType == null)
            {
                errors.Add("Το πεδίο Τύπος Αγγελίας είναι απαραίτητο");
                count++;
            }
            if (model.PropertyType == null)
            {
                errors.Add("Το πεδίο Κατηγορία Ακινήτου είναι απαραίτητο");
                count++;
            }
            if (model.HouseType == null)
            {
                errors.Add("Το είδος ακινήτου είναι απαραίτητο");
                count++;
            }
            if (string.IsNullOrEmpty(listing.Name))
            {
                errors.Add("Το όνομα του ιδιοκτήτη είναι απαραίτητο");
                count++;
            }           
            if (model.Price == 0)
            {
                errors.Add("Η τιμή του ακινήτου είναι απαραίτητη");
                count++;
            }

            if (count > 0) return false;

            return true;
        }        
    }
}
