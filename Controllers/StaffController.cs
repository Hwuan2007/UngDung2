using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using hocvieccuccangMVC.Models;
using Models;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Extensions;

namespace hocvieccuccangMVC.Controllers;

public class StaffController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly NhanVienDataAccess _nhanVienDataAccess;

    

    public StaffController(NhanVienDataAccess nhanVienDataAccess, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _nhanVienDataAccess = nhanVienDataAccess;

        var session = _httpContextAccessor.HttpContext.Session;
        var danhSachNhanVien = session.GetObject<List<NhanVien>>("DanhSachNhanVien");
        if (danhSachNhanVien == null)
        {
            danhSachNhanVien = _nhanVienDataAccess.nhanvienngaunhien();
            session.SetObject("DanhSachNhanVien", danhSachNhanVien);
        }
    }
    public IActionResult Index()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var danhSachNhanVien = session.GetObject<List<NhanVien>>("DanhSachNhanVien");
            if (danhSachNhanVien == null || danhSachNhanVien.Count == 0)
            {
                
                return Content("Không có nhân viên nào trong danh sách.");
            }
            return View(danhSachNhanVien);
        }

    [HttpGet]
    [Route("Create")]
    public IActionResult Create()
    {
        return  View();
    }

    [HttpPost]
    [Route("Create")]
    public IActionResult Create(NhanVien model)
    {
        if (ModelState.IsValid)
    {
        model.MaNhanVien = _nhanVienDataAccess.UpdateMaNhanVien(); 

        _nhanVienDataAccess.AddNhanVien(model);

        // Lấy danh sáchnhân viên từ session
        var danhSachNhanVien = HttpContext.Session.GetObject<List<NhanVien>>("DanhSachNhanVien");
        // Kiểm tra xem danh sách có tồn tại không
        if (danhSachNhanVien == null)
        {
            danhSachNhanVien = new List<NhanVien>(); // Khởi tạo danh sách mới nếu chưa tồn tại
        }
        // Thêm nhân viên mới vào danh sách
        danhSachNhanVien.Add(model);
        // Lưu danh sách nhân viên đã cập nhật vào session
        HttpContext.Session.SetObject("DanhSachNhanVien", danhSachNhanVien);

        return RedirectToAction("Index");
    }
    return View(model);
    }


        public IActionResult Edit(int id)
        {
            return Content("Đang xây dựng");
        }

        [HttpPost]
        public IActionResult Update(object model)
        {
            return Content("Đang xây dựng");
        }

        public IActionResult Delete(int id)
        {
            return Content("Đang xây dựng");
        }

        public IActionResult Report()
        {
            return Content("Đang xây dựng");
        }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
