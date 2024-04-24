using NoteApp.API.Model;
using System.ComponentModel.DataAnnotations;

namespace NoteApp.API
{
    public class Notes : BaseModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Desc { get; set; }
        public string FilePath { get; set; }

    }
}
