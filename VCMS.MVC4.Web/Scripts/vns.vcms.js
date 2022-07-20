(function ($) {


    $.fn.checkAll = function (selector) {
        $(this).change(function () {
            var val = $(this).is(":checked");
            $(selector).prop('checked', val);
        });
    };


    $.fn.update = function (options) {
        var settings = $.extend({
            'url': '',
            'selector': '',
            'param1': 'id',
            'param2': 'order'
        }, options);

        $(this).click(function (e) {
            if (!confirm("do you really want to update selected items ?")) return false;
            e.preventDefault();
            var url = options.url;
            var values = '';
            var orders = '';
            var param1 = (options.param1) ? options.param1 : 'id';
            var param2 = (options.param2) ? options.param2 : 'order';
            $(options.selector).each(function () {
                values += (values ? "," : "") + $(this).attr('rel');
                orders += (orders ? "," : "") + $(this).val();
            });
            if (values != '')
                $.ajax({
                    url: url,
                    data: param1 + '=' + values + '&' + param2 + '=' + orders,
                    type: 'post',
                    success: function (data, status, xhr) {
                        alert(data.Message);
                        window.location.reload();
                    }
                });
        });
    };
    $.fn.validtabs = function (options) {
        var $tabs;

        $tabs = $(this).tabs({
            selected: 0
            //activate: function (event, ui) {
            //    $("textarea[name*='ShortDesc']").vFCK({ toolbar: 'Basic' });
            //    $("textarea[name*='Description']").vFCK({ toolbar: 'VNS', height: '400px' });
            //}
        });

        var validatorSettings = $.data($(this).closest("form")[0], 'validator').settings;
        validatorSettings.ignore = "";


        $(this).closest("form").submit(function (event) {
            var validator = $(this).validate({ ignore: ".ignore" });

            if (validator.errorList.length > 0) {
                var fval = $("#" + validator.errorList[0].element.id);
                var tab = fval.parents("div[id^=tabs-]");

                if (tab.length > 0) {
                    $(".nav-tabs li.active").removeClass("active");
                    var index = $("div[id^=tabs-]").index(tab);
                    $tabs.tabs('option', 'active', index);
                    $tabs.find("[tabindex='0']").addClass("active");
                    fval.focus();
                }
                return false;
            }
        });

    };

    $.fn.vFCK = function (options) {
        $(this).each(function () {
            var name = function () {
                return 'FCK_' + new Date().getTime();
            };
            if ($(this).attr('name') == undefined || $(this).attr('name') == '') {
                $(this).attr('name', name);
            }
            else
                name = $(this).attr('name');
            if (options.reset) {
                var editor = FCKeditorAPI.GetInstance(name);
                editor.SetHTML('');
            }
            else {
                var oFCKeditor = new FCKeditor(name);
                //oFCKeditor.BasePath = '/Scripts/fckeditor/';
                oFCKeditor.ToolbarStartExpanded = false;
                var settings = $.extend({ toolbar: 'VNS', height: '200px', width: '100%' }, options);
                oFCKeditor.ToolbarSet = settings.toolbar;
                oFCKeditor.Height = settings.height;
                if (settings.basePath)
                    oFCKeditor.BasePath = settings.basePath;
                oFCKeditor.Width = settings.width;
                oFCKeditor.ReplaceTextarea();
            }
        });

    };

    $.fn.sdelete = function (options) {
        var settings = $.extend({
            'url': '',
            'selector': '',
            'param': 'id'
        }, options);

        $(this).click(function (e) {
            if (!confirm("do you really want to delete selected items ?")) return false;
            e.preventDefault();
            var url = options.url;
            var values = '';
            var param = (options.param) ? options.param : 'id';
            $(options.selector).filter(":checked").each(function () {
                values += (values ? "," : "") + $(this).val();
            });
            if (values != '')
                $.ajax({
                    url: url,
                    data: param + '=' + values,
                    type: 'post',
                    success: function (data, status, xhr) {
                        if (data.Status == 0)
                            window.location.reload();
                        else
                            alert(data.Message);
                    }
                });
        });

    };
    $.fn.copy = function (options) {
        var settings = $.extend({
            'url': '',
            'selector': '',
            'param': 'id'
        }, options);

        $(this).click(function (e) {
            if (!confirm("do you really want to copy selected items ?")) return false;
            e.preventDefault();
            var url = settings.url;
            var values = '';
            var param = settings.param;
            $(settings.selector).filter(":checked").each(function () {
                values += (values ? "," : "") + $(this).val();
            });
            if (values != '')
                $.ajax({
                    url: url,
                    data: param + '=' + values,
                    type: 'post',
                    success: function (data, status, xhr) {
                        if (data.Status == 0)
                            window.location.reload();
                        else
                            alert(data.Message);
                    }
                });
        });

    };
    $.fn.autoCopy = function (options) {
        var els = $(this);

        $(this).change(function () {
            var el = $(this);
            els.not(el).each(function () {
                if ($(this).val() == '') {
                    $(this).val(el.val());
                    $(this).change();
                }
            })
        });


    };
    $.fn.movetotype = function (options) {
        var settings = $.extend({
            'url': '',
            'selector1': '',
            'selector2': '',
            'param1': 'id',
            'param2': 'typeid'
        }, options);

        $(this).click(function (e) {
            if (!confirm("do you really want to move selected items to other type ?")) return false;
            e.preventDefault();
            var url = options.url;
            var values = '';
            var typeid = '';
            var param1 = (options.param1) ? options.param1 : 'id';
            var param2 = (options.param2) ? options.param2 : 'typeid';
            $(options.selector1).filter(":checked").each(function () {
                values += (values ? "," : "") + $(this).val();
            });
            typeid += $(options.selector2).val();
            if (values != '')
                $.ajax({
                    url: url,
                    data: param1 + '=' + values + '&' + param2 + '=' + typeid,
                    type: 'post',
                    success: function (data, status, xhr) {
                        alert(data.Message);
                        window.location.reload();
                    }
                });
        });
    };
    $.fn.copytotype = function (options) {
        var settings = $.extend({
            'url': '',
            'selector1': '',
            'selector2': '',
            'param1': 'id',
            'param2': 'order'
        }, options);

        $(this).click(function (e) {
            if (!confirm("do you really want to copy selected items to other type ?")) return false;
            e.preventDefault();
            var url = options.url;
            var values = '';
            var typeid = '';
            var param1 = settings.param1;
            var param2 = settings.param2;
            $(settings.selector1).filter(":checked").each(function () {
                values += (values ? "," : "") + $(this).val();
            });
            typeid = $(settings.selector2).val();
            if (values != '')
                $.ajax({
                    url: url,
                    data: param1 + '=' + values + '&' + param2 + '=' + typeid,
                    type: 'post',
                    success: function (data, status, xhr) {
                        alert(data.Message);
                        window.location.reload();
                    }
                });
        });
    };

    $.fn.ajaxSubmit = function (options) {
        
        var obj = $(this);
        /* ADD FILE TO PARAM AJAX */
        var formData = new FormData(obj[0]);
        $.each($(obj).find("input[type='file']"), function (i, tag) {
            $.each($(tag)[0].files, function (i, file) {
                formData.append(tag.name, file);
            });
        });
        options = $.extend({ success: function () { } }, options);

        var xhr = new XMLHttpRequest();
        // Add any event handlers here...
        xhr.open('POST', obj.attr('action'), true);
        xhr.send(formData);
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4) {
                options.success();
            }
        }
        return false;

    };

    $.fn.friendlyUrl = function (options) {
        options = $.extend({ success: function (url) { } }, options);
        $(this).change(function () {
            var url = urlFriendly($(this).val());
            if (options.destination)
                options.destination.val(url);
            options.success(url);
        })
    }

    $.fn.latinize = function (options) {
        var dest = options.destination;
        $(this).on("input", function () {
            $(dest).val($(this).val().latinize());
        })
        $(this).on("change", function () {
            $(dest).val($(this).val().latinize());
        })

    }

    $.fn.vtip = function (options) {
        var defaults = {
            content: '', url: '',
            css: {
                'display': 'block',
                'position': 'absolute',
                'z-index': 1000,
                'background': '#fff',
                'padding': '10px',
                'min-width': '300px',
                'min-height': '100px',
                'max-width': '600px',
                'max-height': '300px',
                'overflow': 'hidden',
                'border': '1px solid #eee',
                '-webkit-box-shadow': '0px 0px 19px 5px rgba(0,0,0,0.84)',
                '-moz-box-shadow': '0px 0px 19px 5px rgba(0,0,0,0.84)',
                'box-shadow': '0px 0px 19px 5px rgba(0,0,0,0.84)',


            }
        };
        if (options.css)
            options.css = $.extend(defaults.css, options.css);
        options = $.extend(defaults, options);

        var popup = $("#popup-container");
        if (popup.length <= 0) {
            popup = $("<div id='popup-container'/>");
            popup.appendTo($('body'));
        }

        $(this).on('mouseenter', function (event) {

            if (options.url) {
                if (!$(this).data('loaded')) {
                    popup.html("loading ...");
                    var el = $(this);
                    $.ajax({
                        url: options.url,
                        data: "X-Requested-With=XMLHttpRequest",
                        success: function (data) {
                            $(this).data('content', data);
                            $(this).data('loaded', true);
                            popup.html(data);
                        }
                    });
                }
            }
            else {
                $(this).data('content', options.content.html());
            }

            popup.html($(this).data('content'));
            var css = $.extend({ top: 0 }, options.css);
            if ($(this).data('top'))
                css.top = $(this).data('top');
            popup.css(css);
            popup.css({ left: -60 - popup.width() });

        });
        $(this).on('mousemove', function (event) {

            var css = { left: event.pageX + 10, top: event.pageY + 10 };
            if (popup.width() + css.left > $(window).width() - 10) {
                css.left = css.left - popup.width() - 20;
            }
            if (popup.height() + css.top > $(window).height() + $(window).scrollTop() - 50) {
                css.top = css.top - popup.height() - 50;
            }
            popup.animate(css, {
                duration: 500,
                queue: false
            });
            $(this).data('top', css.top);

        });
        $(this).on('mouseleave', function (event) {

            popup.hide();
        });

    }
})(jQuery);


function urlFriendly(str) {
    var rExps = [
    { re: /[\xC0-\xC6]/g, ch: 'A' },
    { re: /[\xE0-\xE6]/g, ch: 'a' },
    { re: /[\xC8-\xCB]/g, ch: 'E' },
    { re: /[\xE8-\xEB]/g, ch: 'e' },
    { re: /[\xCC-\xCF]/g, ch: 'I' },
    { re: /[\xEC-\xEF]/g, ch: 'i' },
    { re: /[\xD2-\xD6]/g, ch: 'O' },
    { re: /[\xF2-\xF6]/g, ch: 'o' },
    { re: /[\xD9-\xDC]/g, ch: 'U' },
    { re: /[\xF9-\xFC]/g, ch: 'u' },
    { re: /[\xD1]/g, ch: 'N' },
    { re: /[\xF1]/g, ch: 'n' },
    { re: /[^a-zA-Z0-9]+/g, ch: '-' }];

    for (var i = 0, len = rExps.length; i < len; i++)
        str = str.replace(rExps[i].re, rExps[i].ch);

    return str;
}
function language_changed(data, status, xhr) {

    window.location.href = data.url;
}
$(function () {
    $("form input").each(function () { $(this).data("pl", $(this).attr("placeholder")) });
    $("form input").focus(function () {

        $(this).attr("placeholder", "");
    }).blur(function () {
        $(this).attr("placeholder", $(this).data("pl"));
    });

    if (jQuery().corner) {
        $('.corner,.box,.box-header').corner("5px");
        $("[data-corner]").corner();
    }
    if (jQuery().lazy) {
        
        $("img.lazy").lazy();
    }
});

if (!Array.prototype.indexOf)
{
  Array.prototype.indexOf = function(elt /*, from*/)
  {
    var len = this.length >>> 0;

    var from = Number(arguments[1]) || 0;
    from = (from < 0)
         ? Math.ceil(from)
         : Math.floor(from);
    if (from < 0)
      from += len;

    for (; from < len; from++)
    {
      if (from in this &&
          this[from] === elt)
        return from;
    }
    return -1;
  };
}