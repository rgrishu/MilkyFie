////var dialog;
////((dialog) => {
////    dialog.bankSetting = function (id = 0) {
////        $.post('/Bank/EditSetting', { id: id }).done(result => {
////            Q.alert({
////                title: "Bank Setting",
////                body: result,
////                width: '900px',
////            });
////        }).fail(xhr => Q.renderError(xhr)).always(() => Q.preloader.remove());
////    };
////    dialog.user = function (id = 0) {
////        Q.preloader.load();
////        $.post('/User/Edit', { id: id, role: 'User' }).done(result => {
////            Q.alert({
////                title: "Add User",
////                body: result,
////                width: '900px',
////            });
////        }).fail(xhr => Q.renderError(xhr)).always(() => Q.preloader.remove());
////    };
////})(dialog || (dialog = {}));


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
            $.post('/Users/UsersDropdown', { role: param }).done(result => resolve(result)).fail(xhr => reject(new Error(xhr)));
        }),
        UsersForFosMap: param => new Promise((resolve, reject) => {
            $.post('/Users/UsersDropdownForFosMap', { role: param }).done(result => resolve(result)).fail(xhr => reject(new Error(xhr)));
        }),
        Pincode: param => new Promise((resolve, reject) => {
            $.post('/Master/GetPincodeDrop', param).done(result => resolve(result)).fail(xhr => reject(new Error(xhr)));
        }),
    };


})(services || (services = serviceProperty));



$(function () {
    $("#loaderbody").addClass('hide');
    $(document).bind('ajaxStart', function () {
        Pace.start();
       // Q.preloader.load();
    }).bind('ajaxStop', function () {
        Pace.stop();
      //  Q.preloader.remove();
    });
    $('body').on('submit', 'form', function () {
        ajaxFormSubmit(this)
    })
});






var s = services;
var Dropdown = services.Dropdown;