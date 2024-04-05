function editEmployee(MaNhanVien, HoTen, NgaySinh, SoDienThoai, DiaChi, ChucVu, SoNamCongTac) {
    // Populate the edit modal fields with employee data
    $('#editEmployeeId').val(MaNhanVien);
    $('#editEmployeeHoTen').val(HoTen);
    $('#editEmployeeNgaySinh').val(NgaySinh);
    $('#editEmployeeSoDienThoai').val(SoDienThoai);
    $('#editEmployeeDiaChi').val(DiaChi);
    $('#editEmployeeChucVu').val(ChucVu);
    $('#editEmployeeSoNamCongTac').val(SoNamCongTac);
    

    // Show the edit modal
    $('#editEmployeeModal').modal('show');
}