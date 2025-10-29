using Homesick.UI.Layout;
using Homesick.UI.Models;
using Homesick.UI.Service;
using Homesick.UI.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.JSInterop;
using MudBlazor;
using System.Diagnostics.Tracing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Homesick.UI.Pages.Listings
{
    public partial class CreateListing
    {
        private int _index;
        private bool _completed;
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
        public string priceLabel = "Τιμή Ακινήτου";

        private List<ChatMessage> _messages = new();
        private string _inputMessage = string.Empty;
        private bool _isLoading = false;
        private string? _error = null;
        private CancellationTokenSource? _cancellationTokenSource;
        private IJSObjectReference? _jsModule;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/chat.js");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to load JS module: {ex.Message}");
                }
            }
        }

        private async Task HandleKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "Enter" && !e.ShiftKey && !_isLoading)
                await SendMessage();
        }

        private async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(_inputMessage) || _isLoading)
                return;

            var userMessage = _inputMessage.Trim();
            _inputMessage = string.Empty;
            _error = null;

            _messages.Add(new ChatMessage
            {
                Text = userMessage,
                IsUser = true,
                Timestamp = DateTime.Now
            });

            var assistantMessage = new ChatMessage
            {
                IsUser = false,
                IsStreaming = true,
                Timestamp = DateTime.Now
            };

            _messages.Add(assistantMessage);

            _isLoading = true;
            StateHasChanged();
            await ScrollToBottom();

            try
            {
                _cancellationTokenSource = new CancellationTokenSource();
                var requestDto = new RequestDto
                {
                    Url = SD.AgentAPIBase + "/stream",
                    Data = new { prompt = userMessage }
                };

                await foreach (var response in StreamService.SendAsync(requestDto, _cancellationTokenSource.Token))
                {
                    if (response != null)
                    {

                        if (!string.IsNullOrEmpty(response.Token))
                        {
                            assistantMessage.Text += response.Token;

                            await InvokeAsync(StateHasChanged);

                            if (assistantMessage.Text.Length % 50 == 0)
                                await ScrollToBottom();

                            await Task.Yield();
                        }

                        if (response.Done)
                        {
                            assistantMessage.IsStreaming = false;
                           

                            StateHasChanged();
                            await ScrollToBottom();
                            break;
                        }
                    }
                }
                assistantMessage.IsStreaming = false;
                await InvokeAsync(StateHasChanged);
            }
            catch (OperationCanceledException)
            {
                assistantMessage.Text += "\n\n[Generation stopped]";
                assistantMessage.IsStreaming = false;
            }
            catch (HttpRequestException ex)
            {
                _error = $"Connection error: {ex.Message}";
                _messages.Remove(assistantMessage);
            }
            catch (Exception ex)
            {
                _error = $"Error: {ex.Message}";
                _messages.Remove(assistantMessage);
            }
            finally
            {
                _isLoading = false;
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
                StateHasChanged();
            }
        }

        private async Task ScrollToBottom()
        {
            if (_jsModule != null)
            {
                try
                {
                    await _jsModule.InvokeVoidAsync("chatHelpers.scrollToBottomSmooth", "messages-container");
                }
                catch { }
            }
        }

        private void StopGeneration() => _cancellationTokenSource?.Cancel();

        private void ClearChat()
        {
            _messages.Clear();
            _error = null;
            StateHasChanged();
        }

        public string GetMessageStyle(bool isUser) =>
            isUser ? "background-color:#1976d2; color:white;" : "background-color:white; color:#333;";

        public async ValueTask DisposeAsync()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();

            if (_jsModule != null)
                await _jsModule.DisposeAsync();
        }

        private class ChatMessage
        {
            public string Text { get; set; } = string.Empty;
            public bool IsUser { get; set; }
            public bool IsStreaming { get; set; }
            public DateTime Timestamp { get; set; }
            public string? Sources { get; set; }
        }

        protected override async Task OnInitializedAsync()
        {
            authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            listing.UserId = authState.User.FindFirst("sub").Value;
            listing.Email = authState.User.FindFirst("email").Value;
        }

        private async void AfterStep1()
        {
            if (listing.ListingType == SD.ListingTypeRent)
            {
                priceLabel = "Τιμή / Μήνα";
            }
            else
            {
                priceLabel = "Τιμή Ακινήτου";
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
                    filenames.Add(image.Name);

                    var resizedImage = await image.RequestImageFileAsync(format, 300, 300);

                    using var stream = image.OpenReadStream();
                    using var memoryStream = new MemoryStream();

                    await stream.CopyToAsync(memoryStream);
                    var buffer = memoryStream.ToArray();

                    var imageData = $"data:{format};base64,{Convert.ToBase64String(buffer)}";

                    Console.WriteLine($"Base64 Hash: {Convert.ToBase64String(buffer).Substring(0, 30)}...");

                    model.Images.Add(new ImageDto {Name=image.Name, Data = imageData });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing {image.Name}: {ex}");
                }
            }
        }

        private void DeleteFile(string file)
        {
            filenames.Remove(file);
            model.Images?.RemoveAll(img => img.Name == file); // Adjust this logic if filenames aren't the same

            StateHasChanged();
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
                var parameters = new DialogParameters<ErrorDialog> { { x => x.errors, errors } };

                var dialog = await DialogService.ShowAsync<ErrorDialog>("Λάθη", parameters);
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
