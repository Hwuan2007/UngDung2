document.getElementById("yourFormId").addEventListener("submit", function (event) {
    var ngaySinh = document.getElementById("ngay_sinh").value;
    var soDienThoai = document.getElementById("so_dien_thoai").value;

    if (!isValidDate(ngaySinh)) {
        alert("Vui lòng nhập đúng định dạng ngày tháng (dd/mm/yyyy)");
        event.preventDefault(); // Ngăn chặn gửi form
        return;
    }

    if (!isValidPhoneNumber(soDienThoai)) {
        alert("Vui lòng nhập số điện thoại hợp lệ (10 hoặc 11 chữ số)");
        event.preventDefault(); // Ngăn chặn gửi form
        return;
    }

    // Nếu dữ liệu hợp lệ, tiếp tục gửi form
});

// Validate định dạng ngày tháng
function isValidDate(dateString) {
    // Định dạng ngày tháng (dd/mm/yyyy)
    var regex = /^\d{2}\/\d{2}\/\d{4}$/;
    if (!regex.test(dateString)) {
        return false;
    }
    var parts = dateString.split("/");
    var day = parseInt(parts[0], 10);
    var month = parseInt(parts[1], 10);
    var year = parseInt(parts[2], 10);
    if (year < 1000 || year > 3000 || month == 0 || month > 12) {
        return false;
    }
    var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    if (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0)) {
        monthLength[1] = 29; // Năm nhuận
    }
    return day > 0 && day <= monthLength[month - 1];
}

// Validate số điện thoại
function isValidPhoneNumber(phoneNumber) {
    var regex = /^\d{10,11}$/; // Số điện thoại gồm 10 hoặc 11 chữ số
    return regex.test(phoneNumber);
}
