using System.ComponentModel.DataAnnotations;

namespace NoteApp.API.Model
{
    public class NoteFiles : BaseModel
    {
        [Key]
        public int Id { get; set; }

        public int NoteId { get; set; }
        public string FilePath { get; set; }
    }
}
