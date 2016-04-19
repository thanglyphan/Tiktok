$(function () {

    (function ($) {
        $.fn.textfill = function (maxFontSize) {
            maxFontSize = parseInt(maxFontSize, 10);
            return this.each(function () {
                var ourText = $("span", this),
                    parent = ourText.parent(),
                    maxHeight = parent.height(),
                    maxWidth = parent.width(),
                    fontSize = parseInt(ourText.css("fontSize"), 10),
                    multiplier = maxWidth / ourText.width(),
                    newSize = (fontSize * (multiplier - 0.1));
                ourText.css(
                    "fontSize",
                    (maxFontSize > 0 && newSize > maxFontSize) ?
                    maxFontSize :
                        newSize
                );
            });
        };
    })(jQuery);


    $(".mobile-room").textfill();
    $(".mobile-date").textfill();

    var lastId = "";
    $('*[class^="class-info event-id"]').hover(function () {
        var eventId = $(this).attr('class');
        var arrayId = eventId.split('-');
        toggleOnHover(arrayId[3]);
    });

    $('*[class^="activator usersgoing event-id"]').hover(function () {
        var eventId = $(this).attr('class');
        var arrayId = eventId.split('-');
        toggleOnHover(arrayId[2]);
    });

    function toggleOnHover(id) {
        var stringId = '.going-';
        if (!lastId.isEmpty) {
            $(stringId + lastId).css('visibility', 'hidden');
        }

        $(stringId + id).css('visibility', 'visible');
        lastId = id;
    }

    $('#menu').slicknav({
        prependTo: '.mobile-navbar-container'
    });

    $(".feed-month").mouseenter(function (event) {
        $("badge-container").css("display", "inline-block");
    });

    $(".feed-month").mouseleave(function (event) {
        $("badge-container").css("display", "none");
    });

    function isEmpty(str) {
        return typeof str == 'string' && !str.trim() || typeof str == 'undefined' || str === null;
    }

});