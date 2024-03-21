using Extensions; 
using System;
using Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace Models
{
    public class NhanVien
    {
        public string? MaNhanVien { get; set; }
        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin.")]
        public string? HoTen { get; set; }
        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin.")]
        [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$", ErrorMessage = "Định dạng ngày không hợp lệ. Vui lòng nhập theo định dạng dd/MM/yyyy.")]
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
namespace DataAccess
{


    public class NhanVienDataAccess
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public NhanVienDataAccess(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        private Random random = new Random();
         private List<NhanVien> danhSachNhanVien = new List<NhanVien>(); // Khai báo danh sách nhân viên
         
        public List<NhanVien> GetNhanViens()
        {
            return danhSachNhanVien;
        }
        public List<NhanVien> nhanvienngaunhien()
        {
            List<NhanVien> danhSachNhanVien = new List<NhanVien>();
            for (int i = 0; i < 5; i++)
            {
                NhanVien nhanVien = new NhanVien();
                nhanVien.MaNhanVien = "NV-" + (i + 1).ToString("0000"); 
                nhanVien.HoTen = "Nhân viên " + (i + 1);
                nhanVien.NgaySinh = NgaySinh();
                nhanVien.SoDienThoai = SoDienThoai();
                nhanVien.DiaChi = "Địa chỉ nhân viên " + (i + 1);
                nhanVien.ChucVu = "Chức vụ nhân viên " + (i + 1);
                nhanVien.SoNamCongTac = random.Next(1, 10);  

                danhSachNhanVien.Add(nhanVien);
            }
            return danhSachNhanVien;
        }

public string UpdateMaNhanVien()
{
    var session = _httpContextAccessor.HttpContext.Session;
    var danhSachNhanVien = session.GetObject<List<NhanVien>>("DanhSachNhanVien") ?? new List<NhanVien>();
    string maNhanVienMoi;

    if (danhSachNhanVien.Any())
    {
        string maxId = danhSachNhanVien.Select(nv => nv.MaNhanVien).Max();
        string numId = maxId.Substring(3);
        int currentNum = int.Parse(numId);
        int nextNum = currentNum + 1;
        maNhanVienMoi = "NV-" + nextNum.ToString("0000");

        // Kiểm tra xem mã nhân viên mới đã tồn tại trong danh sách chưa
        while (danhSachNhanVien.Any(nv => nv.MaNhanVien == maNhanVienMoi))
        {
            // Nếu mã đã tồn tại, tạo mã mới
            currentNum = int.Parse(maNhanVienMoi.Substring(3));
            currentNum++;
            maNhanVienMoi = "NV-" + currentNum.ToString("0000");
        }
    }
    else
    {
        maNhanVienMoi = "NV-0001";
    }

    return maNhanVienMoi;
}




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
namespace Extensions
{
    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}