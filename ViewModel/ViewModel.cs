using hocvieccuccangMVC.Models;
using X.PagedList;
namespace hocvieccuccangMVC.ViewModels
{
    public class NhanVienViewModel
    {
        public List<NhanVien> DanhSachNhanVien { get; set; }
        public List<PhongBan> PhongBanList { get; set; }
        public NhanVien? NhanVien { get; set; }
        public PhongBan? PhongBan { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
    }
}