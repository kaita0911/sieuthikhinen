﻿var timeoutsnap = null;
var timeoutsnaphidden = null;
jQuery(window).ready(function () {
    VienNam();
});

function VienNam() {
    jQuery.browserDetect();
    _SiderBar();
    jQuery.Common();
    _plugin();
}

$(window).load(function () {
    setTimeout(function () {
        if ($(".load-iframe").length) {
            $(".load-iframe").each(function () {
                var _this = $(this);
                var data = _this.data("iframe-load");
                $.ajax({
                    url: data,
                    dataType: "html",
                    success: function (e) {
                        _this.after(e);
                        _this.remove();
                    }
                });
            });
        }
    }, 1000);
});

$(window).bind("load", function () {
    var timeout = setTimeout(function () { $("img.lazy.owl").trigger("sporty") }, 2500);
});

function _SiderBar() {
    $(document).delegate('a.header-menu', 'click', function () {
        $("body").toggleClass("snapjs");
        $('.navigation').toggleClass('show-menu');
        $(this).find('.fa').toggleClass('rotate-star');
        return false;
    });

    $(document).delegate('a.close-header-menu', 'click', function () {
        $("body").toggleClass("snapjs");
        $('.navigation').toggleClass('show-menu');
        $(this).parent().parent().find('.fa').removeClass('rotate-star');

        return false;
    });

    $(document).delegate('.open-left-sidebar', 'click', function () {
        clearTimeout(timeoutsnap);
        if ($("body").hasClass("snapjs-right"))
            return false;
        snapjs_Left_Close = !$("body,html").hasClass("snapjs-left") ? true : false;
        hidden_snapjs = !$("body,html").hasClass("hidden-snap") ? true : false;
        $("body,html").removeClass("hidden-snap").toggleClass("snapjs-left hidden-snapjs-right");
        return false;
    });
    $(document).delegate('.open-right-sidebar', 'click', function () {
        $("body,html").removeClass("hidden-snap").toggleClass("snapjs-right");
        hidden_snapjs = !$("body,html").hasClass("hidden-snap") ? true : false;
        return false;
    });
    $(document).delegate('.close-sidebar,.snapjs-left #page,.snapjs-right #page', 'click', function () {
        clearTimeout(timeoutsnap);
        clearTimeout(timeoutsnaphidden);
        $("body, html").removeClass("snapjs-right snapjs-left");
        if (hidden_snapjs) {
            timeoutsnaphidden = setTimeout(function () {
                $("body,html").toggleClass("hidden-snap");
            }, 350);
            hidden_snapjs = false;
        }

        if (snapjs_Left_Close) {
            timeoutsnap = setTimeout(function () {
                $("body,html").toggleClass("hidden-snapjs-right");
            }, 350);
            snapjs_Left_Close = false;
        }
        return false;
    });

    $(window).load(function () {
        $(".snap-drawer").removeClass("hidden");
    });
}
function Fixtop() {
    var e = jQuery(document).scrollTop();
    jQuery("[data-fixtop]").each(function () {
        var _t = jQuery(this);
        var _m = jQuery("[data-margin-top]");
        if (window.topNavSmall === false && e > parseInt(_t.attr("data-scoll"))) {
            _t.addClass("fixtop");
            _m.css("margin-top", parseInt(_m.attr("data-margin-top")) + parseInt(2));
            window.topNavSmall = true;
        }
        if (window.topNavSmall === true && e < parseInt(_t.attr("data-scoll"))) {
            _t.removeClass("fixtop");
            _m.css("margin-top", "0px");
            window.topNavSmall = false;
        }

    });
}
(function ($) {
    $.extend({
        Common: function () {
            function hasParentClass(e, classname) {
                if (e === document) return false;
                if (classie.has(e, classname)) {
                    return true;
                }
                return e.parentNode && hasParentClass(e.parentNode, classname);
            }
            function mobilecheck() {
                var check = false;
                (function (a) { if (/(android|ipad|playbook|silk|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) check = true })(navigator.userAgent || navigator.vendor || window.opera);
                return check;
            }
            function init() {
                var $mcontent = $('#sidebar');

                var $offcmenu = $('<div id="sidebar-column" class="sidebar-column hidden-lg hidden-md"><div class="content"></div></div>');

                $(".content", $offcmenu).append($mcontent.html()).find(".hidden-sm, .hidden-xs").remove();
                $("body .snap-drawers").append($offcmenu);
                $("#sidebar-column").append("<div class='button-close-menu'><i class='fa fa-times-circle'></i></div>");
                var $btn = $("#sidebar-column .button-close-sidebar");
                var eventtype = mobilecheck() ? 'click' : 'click';

                $($btn).bind(eventtype, function (e) {
                    $("body").toggleClass("show-sidebar");
                    $("#page").bind(eventtype, function () {
                        $("#page").unbind(eventtype);
                    });
                    e.stopPropagation();
                    return false;
                });
            }
            init();
        }
    });

})(jQuery);
/** Animate
 **************************************************************** **/


/** OWL Carousel
 **************************************************************** **/


/** 05. LightBox
 **************************************************************** **/

/** WAIT FOR IMAGES [used by masonry]
 **************************************************************** **/
(function ($) {
    // Namespace all events.
    var eventNamespace = 'waitForImages';

    // CSS properties which contain references to images.
    $.waitForImages = {
        hasImageProperties: ['backgroundImage', 'listStyleImage', 'borderImage', 'borderCornerImage', 'cursor']
    };

    // Custom selector to find `img` elements that have a valid `src` attribute and have not already loaded.
    $.expr[':'].uncached = function (obj) {
        // Ensure we are dealing with an `img` element with a valid `src` attribute.
        if (!$(obj).is('img[src!=""]')) {
            return false;
        }

        // Firefox's `complete` property will always be `true` even if the image has not been downloaded.
        // Doing it this way works in Firefox.
        var img = new Image();
        img.src = obj.src;
        return !img.complete;
    };

    $.fn.waitForImages = function (finishedCallback, eachCallback, waitForAll) {

        var allImgsLength = 0;
        var allImgsLoaded = 0;

        // Handle options object.
        if ($.isPlainObject(arguments[0])) {
            waitForAll = arguments[0].waitForAll;
            eachCallback = arguments[0].each;
            // This must be last as arguments[0]
            // is aliased with finishedCallback.
            finishedCallback = arguments[0].finished;
        }

        // Handle missing callbacks.
        finishedCallback = finishedCallback || $.noop;
        eachCallback = eachCallback || $.noop;

        // Convert waitForAll to Boolean
        waitForAll = !!waitForAll;

        // Ensure callbacks are functions.
        if (!$.isFunction(finishedCallback) || !$.isFunction(eachCallback)) {
            throw new TypeError('An invalid callback was supplied.');
        }

        return this.each(function () {
            // Build a list of all imgs, dependent on what images will be considered.
            var obj = $(this);
            var allImgs = [];
            // CSS properties which may contain an image.
            var hasImgProperties = $.waitForImages.hasImageProperties || [];
            // To match `url()` references.
            // Spec: http://www.w3.org/TR/CSS2/syndata.html#value-def-uri
            var matchUrl = /url\(\s*(['"]?)(.*?)\1\s*\)/g;

            if (waitForAll) {

                // Get all elements (including the original), as any one of them could have a background image.
                obj.find('*').addBack().each(function () {
                    var element = $(this);

                    // If an `img` element, add it. But keep iterating in case it has a background image too.
                    if (element.is('img:uncached')) {
                        allImgs.push({
                            src: element.attr('src'),
                            element: element[0]
                        });
                    }

                    $.each(hasImgProperties, function (i, property) {
                        var propertyValue = element.css(property);
                        var match;

                        // If it doesn't contain this property, skip.
                        if (!propertyValue) {
                            return true;
                        }

                        // Get all url() of this element.
                        while (match = matchUrl.exec(propertyValue)) {
                            allImgs.push({
                                src: match[2],
                                element: element[0]
                            });
                        }
                    });
                });
            } else {
                // For images only, the task is simpler.
                obj.find('img:uncached')
                    .each(function () {
                        allImgs.push({
                            src: this.src,
                            element: this
                        });
                    });
            }

            allImgsLength = allImgs.length;
            allImgsLoaded = 0;

            // If no images found, don't bother.
            if (allImgsLength === 0) {
                finishedCallback.call(obj[0]);
            }

            $.each(allImgs, function (i, img) {

                var image = new Image();

                // Handle the image loading and error with the same callback.
                $(image).on('load.' + eventNamespace + ' error.' + eventNamespace, function (event) {
                    allImgsLoaded++;

                    // If an error occurred with loading the image, set the third argument accordingly.
                    eachCallback.call(img.element, allImgsLoaded, allImgsLength, event.type == 'load');

                    if (allImgsLoaded == allImgsLength) {
                        finishedCallback.call(obj[0]);
                        return false;
                    }

                });

                image.src = img.src;
            });
        });
    };
}(jQuery));
/** BROWSER DETECT
	Add browser to html class
 **************************************************************** **/
(function ($) {
    $.extend({
        browserDetect: function () {
            var u = navigator.userAgent,
				ua = u.toLowerCase(),
				is = function (t) {
				    return ua.indexOf(t) > -1;
				},
				g = 'gecko',
				w = 'webkit',
				s = 'safari',
				o = 'opera',
				h = document.documentElement,
				b = [(!(/opera|webtv/i.test(ua)) && /msie\s(\d)/.test(ua)) ? ('ie ie' + parseFloat(navigator.appVersion.split("MSIE")[1])) : is('firefox/2') ? g + ' ff2' : is('firefox/3.5') ? g + ' ff3 ff3_5' : is('firefox/3') ? g + ' ff3' : is('gecko/') ? g : is('opera') ? o + (/version\/(\d+)/.test(ua) ? ' ' + o + RegExp.jQuery1 : (/opera(\s|\/)(\d+)/.test(ua) ? ' ' + o + RegExp.jQuery2 : '')) : is('konqueror') ? 'konqueror' : is('chrome') ? w + ' chrome' : is('iron') ? w + ' iron' : is('applewebkit/') ? w + ' ' + s + (/version\/(\d+)/.test(ua) ? ' ' + s + RegExp.jQuery1 : '') : is('mozilla/') ? g : '', is('j2me') ? 'mobile' : is('iphone') ? 'iphone' : is('ipod') ? 'ipod' : is('mac') ? 'mac' : is('darwin') ? 'mac' : is('webtv') ? 'webtv' : is('win') ? 'win' : is('freebsd') ? 'freebsd' : (is('x11') || is('linux')) ? 'linux' : '', 'js'];

            c = b.join(' ');
            h.className += ' ' + c;
            var isIE11 = !(window.ActiveXObject) && "ActiveXObject" in window;
            if (isIE11) {
                jQuery('html').removeClass('gecko').addClass('ie ie11');
                return;
            }
        }
    });
})(jQuery);

/** SMOOTHSCROLL V1.2.1
	Licensed under the terms of the MIT license.
 **************************************************************** **/
!function (e) { e.extend({ smoothScroll: function () { function e() { var e = !1; if (document.URL.indexOf("google.com/reader/view") > -1 && (e = !0), b.excluded) { var t = b.excluded.split(/[,\n] ?/); t.push("mail.google.com"); for (var o = t.length; o--;) if (document.URL.indexOf(t[o]) > -1) { g && g.disconnect(), u("mousewheel", n), e = !0, y = !0; break } } e && u("keydown", a), b.keyboardSupport && !e && c("keydown", a) } function t() { if (document.body) { var t = document.body, o = document.documentElement, n = window.innerHeight, a = t.scrollHeight; if (D = document.compatMode.indexOf("CSS") >= 0 ? o : t, w = t, e(), S = !0, top != self) k = !0; else if (a > n && (t.offsetHeight <= n || o.offsetHeight <= n)) { var r = !1, i = function () { r || o.scrollHeight == document.height || (r = !0, setTimeout(function () { o.style.height = document.height + "px", r = !1 }, 500)) }; o.style.height = "auto", setTimeout(i, 10); var l = { attributes: !0, childList: !0, characterData: !1 }; if (g = new R(i), g.observe(t, l), D.offsetHeight <= n) { var c = document.createElement("div"); c.style.clear = "both", t.appendChild(c) } } if (document.URL.indexOf("mail.google.com") > -1) { var u = document.createElement("style"); u.innerHTML = ".iu { visibility: hidden }", (document.getElementsByTagName("head")[0] || o).appendChild(u) } else if (document.URL.indexOf("www.facebook.com") > -1) { var s = document.getElementById("home_stream"); s && (s.style.webkitTransform = "translateZ(0)") } b.fixedBackground || y || (t.style.backgroundAttachment = "scroll", o.style.backgroundAttachment = "scroll") } } function o(e, t, o, n) { if (n || (n = 1e3), d(t, o), 1 != b.accelerationMax) { var a = +new Date, r = a - C; if (r < b.accelerationDelta) { var i = (1 + 30 / r) / 2; i > 1 && (i = Math.min(i, b.accelerationMax), t *= i, o *= i) } C = +new Date } if (T.push({ x: t, y: o, lastX: 0 > t ? .99 : -.99, lastY: 0 > o ? .99 : -.99, start: +new Date }), !L) { var l = e === document.body, c = function () { for (var a = +new Date, r = 0, i = 0, u = 0; u < T.length; u++) { var s = T[u], d = a - s.start, f = d >= b.animationTime, m = f ? 1 : d / b.animationTime; b.pulseAlgorithm && (m = p(m)); var h = s.x * m - s.lastX >> 0, w = s.y * m - s.lastY >> 0; r += h, i += w, s.lastX += h, s.lastY += w, f && (T.splice(u, 1), u--) } l ? window.scrollBy(r, i) : (r && (e.scrollLeft += r), i && (e.scrollTop += i)), t || o || (T = []), T.length ? O(c, e, n / b.frameRate + 1) : L = !1 }; O(c, e, 0), L = !0 } } function n(e) { S || t(); var n = e.target, a = l(n); if (!a || e.defaultPrevented || s(w, "embed") || s(n, "embed") && /\.pdf/i.test(n.src)) return !0; var r = e.wheelDeltaX || 0, i = e.wheelDeltaY || 0; return r || i || (i = e.wheelDelta || 0), !b.touchpadSupport && f(i) ? !0 : (Math.abs(r) > 1.2 && (r *= b.stepSize / 120), Math.abs(i) > 1.2 && (i *= b.stepSize / 120), o(a, -r, -i), void e.preventDefault()) } function a(e) { var t = e.target, n = e.ctrlKey || e.altKey || e.metaKey || e.shiftKey && e.keyCode !== M.spacebar; if (/input|textarea|select|embed/i.test(t.nodeName) || t.isContentEditable || e.defaultPrevented || n) return !0; if (s(t, "button") && e.keyCode === M.spacebar) return !0; var a, r = 0, i = 0, c = l(w), u = c.clientHeight; switch (c == document.body && (u = window.innerHeight), e.keyCode) { case M.up: i = -b.arrowScroll; break; case M.down: i = b.arrowScroll; break; case M.spacebar: a = e.shiftKey ? 1 : -1, i = -a * u * .9; break; case M.pageup: i = .9 * -u; break; case M.pagedown: i = .9 * u; break; case M.home: i = -c.scrollTop; break; case M.end: var d = c.scrollHeight - c.scrollTop - u; i = d > 0 ? d + 10 : 0; break; case M.left: r = -b.arrowScroll; break; case M.right: r = b.arrowScroll; break; default: return !0 } o(c, r, i), e.preventDefault() } function r(e) { w = e.target } function i(e, t) { for (var o = e.length; o--;) E[z(e[o])] = t; return t } function l(e) { var t = [], o = D.scrollHeight; do { var n = E[z(e)]; if (n) return i(t, n); if (t.push(e), o === e.scrollHeight) { if (!k || D.clientHeight + 10 < o) return i(t, document.body) } else if (e.clientHeight + 10 < e.scrollHeight && (overflow = getComputedStyle(e, "").getPropertyValue("overflow-y"), "scroll" === overflow || "auto" === overflow)) return i(t, e) } while (e = e.parentNode) } function c(e, t, o) { window.addEventListener(e, t, o || !1) } function u(e, t, o) { window.removeEventListener(e, t, o || !1) } function s(e, t) { return (e.nodeName || "").toLowerCase() === t.toLowerCase() } function d(e, t) { e = e > 0 ? 1 : -1, t = t > 0 ? 1 : -1, (x.x !== e || x.y !== t) && (x.x = e, x.y = t, T = [], C = 0) } function f(e) { if (e) { e = Math.abs(e), H.push(e), H.shift(), clearTimeout(N); var t = H[0] == H[1] && H[1] == H[2], o = m(H[0], 120) && m(H[1], 120) && m(H[2], 120); return !(t || o) } } function m(e, t) { return Math.floor(e / t) == e / t } function h(e) { var t, o, n; return e *= b.pulseScale, 1 > e ? t = e - (1 - Math.exp(-e)) : (o = Math.exp(-1), e -= 1, n = 1 - Math.exp(-e), t = o + n * (1 - o)), t * b.pulseNormalize } function p(e) { return e >= 1 ? 1 : 0 >= e ? 0 : (1 == b.pulseNormalize && (b.pulseNormalize /= h(1)), h(e)) } var w, g, v = { frameRate: 60, animationTime: 700, stepSize: 120, pulseAlgorithm: !0, pulseScale: 10, pulseNormalize: 1, accelerationDelta: 20, accelerationMax: 1, keyboardSupport: !0, arrowScroll: 50, touchpadSupport: !0, fixedBackground: !0, excluded: "" }, b = v, y = !1, k = !1, x = { x: 0, y: 0 }, S = !1, D = document.documentElement, H = [120, 120, 120], M = { left: 37, up: 38, right: 39, down: 40, spacebar: 32, pageup: 33, pagedown: 34, end: 35, home: 36 }, T = [], L = !1, C = +new Date, E = {}; setInterval(function () { E = {} }, 1e4); var N, z = function () { var e = 0; return function (t) { return t.uniqueID || (t.uniqueID = e++) } }(), O = function () { return window.requestAnimationFrame || window.webkitRequestAnimationFrame || function (e, t, o) { window.setTimeout(e, o || 1e3 / 60) } }(), R = window.MutationObserver || window.WebKitMutationObserver; c("mousedown", r), c("mousewheel", n), c("load", t) } }) }(jQuery);

/** COUNT TO
	https://github.com/mhuggins/jquery-countTo
 **************************************************************** **/
(function ($) {
    $.fn.countTo = function (options) {
        options = options || {};

        return jQuery(this).each(function () {
            // set options for current element
            var settings = jQuery.extend({}, $.fn.countTo.defaults, {
                from: jQuery(this).data('from'),
                to: jQuery(this).data('to'),
                speed: jQuery(this).data('speed'),
                refreshInterval: jQuery(this).data('refresh-interval'),
                decimals: jQuery(this).data('decimals')
            }, options);

            // how many times to update the value, and how much to increment the value on each update
            var loops = Math.ceil(settings.speed / settings.refreshInterval),
				increment = (settings.to - settings.from) / loops;

            // references & variables that will change with each update
            var self = this,
				$self = jQuery(this),
				loopCount = 0,
				value = settings.from,
				data = $self.data('countTo') || {};

            $self.data('countTo', data);

            // if an existing interval can be found, clear it first
            if (data.interval) {
                clearInterval(data.interval);
            }
            data.interval = setInterval(updateTimer, settings.refreshInterval);

            // __construct the element with the starting value
            render(value);

            function updateTimer() {
                value += increment;
                loopCount++;

                render(value);

                if (typeof (settings.onUpdate) == 'function') {
                    settings.onUpdate.call(self, value);
                }

                if (loopCount >= loops) {
                    // remove the interval
                    $self.removeData('countTo');
                    clearInterval(data.interval);
                    value = settings.to;

                    if (typeof (settings.onComplete) == 'function') {
                        settings.onComplete.call(self, value);
                    }
                }
            }

            function render(value) {
                var formattedValue = settings.formatter.call(self, value, settings);
                $self.html(formattedValue);
            }
        });
    };

    $.fn.countTo.defaults = {
        from: 0,               // the number the element should start at
        to: 0,                 // the number the element should end at
        speed: 1000,           // how long it should take to count between the target numbers
        refreshInterval: 100,  // how often the element should be updated
        decimals: 0,           // the number of decimal places to show
        formatter: formatter,  // handler for formatting the value before rendering
        onUpdate: null,        // callback method for every time the element is updated
        onComplete: null       // callback method for when the element finishes updating
    };

    function formatter(value, settings) {
        return value.toFixed(settings.decimals);
    }
}(jQuery));



/** Appear
	https://github.com/bas2k/jquery.appear/
 **************************************************************** **/
!function (e) { e.fn.appear = function (a, r) { var n = e.extend({ data: void 0, one: !0, accX: 0, accY: 0 }, r); return this.each(function () { var r = e(this); if (r.appeared = !1, !a) return void r.trigger("appear", n.data); var p = e(window), t = function () { if (!r.is(":visible")) return void (r.appeared = !1); var e = p.scrollLeft(), a = p.scrollTop(), t = r.offset(), c = t.left, i = t.top, o = n.accX, f = n.accY, s = r.height(), u = p.height(), d = r.width(), l = p.width(); i + s + f >= a && a + u + f >= i && c + d + o >= e && e + l + o >= c ? r.appeared || r.trigger("appear", n.data) : r.appeared = !1 }, c = function () { if (r.appeared = !0, n.one) { p.unbind("scroll", t); var c = e.inArray(t, e.fn.appear.checks); c >= 0 && e.fn.appear.checks.splice(c, 1) } a.apply(this, arguments) }; n.one ? r.one("appear", n.data, c) : r.bind("appear", n.data, c), p.scroll(t), e.fn.appear.checks.push(t), t() }) }, e.extend(e.fn.appear, { checks: [], timeout: null, checkAll: function () { var a = e.fn.appear.checks.length; if (a > 0) for (; a--;) e.fn.appear.checks[a]() }, run: function () { e.fn.appear.timeout && clearTimeout(e.fn.appear.timeout), e.fn.appear.timeout = setTimeout(e.fn.appear.checkAll, 20) } }), e.each(["append", "prepend", "after", "before", "attr", "removeAttr", "addClass", "removeClass", "toggleClass", "remove", "css", "show", "hide"], function (a, r) { var n = e.fn[r]; n && (e.fn[r] = function () { var a = n.apply(this, arguments); return e.fn.appear.run(), a }) }) }(jQuery);

/** Parallax
	http://www.ianlunn.co.uk/plugins/jquery-parallax/
 **************************************************************** **/
!function (t) { var e = t(window), o = e.height(); e.resize(function () { o = e.height() }), t.fn.parallax = function (i, n, r) { function s() { p.each(function () { h = p.offset().top }), a = r ? function (t) { return t.outerHeight(!0) } : function (t) { return t.height() }, (arguments.length < 1 || null === i) && (i = "50%"), (arguments.length < 2 || null === n) && (n = .5), (arguments.length < 3 || null === r) && (r = !0); var s = e.scrollTop(); p.each(function () { var e = t(this), r = e.offset().top, u = a(e); s > r + u || r > s + o || p.css("backgroundPosition", i + " " + Math.round((h - s) * n) + "px") }) } var a, h, p = t(this); e.bind("scroll", s).resize(s), s() } }(jQuery), function (t, e) { "use strict"; t.HoverDir = function (e, o) { this.$el = t(o), this._init(e) }, t.HoverDir.defaults = { speed: 300, easing: "ease-in-out", hoverDelay: 0, inverse: !1 }, t.HoverDir.prototype = { _init: function (e) { this.options = t.extend(!0, {}, t.HoverDir.defaults, e), this.transitionProp = "all " + this.options.speed + "ms " + this.options.easing, this.support = Modernizr.csstransitions, this._loadEvents() }, _loadEvents: function () { var e = this; this.$el.on("mouseenter.hoverdir, mouseleave.hoverdir", function (o) { var i = t(this), n = i.find(".hover-mark"), r = e._getDir(i, { x: o.pageX, y: o.pageY }), s = e._getStyle(r); "mouseenter" === o.type ? (n.hide().css(s.from), clearTimeout(e.tmhover), e.tmhover = setTimeout(function () { n.show(0, function () { var o = t(this); e.support && o.css("transition", e.transitionProp), e._applyAnimation(o, s.to, e.options.speed) }) }, e.options.hoverDelay)) : (e.support && n.css("transition", e.transitionProp), clearTimeout(e.tmhover), e._applyAnimation(n, s.from, e.options.speed)) }) }, _getDir: function (t, e) { var o = t.width(), i = t.height(), n = (e.x - t.offset().left - o / 2) * (o > i ? i / o : 1), r = (e.y - t.offset().top - i / 2) * (i > o ? o / i : 1), s = Math.round((Math.atan2(r, n) * (180 / Math.PI) + 180) / 90 + 3) % 4; return s }, _getStyle: function (t) { var e, o, i = { left: "0px", top: "-100%" }, n = { left: "0px", top: "100%" }, r = { left: "-100%", top: "0px" }, s = { left: "100%", top: "0px" }, a = { top: "0px" }, h = { left: "0px" }; switch (t) { case 0: e = this.options.inverse ? n : i, o = a; break; case 1: e = this.options.inverse ? r : s, o = h; break; case 2: e = this.options.inverse ? i : n, o = a; break; case 3: e = this.options.inverse ? s : r, o = h } return { from: e, to: o } }, _applyAnimation: function (e, o, i) { t.fn.applyStyle = this.support ? t.fn.css : t.fn.animate, e.stop().applyStyle(o, t.extend(!0, [], { duration: i + "ms" })) } }; var o = function (t) { e.console && e.console.error(t) }; t.fn.hoverdir = function (e) { var i = t.data(this, "hoverdir"); if ("string" == typeof e) { var n = Array.prototype.slice.call(arguments, 1); this.each(function () { return i ? t.isFunction(i[e]) && "_" !== e.charAt(0) ? void i[e].apply(i, n) : void o("no such method '" + e + "' for hoverdir instance") : void o("cannot call methods on hoverdir prior to initialization; attempted to call method '" + e + "'") }) } else this.each(function () { i ? i.s_init() : i = t.data(this, "hoverdir", new t.HoverDir(e, this)) }); return i } }(jQuery, window);

/*!
 * jQuery One Page Nav Plugin
 */
!function (t, n, i) { var s = function (s, e) { this.elem = s, this.$elem = t(s), this.options = e, this.metadata = this.$elem.data("plugin-options"), this.$win = t(n), this.sections = {}, this.didScroll = !1, this.$doc = t(i), this.docHeight = this.$doc.height() }; s.prototype = { defaults: { navItems: "a", currentClass: "current", changeHash: !1, easing: "swing", filter: "", scrollSpeed: 750, scrollThreshold: .5, begin: !1, end: !1, scrollChange: !1 }, init: function () { return this.config = t.extend({}, this.defaults, this.options, this.metadata), this.$nav = this.$elem.find(this.config.navItems), this.$nav = t.merge(this.$nav, t("a.one-page-nav")), "" !== this.config.filter && (this.$nav = this.$nav.filter(this.config.filter)), this.$nav.on("click.onePageNav", t.proxy(this.handleClick, this)), this.getPositions(), this.bindInterval(), this.$win.on("resize.onePageNav", t.proxy(this.getPositions, this)), this }, adjustNav: function (t, n) { t.$elem.find("." + t.config.currentClass).removeClass(t.config.currentClass), n.addClass(t.config.currentClass) }, bindInterval: function () { var t, n = this; n.$win.on("scroll.onePageNav", function () { n.didScroll = !0 }), n.t = setInterval(function () { t = n.$doc.height(), n.didScroll && (n.didScroll = !1, n.scrollChange()), t !== n.docHeight && (n.docHeight = t, n.getPositions()) }, 250) }, getHash: function (t) { return t.attr("href").split("#")[1] }, getPositions: function () { var n, i, s, e = this; e.$nav.each(function () { n = e.getHash(t(this)), s = t("#" + n), s.length && (i = s.offset().top, e.sections[n] = Math.round(i)) }) }, getSection: function (t) { var n = null, i = Math.round(this.$win.height() * this.config.scrollThreshold); for (var s in this.sections) this.sections[s] - i < t && (n = s); return n }, handleClick: function (i) { var s = this, e = t(i.currentTarget), o = e.parent(), a = "#" + s.getHash(e); o.hasClass(s.config.currentClass) || (s.config.begin && s.config.begin(), s.adjustNav(s, o), s.unbindInterval(), s.scrollTo(a, function () { s.config.changeHash && (n.location.hash = a), s.bindInterval(), s.config.end && s.config.end() })), i.preventDefault() }, scrollChange: function () { var t, n = this.$win.scrollTop(), i = this.getSection(n); null !== i && (t = this.$elem.find('a[href$="#' + i + '"]').parent(), t.hasClass(this.config.currentClass) || (this.adjustNav(this, t), this.config.scrollChange && this.config.scrollChange(t))) }, scrollTo: function (n, i) { var s = t(n).offset().top; t("html, body").animate({ scrollTop: Math.max(0, s - 60) }, this.config.scrollSpeed, this.config.easing, i) }, unbindInterval: function () { clearInterval(this.t), this.$win.unbind("scroll.onePageNav") } }, s.defaults = s.prototype.defaults, t.fn.onePageNav = function (t) { return this.each(function () { new s(this, t).init() }) } }(jQuery, window, document);

/*! 
 *   scroll
 */
!function () { var t = jQuery.event.special, e = "D" + +new Date, n = "D" + (+new Date + 1); t.scrollstart = { setup: function () { var n, s = function (e) { var s = this, l = arguments; n ? clearTimeout(n) : (e.type = "scrollstart", jQuery.event.dispatch.apply(s, l)), n = setTimeout(function () { n = null }, t.scrollstop.latency) }; jQuery(this).bind("scroll", s).data(e, s) }, teardown: function () { jQuery(this).unbind("scroll", jQuery(this).data(e)) } }, t.scrollstop = { latency: 300, setup: function () { var e, s = function (n) { var s = this, l = arguments; e && clearTimeout(e), e = setTimeout(function () { e = null, n.type = "scrollstop", jQuery.event.dispatch.apply(s, l) }, t.scrollstop.latency) }; jQuery(this).bind("scroll", s).data(n, s) }, teardown: function () { jQuery(this).unbind("scroll", jQuery(this).data(n)) } } }();

/*! 
 *   Lazy
 */
!function (e, t, i, o) { var n = e(t); e.fn.lazyload = function (r) { function f() { var t = 0; l.each(function () { var i = e(this); if (!h.skip_invisible || i.is(":visible")) if (e.abovethetop(this, h) || e.leftofbegin(this, h)); else if (e.belowthefold(this, h) || e.rightoffold(this, h)) { if (++t > h.failure_limit) return !1 } else i.trigger("appear"), t = 0 }) } var a, l = this, h = { threshold: 0, failure_limit: 0, event: "scroll", effect: "show", container: t, data_attribute: "src", skip_invisible: !0, appear: null, load: null, placeholder: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAANSURBVBhXYzh8+PB/AAffA0nNPuCLAAAAAElFTkSuQmCC" }; return r && (o !== r.failurelimit && (r.failure_limit = r.failurelimit, delete r.failurelimit), o !== r.effectspeed && (r.effect_speed = r.effectspeed, delete r.effectspeed), e.extend(h, r)), a = h.container === o || h.container === t ? n : e(h.container), 0 === h.event.indexOf("scroll") && a.bind(h.event, function () { return f() }), this.each(function () { var t = this, i = e(t); t.loaded = !1, (i.attr("src") === o || i.attr("src") === !1) && i.is("img") && i.attr("src", h.placeholder), i.one("appear", function () { if (!this.loaded) { if (h.appear) { var o = l.length; h.appear.call(t, o, h) } e("<img />").bind("load", function () { var o = i.attr("data-" + h.data_attribute); i.hide(), i.is("img") ? i.attr("src", o) : i.css("background-image", "url('" + o + "')"), i[h.effect](h.effect_speed), t.loaded = !0; var n = e.grep(l, function (e) { return !e.loaded }); if (l = e(n), h.load) { var r = l.length; h.load.call(t, r, h) } }).attr("src", i.attr("data-" + h.data_attribute)) } }), 0 !== h.event.indexOf("scroll") && i.bind(h.event, function () { t.loaded || i.trigger("appear") }) }), n.bind("resize", function () { f() }), /(?:iphone|ipod|ipad).*os 5/gi.test(navigator.appVersion) && n.bind("pageshow", function (t) { t.originalEvent && t.originalEvent.persisted && l.each(function () { e(this).trigger("appear") }) }), e(i).ready(function () { f() }), this }, e.belowthefold = function (i, r) { var f; return f = r.container === o || r.container === t ? (t.innerHeight ? t.innerHeight : n.height()) + n.scrollTop() : e(r.container).offset().top + e(r.container).height(), f <= e(i).offset().top - r.threshold }, e.rightoffold = function (i, r) { var f; return f = r.container === o || r.container === t ? n.width() + n.scrollLeft() : e(r.container).offset().left + e(r.container).width(), f <= e(i).offset().left - r.threshold }, e.abovethetop = function (i, r) { var f; return f = r.container === o || r.container === t ? n.scrollTop() : e(r.container).offset().top, f >= e(i).offset().top + r.threshold + e(i).height() }, e.leftofbegin = function (i, r) { var f; return f = r.container === o || r.container === t ? n.scrollLeft() : e(r.container).offset().left, f >= e(i).offset().left + r.threshold + e(i).width() }, e.inviewport = function (t, i) { return !(e.rightoffold(t, i) || e.leftofbegin(t, i) || e.belowthefold(t, i) || e.abovethetop(t, i)) }, e.extend(e.expr[":"], { "below-the-fold": function (t) { return e.belowthefold(t, { threshold: 0 }) }, "above-the-top": function (t) { return !e.belowthefold(t, { threshold: 0 }) }, "right-of-screen": function (t) { return e.rightoffold(t, { threshold: 0 }) }, "left-of-screen": function (t) { return !e.rightoffold(t, { threshold: 0 }) }, "in-viewport": function (t) { return e.inviewport(t, { threshold: 0 }) }, "above-the-fold": function (t) { return !e.belowthefold(t, { threshold: 0 }) }, "right-of-fold": function (t) { return e.rightoffold(t, { threshold: 0 }) }, "left-of-fold": function (t) { return !e.rightoffold(t, { threshold: 0 }) } }) }(jQuery, window, document);



/*!
 * Theia Sticky Sidebar v1.2.2
 *
 * Glues your website's sidebars, making them permanently visible while scrolling.
 *
 * Copyright 2013-2014 WeCodePixels and other contributors
 * Released under the MIT license
 */
!function (i) { i.fn.theiaStickySidebar = function (t) { var o = { containerSelector: "", additionalMarginTop: 0, additionalMarginBottom: 0, updateSidebarHeight: !0, minWidth: 0 }; t = i.extend(o, t), t.additionalMarginTop = parseInt(t.additionalMarginTop) || 0, t.additionalMarginBottom = parseInt(t.additionalMarginBottom) || 0, i("head").append(i('<style>.theiaStickySidebar:after {content: ""; display: table; clear: both;}</style>')), this.each(function () { function o() { e.fixedScrollTop = 0, e.sidebar.css({ "min-height": "1px" }), e.stickySidebar.css({ position: "static", width: "" }) } function a(t) { var o = t.height(); return t.children().each(function () { o = Math.max(o, i(this).height()) }), o } var e = {}; e.sidebar = i(this), e.options = t || {}, e.container = i(e.options.containerSelector), 0 == e.container.size() && (e.container = e.sidebar.parent()), e.sidebar.parents().css("-webkit-transform", "none"), e.sidebar.css({ position: "relative", overflow: "visible", "-webkit-box-sizing": "border-box", "-moz-box-sizing": "border-box", "box-sizing": "border-box" }), e.stickySidebar = e.sidebar.find(".theiaStickySidebar"), 0 == e.stickySidebar.length && (e.sidebar.find("script").remove(), e.stickySidebar = i("<div>").addClass("theiaStickySidebar").append(e.sidebar.children()), e.sidebar.append(e.stickySidebar)), e.marginTop = parseInt(e.sidebar.css("margin-top")), e.marginBottom = parseInt(e.sidebar.css("margin-bottom")), e.paddingTop = parseInt(e.sidebar.css("padding-top")), e.paddingBottom = parseInt(e.sidebar.css("padding-bottom")); var d = e.stickySidebar.offset().top, r = e.stickySidebar.outerHeight(); e.stickySidebar.css("padding-top", 1), e.stickySidebar.css("padding-bottom", 1), d -= e.stickySidebar.offset().top, r = e.stickySidebar.outerHeight() - r - d, 0 == d ? (e.stickySidebar.css("padding-top", 0), e.stickySidebarPaddingTop = 0) : e.stickySidebarPaddingTop = 1, 0 == r ? (e.stickySidebar.css("padding-bottom", 0), e.stickySidebarPaddingBottom = 0) : e.stickySidebarPaddingBottom = 1, e.previousScrollTop = null, e.fixedScrollTop = 0, o(), e.onScroll = function (e) { if (e.stickySidebar.is(":visible")) { if (i("body").width() < e.options.minWidth) return void o(); if (e.sidebar.outerWidth(!0) + 50 > e.container.width()) return void o(); var d = i(document).scrollTop(), r = "static"; if (d >= e.container.offset().top + (e.paddingTop + e.marginTop - e.options.additionalMarginTop)) { var n, s = e.paddingTop + e.marginTop + t.additionalMarginTop, c = e.paddingBottom + e.marginBottom + t.additionalMarginBottom, p = e.container.offset().top, b = e.container.offset().top + a(e.container), g = 0 + t.additionalMarginTop, l = e.stickySidebar.outerHeight() + s + c < i(window).height(); n = l ? g + e.stickySidebar.outerHeight() : i(window).height() - e.marginBottom - e.paddingBottom - t.additionalMarginBottom; var f = p - d + e.paddingTop + e.marginTop, S = b - d - e.paddingBottom - e.marginBottom, h = e.stickySidebar.offset().top - d, m = e.previousScrollTop - d; "fixed" == e.stickySidebar.css("position") && (h += m), h = m > 0 ? Math.min(h, g) : Math.max(h, n - e.stickySidebar.outerHeight()), h = Math.max(h, f), h = Math.min(h, S - e.stickySidebar.outerHeight()); var y = e.container.height() == e.stickySidebar.outerHeight(); r = (y || h != g) && (y || h != n - e.stickySidebar.outerHeight()) ? d + h - e.sidebar.offset().top - e.paddingTop <= t.additionalMarginTop ? "static" : "absolute" : "fixed" } if ("fixed" == r) e.stickySidebar.css({ position: "fixed", width: e.sidebar.width(), top: h, left: e.sidebar.offset().left + parseInt(e.sidebar.css("padding-left")) }); else if ("absolute" == r) { var k = {}; "absolute" != e.stickySidebar.css("position") && (k.position = "absolute", k.top = d + h - e.sidebar.offset().top - e.stickySidebarPaddingTop - e.stickySidebarPaddingBottom), k.width = e.sidebar.width(), k.left = "", e.stickySidebar.css(k) } else "static" == r && o(); "static" != r && 1 == e.options.updateSidebarHeight && e.sidebar.css({ "min-height": e.stickySidebar.outerHeight() + e.stickySidebar.offset().top - e.sidebar.offset().top + e.paddingBottom }), e.previousScrollTop = d } }, e.onScroll(e), i(document).scroll(function (i) { return function () { i.onScroll(i) } }(e)), i(window).resize(function (i) { return function () { i.stickySidebar.css({ position: "static" }), i.onScroll(i) } }(e)) }) } }(jQuery);