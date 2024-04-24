using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.IO;
using System.Linq;

namespace NoteApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly AppDbContext ApplicationContex;
        public ProductsController(AppDbContext _ApplicationContex)
        {
            ApplicationContex = _ApplicationContex;
        }
        [HttpGet]
        public IActionResult GetProducts()
        {
            string sPath=Path.Combine(Directory.GetCurrentDirectory(), "UploadFiles\\");
            var t = (from A in ApplicationContex.Notes select new { Desc = A.Desc, Name = A.Name, Id = A.Id,FileName=A.FilePath }).ToList();
            Response.ContentType = "application/json";
            return Ok(t);
            //return Ok(products);
        }
        [HttpPost]
        public IActionResult Post([FromBody] FormDataModel data)
        {

            //ApplicationContex.Notes.Add(data);
            //if (File.Exists(data.FileName))
            //{
            //    File.Delete(filePath);
            //}
            DateTime currentDateTime = DateTime.Now;
            Notes obj = new Notes();
            obj.Name = data.Name;
            obj.Desc = data.Desc;
            if (!String.IsNullOrEmpty(data.FileName))
            {
                // Write the byte array to the file
                string spath = Path.Combine(Directory.GetCurrentDirectory(), "UploadFiles\\" + currentDateTime.ToString("ddMMyyyhhmmss") + "_" + data.FileName);
                obj.FilePath = currentDateTime.ToString("ddMMyyyhhmmss") + "_" + data.FileName;
                System.IO.File.WriteAllBytes(spath, data.Content);
            }
            ApplicationContex.Notes.Add(obj);
            ApplicationContex.SaveChanges();
            return Ok(data);
        }
        //[HttpPost]
        //public IActionResult Post([FromForm] FormDataModel formData)
        //{
        //    //ApplicationContex.Notes.Add(data);
        //    //  ApplicationContex.SaveChanges();
        //    if (formData.FilePath != null && formData.FilePath.Length > 0)
        //    {
        //        // Example: Save the file to a server
        //        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", formData.FilePath.FileName);
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            formData.FilePath.CopyToAsync(stream);
        //        }
        //        return Ok("Form submitted successfully");
        //    }
        //    return Ok(formData);
        //}
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var itemToDelete = ApplicationContex.Notes.FirstOrDefault(item => item.Id == id);
            ApplicationContex.Notes.Remove(itemToDelete);
            ApplicationContex.SaveChanges();
            return NoContent();
        }
        [HttpGet("{id}")]
        public ActionResult<Notes> GetById(int id)
        {
            var entity = ApplicationContex.Notes.Where(e => e.Id == id).Select(A=> new { Desc = A.Desc, Name = A.Name, Id = A.Id, FileName = A.FilePath }).FirstOrDefault();

            if (entity == null)
            {
                return NotFound(); // Return 404 Not Found if the entity with the given ID is not found
            }

            return Ok(entity);
        }
        [HttpGet("DownloadFile/{id}")]
        public IActionResult DownloadFile(int id)
        {
            var entity = ApplicationContex.Notes.Where(e => e.Id == id).FirstOrDefault();

            if (entity == null)
            {
                return NotFound(); // Return 404 Not Found if the entity with the given ID is not found
            }
            // Assuming files are stored in a folder named "Uploads"
            var filePath = Path.Combine("UploadFiles\\", entity.FilePath);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); // Or return a custom response for file not found
            }

            // Read the file into a byte array
            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            // Set the content type based on the file extension
            var contentType = GetContentType(entity.FilePath);

            // Return the file with appropriate headers
            return File(fileBytes, contentType, entity.FilePath);
        }

        private string GetContentType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] FormDataModel data)
        {
            var existingEntity = ApplicationContex.Notes.Where(e => e.Id == id).FirstOrDefault();

            if (existingEntity == null)
            {
                return NotFound(); // Return 404 Not Found if the entity with the given ID is not found
            }
            DateTime currentDateTime = DateTime.Now;
            existingEntity.Name = data.Name;
            existingEntity.Desc = data.Desc;
            if (!String.IsNullOrEmpty(data.FileName))
            {
                // Write the byte array to the file
                string spath = Path.Combine(Directory.GetCurrentDirectory(), "UploadFiles\\" + currentDateTime.ToString("ddMMyyyhhmmss") + "_" + data.FileName);
                existingEntity.FilePath = currentDateTime.ToString("ddMMyyyhhmmss") + "_" + data.FileName;
                System.IO.File.WriteAllBytes(spath, data.Content);
            }
            

            ApplicationContex.Notes.Update(existingEntity);
            ApplicationContex.SaveChanges();
            return Ok(existingEntity);
        }
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
        }
        public class FormDataModel
        {
            public int Id { get; set; }

            public string Name { get; set; }
            public string Desc { get; set; }
            public string FileName { get; set; }
            public IFormFile FilePath { get; set; }
            public byte[] Content { get; set; }


        }
    }
}
