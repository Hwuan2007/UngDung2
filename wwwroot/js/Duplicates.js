document.addEventListener("DOMContentLoaded", function () {
    const addForm = document.getElementById('addEmployeeForm');
    const editForm = document.getElementById('editEmployeeForm');

    function validateAndSubmitForm(form) {
        const hoTenInput = form.querySelector('input[name="ho_ten"]');
        const ngaySinhInput = form.querySelector('input[name="ngay_sinh"]');

        const hoTen = hoTenInput.value.trim();
        const ngaySinh = ngaySinhInput.value.trim();

        if (hoTen === "" || ngaySinh === "") {
            alert("Vui lòng nhập đầy đủ Họ tên và Ngày sinh.");
            return;
        }

        fetch(`/Staff/CheckDuplicate?ho_ten=${encodeURIComponent(hoTen)}&ngay_sinh=${encodeURIComponent(ngaySinh)}`)
            .then(response => response.json())
            .then(data => {
                if (data.exists) {
                    alert('Nhân viên này đã tồn tại!');
                } else {
                    form.submit();
                }
            })
            .catch(error => console.error('Error:', error));
    }

    if (addForm) {
        addForm.addEventListener('submit', function (e) {
            e.preventDefault();
            validateAndSubmitForm(addForm);
        });
    }

    if (editForm) {
        editForm.addEventListener('submit', function (e) {
            e.preventDefault();
            validateAndSubmitForm(editForm);
        });
    }

    // Thêm sự kiện cho các input để xóa thông báo lỗi khi người dùng bắt đầu nhập lại
    addForm.querySelectorAll('input').forEach(input => {
        input.addEventListener('input', function () {
            this.classList.remove('is-invalid');
        });
    });

    editForm.querySelectorAll('input').forEach(input => {
        input.addEventListener('input', function () {
            this.classList.remove('is-invalid');
        });
    });
});
