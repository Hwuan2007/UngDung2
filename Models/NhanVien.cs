using System.ComponentModel.DataAnnotations;
namespace hocvieccuccangMVC.Models
{
    public class NhanVien
    {
        public string? MaNhanVien { get; set; }
        public string? HoTen { get; set; }
        public string? NgaySinh { get; set; }
        public string? SoDienThoai { get; set; }
        public string? DiaChi { get; set; }
        public string? ChucVu { get; set; }
        // [Range(1, int.MaxValue, ErrorMessage = "Số năm công tác phải là một số nguyên dương.")]
        public int SoNamCongTac { get; set; }
    }
}
