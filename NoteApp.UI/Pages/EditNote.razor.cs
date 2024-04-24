using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace NoteApp.UI.Pages
{
    public partial class EditNote
    {
        [Parameter]
        public string Id { get; set; }
        TblNotes obj = new TblNotes();
        [Inject]
        private HttpClient Http { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var obj1 = await Task.Run(() => Http.GetFromJsonAsync<Notes>("products/" + Convert.ToInt32(Id)));
            obj.Name = obj1.Name;
            obj.Desc = obj1.Desc;
            obj.Id = obj1.Id;
            obj.FileName = obj1.FileName;
        }
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
        protected async void UpdateEmployee()
        {
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
                        Id= Convert.ToInt32(Id),
                        Name = obj.Name,
                        Desc = obj.Desc,
                        FileName = obj.FilePath.Name,
                        Content = memoryStream.ToArray()
                    };
                    var response1 = await Http.PutAsJsonAsync("products/" + Convert.ToInt32(Id), fileUploadModel);
                }
            }
            else
            {
                var fileUploadModel = new TblNotes
                {
                    Id = Convert.ToInt32(Id),
                    Name = obj.Name,
                    Desc = obj.Desc
                };
                var response1 = await Http.PutAsJsonAsync("products/" + Convert.ToInt32(Id), fileUploadModel);
            }
            //await Http.PutAsJsonAsync("products/" + Convert.ToInt32(Id), obj);
            NavigationManager.NavigateTo("/fetchdata");
        }
        void Cancel()
        {
            NavigationManager.NavigateTo("/fetchdata");
        }
    }

}
