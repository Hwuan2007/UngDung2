$(document).ready(function() {
    $('form').submit(function() {
        var isValidForm = true;

        $('input[required]').each(function() {
            if (!$(this).val()) {
                isValidForm = false;
                $(this).next('.error-message').text('Vui lòng điền thông tin vào trường này.').show();
            } else {
                $(this).next('.error-message').hide();
            }
        });

        if (!isValidForm) {
            return false;
        }

        var ngaySinh = $('#NgaySinh').val();
        if (!isValidDate(ngaySinh)) {
            $('#NgaySinh').next('.error-message').text('Định dạng ngày sinh không hợp lệ. Vui lòng nhập lại theo định dạng dd/mm/yyyy.').show();
            return false;
        } else {
            $('#NgaySinh').next('.error-message').hide();
        }
    });

    function isValidDate(dateString) {
        if (!dateString) {
            return false; // Trường input trống
        }

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
});