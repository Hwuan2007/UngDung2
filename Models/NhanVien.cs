using System.ComponentModel.DataAnnotations;
using HocViec
namespace hocvieccuccangMVC.Models
{
    public class NhanVien
    {
        public string? MaNhanVien { get; set; }
        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin.")]
        public string? HoTen { get; set; }
        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin.")]
        public string? NgaySinh { get; set; }
        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin.")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? SoDienThoai { get; set; }
        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin.")]
        public string? DiaChi { get; set; }
        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin.")]
        public string? ChucVu { get; set; }
        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số năm công tác phải là một số nguyên dương.")]
        public int SoNamCongTac { get; set; }
    }
}
