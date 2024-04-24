using Microsoft.EntityFrameworkCore;
using NoteApp.API.Model;

namespace NoteApp.API
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Notes> Notes { get; set; }
        public DbSet<NoteLogin> NoteLogin { get; set; }
    }
}


