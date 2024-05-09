function editEmployee(nv_id, ho_ten, ngay_sinh, so_dien_thoai, dia_chi, chuc_vu, so_nam_cong_tac) {
    // Populate the edit modal fields with employee data
    $('#editEmployeeId').val(nv_id);
    $('#editEmployeeHoTen').val(ho_ten);
    $('#editEmployeeNgaySinh').val(ngay_sinh);
    $('#editEmployeeSoDienThoai').val(so_dien_thoai);
    $('#editEmployeeDiaChi').val(dia_chi);
    $('#editEmployeeChucVu').val(chuc_vu);
    $('#editEmployeeSoNamCongTac').val(so_nam_cong_tac);

    // Show the edit modal
    $('#editEmployeeModal').modal('show');
}