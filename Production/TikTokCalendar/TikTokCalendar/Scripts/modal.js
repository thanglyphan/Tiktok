﻿function showLogin() {

    var username = "";

    vex.dialog.open({
        message: 'Westerdals brukernavn',
        input: "<input name=\"username\" type=\"text\" placeholder=\"nelwil14\" required />",

        showCloseButton: true,
        buttons: [
        $.extend({}, vex.dialog.buttons.YES, {
            text: 'Login'
        })
        ],

        callback: function (data) {
            $.ajax({
                url: "Home/UserName",
                type: "POST",
                data: { a: data.username },
                error: function () {
                    console.log("error");
                },
                success: function (a) {
                    console.log("success" + a);
                }
            })
            //$("#user-name").html(data.username);
            if (data === false) {
                return console.log('Cancelled');
            }


            if (isEmpty(username)) {

                //  Check if username matches any of the registered users
                //  userName === "whatever"

                //  If this is a new user
                vex.dialog.open({

                    message: "Velg ditt studieprogram",
                    buttons: [

                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: 'Bachelor IT', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = 'Programmering';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),

                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: 'Programmering', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = 'Programmering';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),
                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: 'Intelligente Systemer', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = 'IntelligenteSystemer';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),
                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: 'Spilldesign', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = 'Spilldesign';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),
                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: 'Spillprogrammering', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = 'Spillprogrammering';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),
                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: '3D Grafikk', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = '3DGrafikk';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),
                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: 'Interaktivt Design', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = 'InteraktivtDesign';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),
                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: 'E-Business', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = 'EBusiness';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),
                    ],
                    callback: function (value) {
                        // value will be one of the following:
                        course = value;
                        $.ajax({
                            url: "Home/UserCourse",
                            type: "POST",
                            data: { a: value },
                            error: function () {
                                console.log("error");
                            },
                            success: function (a) {
                                console.log("success" + a);
                            }
                        })

                        location.reload();
                    }
                })
            }
        }
    });
}

//  Check if username is invalid in some ways 
//  TODO: Replace this with a to
function isEmpty(str) {
    return typeof str == 'string' && !str.trim() || typeof str == 'undefined' || str === null;
}
