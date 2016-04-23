$(function() {

    (function($) {
        $.fn.textfill = function(maxFontSize) {
            maxFontSize = parseInt(maxFontSize, 10);
            return this.each(function() {
                var ourText = $("span", this),
                    parent = ourText.parent(),
                    maxHeight = parent.height(),
                    maxWidth = parent.width(),
                    fontSize = parseInt(ourText.css("fontSize"), 10),
                    multiplier = maxWidth / ourText.width(),
                    newSize = (fontSize * (multiplier - 0.1));
                ourText.css(
                    "fontSize",
                    (maxFontSize > 0 && newSize > maxFontSize) ? maxFontSize : newSize
                );
            });
        };
    })(jQuery);


    $(".mobile-room").textfill();
    $(".mobile-date").textfill();

    var lastId = "";


    $('*[class^="feed-container')
        .mouseenter(function() {
            var eventId = $(this).attr("id");
            var arrayId = eventId.split("-");
            lastId = arrayId;
            toggleOnHover(arrayId[1]);
        });

    $('*[class^="feed-container')
        .mouseleave(function() {
            var stringId = ".going-" + lastId;
            $(stringId).css("visibility", "hidden");
        });

    function toggleOnHover(id) {
        var stringId = ".going-";
        if (!lastId.isEmpty) {
            $(stringId + lastId).css("visibility", "hidden");
        }

        $(stringId + id).css("visibility", "visible");
        lastId = id;
    }

    $("#menu")
        .slicknav({
            prependTo: ".mobile-navbar-container"
        });

    $('#login-modal').modal({
        backdrop: 'static',
        keyboard: false
    });

    function isEmpty(str) {
        return typeof str == "string" && !str.trim() || typeof str == "undefined" || str === null;
    }

});