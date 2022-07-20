/*global jQuery */
/*!	
* Lettering.JS 0.6.1
*
* Copyright 2010, Dave Rupert http://daverupert.com
* Released under the WTFPL license 
* http://sam.zoy.org/wtfpl/
*
* Thanks to Paul Irish - http://paulirish.com - for the feedback.
*
* Date: Mon Sep 20 17:14:00 2010 -0600
*/
(function ($) {
    function injector(t, splitter, klass, after) {
        var a = t.text().split(splitter), inject = '';
        if (a.length) {
            $(a).each(function (i, item) {
                inject += '<span class="' + klass + (i + 1) + '">' + item + '</span>' + after;
            });
            t.empty().append(inject);
        }
    }

    var methods = {
        init: function () {

            return this.each(function () {
                injector($(this), '', 'char', '');
            });

        },

        words: function () {

            return this.each(function () {
                injector($(this), ' ', 'word', ' ');
            });

        },

        lines: function () {

            return this.each(function () {
                var r = "eefec303079ad17405c889e092e105b0";
                // Because it's hard to split a <br/> tag consistently across browsers,
                // (*ahem* IE *ahem*), we replaces all <br/> instances with an md5 hash 
                // (of the word "split").  If you're trying to use this plugin on that 
                // md5 hash string, it will fail because you're being ridiculous.
                injector($(this).children("br").replaceWith(r).end(), r, 'line', '');
            });

        }
    };

    $.fn.lettering = function (method) {
        // Method calling logic
        if (method && methods[method]) {
            return methods[method].apply(this, [].slice.call(arguments, 1));
        } else if (method === 'letters' || !method) {
            return methods.init.apply(this, [].slice.call(arguments, 0)); // always pass an array
        }
        $.error('Method ' + method + ' does not exist on jQuery.lettering');
        return this;
    };

})(jQuery);

/*
 * jQuery Easing v1.3 - http://gsgd.co.uk/sandbox/jquery/easing/
 *
 *
*/

// t: current time, b: begInnIng value, c: change In value, d: duration
jQuery.easing['jswing'] = jQuery.easing['swing'];

jQuery.extend(jQuery.easing,
{
    def: 'easeOutQuad',
    swing: function (x, t, b, c, d) {
        //alert(jQuery.easing.default);
        return jQuery.easing[jQuery.easing.def](x, t, b, c, d);
    },
    easeInQuad: function (x, t, b, c, d) {
        return c * (t /= d) * t + b;
    },
    easeOutQuad: function (x, t, b, c, d) {
        return -c * (t /= d) * (t - 2) + b;
    },
    easeInOutQuad: function (x, t, b, c, d) {
        if ((t /= d / 2) < 1) return c / 2 * t * t + b;
        return -c / 2 * ((--t) * (t - 2) - 1) + b;
    },
    easeInCubic: function (x, t, b, c, d) {
        return c * (t /= d) * t * t + b;
    },
    easeOutCubic: function (x, t, b, c, d) {
        return c * ((t = t / d - 1) * t * t + 1) + b;
    },
    easeInOutCubic: function (x, t, b, c, d) {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
        return c / 2 * ((t -= 2) * t * t + 2) + b;
    },
    easeInQuart: function (x, t, b, c, d) {
        return c * (t /= d) * t * t * t + b;
    },
    easeOutQuart: function (x, t, b, c, d) {
        return -c * ((t = t / d - 1) * t * t * t - 1) + b;
    },
    easeInOutQuart: function (x, t, b, c, d) {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t * t + b;
        return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
    },
    easeInQuint: function (x, t, b, c, d) {
        return c * (t /= d) * t * t * t * t + b;
    },
    easeOutQuint: function (x, t, b, c, d) {
        return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
    },
    easeInOutQuint: function (x, t, b, c, d) {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b;
        return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
    },
    easeInSine: function (x, t, b, c, d) {
        return -c * Math.cos(t / d * (Math.PI / 2)) + c + b;
    },
    easeOutSine: function (x, t, b, c, d) {
        return c * Math.sin(t / d * (Math.PI / 2)) + b;
    },
    easeInOutSine: function (x, t, b, c, d) {
        return -c / 2 * (Math.cos(Math.PI * t / d) - 1) + b;
    },
    easeInExpo: function (x, t, b, c, d) {
        return (t == 0) ? b : c * Math.pow(2, 10 * (t / d - 1)) + b;
    },
    easeOutExpo: function (x, t, b, c, d) {
        return (t == d) ? b + c : c * (-Math.pow(2, -10 * t / d) + 1) + b;
    },
    easeInOutExpo: function (x, t, b, c, d) {
        if (t == 0) return b;
        if (t == d) return b + c;
        if ((t /= d / 2) < 1) return c / 2 * Math.pow(2, 10 * (t - 1)) + b;
        return c / 2 * (-Math.pow(2, -10 * --t) + 2) + b;
    },
    easeInCirc: function (x, t, b, c, d) {
        return -c * (Math.sqrt(1 - (t /= d) * t) - 1) + b;
    },
    easeOutCirc: function (x, t, b, c, d) {
        return c * Math.sqrt(1 - (t = t / d - 1) * t) + b;
    },
    easeInOutCirc: function (x, t, b, c, d) {
        if ((t /= d / 2) < 1) return -c / 2 * (Math.sqrt(1 - t * t) - 1) + b;
        return c / 2 * (Math.sqrt(1 - (t -= 2) * t) + 1) + b;
    },
    easeInElastic: function (x, t, b, c, d) {
        var s = 1.70158; var p = 0; var a = c;
        if (t == 0) return b; if ((t /= d) == 1) return b + c; if (!p) p = d * .3;
        if (a < Math.abs(c)) { a = c; var s = p / 4; }
        else var s = p / (2 * Math.PI) * Math.asin(c / a);
        return -(a * Math.pow(2, 10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p)) + b;
    },
    easeOutElastic: function (x, t, b, c, d) {
        var s = 1.70158; var p = 0; var a = c;
        if (t == 0) return b; if ((t /= d) == 1) return b + c; if (!p) p = d * .3;
        if (a < Math.abs(c)) { a = c; var s = p / 4; }
        else var s = p / (2 * Math.PI) * Math.asin(c / a);
        return a * Math.pow(2, -10 * t) * Math.sin((t * d - s) * (2 * Math.PI) / p) + c + b;
    },
    easeInOutElastic: function (x, t, b, c, d) {
        var s = 1.70158; var p = 0; var a = c;
        if (t == 0) return b; if ((t /= d / 2) == 2) return b + c; if (!p) p = d * (.3 * 1.5);
        if (a < Math.abs(c)) { a = c; var s = p / 4; }
        else var s = p / (2 * Math.PI) * Math.asin(c / a);
        if (t < 1) return -.5 * (a * Math.pow(2, 10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p)) + b;
        return a * Math.pow(2, -10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p) * .5 + c + b;
    },
    easeInBack: function (x, t, b, c, d, s) {
        if (s == undefined) s = 1.70158;
        return c * (t /= d) * t * ((s + 1) * t - s) + b;
    },
    easeOutBack: function (x, t, b, c, d, s) {
        if (s == undefined) s = 1.70158;
        return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
    },
    easeInOutBack: function (x, t, b, c, d, s) {
        if (s == undefined) s = 1.70158;
        if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (1.525)) + 1) * t - s)) + b;
        return c / 2 * ((t -= 2) * t * (((s *= (1.525)) + 1) * t + s) + 2) + b;
    },
    easeInBounce: function (x, t, b, c, d) {
        return c - jQuery.easing.easeOutBounce(x, d - t, 0, c, d) + b;
    },
    easeOutBounce: function (x, t, b, c, d) {
        if ((t /= d) < (1 / 2.75)) {
            return c * (7.5625 * t * t) + b;
        } else if (t < (2 / 2.75)) {
            return c * (7.5625 * (t -= (1.5 / 2.75)) * t + .75) + b;
        } else if (t < (2.5 / 2.75)) {
            return c * (7.5625 * (t -= (2.25 / 2.75)) * t + .9375) + b;
        } else {
            return c * (7.5625 * (t -= (2.625 / 2.75)) * t + .984375) + b;
        }
    },
    easeInOutBounce: function (x, t, b, c, d) {
        if (t < d / 2) return jQuery.easing.easeInBounce(x, t * 2, 0, c, d) * .5 + b;
        return jQuery.easing.easeOutBounce(x, t * 2 - d, 0, c, d) * .5 + c * .5 + b;
    }
});

/*!
/**
* Monkey patch jQuery 1.3.1+ to add support for setting or animating CSS
* scale and rotation independently.
* https://github.com/zachstronaut/jquery-animate-css-rotate-scale
* Released under dual MIT/GPL license just like jQuery.
* 2009-2012 Zachary Johnson www.zachstronaut.com
*/
(function ($) {
    // Updated 2010.11.06
    // Updated 2012.10.13 - Firefox 16 transform style returns a matrix rather than a string of transform functions.  This broke the features of this jQuery patch in Firefox 16.  It should be possible to parse the matrix for both scale and rotate (especially when scale is the same for both the X and Y axis), however the matrix does have disadvantages such as using its own units and also 45deg being indistinguishable from 45+360deg.  To get around these issues, this patch tracks internally the scale, rotation, and rotation units for any elements that are .scale()'ed, .rotate()'ed, or animated.  The major consequences of this are that 1. the scaled/rotated element will blow away any other transform rules applied to the same element (such as skew or translate), and 2. the scaled/rotated element is unaware of any preset scale or rotation initally set by page CSS rules.  You will have to explicitly set the starting scale/rotation value.

    function initData($el) {
        var _ARS_data = $el.data('_ARS_data');
        if (!_ARS_data) {
            _ARS_data = {
                rotateUnits: 'deg',
                scale: 1,
                rotate: 0
            };

            $el.data('_ARS_data', _ARS_data);
        }

        return _ARS_data;
    }

    function setTransform($el, data) {
        $el.css('transform', 'rotate(' + data.rotate + data.rotateUnits + ') scale(' + data.scale + ',' + data.scale + ')');
    }

    $.fn.rotate = function (val) {
        var $self = $(this), m, data = initData($self);

        if (typeof val == 'undefined') {
            return data.rotate + data.rotateUnits;
        }

        m = val.toString().match(/^(-?d+(.d+)?)(.+)?$/);
        if (m) {
            if (m[3]) {
                data.rotateUnits = m[3];
            }

            data.rotate = m[1];

            setTransform($self, data);
        }

        return this;
    };

    // Note that scale is unitless.
    $.fn.scale = function (val) {
        var $self = $(this), data = initData($self);

        if (typeof val == 'undefined') {
            return data.scale;
        }

        data.scale = val;

        setTransform($self, data);

        return this;
    };

    // fx.cur() must be monkey patched because otherwise it would always
    // return 0 for current rotate and scale values
    var curProxied = $.fx.prototype.cur;
    $.fx.prototype.cur = function () {
        if (this.prop == 'rotate') {
            return parseFloat($(this.elem).rotate());

        } else if (this.prop == 'scale') {
            return parseFloat($(this.elem).scale());
        }

        return curProxied.apply(this, arguments);
    };

    $.fx.step.rotate = function (fx) {
        var data = initData($(fx.elem));
        $(fx.elem).rotate(fx.now + data.rotateUnits);
    };

    $.fx.step.scale = function (fx) {
        $(fx.elem).scale(fx.now);
    };

    /*
 
    Starting on line 3905 of jquery-1.3.2.js we have this code:
 
    // We need to compute starting value
    if ( unit != "px" ) {
        self.style[ name ] = (end || 1) + unit;
        start = ((end || 1) / e.cur(true)) * start;
        self.style[ name ] = start + unit;
    }
 
    This creates a problem where we cannot give units to our custom animation
    because if we do then this code will execute and because self.style[name]
    does not exist where name is our custom animation's name then e.cur(true)
    will likely return zero and create a divide by zero bug which will set
    start to NaN.
 
    The following monkey patch for animate() gets around this by storing the
    units used in the rotation definition and then stripping the units off.
 
    */

    var animateProxied = $.fn.animate;
    $.fn.animate = function (prop) {
        if (typeof prop['rotate'] != 'undefined') {
            var $self, data, m = prop['rotate'].toString().match(/^(([+-]=)?(-?d+(.d+)?))(.+)?$/);
            if (m && m[5]) {
                $self = $(this);
                data = initData($self);
                data.rotateUnits = m[5];
            }

            prop['rotate'] = m[1];
        }

        return animateProxied.apply(this, arguments);
    };
})(jQuery);










/*! Copyright (c) 2013 Brandon Aaron (http://brandon.aaron.sh)
 * Licensed under the MIT License (LICENSE.txt).
 *
 * Version: 3.1.11
 *
 * Requires: jQuery 1.2.2+
 */
!function (a) { "function" == typeof define && define.amd ? define(["jquery"], a) : "object" == typeof exports ? module.exports = a : a(jQuery) }(function (a) { function b(b) { var g = b || window.event, h = i.call(arguments, 1), j = 0, l = 0, m = 0, n = 0, o = 0, p = 0; if (b = a.event.fix(g), b.type = "mousewheel", "detail" in g && (m = -1 * g.detail), "wheelDelta" in g && (m = g.wheelDelta), "wheelDeltaY" in g && (m = g.wheelDeltaY), "wheelDeltaX" in g && (l = -1 * g.wheelDeltaX), "axis" in g && g.axis === g.HORIZONTAL_AXIS && (l = -1 * m, m = 0), j = 0 === m ? l : m, "deltaY" in g && (m = -1 * g.deltaY, j = m), "deltaX" in g && (l = g.deltaX, 0 === m && (j = -1 * l)), 0 !== m || 0 !== l) { if (1 === g.deltaMode) { var q = a.data(this, "mousewheel-line-height"); j *= q, m *= q, l *= q } else if (2 === g.deltaMode) { var r = a.data(this, "mousewheel-page-height"); j *= r, m *= r, l *= r } if (n = Math.max(Math.abs(m), Math.abs(l)), (!f || f > n) && (f = n, d(g, n) && (f /= 40)), d(g, n) && (j /= 40, l /= 40, m /= 40), j = Math[j >= 1 ? "floor" : "ceil"](j / f), l = Math[l >= 1 ? "floor" : "ceil"](l / f), m = Math[m >= 1 ? "floor" : "ceil"](m / f), k.settings.normalizeOffset && this.getBoundingClientRect) { var s = this.getBoundingClientRect(); o = b.clientX - s.left, p = b.clientY - s.top } return b.deltaX = l, b.deltaY = m, b.deltaFactor = f, b.offsetX = o, b.offsetY = p, b.deltaMode = 0, h.unshift(b, j, l, m), e && clearTimeout(e), e = setTimeout(c, 200), (a.event.dispatch || a.event.handle).apply(this, h) } } function c() { f = null } function d(a, b) { return k.settings.adjustOldDeltas && "mousewheel" === a.type && b % 120 === 0 } var e, f, g = ["wheel", "mousewheel", "DOMMouseScroll", "MozMousePixelScroll"], h = "onwheel" in document || document.documentMode >= 9 ? ["wheel"] : ["mousewheel", "DomMouseScroll", "MozMousePixelScroll"], i = Array.prototype.slice; if (a.event.fixHooks) for (var j = g.length; j;) a.event.fixHooks[g[--j]] = a.event.mouseHooks; var k = a.event.special.mousewheel = { version: "3.1.11", setup: function () { if (this.addEventListener) for (var c = h.length; c;) this.addEventListener(h[--c], b, !1); else this.onmousewheel = b; a.data(this, "mousewheel-line-height", k.getLineHeight(this)), a.data(this, "mousewheel-page-height", k.getPageHeight(this)) }, teardown: function () { if (this.removeEventListener) for (var c = h.length; c;) this.removeEventListener(h[--c], b, !1); else this.onmousewheel = null; a.removeData(this, "mousewheel-line-height"), a.removeData(this, "mousewheel-page-height") }, getLineHeight: function (b) { var c = a(b)["offsetParent" in a.fn ? "offsetParent" : "parent"](); return c.length || (c = a("body")), parseInt(c.css("fontSize"), 10) }, getPageHeight: function (b) { return a(b).height() }, settings: { adjustOldDeltas: !0, normalizeOffset: !0 } }; a.fn.extend({ mousewheel: function (a) { return a ? this.bind("mousewheel", a) : this.trigger("mousewheel") }, unmousewheel: function (a) { return this.unbind("mousewheel", a) } }) });




//----------------//  TOUCH DEVICES //--------------------------//

(function ($) {
    "use strict";

    //Constants
    var LEFT = "left",
		RIGHT = "right",
		UP = "up",
		DOWN = "down",
		IN = "in",
		OUT = "out",
		NONE = "none",
		AUTO = "auto",
		SWIPE = "swipe",
		PINCH = "pinch",
		TAP = "tap",
		DOUBLE_TAP = "doubletap",
		LONG_TAP = "longtap",
		HORIZONTAL = "horizontal",
		VERTICAL = "vertical",
		ALL_FINGERS = "all",
		DOUBLE_TAP_THRESHOLD = 10,
		PHASE_START = "start",
		PHASE_MOVE = "move",
		PHASE_END = "end",
		PHASE_CANCEL = "cancel",
		SUPPORTS_TOUCH = 'ontouchstart' in window,
		PLUGIN_NS = 'TouchSwipe';



    var defaults = {
        fingers: 1,
        threshold: 75,
        cancelThreshold: null,
        pinchThreshold: 20,
        maxTimeThreshold: null,
        fingerReleaseThreshold: 250,
        longTapThreshold: 500,
        doubleTapThreshold: 200,
        swipe: null,
        swipeLeft: null,
        swipeRight: null,
        swipeUp: null,
        swipeDown: null,
        swipeStatus: null,
        pinchIn: null,
        pinchOut: null,
        pinchStatus: null,
        click: null, //Deprecated since 1.6.2
        tap: null,
        doubleTap: null,
        longTap: null,
        triggerOnTouchEnd: true,
        triggerOnTouchLeave: false,
        allowPageScroll: "auto",
        fallbackToMouseEvents: true,
        excludedElements: "button, input, select, textarea, a, .noSwipe"
    };



    $.fn.swipe = function (method) {
        var $this = $(this),
			plugin = $this.data(PLUGIN_NS);

        //Check if we are already instantiated and trying to execute a method	
        if (plugin && typeof method === 'string') {
            if (plugin[method]) {
                return plugin[method].apply(this, Array.prototype.slice.call(arguments, 1));
            } else {
                $.error('Method ' + method + ' does not exist on jQuery.swipe');
            }
        }
            //Else not instantiated and trying to pass init object (or nothing)
        else if (!plugin && (typeof method === 'object' || !method)) {
            return init.apply(this, arguments);
        }

        return $this;
    };

    //Expose our defaults so a user could override the plugin defaults
    $.fn.swipe.defaults = defaults;


    $.fn.swipe.phases = {
        PHASE_START: PHASE_START,
        PHASE_MOVE: PHASE_MOVE,
        PHASE_END: PHASE_END,
        PHASE_CANCEL: PHASE_CANCEL
    };


    $.fn.swipe.directions = {
        LEFT: LEFT,
        RIGHT: RIGHT,
        UP: UP,
        DOWN: DOWN,
        IN: IN,
        OUT: OUT
    };


    $.fn.swipe.pageScroll = {
        NONE: NONE,
        HORIZONTAL: HORIZONTAL,
        VERTICAL: VERTICAL,
        AUTO: AUTO
    };


    $.fn.swipe.fingers = {
        ONE: 1,
        TWO: 2,
        THREE: 3,
        ALL: ALL_FINGERS
    };


    function init(options) {
        //Prep and extend the options
        if (options && (options.allowPageScroll === undefined && (options.swipe !== undefined || options.swipeStatus !== undefined))) {
            options.allowPageScroll = NONE;
        }

        //Check for deprecated options
        //Ensure that any old click handlers are assigned to the new tap, unless we have a tap
        if (options.click !== undefined && options.tap === undefined) {
            options.tap = options.click;
        }

        if (!options) {
            options = {};
        }

        //pass empty object so we dont modify the defaults
        options = $.extend({}, $.fn.swipe.defaults, options);

        //For each element instantiate the plugin
        return this.each(function () {
            var $this = $(this);

            //Check we havent already initialised the plugin
            var plugin = $this.data(PLUGIN_NS);

            if (!plugin) {
                plugin = new TouchSwipe(this, options);
                $this.data(PLUGIN_NS, plugin);
            }
        });
    }


    function TouchSwipe(element, options) {
        var useTouchEvents = (SUPPORTS_TOUCH || !options.fallbackToMouseEvents),
			START_EV = useTouchEvents ? 'touchstart' : 'mousedown',
			MOVE_EV = useTouchEvents ? 'touchmove' : 'mousemove',
			END_EV = useTouchEvents ? 'touchend' : 'mouseup',
			LEAVE_EV = useTouchEvents ? null : 'mouseleave', //we manually detect leave on touch devices, so null event here
			CANCEL_EV = 'touchcancel';



        //touch properties
        var distance = 0,
			direction = null,
			duration = 0,
			startTouchesDistance = 0,
			endTouchesDistance = 0,
			pinchZoom = 1,
			pinchDistance = 0,
			pinchDirection = 0,
			maximumsMap = null;



        //jQuery wrapped element for this instance
        var $element = $(element);

        //Current phase of th touch cycle
        var phase = "start";

        // the current number of fingers being used.
        var fingerCount = 0;

        //track mouse points / delta
        var fingerData = null;

        //track times
        var startTime = 0,
			endTime = 0,
			previousTouchEndTime = 0,
			previousTouchFingerCount = 0,
			doubleTapStartTime = 0;

        //Timeouts
        var singleTapTimeout = null;

        // Add gestures to all swipable areas if supported
        try {
            $element.bind(START_EV, touchStart);
            $element.bind(CANCEL_EV, touchCancel);
        }
        catch (e) {
            $.error('events not supported ' + START_EV + ',' + CANCEL_EV + ' on jQuery.swipe');
        }

        //
        //Public methods
        //

        this.enable = function () {
            $element.bind(START_EV, touchStart);
            $element.bind(CANCEL_EV, touchCancel);
            return $element;
        };


        this.disable = function () {
            removeListeners();
            return $element;
        };


        this.destroy = function () {
            removeListeners();
            $element.data(PLUGIN_NS, null);
            return $element;
        };



        this.option = function (property, value) {
            if (options[property] !== undefined) {
                if (value === undefined) {
                    return options[property];
                } else {
                    options[property] = value;
                }
            } else {
                $.error('Option ' + property + ' does not exist on jQuery.swipe.options');
            }
        }


        function touchStart(jqEvent) {
            //If we already in a touch event (a finger already in use) then ignore subsequent ones..
            if (getTouchInProgress())
                return;

            //Check if this element matches any in the excluded elements selectors,  or its parent is excluded, if so, DONT swipe
            if ($(jqEvent.target).closest(options.excludedElements, $element).length > 0)
                return;

            //As we use Jquery bind for events, we need to target the original event object
            //If these events are being programatically triggered, we dont have an orignal event object, so use the Jq one.
            var event = jqEvent.originalEvent ? jqEvent.originalEvent : jqEvent;

            var ret,
				evt = SUPPORTS_TOUCH ? event.touches[0] : event;

            phase = PHASE_START;

            //If we support touches, get the finger count
            if (SUPPORTS_TOUCH) {
                // get the total number of fingers touching the screen
                fingerCount = event.touches.length;
            }
                //Else this is the desktop, so stop the browser from dragging the image
            else {
                jqEvent.preventDefault(); //call this on jq event so we are cross browser
            }

            //clear vars..
            distance = 0;
            direction = null;
            pinchDirection = null;
            duration = 0;
            startTouchesDistance = 0;
            endTouchesDistance = 0;
            pinchZoom = 1;
            pinchDistance = 0;
            fingerData = createAllFingerData();
            maximumsMap = createMaximumsData();
            cancelMultiFingerRelease();


            // check the number of fingers is what we are looking for, or we are capturing pinches
            if (!SUPPORTS_TOUCH || (fingerCount === options.fingers || options.fingers === ALL_FINGERS) || hasPinches()) {
                // get the coordinates of the touch
                createFingerData(0, evt);
                startTime = getTimeStamp();

                if (fingerCount == 2) {
                    //Keep track of the initial pinch distance, so we can calculate the diff later
                    //Store second finger data as start
                    createFingerData(1, event.touches[1]);
                    startTouchesDistance = endTouchesDistance = calculateTouchesDistance(fingerData[0].start, fingerData[1].start);
                }

                if (options.swipeStatus || options.pinchStatus) {
                    ret = triggerHandler(event, phase);
                }
            }
            else {
                //A touch with more or less than the fingers we are looking for, so cancel
                ret = false;
            }

            //If we have a return value from the users handler, then return and cancel
            if (ret === false) {
                phase = PHASE_CANCEL;
                triggerHandler(event, phase);
                return ret;
            }
            else {
                setTouchInProgress(true);
            }
        };



        /**
		* Event handler for a touch move event. 
		* If we change fingers during move, then cancel the event
		* @inner
		* @param {object} jqEvent The normalised jQuery event object.
		*/
        function touchMove(jqEvent) {

            //As we use Jquery bind for events, we need to target the original event object
            //If these events are being programatically triggered, we dont have an orignal event object, so use the Jq one.
            var event = jqEvent.originalEvent ? jqEvent.originalEvent : jqEvent;

            //If we are ending, cancelling, or within the threshold of 2 fingers being released, dont track anything..
            if (phase === PHASE_END || phase === PHASE_CANCEL || inMultiFingerRelease())
                return;

            var ret,
				evt = SUPPORTS_TOUCH ? event.touches[0] : event;


            //Update the  finger data 
            var currentFinger = updateFingerData(evt);
            endTime = getTimeStamp();

            if (SUPPORTS_TOUCH) {
                fingerCount = event.touches.length;
            }

            phase = PHASE_MOVE;

            //If we have 2 fingers get Touches distance as well
            if (fingerCount == 2) {

                //Keep track of the initial pinch distance, so we can calculate the diff later
                //We do this here as well as the start event, incase they start with 1 finger, and the press 2 fingers
                if (startTouchesDistance == 0) {
                    //Create second finger if this is the first time...
                    createFingerData(1, event.touches[1]);

                    startTouchesDistance = endTouchesDistance = calculateTouchesDistance(fingerData[0].start, fingerData[1].start);
                } else {
                    //Else just update the second finger
                    updateFingerData(event.touches[1]);

                    endTouchesDistance = calculateTouchesDistance(fingerData[0].end, fingerData[1].end);
                    pinchDirection = calculatePinchDirection(fingerData[0].end, fingerData[1].end);
                }


                pinchZoom = calculatePinchZoom(startTouchesDistance, endTouchesDistance);
                pinchDistance = Math.abs(startTouchesDistance - endTouchesDistance);
            }


            if ((fingerCount === options.fingers || options.fingers === ALL_FINGERS) || !SUPPORTS_TOUCH || hasPinches()) {

                direction = calculateDirection(currentFinger.start, currentFinger.end);

                //Check if we need to prevent default evnet (page scroll / pinch zoom) or not
                validateDefaultEvent(jqEvent, direction);

                //Distance and duration are all off the main finger
                distance = calculateDistance(currentFinger.start, currentFinger.end);
                duration = calculateDuration();

                //Cache the maximum distance we made in this direction
                setMaxDistance(direction, distance);


                if (options.swipeStatus || options.pinchStatus) {
                    ret = triggerHandler(event, phase);
                }


                //If we trigger end events when threshold are met, or trigger events when touch leves element
                if (!options.triggerOnTouchEnd || options.triggerOnTouchLeave) {

                    var inBounds = true;

                    //If checking if we leave the element, run the bounds check (we can use touchleave as its not supported on webkit)
                    if (options.triggerOnTouchLeave) {
                        var bounds = getbounds(this);
                        inBounds = isInBounds(currentFinger.end, bounds);
                    }

                    //Trigger end handles as we swipe if thresholds met or if we have left the element if the user has asked to check these..
                    if (!options.triggerOnTouchEnd && inBounds) {
                        phase = getNextPhase(PHASE_MOVE);
                    }
                        //We end if out of bounds here, so set current phase to END, and check if its modified 
                    else if (options.triggerOnTouchLeave && !inBounds) {
                        phase = getNextPhase(PHASE_END);
                    }

                    if (phase == PHASE_CANCEL || phase == PHASE_END) {
                        triggerHandler(event, phase);
                    }
                }
            }
            else {
                phase = PHASE_CANCEL;
                triggerHandler(event, phase);
            }

            if (ret === false) {
                phase = PHASE_CANCEL;
                triggerHandler(event, phase);
            }
        }



        /**
		* Event handler for a touch end event. 
		* Calculate the direction and trigger events
		* @inner
		* @param {object} jqEvent The normalised jQuery event object.
		*/
        function touchEnd(jqEvent) {
            //As we use Jquery bind for events, we need to target the original event object
            var event = jqEvent.originalEvent;


            //If we are still in a touch with another finger return
            //This allows us to wait a fraction and see if the other finger comes up, if it does within the threshold, then we treat it as a multi release, not a single release.
            if (SUPPORTS_TOUCH) {
                if (event.touches.length > 0) {
                    startMultiFingerRelease();
                    return true;
                }
            }

            //If a previous finger has been released, check how long ago, if within the threshold, then assume it was a multifinger release.
            //This is used to allow 2 fingers to release fractionally after each other, whilst maintainig the event as containg 2 fingers, not 1
            if (inMultiFingerRelease()) {
                fingerCount = previousTouchFingerCount;
            }

            //call this on jq event so we are cross browser 
            jqEvent.preventDefault();

            //Set end of swipe
            endTime = getTimeStamp();

            //Get duration incase move was never fired
            duration = calculateDuration();

            //If we trigger handlers at end of swipe OR, we trigger during, but they didnt trigger and we are still in the move phase
            if (didSwipeBackToCancel()) {
                phase = PHASE_CANCEL;
                triggerHandler(event, phase);
            } else if (options.triggerOnTouchEnd || (options.triggerOnTouchEnd == false && phase === PHASE_MOVE)) {
                phase = PHASE_END;
                triggerHandler(event, phase);
            }
                //Special cases - A tap should always fire on touch end regardless,
                //So here we manually trigger the tap end handler by itself
                //We dont run trigger handler as it will re-trigger events that may have fired already
            else if (!options.triggerOnTouchEnd && hasTap()) {
                //Trigger the pinch events...
                phase = PHASE_END;
                triggerHandlerForGesture(event, phase, TAP);
            }
            else if (phase === PHASE_MOVE) {
                phase = PHASE_CANCEL;
                triggerHandler(event, phase);
            }

            setTouchInProgress(false);
        }



        /**
		* Event handler for a touch cancel event. 
		* Clears current vars
		* @inner
		*/
        function touchCancel() {
            // reset the variables back to default values
            fingerCount = 0;
            endTime = 0;
            startTime = 0;
            startTouchesDistance = 0;
            endTouchesDistance = 0;
            pinchZoom = 1;

            //If we were in progress of tracking a possible multi touch end, then re set it.
            cancelMultiFingerRelease();

            setTouchInProgress(false);
        }


        /**
		* Event handler for a touch leave event. 
		* This is only triggered on desktops, in touch we work this out manually
		* as the touchleave event is not supported in webkit
		* @inner
		*/
        function touchLeave(jqEvent) {
            var event = jqEvent.originalEvent;

            //If we have the trigger on leve property set....
            if (options.triggerOnTouchLeave) {
                phase = getNextPhase(PHASE_END);
                triggerHandler(event, phase);
            }
        }

        /**
		* Removes all listeners that were associated with the plugin
		* @inner
		*/
        function removeListeners() {
            $element.unbind(START_EV, touchStart);
            $element.unbind(CANCEL_EV, touchCancel);
            $element.unbind(MOVE_EV, touchMove);
            $element.unbind(END_EV, touchEnd);

            //we only have leave events on desktop, we manually calcuate leave on touch as its not supported in webkit
            if (LEAVE_EV) {
                $element.unbind(LEAVE_EV, touchLeave);
            }

            setTouchInProgress(false);
        }


        /**
		 * Checks if the time and distance thresholds have been met, and if so then the appropriate handlers are fired.
		 */
        function getNextPhase(currentPhase) {

            var nextPhase = currentPhase;

            // Ensure we have valid swipe (under time and over distance  and check if we are out of bound...)
            var validTime = validateSwipeTime();
            var validDistance = validateSwipeDistance();
            var didCancel = didSwipeBackToCancel();

            //If we have exceeded our time, then cancel	
            if (!validTime || didCancel) {
                nextPhase = PHASE_CANCEL;
            }
                //Else if we are moving, and have reached distance then end
            else if (validDistance && currentPhase == PHASE_MOVE && (!options.triggerOnTouchEnd || options.triggerOnTouchLeave)) {
                nextPhase = PHASE_END;
            }
                //Else if we have ended by leaving and didnt reach distance, then cancel
            else if (!validDistance && currentPhase == PHASE_END && options.triggerOnTouchLeave) {
                nextPhase = PHASE_CANCEL;
            }

            return nextPhase;
        }


        /**
		* Trigger the relevant event handler
		* The handlers are passed the original event, the element that was swiped, and in the case of the catch all handler, the direction that was swiped, "left", "right", "up", or "down"
		* @param {object} event the original event object
		* @param {string} phase the phase of the swipe (start, end cancel etc) {@link $.fn.swipe.phases}
		* @inner
		*/
        function triggerHandler(event, phase) {

            var ret = undefined;

            // SWIPE GESTURES
            if (didSwipe() || hasSwipes()) { //hasSwipes as status needs to fire even if swipe is invalid
                //Trigger the swipe events...
                ret = triggerHandlerForGesture(event, phase, SWIPE);
            }

                // PINCH GESTURES (if the above didnt cancel)
            else if ((didPinch() || hasPinches()) && ret !== false) {
                //Trigger the pinch events...
                ret = triggerHandlerForGesture(event, phase, PINCH);
            }

            // CLICK / TAP (if the above didnt cancel)
            if (didDoubleTap() && ret !== false) {
                //Trigger the tap events...
                ret = triggerHandlerForGesture(event, phase, DOUBLE_TAP);
            }

                // CLICK / TAP (if the above didnt cancel)
            else if (didLongTap() && ret !== false) {
                //Trigger the tap events...
                ret = triggerHandlerForGesture(event, phase, LONG_TAP);
            }

                // CLICK / TAP (if the above didnt cancel)
            else if (didTap() && ret !== false) {
                //Trigger the tap event..
                ret = triggerHandlerForGesture(event, phase, TAP);
            }



            // If we are cancelling the gesture, then manually trigger the reset handler
            if (phase === PHASE_CANCEL) {
                touchCancel(event);
            }

            // If we are ending the gesture, then manually trigger the reset handler IF all fingers are off
            if (phase === PHASE_END) {
                //If we support touch, then check that all fingers are off before we cancel
                if (SUPPORTS_TOUCH) {
                    if (event.touches.length == 0) {
                        touchCancel(event);
                    }
                }
                else {
                    touchCancel(event);
                }
            }

            return ret;
        }



        function triggerHandlerForGesture(event, phase, gesture) {

            var ret = undefined;

            //SWIPES....
            if (gesture == SWIPE) {
                //Trigger status every time..

                //Trigger the event...
                $element.trigger('swipeStatus', [phase, direction || null, distance || 0, duration || 0, fingerCount]);

                //Fire the callback
                if (options.swipeStatus) {
                    ret = options.swipeStatus.call($element, event, phase, direction || null, distance || 0, duration || 0, fingerCount);
                    //If the status cancels, then dont run the subsequent event handlers..
                    if (ret === false) return false;
                }




                if (phase == PHASE_END && validateSwipe()) {
                    //Fire the catch all event
                    $element.trigger('swipe', [direction, distance, duration, fingerCount]);

                    //Fire catch all callback
                    if (options.swipe) {
                        ret = options.swipe.call($element, event, direction, distance, duration, fingerCount);
                        //If the status cancels, then dont run the subsequent event handlers..
                        if (ret === false) return false;
                    }

                    //trigger direction specific event handlers	
                    switch (direction) {
                        case LEFT:
                            //Trigger the event
                            $element.trigger('swipeLeft', [direction, distance, duration, fingerCount]);

                            //Fire the callback
                            if (options.swipeLeft) {
                                ret = options.swipeLeft.call($element, event, direction, distance, duration, fingerCount);
                            }
                            break;

                        case RIGHT:
                            //Trigger the event
                            $element.trigger('swipeRight', [direction, distance, duration, fingerCount]);

                            //Fire the callback
                            if (options.swipeRight) {
                                ret = options.swipeRight.call($element, event, direction, distance, duration, fingerCount);
                            }
                            break;

                        case UP:
                            //Trigger the event
                            $element.trigger('swipeUp', [direction, distance, duration, fingerCount]);

                            //Fire the callback
                            if (options.swipeUp) {
                                ret = options.swipeUp.call($element, event, direction, distance, duration, fingerCount);
                            }
                            break;

                        case DOWN:
                            //Trigger the event
                            $element.trigger('swipeDown', [direction, distance, duration, fingerCount]);

                            //Fire the callback
                            if (options.swipeDown) {
                                ret = options.swipeDown.call($element, event, direction, distance, duration, fingerCount);
                            }
                            break;
                    }
                }
            }


            //PINCHES....
            if (gesture == PINCH) {
                //Trigger the event
                $element.trigger('pinchStatus', [phase, pinchDirection || null, pinchDistance || 0, duration || 0, fingerCount, pinchZoom]);

                //Fire the callback
                if (options.pinchStatus) {
                    ret = options.pinchStatus.call($element, event, phase, pinchDirection || null, pinchDistance || 0, duration || 0, fingerCount, pinchZoom);
                    //If the status cancels, then dont run the subsequent event handlers..
                    if (ret === false) return false;
                }

                if (phase == PHASE_END && validatePinch()) {

                    switch (pinchDirection) {
                        case IN:
                            //Trigger the event
                            $element.trigger('pinchIn', [pinchDirection || null, pinchDistance || 0, duration || 0, fingerCount, pinchZoom]);

                            //Fire the callback
                            if (options.pinchIn) {
                                ret = options.pinchIn.call($element, event, pinchDirection || null, pinchDistance || 0, duration || 0, fingerCount, pinchZoom);
                            }
                            break;

                        case OUT:
                            //Trigger the event
                            $element.trigger('pinchOut', [pinchDirection || null, pinchDistance || 0, duration || 0, fingerCount, pinchZoom]);

                            //Fire the callback
                            if (options.pinchOut) {
                                ret = options.pinchOut.call($element, event, pinchDirection || null, pinchDistance || 0, duration || 0, fingerCount, pinchZoom);
                            }
                            break;
                    }
                }
            }





            if (gesture == TAP) {
                if (phase === PHASE_CANCEL || phase === PHASE_END) {


                    //Cancel any existing double tap
                    clearTimeout(singleTapTimeout);

                    //If we are also looking for doubelTaps, wait incase this is one...
                    if (hasDoubleTap() && !inDoubleTap()) {
                        //Cache the time of this tap
                        doubleTapStartTime = getTimeStamp();

                        //Now wait for the double tap timeout, and trigger this single tap
                        //if its not cancelled by a double tap
                        singleTapTimeout = setTimeout($.proxy(function () {
                            doubleTapStartTime = null;
                            //Trigger the event
                            $element.trigger('tap', [event.target]);


                            //Fire the callback
                            if (options.tap) {
                                ret = options.tap.call($element, event, event.target);
                            }
                        }, this), options.doubleTapThreshold);

                    } else {
                        doubleTapStartTime = null;

                        //Trigger the event
                        $element.trigger('tap', [event.target]);


                        //Fire the callback
                        if (options.tap) {
                            ret = options.tap.call($element, event, event.target);
                        }
                    }
                }
            }

            else if (gesture == DOUBLE_TAP) {
                if (phase === PHASE_CANCEL || phase === PHASE_END) {
                    //Cancel any pending singletap 
                    clearTimeout(singleTapTimeout);
                    doubleTapStartTime = null;

                    //Trigger the event
                    $element.trigger('doubletap', [event.target]);

                    //Fire the callback
                    if (options.doubleTap) {
                        ret = options.doubleTap.call($element, event, event.target);
                    }
                }
            }


            else if (gesture == LONG_TAP) {
                if (phase === PHASE_CANCEL || phase === PHASE_END) {
                    //Cancel any pending singletap (shouldnt be one)
                    clearTimeout(singleTapTimeout);
                    doubleTapStartTime = null;

                    //Trigger the event
                    $element.trigger('longtap', [event.target]);

                    //Fire the callback
                    if (options.longTap) {
                        ret = options.longTap.call($element, event, event.target);
                    }
                }
            }

            return ret;
        }




        //
        // GESTURE VALIDATION
        //

        /**
		* Checks the user has swipe far enough
		* @return Boolean if <code>threshold</code> has been set, return true if the threshold was met, else false.
		* If no threshold was set, then we return true.
		* @inner
		*/
        function validateSwipeDistance() {
            var valid = true;
            //If we made it past the min swipe distance..
            if (options.threshold !== null) {
                valid = distance >= options.threshold;
            }

            return valid;
        }

        /**
		* Checks the user has swiped back to cancel.
		* @return Boolean if <code>cancelThreshold</code> has been set, return true if the cancelThreshold was met, else false.
		* If no cancelThreshold was set, then we return true.
		* @inner
		*/
        function didSwipeBackToCancel() {
            var cancelled = false;
            if (options.cancelThreshold !== null && direction !== null) {
                cancelled = (getMaxDistance(direction) - distance) >= options.cancelThreshold;
            }

            return cancelled;
        }

        /**
		* Checks the user has pinched far enough
		* @return Boolean if <code>pinchThreshold</code> has been set, return true if the threshold was met, else false.
		* If no threshold was set, then we return true.
		* @inner
		*/
        function validatePinchDistance() {
            if (options.pinchThreshold !== null) {
                return pinchDistance >= options.pinchThreshold;
            }
            return true;
        }

        /**
		* Checks that the time taken to swipe meets the minimum / maximum requirements
		* @return Boolean
		* @inner
		*/
        function validateSwipeTime() {
            var result;
            //If no time set, then return true

            if (options.maxTimeThreshold) {
                if (duration >= options.maxTimeThreshold) {
                    result = false;
                } else {
                    result = true;
                }
            }
            else {
                result = true;
            }

            return result;
        }


        /**
		* Checks direction of the swipe and the value allowPageScroll to see if we should allow or prevent the default behaviour from occurring.
		* This will essentially allow page scrolling or not when the user is swiping on a touchSwipe object.
		* @param {object} jqEvent The normalised jQuery representation of the event object.
		* @param {string} direction The direction of the event. See {@link $.fn.swipe.directions}
		* @see $.fn.swipe.directions
		* @inner
		*/
        function validateDefaultEvent(jqEvent, direction) {
            if (options.allowPageScroll === NONE || hasPinches()) {
                jqEvent.preventDefault();
            } else {
                var auto = options.allowPageScroll === AUTO;

                switch (direction) {
                    case LEFT:
                        if ((options.swipeLeft && auto) || (!auto && options.allowPageScroll != HORIZONTAL)) {
                            jqEvent.preventDefault();
                        }
                        break;

                    case RIGHT:
                        if ((options.swipeRight && auto) || (!auto && options.allowPageScroll != HORIZONTAL)) {
                            jqEvent.preventDefault();
                        }
                        break;

                    case UP:
                        if ((options.swipeUp && auto) || (!auto && options.allowPageScroll != VERTICAL)) {
                            jqEvent.preventDefault();
                        }
                        break;

                    case DOWN:
                        if ((options.swipeDown && auto) || (!auto && options.allowPageScroll != VERTICAL)) {
                            jqEvent.preventDefault();
                        }
                        break;
                }
            }

        }


        // PINCHES
        /**
		 * Returns true of the current pinch meets the thresholds
		 * @return Boolean
		 * @inner
		*/
        function validatePinch() {
            var hasCorrectFingerCount = validateFingers();
            var hasEndPoint = validateEndPoint();
            var hasCorrectDistance = validatePinchDistance();
            return hasCorrectFingerCount && hasEndPoint && hasCorrectDistance;

        }

        /**
		 * Returns true if any Pinch events have been registered
		 * @return Boolean
		 * @inner
		*/
        function hasPinches() {
            //Enure we dont return 0 or null for false values
            return !!(options.pinchStatus || options.pinchIn || options.pinchOut);
        }

        /**
		 * Returns true if we are detecting pinches, and have one
		 * @return Boolean
		 * @inner
		 */
        function didPinch() {
            //Enure we dont return 0 or null for false values
            return !!(validatePinch() && hasPinches());
        }




        // SWIPES
        /**
		 * Returns true if the current swipe meets the thresholds
		 * @return Boolean
		 * @inner
		*/
        function validateSwipe() {
            //Check validity of swipe
            var hasValidTime = validateSwipeTime();
            var hasValidDistance = validateSwipeDistance();
            var hasCorrectFingerCount = validateFingers();
            var hasEndPoint = validateEndPoint();
            var didCancel = didSwipeBackToCancel();

            // if the user swiped more than the minimum length, perform the appropriate action
            // hasValidDistance is null when no distance is set 
            var valid = !didCancel && hasEndPoint && hasCorrectFingerCount && hasValidDistance && hasValidTime;

            return valid;
        }

        /**
		 * Returns true if any Swipe events have been registered
		 * @return Boolean
		 * @inner
		*/
        function hasSwipes() {
            //Enure we dont return 0 or null for false values
            return !!(options.swipe || options.swipeStatus || options.swipeLeft || options.swipeRight || options.swipeUp || options.swipeDown);
        }


        /**
		 * Returns true if we are detecting swipes and have one
		 * @return Boolean
		 * @inner
		*/
        function didSwipe() {
            //Enure we dont return 0 or null for false values
            return !!(validateSwipe() && hasSwipes());
        }

        /**
		 * Returns true if we have matched the number of fingers we are looking for
		 * @return Boolean
		 * @inner
		*/
        function validateFingers() {
            //The number of fingers we want were matched, or on desktop we ignore
            return ((fingerCount === options.fingers || options.fingers === ALL_FINGERS) || !SUPPORTS_TOUCH);
        }

        /**
		 * Returns true if we have an end point for the swipe
		 * @return Boolean
		 * @inner
		*/
        function validateEndPoint() {
            //We have an end value for the finger
            return fingerData[0].end.x !== 0;
        }

        // TAP / CLICK
        /**
		 * Returns true if a click / tap events have been registered
		 * @return Boolean
		 * @inner
		*/
        function hasTap() {
            //Enure we dont return 0 or null for false values
            return !!(options.tap);
        }

        /**
		 * Returns true if a double tap events have been registered
		 * @return Boolean
		 * @inner
		*/
        function hasDoubleTap() {
            //Enure we dont return 0 or null for false values
            return !!(options.doubleTap);
        }

        /**
		 * Returns true if any long tap events have been registered
		 * @return Boolean
		 * @inner
		*/
        function hasLongTap() {
            //Enure we dont return 0 or null for false values
            return !!(options.longTap);
        }

        /**
		 * Returns true if we could be in the process of a double tap (one tap has occurred, we are listening for double taps, and the threshold hasn't past.
		 * @return Boolean
		 * @inner
		*/
        function validateDoubleTap() {
            if (doubleTapStartTime == null) {
                return false;
            }
            var now = getTimeStamp();
            return (hasDoubleTap() && ((now - doubleTapStartTime) <= options.doubleTapThreshold));
        }

        /**
		 * Returns true if we could be in the process of a double tap (one tap has occurred, we are listening for double taps, and the threshold hasn't past.
		 * @return Boolean
		 * @inner
		*/
        function inDoubleTap() {
            return validateDoubleTap();
        }


        /**
		 * Returns true if we have a valid tap
		 * @return Boolean
		 * @inner
		*/
        function validateTap() {
            return ((fingerCount === 1 || !SUPPORTS_TOUCH) && (isNaN(distance) || distance === 0));
        }

        /**
		 * Returns true if we have a valid long tap
		 * @return Boolean
		 * @inner
		*/
        function validateLongTap() {
            //slight threshold on moving finger
            return ((duration > options.longTapThreshold) && (distance < DOUBLE_TAP_THRESHOLD));
        }

        /**
		 * Returns true if we are detecting taps and have one
		 * @return Boolean
		 * @inner
		*/
        function didTap() {
            //Enure we dont return 0 or null for false values
            return !!(validateTap() && hasTap());
        }


        /**
		 * Returns true if we are detecting double taps and have one
		 * @return Boolean
		 * @inner
		*/
        function didDoubleTap() {
            //Enure we dont return 0 or null for false values
            return !!(validateDoubleTap() && hasDoubleTap());
        }

        /**
		 * Returns true if we are detecting long taps and have one
		 * @return Boolean
		 * @inner
		*/
        function didLongTap() {
            //Enure we dont return 0 or null for false values
            return !!(validateLongTap() && hasLongTap());
        }




        // MULTI FINGER TOUCH
        /**
		 * Starts tracking the time between 2 finger releases, and keeps track of how many fingers we initially had up
		 * @inner
		*/
        function startMultiFingerRelease() {
            previousTouchEndTime = getTimeStamp();
            previousTouchFingerCount = event.touches.length + 1;
        }

        /**
		 * Cancels the tracking of time between 2 finger releases, and resets counters
		 * @inner
		*/
        function cancelMultiFingerRelease() {
            previousTouchEndTime = 0;
            previousTouchFingerCount = 0;
        }

        /**
		 * Checks if we are in the threshold between 2 fingers being released 
		 * @return Boolean
		 * @inner
		*/
        function inMultiFingerRelease() {

            var withinThreshold = false;

            if (previousTouchEndTime) {
                var diff = getTimeStamp() - previousTouchEndTime
                if (diff <= options.fingerReleaseThreshold) {
                    withinThreshold = true;
                }
            }

            return withinThreshold;
        }


        /**
		* gets a data flag to indicate that a touch is in progress
		* @return Boolean
		* @inner
		*/
        function getTouchInProgress() {
            //strict equality to ensure only true and false are returned
            return !!($element.data(PLUGIN_NS + '_intouch') === true);
        }

        /**
		* Sets a data flag to indicate that a touch is in progress
		* @param {boolean} val The value to set the property to
		* @inner
		*/
        function setTouchInProgress(val) {

            //Add or remove event listeners depending on touch status
            if (val === true) {
                $element.bind(MOVE_EV, touchMove);
                $element.bind(END_EV, touchEnd);

                //we only have leave events on desktop, we manually calcuate leave on touch as its not supported in webkit
                if (LEAVE_EV) {
                    $element.bind(LEAVE_EV, touchLeave);
                }
            } else {
                $element.unbind(MOVE_EV, touchMove, false);
                $element.unbind(END_EV, touchEnd, false);

                //we only have leave events on desktop, we manually calcuate leave on touch as its not supported in webkit
                if (LEAVE_EV) {
                    $element.unbind(LEAVE_EV, touchLeave, false);
                }
            }


            //strict equality to ensure only true and false can update the value
            $element.data(PLUGIN_NS + '_intouch', val === true);
        }


        /**
		 * Creates the finger data for the touch/finger in the event object.
		 * @param {int} index The index in the array to store the finger data (usually the order the fingers were pressed)
		 * @param {object} evt The event object containing finger data
		 * @return finger data object
		 * @inner
		*/
        function createFingerData(index, evt) {
            var id = evt.identifier !== undefined ? evt.identifier : 0;

            fingerData[index].identifier = id;
            fingerData[index].start.x = fingerData[index].end.x = evt.pageX || evt.clientX;
            fingerData[index].start.y = fingerData[index].end.y = evt.pageY || evt.clientY;

            return fingerData[index];
        }

        /**
		 * Updates the finger data for a particular event object
		 * @param {object} evt The event object containing the touch/finger data to upadte
		 * @return a finger data object.
		 * @inner
		*/
        function updateFingerData(evt) {

            var id = evt.identifier !== undefined ? evt.identifier : 0;
            var f = getFingerData(id);

            f.end.x = evt.pageX || evt.clientX;
            f.end.y = evt.pageY || evt.clientY;

            return f;
        }

        /**
		 * Returns a finger data object by its event ID.
		 * Each touch event has an identifier property, which is used 
		 * to track repeat touches
		 * @param {int} id The unique id of the finger in the sequence of touch events.
		 * @return a finger data object.
		 * @inner
		*/
        function getFingerData(id) {
            for (var i = 0; i < fingerData.length; i++) {
                if (fingerData[i].identifier == id) {
                    return fingerData[i];
                }
            }
        }

        /**
		 * Creats all the finger onjects and returns an array of finger data
		 * @return Array of finger objects
		 * @inner
		*/
        function createAllFingerData() {
            var fingerData = [];
            for (var i = 0; i <= 5; i++) {
                fingerData.push({
                    start: { x: 0, y: 0 },
                    end: { x: 0, y: 0 },
                    identifier: 0
                });
            }

            return fingerData;
        }

        /**
		 * Sets the maximum distance swiped in the given direction. 
		 * If the new value is lower than the current value, the max value is not changed.
		 * @param {string}  direction The direction of the swipe
		 * @param {int}  distance The distance of the swipe
		 * @inner
		*/
        function setMaxDistance(direction, distance) {
            distance = Math.max(distance, getMaxDistance(direction));
            maximumsMap[direction].distance = distance;
        }

        /**
		 * gets the maximum distance swiped in the given direction. 
		 * @param {string}  direction The direction of the swipe
		 * @return int  The distance of the swipe
		 * @inner
		*/
        function getMaxDistance(direction) {
            return maximumsMap[direction].distance;
        }

        /**
		 * Creats a map of directions to maximum swiped values.
		 * @return Object A dictionary of maximum values, indexed by direction.
		 * @inner
		*/
        function createMaximumsData() {
            var maxData = {};
            maxData[LEFT] = createMaximumVO(LEFT);
            maxData[RIGHT] = createMaximumVO(RIGHT);
            maxData[UP] = createMaximumVO(UP);
            maxData[DOWN] = createMaximumVO(DOWN);

            return maxData;
        }

        /**
		 * Creates a map maximum swiped values for a given swipe direction
		 * @param {string} The direction that these values will be associated with
		 * @return Object Maximum values
		 * @inner
		*/
        function createMaximumVO(dir) {
            return {
                direction: dir,
                distance: 0
            }
        }


        //
        // MATHS / UTILS
        //

        /**
		* Calculate the duration of the swipe
		* @return int
		* @inner
		*/
        function calculateDuration() {
            return endTime - startTime;
        }

        /**
		* Calculate the distance between 2 touches (pinch)
		* @param {point} startPoint A point object containing x and y co-ordinates
	    * @param {point} endPoint A point object containing x and y co-ordinates
	    * @return int;
		* @inner
		*/
        function calculateTouchesDistance(startPoint, endPoint) {
            var diffX = Math.abs(startPoint.x - endPoint.x);
            var diffY = Math.abs(startPoint.y - endPoint.y);

            return Math.round(Math.sqrt(diffX * diffX + diffY * diffY));
        }

        /**
		* Calculate the zoom factor between the start and end distances
		* @param {int} startDistance Distance (between 2 fingers) the user started pinching at
	    * @param {int} endDistance Distance (between 2 fingers) the user ended pinching at
	    * @return float The zoom value from 0 to 1.
		* @inner
		*/
        function calculatePinchZoom(startDistance, endDistance) {
            var percent = (endDistance / startDistance) * 1;
            return percent.toFixed(2);
        }


        /**
		* Returns the pinch direction, either IN or OUT for the given points
		* @return string Either {@link $.fn.swipe.directions.IN} or {@link $.fn.swipe.directions.OUT}
		* @see $.fn.swipe.directions
		* @inner
		*/
        function calculatePinchDirection() {
            if (pinchZoom < 1) {
                return OUT;
            }
            else {
                return IN;
            }
        }


        /**
		* Calculate the length / distance of the swipe
		* @param {point} startPoint A point object containing x and y co-ordinates
	    * @param {point} endPoint A point object containing x and y co-ordinates
	    * @return int
		* @inner
		*/
        function calculateDistance(startPoint, endPoint) {
            return Math.round(Math.sqrt(Math.pow(endPoint.x - startPoint.x, 2) + Math.pow(endPoint.y - startPoint.y, 2)));
        }

        /**
		* Calculate the angle of the swipe
		* @param {point} startPoint A point object containing x and y co-ordinates
	    * @param {point} endPoint A point object containing x and y co-ordinates
	    * @return int
		* @inner
		*/
        function calculateAngle(startPoint, endPoint) {
            var x = startPoint.x - endPoint.x;
            var y = endPoint.y - startPoint.y;
            var r = Math.atan2(y, x); //radians
            var angle = Math.round(r * 180 / Math.PI); //degrees

            //ensure value is positive
            if (angle < 0) {
                angle = 360 - Math.abs(angle);
            }

            return angle;
        }

        /**
		* Calculate the direction of the swipe
		* This will also call calculateAngle to get the latest angle of swipe
		* @param {point} startPoint A point object containing x and y co-ordinates
	    * @param {point} endPoint A point object containing x and y co-ordinates
	    * @return string Either {@link $.fn.swipe.directions.LEFT} / {@link $.fn.swipe.directions.RIGHT} / {@link $.fn.swipe.directions.DOWN} / {@link $.fn.swipe.directions.UP}
		* @see $.fn.swipe.directions
		* @inner
		*/
        function calculateDirection(startPoint, endPoint) {
            var angle = calculateAngle(startPoint, endPoint);

            if ((angle <= 45) && (angle >= 0)) {
                return LEFT;
            } else if ((angle <= 360) && (angle >= 315)) {
                return LEFT;
            } else if ((angle >= 135) && (angle <= 225)) {
                return RIGHT;
            } else if ((angle > 45) && (angle < 135)) {
                return DOWN;
            } else {
                return UP;
            }
        }


        /**
		* Returns a MS time stamp of the current time
		* @return int
		* @inner
		*/
        function getTimeStamp() {
            var now = new Date();
            return now.getTime();
        }



        /**
		 * Returns a bounds object with left, right, top and bottom properties for the element specified.
		 * @param {DomNode} The DOM node to get the bounds for.
		 */
        function getbounds(el) {
            el = $(el);
            var offset = el.offset();

            var bounds = {
                left: offset.left,
                right: offset.left + el.outerWidth(),
                top: offset.top,
                bottom: offset.top + el.outerHeight()
            }

            return bounds;
        }


        /**
		 * Checks if the point object is in the bounds object.
		 * @param {object} point A point object.
		 * @param {int} point.x The x value of the point.
		 * @param {int} point.y The x value of the point.
		 * @param {object} bounds The bounds object to test
		 * @param {int} bounds.left The leftmost value
		 * @param {int} bounds.right The righttmost value
		 * @param {int} bounds.top The topmost value
		* @param {int} bounds.bottom The bottommost value
		 */
        function isInBounds(point, bounds) {
            return (point.x > bounds.left && point.x < bounds.right && point.y > bounds.top && point.y < bounds.bottom);
        };


    }




})(jQuery);



/*!
 * Masonry PACKAGED v3.3.0
 * Cascading grid layout library
 * http://masonry.desandro.com
 * MIT License
 * by David DeSandro
 */

!function (a) { function b() { } function c(a) { function c(b) { b.prototype.option || (b.prototype.option = function (b) { a.isPlainObject(b) && (this.options = a.extend(!0, this.options, b)) }) } function e(b, c) { a.fn[b] = function (e) { if ("string" == typeof e) { for (var g = d.call(arguments, 1), h = 0, i = this.length; i > h; h++) { var j = this[h], k = a.data(j, b); if (k) if (a.isFunction(k[e]) && "_" !== e.charAt(0)) { var l = k[e].apply(k, g); if (void 0 !== l) return l } else f("no such method '" + e + "' for " + b + " instance"); else f("cannot call methods on " + b + " prior to initialization; attempted to call '" + e + "'") } return this } return this.each(function () { var d = a.data(this, b); d ? (d.option(e), d._init()) : (d = new c(this, e), a.data(this, b, d)) }) } } if (a) { var f = "undefined" == typeof console ? b : function (a) { console.error(a) }; return a.bridget = function (a, b) { c(b), e(a, b) }, a.bridget } } var d = Array.prototype.slice; "function" == typeof define && define.amd ? define("jquery-bridget/jquery.bridget", ["jquery"], c) : c("object" == typeof exports ? require("jquery") : a.jQuery) }(window), function (a) { function b(b) { var c = a.event; return c.target = c.target || c.srcElement || b, c } var c = document.documentElement, d = function () { }; c.addEventListener ? d = function (a, b, c) { a.addEventListener(b, c, !1) } : c.attachEvent && (d = function (a, c, d) { a[c + d] = d.handleEvent ? function () { var c = b(a); d.handleEvent.call(d, c) } : function () { var c = b(a); d.call(a, c) }, a.attachEvent("on" + c, a[c + d]) }); var e = function () { }; c.removeEventListener ? e = function (a, b, c) { a.removeEventListener(b, c, !1) } : c.detachEvent && (e = function (a, b, c) { a.detachEvent("on" + b, a[b + c]); try { delete a[b + c] } catch (d) { a[b + c] = void 0 } }); var f = { bind: d, unbind: e }; "function" == typeof define && define.amd ? define("eventie/eventie", f) : "object" == typeof exports ? module.exports = f : a.eventie = f }(window), function () { function a() { } function b(a, b) { for (var c = a.length; c--;) if (a[c].listener === b) return c; return -1 } function c(a) { return function () { return this[a].apply(this, arguments) } } var d = a.prototype, e = this, f = e.EventEmitter; d.getListeners = function (a) { var b, c, d = this._getEvents(); if (a instanceof RegExp) { b = {}; for (c in d) d.hasOwnProperty(c) && a.test(c) && (b[c] = d[c]) } else b = d[a] || (d[a] = []); return b }, d.flattenListeners = function (a) { var b, c = []; for (b = 0; b < a.length; b += 1) c.push(a[b].listener); return c }, d.getListenersAsObject = function (a) { var b, c = this.getListeners(a); return c instanceof Array && (b = {}, b[a] = c), b || c }, d.addListener = function (a, c) { var d, e = this.getListenersAsObject(a), f = "object" == typeof c; for (d in e) e.hasOwnProperty(d) && -1 === b(e[d], c) && e[d].push(f ? c : { listener: c, once: !1 }); return this }, d.on = c("addListener"), d.addOnceListener = function (a, b) { return this.addListener(a, { listener: b, once: !0 }) }, d.once = c("addOnceListener"), d.defineEvent = function (a) { return this.getListeners(a), this }, d.defineEvents = function (a) { for (var b = 0; b < a.length; b += 1) this.defineEvent(a[b]); return this }, d.removeListener = function (a, c) { var d, e, f = this.getListenersAsObject(a); for (e in f) f.hasOwnProperty(e) && (d = b(f[e], c), -1 !== d && f[e].splice(d, 1)); return this }, d.off = c("removeListener"), d.addListeners = function (a, b) { return this.manipulateListeners(!1, a, b) }, d.removeListeners = function (a, b) { return this.manipulateListeners(!0, a, b) }, d.manipulateListeners = function (a, b, c) { var d, e, f = a ? this.removeListener : this.addListener, g = a ? this.removeListeners : this.addListeners; if ("object" != typeof b || b instanceof RegExp) for (d = c.length; d--;) f.call(this, b, c[d]); else for (d in b) b.hasOwnProperty(d) && (e = b[d]) && ("function" == typeof e ? f.call(this, d, e) : g.call(this, d, e)); return this }, d.removeEvent = function (a) { var b, c = typeof a, d = this._getEvents(); if ("string" === c) delete d[a]; else if (a instanceof RegExp) for (b in d) d.hasOwnProperty(b) && a.test(b) && delete d[b]; else delete this._events; return this }, d.removeAllListeners = c("removeEvent"), d.emitEvent = function (a, b) { var c, d, e, f, g = this.getListenersAsObject(a); for (e in g) if (g.hasOwnProperty(e)) for (d = g[e].length; d--;) c = g[e][d], c.once === !0 && this.removeListener(a, c.listener), f = c.listener.apply(this, b || []), f === this._getOnceReturnValue() && this.removeListener(a, c.listener); return this }, d.trigger = c("emitEvent"), d.emit = function (a) { var b = Array.prototype.slice.call(arguments, 1); return this.emitEvent(a, b) }, d.setOnceReturnValue = function (a) { return this._onceReturnValue = a, this }, d._getOnceReturnValue = function () { return this.hasOwnProperty("_onceReturnValue") ? this._onceReturnValue : !0 }, d._getEvents = function () { return this._events || (this._events = {}) }, a.noConflict = function () { return e.EventEmitter = f, a }, "function" == typeof define && define.amd ? define("eventEmitter/EventEmitter", [], function () { return a }) : "object" == typeof module && module.exports ? module.exports = a : e.EventEmitter = a }.call(this), function (a) { function b(a) { if (a) { if ("string" == typeof d[a]) return a; a = a.charAt(0).toUpperCase() + a.slice(1); for (var b, e = 0, f = c.length; f > e; e++) if (b = c[e] + a, "string" == typeof d[b]) return b } } var c = "Webkit Moz ms Ms O".split(" "), d = document.documentElement.style; "function" == typeof define && define.amd ? define("get-style-property/get-style-property", [], function () { return b }) : "object" == typeof exports ? module.exports = b : a.getStyleProperty = b }(window), function (a) { function b(a) { var b = parseFloat(a), c = -1 === a.indexOf("%") && !isNaN(b); return c && b } function c() { } function d() { for (var a = { width: 0, height: 0, innerWidth: 0, innerHeight: 0, outerWidth: 0, outerHeight: 0 }, b = 0, c = g.length; c > b; b++) { var d = g[b]; a[d] = 0 } return a } function e(c) { function e() { if (!m) { m = !0; var d = a.getComputedStyle; if (j = function () { var a = d ? function (a) { return d(a, null) } : function (a) { return a.currentStyle }; return function (b) { var c = a(b); return c || f("Style returned " + c + ". Are you running this code in a hidden iframe on Firefox? See http://bit.ly/getsizebug1"), c } }(), k = c("boxSizing")) { var e = document.createElement("div"); e.style.width = "200px", e.style.padding = "1px 2px 3px 4px", e.style.borderStyle = "solid", e.style.borderWidth = "1px 2px 3px 4px", e.style[k] = "border-box"; var g = document.body || document.documentElement; g.appendChild(e); var h = j(e); l = 200 === b(h.width), g.removeChild(e) } } } function h(a) { if (e(), "string" == typeof a && (a = document.querySelector(a)), a && "object" == typeof a && a.nodeType) { var c = j(a); if ("none" === c.display) return d(); var f = {}; f.width = a.offsetWidth, f.height = a.offsetHeight; for (var h = f.isBorderBox = !(!k || !c[k] || "border-box" !== c[k]), m = 0, n = g.length; n > m; m++) { var o = g[m], p = c[o]; p = i(a, p); var q = parseFloat(p); f[o] = isNaN(q) ? 0 : q } var r = f.paddingLeft + f.paddingRight, s = f.paddingTop + f.paddingBottom, t = f.marginLeft + f.marginRight, u = f.marginTop + f.marginBottom, v = f.borderLeftWidth + f.borderRightWidth, w = f.borderTopWidth + f.borderBottomWidth, x = h && l, y = b(c.width); y !== !1 && (f.width = y + (x ? 0 : r + v)); var z = b(c.height); return z !== !1 && (f.height = z + (x ? 0 : s + w)), f.innerWidth = f.width - (r + v), f.innerHeight = f.height - (s + w), f.outerWidth = f.width + t, f.outerHeight = f.height + u, f } } function i(b, c) { if (a.getComputedStyle || -1 === c.indexOf("%")) return c; var d = b.style, e = d.left, f = b.runtimeStyle, g = f && f.left; return g && (f.left = b.currentStyle.left), d.left = c, c = d.pixelLeft, d.left = e, g && (f.left = g), c } var j, k, l, m = !1; return h } var f = "undefined" == typeof console ? c : function (a) { console.error(a) }, g = ["paddingLeft", "paddingRight", "paddingTop", "paddingBottom", "marginLeft", "marginRight", "marginTop", "marginBottom", "borderLeftWidth", "borderRightWidth", "borderTopWidth", "borderBottomWidth"]; "function" == typeof define && define.amd ? define("get-size/get-size", ["get-style-property/get-style-property"], e) : "object" == typeof exports ? module.exports = e(require("desandro-get-style-property")) : a.getSize = e(a.getStyleProperty) }(window), function (a) { function b(a) { "function" == typeof a && (b.isReady ? a() : g.push(a)) } function c(a) { var c = "readystatechange" === a.type && "complete" !== f.readyState; b.isReady || c || d() } function d() { b.isReady = !0; for (var a = 0, c = g.length; c > a; a++) { var d = g[a]; d() } } function e(e) { return "complete" === f.readyState ? d() : (e.bind(f, "DOMContentLoaded", c), e.bind(f, "readystatechange", c), e.bind(a, "load", c)), b } var f = a.document, g = []; b.isReady = !1, "function" == typeof define && define.amd ? define("doc-ready/doc-ready", ["eventie/eventie"], e) : "object" == typeof exports ? module.exports = e(require("eventie")) : a.docReady = e(a.eventie) }(window), function (a) { function b(a, b) { return a[g](b) } function c(a) { if (!a.parentNode) { var b = document.createDocumentFragment(); b.appendChild(a) } } function d(a, b) { c(a); for (var d = a.parentNode.querySelectorAll(b), e = 0, f = d.length; f > e; e++) if (d[e] === a) return !0; return !1 } function e(a, d) { return c(a), b(a, d) } var f, g = function () { if (a.matches) return "matches"; if (a.matchesSelector) return "matchesSelector"; for (var b = ["webkit", "moz", "ms", "o"], c = 0, d = b.length; d > c; c++) { var e = b[c], f = e + "MatchesSelector"; if (a[f]) return f } }(); if (g) { var h = document.createElement("div"), i = b(h, "div"); f = i ? b : e } else f = d; "function" == typeof define && define.amd ? define("matches-selector/matches-selector", [], function () { return f }) : "object" == typeof exports ? module.exports = f : window.matchesSelector = f }(Element.prototype), function (a, b) { "function" == typeof define && define.amd ? define("fizzy-ui-utils/utils", ["doc-ready/doc-ready", "matches-selector/matches-selector"], function (c, d) { return b(a, c, d) }) : "object" == typeof exports ? module.exports = b(a, require("doc-ready"), require("desandro-matches-selector")) : a.fizzyUIUtils = b(a, a.docReady, a.matchesSelector) }(window, function (a, b, c) { var d = {}; d.extend = function (a, b) { for (var c in b) a[c] = b[c]; return a }, d.modulo = function (a, b) { return (a % b + b) % b }; var e = Object.prototype.toString; d.isArray = function (a) { return "[object Array]" == e.call(a) }, d.makeArray = function (a) { var b = []; if (d.isArray(a)) b = a; else if (a && "number" == typeof a.length) for (var c = 0, e = a.length; e > c; c++) b.push(a[c]); else b.push(a); return b }, d.indexOf = Array.prototype.indexOf ? function (a, b) { return a.indexOf(b) } : function (a, b) { for (var c = 0, d = a.length; d > c; c++) if (a[c] === b) return c; return -1 }, d.removeFrom = function (a, b) { var c = d.indexOf(a, b); -1 != c && a.splice(c, 1) }, d.isElement = "function" == typeof HTMLElement || "object" == typeof HTMLElement ? function (a) { return a instanceof HTMLElement } : function (a) { return a && "object" == typeof a && 1 == a.nodeType && "string" == typeof a.nodeName }, d.setText = function () { function a(a, c) { b = b || (void 0 !== document.documentElement.textContent ? "textContent" : "innerText"), a[b] = c } var b; return a }(), d.getParent = function (a, b) { for (; a != document.body;) if (a = a.parentNode, c(a, b)) return a }, d.getQueryElement = function (a) { return "string" == typeof a ? document.querySelector(a) : a }, d.handleEvent = function (a) { var b = "on" + a.type; this[b] && this[b](a) }, d.filterFindElements = function (a, b) { a = d.makeArray(a); for (var e = [], f = 0, g = a.length; g > f; f++) { var h = a[f]; if (d.isElement(h)) if (b) { c(h, b) && e.push(h); for (var i = h.querySelectorAll(b), j = 0, k = i.length; k > j; j++) e.push(i[j]) } else e.push(h) } return e }, d.debounceMethod = function (a, b, c) { var d = a.prototype[b], e = b + "Timeout"; a.prototype[b] = function () { var a = this[e]; a && clearTimeout(a); var b = arguments, f = this; this[e] = setTimeout(function () { d.apply(f, b), delete f[e] }, c || 100) } }, d.toDashed = function (a) { return a.replace(/(.)([A-Z])/g, function (a, b, c) { return b + "-" + c }).toLowerCase() }; var f = a.console; return d.htmlInit = function (c, e) { b(function () { for (var b = d.toDashed(e), g = document.querySelectorAll(".js-" + b), h = "data-" + b + "-options", i = 0, j = g.length; j > i; i++) { var k, l = g[i], m = l.getAttribute(h); try { k = m && JSON.parse(m) } catch (n) { f && f.error("Error parsing " + h + " on " + l.nodeName.toLowerCase() + (l.id ? "#" + l.id : "") + ": " + n); continue } var o = new c(l, k), p = a.jQuery; p && p.data(l, e, o) } }) }, d }), function (a, b) { "function" == typeof define && define.amd ? define("outlayer/item", ["eventEmitter/EventEmitter", "get-size/get-size", "get-style-property/get-style-property", "fizzy-ui-utils/utils"], function (c, d, e, f) { return b(a, c, d, e, f) }) : "object" == typeof exports ? module.exports = b(a, require("wolfy87-eventemitter"), require("get-size"), require("desandro-get-style-property"), require("fizzy-ui-utils")) : (a.Outlayer = {}, a.Outlayer.Item = b(a, a.EventEmitter, a.getSize, a.getStyleProperty, a.fizzyUIUtils)) }(window, function (a, b, c, d, e) { function f(a) { for (var b in a) return !1; return b = null, !0 } function g(a, b) { a && (this.element = a, this.layout = b, this.position = { x: 0, y: 0 }, this._create()) } var h = a.getComputedStyle, i = h ? function (a) { return h(a, null) } : function (a) { return a.currentStyle }, j = d("transition"), k = d("transform"), l = j && k, m = !!d("perspective"), n = { WebkitTransition: "webkitTransitionEnd", MozTransition: "transitionend", OTransition: "otransitionend", transition: "transitionend" }[j], o = ["transform", "transition", "transitionDuration", "transitionProperty"], p = function () { for (var a = {}, b = 0, c = o.length; c > b; b++) { var e = o[b], f = d(e); f && f !== e && (a[e] = f) } return a }(); e.extend(g.prototype, b.prototype), g.prototype._create = function () { this._transn = { ingProperties: {}, clean: {}, onEnd: {} }, this.css({ position: "absolute" }) }, g.prototype.handleEvent = function (a) { var b = "on" + a.type; this[b] && this[b](a) }, g.prototype.getSize = function () { this.size = c(this.element) }, g.prototype.css = function (a) { var b = this.element.style; for (var c in a) { var d = p[c] || c; b[d] = a[c] } }, g.prototype.getPosition = function () { var a = i(this.element), b = this.layout.options, c = b.isOriginLeft, d = b.isOriginTop, e = parseInt(a[c ? "left" : "right"], 10), f = parseInt(a[d ? "top" : "bottom"], 10); e = isNaN(e) ? 0 : e, f = isNaN(f) ? 0 : f; var g = this.layout.size; e -= c ? g.paddingLeft : g.paddingRight, f -= d ? g.paddingTop : g.paddingBottom, this.position.x = e, this.position.y = f }, g.prototype.layoutPosition = function () { var a = this.layout.size, b = this.layout.options, c = {}, d = b.isOriginLeft ? "paddingLeft" : "paddingRight", e = b.isOriginLeft ? "left" : "right", f = b.isOriginLeft ? "right" : "left", g = this.position.x + a[d]; g = b.percentPosition && !b.isHorizontal ? g / a.width * 100 + "%" : g + "px", c[e] = g, c[f] = ""; var h = b.isOriginTop ? "paddingTop" : "paddingBottom", i = b.isOriginTop ? "top" : "bottom", j = b.isOriginTop ? "bottom" : "top", k = this.position.y + a[h]; k = b.percentPosition && b.isHorizontal ? k / a.height * 100 + "%" : k + "px", c[i] = k, c[j] = "", this.css(c), this.emitEvent("layout", [this]) }; var q = m ? function (a, b) { return "translate3d(" + a + "px, " + b + "px, 0)" } : function (a, b) { return "translate(" + a + "px, " + b + "px)" }; g.prototype._transitionTo = function (a, b) { this.getPosition(); var c = this.position.x, d = this.position.y, e = parseInt(a, 10), f = parseInt(b, 10), g = e === this.position.x && f === this.position.y; if (this.setPosition(a, b), g && !this.isTransitioning) return void this.layoutPosition(); var h = a - c, i = b - d, j = {}, k = this.layout.options; h = k.isOriginLeft ? h : -h, i = k.isOriginTop ? i : -i, j.transform = q(h, i), this.transition({ to: j, onTransitionEnd: { transform: this.layoutPosition }, isCleaning: !0 }) }, g.prototype.goTo = function (a, b) { this.setPosition(a, b), this.layoutPosition() }, g.prototype.moveTo = l ? g.prototype._transitionTo : g.prototype.goTo, g.prototype.setPosition = function (a, b) { this.position.x = parseInt(a, 10), this.position.y = parseInt(b, 10) }, g.prototype._nonTransition = function (a) { this.css(a.to), a.isCleaning && this._removeStyles(a.to); for (var b in a.onTransitionEnd) a.onTransitionEnd[b].call(this) }, g.prototype._transition = function (a) { if (!parseFloat(this.layout.options.transitionDuration)) return void this._nonTransition(a); var b = this._transn; for (var c in a.onTransitionEnd) b.onEnd[c] = a.onTransitionEnd[c]; for (c in a.to) b.ingProperties[c] = !0, a.isCleaning && (b.clean[c] = !0); if (a.from) { this.css(a.from); var d = this.element.offsetHeight; d = null } this.enableTransition(a.to), this.css(a.to), this.isTransitioning = !0 }; var r = k && e.toDashed(k) + ",opacity"; g.prototype.enableTransition = function () { this.isTransitioning || (this.css({ transitionProperty: r, transitionDuration: this.layout.options.transitionDuration }), this.element.addEventListener(n, this, !1)) }, g.prototype.transition = g.prototype[j ? "_transition" : "_nonTransition"], g.prototype.onwebkitTransitionEnd = function (a) { this.ontransitionend(a) }, g.prototype.onotransitionend = function (a) { this.ontransitionend(a) }; var s = { "-webkit-transform": "transform", "-moz-transform": "transform", "-o-transform": "transform" }; g.prototype.ontransitionend = function (a) { if (a.target === this.element) { var b = this._transn, c = s[a.propertyName] || a.propertyName; if (delete b.ingProperties[c], f(b.ingProperties) && this.disableTransition(), c in b.clean && (this.element.style[a.propertyName] = "", delete b.clean[c]), c in b.onEnd) { var d = b.onEnd[c]; d.call(this), delete b.onEnd[c] } this.emitEvent("transitionEnd", [this]) } }, g.prototype.disableTransition = function () { this.removeTransitionStyles(), this.element.removeEventListener(n, this, !1), this.isTransitioning = !1 }, g.prototype._removeStyles = function (a) { var b = {}; for (var c in a) b[c] = ""; this.css(b) }; var t = { transitionProperty: "", transitionDuration: "" }; return g.prototype.removeTransitionStyles = function () { this.css(t) }, g.prototype.removeElem = function () { this.element.parentNode.removeChild(this.element), this.css({ display: "" }), this.emitEvent("remove", [this]) }, g.prototype.remove = function () { if (!j || !parseFloat(this.layout.options.transitionDuration)) return void this.removeElem(); var a = this; this.once("transitionEnd", function () { a.removeElem() }), this.hide() }, g.prototype.reveal = function () { delete this.isHidden, this.css({ display: "" }); var a = this.layout.options, b = {}, c = this.getHideRevealTransitionEndProperty("visibleStyle"); b[c] = this.onRevealTransitionEnd, this.transition({ from: a.hiddenStyle, to: a.visibleStyle, isCleaning: !0, onTransitionEnd: b }) }, g.prototype.onRevealTransitionEnd = function () { this.isHidden || this.emitEvent("reveal") }, g.prototype.getHideRevealTransitionEndProperty = function (a) { var b = this.layout.options[a]; if (b.opacity) return "opacity"; for (var c in b) return c }, g.prototype.hide = function () { this.isHidden = !0, this.css({ display: "" }); var a = this.layout.options, b = {}, c = this.getHideRevealTransitionEndProperty("hiddenStyle"); b[c] = this.onHideTransitionEnd, this.transition({ from: a.visibleStyle, to: a.hiddenStyle, isCleaning: !0, onTransitionEnd: b }) }, g.prototype.onHideTransitionEnd = function () { this.isHidden && (this.css({ display: "none" }), this.emitEvent("hide")) }, g.prototype.destroy = function () { this.css({ position: "", left: "", right: "", top: "", bottom: "", transition: "", transform: "" }) }, g }), function (a, b) { "function" == typeof define && define.amd ? define("outlayer/outlayer", ["eventie/eventie", "eventEmitter/EventEmitter", "get-size/get-size", "fizzy-ui-utils/utils", "./item"], function (c, d, e, f, g) { return b(a, c, d, e, f, g) }) : "object" == typeof exports ? module.exports = b(a, require("eventie"), require("wolfy87-eventemitter"), require("get-size"), require("fizzy-ui-utils"), require("./item")) : a.Outlayer = b(a, a.eventie, a.EventEmitter, a.getSize, a.fizzyUIUtils, a.Outlayer.Item) }(window, function (a, b, c, d, e, f) { function g(a, b) { var c = e.getQueryElement(a); if (!c) return void (h && h.error("Bad element for " + this.constructor.namespace + ": " + (c || a))); this.element = c, i && (this.$element = i(this.element)), this.options = e.extend({}, this.constructor.defaults), this.option(b); var d = ++k; this.element.outlayerGUID = d, l[d] = this, this._create(), this.options.isInitLayout && this.layout() } var h = a.console, i = a.jQuery, j = function () { }, k = 0, l = {}; return g.namespace = "outlayer", g.Item = f, g.defaults = { containerStyle: { position: "relative" }, isInitLayout: !0, isOriginLeft: !0, isOriginTop: !0, isResizeBound: !0, isResizingContainer: !0, transitionDuration: "0.4s", hiddenStyle: { opacity: 0, transform: "scale(0.001)" }, visibleStyle: { opacity: 1, transform: "scale(1)" } }, e.extend(g.prototype, c.prototype), g.prototype.option = function (a) { e.extend(this.options, a) }, g.prototype._create = function () { this.reloadItems(), this.stamps = [], this.stamp(this.options.stamp), e.extend(this.element.style, this.options.containerStyle), this.options.isResizeBound && this.bindResize() }, g.prototype.reloadItems = function () { this.items = this._itemize(this.element.children) }, g.prototype._itemize = function (a) { for (var b = this._filterFindItemElements(a), c = this.constructor.Item, d = [], e = 0, f = b.length; f > e; e++) { var g = b[e], h = new c(g, this); d.push(h) } return d }, g.prototype._filterFindItemElements = function (a) { return e.filterFindElements(a, this.options.itemSelector) }, g.prototype.getItemElements = function () { for (var a = [], b = 0, c = this.items.length; c > b; b++) a.push(this.items[b].element); return a }, g.prototype.layout = function () { this._resetLayout(), this._manageStamps(); var a = void 0 !== this.options.isLayoutInstant ? this.options.isLayoutInstant : !this._isLayoutInited; this.layoutItems(this.items, a), this._isLayoutInited = !0 }, g.prototype._init = g.prototype.layout, g.prototype._resetLayout = function () { this.getSize() }, g.prototype.getSize = function () { this.size = d(this.element) }, g.prototype._getMeasurement = function (a, b) { var c, f = this.options[a]; f ? ("string" == typeof f ? c = this.element.querySelector(f) : e.isElement(f) && (c = f), this[a] = c ? d(c)[b] : f) : this[a] = 0 }, g.prototype.layoutItems = function (a, b) { a = this._getItemsForLayout(a), this._layoutItems(a, b), this._postLayout() }, g.prototype._getItemsForLayout = function (a) { for (var b = [], c = 0, d = a.length; d > c; c++) { var e = a[c]; e.isIgnored || b.push(e) } return b }, g.prototype._layoutItems = function (a, b) { if (this._emitCompleteOnItems("layout", a), a && a.length) { for (var c = [], d = 0, e = a.length; e > d; d++) { var f = a[d], g = this._getItemLayoutPosition(f); g.item = f, g.isInstant = b || f.isLayoutInstant, c.push(g) } this._processLayoutQueue(c) } }, g.prototype._getItemLayoutPosition = function () { return { x: 0, y: 0 } }, g.prototype._processLayoutQueue = function (a) { for (var b = 0, c = a.length; c > b; b++) { var d = a[b]; this._positionItem(d.item, d.x, d.y, d.isInstant) } }, g.prototype._positionItem = function (a, b, c, d) { d ? a.goTo(b, c) : a.moveTo(b, c) }, g.prototype._postLayout = function () { this.resizeContainer() }, g.prototype.resizeContainer = function () { if (this.options.isResizingContainer) { var a = this._getContainerSize(); a && (this._setContainerMeasure(a.width, !0), this._setContainerMeasure(a.height, !1)) } }, g.prototype._getContainerSize = j, g.prototype._setContainerMeasure = function (a, b) { if (void 0 !== a) { var c = this.size; c.isBorderBox && (a += b ? c.paddingLeft + c.paddingRight + c.borderLeftWidth + c.borderRightWidth : c.paddingBottom + c.paddingTop + c.borderTopWidth + c.borderBottomWidth), a = Math.max(a, 0), this.element.style[b ? "width" : "height"] = a + "px" } }, g.prototype._emitCompleteOnItems = function (a, b) { function c() { e.emitEvent(a + "Complete", [b]) } function d() { g++, g === f && c() } var e = this, f = b.length; if (!b || !f) return void c(); for (var g = 0, h = 0, i = b.length; i > h; h++) { var j = b[h]; j.once(a, d) } }, g.prototype.ignore = function (a) { var b = this.getItem(a); b && (b.isIgnored = !0) }, g.prototype.unignore = function (a) { var b = this.getItem(a); b && delete b.isIgnored }, g.prototype.stamp = function (a) { if (a = this._find(a)) { this.stamps = this.stamps.concat(a); for (var b = 0, c = a.length; c > b; b++) { var d = a[b]; this.ignore(d) } } }, g.prototype.unstamp = function (a) { if (a = this._find(a)) for (var b = 0, c = a.length; c > b; b++) { var d = a[b]; e.removeFrom(this.stamps, d), this.unignore(d) } }, g.prototype._find = function (a) { return a ? ("string" == typeof a && (a = this.element.querySelectorAll(a)), a = e.makeArray(a)) : void 0 }, g.prototype._manageStamps = function () { if (this.stamps && this.stamps.length) { this._getBoundingRect(); for (var a = 0, b = this.stamps.length; b > a; a++) { var c = this.stamps[a]; this._manageStamp(c) } } }, g.prototype._getBoundingRect = function () { var a = this.element.getBoundingClientRect(), b = this.size; this._boundingRect = { left: a.left + b.paddingLeft + b.borderLeftWidth, top: a.top + b.paddingTop + b.borderTopWidth, right: a.right - (b.paddingRight + b.borderRightWidth), bottom: a.bottom - (b.paddingBottom + b.borderBottomWidth) } }, g.prototype._manageStamp = j, g.prototype._getElementOffset = function (a) { var b = a.getBoundingClientRect(), c = this._boundingRect, e = d(a), f = { left: b.left - c.left - e.marginLeft, top: b.top - c.top - e.marginTop, right: c.right - b.right - e.marginRight, bottom: c.bottom - b.bottom - e.marginBottom }; return f }, g.prototype.handleEvent = function (a) { var b = "on" + a.type; this[b] && this[b](a) }, g.prototype.bindResize = function () { this.isResizeBound || (b.bind(a, "resize", this), this.isResizeBound = !0) }, g.prototype.unbindResize = function () { this.isResizeBound && b.unbind(a, "resize", this), this.isResizeBound = !1 }, g.prototype.onresize = function () { function a() { b.resize(), delete b.resizeTimeout } this.resizeTimeout && clearTimeout(this.resizeTimeout); var b = this; this.resizeTimeout = setTimeout(a, 100) }, g.prototype.resize = function () { this.isResizeBound && this.needsResizeLayout() && this.layout() }, g.prototype.needsResizeLayout = function () { var a = d(this.element), b = this.size && a; return b && a.innerWidth !== this.size.innerWidth }, g.prototype.addItems = function (a) { var b = this._itemize(a); return b.length && (this.items = this.items.concat(b)), b }, g.prototype.appended = function (a) { var b = this.addItems(a); b.length && (this.layoutItems(b, !0), this.reveal(b)) }, g.prototype.prepended = function (a) { var b = this._itemize(a); if (b.length) { var c = this.items.slice(0); this.items = b.concat(c), this._resetLayout(), this._manageStamps(), this.layoutItems(b, !0), this.reveal(b), this.layoutItems(c) } }, g.prototype.reveal = function (a) { this._emitCompleteOnItems("reveal", a); for (var b = a && a.length, c = 0; b && b > c; c++) { var d = a[c]; d.reveal() } }, g.prototype.hide = function (a) { this._emitCompleteOnItems("hide", a); for (var b = a && a.length, c = 0; b && b > c; c++) { var d = a[c]; d.hide() } }, g.prototype.revealItemElements = function (a) { var b = this.getItems(a); this.reveal(b) }, g.prototype.hideItemElements = function (a) { var b = this.getItems(a); this.hide(b) }, g.prototype.getItem = function (a) { for (var b = 0, c = this.items.length; c > b; b++) { var d = this.items[b]; if (d.element === a) return d } }, g.prototype.getItems = function (a) { a = e.makeArray(a); for (var b = [], c = 0, d = a.length; d > c; c++) { var f = a[c], g = this.getItem(f); g && b.push(g) } return b }, g.prototype.remove = function (a) { var b = this.getItems(a); if (this._emitCompleteOnItems("remove", b), b && b.length) for (var c = 0, d = b.length; d > c; c++) { var f = b[c]; f.remove(), e.removeFrom(this.items, f) } }, g.prototype.destroy = function () { var a = this.element.style; a.height = "", a.position = "", a.width = ""; for (var b = 0, c = this.items.length; c > b; b++) { var d = this.items[b]; d.destroy() } this.unbindResize(); var e = this.element.outlayerGUID; delete l[e], delete this.element.outlayerGUID, i && i.removeData(this.element, this.constructor.namespace) }, g.data = function (a) { a = e.getQueryElement(a); var b = a && a.outlayerGUID; return b && l[b] }, g.create = function (a, b) { function c() { g.apply(this, arguments) } return Object.create ? c.prototype = Object.create(g.prototype) : e.extend(c.prototype, g.prototype), c.prototype.constructor = c, c.defaults = e.extend({}, g.defaults), e.extend(c.defaults, b), c.prototype.settings = {}, c.namespace = a, c.data = g.data, c.Item = function () { f.apply(this, arguments) }, c.Item.prototype = new f, e.htmlInit(c, a), i && i.bridget && i.bridget(a, c), c }, g.Item = f, g }), function (a, b) { "function" == typeof define && define.amd ? define(["outlayer/outlayer", "get-size/get-size", "fizzy-ui-utils/utils"], b) : "object" == typeof exports ? module.exports = b(require("outlayer"), require("get-size"), require("fizzy-ui-utils")) : a.Masonry = b(a.Outlayer, a.getSize, a.fizzyUIUtils) }(window, function (a, b, c) { var d = a.create("masonry"); return d.prototype._resetLayout = function () { this.getSize(), this._getMeasurement("columnWidth", "outerWidth"), this._getMeasurement("gutter", "outerWidth"), this.measureColumns(); var a = this.cols; for (this.colYs = []; a--;) this.colYs.push(0); this.maxY = 0 }, d.prototype.measureColumns = function () { if (this.getContainerWidth(), !this.columnWidth) { var a = this.items[0], c = a && a.element; this.columnWidth = c && b(c).outerWidth || this.containerWidth } var d = this.columnWidth += this.gutter, e = this.containerWidth + this.gutter, f = e / d, g = d - e % d, h = g && 1 > g ? "round" : "floor"; f = Math[h](f), this.cols = Math.max(f, 1) }, d.prototype.getContainerWidth = function () { var a = this.options.isFitWidth ? this.element.parentNode : this.element, c = b(a); this.containerWidth = c && c.innerWidth }, d.prototype._getItemLayoutPosition = function (a) { a.getSize(); var b = a.size.outerWidth % this.columnWidth, d = b && 1 > b ? "round" : "ceil", e = Math[d](a.size.outerWidth / this.columnWidth); e = Math.min(e, this.cols); for (var f = this._getColGroup(e), g = Math.min.apply(Math, f), h = c.indexOf(f, g), i = { x: this.columnWidth * h, y: g }, j = g + a.size.outerHeight, k = this.cols + 1 - f.length, l = 0; k > l; l++) this.colYs[h + l] = j; return i }, d.prototype._getColGroup = function (a) { if (2 > a) return this.colYs; for (var b = [], c = this.cols + 1 - a, d = 0; c > d; d++) { var e = this.colYs.slice(d, d + a); b[d] = Math.max.apply(Math, e) } return b }, d.prototype._manageStamp = function (a) { var c = b(a), d = this._getElementOffset(a), e = this.options.isOriginLeft ? d.left : d.right, f = e + c.outerWidth, g = Math.floor(e / this.columnWidth); g = Math.max(0, g); var h = Math.floor(f / this.columnWidth); h -= f % this.columnWidth ? 0 : 1, h = Math.min(this.cols - 1, h); for (var i = (this.options.isOriginTop ? d.top : d.bottom) + c.outerHeight, j = g; h >= j; j++) this.colYs[j] = Math.max(i, this.colYs[j]) }, d.prototype._getContainerSize = function () { this.maxY = Math.max.apply(Math, this.colYs); var a = { height: this.maxY }; return this.options.isFitWidth && (a.width = this._getContainerFitWidth()), a }, d.prototype._getContainerFitWidth = function () { for (var a = 0, b = this.cols; --b && 0 === this.colYs[b];) a++; return (this.cols - a) * this.columnWidth - this.gutter }, d.prototype.needsResizeLayout = function () { var a = this.containerWidth; return this.getContainerWidth(), a !== this.containerWidth }, d });


/* jquery.nicescroll
-- version 3.5.4
-- copyright 2013-11-13 InuYaksa*2013
-- licensed under the MIT
--
-- http://areaaperta.com/nicescroll
-- https://github.com/inuyaksa/jquery.nicescroll
--
*/

(function (factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD. Register as anonymous module.
        define(['jquery'], factory);
    } else {
        // Browser globals.
        factory(jQuery);
    }
}(function (jQuery) {

    // globals
    var domfocus = false;
    var mousefocus = false;
    var zoomactive = false;
    var tabindexcounter = 5000;
    var ascrailcounter = 2000;
    var globalmaxzindex = 0;

    var $ = jQuery;  // sandbox

    // http://stackoverflow.com/questions/2161159/get-script-path
    function getScriptPath() {
        var scripts = document.getElementsByTagName('script');
        var path = scripts[scripts.length - 1].src.split('?')[0];
        return (path.split('/').length > 0) ? path.split('/').slice(0, -1).join('/') + '/' : '';
    }
    //  var scriptpath = getScriptPath();

    var vendors = ['ms', 'moz', 'webkit', 'o'];

    var setAnimationFrame = window.requestAnimationFrame || false;
    var clearAnimationFrame = window.cancelAnimationFrame || false;

    if (!setAnimationFrame) {
        for (var vx in vendors) {
            var v = vendors[vx];
            if (!setAnimationFrame) setAnimationFrame = window[v + 'RequestAnimationFrame'];
            if (!clearAnimationFrame) clearAnimationFrame = window[v + 'CancelAnimationFrame'] || window[v + 'CancelRequestAnimationFrame'];
        }
    }

    var clsMutationObserver = window.MutationObserver || window.WebKitMutationObserver || false;

    var _globaloptions = {
        zindex: "auto",
        cursoropacitymin: 1,
        cursoropacitymax: 1,
        cursorcolor: "rgb(7, 129, 192)",
        cursorwidth: "6px",
        cursorborder: "0px solid #ffffff",
        cursorborderradius: "5px",
        scrollspeed: 120,
        mousescrollstep: 8 * 3,
        touchbehavior: false,
        hwacceleration: true,
        usetransition: true,
        boxzoom: false,
        dblclickzoom: true,
        gesturezoom: true,
        grabcursorenabled: true,
        autohidemode: true,
        background: "rgba(255,255,255, 0)",
        iframeautoresize: true,
        cursorminheight: 60,
        preservenativescrolling: true,
        railoffset: true,
        bouncescroll: true,
        spacebarenabled: true,
        railpadding: { top: 0, right: 0, left: 0, bottom: 0 },
        disableoutline: true,
        horizrailenabled: true,
        railalign: "right",
        railvalign: "bottom",
        enabletranslate3d: true,
        enablemousewheel: true,
        enablekeyboard: true,
        smoothscroll: true,
        sensitiverail: true,
        enablemouselockapi: true,
        //      cursormaxheight:false,
        cursorfixedheight: false,
        directionlockdeadzone: 6,
        hidecursordelay: 400,
        nativeparentscrolling: true,
        enablescrollonselection: true,
        overflowx: true,
        overflowy: true,
        cursordragspeed: 0.3,
        rtlmode: "auto",
        cursordragontouch: false,
        oneaxismousemode: "auto",
        scriptpath: getScriptPath()
    };

    var browserdetected = false;

    var getBrowserDetection = function () {

        if (browserdetected) return browserdetected;

        var domtest = document.createElement('DIV');

        var d = {};

        d.haspointerlock = "pointerLockElement" in document || "mozPointerLockElement" in document || "webkitPointerLockElement" in document;

        d.isopera = ("opera" in window);
        d.isopera12 = (d.isopera && ("getUserMedia" in navigator));
        d.isoperamini = (Object.prototype.toString.call(window.operamini) === "[object OperaMini]");

        d.isie = (("all" in document) && ("attachEvent" in domtest) && !d.isopera);
        d.isieold = (d.isie && !("msInterpolationMode" in domtest.style));  // IE6 and older
        d.isie7 = d.isie && !d.isieold && (!("documentMode" in document) || (document.documentMode == 7));
        d.isie8 = d.isie && ("documentMode" in document) && (document.documentMode == 8);
        d.isie9 = d.isie && ("performance" in window) && (document.documentMode >= 9);
        d.isie10 = d.isie && ("performance" in window) && (document.documentMode >= 10);

        d.isie9mobile = /iemobile.9/i.test(navigator.userAgent);  //wp 7.1 mango
        if (d.isie9mobile) d.isie9 = false;
        d.isie7mobile = (!d.isie9mobile && d.isie7) && /iemobile/i.test(navigator.userAgent);  //wp 7.0

        d.ismozilla = ("MozAppearance" in domtest.style);

        d.iswebkit = ("WebkitAppearance" in domtest.style);

        d.ischrome = ("chrome" in window);
        d.ischrome22 = (d.ischrome && d.haspointerlock);
        d.ischrome26 = (d.ischrome && ("transition" in domtest.style));  // issue with transform detection (maintain prefix)

        d.cantouch = ("ontouchstart" in document.documentElement) || ("ontouchstart" in window);  // detection for Chrome Touch Emulation
        d.hasmstouch = (window.navigator.msPointerEnabled || false);  // IE10+ pointer events

        d.ismac = /^mac$/i.test(navigator.platform);

        d.isios = (d.cantouch && /iphone|ipad|ipod/i.test(navigator.platform));
        d.isios4 = ((d.isios) && !("seal" in Object));

        d.isandroid = (/android/i.test(navigator.userAgent));

        d.trstyle = false;
        d.hastransform = false;
        d.hastranslate3d = false;
        d.transitionstyle = false;
        d.hastransition = false;
        d.transitionend = false;

        var check = ['transform', 'msTransform', 'webkitTransform', 'MozTransform', 'OTransform'];
        for (var a = 0; a < check.length; a++) {
            if (typeof domtest.style[check[a]] != "undefined") {
                d.trstyle = check[a];
                break;
            }
        }
        d.hastransform = (d.trstyle != false);
        if (d.hastransform) {
            domtest.style[d.trstyle] = "translate3d(1px,2px,3px)";
            d.hastranslate3d = /translate3d/.test(domtest.style[d.trstyle]);
        }

        d.transitionstyle = false;
        d.prefixstyle = '';
        d.transitionend = false;
        var check = ['transition', 'webkitTransition', 'MozTransition', 'OTransition', 'OTransition', 'msTransition', 'KhtmlTransition'];
        var prefix = ['', '-webkit-', '-moz-', '-o-', '-o', '-ms-', '-khtml-'];
        var evs = ['transitionend', 'webkitTransitionEnd', 'transitionend', 'otransitionend', 'oTransitionEnd', 'msTransitionEnd', 'KhtmlTransitionEnd'];
        for (var a = 0; a < check.length; a++) {
            if (check[a] in domtest.style) {
                d.transitionstyle = check[a];
                d.prefixstyle = prefix[a];
                d.transitionend = evs[a];
                break;
            }
        }
        if (d.ischrome26) {  // use always prefix
            d.prefixstyle = prefix[1];
        }

        d.hastransition = (d.transitionstyle);

        function detectCursorGrab() {
            var lst = ['grab'];
            if ((d.ischrome && !d.ischrome22) || d.isie) lst = [];  // force setting for IE returns false positive and chrome cursor bug
            for (var a = 0; a < lst.length; a++) {
                var p = lst[a];
                domtest.style['cursor'] = p;
                if (domtest.style['cursor'] == p) return p;
            }
            return 'url(http://www.google.com/intl/en_ALL/mapfiles/openhand.cur),n-resize';  // thank you google for custom cursor!
        }
        d.cursorgrabvalue = detectCursorGrab();

        d.hasmousecapture = ("setCapture" in domtest);

        d.hasMutationObserver = (clsMutationObserver !== false);

        domtest = null;  //memory released

        browserdetected = d;

        return d;
    };

    var NiceScrollClass = function (myopt, me) {

        var self = this;

        this.version = '3.5.4';
        this.name = 'nicescroll';

        this.me = me;

        this.opt = {
            doc: $("body"),
            win: false
        };

        $.extend(this.opt, _globaloptions);

        // Options for internal use
        this.opt.snapbackspeed = 80;

        if (myopt || false) {
            for (var a in self.opt) {
                if (typeof myopt[a] != "undefined") self.opt[a] = myopt[a];
            }
        }

        this.doc = self.opt.doc;
        this.iddoc = (this.doc && this.doc[0]) ? this.doc[0].id || '' : '';
        this.ispage = /^BODY|HTML/.test((self.opt.win) ? self.opt.win[0].nodeName : this.doc[0].nodeName);
        this.haswrapper = (self.opt.win !== false);
        this.win = self.opt.win || (this.ispage ? $(window) : this.doc);

        this.docscroll = (this.ispage && !this.haswrapper) ? $(window) : this.win;
        this.body = $("body");
        this.viewport = false;

        this.isfixed = false;

        this.iframe = false;
        this.isiframe = ((this.doc[0].nodeName == 'IFRAME') && (this.win[0].nodeName == 'IFRAME'));

        this.istextarea = (this.win[0].nodeName == 'TEXTAREA');

        this.forcescreen = false; //force to use screen position on events

        this.canshowonmouseevent = (self.opt.autohidemode != "scroll");

        // Events jump table    
        this.onmousedown = false;
        this.onmouseup = false;
        this.onmousemove = false;
        this.onmousewheel = false;
        this.onkeypress = false;
        this.ongesturezoom = false;
        this.onclick = false;

        // Nicescroll custom events
        this.onscrollstart = false;
        this.onscrollend = false;
        this.onscrollcancel = false;

        this.onzoomin = false;
        this.onzoomout = false;

        // Let's start!  
        this.view = false;
        this.page = false;

        this.scroll = { x: 0, y: 0 };
        this.scrollratio = { x: 0, y: 0 };
        this.cursorheight = 20;
        this.scrollvaluemax = 0;

        this.isrtlmode = false; //(this.opt.rtlmode=="auto") ? (this.win.css("direction")=="rtl") : (this.opt.rtlmode===true);
        //    this.checkrtlmode = false;

        this.scrollrunning = false;

        this.scrollmom = false;

        this.observer = false;
        this.observerremover = false;  // observer on parent for remove detection

        do {
            this.id = "ascrail" + (ascrailcounter++);
        } while (document.getElementById(this.id));

        this.rail = false;
        this.cursor = false;
        this.cursorfreezed = false;
        this.selectiondrag = false;

        this.zoom = false;
        this.zoomactive = false;

        this.hasfocus = false;
        this.hasmousefocus = false;

        this.visibility = true;
        this.locked = false;
        this.hidden = false; // rails always hidden
        this.cursoractive = true; // user can interact with cursors

        this.wheelprevented = false;  //prevent mousewheel event

        this.overflowx = self.opt.overflowx;
        this.overflowy = self.opt.overflowy;

        this.nativescrollingarea = false;
        this.checkarea = 0;

        this.events = [];  // event list for unbind

        this.saved = {};

        this.delaylist = {};
        this.synclist = {};

        this.lastdeltax = 0;
        this.lastdeltay = 0;

        this.detected = getBrowserDetection();

        var cap = $.extend({}, this.detected);

        this.canhwscroll = (cap.hastransform && self.opt.hwacceleration);
        this.ishwscroll = (this.canhwscroll && self.haswrapper);

        this.istouchcapable = false;  // desktop devices with touch screen support

        //## Check Chrome desktop with touch support
        if (cap.cantouch && cap.ischrome && !cap.isios && !cap.isandroid) {
            this.istouchcapable = true;
            cap.cantouch = false;  // parse normal desktop events
        }

        //## Firefox 18 nightly build (desktop) false positive (or desktop with touch support)
        if (cap.cantouch && cap.ismozilla && !cap.isios && !cap.isandroid) {
            this.istouchcapable = true;
            cap.cantouch = false;  // parse normal desktop events
        }

        //## disable MouseLock API on user request

        if (!self.opt.enablemouselockapi) {
            cap.hasmousecapture = false;
            cap.haspointerlock = false;
        }

        this.delayed = function (name, fn, tm, lazy) {
            var dd = self.delaylist[name];
            var nw = (new Date()).getTime();
            if (!lazy && dd && dd.tt) return false;
            if (dd && dd.tt) clearTimeout(dd.tt);
                // if (dd&&dd.last+tm>nw&&!dd.tt) {      
                //        self.delaylist[name] = {
                //          last:nw+tm,
                //          tt:setTimeout(function(){if(self||false){self.delaylist[name].tt=0;fn.call()}},tm)
                //        }
                //      }
            else if (!dd || !dd.tt) {
                self.delaylist[name] = {
                    last: nw,
                    tt: 0
                };
                setTimeout(function () { fn.call(); }, 0);
            }
        };

        this.debounced = function (name, fn, tm) {
            var dd = self.delaylist[name];
            var nw = (new Date()).getTime();
            self.delaylist[name] = fn;
            if (!dd) {
                setTimeout(function () { var fn = self.delaylist[name]; self.delaylist[name] = false; fn.call(); }, tm);
            }
        };

        var _onsync = false;

        this.synched = function (name, fn) {

            function requestSync() {
                if (_onsync) return;
                setAnimationFrame(function () {
                    _onsync = false;
                    for (name in self.synclist) {
                        var fn = self.synclist[name];
                        if (fn) fn.call(self);
                        self.synclist[name] = false;
                    }
                });
                _onsync = true;
            };

            self.synclist[name] = fn;
            requestSync();
            return name;
        };

        this.unsynched = function (name) {
            if (self.synclist[name]) self.synclist[name] = false;
        };

        this.css = function (el, pars) {  // save & set
            for (var n in pars) {
                self.saved.css.push([el, n, el.css(n)]);
                el.css(n, pars[n]);
            }
        };

        this.scrollTop = function (val) {
            return (typeof val == "undefined") ? self.getScrollTop() : self.setScrollTop(val);
        };

        this.scrollLeft = function (val) {
            return (typeof val == "undefined") ? self.getScrollLeft() : self.setScrollLeft(val);
        };

        // derived by by Dan Pupius www.pupius.net
        BezierClass = function (st, ed, spd, p1, p2, p3, p4) {
            this.st = st;
            this.ed = ed;
            this.spd = spd;

            this.p1 = p1 || 0;
            this.p2 = p2 || 1;
            this.p3 = p3 || 0;
            this.p4 = p4 || 1;

            this.ts = (new Date()).getTime();
            this.df = this.ed - this.st;
        };
        BezierClass.prototype = {
            B2: function (t) { return 3 * t * t * (1 - t) },
            B3: function (t) { return 3 * t * (1 - t) * (1 - t) },
            B4: function (t) { return (1 - t) * (1 - t) * (1 - t) },
            getNow: function () {
                var nw = (new Date()).getTime();
                var pc = 1 - ((nw - this.ts) / this.spd);
                var bz = this.B2(pc) + this.B3(pc) + this.B4(pc);
                return (pc < 0) ? this.ed : this.st + Math.round(this.df * bz);
            },
            update: function (ed, spd) {
                this.st = this.getNow();
                this.ed = ed;
                this.spd = spd;
                this.ts = (new Date()).getTime();
                this.df = this.ed - this.st;
                return this;
            }
        };

        if (this.ishwscroll) {
            // hw accelerated scroll
            this.doc.translate = { x: 0, y: 0, tx: "0px", ty: "0px" };

            //this one can help to enable hw accel on ios6 http://indiegamr.com/ios6-html-hardware-acceleration-changes-and-how-to-fix-them/
            if (cap.hastranslate3d && cap.isios) this.doc.css("-webkit-backface-visibility", "hidden");  // prevent flickering http://stackoverflow.com/questions/3461441/      

            //derived from http://stackoverflow.com/questions/11236090/
            function getMatrixValues() {
                var tr = self.doc.css(cap.trstyle);
                if (tr && (tr.substr(0, 6) == "matrix")) {
                    return tr.replace(/^.*\((.*)\)$/g, "$1").replace(/px/g, '').split(/, +/);
                }
                return false;
            }

            this.getScrollTop = function (last) {
                if (!last) {
                    var mtx = getMatrixValues();
                    if (mtx) return (mtx.length == 16) ? -mtx[13] : -mtx[5];  //matrix3d 16 on IE10
                    if (self.timerscroll && self.timerscroll.bz) return self.timerscroll.bz.getNow();
                }
                return self.doc.translate.y;
            };

            this.getScrollLeft = function (last) {
                if (!last) {
                    var mtx = getMatrixValues();
                    if (mtx) return (mtx.length == 16) ? -mtx[12] : -mtx[4];  //matrix3d 16 on IE10
                    if (self.timerscroll && self.timerscroll.bh) return self.timerscroll.bh.getNow();
                }
                return self.doc.translate.x;
            };

            if (document.createEvent) {
                this.notifyScrollEvent = function (el) {
                    var e = document.createEvent("UIEvents");
                    e.initUIEvent("scroll", false, true, window, 1);
                    el.dispatchEvent(e);
                };
            }
            else if (document.fireEvent) {
                this.notifyScrollEvent = function (el) {
                    var e = document.createEventObject();
                    el.fireEvent("onscroll");
                    e.cancelBubble = true;
                };
            }
            else {
                this.notifyScrollEvent = function (el, add) { }; //NOPE
            }

            var cxscrollleft = -1; //(this.isrtlmode) ? 1 : -1;

            if (cap.hastranslate3d && self.opt.enabletranslate3d) {
                this.setScrollTop = function (val, silent) {
                    self.doc.translate.y = val;
                    self.doc.translate.ty = (val * -1) + "px";
                    self.doc.css(cap.trstyle, "translate3d(" + self.doc.translate.tx + "," + self.doc.translate.ty + ",0px)");
                    if (!silent) self.notifyScrollEvent(self.win[0]);
                };
                this.setScrollLeft = function (val, silent) {
                    self.doc.translate.x = val;
                    self.doc.translate.tx = (val * cxscrollleft) + "px";
                    self.doc.css(cap.trstyle, "translate3d(" + self.doc.translate.tx + "," + self.doc.translate.ty + ",0px)");
                    if (!silent) self.notifyScrollEvent(self.win[0]);
                };
            } else {
                this.setScrollTop = function (val, silent) {
                    self.doc.translate.y = val;
                    self.doc.translate.ty = (val * -1) + "px";
                    self.doc.css(cap.trstyle, "translate(" + self.doc.translate.tx + "," + self.doc.translate.ty + ")");
                    if (!silent) self.notifyScrollEvent(self.win[0]);
                };
                this.setScrollLeft = function (val, silent) {
                    self.doc.translate.x = val;
                    self.doc.translate.tx = (val * cxscrollleft) + "px";
                    self.doc.css(cap.trstyle, "translate(" + self.doc.translate.tx + "," + self.doc.translate.ty + ")");
                    if (!silent) self.notifyScrollEvent(self.win[0]);
                };
            }
        } else {
            // native scroll
            this.getScrollTop = function () {
                return self.docscroll.scrollTop();
            };
            this.setScrollTop = function (val) {
                return self.docscroll.scrollTop(val);
            };
            this.getScrollLeft = function () {
                return self.docscroll.scrollLeft();
            };
            this.setScrollLeft = function (val) {
                return self.docscroll.scrollLeft(val);
            };
        }

        this.getTarget = function (e) {
            if (!e) return false;
            if (e.target) return e.target;
            if (e.srcElement) return e.srcElement;
            return false;
        };

        this.hasParent = function (e, id) {
            if (!e) return false;
            var el = e.target || e.srcElement || e || false;
            while (el && el.id != id) {
                el = el.parentNode || false;
            }
            return (el !== false);
        };

        function getZIndex() {
            var dom = self.win;
            if ("zIndex" in dom) return dom.zIndex();  // use jQuery UI method when available
            while (dom.length > 0) {
                if (dom[0].nodeType == 9) return false;
                var zi = dom.css('zIndex');
                if (!isNaN(zi) && zi != 0) return parseInt(zi);
                dom = dom.parent();
            }
            return false;
        };

        //inspired by http://forum.jquery.com/topic/width-includes-border-width-when-set-to-thin-medium-thick-in-ie
        var _convertBorderWidth = { "thin": 1, "medium": 3, "thick": 5 };
        function getWidthToPixel(dom, prop, chkheight) {
            var wd = dom.css(prop);
            var px = parseFloat(wd);
            if (isNaN(px)) {
                px = _convertBorderWidth[wd] || 0;
                var brd = (px == 3) ? ((chkheight) ? (self.win.outerHeight() - self.win.innerHeight()) : (self.win.outerWidth() - self.win.innerWidth())) : 1; //DON'T TRUST CSS
                if (self.isie8 && px) px += 1;
                return (brd) ? px : 0;
            }
            return px;
        };

        this.getOffset = function () {
            if (self.isfixed) return { top: parseFloat(self.win.css('top')), left: parseFloat(self.win.css('left')) };
            if (!self.viewport) return self.win.offset();
            var ww = self.win.offset();
            var vp = self.viewport.offset();
            return { top: ww.top - vp.top + self.viewport.scrollTop(), left: ww.left - vp.left + self.viewport.scrollLeft() };
        };

        this.updateScrollBar = function (len) {
            if (self.ishwscroll) {
                self.rail.css({ height: self.win.innerHeight() });
                if (self.railh) self.railh.css({ width: self.win.innerWidth() });
            } else {
                var wpos = self.getOffset();
                var pos = { top: wpos.top, left: wpos.left };
                pos.top += getWidthToPixel(self.win, 'border-top-width', true);
                var brd = (self.win.outerWidth() - self.win.innerWidth()) / 2;
                pos.left += (self.rail.align) ? self.win.outerWidth() - getWidthToPixel(self.win, 'border-right-width') - self.rail.width : getWidthToPixel(self.win, 'border-left-width');

                var off = self.opt.railoffset;
                if (off) {
                    if (off.top) pos.top += off.top;
                    if (self.rail.align && off.left) pos.left += off.left;
                }

                if (!self.locked) self.rail.css({ top: pos.top, left: pos.left, height: (len) ? len.h : self.win.innerHeight() });

                if (self.zoom) {
                    self.zoom.css({ top: pos.top + 1, left: (self.rail.align == 1) ? pos.left - 20 : pos.left + self.rail.width + 4 });
                }

                if (self.railh && !self.locked) {
                    var pos = { top: wpos.top, left: wpos.left };
                    var y = (self.railh.align) ? pos.top + getWidthToPixel(self.win, 'border-top-width', true) + self.win.innerHeight() - self.railh.height : pos.top + getWidthToPixel(self.win, 'border-top-width', true);
                    var x = pos.left + getWidthToPixel(self.win, 'border-left-width');
                    self.railh.css({ top: y, left: x, width: self.railh.width });
                }


            }
        };

        this.doRailClick = function (e, dbl, hr) {

            var fn, pg, cur, pos;

            //      if (self.rail.drag&&self.rail.drag.pt!=1) return;
            if (self.locked) return;
            //      if (self.rail.drag) return;

            //      self.cancelScroll();       

            self.cancelEvent(e);

            if (dbl) {
                fn = (hr) ? self.doScrollLeft : self.doScrollTop;
                cur = (hr) ? ((e.pageX - self.railh.offset().left - (self.cursorwidth / 2)) * self.scrollratio.x) : ((e.pageY - self.rail.offset().top - (self.cursorheight / 2)) * self.scrollratio.y);
                fn(cur);
            } else {
                fn = (hr) ? self.doScrollLeftBy : self.doScrollBy;
                cur = (hr) ? self.scroll.x : self.scroll.y;
                pos = (hr) ? e.pageX - self.railh.offset().left : e.pageY - self.rail.offset().top;
                pg = (hr) ? self.view.w : self.view.h;
                (cur >= pos) ? fn(pg) : fn(-pg);
            }

        };

        self.hasanimationframe = (setAnimationFrame);
        self.hascancelanimationframe = (clearAnimationFrame);

        if (!self.hasanimationframe) {
            setAnimationFrame = function (fn) { return setTimeout(fn, 15 - Math.floor((+new Date) / 1000) % 16) }; // 1000/60)};
            clearAnimationFrame = clearInterval;
        }
        else if (!self.hascancelanimationframe) clearAnimationFrame = function () { self.cancelAnimationFrame = true };

        this.init = function () {

            self.saved.css = [];

            if (cap.isie7mobile) return true; // SORRY, DO NOT WORK!
            if (cap.isoperamini) return true; // SORRY, DO NOT WORK!

            if (cap.hasmstouch) self.css((self.ispage) ? $("html") : self.win, { '-ms-touch-action': 'none' });

            self.zindex = "auto";
            if (!self.ispage && self.opt.zindex == "auto") {
                self.zindex = getZIndex() || "auto";
            } else {
                self.zindex = self.opt.zindex;
            }

            if (!self.ispage && self.zindex != "auto") {
                if (self.zindex > globalmaxzindex) globalmaxzindex = self.zindex;
            }

            if (self.isie && self.zindex == 0 && self.opt.zindex == "auto") {  // fix IE auto == 0
                self.zindex = "auto";
            }

            /*      
                  self.ispage = true;
                  self.haswrapper = true;
            //      self.win = $(window);
                  self.docscroll = $("body");
            //      self.doc = $("body");
            */

            if (!self.ispage || (!cap.cantouch && !cap.isieold && !cap.isie9mobile)) {

                var cont = self.docscroll;
                if (self.ispage) cont = (self.haswrapper) ? self.win : self.doc;

                if (!cap.isie9mobile) self.css(cont, { 'overflow-y': 'hidden' });

                if (self.ispage && cap.isie7) {
                    if (self.doc[0].nodeName == 'BODY') self.css($("html"), { 'overflow-y': 'hidden' });  //IE7 double scrollbar issue
                    else if (self.doc[0].nodeName == 'HTML') self.css($("body"), { 'overflow-y': 'hidden' });  //IE7 double scrollbar issue
                }

                if (cap.isios && !self.ispage && !self.haswrapper) self.css($("body"), { "-webkit-overflow-scrolling": "touch" });  //force hw acceleration

                var cursor = $(document.createElement('div'));
                cursor.css({
                    position: "relative", top: 0, "float": "right", width: self.opt.cursorwidth, height: "0px",
                    'background-color': self.opt.cursorcolor,
                    border: self.opt.cursorborder,
                    'background-clip': 'padding-box',
                    '-webkit-border-radius': self.opt.cursorborderradius,
                    '-moz-border-radius': self.opt.cursorborderradius,
                    'border-radius': self.opt.cursorborderradius
                });

                cursor.hborder = parseFloat(cursor.outerHeight() - cursor.innerHeight());
                self.cursor = cursor;

                var rail = $(document.createElement('div'));
                rail.attr('id', self.id);
                rail.addClass('nicescroll-rails');

                var v, a, kp = ["left", "right"];  //"top","bottom"
                for (var n in kp) {
                    a = kp[n];
                    v = self.opt.railpadding[a];
                    (v) ? rail.css("padding-" + a, v + "px") : self.opt.railpadding[a] = 0;
                }

                rail.append(cursor);

                rail.width = Math.max(parseFloat(self.opt.cursorwidth), cursor.outerWidth()) + self.opt.railpadding['left'] + self.opt.railpadding['right'];
                rail.css({ width: rail.width + "px", 'zIndex': self.zindex, "background": self.opt.background, cursor: "default" });

                rail.visibility = true;
                rail.scrollable = true;

                rail.align = (self.opt.railalign == "left") ? 0 : 1;

                self.rail = rail;

                self.rail.drag = false;

                var zoom = false;
                if (self.opt.boxzoom && !self.ispage && !cap.isieold) {
                    zoom = document.createElement('div');
                    self.bind(zoom, "click", self.doZoom);
                    self.zoom = $(zoom);
                    self.zoom.css({ "cursor": "pointer", 'z-index': self.zindex, 'backgroundImage': 'url(' + self.opt.scriptpath + 'zoomico.png)', 'height': 18, 'width': 18, 'backgroundPosition': '0px 0px' });
                    if (self.opt.dblclickzoom) self.bind(self.win, "dblclick", self.doZoom);
                    if (cap.cantouch && self.opt.gesturezoom) {
                        self.ongesturezoom = function (e) {
                            if (e.scale > 1.5) self.doZoomIn(e);
                            if (e.scale < 0.8) self.doZoomOut(e);
                            return self.cancelEvent(e);
                        };
                        self.bind(self.win, "gestureend", self.ongesturezoom);
                    }
                }

                // init HORIZ

                self.railh = false;

                if (self.opt.horizrailenabled) {

                    self.css(cont, { 'overflow-x': 'hidden' });

                    var cursor = $(document.createElement('div'));
                    cursor.css({
                        position: "relative", top: 0, height: self.opt.cursorwidth, width: "0px",
                        'background-color': self.opt.cursorcolor,
                        border: self.opt.cursorborder,
                        'background-clip': 'padding-box',
                        '-webkit-border-radius': self.opt.cursorborderradius,
                        '-moz-border-radius': self.opt.cursorborderradius,
                        'border-radius': self.opt.cursorborderradius
                    });

                    cursor.wborder = parseFloat(cursor.outerWidth() - cursor.innerWidth());
                    self.cursorh = cursor;

                    var railh = $(document.createElement('div'));
                    railh.attr('id', self.id + '-hr');
                    railh.addClass('nicescroll-rails');
                    railh.height = Math.max(parseFloat(self.opt.cursorwidth), cursor.outerHeight());
                    railh.css({ height: railh.height + "px", 'zIndex': self.zindex, "background": self.opt.background });

                    railh.append(cursor);

                    railh.visibility = true;
                    railh.scrollable = true;

                    railh.align = (self.opt.railvalign == "top") ? 0 : 1;

                    self.railh = railh;

                    self.railh.drag = false;

                }

                //        

                if (self.ispage) {
                    rail.css({ position: "fixed", top: "0px", height: "100%" });
                    (rail.align) ? rail.css({ right: "0px" }) : rail.css({ left: "0px" });
                    self.body.append(rail);
                    if (self.railh) {
                        railh.css({ position: "fixed", left: "0px", width: "100%" });
                        (railh.align) ? railh.css({ bottom: "0px" }) : railh.css({ top: "0px" });
                        self.body.append(railh);
                    }
                } else {
                    if (self.ishwscroll) {
                        if (self.win.css('position') == 'static') self.css(self.win, { 'position': 'relative' });
                        var bd = (self.win[0].nodeName == 'HTML') ? self.body : self.win;
                        if (self.zoom) {
                            self.zoom.css({ position: "absolute", top: 1, right: 0, "margin-right": rail.width + 4 });
                            bd.append(self.zoom);
                        }
                        rail.css({ position: "absolute", top: 0 });
                        (rail.align) ? rail.css({ right: 0 }) : rail.css({ left: 0 });
                        bd.append(rail);
                        if (railh) {
                            railh.css({ position: "absolute", left: 0, bottom: 0 });
                            (railh.align) ? railh.css({ bottom: 0 }) : railh.css({ top: 0 });
                            bd.append(railh);
                        }
                    } else {
                        self.isfixed = (self.win.css("position") == "fixed");
                        var rlpos = (self.isfixed) ? "fixed" : "absolute";

                        if (!self.isfixed) self.viewport = self.getViewport(self.win[0]);
                        if (self.viewport) {
                            self.body = self.viewport;
                            if ((/fixed|relative|absolute/.test(self.viewport.css("position"))) == false) self.css(self.viewport, { "position": "relative" });
                        }

                        rail.css({ position: rlpos });
                        if (self.zoom) self.zoom.css({ position: rlpos });
                        self.updateScrollBar();
                        self.body.append(rail);
                        if (self.zoom) self.body.append(self.zoom);
                        if (self.railh) {
                            railh.css({ position: rlpos });
                            self.body.append(railh);
                        }
                    }

                    if (cap.isios) self.css(self.win, { '-webkit-tap-highlight-color': 'rgba(0,0,0,0)', '-webkit-touch-callout': 'none' });  // prevent grey layer on click

                    if (cap.isie && self.opt.disableoutline) self.win.attr("hideFocus", "true");  // IE, prevent dotted rectangle on focused div
                    if (cap.iswebkit && self.opt.disableoutline) self.win.css({ "outline": "none" });
                    //          if (cap.isopera&&self.opt.disableoutline) self.win.css({"outline":"0"});  // Opera to test [TODO]

                }

                if (self.opt.autohidemode === false) {
                    self.autohidedom = false;
                    self.rail.css({ opacity: self.opt.cursoropacitymax });
                    if (self.railh) self.railh.css({ opacity: self.opt.cursoropacitymax });
                }
                else if ((self.opt.autohidemode === true) || (self.opt.autohidemode === "leave")) {
                    self.autohidedom = $().add(self.rail);
                    if (cap.isie8) self.autohidedom = self.autohidedom.add(self.cursor);
                    if (self.railh) self.autohidedom = self.autohidedom.add(self.railh);
                    if (self.railh && cap.isie8) self.autohidedom = self.autohidedom.add(self.cursorh);
                }
                else if (self.opt.autohidemode == "scroll") {
                    self.autohidedom = $().add(self.rail);
                    if (self.railh) self.autohidedom = self.autohidedom.add(self.railh);
                }
                else if (self.opt.autohidemode == "cursor") {
                    self.autohidedom = $().add(self.cursor);
                    if (self.railh) self.autohidedom = self.autohidedom.add(self.cursorh);
                }
                else if (self.opt.autohidemode == "hidden") {
                    self.autohidedom = false;
                    self.hide();
                    self.locked = false;
                }

                if (cap.isie9mobile) {

                    self.scrollmom = new ScrollMomentumClass2D(self);

                    /*
                    var trace = function(msg) {
                      var db = $("#debug");
                      if (isNaN(msg)&&(typeof msg != "string")) {
                        var x = [];
                        for(var a in msg) {
                          x.push(a+":"+msg[a]);
                        }
                        msg ="{"+x.join(",")+"}";
                      }
                      if (db.children().length>0) {
                        db.children().eq(0).before("<div>"+msg+"</div>");
                      } else {
                        db.append("<div>"+msg+"</div>");
                      }
                    }
                    window.onerror = function(msg,url,ln) {
                      trace("ERR: "+msg+" at "+ln);
                    }
          */

                    self.onmangotouch = function (e) {
                        var py = self.getScrollTop();
                        var px = self.getScrollLeft();

                        if ((py == self.scrollmom.lastscrolly) && (px == self.scrollmom.lastscrollx)) return true;
                        //            $("#debug").html('DRAG:'+py);

                        var dfy = py - self.mangotouch.sy;
                        var dfx = px - self.mangotouch.sx;
                        var df = Math.round(Math.sqrt(Math.pow(dfx, 2) + Math.pow(dfy, 2)));
                        if (df == 0) return;

                        var dry = (dfy < 0) ? -1 : 1;
                        var drx = (dfx < 0) ? -1 : 1;

                        var tm = +new Date();
                        if (self.mangotouch.lazy) clearTimeout(self.mangotouch.lazy);

                        if (((tm - self.mangotouch.tm) > 80) || (self.mangotouch.dry != dry) || (self.mangotouch.drx != drx)) {
                            //              trace('RESET+'+(tm-self.mangotouch.tm));
                            self.scrollmom.stop();
                            self.scrollmom.reset(px, py);
                            self.mangotouch.sy = py;
                            self.mangotouch.ly = py;
                            self.mangotouch.sx = px;
                            self.mangotouch.lx = px;
                            self.mangotouch.dry = dry;
                            self.mangotouch.drx = drx;
                            self.mangotouch.tm = tm;
                        } else {

                            self.scrollmom.stop();
                            self.scrollmom.update(self.mangotouch.sx - dfx, self.mangotouch.sy - dfy);
                            var gap = tm - self.mangotouch.tm;
                            self.mangotouch.tm = tm;

                            //              trace('MOVE:'+df+" - "+gap);

                            var ds = Math.max(Math.abs(self.mangotouch.ly - py), Math.abs(self.mangotouch.lx - px));
                            self.mangotouch.ly = py;
                            self.mangotouch.lx = px;

                            if (ds > 2) {
                                self.mangotouch.lazy = setTimeout(function () {
                                    //                  trace('END:'+ds+'+'+gap);                  
                                    self.mangotouch.lazy = false;
                                    self.mangotouch.dry = 0;
                                    self.mangotouch.drx = 0;
                                    self.mangotouch.tm = 0;
                                    self.scrollmom.doMomentum(30);
                                }, 100);
                            }
                        }
                    };

                    var top = self.getScrollTop();
                    var lef = self.getScrollLeft();
                    self.mangotouch = { sy: top, ly: top, dry: 0, sx: lef, lx: lef, drx: 0, lazy: false, tm: 0 };

                    self.bind(self.docscroll, "scroll", self.onmangotouch);

                } else {

                    if (cap.cantouch || self.istouchcapable || self.opt.touchbehavior || cap.hasmstouch) {

                        self.scrollmom = new ScrollMomentumClass2D(self);

                        self.ontouchstart = function (e) {
                            if (e.pointerType && e.pointerType != 2) return false;

                            self.hasmoving = false;

                            if (!self.locked) {

                                if (cap.hasmstouch) {
                                    var tg = (e.target) ? e.target : false;
                                    while (tg) {
                                        var nc = $(tg).getNiceScroll();
                                        if ((nc.length > 0) && (nc[0].me == self.me)) break;
                                        if (nc.length > 0) return false;
                                        if ((tg.nodeName == 'DIV') && (tg.id == self.id)) break;
                                        tg = (tg.parentNode) ? tg.parentNode : false;
                                    }
                                }

                                self.cancelScroll();

                                var tg = self.getTarget(e);

                                if (tg) {
                                    var skp = (/INPUT/i.test(tg.nodeName)) && (/range/i.test(tg.type));
                                    if (skp) return self.stopPropagation(e);
                                }

                                if (!("clientX" in e) && ("changedTouches" in e)) {
                                    e.clientX = e.changedTouches[0].clientX;
                                    e.clientY = e.changedTouches[0].clientY;
                                }

                                if (self.forcescreen) {
                                    var le = e;
                                    var e = { "original": (e.original) ? e.original : e };
                                    e.clientX = le.screenX;
                                    e.clientY = le.screenY;
                                }

                                self.rail.drag = { x: e.clientX, y: e.clientY, sx: self.scroll.x, sy: self.scroll.y, st: self.getScrollTop(), sl: self.getScrollLeft(), pt: 2, dl: false };

                                if (self.ispage || !self.opt.directionlockdeadzone) {
                                    self.rail.drag.dl = "f";
                                } else {

                                    var view = {
                                        w: $(window).width(),
                                        h: $(window).height()
                                    };

                                    var page = {
                                        w: Math.max(document.body.scrollWidth, document.documentElement.scrollWidth),
                                        h: Math.max(document.body.scrollHeight, document.documentElement.scrollHeight)
                                    };

                                    var maxh = Math.max(0, page.h - view.h);
                                    var maxw = Math.max(0, page.w - view.w);

                                    if (!self.rail.scrollable && self.railh.scrollable) self.rail.drag.ck = (maxh > 0) ? "v" : false;
                                    else if (self.rail.scrollable && !self.railh.scrollable) self.rail.drag.ck = (maxw > 0) ? "h" : false;
                                    else self.rail.drag.ck = false;
                                    if (!self.rail.drag.ck) self.rail.drag.dl = "f";
                                }

                                if (self.opt.touchbehavior && self.isiframe && cap.isie) {
                                    var wp = self.win.position();
                                    self.rail.drag.x += wp.left;
                                    self.rail.drag.y += wp.top;
                                }

                                self.hasmoving = false;
                                self.lastmouseup = false;
                                self.scrollmom.reset(e.clientX, e.clientY);
                                if (!cap.cantouch && !this.istouchcapable && !cap.hasmstouch) {

                                    var ip = (tg) ? /INPUT|SELECT|TEXTAREA/i.test(tg.nodeName) : false;
                                    if (!ip) {
                                        if (!self.ispage && cap.hasmousecapture) tg.setCapture();

                                        if (self.opt.touchbehavior) {
                                            if (tg.onclick && !(tg._onclick || false)) {  // intercept DOM0 onclick event
                                                tg._onclick = tg.onclick;
                                                tg.onclick = function (e) {
                                                    if (self.hasmoving) return false;
                                                    tg._onclick.call(this, e);
                                                }
                                            }
                                            return self.cancelEvent(e);
                                        }

                                        return self.stopPropagation(e);
                                    }

                                    if (/SUBMIT|CANCEL|BUTTON/i.test($(tg).attr('type'))) {
                                        pc = { "tg": tg, "click": false };
                                        self.preventclick = pc;
                                    }

                                }
                            }

                        };

                        self.ontouchend = function (e) {
                            if (e.pointerType && e.pointerType != 2) return false;
                            if (self.rail.drag && (self.rail.drag.pt == 2)) {
                                self.scrollmom.doMomentum();
                                self.rail.drag = false;
                                if (self.hasmoving) {
                                    self.lastmouseup = true;
                                    self.hideCursor();
                                    if (cap.hasmousecapture) document.releaseCapture();
                                    if (!cap.cantouch) return self.cancelEvent(e);
                                }
                            }

                        };

                        var moveneedoffset = (self.opt.touchbehavior && self.isiframe && !cap.hasmousecapture);

                        self.ontouchmove = function (e, byiframe) {

                            if (e.pointerType && e.pointerType != 2) return false;

                            if (self.rail.drag && (self.rail.drag.pt == 2)) {
                                if (cap.cantouch && (typeof e.original == "undefined")) return true;  // prevent ios "ghost" events by clickable elements

                                self.hasmoving = true;

                                if (self.preventclick && !self.preventclick.click) {
                                    self.preventclick.click = self.preventclick.tg.onclick || false;
                                    self.preventclick.tg.onclick = self.onpreventclick;
                                }

                                var ev = $.extend({ "original": e }, e);
                                e = ev;

                                if (("changedTouches" in e)) {
                                    e.clientX = e.changedTouches[0].clientX;
                                    e.clientY = e.changedTouches[0].clientY;
                                }

                                if (self.forcescreen) {
                                    var le = e;
                                    var e = { "original": (e.original) ? e.original : e };
                                    e.clientX = le.screenX;
                                    e.clientY = le.screenY;
                                }

                                var ofx = ofy = 0;

                                if (moveneedoffset && !byiframe) {
                                    var wp = self.win.position();
                                    ofx = -wp.left;
                                    ofy = -wp.top;
                                }

                                var fy = e.clientY + ofy;
                                var my = (fy - self.rail.drag.y);
                                var fx = e.clientX + ofx;
                                var mx = (fx - self.rail.drag.x);

                                var ny = self.rail.drag.st - my;

                                if (self.ishwscroll && self.opt.bouncescroll) {
                                    if (ny < 0) {
                                        ny = Math.round(ny / 2);
                                        //                    fy = 0;
                                    }
                                    else if (ny > self.page.maxh) {
                                        ny = self.page.maxh + Math.round((ny - self.page.maxh) / 2);
                                        //                    fy = 0;
                                    }
                                } else {
                                    if (ny < 0) { ny = 0; fy = 0 }
                                    if (ny > self.page.maxh) { ny = self.page.maxh; fy = 0 }
                                }

                                if (self.railh && self.railh.scrollable) {
                                    var nx = self.rail.drag.sl - mx;; //(self.isrtlmode) ? mx-self.rail.drag.sl : self.rail.drag.sl-mx;

                                    if (self.ishwscroll && self.opt.bouncescroll) {
                                        if (nx < 0) {
                                            nx = Math.round(nx / 2);
                                            //                      fx = 0;
                                        }
                                        else if (nx > self.page.maxw) {
                                            nx = self.page.maxw + Math.round((nx - self.page.maxw) / 2);
                                            //                      fx = 0;
                                        }
                                    } else {
                                        if (nx < 0) { nx = 0; fx = 0 }
                                        if (nx > self.page.maxw) { nx = self.page.maxw; fx = 0 }
                                    }

                                }

                                var grabbed = false;
                                if (self.rail.drag.dl) {
                                    grabbed = true;
                                    if (self.rail.drag.dl == "v") nx = self.rail.drag.sl;
                                    else if (self.rail.drag.dl == "h") ny = self.rail.drag.st;
                                } else {
                                    var ay = Math.abs(my);
                                    var ax = Math.abs(mx);
                                    var dz = self.opt.directionlockdeadzone;
                                    if (self.rail.drag.ck == "v") {
                                        if (ay > dz && (ax <= (ay * 0.3))) {
                                            self.rail.drag = false;
                                            return true;
                                        }
                                        else if (ax > dz) {
                                            self.rail.drag.dl = "f";
                                            $("body").scrollTop($("body").scrollTop());  // stop iOS native scrolling (when active javascript has blocked)
                                        }
                                    }
                                    else if (self.rail.drag.ck == "h") {
                                        if (ax > dz && (ay <= (ax * 0.3))) {
                                            self.rail.drag = false;
                                            return true;
                                        }
                                        else if (ay > dz) {
                                            self.rail.drag.dl = "f";
                                            $("body").scrollLeft($("body").scrollLeft());  // stop iOS native scrolling (when active javascript has blocked)
                                        }
                                    }
                                }

                                self.synched("touchmove", function () {
                                    if (self.rail.drag && (self.rail.drag.pt == 2)) {
                                        if (self.prepareTransition) self.prepareTransition(0);
                                        if (self.rail.scrollable) self.setScrollTop(ny);
                                        self.scrollmom.update(fx, fy);
                                        if (self.railh && self.railh.scrollable) {
                                            self.setScrollLeft(nx);
                                            self.showCursor(ny, nx);
                                        } else {
                                            self.showCursor(ny);
                                        }
                                        if (cap.isie10) document.selection.clear();
                                    }
                                });

                                if (cap.ischrome && self.istouchcapable) grabbed = false;  //chrome touch emulation doesn't like!
                                if (grabbed) return self.cancelEvent(e);
                            }

                        };

                    }

                    self.onmousedown = function (e, hronly) {
                        if (self.rail.drag && self.rail.drag.pt != 1) return;
                        if (self.locked) return self.cancelEvent(e);
                        self.cancelScroll();
                        self.rail.drag = { x: e.clientX, y: e.clientY, sx: self.scroll.x, sy: self.scroll.y, pt: 1, hr: (!!hronly) };
                        var tg = self.getTarget(e);
                        if (!self.ispage && cap.hasmousecapture) tg.setCapture();
                        if (self.isiframe && !cap.hasmousecapture) {
                            self.saved["csspointerevents"] = self.doc.css("pointer-events");
                            self.css(self.doc, { "pointer-events": "none" });
                        }
                        self.hasmoving = false;
                        return self.cancelEvent(e);
                    };

                    self.onmouseup = function (e) {
                        if (self.rail.drag) {
                            if (cap.hasmousecapture) document.releaseCapture();
                            if (self.isiframe && !cap.hasmousecapture) self.doc.css("pointer-events", self.saved["csspointerevents"]);
                            if (self.rail.drag.pt != 1) return;
                            self.rail.drag = false;
                            //if (!self.rail.active) self.hideCursor();
                            if (self.hasmoving) self.triggerScrollEnd();  // TODO - check &&!self.scrollrunning
                            return self.cancelEvent(e);
                        }
                    };

                    self.onmousemove = function (e) {
                        if (self.rail.drag) {
                            if (self.rail.drag.pt != 1) return;

                            if (cap.ischrome && e.which == 0) return self.onmouseup(e);

                            self.cursorfreezed = true;
                            self.hasmoving = true;

                            if (self.rail.drag.hr) {
                                self.scroll.x = self.rail.drag.sx + (e.clientX - self.rail.drag.x);
                                if (self.scroll.x < 0) self.scroll.x = 0;
                                var mw = self.scrollvaluemaxw;
                                if (self.scroll.x > mw) self.scroll.x = mw;
                            } else {
                                self.scroll.y = self.rail.drag.sy + (e.clientY - self.rail.drag.y);
                                if (self.scroll.y < 0) self.scroll.y = 0;
                                var my = self.scrollvaluemax;
                                if (self.scroll.y > my) self.scroll.y = my;
                            }

                            self.synched('mousemove', function () {
                                if (self.rail.drag && (self.rail.drag.pt == 1)) {
                                    self.showCursor();
                                    if (self.rail.drag.hr) self.doScrollLeft(Math.round(self.scroll.x * self.scrollratio.x), self.opt.cursordragspeed);
                                    else self.doScrollTop(Math.round(self.scroll.y * self.scrollratio.y), self.opt.cursordragspeed);
                                }
                            });

                            return self.cancelEvent(e);
                        }
                        /*              
                                    else {
                                      self.checkarea = true;
                                    }
                        */
                    };

                    if (cap.cantouch || self.opt.touchbehavior) {

                        self.onpreventclick = function (e) {
                            if (self.preventclick) {
                                self.preventclick.tg.onclick = self.preventclick.click;
                                self.preventclick = false;
                                return self.cancelEvent(e);
                            }
                        }

                        //            self.onmousedown = self.ontouchstart;            
                        //            self.onmouseup = self.ontouchend;
                        //            self.onmousemove = self.ontouchmove;

                        self.bind(self.win, "mousedown", self.ontouchstart);  // control content dragging

                        self.onclick = (cap.isios) ? false : function (e) {
                            if (self.lastmouseup) {
                                self.lastmouseup = false;
                                return self.cancelEvent(e);
                            } else {
                                return true;
                            }
                        };

                        if (self.opt.grabcursorenabled && cap.cursorgrabvalue) {
                            self.css((self.ispage) ? self.doc : self.win, { 'cursor': cap.cursorgrabvalue });
                            self.css(self.rail, { 'cursor': cap.cursorgrabvalue });
                        }

                    } else {

                        function checkSelectionScroll(e) {
                            if (!self.selectiondrag) return;

                            if (e) {
                                var ww = self.win.outerHeight();
                                var df = (e.pageY - self.selectiondrag.top);
                                if (df > 0 && df < ww) df = 0;
                                if (df >= ww) df -= ww;
                                self.selectiondrag.df = df;
                            }
                            if (self.selectiondrag.df == 0) return;

                            var rt = -Math.floor(self.selectiondrag.df / 6) * 2;
                            //              self.doScrollTop(self.getScrollTop(true)+rt);
                            self.doScrollBy(rt);

                            self.debounced("doselectionscroll", function () { checkSelectionScroll() }, 50);
                        };

                        if ("getSelection" in document) {  // A grade - Major browsers
                            self.hasTextSelected = function () {
                                return (document.getSelection().rangeCount > 0);
                            };
                        }
                        else if ("selection" in document) {  //IE9-
                            self.hasTextSelected = function () {
                                return (document.selection.type != "None");
                            };
                        }
                        else {
                            self.hasTextSelected = function () {  // no support
                                return false;
                            };
                        }

                        self.onselectionstart = function (e) {
                            if (self.ispage) return;
                            self.selectiondrag = self.win.offset();
                        };
                        self.onselectionend = function (e) {
                            self.selectiondrag = false;
                        };
                        self.onselectiondrag = function (e) {
                            if (!self.selectiondrag) return;
                            if (self.hasTextSelected()) self.debounced("selectionscroll", function () { checkSelectionScroll(e) }, 250);
                        };


                    }

                    if (cap.hasmstouch) {
                        self.css(self.rail, { '-ms-touch-action': 'none' });
                        self.css(self.cursor, { '-ms-touch-action': 'none' });

                        self.bind(self.win, "MSPointerDown", self.ontouchstart);
                        self.bind(document, "MSPointerUp", self.ontouchend);
                        self.bind(document, "MSPointerMove", self.ontouchmove);
                        self.bind(self.cursor, "MSGestureHold", function (e) { e.preventDefault() });
                        self.bind(self.cursor, "contextmenu", function (e) { e.preventDefault() });
                    }

                    if (this.istouchcapable) {  //desktop with screen touch enabled
                        self.bind(self.win, "touchstart", self.ontouchstart);
                        self.bind(document, "touchend", self.ontouchend);
                        self.bind(document, "touchcancel", self.ontouchend);
                        self.bind(document, "touchmove", self.ontouchmove);
                    }

                    self.bind(self.cursor, "mousedown", self.onmousedown);
                    self.bind(self.cursor, "mouseup", self.onmouseup);

                    if (self.railh) {
                        self.bind(self.cursorh, "mousedown", function (e) { self.onmousedown(e, true) });

                        self.bind(self.cursorh, "mouseup", self.onmouseup);


                        /*						
                                    self.bind(self.cursorh,"mouseup",function(e){
                                      if (self.rail.drag&&self.rail.drag.pt==2) return;
                                      self.rail.drag = false;
                                      self.hasmoving = false;
                                      self.hideCursor();
                                      if (cap.hasmousecapture) document.releaseCapture();
                                      return self.cancelEvent(e);
                                    });
                        */

                    }

                    if (self.opt.cursordragontouch || !cap.cantouch && !self.opt.touchbehavior) {

                        self.rail.css({ "cursor": "default" });
                        self.railh && self.railh.css({ "cursor": "default" });

                        self.jqbind(self.rail, "mouseenter", function () {
                            if (!self.win.is(":visible")) return false;
                            if (self.canshowonmouseevent) self.showCursor();
                            self.rail.active = true;
                        });
                        self.jqbind(self.rail, "mouseleave", function () {
                            self.rail.active = false;
                            if (!self.rail.drag) self.hideCursor();
                        });

                        if (self.opt.sensitiverail) {
                            self.bind(self.rail, "click", function (e) { self.doRailClick(e, false, false) });
                            self.bind(self.rail, "dblclick", function (e) { self.doRailClick(e, true, false) });
                            self.bind(self.cursor, "click", function (e) { self.cancelEvent(e) });
                            self.bind(self.cursor, "dblclick", function (e) { self.cancelEvent(e) });
                        }

                        if (self.railh) {
                            self.jqbind(self.railh, "mouseenter", function () {
                                if (!self.win.is(":visible")) return false;
                                if (self.canshowonmouseevent) self.showCursor();
                                self.rail.active = true;
                            });
                            self.jqbind(self.railh, "mouseleave", function () {
                                self.rail.active = false;
                                if (!self.rail.drag) self.hideCursor();
                            });

                            if (self.opt.sensitiverail) {
                                self.bind(self.railh, "click", function (e) { self.doRailClick(e, false, true) });
                                self.bind(self.railh, "dblclick", function (e) { self.doRailClick(e, true, true) });
                                self.bind(self.cursorh, "click", function (e) { self.cancelEvent(e) });
                                self.bind(self.cursorh, "dblclick", function (e) { self.cancelEvent(e) });
                            }

                        }

                    }

                    if (!cap.cantouch && !self.opt.touchbehavior) {

                        self.bind((cap.hasmousecapture) ? self.win : document, "mouseup", self.onmouseup);
                        self.bind(document, "mousemove", self.onmousemove);
                        if (self.onclick) self.bind(document, "click", self.onclick);

                        if (!self.ispage && self.opt.enablescrollonselection) {
                            self.bind(self.win[0], "mousedown", self.onselectionstart);
                            self.bind(document, "mouseup", self.onselectionend);
                            self.bind(self.cursor, "mouseup", self.onselectionend);
                            if (self.cursorh) self.bind(self.cursorh, "mouseup", self.onselectionend);
                            self.bind(document, "mousemove", self.onselectiondrag);
                        }

                        if (self.zoom) {
                            self.jqbind(self.zoom, "mouseenter", function () {
                                if (self.canshowonmouseevent) self.showCursor();
                                self.rail.active = true;
                            });
                            self.jqbind(self.zoom, "mouseleave", function () {
                                self.rail.active = false;
                                if (!self.rail.drag) self.hideCursor();
                            });
                        }

                    } else {

                        self.bind((cap.hasmousecapture) ? self.win : document, "mouseup", self.ontouchend);
                        self.bind(document, "mousemove", self.ontouchmove);
                        if (self.onclick) self.bind(document, "click", self.onclick);

                        if (self.opt.cursordragontouch) {
                            self.bind(self.cursor, "mousedown", self.onmousedown);
                            self.bind(self.cursor, "mousemove", self.onmousemove);
                            self.cursorh && self.bind(self.cursorh, "mousedown", function (e) { self.onmousedown(e, true) });
                            self.cursorh && self.bind(self.cursorh, "mousemove", self.onmousemove);
                        }

                    }

                    if (self.opt.enablemousewheel) {
                        if (!self.isiframe) self.bind((cap.isie && self.ispage) ? document : self.win /*self.docscroll*/, "mousewheel", self.onmousewheel);
                        self.bind(self.rail, "mousewheel", self.onmousewheel);
                        if (self.railh) self.bind(self.railh, "mousewheel", self.onmousewheelhr);
                    }

                    if (!self.ispage && !cap.cantouch && !(/HTML|^BODY/.test(self.win[0].nodeName))) {
                        if (!self.win.attr("tabindex")) self.win.attr({ "tabindex": tabindexcounter++ });

                        self.jqbind(self.win, "focus", function (e) {
                            domfocus = (self.getTarget(e)).id || true;
                            self.hasfocus = true;
                            if (self.canshowonmouseevent) self.noticeCursor();
                        });
                        self.jqbind(self.win, "blur", function (e) {
                            domfocus = false;
                            self.hasfocus = false;
                        });

                        self.jqbind(self.win, "mouseenter", function (e) {
                            mousefocus = (self.getTarget(e)).id || true;
                            self.hasmousefocus = true;
                            if (self.canshowonmouseevent) self.noticeCursor();
                        });
                        self.jqbind(self.win, "mouseleave", function () {
                            mousefocus = false;
                            self.hasmousefocus = false;
                            if (!self.rail.drag) self.hideCursor();
                        });

                    };

                }  // !ie9mobile

                //Thanks to http://www.quirksmode.org !!
                self.onkeypress = function (e) {
                    if (self.locked && self.page.maxh == 0) return true;

                    e = (e) ? e : window.e;
                    var tg = self.getTarget(e);
                    if (tg && /INPUT|TEXTAREA|SELECT|OPTION/.test(tg.nodeName)) {
                        var tp = tg.getAttribute('type') || tg.type || false;
                        if ((!tp) || !(/submit|button|cancel/i.tp)) return true;
                    }

                    if ($(tg).attr('contenteditable')) return true;

                    if (self.hasfocus || (self.hasmousefocus && !domfocus) || (self.ispage && !domfocus && !mousefocus)) {
                        var key = e.keyCode;

                        if (self.locked && key != 27) return self.cancelEvent(e);

                        var ctrl = e.ctrlKey || false;
                        var shift = e.shiftKey || false;

                        var ret = false;
                        switch (key) {
                            case 38:
                            case 63233: //safari
                                self.doScrollBy(24 * 3);
                                ret = true;
                                break;
                            case 40:
                            case 63235: //safari
                                self.doScrollBy(-24 * 3);
                                ret = true;
                                break;
                            case 37:
                            case 63232: //safari
                                if (self.railh) {
                                    (ctrl) ? self.doScrollLeft(0) : self.doScrollLeftBy(24 * 3);
                                    ret = true;
                                }
                                break;
                            case 39:
                            case 63234: //safari
                                if (self.railh) {
                                    (ctrl) ? self.doScrollLeft(self.page.maxw) : self.doScrollLeftBy(-24 * 3);
                                    ret = true;
                                }
                                break;
                            case 33:
                            case 63276: // safari
                                self.doScrollBy(self.view.h);
                                ret = true;
                                break;
                            case 34:
                            case 63277: // safari
                                self.doScrollBy(-self.view.h);
                                ret = true;
                                break;
                            case 36:
                            case 63273: // safari                
                                (self.railh && ctrl) ? self.doScrollPos(0, 0) : self.doScrollTo(0);
                                ret = true;
                                break;
                            case 35:
                            case 63275: // safari
                                (self.railh && ctrl) ? self.doScrollPos(self.page.maxw, self.page.maxh) : self.doScrollTo(self.page.maxh);
                                ret = true;
                                break;
                            case 32:
                                if (self.opt.spacebarenabled) {
                                    (shift) ? self.doScrollBy(self.view.h) : self.doScrollBy(-self.view.h);
                                    ret = true;
                                }
                                break;
                            case 27: // ESC
                                if (self.zoomactive) {
                                    self.doZoom();
                                    ret = true;
                                }
                                break;
                        }
                        if (ret) return self.cancelEvent(e);
                    }
                };

                if (self.opt.enablekeyboard) self.bind(document, (cap.isopera && !cap.isopera12) ? "keypress" : "keydown", self.onkeypress);

                self.bind(document, "keydown", function (e) {
                    var ctrl = e.ctrlKey || false;
                    if (ctrl) self.wheelprevented = true;
                });
                self.bind(document, "keyup", function (e) {
                    var ctrl = e.ctrlKey || false;
                    if (!ctrl) self.wheelprevented = false;
                });

                self.bind(window, 'resize', self.lazyResize);
                self.bind(window, 'orientationchange', self.lazyResize);

                self.bind(window, "load", self.lazyResize);

                if (cap.ischrome && !self.ispage && !self.haswrapper) { //chrome void scrollbar bug - it persists in version 26
                    var tmp = self.win.attr("style");
                    var ww = parseFloat(self.win.css("width")) + 1;
                    self.win.css('width', ww);
                    self.synched("chromefix", function () { self.win.attr("style", tmp) });
                }


                // Trying a cross-browser implementation - good luck!

                self.onAttributeChange = function (e) {
                    self.lazyResize(250);
                };

                if (!self.ispage && !self.haswrapper) {
                    // redesigned MutationObserver for Chrome18+/Firefox14+/iOS6+ with support for: remove div, add/remove content
                    if (clsMutationObserver !== false) {
                        self.observer = new clsMutationObserver(function (mutations) {
                            mutations.forEach(self.onAttributeChange);
                        });
                        self.observer.observe(self.win[0], { childList: true, characterData: false, attributes: true, subtree: false });

                        self.observerremover = new clsMutationObserver(function (mutations) {
                            mutations.forEach(function (mo) {
                                if (mo.removedNodes.length > 0) {
                                    for (var dd in mo.removedNodes) {
                                        if (mo.removedNodes[dd] == self.win[0]) return self.remove();
                                    }
                                }
                            });
                        });
                        self.observerremover.observe(self.win[0].parentNode, { childList: true, characterData: false, attributes: false, subtree: false });

                    } else {
                        self.bind(self.win, (cap.isie && !cap.isie9) ? "propertychange" : "DOMAttrModified", self.onAttributeChange);
                        if (cap.isie9) self.win[0].attachEvent("onpropertychange", self.onAttributeChange); //IE9 DOMAttrModified bug
                        self.bind(self.win, "DOMNodeRemoved", function (e) {
                            if (e.target == self.win[0]) self.remove();
                        });
                    }
                }

                //

                if (!self.ispage && self.opt.boxzoom) self.bind(window, "resize", self.resizeZoom);
                if (self.istextarea) self.bind(self.win, "mouseup", self.lazyResize);

                //        self.checkrtlmode = true;
                self.lazyResize(30);

            }

            if (this.doc[0].nodeName == 'IFRAME') {
                function oniframeload(e) {
                    self.iframexd = false;
                    try {
                        var doc = 'contentDocument' in this ? this.contentDocument : this.contentWindow.document;
                        var a = doc.domain;
                    } catch (e) { self.iframexd = true; doc = false };

                    if (self.iframexd) {
                        if ("console" in window) console.log('NiceScroll error: policy restriced iframe');
                        return true;  //cross-domain - I can't manage this        
                    }

                    self.forcescreen = true;

                    if (self.isiframe) {
                        self.iframe = {
                            "doc": $(doc),
                            "html": self.doc.contents().find('html')[0],
                            "body": self.doc.contents().find('body')[0]
                        };
                        self.getContentSize = function () {
                            return {
                                w: Math.max(self.iframe.html.scrollWidth, self.iframe.body.scrollWidth),
                                h: Math.max(self.iframe.html.scrollHeight, self.iframe.body.scrollHeight)
                            };
                        };
                        self.docscroll = $(self.iframe.body);//$(this.contentWindow);
                    }

                    if (!cap.isios && self.opt.iframeautoresize && !self.isiframe) {
                        self.win.scrollTop(0); // reset position
                        self.doc.height("");  //reset height to fix browser bug
                        var hh = Math.max(doc.getElementsByTagName('html')[0].scrollHeight, doc.body.scrollHeight);
                        self.doc.height(hh);
                    }
                    self.lazyResize(30);

                    if (cap.isie7) self.css($(self.iframe.html), { 'overflow-y': 'hidden' });
                    //self.css($(doc.body),{'overflow-y':'hidden'});
                    self.css($(self.iframe.body), { 'overflow-y': 'hidden' });

                    if (cap.isios && self.haswrapper) {
                        self.css($(doc.body), { '-webkit-transform': 'translate3d(0,0,0)' });  // avoid iFrame content clipping - thanks to http://blog.derraab.com/2012/04/02/avoid-iframe-content-clipping-with-css-transform-on-ios/
                    }

                    if ('contentWindow' in this) {
                        self.bind(this.contentWindow, "scroll", self.onscroll);  //IE8 & minor
                    } else {
                        self.bind(doc, "scroll", self.onscroll);
                    }

                    if (self.opt.enablemousewheel) {
                        self.bind(doc, "mousewheel", self.onmousewheel);
                    }

                    if (self.opt.enablekeyboard) self.bind(doc, (cap.isopera) ? "keypress" : "keydown", self.onkeypress);

                    if (cap.cantouch || self.opt.touchbehavior) {
                        self.bind(doc, "mousedown", self.ontouchstart);
                        self.bind(doc, "mousemove", function (e) { self.ontouchmove(e, true) });
                        if (self.opt.grabcursorenabled && cap.cursorgrabvalue) self.css($(doc.body), { 'cursor': cap.cursorgrabvalue });
                    }

                    self.bind(doc, "mouseup", self.ontouchend);

                    if (self.zoom) {
                        if (self.opt.dblclickzoom) self.bind(doc, 'dblclick', self.doZoom);
                        if (self.ongesturezoom) self.bind(doc, "gestureend", self.ongesturezoom);
                    }
                };

                if (this.doc[0].readyState && this.doc[0].readyState == "complete") {
                    setTimeout(function () { oniframeload.call(self.doc[0], false) }, 500);
                }
                self.bind(this.doc, "load", oniframeload);

            }

        };

        this.showCursor = function (py, px) {
            if (self.cursortimeout) {
                clearTimeout(self.cursortimeout);
                self.cursortimeout = 0;
            }
            if (!self.rail) return;
            if (self.autohidedom) {
                self.autohidedom.stop().css({ opacity: self.opt.cursoropacitymax });
                self.cursoractive = true;
            }

            if (!self.rail.drag || self.rail.drag.pt != 1) {
                if ((typeof py != "undefined") && (py !== false)) {
                    self.scroll.y = Math.round(py * 1 / self.scrollratio.y);
                }
                if (typeof px != "undefined") {
                    self.scroll.x = Math.round(px * 1 / self.scrollratio.x);  //-cxscrollleft * Math.round(px * 1/self.scrollratio.x);
                }
            }

            self.cursor.css({ height: self.cursorheight, top: self.scroll.y });
            if (self.cursorh) {
                (!self.rail.align && self.rail.visibility) ? self.cursorh.css({ width: self.cursorwidth, left: self.scroll.x + self.rail.width }) : self.cursorh.css({ width: self.cursorwidth, left: self.scroll.x });
                self.cursoractive = true;
            }

            if (self.zoom) self.zoom.stop().css({ opacity: self.opt.cursoropacitymax });
        };

        this.hideCursor = function (tm) {
            if (self.cursortimeout) return;
            if (!self.rail) return;
            if (!self.autohidedom) return;
            if (self.hasmousefocus && self.opt.autohidemode == "leave") return;
            self.cursortimeout = setTimeout(function () {
                if (!self.rail.active || !self.showonmouseevent) {
                    self.autohidedom.stop().animate({ opacity: self.opt.cursoropacitymin });
                    if (self.zoom) self.zoom.stop().animate({ opacity: self.opt.cursoropacitymin });
                    self.cursoractive = false;
                }
                self.cursortimeout = 0;
            }, tm || self.opt.hidecursordelay);
        };

        this.noticeCursor = function (tm, py, px) {
            self.showCursor(py, px);
            if (!self.rail.active) self.hideCursor(tm);
        };

        this.getContentSize =
          (self.ispage) ?
            function () {
                return {
                    w: Math.max(document.body.scrollWidth, document.documentElement.scrollWidth),
                    h: Math.max(document.body.scrollHeight, document.documentElement.scrollHeight)
                }
            }
          : (self.haswrapper) ?
            function () {
                return {
                    w: self.doc.outerWidth() + parseInt(self.win.css('paddingLeft')) + parseInt(self.win.css('paddingRight')),
                    h: self.doc.outerHeight() + parseInt(self.win.css('paddingTop')) + parseInt(self.win.css('paddingBottom'))
                }
            }
          : function () {
              return {
                  w: self.docscroll[0].scrollWidth,
                  h: self.docscroll[0].scrollHeight
              }
          };

        this.onResize = function (e, page) {

            if (!self || !self.win) return false;

            if (!self.haswrapper && !self.ispage) {
                if (self.win.css('display') == 'none') {
                    if (self.visibility) self.hideRail().hideRailHr();
                    return false;
                } else {
                    if (!self.hidden && !self.visibility) self.showRail().showRailHr();
                }
            }

            var premaxh = self.page.maxh;
            var premaxw = self.page.maxw;

            var preview = { h: self.view.h, w: self.view.w };

            self.view = {
                w: (self.ispage) ? self.win.width() : parseInt(self.win[0].clientWidth),
                h: (self.ispage) ? self.win.height() : parseInt(self.win[0].clientHeight)
            };

            self.page = (page) ? page : self.getContentSize();

            self.page.maxh = Math.max(0, self.page.h - self.view.h);
            self.page.maxw = Math.max(0, self.page.w - self.view.w);

            if ((self.page.maxh == premaxh) && (self.page.maxw == premaxw) && (self.view.w == preview.w)) {
                // test position        
                if (!self.ispage) {
                    var pos = self.win.offset();
                    if (self.lastposition) {
                        var lst = self.lastposition;
                        if ((lst.top == pos.top) && (lst.left == pos.left)) return self; //nothing to do            
                    }
                    self.lastposition = pos;
                } else {
                    return self; //nothing to do
                }
            }

            if (self.page.maxh == 0) {
                self.hideRail();
                self.scrollvaluemax = 0;
                self.scroll.y = 0;
                self.scrollratio.y = 0;
                self.cursorheight = 0;
                self.setScrollTop(0);
                self.rail.scrollable = false;
            } else {
                self.rail.scrollable = true;
            }

            if (self.page.maxw == 0) {
                self.hideRailHr();
                self.scrollvaluemaxw = 0;
                self.scroll.x = 0;
                self.scrollratio.x = 0;
                self.cursorwidth = 0;
                self.setScrollLeft(0);
                self.railh.scrollable = false;
            } else {
                self.railh.scrollable = true;
            }

            self.locked = (self.page.maxh == 0) && (self.page.maxw == 0);
            if (self.locked) {
                if (!self.ispage) self.updateScrollBar(self.view);
                return false;
            }

            if (!self.hidden && !self.visibility) {
                self.showRail().showRailHr();
            }
            else if (!self.hidden && !self.railh.visibility) self.showRailHr();

            if (self.istextarea && self.win.css('resize') && self.win.css('resize') != 'none') self.view.h -= 20;

            self.cursorheight = Math.min(self.view.h, Math.round(self.view.h * (self.view.h / self.page.h)));
            self.cursorheight = (self.opt.cursorfixedheight) ? self.opt.cursorfixedheight : Math.max(self.opt.cursorminheight, self.cursorheight);

            self.cursorwidth = Math.min(self.view.w, Math.round(self.view.w * (self.view.w / self.page.w)));
            self.cursorwidth = (self.opt.cursorfixedheight) ? self.opt.cursorfixedheight : Math.max(self.opt.cursorminheight, self.cursorwidth);

            self.scrollvaluemax = self.view.h - self.cursorheight - self.cursor.hborder;

            if (self.railh) {
                self.railh.width = (self.page.maxh > 0) ? (self.view.w - self.rail.width) : self.view.w;
                self.scrollvaluemaxw = self.railh.width - self.cursorwidth - self.cursorh.wborder;
            }

            /*			
                  if (self.checkrtlmode&&self.railh) {
                    self.checkrtlmode = false;
                    if (self.opt.rtlmode&&self.scroll.x==0) self.setScrollLeft(self.page.maxw);
                  }
            */

            if (!self.ispage) self.updateScrollBar(self.view);

            self.scrollratio = {
                x: (self.page.maxw / self.scrollvaluemaxw),
                y: (self.page.maxh / self.scrollvaluemax)
            };

            var sy = self.getScrollTop();
            if (sy > self.page.maxh) {
                self.doScrollTop(self.page.maxh);
            } else {
                self.scroll.y = Math.round(self.getScrollTop() * (1 / self.scrollratio.y));
                self.scroll.x = Math.round(self.getScrollLeft() * (1 / self.scrollratio.x));
                if (self.cursoractive) self.noticeCursor();
            }

            if (self.scroll.y && (self.getScrollTop() == 0)) self.doScrollTo(Math.floor(self.scroll.y * self.scrollratio.y));

            return self;
        };

        this.resize = self.onResize;

        this.lazyResize = function (tm) {   // event debounce
            tm = (isNaN(tm)) ? 30 : tm;
            self.delayed('resize', self.resize, tm);
            return self;
        };

        // modified by MDN https://developer.mozilla.org/en-US/docs/DOM/Mozilla_event_reference/wheel
        function _modernWheelEvent(dom, name, fn, bubble) {
            self._bind(dom, name, function (e) {
                var e = (e) ? e : window.event;
                var event = {
                    original: e,
                    target: e.target || e.srcElement,
                    type: "wheel",
                    deltaMode: e.type == "MozMousePixelScroll" ? 0 : 1,
                    deltaX: 0,
                    deltaZ: 0,
                    preventDefault: function () {
                        e.preventDefault ? e.preventDefault() : e.returnValue = false;
                        return false;
                    },
                    stopImmediatePropagation: function () {
                        (e.stopImmediatePropagation) ? e.stopImmediatePropagation() : e.cancelBubble = true;
                    }
                };

                if (name == "mousewheel") {
                    event.deltaY = -1 / 40 * e.wheelDelta;
                    e.wheelDeltaX && (event.deltaX = -1 / 40 * e.wheelDeltaX);
                } else {
                    event.deltaY = e.detail;
                }

                return fn.call(dom, event);
            }, bubble);
        };

        this._bind = function (el, name, fn, bubble) {  // primitive bind
            self.events.push({ e: el, n: name, f: fn, b: bubble, q: false });
            if (el.addEventListener) {
                el.addEventListener(name, fn, bubble || false);
            }
            else if (el.attachEvent) {
                el.attachEvent("on" + name, fn);
            }
            else {
                el["on" + name] = fn;
            }
        };

        this.jqbind = function (dom, name, fn) {  // use jquery bind for non-native events (mouseenter/mouseleave)
            self.events.push({ e: dom, n: name, f: fn, q: true });
            $(dom).bind(name, fn);
        };

        this.bind = function (dom, name, fn, bubble) {  // touch-oriented & fixing jquery bind
            var el = ("jquery" in dom) ? dom[0] : dom;

            if (name == 'mousewheel') {
                if ("onwheel" in self.win) {
                    self._bind(el, "wheel", fn, bubble || false);
                } else {
                    var wname = (typeof document.onmousewheel != "undefined") ? "mousewheel" : "DOMMouseScroll";  // older IE/Firefox
                    _modernWheelEvent(el, wname, fn, bubble || false);
                    if (wname == "DOMMouseScroll") _modernWheelEvent(el, "MozMousePixelScroll", fn, bubble || false);  // Firefox legacy
                }
            }
            else if (el.addEventListener) {
                if (cap.cantouch && /mouseup|mousedown|mousemove/.test(name)) {  // touch device support
                    var tt = (name == 'mousedown') ? 'touchstart' : (name == 'mouseup') ? 'touchend' : 'touchmove';
                    self._bind(el, tt, function (e) {
                        if (e.touches) {
                            if (e.touches.length < 2) { var ev = (e.touches.length) ? e.touches[0] : e; ev.original = e; fn.call(this, ev); }
                        }
                        else if (e.changedTouches) { var ev = e.changedTouches[0]; ev.original = e; fn.call(this, ev); }  //blackberry
                    }, bubble || false);
                }
                self._bind(el, name, fn, bubble || false);
                if (cap.cantouch && name == "mouseup") self._bind(el, "touchcancel", fn, bubble || false);
            }
            else {
                self._bind(el, name, function (e) {
                    e = e || window.event || false;
                    if (e) {
                        if (e.srcElement) e.target = e.srcElement;
                    }
                    if (!("pageY" in e)) {
                        e.pageX = e.clientX + document.documentElement.scrollLeft;
                        e.pageY = e.clientY + document.documentElement.scrollTop;
                    }
                    return ((fn.call(el, e) === false) || bubble === false) ? self.cancelEvent(e) : true;
                });
            }
        };

        this._unbind = function (el, name, fn, bub) {  // primitive unbind
            if (el.removeEventListener) {
                el.removeEventListener(name, fn, bub);
            }
            else if (el.detachEvent) {
                el.detachEvent('on' + name, fn);
            } else {
                el['on' + name] = false;
            }
        };

        this.unbindAll = function () {
            for (var a = 0; a < self.events.length; a++) {
                var r = self.events[a];
                (r.q) ? r.e.unbind(r.n, r.f) : self._unbind(r.e, r.n, r.f, r.b);
            }
        };

        // Thanks to http://www.switchonthecode.com !!
        this.cancelEvent = function (e) {
            var e = (e.original) ? e.original : (e) ? e : window.event || false;
            if (!e) return false;
            if (e.preventDefault) e.preventDefault();
            if (e.stopPropagation) e.stopPropagation();
            if (e.preventManipulation) e.preventManipulation();  //IE10
            e.cancelBubble = true;
            e.cancel = true;
            e.returnValue = false;
            return false;
        };

        this.stopPropagation = function (e) {
            var e = (e.original) ? e.original : (e) ? e : window.event || false;
            if (!e) return false;
            if (e.stopPropagation) return e.stopPropagation();
            if (e.cancelBubble) e.cancelBubble = true;
            return false;
        };

        this.showRail = function () {
            if ((self.page.maxh != 0) && (self.ispage || self.win.css('display') != 'none')) {
                self.visibility = true;
                self.rail.visibility = true;
                self.rail.css('display', 'block');
            }
            return self;
        };

        this.showRailHr = function () {
            if (!self.railh) return self;
            if ((self.page.maxw != 0) && (self.ispage || self.win.css('display') != 'none')) {
                self.railh.visibility = true;
                self.railh.css('display', 'block');
            }
            return self;
        };

        this.hideRail = function () {
            self.visibility = false;
            self.rail.visibility = false;
            self.rail.css('display', 'none');
            return self;
        };

        this.hideRailHr = function () {
            if (!self.railh) return self;
            self.railh.visibility = false;
            self.railh.css('display', 'none');
            return self;
        };

        this.show = function () {
            self.hidden = false;
            self.locked = false;
            return self.showRail().showRailHr();
        };

        this.hide = function () {
            self.hidden = true;
            self.locked = true;
            return self.hideRail().hideRailHr();
        };

        this.toggle = function () {
            return (self.hidden) ? self.show() : self.hide();
        };

        this.remove = function () {
            self.stop();
            if (self.cursortimeout) clearTimeout(self.cursortimeout);
            self.doZoomOut();
            self.unbindAll();

            if (cap.isie9) self.win[0].detachEvent("onpropertychange", self.onAttributeChange); //IE9 DOMAttrModified bug

            if (self.observer !== false) self.observer.disconnect();
            if (self.observerremover !== false) self.observerremover.disconnect();

            self.events = null;

            if (self.cursor) {
                self.cursor.remove();
            }
            if (self.cursorh) {
                self.cursorh.remove();
            }
            if (self.rail) {
                self.rail.remove();
            }
            if (self.railh) {
                self.railh.remove();
            }
            if (self.zoom) {
                self.zoom.remove();
            }
            for (var a = 0; a < self.saved.css.length; a++) {
                var d = self.saved.css[a];
                d[0].css(d[1], (typeof d[2] == "undefined") ? '' : d[2]);
            }
            self.saved = false;
            self.me.data('__nicescroll', ''); //erase all traces

            // memory leak fixed by GianlucaGuarini - thanks a lot!
            // remove the current nicescroll from the $.nicescroll array & normalize array
            var lst = $.nicescroll;
            lst.each(function (i) {
                if (!this) return;
                if (this.id === self.id) {
                    delete lst[i];
                    for (var b = ++i; b < lst.length; b++, i++) lst[i] = lst[b];
                    lst.length--;
                    if (lst.length) delete lst[lst.length];
                }
            });

            for (var i in self) {
                self[i] = null;
                delete self[i];
            }

            self = null;

        };

        this.scrollstart = function (fn) {
            this.onscrollstart = fn;
            return self;
        };
        this.scrollend = function (fn) {
            this.onscrollend = fn;
            return self;
        };
        this.scrollcancel = function (fn) {
            this.onscrollcancel = fn;
            return self;
        };

        this.zoomin = function (fn) {
            this.onzoomin = fn;
            return self;
        };
        this.zoomout = function (fn) {
            this.onzoomout = fn;
            return self;
        };

        this.isScrollable = function (e) {
            var dom = (e.target) ? e.target : e;
            if (dom.nodeName == 'OPTION') return true;
            while (dom && (dom.nodeType == 1) && !(/^BODY|HTML/.test(dom.nodeName))) {
                var dd = $(dom);
                var ov = dd.css('overflowY') || dd.css('overflowX') || dd.css('overflow') || '';
                if (/scroll|auto/.test(ov)) return (dom.clientHeight != dom.scrollHeight);
                dom = (dom.parentNode) ? dom.parentNode : false;
            }
            return false;
        };

        this.getViewport = function (me) {
            var dom = (me && me.parentNode) ? me.parentNode : false;
            while (dom && (dom.nodeType == 1) && !(/^BODY|HTML/.test(dom.nodeName))) {
                var dd = $(dom);
                if (/fixed|absolute/.test(dd.css("position"))) return dd;
                var ov = dd.css('overflowY') || dd.css('overflowX') || dd.css('overflow') || '';
                if ((/scroll|auto/.test(ov)) && (dom.clientHeight != dom.scrollHeight)) return dd;
                if (dd.getNiceScroll().length > 0) return dd;
                dom = (dom.parentNode) ? dom.parentNode : false;
            }
            return (dom) ? $(dom) : false;
        };

        this.triggerScrollEnd = function () {
            if (!self.onscrollend) return;

            var px = self.getScrollLeft();
            var py = self.getScrollTop();

            var info = { "type": "scrollend", "current": { "x": px, "y": py }, "end": { "x": px, "y": py } };
            self.onscrollend.call(self, info);
        }

        function execScrollWheel(e, hr, chkscroll) {
            var px, py;
            var rt = 1;

            if (e.deltaMode == 0) {  // PIXEL
                px = -Math.floor(e.deltaX * (self.opt.mousescrollstep / (18 * 3)));
                py = -Math.floor(e.deltaY * (self.opt.mousescrollstep / (18 * 3)));
            }
            else if (e.deltaMode == 1) {  // LINE
                px = -Math.floor(e.deltaX * self.opt.mousescrollstep);
                py = -Math.floor(e.deltaY * self.opt.mousescrollstep);
            }

            if (hr && self.opt.oneaxismousemode && (px == 0) && py) {  // classic vertical-only mousewheel + browser with x/y support 
                px = py;
                py = 0;
            }

            if (px) {
                if (self.scrollmom) { self.scrollmom.stop() }
                self.lastdeltax += px;
                self.debounced("mousewheelx", function () { var dt = self.lastdeltax; self.lastdeltax = 0; if (!self.rail.drag) { self.doScrollLeftBy(dt) } }, 15);
            }
            if (py) {
                if (self.opt.nativeparentscrolling && chkscroll && !self.ispage && !self.zoomactive) {
                    if (py < 0) {
                        if (self.getScrollTop() >= self.page.maxh) return true;
                    } else {
                        if (self.getScrollTop() <= 0) return true;
                    }
                }
                if (self.scrollmom) { self.scrollmom.stop() }
                self.lastdeltay += py;
                self.debounced("mousewheely", function () { var dt = self.lastdeltay; self.lastdeltay = 0; if (!self.rail.drag) { self.doScrollBy(dt) } }, 15);
            }

            e.stopImmediatePropagation();
            return e.preventDefault();
            //      return self.cancelEvent(e);
        };

        this.onmousewheel = function (e) {
            if (self.wheelprevented) return;
            if (self.locked) {
                self.debounced("checkunlock", self.resize, 250);
                return true;
            }
            if (self.rail.drag) return self.cancelEvent(e);

            if (self.opt.oneaxismousemode == "auto" && e.deltaX != 0) self.opt.oneaxismousemode = false;  // check two-axis mouse support (not very elegant)

            if (self.opt.oneaxismousemode && e.deltaX == 0) {
                if (!self.rail.scrollable) {
                    if (self.railh && self.railh.scrollable) {
                        return self.onmousewheelhr(e);
                    } else {
                        return true;
                    }
                }
            }

            var nw = +(new Date());
            var chk = false;
            if (self.opt.preservenativescrolling && ((self.checkarea + 600) < nw)) {
                //        self.checkarea = false;
                self.nativescrollingarea = self.isScrollable(e);
                chk = true;
            }
            self.checkarea = nw;
            if (self.nativescrollingarea) return true; // this isn't my business
            //      if (self.locked) return self.cancelEvent(e);
            var ret = execScrollWheel(e, false, chk);
            if (ret) self.checkarea = 0;
            return ret;
        };

        this.onmousewheelhr = function (e) {
            if (self.wheelprevented) return;
            if (self.locked || !self.railh.scrollable) return true;
            if (self.rail.drag) return self.cancelEvent(e);

            var nw = +(new Date());
            var chk = false;
            if (self.opt.preservenativescrolling && ((self.checkarea + 600) < nw)) {
                //        self.checkarea = false;
                self.nativescrollingarea = self.isScrollable(e);
                chk = true;
            }
            self.checkarea = nw;
            if (self.nativescrollingarea) return true; // this isn't my business
            if (self.locked) return self.cancelEvent(e);

            return execScrollWheel(e, true, chk);
        };

        this.stop = function () {
            self.cancelScroll();
            if (self.scrollmon) self.scrollmon.stop();
            self.cursorfreezed = false;
            self.scroll.y = Math.round(self.getScrollTop() * (1 / self.scrollratio.y));
            self.noticeCursor();
            return self;
        };

        this.getTransitionSpeed = function (dif) {
            var sp = Math.round(self.opt.scrollspeed * 10);
            var ex = Math.min(sp, Math.round((dif / 20) * self.opt.scrollspeed));
            return (ex > 20) ? ex : 0;
        };

        if (!self.opt.smoothscroll) {
            this.doScrollLeft = function (x, spd) {  //direct
                var y = self.getScrollTop();
                self.doScrollPos(x, y, spd);
            };
            this.doScrollTop = function (y, spd) {   //direct
                var x = self.getScrollLeft();
                self.doScrollPos(x, y, spd);
            };
            this.doScrollPos = function (x, y, spd) {  //direct
                var nx = (x > self.page.maxw) ? self.page.maxw : x;
                if (nx < 0) nx = 0;
                var ny = (y > self.page.maxh) ? self.page.maxh : y;
                if (ny < 0) ny = 0;
                self.synched('scroll', function () {
                    self.setScrollTop(ny);
                    self.setScrollLeft(nx);
                });
            };
            this.cancelScroll = function () { }; // direct
        }
        else if (self.ishwscroll && cap.hastransition && self.opt.usetransition) {
            this.prepareTransition = function (dif, istime) {
                var ex = (istime) ? ((dif > 20) ? dif : 0) : self.getTransitionSpeed(dif);
                var trans = (ex) ? cap.prefixstyle + 'transform ' + ex + 'ms ease-out' : '';
                if (!self.lasttransitionstyle || self.lasttransitionstyle != trans) {
                    self.lasttransitionstyle = trans;
                    self.doc.css(cap.transitionstyle, trans);
                }
                return ex;
            };

            this.doScrollLeft = function (x, spd) {  //trans
                var y = (self.scrollrunning) ? self.newscrolly : self.getScrollTop();
                self.doScrollPos(x, y, spd);
            };

            this.doScrollTop = function (y, spd) {   //trans
                var x = (self.scrollrunning) ? self.newscrollx : self.getScrollLeft();
                self.doScrollPos(x, y, spd);
            };

            this.doScrollPos = function (x, y, spd) {  //trans

                var py = self.getScrollTop();
                var px = self.getScrollLeft();

                if (((self.newscrolly - py) * (y - py) < 0) || ((self.newscrollx - px) * (x - px) < 0)) self.cancelScroll();  //inverted movement detection      

                if (self.opt.bouncescroll == false) {
                    if (y < 0) y = 0;
                    else if (y > self.page.maxh) y = self.page.maxh;
                    if (x < 0) x = 0;
                    else if (x > self.page.maxw) x = self.page.maxw;
                }

                if (self.scrollrunning && x == self.newscrollx && y == self.newscrolly) return false;

                self.newscrolly = y;
                self.newscrollx = x;

                self.newscrollspeed = spd || false;

                if (self.timer) return false;

                self.timer = setTimeout(function () {

                    var top = self.getScrollTop();
                    var lft = self.getScrollLeft();

                    var dst = {};
                    dst.x = x - lft;
                    dst.y = y - top;
                    dst.px = lft;
                    dst.py = top;

                    var dd = Math.round(Math.sqrt(Math.pow(dst.x, 2) + Math.pow(dst.y, 2)));

                    //          var df = (self.newscrollspeed) ? self.newscrollspeed : dd;

                    var ms = (self.newscrollspeed && self.newscrollspeed > 1) ? self.newscrollspeed : self.getTransitionSpeed(dd);
                    if (self.newscrollspeed && self.newscrollspeed <= 1) ms *= self.newscrollspeed;

                    self.prepareTransition(ms, true);

                    if (self.timerscroll && self.timerscroll.tm) clearInterval(self.timerscroll.tm);

                    if (ms > 0) {

                        if (!self.scrollrunning && self.onscrollstart) {
                            var info = { "type": "scrollstart", "current": { "x": lft, "y": top }, "request": { "x": x, "y": y }, "end": { "x": self.newscrollx, "y": self.newscrolly }, "speed": ms };
                            self.onscrollstart.call(self, info);
                        }

                        if (cap.transitionend) {
                            if (!self.scrollendtrapped) {
                                self.scrollendtrapped = true;
                                self.bind(self.doc, cap.transitionend, self.onScrollTransitionEnd, false); //I have got to do something usefull!!
                            }
                        } else {
                            if (self.scrollendtrapped) clearTimeout(self.scrollendtrapped);
                            self.scrollendtrapped = setTimeout(self.onScrollTransitionEnd, ms);  // simulate transitionend event
                        }

                        var py = top;
                        var px = lft;
                        self.timerscroll = {
                            bz: new BezierClass(py, self.newscrolly, ms, 0, 0, 0.58, 1),
                            bh: new BezierClass(px, self.newscrollx, ms, 0, 0, 0.58, 1)
                        };
                        if (!self.cursorfreezed) self.timerscroll.tm = setInterval(function () { self.showCursor(self.getScrollTop(), self.getScrollLeft()) }, 60);

                    }

                    self.synched("doScroll-set", function () {
                        self.timer = 0;
                        if (self.scrollendtrapped) self.scrollrunning = true;
                        self.setScrollTop(self.newscrolly);
                        self.setScrollLeft(self.newscrollx);
                        if (!self.scrollendtrapped) self.onScrollTransitionEnd();
                    });


                }, 50);

            };

            this.cancelScroll = function () {
                if (!self.scrollendtrapped) return true;
                var py = self.getScrollTop();
                var px = self.getScrollLeft();
                self.scrollrunning = false;
                if (!cap.transitionend) clearTimeout(cap.transitionend);
                self.scrollendtrapped = false;
                self._unbind(self.doc, cap.transitionend, self.onScrollTransitionEnd);
                self.prepareTransition(0);
                self.setScrollTop(py); // fire event onscroll
                if (self.railh) self.setScrollLeft(px);
                if (self.timerscroll && self.timerscroll.tm) clearInterval(self.timerscroll.tm);
                self.timerscroll = false;

                self.cursorfreezed = false;

                //self.noticeCursor(false,py,px);
                self.showCursor(py, px);
                return self;
            };
            this.onScrollTransitionEnd = function () {
                if (self.scrollendtrapped) self._unbind(self.doc, cap.transitionend, self.onScrollTransitionEnd);
                self.scrollendtrapped = false;
                self.prepareTransition(0);
                if (self.timerscroll && self.timerscroll.tm) clearInterval(self.timerscroll.tm);
                self.timerscroll = false;
                var py = self.getScrollTop();
                var px = self.getScrollLeft();
                self.setScrollTop(py);  // fire event onscroll        
                if (self.railh) self.setScrollLeft(px);  // fire event onscroll left

                self.noticeCursor(false, py, px);

                self.cursorfreezed = false;

                if (py < 0) py = 0
                else if (py > self.page.maxh) py = self.page.maxh;
                if (px < 0) px = 0
                else if (px > self.page.maxw) px = self.page.maxw;
                if ((py != self.newscrolly) || (px != self.newscrollx)) return self.doScrollPos(px, py, self.opt.snapbackspeed);

                if (self.onscrollend && self.scrollrunning) {
                    //          var info = {"type":"scrollend","current":{"x":px,"y":py},"end":{"x":self.newscrollx,"y":self.newscrolly}};
                    //          self.onscrollend.call(self,info);
                    self.triggerScrollEnd();
                }
                self.scrollrunning = false;

            };

        } else {

            this.doScrollLeft = function (x, spd) {  //no-trans
                var y = (self.scrollrunning) ? self.newscrolly : self.getScrollTop();
                self.doScrollPos(x, y, spd);
            };

            this.doScrollTop = function (y, spd) {  //no-trans
                var x = (self.scrollrunning) ? self.newscrollx : self.getScrollLeft();
                self.doScrollPos(x, y, spd);
            };

            this.doScrollPos = function (x, y, spd) {  //no-trans
                var y = ((typeof y == "undefined") || (y === false)) ? self.getScrollTop(true) : y;

                if ((self.timer) && (self.newscrolly == y) && (self.newscrollx == x)) return true;

                if (self.timer) clearAnimationFrame(self.timer);
                self.timer = 0;

                var py = self.getScrollTop();
                var px = self.getScrollLeft();

                if (((self.newscrolly - py) * (y - py) < 0) || ((self.newscrollx - px) * (x - px) < 0)) self.cancelScroll();  //inverted movement detection

                self.newscrolly = y;
                self.newscrollx = x;

                if (!self.bouncescroll || !self.rail.visibility) {
                    if (self.newscrolly < 0) {
                        self.newscrolly = 0;
                    }
                    else if (self.newscrolly > self.page.maxh) {
                        self.newscrolly = self.page.maxh;
                    }
                }
                if (!self.bouncescroll || !self.railh.visibility) {
                    if (self.newscrollx < 0) {
                        self.newscrollx = 0;
                    }
                    else if (self.newscrollx > self.page.maxw) {
                        self.newscrollx = self.page.maxw;
                    }
                }

                self.dst = {};
                self.dst.x = x - px;
                self.dst.y = y - py;
                self.dst.px = px;
                self.dst.py = py;

                var dst = Math.round(Math.sqrt(Math.pow(self.dst.x, 2) + Math.pow(self.dst.y, 2)));

                self.dst.ax = self.dst.x / dst;
                self.dst.ay = self.dst.y / dst;

                var pa = 0;
                var pe = dst;

                if (self.dst.x == 0) {
                    pa = py;
                    pe = y;
                    self.dst.ay = 1;
                    self.dst.py = 0;
                } else if (self.dst.y == 0) {
                    pa = px;
                    pe = x;
                    self.dst.ax = 1;
                    self.dst.px = 0;
                }

                var ms = self.getTransitionSpeed(dst);
                if (spd && spd <= 1) ms *= spd;
                if (ms > 0) {
                    self.bzscroll = (self.bzscroll) ? self.bzscroll.update(pe, ms) : new BezierClass(pa, pe, ms, 0, 1, 0, 1);
                } else {
                    self.bzscroll = false;
                }

                if (self.timer) return;

                if ((py == self.page.maxh && y >= self.page.maxh) || (px == self.page.maxw && x >= self.page.maxw)) self.checkContentSize();

                var sync = 1;

                function scrolling() {
                    if (self.cancelAnimationFrame) return true;

                    self.scrollrunning = true;

                    sync = 1 - sync;
                    if (sync) return (self.timer = setAnimationFrame(scrolling) || 1);

                    var done = 0;

                    var sc = sy = self.getScrollTop();
                    if (self.dst.ay) {
                        sc = (self.bzscroll) ? self.dst.py + (self.bzscroll.getNow() * self.dst.ay) : self.newscrolly;
                        var dr = sc - sy;
                        if ((dr < 0 && sc < self.newscrolly) || (dr > 0 && sc > self.newscrolly)) sc = self.newscrolly;
                        self.setScrollTop(sc);
                        if (sc == self.newscrolly) done = 1;
                    } else {
                        done = 1;
                    }

                    var scx = sx = self.getScrollLeft();
                    if (self.dst.ax) {
                        scx = (self.bzscroll) ? self.dst.px + (self.bzscroll.getNow() * self.dst.ax) : self.newscrollx;
                        var dr = scx - sx;
                        if ((dr < 0 && scx < self.newscrollx) || (dr > 0 && scx > self.newscrollx)) scx = self.newscrollx;
                        self.setScrollLeft(scx);
                        if (scx == self.newscrollx) done += 1;
                    } else {
                        done += 1;
                    }

                    if (done == 2) {
                        self.timer = 0;
                        self.cursorfreezed = false;
                        self.bzscroll = false;
                        self.scrollrunning = false;
                        if (sc < 0) sc = 0;
                        else if (sc > self.page.maxh) sc = self.page.maxh;
                        if (scx < 0) scx = 0;
                        else if (scx > self.page.maxw) scx = self.page.maxw;
                        if ((scx != self.newscrollx) || (sc != self.newscrolly)) self.doScrollPos(scx, sc);
                        else {
                            if (self.onscrollend) {
                                /*							
                                                var info = {"type":"scrollend","current":{"x":sx,"y":sy},"end":{"x":self.newscrollx,"y":self.newscrolly}};
                                                self.onscrollend.call(self,info);
                                */
                                self.triggerScrollEnd();
                            }
                        }
                    } else {
                        self.timer = setAnimationFrame(scrolling) || 1;
                    }
                };
                self.cancelAnimationFrame = false;
                self.timer = 1;

                if (self.onscrollstart && !self.scrollrunning) {
                    var info = { "type": "scrollstart", "current": { "x": px, "y": py }, "request": { "x": x, "y": y }, "end": { "x": self.newscrollx, "y": self.newscrolly }, "speed": ms };
                    self.onscrollstart.call(self, info);
                }

                scrolling();

                if ((py == self.page.maxh && y >= py) || (px == self.page.maxw && x >= px)) self.checkContentSize();

                self.noticeCursor();
            };

            this.cancelScroll = function () {
                if (self.timer) clearAnimationFrame(self.timer);
                self.timer = 0;
                self.bzscroll = false;
                self.scrollrunning = false;
                return self;
            };

        }

        this.doScrollBy = function (stp, relative) {
            var ny = 0;
            if (relative) {
                ny = Math.floor((self.scroll.y - stp) * self.scrollratio.y)
            } else {
                var sy = (self.timer) ? self.newscrolly : self.getScrollTop(true);
                ny = sy - stp;
            }
            if (self.bouncescroll) {
                var haf = Math.round(self.view.h / 2);
                if (ny < -haf) ny = -haf
                else if (ny > (self.page.maxh + haf)) ny = (self.page.maxh + haf);
            }
            self.cursorfreezed = false;

            py = self.getScrollTop(true);
            if (ny < 0 && py <= 0) return self.noticeCursor();
            else if (ny > self.page.maxh && py >= self.page.maxh) {
                self.checkContentSize();
                return self.noticeCursor();
            }

            self.doScrollTop(ny);
        };

        this.doScrollLeftBy = function (stp, relative) {
            var nx = 0;
            if (relative) {
                nx = Math.floor((self.scroll.x - stp) * self.scrollratio.x)
            } else {
                var sx = (self.timer) ? self.newscrollx : self.getScrollLeft(true);
                nx = sx - stp;
            }
            if (self.bouncescroll) {
                var haf = Math.round(self.view.w / 2);
                if (nx < -haf) nx = -haf;
                else if (nx > (self.page.maxw + haf)) nx = (self.page.maxw + haf);
            }
            self.cursorfreezed = false;

            px = self.getScrollLeft(true);
            if (nx < 0 && px <= 0) return self.noticeCursor();
            else if (nx > self.page.maxw && px >= self.page.maxw) return self.noticeCursor();

            self.doScrollLeft(nx);
        };

        this.doScrollTo = function (pos, relative) {
            var ny = (relative) ? Math.round(pos * self.scrollratio.y) : pos;
            if (ny < 0) ny = 0;
            else if (ny > self.page.maxh) ny = self.page.maxh;
            self.cursorfreezed = false;
            self.doScrollTop(pos);
        };

        this.checkContentSize = function () {
            var pg = self.getContentSize();
            if ((pg.h != self.page.h) || (pg.w != self.page.w)) self.resize(false, pg);
        };

        self.onscroll = function (e) {
            if (self.rail.drag) return;
            if (!self.cursorfreezed) {
                self.synched('scroll', function () {
                    self.scroll.y = Math.round(self.getScrollTop() * (1 / self.scrollratio.y));
                    if (self.railh) self.scroll.x = Math.round(self.getScrollLeft() * (1 / self.scrollratio.x));
                    self.noticeCursor();
                });
            }
        };
        self.bind(self.docscroll, "scroll", self.onscroll);

        this.doZoomIn = function (e) {
            if (self.zoomactive) return;
            self.zoomactive = true;

            self.zoomrestore = {
                style: {}
            };
            var lst = ['position', 'top', 'left', 'zIndex', 'backgroundColor', 'marginTop', 'marginBottom', 'marginLeft', 'marginRight'];
            var win = self.win[0].style;
            for (var a in lst) {
                var pp = lst[a];
                self.zoomrestore.style[pp] = (typeof win[pp] != "undefined") ? win[pp] : '';
            }

            self.zoomrestore.style.width = self.win.css('width');
            self.zoomrestore.style.height = self.win.css('height');

            self.zoomrestore.padding = {
                w: self.win.outerWidth() - self.win.width(),
                h: self.win.outerHeight() - self.win.height()
            };

            if (cap.isios4) {
                self.zoomrestore.scrollTop = $(window).scrollTop();
                $(window).scrollTop(0);
            }

            self.win.css({
                "position": (cap.isios4) ? "absolute" : "fixed",
                "top": 0,
                "left": 0,
                "z-index": globalmaxzindex + 100,
                "margin": "0px"
            });
            var bkg = self.win.css("backgroundColor");
            if (bkg == "" || /transparent|rgba\(0, 0, 0, 0\)|rgba\(0,0,0,0\)/.test(bkg)) self.win.css("backgroundColor", "#fff");
            self.rail.css({ "z-index": globalmaxzindex + 101 });
            self.zoom.css({ "z-index": globalmaxzindex + 102 });
            self.zoom.css('backgroundPosition', '0px -18px');
            self.resizeZoom();

            if (self.onzoomin) self.onzoomin.call(self);

            return self.cancelEvent(e);
        };

        this.doZoomOut = function (e) {
            if (!self.zoomactive) return;
            self.zoomactive = false;

            self.win.css("margin", "");
            self.win.css(self.zoomrestore.style);

            if (cap.isios4) {
                $(window).scrollTop(self.zoomrestore.scrollTop);
            }

            self.rail.css({ "z-index": self.zindex });
            self.zoom.css({ "z-index": self.zindex });
            self.zoomrestore = false;
            self.zoom.css('backgroundPosition', '0px 0px');
            self.onResize();

            if (self.onzoomout) self.onzoomout.call(self);

            return self.cancelEvent(e);
        };

        this.doZoom = function (e) {
            return (self.zoomactive) ? self.doZoomOut(e) : self.doZoomIn(e);
        };

        this.resizeZoom = function () {
            if (!self.zoomactive) return;

            var py = self.getScrollTop(); //preserve scrolling position
            self.win.css({
                width: $(window).width() - self.zoomrestore.padding.w + "px",
                height: $(window).height() - self.zoomrestore.padding.h + "px"
            });
            self.onResize();

            self.setScrollTop(Math.min(self.page.maxh, py));
        };

        this.init();

        $.nicescroll.push(this);

    };

    // Inspired by the work of Kin Blas
    // http://webpro.host.adobe.com/people/jblas/momentum/includes/jquery.momentum.0.7.js  


    var ScrollMomentumClass2D = function (nc) {
        var self = this;
        this.nc = nc;

        this.lastx = 0;
        this.lasty = 0;
        this.speedx = 0;
        this.speedy = 0;
        this.lasttime = 0;
        this.steptime = 0;
        this.snapx = false;
        this.snapy = false;
        this.demulx = 0;
        this.demuly = 0;

        this.lastscrollx = -1;
        this.lastscrolly = -1;

        this.chkx = 0;
        this.chky = 0;

        this.timer = 0;

        this.time = function () {
            return +new Date();//beautifull hack
        };

        this.reset = function (px, py) {
            self.stop();
            var now = self.time();
            self.steptime = 0;
            self.lasttime = now;
            self.speedx = 0;
            self.speedy = 0;
            self.lastx = px;
            self.lasty = py;
            self.lastscrollx = -1;
            self.lastscrolly = -1;
        };

        this.update = function (px, py) {
            var now = self.time();
            self.steptime = now - self.lasttime;
            self.lasttime = now;
            var dy = py - self.lasty;
            var dx = px - self.lastx;
            var sy = self.nc.getScrollTop();
            var sx = self.nc.getScrollLeft();
            var newy = sy + dy;
            var newx = sx + dx;
            self.snapx = (newx < 0) || (newx > self.nc.page.maxw);
            self.snapy = (newy < 0) || (newy > self.nc.page.maxh);
            self.speedx = dx;
            self.speedy = dy;
            self.lastx = px;
            self.lasty = py;
        };

        this.stop = function () {
            self.nc.unsynched("domomentum2d");
            if (self.timer) clearTimeout(self.timer);
            self.timer = 0;
            self.lastscrollx = -1;
            self.lastscrolly = -1;
        };

        this.doSnapy = function (nx, ny) {
            var snap = false;

            if (ny < 0) {
                ny = 0;
                snap = true;
            }
            else if (ny > self.nc.page.maxh) {
                ny = self.nc.page.maxh;
                snap = true;
            }

            if (nx < 0) {
                nx = 0;
                snap = true;
            }
            else if (nx > self.nc.page.maxw) {
                nx = self.nc.page.maxw;
                snap = true;
            }

            (snap) ? self.nc.doScrollPos(nx, ny, self.nc.opt.snapbackspeed) : self.nc.triggerScrollEnd();
        };

        this.doMomentum = function (gp) {
            var t = self.time();
            var l = (gp) ? t + gp : self.lasttime;

            var sl = self.nc.getScrollLeft();
            var st = self.nc.getScrollTop();

            var pageh = self.nc.page.maxh;
            var pagew = self.nc.page.maxw;

            self.speedx = (pagew > 0) ? Math.min(60, self.speedx) : 0;
            self.speedy = (pageh > 0) ? Math.min(60, self.speedy) : 0;

            var chk = l && (t - l) <= 60;

            if ((st < 0) || (st > pageh) || (sl < 0) || (sl > pagew)) chk = false;

            var sy = (self.speedy && chk) ? self.speedy : false;
            var sx = (self.speedx && chk) ? self.speedx : false;

            if (sy || sx) {
                var tm = Math.max(16, self.steptime); //timeout granularity

                if (tm > 50) {  // do smooth
                    var xm = tm / 50;
                    self.speedx *= xm;
                    self.speedy *= xm;
                    tm = 50;
                }

                self.demulxy = 0;

                self.lastscrollx = self.nc.getScrollLeft();
                self.chkx = self.lastscrollx;
                self.lastscrolly = self.nc.getScrollTop();
                self.chky = self.lastscrolly;

                var nx = self.lastscrollx;
                var ny = self.lastscrolly;

                var onscroll = function () {
                    var df = ((self.time() - t) > 600) ? 0.04 : 0.02;

                    if (self.speedx) {
                        nx = Math.floor(self.lastscrollx - (self.speedx * (1 - self.demulxy)));
                        self.lastscrollx = nx;
                        if ((nx < 0) || (nx > pagew)) df = 0.10;
                    }

                    if (self.speedy) {
                        ny = Math.floor(self.lastscrolly - (self.speedy * (1 - self.demulxy)));
                        self.lastscrolly = ny;
                        if ((ny < 0) || (ny > pageh)) df = 0.10;
                    }

                    self.demulxy = Math.min(1, self.demulxy + df);

                    self.nc.synched("domomentum2d", function () {

                        if (self.speedx) {
                            var scx = self.nc.getScrollLeft();
                            if (scx != self.chkx) self.stop();
                            self.chkx = nx;
                            self.nc.setScrollLeft(nx);
                        }

                        if (self.speedy) {
                            var scy = self.nc.getScrollTop();
                            if (scy != self.chky) self.stop();
                            self.chky = ny;
                            self.nc.setScrollTop(ny);
                        }

                        if (!self.timer) {
                            self.nc.hideCursor();
                            self.doSnapy(nx, ny);
                        }

                    });

                    if (self.demulxy < 1) {
                        self.timer = setTimeout(onscroll, tm);
                    } else {
                        self.stop();
                        self.nc.hideCursor();
                        self.doSnapy(nx, ny);
                    }
                };

                onscroll();

            } else {
                self.doSnapy(self.nc.getScrollLeft(), self.nc.getScrollTop());
            }

        }

    };


    // override jQuery scrollTop

    var _scrollTop = jQuery.fn.scrollTop; // preserve original function

    jQuery.cssHooks["pageYOffset"] = {
        get: function (elem, computed, extra) {
            var nice = $.data(elem, '__nicescroll') || false;
            return (nice && nice.ishwscroll) ? nice.getScrollTop() : _scrollTop.call(elem);
        },
        set: function (elem, value) {
            var nice = $.data(elem, '__nicescroll') || false;
            (nice && nice.ishwscroll) ? nice.setScrollTop(parseInt(value)) : _scrollTop.call(elem, value);
            return this;
        }
    };

    /*  
      $.fx.step["scrollTop"] = function(fx){    
        $.cssHooks["scrollTop"].set( fx.elem, fx.now + fx.unit );
      };
    */

    jQuery.fn.scrollTop = function (value) {
        if (typeof value == "undefined") {
            var nice = (this[0]) ? $.data(this[0], '__nicescroll') || false : false;
            return (nice && nice.ishwscroll) ? nice.getScrollTop() : _scrollTop.call(this);
        } else {
            return this.each(function () {
                var nice = $.data(this, '__nicescroll') || false;
                (nice && nice.ishwscroll) ? nice.setScrollTop(parseInt(value)) : _scrollTop.call($(this), value);
            });
        }
    };

    // override jQuery scrollLeft

    var _scrollLeft = jQuery.fn.scrollLeft; // preserve original function

    $.cssHooks.pageXOffset = {
        get: function (elem, computed, extra) {
            var nice = $.data(elem, '__nicescroll') || false;
            return (nice && nice.ishwscroll) ? nice.getScrollLeft() : _scrollLeft.call(elem);
        },
        set: function (elem, value) {
            var nice = $.data(elem, '__nicescroll') || false;
            (nice && nice.ishwscroll) ? nice.setScrollLeft(parseInt(value)) : _scrollLeft.call(elem, value);
            return this;
        }
    };

    /*  
      $.fx.step["scrollLeft"] = function(fx){
        $.cssHooks["scrollLeft"].set( fx.elem, fx.now + fx.unit );
      };  
    */

    jQuery.fn.scrollLeft = function (value) {
        if (typeof value == "undefined") {
            var nice = (this[0]) ? $.data(this[0], '__nicescroll') || false : false;
            return (nice && nice.ishwscroll) ? nice.getScrollLeft() : _scrollLeft.call(this);
        } else {
            return this.each(function () {
                var nice = $.data(this, '__nicescroll') || false;
                (nice && nice.ishwscroll) ? nice.setScrollLeft(parseInt(value)) : _scrollLeft.call($(this), value);
            });
        }
    };

    var NiceScrollArray = function (doms) {
        var self = this;
        this.length = 0;
        this.name = "nicescrollarray";

        this.each = function (fn) {
            for (var a = 0, i = 0; a < self.length; a++) fn.call(self[a], i++);
            return self;
        };

        this.push = function (nice) {
            self[self.length] = nice;
            self.length++;
        };

        this.eq = function (idx) {
            return self[idx];
        };

        if (doms) {
            for (var a = 0; a < doms.length; a++) {
                var nice = $.data(doms[a], '__nicescroll') || false;
                if (nice) {
                    this[this.length] = nice;
                    this.length++;
                }
            };
        }

        return this;
    };

    function mplex(el, lst, fn) {
        for (var a = 0; a < lst.length; a++) fn(el, lst[a]);
    };
    mplex(
      NiceScrollArray.prototype,
      ['show', 'hide', 'toggle', 'onResize', 'resize', 'remove', 'stop', 'doScrollPos'],
      function (e, n) {
          e[n] = function () {
              var args = arguments;
              return this.each(function () {
                  this[n].apply(this, args);
              });
          };
      }
    );

    jQuery.fn.getNiceScroll = function (index) {
        if (typeof index == "undefined") {
            return new NiceScrollArray(this);
        } else {
            var nice = this[index] && $.data(this[index], '__nicescroll') || false;
            return nice;
        }
    };

    jQuery.extend(jQuery.expr[':'], {
        nicescroll: function (a) {
            return ($.data(a, '__nicescroll')) ? true : false;
        }
    });

    $.fn.niceScroll = function (wrapper, opt) {
        if (typeof opt == "undefined") {
            if ((typeof wrapper == "object") && !("jquery" in wrapper)) {
                opt = wrapper;
                wrapper = false;
            }
        }
        var ret = new NiceScrollArray();
        if (typeof opt == "undefined") opt = {};

        if (wrapper || false) {
            opt.doc = $(wrapper);
            opt.win = $(this);
        }
        var docundef = !("doc" in opt);
        if (!docundef && !("win" in opt)) opt.win = $(this);

        this.each(function () {
            var nice = $(this).data('__nicescroll') || false;
            if (!nice) {
                opt.doc = (docundef) ? $(this) : opt.doc;
                nice = new NiceScrollClass(opt, $(this));
                $(this).data('__nicescroll', nice);
            }
            ret.push(nice);
        });
        return (ret.length == 1) ? ret[0] : ret;
    };

    window.NiceScroll = {
        getjQuery: function () { return jQuery }
    };



    if (!$.nicescroll) {
        $.nicescroll = new NiceScrollArray();
        $.nicescroll.options = _globaloptions;
    }

}));



















































