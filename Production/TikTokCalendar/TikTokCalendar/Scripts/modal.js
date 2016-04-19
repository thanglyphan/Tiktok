
function checkLogin() {
    $.ajax({
        url: "/Home/GetVisited",
        type: "GET",
        dataType: "json",
        success: function (data) {
            console.log(data);
            if (data) {
                console.log("Data er: " + data)
            } else {
                showLogin();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });

}

function showLogin() {
    var username = "";
    var showRooms = false;
    vex.dialog.open({
        message: 'Navn og klassetrinn',
        input: '<input name=\"username\" type=\"text\" placeholder=\"Andreas\" required /><div class="class-box"><label for="first"> Første</label><input class="input-year" type="radio" name="trinn" value="first" id="first" first></div> <div class="class-box"><label for="second"> Andre</label><input class="input-year" type="radio" name="trinn" value="second" id="second"></div><div class="class-box"><label for="third"> Tredje</label><input class="input-year" type="radio" name="trinn" value="third" id="third"></div>',

        showCloseButton: true,
        buttons: [
        $.extend({}, vex.dialog.buttons.YES, {
            text: 'Login'
        }),

        $.extend({}, vex.dialog.buttons.NO, {
            className: 'show-available-rooms', text: 'Vis ledige rom', click: function ($vexContent, event) {
                $vexContent.data().vex.value = 'available';
                vex.close($vexContent.data().vex.id);
                showRooms = true;

            }
        }),
        ],

        callback: function (data) {
            if (data === 'available') {

                $.ajax({
                    url: "Home/Rooms",
                    type: "POST",
                    data: { a: "yes" },
                    error: function () {
                        console.log("error");
                    },
                    success: function (a) {
                        console.log("success" + a);
                    }
                })
                //window.location.reload();

                
                
            }

            else {
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


                $.ajax({
                    url: "Home/UserYear",
                    type: "POST",
                    data: { a: data.trinn },
                    error: function () {
                        console.log("error");
                    },
                    success: function (a) {
                        console.log("success" + a);
                    }
                })

                if (data === false) {
                    $.ajax({
                        url: "Home/ShowDefault",
                        type: "POST",
                        data: { a: "Default" },
                        error: function () {
                            console.log("error");
                        },
                        success: function (a) {
                            console.log("success" + a);
                        }
                    })

                    return;
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
                                    $vexContent.data().vex.value = 'Bachelor IT';
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
                                error: function (jqXHR, textStatus, errorThrown) {
                                	alert("Error in the usercourses AJAX:\n\n" + textStatus + "\n\n" + errorThrown);
                                    console.log("error");
                                },
                                success: function (a) {
                                    window.location.reload();
                                }
                            })
                            return;
                        }
                    })
                }
            }
        }
    });
}

function isEmpty(str) {
    return typeof str == 'string' && !str.trim() || typeof str == null || str === null;
}
function vexHelper() {
    for (var a = 0; a < 3; a++) {

    }
    return "lol økaøai jailø jiaøw jaiwøf jawøoed |wakød awfljea u5l-dhawfueah aekfhaeku hfaekuhu";
}