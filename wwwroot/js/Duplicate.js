document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById('yourFormId');
    const hoTenInput = form.querySelector('input[name="ho_ten"]');
    const ngaySinhInput = form.querySelector('input[name="ngay_sinh"]');

    form.addEventListener('submit', function (e) {
        e.preventDefault();

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
    });
});
