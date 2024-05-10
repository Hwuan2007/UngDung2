// Hàm để chọn option trong dropdown dựa trên giá trị pb_id
function selectPhongBan(pb_id) {
    $('#phongBan').val(pb_id); // Chọn option có giá trị pb_id
}

$(document).ready(function () {
    // Lấy giá trị pb_id từ query string nếu có
    var pb_id = new URLSearchParams(window.location.search).get('pb_id');

    // Nếu pb_id không rỗng, chọn option tương ứng
    if (pb_id !== null) {
        selectPhongBan(pb_id);
    }
});

// Xử lý sự kiện khi thay đổi giá trị trong dropdown
$('#phongBan').change(function () {
    var selectedPbId = $(this).val();
    if (selectedPbId === '') {
        // Nếu giá trị dropdown là rỗng, thực hiện hiển thị tất cả nhân viên
        window.location.href = '/Staff/Index';
    } else {
        // Nếu không, gửi yêu cầu đến server để lấy danh sách nhân viên dựa trên phòng ban được chọn
        window.location.href = '/Staff/Index?pb_id=' + selectedPbId;
    }
});
