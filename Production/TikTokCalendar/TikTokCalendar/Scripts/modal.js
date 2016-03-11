
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

    vex.dialog.open({
        message: 'Navn og klassetrinn',
        input: '<input name=\"username\" type=\"text\" placeholder=\"Andreas\" required />\n<form><input class="input-year" type="radio" name="trinn" value="first" first> Første<input class="input-year" type="radio" name="trinn" value="second"> Andre<input class="input-year" type="radio" name="trinn" value="third"> Tredje</form> ',

        showCloseButton: true,
        buttons: [
        $.extend({}, vex.dialog.buttons.YES, {
            text: 'Login'
        }),
        
         $.extend({}, vex.dialog.buttons.NO, {
             className: 'show-all-events',
             text: 'Vis alle hendelser'
         }) 
        ],
        
        callback: function (data) {
            $.ajax({
                url: "Home/UserName",
                type: "POST",
                data: { a: data.username + ";" + data.year },
                error: function () {
                    console.log("error");
                },
                success: function (a) {
                    console.log("success" + a);
                }
            })
            //$("#user-name").html(data.username);
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
                        window.location.reload();
                    }
                })
                window.location.reload();
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
                            error: function () {
                                alert("usercourse failer");
                                console.log("error");
                            },
                            success: function (a) {
                            	console.log("success" + a);
                            	window.location.reload();
                            }
                        })
                        return;
                        //window.location.reload();
                    }
                })
            }
        }
    });
}

function isEmpty(str) {
    return typeof str == 'string' && !str.trim() || typeof str == null || str === null;
}
