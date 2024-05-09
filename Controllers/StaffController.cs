using hocvieccuccangMVC.DataAccess;
using hocvieccuccangMVC.Extensions;
using hocvieccuccangMVC.Models;
using hocvieccuccangMVC.ViewModels;
using hocvieccuccangMVC.db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using OfficeOpenXml;
using Dapper;
using System.Data;
using System.Data.SqlClient;



namespace hocvieccuccangMVC.Controllers;
public class StaffController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _context;
    private readonly NhanVienDataAccess _nhanVienDataAccess;
    private readonly IDbConnection _dbConnection;

    private List<PhongBan> GetPhongBanList()
    {
        var phongBanList = _context.phong_ban.ToList();
        return phongBanList;
    }

    public StaffController(NhanVienDataAccess nhanVienDataAccess, IHttpContextAccessor httpContextAccessor, IDbConnection dbConnection)
    {
        _httpContextAccessor = httpContextAccessor;
        _nhanVienDataAccess = nhanVienDataAccess;
        _dbConnection = dbConnection;

    }
    public IActionResult Index(int? pb_id, int? page)
    {
        int pageSize = 10;
        int pageNumber = page ?? 1;

        string query = "SELECT n.*, p.ten_phong_ban FROM nhan_vien n LEFT JOIN phong_ban p ON n.phong_ban_id = p.pb_id";
        // Kiểm tra xem pb_id có giá trị không
        if (pb_id.HasValue)
        {
            query += $" WHERE n.phong_ban_id = {pb_id}";
        }
        // Thêm điều kiện sắp xếp theo mã nhân viên tăng dần
        query += " ORDER BY n.nv_id ASC";
        // Thực hiện truy vấn và chuyển kết quả sang danh sách phân trang
        var danhSachNhanVien = _dbConnection.Query<NhanVien, PhongBan, NhanVien>(
            query,
            (nv, pb) =>
            {
                nv.phong_ban = pb;
                return nv;
            },
            splitOn: "ten_phong_ban"
        ).ToPagedList(pageNumber, pageSize);
        // Truy vấn danh sách phòng ban và gán vào ViewBag hoặc ViewModel
        var phongBanList = _dbConnection.Query<PhongBan>("SELECT * FROM phong_ban").ToList();
        ViewBag.PhongBanList = phongBanList;

        var viewModel = new NhanVienViewModel
        {
            DanhSachNhanVien = danhSachNhanVien,
            PageNumber = pageNumber,
            PageCount = danhSachNhanVien.PageCount,
            PhongBanList = phongBanList
        };

        return View(viewModel);
    }

    [HttpGet]
    [Route("Create")]
    public IActionResult Create()
    {
        // Lấy danh sách phòng ban
        var phongBanList = _dbConnection.Query<PhongBan>("SELECT * FROM phong_ban").ToList();
        ViewBag.PhongBanList = phongBanList;
        return View(new NhanVien());
    }


    [HttpPost]
    [Route("Create")]
    public IActionResult Create(NhanVien viewModel)
    {
        if (!ModelState.IsValid)
        {
            // Nếu ModelState không hợp lệ, trả về view với model và danh sách phòng ban
            var phongBanList = _dbConnection.Query<PhongBan>("SELECT * FROM phong_ban").ToList();
            ViewBag.PhongBanList = phongBanList;
            return View(viewModel);
        }

        try
        {
            // Kiểm tra xem phòng ban đã được chọn hay không
            if (viewModel.phong_ban_id == null || viewModel.phong_ban_id == 0)
            {
                ModelState.AddModelError("", "Vui lòng chọn phòng ban.");
                return View(viewModel);
            }

            // Thực hiện thêm mới nhân viên bằng cách thực thi câu lệnh SQL INSERT INTO
            string sql = @"INSERT INTO nhan_vien (ho_ten, ngay_sinh, so_dien_thoai, dia_chi, chuc_vu, so_nam_cong_tac, phong_ban_id) 
                        VALUES (@ho_ten, @ngay_sinh, @so_dien_thoai, @dia_chi, @chuc_vu, @so_nam_cong_tac, @phong_ban_id)";

            // Thực thi câu lệnh SQL với tham số được truyền vào từ đối tượng viewModel
            _dbConnection.Execute(sql, viewModel);

            // Chuyển hướng về trang Index sau khi thêm mới thành công
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Đã xảy ra lỗi khi thêm mới nhân viên: " + ex.Message);
            return View(viewModel);
        }
    }

    [HttpGet]
    [Route("Edit")]
    public IActionResult Edit(int id)
    {
        // Truy vấn thông tin nhân viên cần chỉnh sửa
        var nhanVien = _dbConnection.QueryFirstOrDefault<NhanVien>("SELECT * FROM nhan_vien WHERE nv_id = @Id", new { Id = id });
        if (nhanVien == null)
        {
            return NotFound(); // Trả về trang 404 nếu không tìm thấy nhân viên
        }
        // Lấy danh sách phòng ban
        var phongBanList = _dbConnection.Query<PhongBan>("SELECT * FROM phong_ban").ToList();
        ViewBag.PhongBanList = phongBanList;
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
                // Cập nhật thông tin của nhân viên
                _dbConnection.Execute("UPDATE nhan_vien SET ho_ten = @ho_ten, ngay_sinh = @ngay_sinh, so_dien_thoai = @so_dien_thoai, dia_chi = @dia_chi, chuc_vu = @chuc_vu, so_nam_cong_tac = @so_nam_cong_tac WHERE nv_id = @Id", new
                {
                    ho_ten = updatedNhanVien.ho_ten,
                    ngay_sinh = updatedNhanVien.ngay_sinh,
                    so_dien_thoai = updatedNhanVien.so_dien_thoai,
                    dia_chi = updatedNhanVien.dia_chi,
                    chuc_vu = updatedNhanVien.chuc_vu,
                    so_nam_cong_tac = updatedNhanVien.so_nam_cong_tac,
                    Id = id
                });

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
            // Xóa nhân viên từ cơ sở dữ liệu
            _dbConnection.Execute("DELETE FROM nhan_vien WHERE nv_id = @Id", new { Id = id });

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
        var duplicate = _dbConnection.ExecuteScalar<bool>("SELECT COUNT(*) FROM nhan_vien WHERE ho_ten = @hoTen AND ngay_sinh = @ngaySinh", new { hoTen, ngaySinh });

        return Json(new { exists = duplicate });
    }

    public async Task<IActionResult> ExportToExcel()
    {
        // Truy vấn danh sách nhân viên từ cơ sở dữ liệu
        var employees = await _dbConnection.QueryAsync<NhanVien>("SELECT * FROM nhan_vien");

        // Tạo một package Excel
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Danh Sách Nhân Viên");

            // Định dạng tiêu đề
            worksheet.Cells["A1"].Value = "ID";
            worksheet.Cells["B1"].Value = "Họ và tên";
            worksheet.Cells["C1"].Value = "Ngày sinh";
            worksheet.Cells["D1"].Value = "Số điện thoại";
            worksheet.Cells["E1"].Value = "Địa chỉ";
            worksheet.Cells["F1"].Value = "Chức vụ";
            worksheet.Cells["G1"].Value = "Số năm công tác";

            // Ghi dữ liệu vào các ô tương ứng
            int row = 2;
            foreach (var employee in employees)
            {
                worksheet.Cells[string.Format("A{0}", row)].Value = employee.nv_id;
                worksheet.Cells[string.Format("B{0}", row)].Value = employee.ho_ten;
                worksheet.Cells[string.Format("C{0}", row)].Value = employee.ngay_sinh;
                worksheet.Cells[string.Format("D{0}", row)].Value = employee.so_dien_thoai;
                worksheet.Cells[string.Format("E{0}", row)].Value = employee.dia_chi;
                worksheet.Cells[string.Format("F{0}", row)].Value = employee.chuc_vu;
                worksheet.Cells[string.Format("G{0}", row)].Value = employee.so_nam_cong_tac;
                row++;
            }

            // Tạo file Excel từ package và trả về cho người dùng
            return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "employees.xlsx");
        }
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
