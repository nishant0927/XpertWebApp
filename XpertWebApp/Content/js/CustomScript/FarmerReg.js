function SearchFun() {
    const searchInput = document.getElementById('searchFarmer');
    const searchableDiv = document.getElementById('searchResult');
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
function SaveMPMasterData() {
    var fileData = new FormData();
    fileData.append("MP_NAME", $("#txtmpname").val());
    fileData.append("Father_Name", $("#txtFatherName").val());
    fileData.append("Add1", $("#txtAddress").val());
    fileData.append("Telphone", $("#txtMobile").val());
    fileData.append("MP_CODE_VLC_UPLOADER", $("#txtmpcode").val());
    fileData.append("PayeeName", $("#ddlDestination").val());
    fileData.append("BankName", $("#txtBankName").val());
    fileData.append("IFCICode", $("#txtIfscCode").val());
    fileData.append("AccountNO", $("#txtAccountNo").val());
    fileData.append("Gender", $("#ddlGender").val());
    fileData.append("InActive", $("#chkInactive").is(":checked") ? 1 : 0);
    fileData.append("AadharNo", $("#txtAadharNo").val());
    fileData.append("MP_Name_Hindi", $("#txtmpnameHindi").val());
    fileData.append("Jan_Aadhar_No", $("#txtJanAadharNo").val());
    fileData.append("CAST_CATEGORY_CODE", $("#ddlcategory").val());
    fileData.append("Age", $("#txtAge").val());
    fileData.append("DISTRICT_Code", $("#ddlDistrict").val());
    fileData.append("Zone_Code", $("#ddlZone").val());
    fileData.append("BLOCK_CODE", $("#ddlBlock").val());
    fileData.append("REVENUE_VILLAGE_CODE", $("#ddlRvillage").val());
    fileData.append("GRAMPANCHAYAT_CODE", $("#ddlGpanchayat").val());
    fileData.append("PANCHAYAT_SAMITI_CODE", $("#ddlPsamiti").val());
    fileData.append("VIDHAN_SABHA_CODE", $("#ddlVidhanSabha").val());
    $.ajax({
        url: '/Farmer/Farmer_SaveData',
        type: 'POST',
        contentType: false, // Not to set any content header  
        processData: false,
        data: fileData,
        success: function (data) {
            debugger;
            if (data.ResponseData.includes("Success")) {
                $('#callaway_popup2').hide();
                ShowSweet2Message("success", "Farmer Registration", "Data Saved Successfully.")
                //alert('Success');
                //window.location.reload();
            }
            else {
                $('#callaway_popup2').hide();

                ShowSweet2Message("error", "Farmer Registration", data.ResponseData.split('"')[3])

            }
        },
        error: function (Rval) {
            $('#callaway_popup2').hide();
            ShowSweet2Message("error", "Farmer Registration", "Something went worng, Try again!")
        }
    });
}