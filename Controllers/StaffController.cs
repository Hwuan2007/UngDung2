using hocvieccuccangMVC.DataAccess;
using hocvieccuccangMVC.Extensions;
using hocvieccuccangMVC.Models;
using hocvieccuccangMVC.ViewModels;
using hocvieccuccangMVC.db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using X.PagedList;


namespace hocvieccuccangMVC.Controllers;
public class StaffController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _context;
    private readonly NhanVienDataAccess _nhanVienDataAccess;
    public StaffController(NhanVienDataAccess nhanVienDataAccess, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _nhanVienDataAccess = nhanVienDataAccess;
        _context = context;
        var session = _httpContextAccessor.HttpContext.Session;

    }
    public IActionResult Index(int? page)
    {
        int pageSize = 10;
        int pageNumber = page ?? 1;

        var danhSachNhanVien = _context.NhanViens.OrderBy(nv => nv.Id).ToPagedList(pageNumber, pageSize);

        var viewModel = new NhanVienViewModel
        {
            DanhSachNhanVien = danhSachNhanVien,
            PageNumber = pageNumber,
            PageCount = danhSachNhanVien.PageCount
        };

        return View(viewModel);
    }
    public IActionResult GetPartialEmployeeList(int? page)
    {
        int pageSize = 10;
        int pageNumber = page ?? 1;

        var danhSachNhanVien = _context.NhanViens.OrderBy(nv => nv.Id).ToPagedList(pageNumber, pageSize);

        return PartialView("_EmployeeListPartial", danhSachNhanVien);
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
            try
            {
                // Thêm mới nhân viên vào cơ sở dữ liệu
                _context.NhanViens.Add(model);
                _context.SaveChanges();

                // Redirect người dùng đến trang Index sau khi thêm mới thành công
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                ModelState.AddModelError("", "Đã xảy ra lỗi khi thêm mới nhân viên: " + ex.Message);
            }
        }

        // Nếu dữ liệu không hợp lệ hoặc có lỗi, trả về view Create với model để người dùng nhập lại
        return View(model);
    }

    [HttpGet]
    [Route("Edit")]
    public IActionResult Edit(int id)
    {

        // Tìm nhân viên cần chỉnh sửa trong cơ sở dữ liệu
        var nhanVien = _context.NhanViens.FirstOrDefault(nv => nv.Id == id);
        if (nhanVien == null)
        {
            return NotFound(); // Trả về trang 404 nếu không tìm thấy nhân viên
        }

        // Trả về view chỉnh sửa với thông tin của nhân viên được tìm thấy
        return View(nhanVien);
    }

    [HttpPost]
    [Route("Edit")]
    public IActionResult Edit(int id, NhanVien updatedNhanVien)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Tìm nhân viên cần chỉnh sửa trong cơ sở dữ liệu
                var existingNhanVien = _context.NhanViens.FirstOrDefault(nv => nv.Id == id);
                if (existingNhanVien == null)
                {
                    return NotFound(); // Trả về trang 404 nếu không tìm thấy nhân viên
                }

                // Cập nhật thông tin của nhân viên từ dữ liệu gửi lên
                existingNhanVien.HoTen = updatedNhanVien.HoTen;
                existingNhanVien.NgaySinh = updatedNhanVien.NgaySinh;
                existingNhanVien.SoDienThoai = updatedNhanVien.SoDienThoai;
                existingNhanVien.DiaChi = updatedNhanVien.DiaChi;
                existingNhanVien.ChucVu = updatedNhanVien.ChucVu;
                existingNhanVien.SoNamCongTac = updatedNhanVien.SoNamCongTac;

                // Lưu các thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();

                // Redirect người dùng đến trang Index sau khi chỉnh sửa thành công
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                ModelState.AddModelError("", "Đã xảy ra lỗi khi cập nhật thông tin nhân viên: " + ex.Message);
            }
        }

        // Nếu dữ liệu không hợp lệ hoặc có lỗi, trả về view Edit với model để người dùng nhập lại
        return View(updatedNhanVien);
    }

    public IActionResult Delete(int id)
    {
        try
        {
            // Tìm nhân viên cần xóa trong cơ sở dữ liệu
            var nhanVienToDelete = _context.NhanViens.FirstOrDefault(nv => nv.Id == id);
            if (nhanVienToDelete == null)
            {
                return NotFound(); // Trả về trang 404 nếu không tìm thấy nhân viên
            }

            // Xóa nhân viên từ cơ sở dữ liệu
            _context.NhanViens.Remove(nhanVienToDelete);
            _context.SaveChanges();

            // Redirect người dùng đến trang Index sau khi xóa thành công
            TempData["Message"] = "Nhân viên đã được xóa thành công.";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            // Xử lý lỗi nếu có
            TempData["Message"] = "Đã xảy ra lỗi khi xóa nhân viên: " + ex.Message;
            return RedirectToAction("Index");
        }
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
