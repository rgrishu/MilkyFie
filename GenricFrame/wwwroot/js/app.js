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
    
    };
   
   
})(services || (services = serviceProperty));

var s = services;
var Dropdown = services.Dropdown;