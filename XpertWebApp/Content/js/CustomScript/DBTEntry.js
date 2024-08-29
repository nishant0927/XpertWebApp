function handler(e) {
    debugger;
    $('#callaway_popup2').show();
    //alert(e.target.value);
    $.ajax({
        url: '/DBT/GetData',
        type: 'POST',
        data: {
            FromDate: $('#txtFromDate').val(),
        },
        success: function (data) {
            debugger;
            if (!data.ResponseData.includes("Error")) {
                $('#callaway_popup2').hide();
                document.location.href = "/DBT/DBTEntry?Guid=" + data.ResponseData + "";
            }
            else {
                //alert("Error:");

                $('#callaway_popup2').hide();
                ShowSweet2Message("error", "DBT Entry", data.ResponseData.split('"')[3])

            }
        },
        error: function (errorThrown) {
            $('#callaway_popup2').hide();
            ShowSweet2Message("error", "DBT Entry", "Something went worng!");
        }
    });
}
function Registerhandler(e) {
    debugger;
    $('#callaway_popup2').show();
    //alert(e.target.value);
    $.ajax({
        url: '/DBT/GetPCycle',
        type: 'POST',
        data: {
            FromDate: $('#txtFromDate').val(),
        },
        success: function (data) {
            debugger;
            if (!data.ResponseData.includes("Error")) {
                $('#callaway_popup2').hide();
                document.location.href = "/DBT/DBTRegister?Guid=" + data.ResponseData + "";
            }
            else {
                //alert("Error:");

                $('#callaway_popup2').hide();
                ShowSweet2Message("error", "DBT Register", data.ResponseData.split('"')[3])
                                
            }
        },
        error: function (errorThrown) {
            $('#callaway_popup2').hide();
            ShowSweet2Message("error", "DBT Register", "Something went worng!");
        }
    });
}
function setFormDate() {
    debugger;
    const fdate = document.getElementById("txtFromDate");
    const formattedDate = $('#hdnfdate').val();
    const parts = formattedDate.split("-");
    const year = parts[2];
    const month = getMonthNumber(parts[1]);
    const day = parts[0];
    const standardDateFormat = `${year}-${month}-${day}`;
    fdate.value = standardDateFormat;
}
function setToDate() {
    const tdate = document.getElementById("txtToDate");
    const formattedDate = $('#hdntdate').val();
    const parts = formattedDate.split("-");
    const year = parts[2];
    const month = getMonthNumber(parts[1]);
    const day = parts[0];
    const standardDateFormat = `${year}-${month}-${day}`;
    tdate.value = standardDateFormat;
}
function getMonthNumber(month) {
    const months = {
        "Jan": "01",
        "Feb": "02",
        "Mar": "03",
        "Apr": "04",
        "May": "05",
        "Jun": "06",
        "Jul": "07",
        "Aug": "08",
        "Sep": "09",
        "Oct": "10",
        "Nov": "11",
        "Dec": "12",
    };
    return months[month];
}
function getValuesAsJson() {
    debugger;
    var rows = document.querySelectorAll('.tp-row');
    var data = []; 
    rows.forEach(function (row) {        
        var item = {}; 
        var cells = row.querySelectorAll('.tp-v'); 
        var inputField = row.querySelector('.input-tp');
        if (cells.length >= 3 && inputField) {
            item.Code = cells[0].innerText;
            item.Farmer = cells[1].innerText;
            item.FatherName = cells[2].innerText;
            item.Qty = inputField.value;
            data.push(item); 
        }
    });
    var jsonString = JSON.stringify(data);
    //console.log(jsonString);
    
    $.ajax({
        type: "POST",
        url: "/DBT/SaveBulkData",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            debugger;
            if (response.res.includes("Success")) {
                $('#callaway_popup2').hide();
                ShowSweet2Message("success", "DBT Entry", "Data Saved Successfully.");
                //location.reload();
            }
            else {
                $('#callaway_popup2').hide();
                ShowSweet2Message("error", "DBT Entry", data.ResponseData.split('"')[3])
            }
        },
        error: function (error) {
            $('#callaway_popup2').hide();
            ShowSweet2Message("error", "DBT Entry", "Something went worng!");

        }
    });
}
function SearchFun() {
    const searchInput = document.getElementById('searchInput');
    const searchableDiv = document.getElementById('searchableDiv');
    searchInput.addEventListener('keyup', function () {
        const searchTerm = searchInput.value.trim().toLowerCase();
        const rows = searchableDiv.getElementsByClassName('tp-r');
        for (let i = 0; i < rows.length; i++) {
            const row = rows[i];
            const elements = row.getElementsByClassName('tp-text');
            let showRow = false;
            for (let j = 0; j < elements.length; j++) {
                const elementText = elements[j].innerText.trim().toLowerCase();
                if (elementText.includes(searchTerm)) {
                    showRow = true;
                    break;
                }
            }
            row.style.display = showRow ? 'flex' : 'none';
        }
    });
}
