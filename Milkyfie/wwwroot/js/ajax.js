$(function () {
    //$("#loaderbody").addClass('hide');
    //$(document).bind('ajaxStart', function () {
    //    $("#loaderbody").removeClass('hide');
    //}).bind('ajaxStop', function () {
    //    $("#loaderbody").addClass('hide');
    //});
    $('body').on('submit', 'form', function () {
        ajaxFormSubmit(this)
    });
});




function ajaxFormSubmit(form) {    
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        var ajaxConfig = {
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            success: function (response) {
                if (response.success) {
                    $("#firstTab").html(response.html);
                    refreshAddNewTab($(form).attr('data-restUrl'), true);
                    $.notify(response.message, "success");
                    if (typeof activatejQueryTable !== 'undefined' && $.isFunction(activatejQueryTable))
                        activatejQueryTable();
                }
                else {
                    $.notify(response.message, "error");
                }
            }
        }
        if ($(form).attr('enctype') == "multipart/form-data") {
            ajaxConfig["contentType"] = false;
            ajaxConfig["processData"] = false;
        }
        $.ajax(ajaxConfig);

    }
    return false;
}