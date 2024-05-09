$(document).ready(function () {
    $('#ho_ten, #ngay_sinh').blur(function () {
        var ho_ten = $('#ho_ten').val();
        var ngay_sinh = $('#ngay_sinh').val();
        // Gửi yêu cầu kiểm tra đến máy chủ
        $.ajax({
            url: '/Staff/CheckDuplicate', // Chỉnh sửa địa chỉ URL nếu cần thiết
            type: 'GET',
            data: { ho_ten: ho_ten, ngaySinh: ngay_sinh },
            success: function (result) {
                if (result.exists) {
                    // Hiển thị thông báo nếu nhân viên đã tồn tại
                    alert('Nhân viên này đã tồn tại!');
                    $('#ho_ten').val(''); // Xóa dữ liệu đã nhập
                    $('#ngay_sinh').val(''); // Xóa dữ liệu đã nhập
                }
            }
        });
    });
});