function search() {
    var keyword = document.querySelector('#Search input[type="text"]').value.trim().toLowerCase();
    var rows = document.querySelectorAll('.content table tbody tr');

    rows.forEach(function(row) {
        var cells = row.querySelectorAll('td');
        var match = false;

        cells.forEach(function(cell, index) {
            if (index === 1 || index === 4) { // Tìm kiếm theo cột họ tên (index = 1) và địa chỉ (index = 4)
                if (cell.textContent.toLowerCase().indexOf(keyword) > -1) {
                    match = true;
                }
            }
        });

        row.style.display = match ? '' : 'none';
    });
}