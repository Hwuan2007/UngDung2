using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace hocvieccuccangMVC.Models
{
    public class PhongBan
    {
        // public string? MaNhanVien { get; set; }
        [Key]
        public int? pb_id { get; set; }
        public string? ten_phong_ban { get; set; }

        public ICollection<NhanVien> nhan_vien { get; set; }

    }
}
