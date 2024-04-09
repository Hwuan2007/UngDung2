using Microsoft.EntityFrameworkCore;
using hocvieccuccangMVC.Models;

namespace hocvieccuccangMVC.db
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<NhanVien> NhanViens { get; set; }
    }
}
