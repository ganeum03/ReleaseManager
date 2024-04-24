using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace NoteApp.UI.Pages
{
    public partial class AddNote
    {
        TblNotes obj = new TblNotes();
        [Inject]
        private HttpClient Http { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        //[Inject]
        public ElementReference fileInputRef { get; set; }

        private string fileContent;
        
        private async Task HandleFileChange(InputFileChangeEventArgs e)
        {
            var file = e.File;
            obj.FilePath = file;
            //using (var stream = file.OpenReadStream())
            //using (var reader = new StreamReader(stream))
            //{
            //    fileContent = await reader.ReadToEndAsync();
            //}
        }
        protected async void CreateEmployee()
        {
            // Get files from InputFile using JavaScript interop
            //var files = await JSRuntime.InvokeAsync<IJSObjectReference>("getFileInputFiles", fileInputRef);
            //var files = await JSRuntime.InvokeAsync<IJSObjectReference>("getFileInputFiles", DotNetObjectReference.Create(fileInputRef));


            // Process files as needed
            //foreach (var file in await files.InvokeAsync<IFileList>("files"))
            //{
            //    // Access file properties
            //    Console.WriteLine($"File Name: {file.Name}, Size: {file.Size} bytes");
            //}
            if (obj.FilePath != null)
            {
                // Read the file content
                using (var stream = obj.FilePath.OpenReadStream())
                {
                    var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);

                    // Create a FileUploadModel
                    var fileUploadModel = new TblNotes
                    {
                        Name=obj.Name,
                        Desc=obj.Desc,
                        FileName = obj.FilePath.Name,
                        Content = memoryStream.ToArray()
                    };
                    var response1 = await Http.PostAsJsonAsync("products", fileUploadModel);
                }
            }
            else
            {
                var fileUploadModel = new TblNotes
                {
                    Name = obj.Name,
                    Desc = obj.Desc
                };
                var response1 = await Http.PostAsJsonAsync("products", fileUploadModel);
            }
            //        var content = new MultipartFormDataContent();
            //content.Add(new StringContent(obj.Name), "textValue");
            //content.Add(new StreamContent(obj.FilePath.OpenReadStream()), "file", obj.FilePath.Name);
            //var response = await Http.PostAsync("products", content);

            //await Http.PostAsJsonAsync("products", obj);
            NavigationManager.NavigateTo("fetchdata");
        }
        // JavaScript interop function to get files from InputFile
        private static string GetFilesFunction => @"
        window.getFileInputFiles = function(element) {
            return {
                files: function() {
                    return element.files;
                }
            };
        };
    ";
        public void Cancel()
        {
            NavigationManager.NavigateTo("fetchdata");
        }

    }

}
