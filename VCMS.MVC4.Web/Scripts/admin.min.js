$.navAsAjax = true;
var throttle_delay = 350,
    menu_speed = 235;
$.root_ = $("body"), $.intervalArr = [];
var calc_navbar_height = function() {
        var e = null;
        return $("#header").length && (e = $("#header").height()), null === e && (e = $('<div id="header"></div>').height()), null === e ? 49 : e
    },
    navbar_height = calc_navbar_height,
    topmenu = !1,
    thisDevice = null,
    ismobile = /iphone|ipad|ipod|android|blackberry|mini|windows\sce|palm/i.test(navigator.userAgent.toLowerCase()),
    jsArray = {},
    initApp = function(e) {
        return e.addDeviceType = function() {
            return ismobile ? ($.root_.addClass("mobile-detected"), thisDevice = "mobile", fastClick ? ($.root_.addClass("needsclick"), FastClick.attach(document.body), !1) : void 0) : ($.root_.addClass("desktop-detected"), thisDevice = "desktop", !1)
        }, e.menuPos = function() {
            ($.root_.hasClass("menu-on-top") || "top" == localStorage.getItem("sm-setmenu")) && (topmenu = !0, $.root_.addClass("menu-on-top"))
        }, e.SmartActions = function() {
            var e = {
                launchFullscreen: function(e) {
                    $.root_.hasClass("full-screen") ? ($.root_.removeClass("full-screen"), document.exitFullscreen ? document.exitFullscreen() : document.mozCancelFullScreen ? document.mozCancelFullScreen() : document.webkitExitFullscreen && document.webkitExitFullscreen()) : ($.root_.addClass("full-screen"), e.requestFullscreen ? e.requestFullscreen() : e.mozRequestFullScreen ? e.mozRequestFullScreen() : e.webkitRequestFullscreen ? e.webkitRequestFullscreen() : e.msRequestFullscreen && e.msRequestFullscreen())
                },
                minifyMenu: function(e) {
                    $.root_.hasClass("menu-on-top") || ($.root_.toggleClass("minified"), $.root_.removeClass("hidden-menu"), $("html").removeClass("hidden-menu-mobile-lock"))
                },
                toggleMenu: function() {
                    $.root_.hasClass("menu-on-top") ? $.root_.hasClass("menu-on-top") && $.root_.hasClass("mobile-view-activated") && ($("html").toggleClass("hidden-menu-mobile-lock"), $.root_.toggleClass("hidden-menu"), $.root_.removeClass("minified")) : ($("html").toggleClass("hidden-menu-mobile-lock"), $.root_.toggleClass("hidden-menu"), $.root_.removeClass("minified"))
                }
            };
            $.root_.on("click", '[data-action="userLogout"]', function(t) {
                var n = $(this);
                e.userLogout(n), t.preventDefault(), n = null
            }), $.root_.on("click", '[data-action="launchFullscreen"]', function(t) {
                e.launchFullscreen(document.documentElement), t.preventDefault()
            }), $.root_.on("click", '[data-action="minifyMenu"]', function(t) {
                var n = $(this);
                e.minifyMenu(n), t.preventDefault(), n = null
            }), $.root_.on("click", '[data-action="toggleMenu"]', function(t) {
                e.toggleMenu(), t.preventDefault()
            }), $.root_.on("click", '[data-action="toggleShortcut"]', function(t) {
                e.toggleShortcut(), t.preventDefault()
            })
        }, e.leftNav = function() {
            topmenu || $("nav ul").jarvismenu({
                accordion: !0,
                speed: menu_speed,
                closedSign: '<em class="fa fa-plus-square-o"></em>',
                openedSign: '<em class="fa fa-minus-square-o"></em>'
            })
        }, e.domReadyMisc = function() {
            $("[rel=tooltip]").length && $("[rel=tooltip]").tooltip(), $("#search-mobile").click(function() {
                $.root_.addClass("search-mobile")
            }), $("#cancel-search-js").click(function() {
                $.root_.removeClass("search-mobile")
            })
        }, e
    }({});
initApp.addDeviceType(), initApp.menuPos(), jQuery(document).ready(function() {
    initApp.SmartActions(), initApp.leftNav(), initApp.domReadyMisc()
}), $("#main").resize(function() {
    $(window).width() < 979 ? ($.root_.addClass("mobile-view-activated"), $.root_.removeClass("minified")) : $.root_.hasClass("mobile-view-activated") && $.root_.removeClass("mobile-view-activated")
});
if ($.fn.extend({
    jarvismenu: function(e) {
        var t = {
                accordion: "true",
                speed: 200,
                closedSign: "[+]",
                openedSign: "[-]"
            },
            n = $.extend(t, e),
            r = $(this);
        r.find("li").each(function() {
            0 !== $(this).find("ul").size() && ($(this).find("a:first").append("<b class='collapse-sign'>" + n.closedSign + "</b>"), "#" == $(this).find("a:first").attr("href") && $(this).find("a:first").click(function() {
                return !1
            }))
}),
    r.find("li.active").each(function () {
            $(this).parents("ul").slideDown(n.speed), $(this).parents("ul").parent("li").find("b:first").html(n.openedSign), $(this).parents("ul").parent("li").addClass("open")
})
    //r.find("li a").click(function () {
    //        0 !== $(this).parent().find("ul").size() && (n.accordion && ($(this).parent().find("ul").is(":visible") || (parents = $(this).parent().parents("ul"), visible = r.find("ul:visible"), visible.each(function(e) {
    //            var t = !0;
    //            parents.each(function(n) {
    //                return parents[n] == visible[e] ? (t = !1, !1) : void 0
    //            }), t && $(this).parent().find("ul") != visible[e] && $(visible[e]).slideUp(n.speed, function() {
    //                $(this).parent("li").find("b:first").html(n.closedSign), $(this).parent("li").removeClass("open")
    //            })
    //        }))), $(this).parent().find("ul:first").is(":visible") && !$(this).parent().find("ul:first").hasClass("active") ? $(this).parent().find("ul:first").slideUp(n.speed, function() {
    //            $(this).parent("li").removeClass("open"), $(this).parent("li").find("b:first").delay(n.speed).html(n.closedSign)
    //        }) : $(this).parent().find("ul:first").slideDown(n.speed, function() {
    //            $(this).parent("li").addClass("open"), $(this).parent("li").find("b:first").delay(n.speed).html(n.openedSign)
    //        }))
    //    })
    }
}), jQuery.fn.doesExist = function() {
    return jQuery(this).length > 0
});
//$.navAsAjax && ($("nav").length, $(document).on("click", 'nav a[href!="#"]', function (a) {
//    a.preventDefault();
//    var b = $(a.currentTarget);
//    b.parent().hasClass("active") || b.attr("target") || ($.root_.hasClass("mobile-view-activated") ? ($.root_.removeClass("hidden-menu"), $("html").removeClass("hidden-menu-mobile-lock"), window.setTimeout(function () {
//        window.location.search ? window.location.href = window.location.href.replace(window.location.search, "").replace(window.location.hash, "") + "#" + b.attr("href") : window.location.hash = b.attr("href")
//    }, 150)) : window.location.search ? window.location.href = window.location.href.replace(window.location.search, "").replace(window.location.hash, "") + "#" + b.attr("href") : window.location.hash = b.attr("href"))
//})), $("body").on("click", function (a) {
//    $('[rel="popover"]').each(function () {
//        $(this).is(a.target) || 0 !== $(this).has(a.target).length || 0 !== $(".popover").has(a.target).length || $(this).popover("hide")
//    })
//});