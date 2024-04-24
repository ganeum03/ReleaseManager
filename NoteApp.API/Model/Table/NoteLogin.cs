using System.ComponentModel.DataAnnotations;

namespace NoteApp.API.Model
{
    public class NoteLogin
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
