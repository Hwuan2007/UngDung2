function confirmDelete(nv_id) {
    if (confirm("Bạn có chắc chắn muốn xóa nhân viên này không?")) {
        window.location.href = '/Staff/Delete?nv_id=' + nv_id;
    } else {
        event.preventDefault(); // Ngăn chặn hành động mặc định của thẻ <a>
    }
}