using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace hocvieccuccangMVC.Models
{
    public class NhanVien
    {
        [Key]
        public int? nv_id { get; set; }
        public string? ho_ten { get; set; }
        public string? ngay_sinh { get; set; }
        public string? so_dien_thoai { get; set; }
        public string? dia_chi { get; set; }
        public string? chuc_vu { get; set; }
        public int? so_nam_cong_tac { get; set; }

        [ForeignKey("phong_ban_id")] // Sử dụng tên của cột khóa ngoại
        public int? phong_ban_id { get; set; }
        public PhongBan phong_ban { get; set; }

    }
}
