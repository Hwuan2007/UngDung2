using Microsoft.EntityFrameworkCore;
using hocvieccuccangMVC.Models;

namespace hocvieccuccangMVC.db
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<NhanVien> nhan_vien { get; set; }
        public DbSet<PhongBan> phong_ban { get; set; }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);

        //     // Thiết lập mối quan hệ 1-n giữa Phòng ban và Nhân viên
        //     modelBuilder.Entity<NhanVien>()
        //         .HasOne(nv => nv.phong_ban)
        //         .WithMany(pb => pb.nhan_vien)
        //         .HasForeignKey(nv => nv.phong_ban_id)
        //         .OnDelete(DeleteBehavior.Restrict);
        // }

    }
}