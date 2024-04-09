using hocvieccuccangMVC.Models;
using X.PagedList;
namespace hocvieccuccangMVC.ViewModels
{
    public class NhanVienViewModel
    {
        public IPagedList<NhanVien>? DanhSachNhanVien { get; set; }
        public NhanVien? NhanVien { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
    }
}