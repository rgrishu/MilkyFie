
var services;
var serviceProperty = {
    Add: {},
    Delete: {},
    Change: {},
    Detail: {},
    Dropdown: {}
};

((services) => {

    services.Dropdown = {
        Category: param => new Promise((resolve, reject) => {
            if (!param) {
                param = {
                    CategoryID: 0,
                    CategoryName: '',
                }
            }
            $.post('/Master/GetCategoryDrop', param).done(result => resolve(result)).fail(xhr => reject(new Error(xhr)));
        }),
        Unit: param => new Promise((resolve, reject) => {
            if (!param) {
                param = {
                    UnitID: 0,
                    UnitName: '',
                }
            }
            $.post('/Master/Unit', param).done(result => resolve(result)).fail(xhr => reject(new Error(xhr)));
        }),
        Products: param => new Promise((resolve, reject) => {
            $.post('/Master/GetProductDrop', { categoryid: param }).done(result => resolve(result)).fail(xhr => reject(new Error(xhr)));
        }),
        Frequency: param => new Promise((resolve, reject) => {
            $.post('/Master/GetFrequency', param).done(result => resolve(result)).fail(xhr => reject(new Error(xhr)));
        }),
        Users: param => new Promise((resolve, reject) => {
            $.post('/Account/UsersDropdown', param).done(result => resolve(result)).fail(xhr => reject(new Error(xhr)));
        }),
    };


})(services || (services = serviceProperty));



$(function () {
   // $("#loaderbody").addClass('hide');
    $(document).bind('ajaxStart', function () {
        Q.preloader.load();
    }).bind('ajaxStop', function () {
        Q.preloader.remove();
    });
    $('body').on('submit', 'form', function () {
        ajaxFormSubmit(this)
    })
});




function ajaxFormSubmit(form) {
    event.preventDefault();
    /*$.validator.unobtrusive.parse(form);*/
    /*if ($(form).valid()) {*/
    //let data = new FormData(form);
    let data = $(form).serializeArray()
        var ajaxConfig = {
            type: 'POST',
            url: form.action,
            data: data,
            success: function (response) {
                Q.notify(response.statusCode, response.responseText);
                if (response.statusCode == 1) {
                    $(form).trigger("reset");
                    $("#firstTab").html(response.html);
                   // refreshAddNewTab($(form).attr('data-restUrl'), true);
                    if (typeof loadData !== 'undefined' && $.isFunction(loadData))
                        loadData();
                }
            }
        }
        if ($(form).attr('enctype') == "multipart/form-data") {
            ajaxConfig["contentType"] = false;
            ajaxConfig["processData"] = false;
        }
        $.ajax(ajaxConfig);

    //}
    //return false;
}

var s = services;
var Dropdown = services.Dropdown;