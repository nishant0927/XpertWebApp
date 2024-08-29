function ShowSweet2Message(Type, Title, Message) {
   
    if (Type == "success") { swal.fire({ html: Message, title: Title, icon: Type }); }
    if (Type == "info") { swal.fire({ html: Message, title: Title, icon: Type }); }
    if (Type == "warning") { swal.fire({ html: Message, title: Title, icon: Type }); }
    if (Type == "error") { swal.fire({ html: Message, title: Title, icon: Type }); }
    if (Type == "question") { swal.fire({ html: Message, title: Title, icon: Type }); }
}

function ShowSweetAlertMessage(Type, Title, Message, RedirectUrl) {
    debugger;
    if (Type == "success") { swal.fire({ html: Message, title: Title, icon: Type }).then(function () { window.location.href = RedirectUrl }); }
    if (Type == "info") { swal.fire({ html: Message, title: Title, icon: Type }).then(function () { window.location.href = RedirectUrl }); }
    if (Type == "warning") { swal.fire({ html: Message, title: Title, icon: Type }).then(function () { window.location.href = RedirectUrl }); }
    if (Type == "error") { swal.fire({ html: Message, title: Title, icon: Type }).then(function () { window.location.href = RedirectUrl }); }
    if (Type == "question") {
        swal.fire({ html: Message, title: Title, icon: Type }).then(function () { window.location.href = RedirectUrl });
    }
}


/*--------------------------------------------------------------------VALIDATION--------------------------------------------------------------------*/
function isNumberKeyWithStarSignup(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode
    if ((charCode > 33 && charCode < 65) || (charCode > 90 && charCode < 97) || (charCode > 122 && charCode < 127) || charCode == 31)
        return false;
}
function isNumberKeySignup(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 47 && charCode < 58) {
        return true;
    }
    else {
        return false;
    }
}

function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 47 && charCode < 58) {
        return true;
    }
    else {
        return false;
    }
}
function isNumberKeyWithDecimal(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode
    if ((charCode > 47 && charCode < 58) || charCode == 46) {
        return true;
    }
    else {
        return false;
    }
}

function isAlphaKey(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 64 && charCode < 123) {
        return true;
    }
    else {
        return false;
    }
}
function isAlphaKeyWithSpace(evt) {
    debugger
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 64 && charCode < 123 || charCode == 32) {
        return true;
    }
    else {
        return false;
    }
}
/*--------------------------------------------------------------END - VALIDATION--------------------------------------------------------------------*/