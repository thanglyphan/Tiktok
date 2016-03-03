function showLogin() {

    var userName = "";

    vex.dialog.open({
        message: 'Westerdals brukernavn',
        input: "<input name=\"userName\" type=\"text\" placeholder=\"nelwil14\" required />",

        buttons: [
        $.extend({}, vex.dialog.buttons.YES, {
            text: 'Login'
        })
        ],

        callback: function (data) {
            if (data === false) {
                return console.log('Cancelled');
            }

            if (isEmpty(userName)) {

                //  Check if username matches any of the registered users
                //  userName === "whatever"

                //  If this is a new user
                vex.dialog.open({

                    message: "Velg ditt studieprogram",
                    buttons: [

                        /*=======================================================
                            1. K L A S S E
                        =======================================================*/
                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: '1. Klasse Bachelor IT', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = 'firstBachelorIT';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),

                        /*=======================================================
                            2. K L A S S E
                        =======================================================*/
                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: '2. Klasse Programmering', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = 'secondProgrammering';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),
                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: '2. Klasse Intelligente Systemer', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = 'secondIntelligenteSystemer';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),
                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: '2. Klasse Spilldesign', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = 'secondSpilldesign';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),
                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: '2. Klasse Spillprogrammering', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = 'secondSpillprogrammering';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),
                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: '2. Klasse 3D Grafikk', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = 'second3DGrafikk';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),
                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: '2. Klasse Interaktivt Design', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = 'secondInteraktivtDesign';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),
                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: '2. Klasse E-Business', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = 'secondEBusiness';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),


                        /*=======================================================
                            3. K L A S S E
                        =======================================================*/
                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: '3. Klasse Programmering', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = 'thirdProgrammering';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),
                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: '3. Klasse Spilldesign', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = 'thirdSpilldesign';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),
                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: '3. Klasse Spillprogrammering', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = 'thirdSpillprogrammering';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),
                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: '3. Klasse 3D Grafikk', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = 'third3DGrafikk';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),
                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: '3. Klasse Interaktivt Design', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = 'thirdInteraktivtDesign';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),
                        $.extend({}, vex.dialog.buttons.NO, {
                            className: 'vex-dialog-button', text: '3. Klasse E-Business', click: function ($vexContent, event) {
                                $vexContent.data().vex.value = 'thirdEBusiness';
                                vex.close($vexContent.data().vex.id);
                            }
                        }),
                    ],
                    callback: function (value) {
                        // value will be one of the following:
                        course = value;
                        $("#user-course").html(value);
                        $("#user-name").html("TEKST FRA MODAL.JS");
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
