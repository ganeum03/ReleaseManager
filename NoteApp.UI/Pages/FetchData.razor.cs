using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace NoteApp.UI.Pages
{
    public partial class FetchData
    {
        private List<TblNotes> products;

        [Inject]
        private HttpClient Http1 { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        IJSRuntime JSRuntime { get; set; }

        private List<TblNotes> displayedItems;
        private int pageSize = 10;
        private int currentPage = 1;
        private string searchTerm = "";
        protected override async Task OnInitializedAsync()
        {
            products = await Http1.GetFromJsonAsync<List<TblNotes>>("products");
            UpdateDisplayedItems();
        }
        protected async void Delete(int Id)
        {
            await Http1.DeleteAsync("products/" + Id);
            products=new List<TblNotes>();
            StateHasChanged();
            //NavigationManager.NavigateTo("fetchdata");
            products =await Http1.GetFromJsonAsync<List<TblNotes>>("products");
            UpdateDisplayedItems();
            StateHasChanged();
        }
        protected async void DownloadFile(int Id,string fileName)
        {
            //await Http1.DeleteAsync("products/" + Id);
            var response = await Http1.GetAsync($"products/DownloadFile/" +Id);
            if (response.IsSuccessStatusCode)
            {
                //var content = await response.Content.ReadAsByteArrayAsync();
                //await JSRuntime.InvokeAsync<object>("saveAsFile", fileName, Convert.ToBase64String(content));
                byte[] fileContent = await response.Content.ReadAsByteArrayAsync();
                var contentType = response.Content.Headers.ContentType?.MediaType;

                // Create a Blob and download it
                //var file = new Blob(fileContent, "application/octet-stream");
                //await BlazorDownloadFile(file, fileName, "application/octet-stream");
                await BlazorDownloadFile(fileName, contentType, fileContent);

            }
            else
            {
                // Handle error
            }
        }
        private async Task BlazorDownloadFile(string fileName, string contentType, byte[] file)
        {
            await JSRuntime.InvokeAsync<object>("BlazorDownloadFile", fileName, contentType, file);
        }
        //private async Task BlazorDownloadFile(Blob file, string fileName, string contentType)
        //{
        //    await JSRuntime.InvokeAsync<object>("BlazorDownloadFile", fileName, contentType, file);
        //}
        private void UpdateDisplayedItems()
        {
            // Filter based on search term
            var filteredItems = string.IsNullOrEmpty(searchTerm)
                ? products
                : products.Where(item => item.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            // Paginate
            displayedItems = filteredItems.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        private void Search()
        {
            currentPage = 1; // Reset to the first page when searching
            UpdateDisplayedItems();
        }

        private void NextPage()
        {
            currentPage++;
            UpdateDisplayedItems();
        }

        private void PreviousPage()
        {
            currentPage--;
            UpdateDisplayedItems();
        }

        private bool IsFirstPage => currentPage == 1;
        private bool IsLastPage => currentPage == totalPages;
        private int totalPages => (int)Math.Ceiling((double)products.Count / pageSize);

    }

    public class TblNotes : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string FileName { get; set; }
        //public IFileListEntry FilePath { get; set; }
        public IBrowserFile FilePath { get; set; }
        public byte[] Content { get; set; }


    }
    public class BaseModel
    {
        public bool? Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
    public class Notes : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        

        //public IFileListEntry FilePath { get; set; }
    }
}
