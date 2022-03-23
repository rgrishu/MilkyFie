﻿
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
        Pincode: param => new Promise((resolve, reject) => {
            $.post('/Master/GetPincodeDrop', param).done(result => resolve(result)).fail(xhr => reject(new Error(xhr)));
        }),
    };


})(services || (services = serviceProperty));



$(function () {
   // $("#loaderbody").addClass('hide');
    //$(document).bind('ajaxStart', function () {
    //    Q.preloader.load();
    //}).bind('ajaxStop', function () {
    //    Q.preloader.remove();
    //});
    $('body').on('submit', 'form', function () {
        ajaxFormSubmit(this)
    })
});






var s = services;
var Dropdown = services.Dropdown;