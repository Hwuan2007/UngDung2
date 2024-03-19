using Extensions; 
using System;
using Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;


namespace Models
{
    public class NhanVien
    {
        public string? MaNhanVien { get; set; }
        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin.")]
        public string? HoTen { get; set; }
        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin.")]
        [DataType(DataType.Date)]
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
         public void AddNhanVien(NhanVien nhanVien)
        {
            danhSachNhanVien.Add(nhanVien);
        }

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
    var maNhanVienList = session.GetObject<List<string>>("MaNhanVienList") ?? new List<string>();

    if (maNhanVienList.Any())
    {
        string maxId = maNhanVienList.Max();
        string numId = maxId.Substring(3);
        int currentNum = int.Parse(numId);
        int nextNum = currentNum + 1;
        string maNhanVienMoi = "NV-" + nextNum.ToString("0000");

        maNhanVienList.Add(maNhanVienMoi);
        session.SetObject("MaNhanVienList", maNhanVienList);

        return maNhanVienMoi;
    }
    else
    {
        string maNhanVienMoi = "NV-0001";

        maNhanVienList.Add(maNhanVienMoi);
        session.SetObject("MaNhanVienList", maNhanVienList);

        return maNhanVienMoi;
    }
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