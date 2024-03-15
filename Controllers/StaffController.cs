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

    }

    

    public IActionResult Index()
        {
            var danhSachNhanVien = _nhanVienDataAccess.nhanvienngaunhien(); // Sửa tên phương thức
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
               return Content("Đang xây dựng");
            }
            return View();
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
