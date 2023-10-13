using FileWithFakeExtensions.Models;
using Microsoft.EntityFrameworkCore;

namespace FileWithFakeExtensions.Data
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options)
        {

        }

        public DbSet<UploadedFile> UploadedFiles { get; set; }
    }
}
