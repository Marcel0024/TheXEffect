﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

if (!(Cookies.get('registernotification'))) {
    Cookies.set('registernotification', 'true', { expires: 365 });

    $.toast({
        heading: 'New',
        text: "You can now register to monitor the same progress on multiple devices!",
        showHideTransition: 'fade',
        hideAfter: 10000,
        position: 'top-left',
        icon: 'info'
    });
} 
