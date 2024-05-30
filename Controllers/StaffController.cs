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

    public List<PhongBan> GetPhongBanList()
    {
        string query = "SELECT * FROM phong_ban";
        var phongBanList = _dbConnection.Query<PhongBan>(query).ToList();
        return phongBanList;
    }
    public StaffController(NhanVienDataAccess nhanVienDataAccess, IHttpContextAccessor httpContextAccessor, IDbConnection dbConnection)
    {
        _httpContextAccessor = httpContextAccessor;
        _nhanVienDataAccess = nhanVienDataAccess;
        _dbConnection = dbConnection;

    }
    public IActionResult Index(int? pb_id, int? page, string search)
    {
        int pageSize = 10;
        int pageNumber = page ?? 1;
        int offset = (pageNumber - 1) * pageSize;
        ViewBag.SelectedPhongBan = pb_id; // Cập nhật giá trị của phòng ban được chọn
        ViewBag.SearchTerm = search; // Cập nhật giá trị của từ khóa tìm kiếm

        string query = @"
        SELECT n.*, p.pb_id, p.ten_phong_ban 
        FROM nhan_vien n 
        LEFT JOIN phong_ban p ON n.phong_ban_id = p.pb_id 
        WHERE 1 = 1";

        // Kiểm tra xem có yêu cầu tìm kiếm hay không
        if (!string.IsNullOrEmpty(search))
        {
            string searchLower = search.ToLower();
            query += " AND (LOWER(n.ho_ten) LIKE @Search OR LOWER(n.dia_chi) LIKE @Search)";
        }

        // Kiểm tra pb_id có giá trị không
        if (pb_id.HasValue)
        {
            query += " AND n.phong_ban_id = @pb_id";
        }

        // Thêm điều kiện sắp xếp theo mã nhân viên tăng dần
        query += " ORDER BY n.nv_id ASC";

        // Thêm phân trang bằng OFFSET và FETCH
        query += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        // Thực hiện truy vấn và lấy kết quả
        var danhSachNhanVien = _dbConnection.Query<NhanVien, PhongBan, NhanVien>(
            query,
            (nv, pb) =>
            {
                nv.phong_ban = pb;
                return nv;
            },
            new { Offset = offset, PageSize = pageSize, Search = $"%{search}%", pb_id },
            splitOn: "pb_id"
        ).ToList();

        // Lấy tổng số bản ghi để tính số trang
        string countQuery = @"
        SELECT COUNT(*) 
        FROM nhan_vien n 
        LEFT JOIN phong_ban p ON n.phong_ban_id = p.pb_id 
        WHERE 1 = 1";

        if (!string.IsNullOrEmpty(search))
        {
            countQuery += " AND (LOWER(n.ho_ten) LIKE @Search OR LOWER(n.dia_chi) LIKE @Search)";
        }

        if (pb_id.HasValue)
        {
            countQuery += " AND n.phong_ban_id = @pb_id";
        }

        int totalRecords = _dbConnection.ExecuteScalar<int>(countQuery, new { Search = $"%{search}%", pb_id });
        int pageCount = (int)Math.Ceiling((double)totalRecords / pageSize);

        // Truy vấn danh sách phòng ban và gán vào ViewBag hoặc ViewModel
        var phongBanList = _dbConnection.Query<PhongBan>("SELECT * FROM phong_ban").ToList();
        ViewBag.PhongBanList = phongBanList;

        var viewModel = new NhanVienViewModel
        {
            DanhSachNhanVien = danhSachNhanVien,
            PageNumber = pageNumber,
            PageCount = pageCount,
            PhongBanList = phongBanList
        };

        // Trả về một phản hồi JSON nếu yêu cầu là AJAX
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView("EmployeeTable", viewModel);
        }
        else
        {
            return View(viewModel);
        }
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

            return Redirect(Request.Headers["Referer"].ToString());

        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Đã xảy ra lỗi khi thêm mới nhân viên: " + ex.Message);
            return View(viewModel);
        }
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var nhanVien = _dbConnection.QueryFirstOrDefault<NhanVien>(
        "SELECT * FROM nhan_vien WHERE nv_id = @Id",
            new { Id = id }
        );

        if (nhanVien == null)
        {
            return NotFound(); // Trả về trang 404 nếu không tìm thấy nhân viên
        }

        // Tạo viewModel và truyền dữ liệu vào
        var phongBanList = _dbConnection.Query<PhongBan>("SELECT * FROM phong_ban").ToList();
        ViewBag.PhongBanList = phongBanList;

        return View(nhanVien);
    }

    [HttpPost]
    public IActionResult Edit(int id, NhanVien updatedNhanVien)
    {
        if (!ModelState.IsValid)
        {
            // Nếu ModelState không hợp lệ, trả về view chỉnh sửa với model để người dùng nhập lại
            var phongBanList = _dbConnection.Query<PhongBan>("SELECT * FROM phong_ban").ToList();
            ViewBag.PhongBanList = phongBanList;
            return View(updatedNhanVien);

        }

        try
        {
            // Thực hiện câu lệnh SQL UPDATE để cập nhật thông tin nhân viên
            string sql = @"UPDATE nhan_vien 
                        SET ho_ten = @ho_ten, 
                            ngay_sinh = @ngay_sinh, 
                            so_dien_thoai = @so_dien_thoai, 
                            dia_chi = @dia_chi, 
                            chuc_vu = @chuc_vu, 
                            so_nam_cong_tac = @so_nam_cong_tac, 
                            phong_ban_id = @phong_ban_id 
                        WHERE nv_id = @nv_id";

            // Thực thi câu lệnh SQL với tham số được truyền vào từ đối tượng updatedNhanVien
            _dbConnection.Execute(sql, updatedNhanVien);

            // Redirect người dùng đến trang Index sau khi chỉnh sửa thành công
            return Redirect(Request.Headers["Referer"].ToString());
        }
        catch (Exception ex)
        {
            // Xử lý lỗi nếu có
            ModelState.AddModelError("", "Đã xảy ra lỗi khi cập nhật thông tin nhân viên: " + ex.Message);
            var phongBanList = _dbConnection.Query<PhongBan>("SELECT * FROM phong_ban").ToList();
            ViewBag.PhongBanList = phongBanList;
            return View(updatedNhanVien);
        }
    }

    public IActionResult Delete(int id)
    {
        try
        {
            // Xóa nhân viên từ cơ sở dữ liệu
            _dbConnection.Execute("DELETE FROM nhan_vien WHERE nv_id = @Id", new { Id = id });

            // Redirect người dùng đến trang Index sau khi xóa thành công
            TempData["Message"] = "Nhân viên đã được xóa thành công.";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        catch (Exception ex)
        {
            // Xử lý lỗi nếu có
            TempData["Message"] = "Đã xảy ra lỗi khi xóa nhân viên: " + ex.Message;
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    [HttpGet]
    public IActionResult CheckDuplicate(string ho_ten, string ngay_sinh)
    {
        if (_dbConnection.ExecuteScalar<bool>("SELECT COUNT(*) FROM nhan_vien WHERE ho_ten = @ho_ten AND ngay_sinh = @ngay_sinh", new { ho_ten, ngay_sinh }))
        {
            return Json(new { exists = true });
        }
        else
        {
            return Json(new { exists = false });
        }
    }



    public async Task<IActionResult> ExportToExcel(int? pb_id, string search)
    {
        // Tạo truy vấn cơ bản
        string query = "SELECT n.*, p.ten_phong_ban FROM nhan_vien n LEFT JOIN phong_ban p ON n.phong_ban_id = p.pb_id WHERE 1 = 1";

        // Kiểm tra xem có yêu cầu tìm kiếm hay không
        if (!string.IsNullOrEmpty(search))
        {
            string searchLower = search.ToLower();
            query += $" AND (LOWER(n.ho_ten) LIKE '%{searchLower}%' OR LOWER(n.dia_chi) LIKE '%{searchLower}%')";
        }

        // Kiểm tra pb_id có giá trị không
        if (pb_id.HasValue)
        {
            query += $" AND n.phong_ban_id = {pb_id}";
        }

        // Thêm điều kiện sắp xếp theo mã nhân viên tăng dần
        query += " ORDER BY n.nv_id ASC";

        // Truy vấn danh sách nhân viên từ cơ sở dữ liệu
        var employees = await _dbConnection.QueryAsync<NhanVien, PhongBan, NhanVien>(
            query,
            (nv, pb) =>
            {
                nv.phong_ban = pb;
                return nv;
            },
            splitOn: "ten_phong_ban"
        );



        // Tạo một package Excel
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Danh Sách Nhân Viên");

            // Thêm dòng tiêu đề động
            string title = "Danh Sách Nhân Viên";
            if (pb_id.HasValue || !string.IsNullOrEmpty(search))
            {
                title = "Danh Sách Nhân Viên";
                if (pb_id.HasValue)
                {
                    var selectedPhongBan = (await _dbConnection.QueryAsync<PhongBan>("SELECT * FROM phong_ban WHERE pb_id = @Id", new { Id = pb_id })).FirstOrDefault();
                    if (selectedPhongBan != null)
                    {
                        title += $" theo Phòng Ban: {selectedPhongBan.ten_phong_ban}";
                    }
                }
                if (!string.IsNullOrEmpty(search))
                {
                    title += $" với Từ Khóa: '{search}'";
                }
            }

            worksheet.Cells["A1"].Value = title;
            worksheet.Cells["A1:H1"].Merge = true;
            worksheet.Cells["A1"].Style.Font.Size = 14;
            worksheet.Cells["A1"].Style.Font.Bold = true;
            worksheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            // Tô màu thanh tiêu đề
            worksheet.Cells["A1:H1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells["A1:H1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);


            // Định dạng tiêu đề
            worksheet.Cells["A2"].Value = "ID";
            worksheet.Cells["B2"].Value = "Họ và tên";
            worksheet.Cells["C2"].Value = "Ngày sinh";
            worksheet.Cells["D2"].Value = "Số điện thoại";
            worksheet.Cells["E2"].Value = "Địa chỉ";
            worksheet.Cells["F2"].Value = "Chức vụ";
            worksheet.Cells["G2"].Value = "Số năm công tác";
            worksheet.Cells["H2"].Value = "Phòng ban";

            // Định Dạng về cột
            using (var range = worksheet.Cells["A2:H2"])
            {
                range.Style.Font.Bold = true;
                range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                // Tô màu tiêu đề cột
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            // Ghi dữ liệu vào các ô tương ứng
            int row = 3;
            foreach (var employee in employees)
            {
                worksheet.Cells[string.Format("A{0}", row)].Value = employee.nv_id;
                worksheet.Cells[string.Format("B{0}", row)].Value = employee.ho_ten;
                worksheet.Cells[string.Format("C{0}", row)].Value = employee.ngay_sinh;
                worksheet.Cells[string.Format("D{0}", row)].Value = employee.so_dien_thoai;
                worksheet.Cells[string.Format("E{0}", row)].Value = employee.dia_chi;
                worksheet.Cells[string.Format("F{0}", row)].Value = employee.chuc_vu;
                worksheet.Cells[string.Format("G{0}", row)].Value = employee.so_nam_cong_tac;
                worksheet.Cells[string.Format("H{0}", row)].Value = employee.phong_ban.ten_phong_ban;
                row++;
            }

            // Định dạng khung dữ liệu
            using (var range = worksheet.Cells[string.Format("A2:H{0}", row - 1)])
            {
                range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            }

            // Tự động điều chỉnh độ rộng cột cho khớp dữ liệu
            worksheet.Cells.AutoFitColumns();

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
