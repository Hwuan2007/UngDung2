$(document).ready(function () {
    $('#HoTen, #NgaySinh').blur(function () {
        var hoTen = $('#HoTen').val();
        var ngaySinh = $('#NgaySinh').val();
        // Gửi yêu cầu kiểm tra đến máy chủ
        $.ajax({
            url: '/Staff/CheckDuplicate', // Chỉnh sửa địa chỉ URL nếu cần thiết
            type: 'GET',
            data: { hoTen: hoTen, ngaySinh: ngaySinh },
            success: function (result) {
                if (result.exists) {
                    // Hiển thị thông báo nếu nhân viên đã tồn tại
                    alert('Nhân viên này đã tồn tại!');
                    $('#HoTen').val(''); // Xóa dữ liệu đã nhập
                    $('#NgaySinh').val(''); // Xóa dữ liệu đã nhập
                }
            }
        });
    });
});