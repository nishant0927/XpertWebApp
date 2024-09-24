$(document).on('click', '#btnLogin', function () {
    
    if (ValidationLogin()) {
        $.ajax({
            url: '/Home/Login',
            type: 'POST',
            data: {
                UserId: $('#txtUserID').val(),
                Pwd: $('#txtPwd').val(),
                code: $("#ddlLocation").val()
            },
            success: function (data) {
                //if (data.ResponseData != "A" && data.ResponseData != "Error") {
                //    document.location.href = "/User/Index?Guid=" + data.ResponseData + "";
                //}
                if (data.ResponseData !="Error") {
                    document.location.href = "/User/Index?Guid=" + data.ResponseData + "";
                }
                //else if (data.ResponseData =="A"){
                //    $('input[type=text]').val('');
                //    $('input[type=password]').val('');
                //    ShowSweet2Message("warning", "Login", "Opp!! Access Denied");
                //}
                else {
                    $('input[type=text]').val('');
                    $('input[type=password]').val('');
                    ShowSweet2Message("warning", "Login", "Opp!! UserName or Password is Invalid!");
                }
            },
            error: function (Rval) {
            }
        });
    }
    

});

function ValidationLogin() {
   
    var status = true;
    if ($("#txtUserID").val() == "") {
        ShowSweet2Message("warning","Login","Please enter User name!");
        $('#txtUserID').focus();
        $('#txtUserID').attr("style", "border-color:red;");
        status = false;
    }
    else if ($("#txtPwd").val() == "") {
        ShowSweet2Message("warning","Login","Please enter Password!");
        $('#txtPwd').focus();
        $('#txtPwd').attr("style", "border-color:red;");
        status = false;
    }
   
    return status;
}
