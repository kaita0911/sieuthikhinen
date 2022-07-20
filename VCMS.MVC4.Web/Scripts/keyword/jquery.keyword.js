(function ($) {
    $.fn.keyword = function (config) {
        $(this).each(function () {
            var el = $(this);
            el.attr('autocomplete', 'off');
            var panel = $("<div class='keyword-panel'></div>").insertAfter($(this));
            var form = el.closest('form');
            ///process current item
            var keywords = $(this).val().split(/[;,]/);
            for (var i = 0; i < keywords.length; i++) {
                var val = keywords[i];
                if (val != "") {
                    $("<span class='keyword-item' title='click to remove'>" + val + "</span>").appendTo(panel).click(function () {
                        $(this).next().remove();
                        $(this).remove();
                    });
                    $("<input type='hidden' name='" + el.attr("name") + "' value='" + val + "'/>").appendTo(panel);
                }
            }
            $(this).val("");
            if (config != undefined && config.disabled) $(this).hide();
            el.on('keydown', function (event) {
                if (event.keyCode == 13 || event.keyCode == 186 || event.keyCode == 188) {
                    event.preventDefault();
                    var val = $(this).val();

                    if (val != '') {
                        $("<span class='keyword-item' title='click to remove'>" + val + "</span>").appendTo(panel).click(function () {
                            $(this).next().remove();
                            $(this).remove();
                        });
                        $("<input type='hidden' name='" + el.attr("name") + "' value='" + val + "'/>").appendTo(panel);
                        $(this).val("");
                    }
                }
            });
            form.on("submit", function () {
                var val = el.val();
                $("input[type=hidden][name='" + el.attr("name") + "']").each(function () {
                    var hVal = $(this).val();
                    if (hVal != "") {
                        val += (val != "" ? "," : "") + hVal;
                        $(this).val("");
                    }
                });
                el.val(val);
            })

        });

    }

    $(".keyword").keyword();
})(jQuery);