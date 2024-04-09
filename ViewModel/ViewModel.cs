using hocvieccuccangMVC.Models;
namespace hocvieccuccangMVC.ViewModels
{
    public class NhanVienViewModel
    {
        public IEnumerable<NhanVien>? DanhSachNhanVien { get; set; }
        public NhanVien? NhanVien { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
    }
}