$(function () {
    $("div[data-widget]").each(function (index, item) {
        $(item).load($(item).data("widget"), function () {
            var callback = $(item).data("callback");
            if (callback)
                eval(callback);
        });
    });
    $("div[data-widget-group]").each(function (index,item) {
        $(item).load($(item).data("widget-group"), function () {
            var callback = $(item).data("callback");
            if (callback)
                eval(callback);
        });

    });
    $("div[data-partial]").each(function (index, item) {
        $(item).load($(item).data("partial"), function () {
            var callback = $(item).data("callback");
            if (callback)
                eval(callback);
        });
    });
})