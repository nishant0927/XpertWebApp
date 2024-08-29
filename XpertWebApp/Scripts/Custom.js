$('#AddModal').on('shown.bs.modal', function () {
    // Calculate the width of the scrollbar
    var scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;

    // Adjust the width of the table to accommodate the scrollbar
    $('.table-fixed').css('width', 'calc(100% - ' + scrollbarWidth + 'px)');
});


function AddDataModal(modalId, title, header, data, onAnchorClick) {
    var modalTitle = document.getElementById('tblModalTitle');
    modalTitle.innerText = title;

    // Update table header
    var modalHeaderRow = document.getElementById('tblheadermodal');
    modalHeaderRow.innerHTML = header.map(function (columnName) {
        return '<th>' + columnName + '<br><input type="text" class="column-search-input"></th>';
    }).join('');

    // Build table rows
    var rowsHtml = data.map(function (row) {
        return '<tr>' + Object.values(row).map(function (cellValue, index) {
            return '<td>' + cellValue + '</td>';
        }).join('') + '</tr>';
    }).join('');

    var modalBody = document.getElementById('tbodymodal');
    modalBody.innerHTML = rowsHtml;

    // Event delegation for search inputs
    modalHeaderRow.querySelectorAll('.column-search-input').forEach(function (input, columnIndex) {
        input.addEventListener('input', function () {
            var searchText = this.value.trim().toLowerCase();
            var rows = modalBody.querySelectorAll('tr');

            rows.forEach(function (row) {
                var cellText = row.children[columnIndex].textContent.trim().toLowerCase();
                row.style.display = cellText.includes(searchText) ? '' : 'none';
            });
        });
    });

    var rowCells = document.querySelectorAll('.modal-content .table tbody tr');
    rowCells.forEach(function (anchor) {
        anchor.addEventListener('click', function () {
            console.log("Anchor clicked!");

            // Retrieve all values from the row
            var rowValues = [];
            var rowCells = this.querySelectorAll('td');
            for (var i = 0; i < rowCells.length; i++) {
                rowValues.push(rowCells[i].textContent);
            }

            // Execute the custom logic when an anchor is clicked
            onAnchorClick(rowValues);

            // Close the modal if needed
            $('#' + modalId).modal('hide');
        });
    });
    $('#' + modalId).modal('show');
}
function formatDate(dateString) {
    // Extract the milliseconds from the date string
    /* var milliseconds = parseInt(dateString.replace(/[^0-9]/g, ''));*/

    if (dateString == undefined || !dateString.match(/\d+/)) {
        return console.log("Not Definde", dateString);
    }
    var ticks = parseInt(dateString.match(/\d+/)[0]);

    // Convert ticks to milliseconds
    var milliseconds = ticks - (new Date(1970, 0, 1)).getTimezoneOffset() * 60000;
    // Create a new Date object using the milliseconds
    var date = new Date(milliseconds);

    // Define month names
    var monthNames = [
        "Jan", "Feb", "Mar",
        "Apr", "May", "Jun",
        "Jul", "Aug", "Sep",
        "Oct", "Nov", "Dec"
    ];

    // Format the date as dd-Mon-yyyy
    var day = date.getDate().toString().padStart(2, '0');
    var monthIndex = date.getMonth();
    var month = monthNames[monthIndex];
    var year = date.getFullYear();

    return day + '-' + month + '-' + year;

   
}
