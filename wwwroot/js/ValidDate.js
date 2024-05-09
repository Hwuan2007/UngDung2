$(document).ready(function() {
    $('form').submit(function() {
        var isValidForm = true;

        $('input[required]').each(function() {
            if (!$(this).val()) {
                isValidForm = false;
                $(this).next('.text-danger').text('Vui lòng điền đầy đủ thông tin!!!.').show();
            } else {
                $(this).next('.text-danger').hide();
            }
        });

        if (!isValidForm) {
            return false;
        }

        var ngay_sinh = $('#ngay_sinh').val();
        if (!isValidDate(ngay_sinh)) {
            $('#ngay_sinh').next('.text-danger').text('Định dạng ngày sinh không hợp lệ. Vui lòng nhập lại theo định dạng dd/mm/yyyy.').show();
            return false;
        } else {
            $('#ngay_sinh').next('.text-danger').hide();
        }

        var so_dien_thoai = $('#so_dien_thoai').val();
        if (!isValidPhoneNumber(so_dien_thoai)) {
            $('#so_dien_thoai').next('.text-danger').text('Số điện thoại không hợp lệ.').show();
            return false;
        } else {
            $('#so_dien_thoai').next('.text-danger').hide();
        }
    });

    function isValidDate(dateString) {
        var pattern = /^\d{1,2}\/\d{1,2}\/\d{4}$/;
        if (!pattern.test(dateString)) {
            return false;
        }

        var parts = dateString.split('/');
        var day = parseInt(parts[0], 10);
        var month = parseInt(parts[1], 10);
        var year = parseInt(parts[2], 10);

        if (year < 1900 || year > (new Date()).getFullYear()) {
            return false;
        }
        if (month < 1 || month > 12) {
            return false;
        }
        var maxDays = (new Date(year, month, 0)).getDate();
        if (day < 1 || day > maxDays) {
            return false;
        }

        return true;
    }

    function isValidPhoneNumber(phoneNumber) {
        var pattern = /^\d{10,11}$/;
        return pattern.test(phoneNumber);
    }
});