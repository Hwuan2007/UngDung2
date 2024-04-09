using hocvieccuccangMVC.Models;
using hocvieccuccangMVC.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace hocvieccuccangMVC.DataAccess
{
    public class NhanVienDataAccess
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public NhanVienDataAccess(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        private Random random = new Random();
        //     private List<NhanVien> danhSachNhanVien = new List<NhanVien>(); // Khai báo danh sách nhân viên

        //     public List<NhanVien> GetNhanViens()
        //     {
        //         return danhSachNhanVien;
        //     }
        //     public List<NhanVien> nhanvienngaunhien()
        //     {
        //         List<NhanVien> danhSachNhanVien = new List<NhanVien>();
        //         for (int i = 0; i < 5; i++)
        //         {
        //             NhanVien nhanVien = new NhanVien();
        //             nhanVien.MaNhanVien = "NV-" + (i + 1).ToString("0000");
        //             nhanVien.HoTen = "Nhân viên " + (i + 1);
        //             nhanVien.NgaySinh = NgaySinh();
        //             nhanVien.SoDienThoai = SoDienThoai();
        //             nhanVien.DiaChi = "Địa chỉ nhân viên " + (i + 1);
        //             nhanVien.ChucVu = "Chức vụ nhân viên " + (i + 1);
        //             nhanVien.SoNamCongTac = random.Next(1, 10);

        //             danhSachNhanVien.Add(nhanVien);
        //         }
        //         return danhSachNhanVien;
        //     }

        //     public string UpdateMaNhanVien()
        //     {
        //         var session = _httpContextAccessor.HttpContext.Session;
        //         var danhSachNhanVien = session.GetObject<List<NhanVien>>("DanhSachNhanVien") ?? new List<NhanVien>();
        //         string maNhanVienMoi;

        //         if (danhSachNhanVien.Any())
        //         {
        //             string maxId = danhSachNhanVien.Select(nv => nv.MaNhanVien).Max();
        //             string numId = maxId.Substring(3);
        //             int currentNum = int.Parse(numId);
        //             int nextNum = currentNum + 1;
        //             maNhanVienMoi = "NV-" + nextNum.ToString("0000");

        //             // Kiểm tra xem mã nhân viên mới đã tồn tại trong danh sách chưa
        //             while (danhSachNhanVien.Any(nv => nv.MaNhanVien == maNhanVienMoi))
        //             {
        //                 // Nếu mã đã tồn tại, tạo mã mới
        //                 currentNum = int.Parse(maNhanVienMoi.Substring(3));
        //                 currentNum++;
        //                 maNhanVienMoi = "NV-" + currentNum.ToString("0000");
        //             }
        //         }
        //         else
        //         {
        //             maNhanVienMoi = "NV-0001";
        //         }

        //         return maNhanVienMoi;
        //     }

        //Tạo giới hạn cho ngày sinh 
        private string NgaySinh()
        {
            DateTime ngayBatDau = DateTime.Now.AddYears(-50);
            int soNgay = (DateTime.Now - ngayBatDau).Days;
            int soNgayNgauNhien = random.Next(soNgay);
            DateTime ngaySinh = ngayBatDau.AddDays(soNgayNgauNhien).Date;
            return ngaySinh.ToString("dd/MM/yyyy");
        }

        //đặt quy luật random sdt
        private string SoDienThoai()
        {
            return "0" + random.Next(100000000, 999999999).ToString();
        }


    }
}
