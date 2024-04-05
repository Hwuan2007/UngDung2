using hocvieccuccangMVC.DataAccess;
using hocvieccuccangMVC.Extensions;
using hocvieccuccangMVC.Models;
using hocvieccuccangMVC.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

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

        if (danhSachNhanVien == null || !danhSachNhanVien.Any())
        {
            return Content("Không có nhân viên nào trong danh sách.");
        }

        var viewModel = new NhanVienViewModel
        {
            DanhSachNhanVien = danhSachNhanVien
        };

        return View(viewModel);

    }

    [HttpGet]
    [Route("Create")]
    public IActionResult Create()
    {
        return View(new NhanVien());
    }

    [HttpPost]
    [Route("Create")]
    public IActionResult Create(NhanVien model)
    {
        if (ModelState.IsValid)
        {
            // Gán mã nhân viên mới
            model.MaNhanVien = _nhanVienDataAccess.UpdateMaNhanVien();
            // Lấy danh sách nhân viên từ session
            var danhSachNhanVien = HttpContext.Session.GetObject<List<NhanVien>>("DanhSachNhanVien");
            if (danhSachNhanVien == null)
            {
                danhSachNhanVien = new List<NhanVien>();
            }
            danhSachNhanVien.Add(model);
            HttpContext.Session.SetObject("DanhSachNhanVien", danhSachNhanVien);
            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpGet]
    [Route("Edit")]
    public IActionResult Edit(string id)
    {
        var danhSachNhanVien = HttpContext.Session.GetObject<List<NhanVien>>("DanhSachNhanVien");
        var nhanVien = danhSachNhanVien.FirstOrDefault(nv => nv.MaNhanVien == id);
        if (nhanVien == null)
        {
            return NotFound(); // Trả về 404 nếu không tìm thấy nhân viên
        }
        return View(nhanVien);
    }

    [HttpPost]
    [Route("Edit")]
    public IActionResult Edit(string id, NhanVien updatedNhanVien)
    {
        if (ModelState.IsValid)
        {
            var danhSachNhanVien = HttpContext.Session.GetObject<List<NhanVien>>("DanhSachNhanVien");
            var existingNhanVien = danhSachNhanVien.FirstOrDefault(nv => nv.MaNhanVien == id);
            if (existingNhanVien == null)
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy nhân viên
            }
            // Cập nhật thông tin của nhân viên trong danh sách
            existingNhanVien.HoTen = updatedNhanVien.HoTen;
            existingNhanVien.NgaySinh = updatedNhanVien.NgaySinh;
            existingNhanVien.SoDienThoai = updatedNhanVien.SoDienThoai;
            existingNhanVien.DiaChi = updatedNhanVien.DiaChi;
            existingNhanVien.ChucVu = updatedNhanVien.ChucVu;
            existingNhanVien.SoNamCongTac = updatedNhanVien.SoNamCongTac;
            HttpContext.Session.SetObject("DanhSachNhanVien", danhSachNhanVien);
            return RedirectToAction("Index");
        }
        return View(updatedNhanVien);
    }

    public IActionResult Delete(string id)
    {
        var session = _httpContextAccessor.HttpContext.Session;
        var danhSachNhanVien = session.GetObject<List<NhanVien>>("DanhSachNhanVien");
        var nhanVien = danhSachNhanVien.FirstOrDefault(nv => nv.MaNhanVien == id);
        if (nhanVien != null)
        {
            danhSachNhanVien.Remove(nhanVien);
            session.SetObject("DanhSachNhanVien", danhSachNhanVien);
            TempData["Message"] = "Nhân viên đã được xóa thành công.";
        }
        else
        {
            TempData["Message"] = "Không tìm thấy nhân viên cần xóa.";
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult CheckDuplicate(string hoTen, string ngaySinh)
    {
        var danhSachNhanVien = HttpContext.Session.GetObject<List<NhanVien>>("DanhSachNhanVien");
        if (danhSachNhanVien != null)
        {
            bool exists = danhSachNhanVien.Any(nv => nv.HoTen == hoTen && nv.NgaySinh == ngaySinh);
            return Json(new { exists = exists });
        }
        return Json(new { exists = false });
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
