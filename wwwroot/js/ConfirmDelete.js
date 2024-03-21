function confirmDelete(id) {
    if (confirm("Bạn có chắc chắn muốn xóa nhân viên này không?")) {
        window.location.href = '/Staff/Delete?id=' + id;
    } else {
        event.preventDefault(); // Ngăn chặn hành động mặc định của thẻ <a>
    }
}