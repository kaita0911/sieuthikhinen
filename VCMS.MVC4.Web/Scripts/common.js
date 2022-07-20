jQuery(window).ready(function () {
    VienNam();
});

function zeroFill(num, len) {
    return (Array(len).join("0") + num).slice(-len);
}

function VienNam() {
    jQuery.browserDetect();
    _animate();
    _owl_carousel();
    _parallax();
    _masonry();
    _lightbox();
    _toggle();
    jQuery.Common();
    _Sidebars();
    Colorbox();
    TopDown();
    ///* --- */
    if (jQuery("html").hasClass("chrome") && jQuery("body").hasClass("smoothscroll")) {
        jQuery.smoothScroll();
    }
    jQuery("li.list-toggle").bind("click", function () {
        jQuery(this).toggleClass("active");
    });
    /* <![CDATA[ */
    jQuery(".iconsPreview a").bind("click", function (e) {
        window.prompt("Copy to clipboard: Ctrl+C, Enter/Esc", jQuery("i", this).attr('class'));
        e.preventDefault();
    });
    /* ]]> */
    /** Bootstrap Tooltip **/
    jQuery("[data-toggle=tooltip]").tooltip();
    $(document.body).on('click', '#pav-mainnav [data-toggle="dropdown"], #menu-offcanvas [data-toggle="dropdown"]', function () {
        if (!$(this).parent().hasClass('open') && this.href && this.href != '#') {
            window.location.href = this.href;
        }
    });

    window.topNavSmall = false;
    jQuery(window).scroll(function () {
        Fixtop();
    });
    var w = 640;
    var h = 445;
    var left = (screen.width / 2) - (w / 2);
    var top = (screen.height / 2) - (h / 2);
    //var message = encodeURIComponent(location.href);
    /* share twitter
    **************************************************************************/
    $("a#twitter").click(function (message) {
        if (typeof message === 'undefined')
            message = encodeURIComponent(location.href);
        window.open('https://twitter.com/intent/tweet?text=' + message, 'sharertwt', 'toolbar=0,status=0,width=640,height=445,top=' + top + ', left=' + left);
        return false;
    });

    /* share facebook
    **************************************************************************/
    $("a#facebook").click(function (message) {
        var w = 640;
        var h = 368;
        var left = (screen.width / 2) - (w / 2);
        var top = (screen.height / 2) - (h / 2);
        window.open('http://www.facebook.com/sharer.php?u=' + encodeURIComponent(location.href), 'sharer', 'toolbar=0,status=0,width=660,height=368,top=' + top + ', left=' + left);
        return false;
    });

    /* share google
    **************************************************************************/
    $("a#google").click(function (message) {
        window.open('https://plus.google.com/share?url=' + encodeURIComponent(location.href), '_blank');
        return false;
    });

    /* share pinterest
    **************************************************************************/
    $("a#pinterest").click(function (message) {
        window.open('http://www.pinterest.com/pin/create/button/?url=' + encodeURIComponent(location.href), 'sharerpinterest', 'toolbar=0,status=0,width=660,height=445,top=' + top + ', left=' + left);
        return false;
    });
    /* share linkedin
    **************************************************************************/
    $("a#linkedin").click(function (message) {
        var w = 840;
        var h = 500;
        var left = (screen.width / 2) - (w / 2);
        var top = (screen.height / 2) - (h / 2);
        window.open('http://www.linkedin.com/shareArticle?mini=true&amp;&amp;summary=Put your summary here&url=' + encodeURIComponent(message), 'sharerpinterest', 'toolbar=0,status=0,width=840,height=500,top=' + top + ', left=' + left);
        return false;
    });
    jQuery("#accordion li a.subcart").each(function () {
        var _t = $(this);
        var _p = _t.closest("li");
        _t.click(function () {
            _p.toggleClass("active");
        });
    });

    jQuery('#accordion').find('a.active').parents("li").addClass("active");
    jQuery('#accordion').find('li.active').find("ul.collapse ").addClass("in");
    jQuery('#accordion').find('li.active').find("a.subcart").toggleClass("collapsed");
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
                $(document).ready(function () {

                    var $mcontent = $('#pav-mainnav .navbar .navbar-nav');

                    var effect = 3;

                    var $offcmenu = $('<nav id="menu-offcanvas" class="offcanvas-menu offcanvas-effect-' + effect + ' hidden-lg"><div class="menu-offcanvas-inner"></div></nav>');
                    $(".menu-offcanvas-inner", $offcmenu).append($mcontent.clone());
                    $("body").append($offcmenu);
                    $(".navbar-nav", $offcmenu).removeClass("navbar-nav").removeClass("nav").addClass("menu-offcanvas-content");
                    $(".menu-offcanvas-inner").append("<div class='button-close-menu'><i class='fa fa-times-circle'></i></div>");

                    var $btn = $("#pav-mainnav .navbar-toggle, .menu-offcanvas-inner .button-close-menu");
                    var eventtype = mobilecheck() ? 'click' : 'click';

                    $($btn).bind(eventtype, function (e) {
                        $("#offcanvas-container").toggleClass("offcanvas-menu-open").addClass("offcanvas-effect-" + effect);
                        $("#page").bind(eventtype, function () {
                            $("#offcanvas-container").toggleClass("offcanvas-menu-open");
                            $("#page").unbind(eventtype);
                        });
                        e.stopPropagation();
                        return false;
                    });
                });
            }
            init();
        }

    });
})(jQuery);
/** Sidebars
 **************************************************************** **/
function _Sidebars() {
    if ($("#columns").hasClass("offcanvas-siderbars")) {
        $('.offcanvas-sidebars-buttons button').hide();
        $(".sidebar").parent().parent().find("section").addClass("main-column");
        $(".sidebar").each(function () {
            $('[data-for="' + $(this).attr("id") + '"]').show();
            $(this).parent().attr("id", "oc-" + $(this).attr("id")).addClass("offcanvas-sidebar");
        });
        $(".offcanvas-sidebars-buttons button").bind("click", function () {
            if ($(this).data("for") == "column-right") {
                $(".offcanvas-siderbars").removeClass("column-left-active");
            } else {
                $(".offcanvas-siderbars").removeClass("column-right-active");
            }
            $(".offcanvas-siderbars").toggleClass($(this).data("for") + "-active");
            $("#oc-" + $(this).data("for")).toggleClass("canvas-show");
        });
    }
}

/** Animate
 **************************************************************** **/
function _animate() {

    // Animation [appear]
    jQuery("[data-animation]").each(function () {
        var _t = jQuery(this);
        var visit = (_t.attr("data-visit") ? _t.attr("data-visit") : 90);
        if (jQuery(window).width() > 767) {
            _t.appear(function () {

                var delay = (_t.attr("data-animation-delay") ? _t.attr("data-animation-delay") : 0.1);

                if (delay > 0.1)
                    _t.css({
                        "-webkit-animation-delay": delay + "s",
                        "-moz-animation-delay": delay + "s",
                        "-ms-animation-delay": delay + "s",
                        "-o-animation-delay": delay + "s",
                        "animation-delay": delay + "s",
                    });
                _t.addClass(_t.attr("data-animation"));

                setTimeout(function () {
                    _t.addClass("animation-visible");
                }, delay);

            }, { accX: 0, accY: parseInt(visit) }); /* -150 */

        } else {
            _t.addClass("animation-visible");

        }

    });

    // Bootstrap Progress Bar
    jQuery("[data-appear-progress-animation]").each(function () {
        var $_t = jQuery(this);

        if (jQuery(window).width() > 767) {

            $_t.appear(function () {
                _delay = 1;

                if ($_t.attr("data-appear-progress-animation-delay")) {
                    _delay = $_t.attr("data-appear-progress-animation-delay");
                }

                if (_delay > 1) {
                    $_t.css("animation-delay", _delay + "ms");
                }

                $_t.addClass($_t.attr("data-appear-progress-animation"));

                setTimeout(function () {

                    $_t.addClass("animation-visible");

                }, _delay);

            }, { accX: 0, accY: -50 }); /* -150 */

        } else {

            $_t.addClass("animation-visible");
        }
    });


    jQuery("[data-appear-progress-animation]").each(function () {
        var $_t = jQuery(this);

        $_t.appear(function () {
            var _delay = ($_t.attr("data-appear-animation-delay") ? $_t.attr("data-appear-animation-delay") : 1);
            if (_delay > 1) {
                $_t.css("animation-delay", _delay + "ms");
            }

            $_t.addClass($_t.attr("data-appear-animation"));
            setTimeout(function () {
                $_t.animate({ "width": $_t.attr("data-appear-progress-animation") }, 1000, function () {
                    $_t.find(".progress-bar-tooltip").animate({ "opacity": 1 }, 500);
                });
                //$_t.animate({ "width": $_t.attr("data-appear-progress-animation") }, 1000);
                //$_t.find(".progress-bar-tooltip").animate({ "opacity": 1 }, 1500);
                //$_t.find(".progress-bar-label").animate({ "opacity": 1 }, 500);
                $_t.addClass("active");
            }, _delay);

        }, { accX: 0, accY: -25 });

    });

    // Count To
    jQuery(".countTo [data-to]").each(function () {
        var $_t = jQuery(this);
        $_t.appear(function () {
            $_t.countTo();
        }, { accX: 0, accY: -50 });
    });
}

/** OWL Carousel
 **************************************************************** **/
function _owl_carousel() {

    var total = jQuery("div.owl-carousel").length,
		count = 0;

    jQuery("div.owl-carousel").each(function () {

        var slider = jQuery(this);
        var options = slider.attr('data-plugin-options');

        // Progress Bar
        var $opt = eval('(' + options + ')');  // convert text to json

        if ($opt.progressBar == 'true') {
            var afterInit = progressBar;
        } else {
            var afterInit = false;
        }

        var defaults = {
            items: 5,
            itemsCustom: false,
            itemsDesktop: [1199, 4],
            itemsDesktopSmall: [980, 3],
            itemsTablet: [768, 2],
            itemsTabletSmall: false,
            itemsMobile: [479, 1],
            singleItem: true,
            itemsScaleUp: false,

            slideSpeed: 200,
            paginationSpeed: 800,
            rewindSpeed: 1000,

            autoPlay: false,
            stopOnHover: false,

            navigation: false,
            navigationText: [
								'<i class="fa fa-chevron-left"></i>',
								'<i class="fa fa-chevron-right"></i>'
            ],
            rewindNav: true,
            scrollPerPage: false,

            pagination: true,
            paginationNumbers: false,

            responsive: true,
            responsiveRefreshRate: 200,
            responsiveBaseWidth: window,

            baseClass: "owl-carousel",
            theme: "owl-theme",

            lazyLoad: false,
            lazyFollow: true,
            lazyEffect: "fade",

            autoHeight: false,

            jsonPath: false,
            jsonSuccess: false,

            dragBeforeAnimFinish: true,
            mouseDrag: true,
            touchDrag: true,

            transitionStyle: false,

            addClassActive: false,

            beforeUpdate: false,
            afterUpdate: false,
            beforeInit: false,
            afterInit: afterInit,
            beforeMove: false,
            afterMove: (afterInit == false) ? false : moved,
            afterAction: false,
            startDragging: false,
            afterLazyLoad: false
        }

        var config = jQuery.extend({}, defaults, options, slider.data("plugin-options"));
        slider.owlCarousel(config).addClass("owl-carousel-init");


        //// Progress Bar
        //var elem = jQuery(this);

        ////Init progressBar where elem is $("#owl-demo")
        //function progressBar(elem) {
        //    $elem = elem;
        //    //build progress bar elements
        //    buildProgressBar();
        //    //start counting
        //    start();
        //}

        ////create div#progressBar and div#bar then prepend to $("#owl-demo")
        //function buildProgressBar() {
        //    $progressBar = $("<div>", {
        //        id: "progressBar"
        //    });
        //    $bar = $("<div>", {
        //        id: "bar"
        //    });
        //    $progressBar.append($bar).prependTo($elem);
        //}

        //function start() {
        //    //reset timer
        //    percentTime = 0;
        //    isPause = false;
        //    //run interval every 0.01 second
        //    tick = setInterval(interval, 10);
        //};


        //var time = 7; // time in seconds
        //function interval() {
        //    if (isPause === false) {
        //        percentTime += 1 / time;
        //        $bar.css({
        //            width: percentTime + "%"
        //        });
        //        //if percentTime is equal or greater than 100
        //        if (percentTime >= 100) {
        //            //slide to next item 
        //            $elem.trigger('owl.next')
        //        }
        //    }
        //}

        ////pause while dragging 
        //function pauseOnDragging() {
        //    isPause = true;
        //}

        ////moved callback
        //function moved() {
        //    //clear interval
        //    clearTimeout(tick);
        //    //start again
        //    start();
        //}

    });
}

/** Masonry Filter
 **************************************************************** **/
function _masonry() {

    jQuery(window).load(function () {
        jQuery("span.js_loader").remove();
        jQuery("li.masonry-item").addClass('fadeIn');
        jQuery(".masonry-list").each(function () {
            var _c = jQuery(this);
            _c.waitForImages(function () { // waitForImages Plugin - bottom of this file
                _c.masonry({
                    itemSelector: '.masonry-item'
                });
            });
        });
    });

    jQuery("ul.isotope-filter").each(function () {

        var _el = jQuery(this),
			destination = jQuery("ul.sort-destination[data-sort-id=" + jQuery(this).attr("data-sort-id") + "]");

        if (destination.get(0)) {
            jQuery(window).load(function () {
                destination.isotope({
                    itemSelector: "li",
                    layoutMode: 'sloppyMasonry'
                });

                _el.find("a").click(function (e) {
                    e.preventDefault();
                    var $_t = jQuery(this),
						sortId = $_t.parents(".sort-source").attr("data-sort-id"),
						filter = $_t.parent().attr("data-option-value");
                    _el.find("li.active").removeClass("active");
                    $_t.parent().addClass("active");
                    destination.isotope({
                        filter: filter
                    });
                    jQuery(".sort-source-title[data-sort-id=" + sortId + "] strong").html($_t.html());
                    return false;
                });
            });
        }
    });


    jQuery(window).load(function () {

        jQuery("ul.isotope").addClass('fadeIn');

    });
}

/** 05. LightBox
 **************************************************************** **/
function _lightbox() {

    if (typeof (jQuery.magnificPopup) == "undefined") {
        return false;
    }

    jQuery.extend(true, jQuery.magnificPopup.defaults, {
        tClose: 'Close',
        tLoading: 'Loading...',

        gallery: {
            tPrev: 'Previous',
            tNext: 'Next',
            tCounter: '%curr% / %total%'
        },

        image: {
            tError: 'Image not loaded!'
        },

        ajax: {
            tError: 'Content not loaded!'
        }
    });

    jQuery(".lightbox").each(function () {

        var _t = jQuery(this),
			options = _t.attr('data-plugin-options'),
			config = {},
			defaults = {
			    type: 'image',
			    fixedContentPos: false,
			    fixedBgPos: false,
			    mainClass: 'mfp-no-margins mfp-with-zoom',
			    image: {
			        verticalFit: true
			    },

			    zoom: {
			        enabled: false,
			        duration: 300
			    },

			    gallery: {
			        enabled: true,
			        navigateByImgClick: true,
			        preload: [0, 1],
			        arrowMarkup: '<button title="%title%" type="button" class="mfp-arrow mfp-arrow-%dir%"></button>',
			        tPrev: 'Previou',
			        tNext: 'Next',
			        tCounter: '<span class="mfp-counter">%curr% / %total%</span>'
			    },
			};

        if (_t.data("plugin-options")) {
            config = jQuery.extend({}, defaults, options, _t.data("plugin-options"));
        }

        jQuery(this).magnificPopup(config);

    });
}


/** COUNT TO
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
(function ($) {
    $.extend({

        smoothScroll: function () {

            // Scroll Variables (tweakable)
            var defaultOptions = {

                // Scrolling Core
                frameRate: 60, // [Hz]
                animationTime: 700, // [px]
                stepSize: 120, // [px]

                // Pulse (less tweakable)
                // ratio of "tail" to "acceleration"
                pulseAlgorithm: true,
                pulseScale: 10,
                pulseNormalize: 1,

                // Acceleration
                accelerationDelta: 20,  // 20
                accelerationMax: 1,   // 1

                // Keyboard Settings
                keyboardSupport: true,  // option
                arrowScroll: 50,     // [px]

                // Other
                touchpadSupport: true,
                fixedBackground: true,
                excluded: ""
            };

            var options = defaultOptions;

            // Other Variables
            var isExcluded = false;
            var isFrame = false;
            var direction = { x: 0, y: 0 };
            var initDone = false;
            var root = document.documentElement;
            var activeElement;
            var observer;
            var deltaBuffer = [120, 120, 120];

            var key = {
                left: 37, up: 38, right: 39, down: 40, spacebar: 32,
                pageup: 33, pagedown: 34, end: 35, home: 36
            };


            /***********************************************
			 * INITIALIZE
			 ***********************************************/

            /**
			 * Tests if smooth scrolling is allowed. Shuts down everything if not.
			 */
            function initTest() {

                var disableKeyboard = false;

                // disable keys for google reader (spacebar conflict)
                if (document.URL.indexOf("google.com/reader/view") > -1) {
                    disableKeyboard = true;
                }

                // disable everything if the page is blacklisted
                if (options.excluded) {
                    var domains = options.excluded.split(/[,\n] ?/);
                    domains.push("mail.google.com"); // exclude Gmail for now
                    for (var i = domains.length; i--;) {
                        if (document.URL.indexOf(domains[i]) > -1) {
                            observer && observer.disconnect();
                            removeEvent("mousewheel", wheel);
                            disableKeyboard = true;
                            isExcluded = true;
                            break;
                        }
                    }
                }

                // disable keyboard support if anything above requested it
                if (disableKeyboard) {
                    removeEvent("keydown", keydown);
                }

                if (options.keyboardSupport && !disableKeyboard) {
                    addEvent("keydown", keydown);
                }
            }

            /**
			 * Sets up scrolls array, determines if frames are involved.
			 */
            function init() {

                if (!document.body) return;

                var body = document.body;
                var html = document.documentElement;
                var windowHeight = window.innerHeight;
                var scrollHeight = body.scrollHeight;

                // check compat mode for root element
                root = (document.compatMode.indexOf('CSS') >= 0) ? html : body;
                activeElement = body;

                initTest();
                initDone = true;

                // Checks if this script is running in a frame
                if (top != self) {
                    isFrame = true;
                }

                    /**
                     * This fixes a bug where the areas left and right to
                     * the content does not trigger the onmousewheel event
                     * on some pages. e.g.: html, body { height: 100% }
                     */
                else if (scrollHeight > windowHeight &&
						(body.offsetHeight <= windowHeight ||
						 html.offsetHeight <= windowHeight)) {

                    // DOMChange (throttle): fix height
                    var pending = false;
                    var refresh = function () {
                        if (!pending && html.scrollHeight != document.height) {
                            pending = true; // add a new pending action
                            setTimeout(function () {
                                html.style.height = document.height + 'px';
                                pending = false;
                            }, 500); // act rarely to stay fast
                        }
                    };
                    html.style.height = 'auto';
                    setTimeout(refresh, 10);

                    var config = {
                        attributes: true,
                        childList: true,
                        characterData: false
                    };

                    observer = new MutationObserver(refresh);
                    observer.observe(body, config);

                    // clearfix
                    if (root.offsetHeight <= windowHeight) {
                        var underlay = document.createElement("div");
                        underlay.style.clear = "both";
                        body.appendChild(underlay);
                    }
                }

                // gmail performance fix
                if (document.URL.indexOf("mail.google.com") > -1) {
                    var s = document.createElement("style");
                    s.innerHTML = ".iu { visibility: hidden }";
                    (document.getElementsByTagName("head")[0] || html).appendChild(s);
                }
                    // facebook better home timeline performance
                    // all the HTML resized images make rendering CPU intensive
                else if (document.URL.indexOf("www.facebook.com") > -1) {
                    var home_stream = document.getElementById("home_stream");
                    home_stream && (home_stream.style.webkitTransform = "translateZ(0)");
                }
                // disable fixed background
                if (!options.fixedBackground && !isExcluded) {
                    body.style.backgroundAttachment = "scroll";
                    html.style.backgroundAttachment = "scroll";
                }
            }


            /************************************************
			 * SCROLLING
			 ************************************************/

            var que = [];
            var pending = false;
            var lastScroll = +new Date;

            /**
			 * Pushes scroll actions to the scrolling queue.
			 */
            function scrollArray(elem, left, top, delay) {

                delay || (delay = 1000);
                directionCheck(left, top);

                if (options.accelerationMax != 1) {
                    var now = +new Date;
                    var elapsed = now - lastScroll;
                    if (elapsed < options.accelerationDelta) {
                        var factor = (1 + (30 / elapsed)) / 2;
                        if (factor > 1) {
                            factor = Math.min(factor, options.accelerationMax);
                            left *= factor;
                            top *= factor;
                        }
                    }
                    lastScroll = +new Date;
                }

                // push a scroll command
                que.push({
                    x: left,
                    y: top,
                    lastX: (left < 0) ? 0.99 : -0.99,
                    lastY: (top < 0) ? 0.99 : -0.99,
                    start: +new Date
                });

                // don't act if there's a pending queue
                if (pending) {
                    return;
                }

                var scrollWindow = (elem === document.body);

                var step = function (time) {

                    var now = +new Date;
                    var scrollX = 0;
                    var scrollY = 0;

                    for (var i = 0; i < que.length; i++) {

                        var item = que[i];
                        var elapsed = now - item.start;
                        var finished = (elapsed >= options.animationTime);

                        // scroll position: [0, 1]
                        var position = (finished) ? 1 : elapsed / options.animationTime;

                        // easing [optional]
                        if (options.pulseAlgorithm) {
                            position = pulse(position);
                        }

                        // only need the difference
                        var x = (item.x * position - item.lastX) >> 0;
                        var y = (item.y * position - item.lastY) >> 0;

                        // add this to the total scrolling
                        scrollX += x;
                        scrollY += y;

                        // update last values
                        item.lastX += x;
                        item.lastY += y;

                        // delete and step back if it's over
                        if (finished) {
                            que.splice(i, 1); i--;
                        }
                    }

                    // scroll left and top
                    if (scrollWindow) {
                        window.scrollBy(scrollX, scrollY);
                    }
                    else {
                        if (scrollX) elem.scrollLeft += scrollX;
                        if (scrollY) elem.scrollTop += scrollY;
                    }

                    // clean up if there's nothing left to do
                    if (!left && !top) {
                        que = [];
                    }

                    if (que.length) {
                        requestFrame(step, elem, (delay / options.frameRate + 1));
                    } else {
                        pending = false;
                    }
                };

                // start a new queue of actions
                requestFrame(step, elem, 0);
                pending = true;
            }


            /***********************************************
			 * EVENTS
			 ***********************************************/

            /**
			 * Mouse wheel handler.
			 * @param {Object} event
			 */
            function wheel(event) {

                if (!initDone) {
                    init();
                }

                var target = event.target;
                var overflowing = overflowingAncestor(target);

                // use default if there's no overflowing
                // element or default action is prevented
                if (!overflowing || event.defaultPrevented ||
					isNodeName(activeElement, "embed") ||
				   (isNodeName(target, "embed") && /\.pdf/i.test(target.src))) {
                    return true;
                }

                var deltaX = event.wheelDeltaX || 0;
                var deltaY = event.wheelDeltaY || 0;

                // use wheelDelta if deltaX/Y is not available
                if (!deltaX && !deltaY) {
                    deltaY = event.wheelDelta || 0;
                }

                // check if it's a touchpad scroll that should be ignored
                if (!options.touchpadSupport && isTouchpad(deltaY)) {
                    return true;
                }

                // scale by step size
                // delta is 120 most of the time
                // synaptics seems to send 1 sometimes
                if (Math.abs(deltaX) > 1.2) {
                    deltaX *= options.stepSize / 120;
                }
                if (Math.abs(deltaY) > 1.2) {
                    deltaY *= options.stepSize / 120;
                }

                scrollArray(overflowing, -deltaX, -deltaY);
                event.preventDefault();
            }

            /**
			 * Keydown event handler.
			 * @param {Object} event
			 */
            function keydown(event) {

                var target = event.target;
                var modifier = event.ctrlKey || event.altKey || event.metaKey ||
							  (event.shiftKey && event.keyCode !== key.spacebar);

                // do nothing if user is editing text
                // or using a modifier key (except shift)
                // or in a dropdown
                if (/input|textarea|select|embed/i.test(target.nodeName) ||
					 target.isContentEditable ||
					 event.defaultPrevented ||
					 modifier) {
                    return true;
                }
                // spacebar should trigger button press
                if (isNodeName(target, "button") &&
					event.keyCode === key.spacebar) {
                    return true;
                }

                var shift, x = 0, y = 0;
                var elem = overflowingAncestor(activeElement);
                var clientHeight = elem.clientHeight;

                if (elem == document.body) {
                    clientHeight = window.innerHeight;
                }

                switch (event.keyCode) {
                    case key.up:
                        y = -options.arrowScroll;
                        break;
                    case key.down:
                        y = options.arrowScroll;
                        break;
                    case key.spacebar: // (+ shift)
                        shift = event.shiftKey ? 1 : -1;
                        y = -shift * clientHeight * 0.9;
                        break;
                    case key.pageup:
                        y = -clientHeight * 0.9;
                        break;
                    case key.pagedown:
                        y = clientHeight * 0.9;
                        break;
                    case key.home:
                        y = -elem.scrollTop;
                        break;
                    case key.end:
                        var damt = elem.scrollHeight - elem.scrollTop - clientHeight;
                        y = (damt > 0) ? damt + 10 : 0;
                        break;
                    case key.left:
                        x = -options.arrowScroll;
                        break;
                    case key.right:
                        x = options.arrowScroll;
                        break;
                    default:
                        return true; // a key we don't care about
                }

                scrollArray(elem, x, y);
                event.preventDefault();
            }

            /**
			 * Mousedown event only for updating activeElement
			 */
            function mousedown(event) {
                activeElement = event.target;
            }


            /***********************************************
			 * OVERFLOW
			 ***********************************************/

            var cache = {}; // cleared out every once in while
            setInterval(function () { cache = {}; }, 10 * 1000);

            var uniqueID = (function () {
                var i = 0;
                return function (el) {
                    return el.uniqueID || (el.uniqueID = i++);
                };
            })();

            function setCache(elems, overflowing) {
                for (var i = elems.length; i--;)
                    cache[uniqueID(elems[i])] = overflowing;
                return overflowing;
            }

            function overflowingAncestor(el) {
                var elems = [];
                var rootScrollHeight = root.scrollHeight;
                do {
                    var cached = cache[uniqueID(el)];
                    if (cached) {
                        return setCache(elems, cached);
                    }
                    elems.push(el);
                    if (rootScrollHeight === el.scrollHeight) {
                        if (!isFrame || root.clientHeight + 10 < rootScrollHeight) {
                            return setCache(elems, document.body); // scrolling root in WebKit
                        }
                    } else if (el.clientHeight + 10 < el.scrollHeight) {
                        overflow = getComputedStyle(el, "").getPropertyValue("overflow-y");
                        if (overflow === "scroll" || overflow === "auto") {
                            return setCache(elems, el);
                        }
                    }
                } while (el = el.parentNode);
            }


            /***********************************************
			 * HELPERS
			 ***********************************************/

            function addEvent(type, fn, bubble) {
                window.addEventListener(type, fn, (bubble || false));
            }

            function removeEvent(type, fn, bubble) {
                window.removeEventListener(type, fn, (bubble || false));
            }

            function isNodeName(el, tag) {
                return (el.nodeName || "").toLowerCase() === tag.toLowerCase();
            }

            function directionCheck(x, y) {
                x = (x > 0) ? 1 : -1;
                y = (y > 0) ? 1 : -1;
                if (direction.x !== x || direction.y !== y) {
                    direction.x = x;
                    direction.y = y;
                    que = [];
                    lastScroll = 0;
                }
            }

            var deltaBufferTimer;

            function isTouchpad(deltaY) {
                if (!deltaY) return;
                deltaY = Math.abs(deltaY)
                deltaBuffer.push(deltaY);
                deltaBuffer.shift();
                clearTimeout(deltaBufferTimer);
                var allEquals = (deltaBuffer[0] == deltaBuffer[1] &&
									deltaBuffer[1] == deltaBuffer[2]);
                var allDivisable = (isDivisible(deltaBuffer[0], 120) &&
									isDivisible(deltaBuffer[1], 120) &&
									isDivisible(deltaBuffer[2], 120));
                return !(allEquals || allDivisable);
            }

            function isDivisible(n, divisor) {
                return (Math.floor(n / divisor) == n / divisor);
            }

            var requestFrame = (function () {
                return window.requestAnimationFrame ||
                        window.webkitRequestAnimationFrame ||
                        function (callback, element, delay) {
                            window.setTimeout(callback, delay || (1000 / 60));
                        };
            })();

            var MutationObserver = window.MutationObserver || window.WebKitMutationObserver;


            /***********************************************
			 * PULSE
			 ***********************************************/

            /**
			 * Viscous fluid with a pulse for part and decay for the rest.
			 * - Applies a fixed force over an interval (a damped acceleration), and
			 * - Lets the exponential bleed away the velocity over a longer interval
			 * - Michael Herf, http://stereopsis.com/stopping/
			 */
            function pulse_(x) {
                var val, start, expx;
                // test
                x = x * options.pulseScale;
                if (x < 1) { // acceleartion
                    val = x - (1 - Math.exp(-x));
                } else {     // tail
                    // the previous animation ended here:
                    start = Math.exp(-1);
                    // simple viscous drag
                    x -= 1;
                    expx = 1 - Math.exp(-x);
                    val = start + (expx * (1 - start));
                }
                return val * options.pulseNormalize;
            }

            function pulse(x) {
                if (x >= 1) return 1;
                if (x <= 0) return 0;

                if (options.pulseNormalize == 1) {
                    options.pulseNormalize /= pulse_(1);
                }
                return pulse_(x);
            }

            addEvent("mousedown", mousedown);
            addEvent("mousewheel", wheel);
            addEvent("load", init);

        }

    });
})(jQuery);


/** Toggle
 **************************************************************** **/
function _toggle() {

    var $_t = this,
		previewParClosedHeight = 25;

    jQuery("div.toggle.active > p").addClass("preview-active");
    jQuery("div.toggle.active > div.toggle-content").slideDown(400);
    jQuery("div.toggle > label").click(function (e) {

        var parentSection = jQuery(this).parent(),
			parentWrapper = jQuery(this).parents("div.toggle"),
			previewPar = false,
			isAccordion = parentWrapper.hasClass("toggle-accordion");

        if (isAccordion && typeof (e.originalEvent) != "undefined") {
            parentWrapper.find("div.toggle.active > label").trigger("click");
        }

        parentSection.toggleClass("active");

        if (parentSection.find("> p").get(0)) {

            previewPar = parentSection.find("> p");
            var previewParCurrentHeight = previewPar.css("height");
            var previewParAnimateHeight = previewPar.css("height");
            previewPar.css("height", "auto");
            previewPar.css("height", previewParCurrentHeight);

        }

        var toggleContent = parentSection.find("> div.toggle-content");

        if (parentSection.hasClass("active")) {

            jQuery(previewPar).animate({ height: previewParAnimateHeight }, 350, function () { jQuery(this).addClass("preview-active"); });
            toggleContent.slideDown(350);

        } else {

            jQuery(previewPar).animate({ height: previewParClosedHeight }, 350, function () { jQuery(this).removeClass("preview-active"); });
            toggleContent.slideUp(350);

        }

    });
}


/** Parallax
 **************************************************************** **/
function _parallax() {

    if (jQuery().parallax) {

        /* Default */
        jQuery(".parallax.parallax-default").css("background-attachment", "fixed");
        jQuery(".parallax.parallax-default").parallax("50%", "0.4");

        jQuery(".parallax.parallax-1").css("background-attachment", "fixed");
        jQuery(".parallax.parallax-1").parallax("50%", "0.4");

        jQuery(".parallax.parallax-2").css("background-attachment", "fixed");
        jQuery(".parallax.parallax-2").parallax("50%", "0.4");

        jQuery(".parallax.parallax-3").css("background-attachment", "fixed");
        jQuery(".parallax.parallax-3").parallax("50%", "0.4");

        jQuery(".parallax.parallax-4").css("background-attachment", "fixed");
        jQuery(".parallax.parallax-4").parallax("50%", "0.4");

        /* Home Slider */
        jQuery("#home div.slider").css("background-attachment", "fixed");
        jQuery("#home div.slider").parallax("50%", "0.4");

    }

}

jQuery(window).load(function () {
    var e = jQuery("body").height();
    if (jQuery(".menu-offcanvas-inner").height() < e) {
        jQuery(".menu-offcanvas-inner").height(e)
    }
});
jQuery(window).resize(function () {
    //var d = $(".menu-offcanvas-inner").height();
    var e = jQuery("body").height();
    jQuery(".menu-offcanvas-inner").height(e)
    //if (jQuery(".menu-offcanvas-inner").height() < e) {
    //    jQuery(".menu-offcanvas-inner").height(e)
    //} else {
    //    jQuery(".menu-offcanvas-inner").height(e)
    //}
});

/** Topdown
 **************************************************************** **/
function TopDown() {
    $(window).scroll(function () {
        if ($(this).scrollTop() > 150) {
            $('#top-down').fadeIn();
        } else {
            $('#top-down').fadeOut();
        }
    });
    $('.btn-top').click(function () {
        $('html, body').animate({
            scrollTop: '0px'
        },
        1500);
        return false;
    });
    $('.btn-down').click(function () {
        $('html, body').animate({
            scrollTop: $(document).height()
        },
        1500);
        return false;
    });
}
/** ColorBox
 **************************************************************** **/
function Colorbox() {
    if (jQuery().colorbox) {
        $('.colorbox').colorbox({
            overlayClose: true,
            opacity: 0.5,
            rel: false,
        });
        $('.product-zoom').colorbox({
            width: '800px',
            height: '600px',
            overlayClose: true,
            opacity: 0.5,
            iframe: true,
        });
    }
}


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
(function (a) { a.fn.appear = function (d, b) { var c = a.extend({ data: undefined, one: true, accX: 0, accY: 0 }, b); return this.each(function () { var g = a(this); g.appeared = false; if (!d) { g.trigger("appear", c.data); return } var f = a(window); var e = function () { if (!g.is(":visible")) { g.appeared = false; return } var r = f.scrollLeft(); var q = f.scrollTop(); var l = g.offset(); var s = l.left; var p = l.top; var i = c.accX; var t = c.accY; var k = g.height(); var j = f.height(); var n = g.width(); var m = f.width(); if (p + k + t >= q && p <= q + j + t && s + n + i >= r && s <= r + m + i) { if (!g.appeared) { g.trigger("appear", c.data) } } else { g.appeared = false } }; var h = function () { g.appeared = true; if (c.one) { f.unbind("scroll", e); var j = a.inArray(e, a.fn.appear.checks); if (j >= 0) { a.fn.appear.checks.splice(j, 1) } } d.apply(this, arguments) }; if (c.one) { g.one("appear", c.data, h) } else { g.bind("appear", c.data, h) } f.scroll(e); a.fn.appear.checks.push(e); (e)() }) }; a.extend(a.fn.appear, { checks: [], timeout: null, checkAll: function () { var b = a.fn.appear.checks.length; if (b > 0) { while (b--) { (a.fn.appear.checks[b])() } } }, run: function () { if (a.fn.appear.timeout) { clearTimeout(a.fn.appear.timeout) } a.fn.appear.timeout = setTimeout(a.fn.appear.checkAll, 20) } }); a.each(["append", "prepend", "after", "before", "attr", "removeAttr", "addClass", "removeClass", "toggleClass", "remove", "css", "show", "hide"], function (c, d) { var b = a.fn[d]; if (b) { a.fn[d] = function () { var e = b.apply(this, arguments); a.fn.appear.run(); return e } } }) })(jQuery);


/** Parallax
	http://www.ianlunn.co.uk/plugins/jquery-parallax/
 **************************************************************** **/
(function (a) { var b = a(window); var c = b.height(); b.resize(function () { c = b.height() }); a.fn.parallax = function (e, d, g) { var i = a(this); var j; var h; var f = 0; function k() { i.each(function () { h = i.offset().top }); if (g) { j = function (m) { return m.outerHeight(true) } } else { j = function (m) { return m.height() } } if (arguments.length < 1 || e === null) { e = "50%" } if (arguments.length < 2 || d === null) { d = 0.5 } if (arguments.length < 3 || g === null) { g = true } var l = b.scrollTop(); i.each(function () { var n = a(this); var o = n.offset().top; var m = j(n); if (o + m < l || o > l + c) { return } i.css("backgroundPosition", e + " " + Math.round((h - l) * d) + "px") }) } b.bind("scroll", k).resize(k); k() } })(jQuery);

