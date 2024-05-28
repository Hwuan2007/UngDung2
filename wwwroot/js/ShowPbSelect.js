$('#addEmployeeModal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget); // Button that triggered the modal
    var currentPhongBanId = $('#phongBan').val(); // Lấy giá trị của phòng ban được chọn từ trang chính
    $('#addEmployeeForm select[name="phong_ban_id"]').val(currentPhongBanId); // Đặt giá trị của dropdown phòng ban trong modal thêm mới
});