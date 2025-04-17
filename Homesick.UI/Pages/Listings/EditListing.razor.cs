using Homesick.UI.Layout;
using Homesick.UI.Models;
using Homesick.UI.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting.Server;
using MudBlazor;
using Newtonsoft.Json;
using System;
using System.Reflection;
using static MudBlazor.CategoryTypes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Homesick.UI.Pages.Listings
{
    public partial class EditListing
    {
        [Parameter] public int id { get; set; }

        public ListingDto listing = new ListingDto();
        public HouseDto house = new HouseDto();
        private List<string> _areas = SD.Areas;
        private MudFileUpload<IReadOnlyList<IBrowserFile>> _fileUpload;
        bool success = false;
        List<string> errors = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            ResponseDto response = await listingService.GetListing(id);

            if (response != null && response.IsSuccess)
            {
                listing = JsonConvert.DeserializeObject<ListingDto>(Convert.ToString(response.Result));
                house = listing.House;
            }
            else
            {
                Snackbar.Add("Error loading listing", Severity.Error);
            }

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
                    var resizedImage = await image.RequestImageFileAsync(format, 300, 300);

                    using var stream = image.OpenReadStream();
                    using var memoryStream = new MemoryStream();

                    await stream.CopyToAsync(memoryStream);
                    var buffer = memoryStream.ToArray();

                    var imageData = $"data:{format};base64,{Convert.ToBase64String(buffer)}";

                    Console.WriteLine($"Base64 Hash: {Convert.ToBase64String(buffer).Substring(0, 30)}...");

                    house.Images.Add(new ImageDto {Name=image.Name, Data = imageData });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing {image.Name}: {ex}");
                }
            }
        }
        private async Task ClearAsync()
        {
            house.Images?.Clear();

            if (_fileUpload is not null)
            {
                await _fileUpload.ClearAsync(); // Resets the internal file list
            }

            StateHasChanged(); // Force UI refresh
        }

        private void DeleteFile(ImageDto file)
        {
            house.Images?.RemoveAll(img => img.Name == file.Name); // Adjust this logic if filenames aren't the same

            StateHasChanged();
        }

        private async Task HandleValidSubmit()
        {
            success = await Validate();

            if (success)
            {
                try
                {
                    ResponseDto responseDto = await listingService.UpdateListing(listing);
                    if (responseDto?.IsSuccess == true)
                    {
                        Navigation.NavigateTo("/profile");
                        Snackbar.Add("Listing updated successfully!", Severity.Success);
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
                var parameters = new DialogParameters<ErrorDialog> { { x => x.errors, errors } };

                var dialog = await DialogService.ShowAsync<ErrorDialog>("Λάθη", parameters);
            }
        }

        private async Task<bool> Validate()
        {
            errors.Clear();
            int count = 0;
            if (string.IsNullOrEmpty(house.Area))
            {
                errors.Add("Το πεδίο Περιοχή είναι απαραίτητο");
                count++;
            }
            if (string.IsNullOrEmpty(house.Name))
            {
                errors.Add("Ο τίτλος του σπιτιού είναι απαραίτητος");
                count++;
            }
            if (listing.ListingType == null)
            {
                errors.Add("Το πεδίο Τύπος Αγγελίας είναι απαραίτητο");
                count++;
            }
            if (house.PropertyType == null)
            {
                errors.Add("Το πεδίο Κατηγορία Ακινήτου είναι απαραίτητο");
                count++;
            }
            if (house.HouseType == null)
            {
                errors.Add("Το είδος ακινήτου είναι απαραίτητο");
                count++;
            }
            if (string.IsNullOrEmpty(listing.Name))
            {
                errors.Add("Το όνομα του ιδιοκτήτη είναι απαραίτητο");
                count++;
            }
            if (house.Price == 0)
            {
                errors.Add("Η τιμή του ακινήτου είναι απαραίτητη");
                count++;
            }

            if (count > 0) return false;

            return true;
        }
    }
}
