var isTouchDevice = 'ontouchstart' in window || 'onmsgesturechange' in window;
var isDesktop = $(window).width() != 0 && !isTouchDevice ? true : false;
var isTouchIE = navigator.msMaxTouchPoints > 0;
var isiPad = navigator.userAgent.indexOf('iPad') != -1;
var isiPhone = navigator.userAgent.indexOf('iPhone') != -1;
var isAndroid = navigator.userAgent.indexOf('Android') != -1;
var isIE = navigator.userAgent.toLowerCase().indexOf('msie') != -1 && navigator.msMaxTouchPoints < 0;
var isChrome = navigator.userAgent.toLowerCase().indexOf('chrome') > -1;
var isFirefox = navigator.userAgent.toLowerCase().indexOf('firefox') > -1;
var PageLoad = true;
var PageHome = false;
var Yheight = $(window).height();
var Xwidth = $(window).width();
var Finish = 0;
var current;
var maxWidth = 900;



function changeUrl(url, title, description, keyword, dataName, titleog, descriptionog) {
    if (window.history.pushState !== undefined) {
        var c_href = document.URL;
        if (c_href != url && url != '')
            window.history.pushState({ path: url, dataName: dataName, title: title, keyword: keyword, description: description, titleog: titleog, descriptionog: descriptionog }, "", url);
    }
    if (title != '') {
        var currenthost = $('#currenthost').html();
        var sitename = $('#sitename').html();
        $('title').html(title + ' - ' + sitename);
        $('meta[property="og:title"]').attr('content', title + '-' + sitename);
        $('meta[property="og:url"]').attr('content', currenthost + url);
    }

}
function init() {
    var Yheight = $(window).height();
    var Xwidth = $(window).width();

}
//click prev next page reload
$(window).bind("popstate", function (e) {
    if (e.originalEvent.state !== null) {
        location.reload();
    }

});
$(window).bind('resize', function () {
    init();
    LayOut();

});


/*****SEARCH*****/
var show;
function SubmitSearch() {
    var keysearch = $('#keysearch').val();
    var href_search = "/tim-kiem.aspx";
    if (keysearch != '') {
        var url = href_search + '?keysearch=' + encodeURIComponent(keysearch)
        window.location = url;
        return false;
    }
}
function Search() {



    $('.btn-search a').bind('click', function (e) {
        if ($(window).width() > maxWidth) {
            if (show == 1) {
                $('#keysearch').css({ 'width': 0, 'opacity': 0 });
                show = 0;
                SubmitSearch();
            }
            else {
                $('#keysearch').css({ 'width': 250, 'opacity': 1 });
                show = 1;
                SubmitSearch();
            }
        }
        else {

        }

        $('#keysearch').keydown(function (e) {
            if (e.keyCode == 13) {
                var keysearch = $('#keysearch').val();
                var href_search = "/tim-kiem.aspx";
                if (keysearch != '') {
                    var url = href_search + '?keysearch=' + encodeURIComponent(keysearch)
                    window.location = url;
                    return false;
                }

            }
        });

    });




}
function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == sParam) {
            return sParameterName[1];
        }
    }
}
/*****end SEARCH*****/


function ChangePages() {


}
var LoadSlide = {

    FullBackground: function () {

       

    },
    HomeSlide: function () {
        var SlideHome = $('.slide-home ul');
        SlideHome.owlCarousel({
            nav: true,
            lazyLoad: false,
            transitionStyle: "fade",
            responsive: {
                960: {
                    items: 1
                },
                maxWidth: {
                    items: 1
                }
            },
            animateOut: 'slideOutLeft',
            animateIn: 'flipInX',
            items: 3,
        });
    },
    Solutions: function () {
        if ($('.solutions-list').length) {

            if ($(window).width() <= maxWidth) {
                TimePlay = false;
            } else {
                TimePlay = $('.solutions-list').attr('data-time');
            }
            $('.solutions-list').owlCarousel({
                itemsCustom: [
                  [0, 1],
                  [500, 1],
                  [600, 1],
                  [700, 1],
                  [1000, 1],
                  [1400, 1],
                  [1600, 1],
                  [1900, 1],
                ],
                autoPlay: TimePlay,
                stopOnHover: true,
                autoHeight: true,
                slideSpeed: 800,
                paginationSpeed: 800,
                navigation: true,
                lazyLoad: true,
                lazyEffect: "fade",
            });

            var allitem = $('.solutions-list .slide-item').length;

            if ($(window).width() > 690) {
                if (allitem < 2) {
                    $('.slide-item').css({ 'float': 'none' });
                    $('.slide-item').parent().parent().parent().css({ 'text-align': 'center' });
                } else {
                    $('.slide-item').css({ 'float': 'left' });
                    $('.slide-item').parent().parent().parent().css({ 'text-align': 'left' });
                }

            } else {
                $('.slide-item').css({ 'float': 'left' });
                $('.slide-item').parent().parent().parent().css({ 'text-align': 'left' });
            }


        }
    },
    News: function () {
        if ($('.news-link').length) {


            $('.news-link').owlCarousel({
                itemsCustom: [
                  [0, 1],
                  [500, 2],
                  [600, 2],
                  [700, 2],
                  [1000, 3],
                  [1400, 3],
                  [1600, 3],
                  [1900, 3],
                ],
                slideSpeed: 800,
                paginationSpeed: 800,
                navigation: true,
                lazyLoad: true,
                lazyEffect: "fade",
            });

            var allitem = $('.news-link .slide-item').length;

            if ($(window).width() > 690) {
                if (allitem < 2) {
                    $('.slide-item').css({ 'float': 'none' });
                    $('.slide-item').parent().parent().parent().css({ 'text-align': 'center' });
                } else {
                    $('.slide-item').css({ 'float': 'left' });
                    $('.slide-item').parent().parent().parent().css({ 'text-align': 'left' });
                }

            } else {
                $('.slide-item').css({ 'float': 'left' });
                $('.slide-item').parent().parent().parent().css({ 'text-align': 'left' });
            }


        }
    },
    Service: function () {
        if ($('.solutions-list').length) {

            if ($(window).width() <= maxWidth) {
                TimePlay = false;
            } else {
                TimePlay = $('.solutions-list').attr('data-time');
            }
            $('.solutions-list').owlCarousel({
                itemsCustom: [
                  [0, 1],
                  [500, 1],
                  [600, 1],
                  [700, 1],
                  [1000, 1],
                  [1400, 1],
                  [1600, 1],
                  [1900, 1],
                ],
                autoPlay: TimePlay,
                stopOnHover: true,
                autoHeight: true,
                slideSpeed: 800,
                paginationSpeed: 800,
                navigation: true,
                lazyLoad: true,
                lazyEffect: "fade",
            });

            var allitem = $('.solutions-list .slide-item').length;

            if ($(window).width() > 690) {
                if (allitem < 2) {
                    $('.slide-item').css({ 'float': 'none' });
                    $('.slide-item').parent().parent().parent().css({ 'text-align': 'center' });
                } else {
                    $('.slide-item').css({ 'float': 'left' });
                    $('.slide-item').parent().parent().parent().css({ 'text-align': 'left' });
                }

            } else {
                $('.slide-item').css({ 'float': 'left' });
                $('.slide-item').parent().parent().parent().css({ 'text-align': 'left' });
            }


        }
    },
    Logos: function () {
        if ($('.logo-list').length) {

            if ($(window).width() <= maxWidth) {
                TimePlay = false;
            } else {
                TimePlay = $('.logo-list').data('time');
            }
            $('.logo-list').owlCarousel({
                itemsCustom: [
                  [0, 1],
                  [500, 2],
                  [600, 2],
                  [700, 3],
                  [1000, 6],
                  [1400, 8],
                  [1600, 9],
                  [1900, 9],
                ],
                autoPlay: TimePlay,
                stopOnHover: true,
                autoHeight: true,
                slideSpeed: 800,
                paginationSpeed: 800,
                navigation: true,
                lazyLoad: true,
                lazyEffect: "fade",
            });

            var allitem = $('.logo-list .slide-item').length;

            //if ($(window).width() > 690) {
            //    if (allitem < 2) {
            //        $('.slide-item').css({ 'float': 'none' });
            //        $('.slide-item').parent().parent().parent().css({ 'text-align': 'center' });
            //    } else {
            //        $('.slide-item').css({ 'float': 'left' });
            //        $('.slide-item').parent().parent().parent().css({ 'text-align': 'left' });
            //    }

            //} else {
            //    $('.slide-item').css({ 'float': 'left' });
            //    $('.slide-item').parent().css({ 'text-align': 'left' });
            //}


        }
    },
}//LoadSlide

function contentslide() {
    $('.active').find('.content-slide').addClass('show');
}
function scrollBody() {
    if (Xwidth > maxWidth) {

        if (isFirefox) {
            $('body').getNiceScroll().show();
            $('body').niceScroll({ touchbehavior: false, horizrailenabled: false, cursordragontouch: true, grabcursorenabled: false });
            $('.scrollA').stop().animate({ scrollTop: "0px" }, 200, 'linear');
        } else {
            $('.nicescroll-rails').css({ 'width': 6, 'background-color': 'rgba(0,0,0,0.3)', 'z-index': 100 });
            $('body').getNiceScroll().show();
            $('body').niceScroll({ touchbehavior: false, horizrailenabled: false, cursordragontouch: true, grabcursorenabled: false });
            $('.scrollA').stop().animate({ scrollTop: "0px" }, 200, 'linear');
            setTimeout(function () {
                $('body').getNiceScroll().resize()
            }, 500);
        }
        //$('.scrollA').bind('scroll', function () {
        //    var scrollActive = $('.scrollA').scrollTop();
        //    if (scrollActive > 30) {

        //    }
        //    if (scrollActive == 0) {

        //    }
        //});

    }
}
function scrollA() {

    if (isFirefox) {
        $('.scrollA').getNiceScroll().show();
        $('.scrollA').niceScroll({ touchbehavior: true, horizrailenabled: false, cursordragontouch: true, grabcursorenabled: false });
        // $('.scrollA').stop().animate({ scrollTop: "0px" }, 200, 'linear');
    } else {

        $('.scrollA').getNiceScroll().show();
        $('.scrollA').niceScroll({ touchbehavior: true, horizrailenabled: false, cursordragontouch: true, grabcursorenabled: false });
        // $('.scrollA').stop().animate({ scrollTop: "0px" }, 200, 'linear');
        setTimeout(function () {
            $('.scrollA').getNiceScroll().resize()
        }, 500);
    }
    //$('.scrollA').bind('scroll', function () {
    //    var scrollActive = $('.scrollA').scrollTop();
    //    if (scrollActive > 30) {

    //    }
    //    if (scrollActive == 0) {

    //    }
    //});


}
function scrollN() {
    if (Xwidth > maxWidth) {

        if (isFirefox) {
            $('.scrollN').getNiceScroll().show();
            $('.scrollN').niceScroll({ touchbehavior: true, horizrailenabled: false, cursordragontouch: true, grabcursorenabled: false });
            // $('.scrollA').stop().animate({ scrollTop: "0px" }, 200, 'linear');
        } else {

            $('.scrollN').getNiceScroll().show();
            $('.scrollN').niceScroll({ touchbehavior: true, horizrailenabled: false, cursordragontouch: true, grabcursorenabled: false });
            // $('.scrollA').stop().animate({ scrollTop: "0px" }, 200, 'linear');
            setTimeout(function () {
                $('.scrollN').getNiceScroll().resize()
            }, 500);
        }
        $('.scrollN').bind('scroll', function () {
            var scrollActive = $('.scrollN').scrollTop();
            if (scrollActive > 30) {
                $('.header').css({ 'bottom': -100 });
                $('.right-news').css({ 'height': '95%' });


            }
            if (scrollActive == 0) {
                $('.header').css({ 'bottom': 0 });
                $('.right-news').css({ 'height': '78%' });
            }

            //var delta = 0;
            //$('.content-detail-news').mousewheel(function (e, delta) {
            //    if (delta > 0) {
            //        $('.header').css({ 'bottom': 0 });
            //        $('.right-news').css({ 'height': '78%' });
            //    } else {

            //    }
            //});

        });

    }
}
function scrollTab() {
    if (Xwidth > maxWidth) {

        if (isFirefox) {
            $('.scrollTab').getNiceScroll().show();
            $('.scrollTab').niceScroll({ touchbehavior: true, horizrailenabled: false, cursordragontouch: false, grabcursorenabled: false });
            $('.scrollTab').stop().animate({ scrollTop: "0px" }, 200, 'linear');
        } else {

            $('.scrollTab').getNiceScroll().show();
            $('.scrollTab').niceScroll({ touchbehavior: true, horizrailenabled: false, cursordragontouch: false, grabcursorenabled: false });
            $('.scrollTab').stop().animate({ scrollTop: "0px" }, 200, 'linear');
            var scrollActive = $('.scrollTab').scrollTop();
            setTimeout(function () {
                $('.scrollTab').getNiceScroll().resize()
            }, 500);
        }



        $('.scrollTab').bind('scroll', function () {
            var scrollActive = $('.scrollTab').scrollTop();
            if (scrollActive > 30) {

            }
            if (scrollActive == 0) {

            }
        });

    }
}

function scrollPic() {
    if (isFirefox) {
        $('.scrollPic').getNiceScroll().show();
        $('.scrollPic').niceScroll({ touchbehavior: false, horizrailenabled: false, cursordragontouch: false, grabcursorenabled: false });
        $('.scrollPic').stop().animate({ scrollTop: "0px" }, 200, 'linear');
    } else {

        $('.scrollPic').getNiceScroll().show();
        $('.scrollPic').niceScroll({ touchbehavior: true, horizrailenabled: false, cursordragontouch: false, grabcursorenabled: false });
        $('.scrollPic').stop().animate({ scrollTop: "0px" }, 200, 'linear');
        var scrollActive = $('.scrollPic').scrollTop();
        setTimeout(function () {
            $('.scrollPic').getNiceScroll().resize()
        }, 500);
    }
}

function Content() {


}

function LoadVideo(src) {
    $('html, body').stop().animate({ scrollTop: 0 }, 'slow');
    $("body").getNiceScroll().hide();
    if ($('.video-play').length) {

        $('.video-play').delay(500).fadeIn(300, 'linear');
        $('body').append('<div class="loadicon" style="display:block"><span class="circle"></span></div>');
        $('.video-wrap iframe').attr('src', src);
        $('.video-play iframe').delay(300).load(function () {
            $('.video-wrap').fadeIn(400, 'linear', function () {
                $('.video-wrap').css({ 'opacity': 1 });
                $('.loadicon').remove();

            });
        });
        $('.close-video').click(function () {
            $('.video-play').fadeOut(300, 'linear');
            $('.video-wrap iframe').attr('src', '');
            $('.loadicon').remove();
            $("body").getNiceScroll().show();
        });

        return false;

    }

}
function Load() {
    if ($('.zoom-pic').length) {
        $('.zoom-pic').click(function () {
            $('.load-pics').fadeIn(300, 'linear');
            $('.load-pics').append('<div class="pic"></div>');
            $('html, body').animate({ scrollTop: 0 }, 'slow');
            $('body').css({ 'overflow': 'hidden' });
            $('.container').css({ 'width': '100%', 'height': $(window).height() });
            $('body').append('<div class="loadicon" style="display:block"><span class="circle"></span></div>');
            $('body').append('<div class="close-pics" style="display:block"></div>');

            var src = $(this).attr('data-src');
            $('.load-pics .pic').append('<img src ="' + src + '" alt="pic" />');
            $('.load-pics img').load(function () {
                $('.pic').fadeIn(400, 'linear', function () {
                    if (isTouchDevice && isChrome) {
                        $('.pic').getNiceScroll().show();
                        $('.pic').niceScroll({ touchbehavior: false, grabcursorenabled: true, horizrailenabled: false, cursordragontouch: true });
                        $('.pic').animate({ scrollTop: "0px" });
                    } else {
                        $('.pic').getNiceScroll().show();
                        $('.pic').niceScroll({ touchbehavior: true, grabcursorenabled: true, horizrailenabled: false, cursordragontouch: true });
                        $('.pic').animate({ scrollTop: "0px" });
                    }
                    $('.pic img').css({ 'opacity': 1 });
                    $('.loadicon').remove();

                });
            });
            $('.close-pics').click(function () {
                $('.load-pics').fadeOut(300, 'linear');
                $('.load-pics .pic').remove();
                $('.close-pics').remove();
            });
            return false;
        });
    }

}
function ZoomPicPorudct(url) {
    $('.loadicon').remove();
    $('body').append('<div class="loadicon" style="display:block"><span class="circle"></span></div>');
    if ($('.pic-zoom').length) {
        $('.pic-zoom .item-pic').children().remove();

        $('.pic-zoom .item-pic').append('<img src ="' + url + '" alt="pic-item" id="wheelzoom" />');
        $('.pic-zoom .item-pic img').load(function () {
            $('.item-pic').animate({ 'opacity': 1 }, 300, 'linear', function () {
                if (Xwidth <= maxWidth) {
                    $('.item-pic,.item-pic img').css({ 'height': 'auto' });
                }
                else {
                    $('.item-pic,.item-pic img').css({ 'height': Yheight });
                }

                $('.item-pic img').delay(200).css({ 'opacity': 1 });
                $('.loadicon').remove();

            });
        });//load
    }

}
function LoadPicProduct(id) {
    $('html, body').stop().animate({ scrollTop: 0 }, 'slow');
    $("body").getNiceScroll().hide();
    $('.pic-album').fadeIn().animate({ 'height': Yheight }, 800, 'easeInOutExpo', function () {
        $('.pic-album').append('<div class="close-album"></div>');

        $.ajax({
            url: '/load/picproduct.aspx?id=' + id, cache: false, success: function (data) {
                $('.pic-album').append(data);
                $('.load-album').delay(300).fadeIn(800, 'linear', function () {
                    $('.loadicon').remove();
                    $('.pic-right,.pics-product').css({ 'height': Yheight });
                    scrollPic();

                    $('.pics-product ul li:first-child a.view-pic-product').trigger('click');
                });



                //load zoom pic item

                $('.view-pic-product').click(function (e) {
                    e.preventDefault();

                    $('.pics-product ul li').removeClass('active');
                    $(this).parent().addClass('active');

                    var url = $(this).attr('data-url');

                    $('.pic-zoom').animate({ 'opacity': 1 }, 100, 'linear');
                    ZoomPicPorudct(url);
                })

                $('.close-album').click(function () {
                    $('.pic-album').animate({ 'height': '0%' }, 800, 'easeInOutExpo', function () {
                        $('.pic-album').fadeOut();
                        $('.pic-album').children().remove();
                        $('.loadicon').remove();
                        $("body").getNiceScroll().show();
                    });
                });


            }
        });//end ajax
    });

}
function LoadAlbum(id, title) {
    $('body').append('<div class="loadicon" style="display:block"></div>');

    $('.images-album').fadeIn(600, 'easeInOutExpo', function () {

        $.ajax({
            url: '/load/album.aspx?id=' + id, cache: false, success: function (data) {
                $('.album-wrap').append(data);


                //slide


                $('.load-album').delay(300).fadeIn(800, 'linear', function () {
                    $('.title-album').html(title);
                    $('.loadicon').remove();
                    $('.slide-album').animate({ 'opacity': 1 }, 600, 'linear');

                    $('.slide-album').cycle({
                        fx: 'scrollHorz',
                        timeout: 0,
                        containerResize: 0,
                        slideResize: 0,
                        fit: 1,
                        prev: '.slide-album-nav .prev-pic',
                        next: '.slide-album-nav .next-pic',
                        speed: 1000,
                        timeout: 7000,
                        after: function (currSlideElement, nextSlideElement, options, forwardFlag) {

                            $('.counter-album').text((options.currSlide + 1) + '/' + (options.slideCount));
                        }
                    });

                });

                $('.close-album').click(function () {
                    $('.images-album').fadeOut(800, 'easeInOutExpo', function () {
                        $('.images-album').fadeOut();
                        $('.images-album .album-wrap').children().remove();
                    });
                });

            }
        });//end ajax
    });

}
function NewsLoad(url) {
    $.ajax({
        url: url, cache: false, success: function (data) {
            $('.content-box').append(data);
            $('.loadicon').remove();
        }
    });
}
function LoadNewsList(id) {
    $('body').append('<div class="loadicon" style="display:block"><span class="circle"></span></div>');
    $('.news-link').animate({ 'opacity': 0, 'transform': 'scale(0)' }, 10, 'linear', function () {
        $('.news-link').children().remove();
        $.ajax({
            url: '/load/listnews.aspx?id=' + id, cache: false, success: function (data) {

                $('.news-link').animate({ 'opacity': 1 }, 600, 'linear', function () {
                    $('.news-link').children().remove();
                    $('.news-link').append(data);

                    $('.box-news').each(function (i) {
                        var item = $(this);
                        setTimeout(function () { $(item).addClass('show') }, (i + 3) * 120);

                    });

                });
                $('.loadicon').remove();
                scrollN();
            }
        });
    });
}
function LoadNewsDetail(id) {
    $('body').append('<div class="loadicon" style="display:block"><span class="circle"></span></div>');
    $('.news-content-detail').animate({ 'opacity': 0, 'transform': 'scale(0)' }, 10, 'linear', function () {
        $('.news-content-detail').children().remove();
        $.ajax({
            url: '/load/newsdetail.aspx?id=' + id, cache: false, success: function (data) {
                $('.news-content-detail').children().remove();
                $('.news-content-detail').append(data);
                $('.news-content-detail').animate({ 'opacity': 1 }, 600, 'linear');
                $('.loadicon').remove();
                scrollN();
            }
        });
    });


}
function LoadServiceDetail(id) {
    $('body').append('<div class="loadicon" style="display:block"><span class="circle"></span></div>');

    $('.detail-pages').animate({ 'opacity': 0, 'transform': 'scale(0)' }, 10, 'linear', function () {
        $('.detail-pages').children().remove();
        $.ajax({
            url: '/load/servicedetail.aspx?id=' + id, cache: false, success: function (data) {
                $('.detail-pages').append(data);
                //   $('.text-about').css({ 'opacity': 0 });
                $('.text-about').css({ 'top': 200, 'opacity': 0 });

                $('.detail-pages').animate({ 'opacity': 1 }, 600, 'linear');
                $('.loadicon').animate({ 'opacity': 0 }, 600, 'linear', function () {
                    $(this).remove();
                });
                scrollN();

                setTimeout(function () { $('.text-about').css({ 'top': 0, 'opacity': 1 }); }, 500);
            }
        });
    });


}
function LoadRecruitmentDetail(id, title) {
    $('.loadicon').remove();
    $('body').append('<div class="loadicon" style="display:block"><span class="circle"></span></div>');
    //$('html, body').stop().animate({ scrollTop: 0 }, 'slow');
    $('.content-recruitment').removeAttr('style');

    $('.content-recruitment').animate({ 'opacity': 0 }, 600, 'linear', function () {
        $(this).children().remove();
        $.ajax({
            url: '/load/recruitmentdetail.aspx?id=' + id, cache: false, success: function (data) {
                $('.content-recruitment').append(data);

                setTimeout(function () { $('.content-recruitment').animate({ 'opacity': 1 }, 600, 'linear'); }, 500);
                $('.loadicon').remove();
                scrollA();

                $('.top-pages h1').text(title);
            }
        });
    });


}
function LoadDetailPolicies(id) {
    $('body').append('<div class="loadicon" style="display:block"></div>');
    $('.content-policies').animate({ 'opacity': 0 }, 600, 'linear', function () {
        $(this).children().remove();
        $.ajax({
            url: '/load/policies.aspx?id=' + id, cache: false, success: function (data) {
                $('.content-policies').append(data);
                $('.content-policies').animate({ 'opacity': 1 }, 600, 'linear');
                $('.loadicon').remove();
                scrollA();
            }
        });
    });


}
function LoadProducts(url, callback) {
    $('body').append('<div class="loadicon" style="display:block"></div>');
    $.ajax({
        url: url, cache: false, success: function (data) {
            $('.content-products-sub').append(data);
            $('.loadicon').remove();
            callback();
        }
    });
}
function LoadProductDetail(url, callback) {
    $('body').append('<div class="loadicon" style="display:block"></div>');
    $.ajax({
        url: url, cache: false, success: function (data) {
            $('.load-product-detail').append(data);
            $('.loadicon').remove();
            callback();
        }
    });
}

function LinkPages() {
    $('').click(function (e) {
        e.preventDefault();
        $('.loadicon').remove();
        $('body').append('<div class="loadicon" style="display:block"><span class="circle"></span></div>');
        linkLocation = $(this).attr('href');
        //    $('.over-load').fadeOut();
        $('.support-icon').css({ 'height': 0 });
        $('.all-sub').css({ 'height': 0 });
        setTimeout(function () { $('.container').addClass('hidex'); });
        $('.logo-m').fadeOut();
        if (Xwidth <= maxWidth) {
            $('.nav-open').removeClass('active');
            $('body,html').removeClass('no-scroll');
            $('.header').removeClass('show');
            $('.container').removeClass('show');
        }

        setTimeout(function () { window.location = linkLocation; });


        return false;
    });



}

//string slogan
$(".slogan-top").css('opacity', '1').lettering('words').children("span").lettering();
play();

function LoadPages() {
    LayOut();
    LinkPages();
    Load();


    $('.hotline-top').delay(3000).fadeIn(800);

    var Xwidth = $(window).width();
    var Yheight = $(window).height();
    $('.colum-box').css({ 'width': Xwidth });
    $('.box-products').css({ 'width': Xwidth - 350 });

    var url = window.location.href.toLowerCase();

    $('.navigation ul li').each(function () {

        if (url.indexOf($(this).children('a').attr('href').toLowerCase().replace('.aspx', '')) != -1) {
            $(this).addClass('current');
        }
    });

    //product-hot
    //var getCurrent = '';
    //$('.products-hot').css({ 'height': Yheight - 75 });
    ////get current
    //$('.navigation ul li').each(function (e) {
    //    if ($(this).attr('class') == 'current') {
    //        getCurrent = e;

    //    }
    //});

    //$('.navigation ul li:nth-child(3) a').click(function (e) {
    //    e.preventDefault();

    //    $('.navigation ul li').removeClass('current');
    //    $(this).parent().addClass('current');


    //    $('.products-hot').fadeIn();
    //    scrollA();
    //$('.navigation .item').each(function (i) {
    //    var item = $(this);
    //    setTimeout(function () { $(item).addClass('show') }, (i + 3) * 120);

    //});
    //    $('.products-hot .item-products').each(function (i) {
    //        var item = $(this);
    //        setTimeout(function () { $(item).addClass('show') }, (i + 3) * 120);

    //    });
    //});


    //$('.close-products-hot').click(function (e) {
    //    // $('.products-hot').removeClass('show');
    //    $('.products-hot').fadeOut().animate(300, 'linear', function () {
    //        //  $('.navigation ul li:nth-child(' + e + ') a').addClass('current');
    //        $('.products-hot .item-products').removeClass('show');
    //        $('.navigation ul li').removeClass('current');
    //        $('.sub-nav-product ul li').removeClass('show');
    //        if (getCurrent != '') {
    //            $('.navigation ul li:nth-child(' + getCurrent + ')').addClass('current');
    //        }
    //    });
    //});
    //$('.products-hot a').click(function (e) {
    //    e.preventDefault();
    //    $('.close-products-hot').trigger('click');
    //    $('.products-hot').removeClass('show');

    //});
    //product-hot

    LoadSlide.Logos();
    //home-pages
    //if ($('#home-pages').length) {
    //    $('.navigation ul li:nth-child(1)').addClass('current');
    //    $('.playvideo').click(function (e) {
    //        e.preventDefault();
    //        var src = $(this).attr('data-video');
    //        LoadVideo(src);
    //    });

    //    $('.box-top').each(function (i) {
    //        var item = $(this);
    //        setTimeout(function () { $(item).addClass('show') }, (i + 1) * 200);
    //    });


    //    LoadSlide.Logos();
    //}



    //if ($('#about-page').length) {
    //    $('.navigation ul li:nth-child(2)').addClass('current');
    //}
    //about-pages
    //if ($('#about-page').length) {
    //    $('.navigation ul li:nth-child(2)').addClass('current');
    //    $('.sub-nav ul li a').click(function (e) {
    //        e.preventDefault();
    //        $('.sub-nav li').removeClass('current');
    //        $(this).parent().addClass('current');
    //        $('.item-certificate').removeClass('fadeinup');
    //        $('.item-certificate').each(function (i) {
    //            var item = $(this);
    //            setTimeout(function () { $(item).addClass('fadeinup') }, (i + 1) * 500);
    //        });
    //        $('.content-box .colum-box').addClass('play');
    //        var allcolum = $('.colum-box').length;
    //        var widthcolum = $('.colum-box').width();
    //        $('.content-box').width(allcolum * widthcolum);
    //        var XCurrent = $('.content-box').offset().left;
    //        var ShowColum = $(this).attr('data-id');
    //        var XColum = $('.content-box .colum-box[data-id= "' + ShowColum + '"]').offset().left;
    //        $('.content-box').stop().animate({ 'left': XCurrent - XColum }, 800, 'easeInOutExpo', function () {
    //            $('.content-box .colum-box').removeClass('play');
    //        });
    //        var url = $(this).attr('href');
    //        var title = $(this).attr('data-title');
    //        changeUrl(url, title, '', '', '', '', '');
    //        scrollA();
    //        var dataID = $(this).attr('data-id');
    //        $('.content-box .colum-box').removeClass('active');
    //        $('.content-box .colum-box').each(function (i) {
    //            if ($(this).attr('data-id') == dataID) {
    //                $(this).addClass('active');

    //            }
    //        });

    //    });

    //    if ($('.sub-nav li.current').length) {
    //        $('.sub-nav li.current a').trigger('click');
    //    } else {
    //        $('.sub-nav li:first-child').find('a').trigger('click');
    //    }

    //}//#about-pages



    //if ($('#products-pages').length) {
    //    $('.navigation ul li:nth-child(3)').addClass('current');
    //    $('.box-product').each(function (i) {
    //        var item = $(this);
    //        setTimeout(function () { $(item).addClass('show') }, (i + 3) * 120);

    //    });
    //    LoadSlide.Solutions();

    //$('.navigation ul li:nth-child(3)').addClass('current');

    //var has = window.location.hash;

    //if (has == '#show') {
    //    setTimeout(function () { $('.view-products').trigger('click'); }, 500);
    //}

    //$('.view-products').click(function (e) {
    //    e.preventDefault();
    //    $('.about-products,.content-products').animate({ 'top': -(Yheight - 45) }, maxWidth, 'easeInOutExpo');
    //    if ($('.sub-nav li.current').length) {
    //        $('.sub-nav li.current a').trigger('click');

    //    } else {
    //        $('.sub-nav li:first-child').find('a').trigger('click');
    //    }
    //});

    //if ($('.nodelevel').html() == '3') {
    //    $('.view-products').trigger('click');
    //}

    //setTimeout(function () {
    //    if ($('.sub-nav li.current').length) {
    //        $('.sub-nav li.current a').trigger('click');
    //    }
    //}, 500);
    //$('.sub-nav ul li a').click(function (e) {
    //    e.preventDefault();
    //    $('.sub-nav li').removeClass('current');
    //    $(this).parent().addClass('current');
    //    var url = $(this).attr('href');
    //    $('.content-products-sub .colum-box').css({ 'width': Xwidth, 'min-height': Yheight });
    //    var allcolum = $('.content-products-sub .colum-box').length;
    //    var widthcolum = $('.content-products-sub .colum-box').width();
    //    $('.content-products-sub').width(allcolum * widthcolum);
    //    var XCurrent = $('.content-products-sub').offset().left;
    //    var ShowColum = $(this).attr('data-id');
    //    $('.content-products-sub .colum-box').removeClass('active');
    //    var XColum = $('.content-products-sub .colum-box[data-id= "' + ShowColum + '"]').addClass('active').offset().left;
    //    $('.content-products-sub').stop().animate({ 'left': XCurrent - XColum }, 800, 'easeInOutExpo');
    //    var url = $(this).attr('href');
    //    var title = $(this).attr('data-title');
    //    changeUrl(url, title, '', '', '', '', '');

    //    slideproduct();

    //    if ($('.box-products .slide-item').length <= 3) {
    //        $('.box-products .slide-controls').fadeOut();
    //    }

    //});//sub-nav


    //if ($('.colum-box').length == 1) {
    //    $('.colum-box').addClass('active');
    //    slideproduct();
    //}
    // }

    //productdetail
    //if ($('#productdetail-pages').length) {
    //    $('.navigation ul li:nth-child(3)').addClass('current');
    //    $('.sub-nav-top').css({ 'display': 'none' });
    //    //tabs
    //    // setTimeout(function () { $('.product-tab li:first-child').find('a').trigger('click'); }, 500);
    //    $('.product-tab li a').click(function (e) {
    //        e.preventDefault();
    //        $('.product-tab li').removeClass('current');
    //        $(this).parent().addClass('current');
    //        $('.scroll-tab').css({ 'display': 'none' });
    //        var tabselect = $(this).attr('data-tab');
    //        $('.product-tab-detail .scroll-tab[data-tab= "' + tabselect + '"]').addClass('active').fadeIn(300);
    //        scrollTab();
    //    });

    //    //load pics
    //    //$('.view-album').click(function (e) {
    //    //    e.preventDefault();
    //    //    var id = $(this).attr('data-id');
    //    //    $('body').append('<div class="loadicon" style="display:block"></div>');

    //    //    LoadPicProduct(id);



    //    //});

    //    //  LoadSlide.Solutions();

    //}

    //solution-pages
    //if ($('#solutions-pages').length) {
    //    $('.navigation ul li:nth-child(4)').addClass('current');
    //    $('.box-solutions').each(function (i) {
    //        var item = $(this);
    //        // setTimeout(function () { $(item).addClass('fadeinup'); scrollA(); }, (i + 3) * 220);

    //    });
    //    LoadSlide.Solutions();
    //}

    //service-pages
    //if ($('#service-pages').length) {
    //    $('.navigation ul li:nth-child(5)').addClass('current');
    //    $('.box-service').each(function (i) {
    //        var item = $(this);
    //        // setTimeout(function () { $(item).addClass('fadeinup'); scrollA(); }, (i + 3) * 220);

    //    });

    //    LoadSlide.Service();
    //}
    //if ($('#servicedetail-pages').length) {
    //    $('.navigation ul li:nth-child(5)').addClass('current');
    //    LoadSlide.News();
    //    $('.link-news a').click(function (e) {
    //        e.preventDefault();
    //        $('.text-about').addClass('hide');
    //        $('.link-news a').removeClass('current');
    //        $(this).addClass('current');
    //        var id = $(this).attr('data-id');
    //        var url = $(this).attr('href');
    //        var title = $(this).attr('data-title');
    //        changeUrl(url, title, '', '', '', '', '');
    //        $('.text-about').removeAttr('style');
    //        $('.top-pages h1,.CMSBreadCrumbsCurrentItem').html(title);
    //        setTimeout(function () { LoadServiceDetail(id); }, 500);

    //    });

    //    setTimeout(function () {
    //        if ($('.link-news a.current').length) {
    //            //   $('.link-news a.current').trigger('click');
    //        }
    //        else {
    //            $('.slide-item:first-child .link-news a').trigger('click');
    //        }
    //    }, 500);
    //}

    //news-pages
    //if ($('#news-pages').length) {
    //    $('.navigation ul li:nth-child(6)').addClass('current');

    //    LoadSlide.News();
    //    $('.link-news a').click(function (e) {
    //        e.preventDefault();
    //        $('.link-news a').removeClass('current');
    //        $(this).addClass('current');
    //        var id = $(this).attr('data-id');
    //        var url = $(this).attr('href');
    //        var title = $(this).attr('data-title');
    //        changeUrl(url, title, '', '', '', '', '');

    //        LoadNewsDetail(id);

    //    });

    //    setTimeout(function () {
    //        if ($('.link-news a.current').length) {
    //            $('.link-news a.current').trigger('click');
    //        }
    //        else {
    //            $('.slide-item:first-child .link-news a').trigger('click');
    //        }
    //    }, 500);
    //}
    //pagenews-pages
    //if ($('#pagenews-pages').length) {
    //    $('.navigation ul li:nth-child(6)').addClass('current');
    //    $('.top-pages a').click(function (e) {
    //        e.preventDefault();
    //        $('.top-pages a').removeClass('current');
    //        $(this).addClass('current');
    //        var id = $(this).attr('data-id');
    //        var url = $(this).attr('href');
    //        var title = $(this).attr('data-title');
    //        changeUrl(url, title, '', '', '', '', '');
    //        LoadNewsList(id);

    //    });

    //    setTimeout(function () {
    //        if ($('.top-pages a.current').length) {
    //            $('.top-pages a.current').trigger('click');
    //        }
    //        else {
    //            $('.top-pages a:first-child').trigger('click');
    //        }
    //    }, 500);
    //}
    //newsdetail
    //if ($('#newsdetail-pages').length) {
    //    $('.navigation ul li:nth-child(6)').addClass('current');
    //    $('.navigation ul li:nth-child(6)').addClass('current');

    //    LoadSlide.News();
    //    $('.link-news a').click(function (e) {
    //        e.preventDefault();
    //        $('.link-news a').removeClass('current');
    //        $(this).addClass('current');
    //        var id = $(this).attr('data-id');
    //        var url = $(this).attr('href');
    //        var title = $(this).attr('data-title');
    //        changeUrl(url, title, '', '', '', '', '');

    //        LoadNewsDetail(id);

    //    });

    //    setTimeout(function () {
    //        if ($('.link-news a.current').length) {
    //            $('.link-news a.current').trigger('click');
    //        }
    //        else {
    //            $('.slide-item:first-child .link-news a').trigger('click');
    //        }
    //    }, 500);
    //}

    //if ($('#products-pages').length) {
    //    $('.navigation ul li:nth-child(3)').addClass('current');

    //}

    //news-pages
    //if ($('#news-pages').length) {
    //    $('.navigation ul li:nth-child(4)').addClass('current');
    //    setTimeout(function () {
    //        if ($('.sub-nav li.current').length) {
    //            $('.sub-nav li.current a').trigger('click');
    //        }
    //        else {
    //            $('.sub-nav li:first-child a').trigger('click');
    //        }
    //    }, 500);

    //    $('.sub-nav ul li a').click(function (e) {
    //        e.preventDefault();
    //        $('.sub-nav li').removeClass('current');
    //        $(this).parent().addClass('current');
    //        var url = $(this).attr('href');
    //        $('.content-news .colum-box').css({ 'width': Xwidth, 'min-height': Yheight });
    //        var allcolum = $('.content-news .colum-box').length;
    //        var widthcolum = $('.content-news .colum-box').width();
    //        $('.content-news').width(allcolum * widthcolum);
    //        var XCurrent = $('.content-news').offset().left;
    //        var ShowColum = $(this).attr('data-id');
    //        $('.content-news .colum-box').removeClass('active');
    //        var XColum = $('.content-news .colum-box[data-id= "' + ShowColum + '"]').addClass('active').offset().left;
    //        $('.content-news').stop().animate({ 'left': XCurrent - XColum }, 800, 'easeInOutExpo');
    //        var url = $(this).attr('href');
    //        var title = $(this).attr('data-title');
    //        changeUrl(url, title, '', '', '', '', '');
    //        scrollA();
    //    });//sub-nav


    //}


    //news-page detail
    //if ($('#news-detail-pages').length) {
    //    $('.navigation ul li:nth-child(6)').addClass('current');
    //setTimeout(function () {
    //    if ($('.row-news.current').length) {
    //        $('.row-news.current .view-load').trigger('click');
    //    }
    //    else {
    //        $('.row-news:first-child a').trigger('click');
    //    }
    //}, 500);
    //$('.view-load').click(function (e) {
    //    e.preventDefault();
    //    $('.content-list .row-news').removeClass('current');
    //    $(this).parent().addClass('current');
    //    var id = $(this).attr('data-id');
    //    var url = $(this).attr('href');
    //    var title = $(this).attr('data-title');
    //    changeUrl(url, title, '', '', '', '', '');
    //    LoadNewsDetail(id);

    //});
    // }

    //recruitment-pages
    //if ($('#recruitment-pages').length) {
    //    $('.navigation ul li:nth-child(7)').addClass('current');
    //    setTimeout(function () {
    //        if ($('.list-recruitment a.current').length) {
    //            $('.list-recruitment a.current').trigger('click');
    //        }
    //        else {
    //            $('.list-recruitment a:first-child').trigger('click');
    //        }
    //    }, 500);
    //    $('.list-recruitment a').click(function (e) {
    //        e.preventDefault();
    //        $('.list-recruitment a').removeClass('current');
    //        $(this).addClass('current');
    //        var id = $(this).attr('data-id');
    //        var url = $(this).attr('href');
    //        var title = $(this).attr('data-title');
    //        changeUrl(url, title, '', '', '', '', '');
    //        $('.CMSBreadCrumbsCurrentItem').html(title);
    //        LoadRecruitmentDetail(id, title);

    //    });

    //    //scrollA();
    //}

    //customer-pages
    //if ($('#customer-pages').length) {
    //    $('.navigation ul li:nth-child(5)').addClass('current');

    //    $('.item-customer').each(function (i) {
    //        var item = $(this);
    //        setTimeout(function () { $(item).addClass('fadeinup'); scrollA(); }, (i + 3) * 220);

    //    });
    //}

    //gallery-pages
    //if ($('#gallery-pages').length) {
    //    $('.nav-top ul li:nth-child(1)').addClass('current');
    //    setTimeout(function () { $('.content-album').addClass('fadeinup'); }, 500);
    //    setTimeout(function () { $('.content-video').addClass('fadeinup'); }, 800);

    //    $('.playvideo').click(function (e) {
    //        e.preventDefault();
    //        var src = $(this).attr('data-video');
    //        LoadVideo(src);
    //    });

    //    $('.playalbum').click(function (e) {
    //        e.preventDefault();
    //        var id = $(this).attr('data-album');
    //        var title = $(this).attr('data-title');
    //        LoadAlbum(id, title);
    //    });
    //}

    //policies-pages
    //if ($('#policies-pages').length) {
    //    setTimeout(function () {
    //        if ($('.policies.sub-nav li.current').length) {
    //            $('.policies.sub-nav li.current a').trigger('click');
    //        }
    //        else {
    //            $('.policies.sub-nav li:first-child a').trigger('click');
    //        }
    //    }, 500);
    //    $('.policies.sub-nav li a').click(function (e) {
    //        e.preventDefault();
    //        $('.policies.sub-nav li').removeClass('current');
    //        $(this).parent().addClass('current');
    //        var id = $(this).attr('data-id');
    //        var url = $(this).attr('href');
    //        var title = $(this).attr('data-title');
    //        changeUrl(url, title, '', '', '', '', '');
    //        LoadDetailPolicies(id);

    //    });

    //    scrollA();
    //}

    ///contact-pages
    if ($('#contact-pages').length) {
        $('.navigation ul li:nth-child(8)').addClass('current');
        $('.googlemap').css({ 'width': Xwidth, 'height': Yheight });
        $('#map-canvas').css({ 'width': Xwidth + 600, 'height': Yheight });
        setTimeout(function () { googlemap(); }, 500);

    }
}

function slideproduct() {
    //slide
    var slidepro = $('.colum-box.active .box-products');
    slidepro.owlCarousel({
        nav: true,
        lazyLoad: true,
        responsive: {
            960: {
                items: 3
            },
            maxWidth: {
                items: 3
            }
        },
        animateOut: 'slideOutLeft',
        animateIn: 'flipInX',
        items: 3,
    });

    slidepro.on('initialized.owl.carousel changed.owl.carousel', function (e) {
        if (!e.namespace || e.type != 'initialized' && e.property.name != 'position') return
        $('.slide-number').text(e.item.index + 1 + '/' + e.page.size);
    })

    slidepro.on('mousewheel', '.slide-stage', function (e) {
        if (e.deltaY > 0) {

            $('.colum-box.active .slide-prev').trigger('click');
        } else {
            $('.colum-box.active .slide-next').trigger('click');

        }
        e.preventDefault();
    });
    //slide
}
function play() {




    $('.slogan-top').children().each(function (i) {
        var text = $(this);
        timex = setTimeout(function () { $(text).addClass('showtext') }, (i + 1) * 150);

    });
    $('.slogan-top span').children().each(function (i) {
        var text = $(this);
        timex = setTimeout(function () { $(text).addClass('showtext') }, (i + 1) * 150);

    });

}
function LayOut() {
    $('.over-load').fadeOut();
    $('.container').addClass('show');
    $('.container,.slider-home').css({ 'opacity': 1 });
    $('.support-icon').addClass('show');
    $('.footer').css({ 'opacity': 1 });
    $(document).bind('keydown', function (e) {
        if (e.which === 27) {
            $('.video-play iframe').attr('src', '');
            $('.close-video').trigger('click');
            $('.close-support').trigger('click');
            $('.close-products-hot').trigger('click');
            $('.close-album').trigger('click');
        }
        if (event.keyCode == 8) {
            $('.btn-back a').trigger('click');
        }
    });
    NavClick();
    //all pages
    var img = $('.bg-home img');
    var Xwidth = $(window).width();
    var Yheight = $(window).height();
    var RatioScreeen = Yheight / Xwidth;
    var RatioIMG = 425 / 1440;
    var RatioBanner = 425 / 1440;
    var newXwidth;
    var newYheight;
    if (RatioScreeen > RatioIMG) {
        newYheight = Yheight;
        newXwidth = Yheight / RatioIMG;
    } else {
        newYheight = Xwidth * RatioIMG;
        newXwidth = Xwidth;

    }

    //set



    Search();
    //scrollBody();
    scrollN();
    scrollTab();
    scrollA();

    //if (Xwidth > maxWidth) {
    //    $(img).css({ 'width': newXwidth, 'height': newYheight, 'left': (Xwidth - newXwidth) / 2, 'top': 'auto', 'bottom': 0 });
    //    $('.bg-home').css({ 'width': Xwidth, 'height': Yheight });
    //    $('.slide-bg, .slider-home').css({ 'width': '100%', 'height': Yheight });
    //    $(img).css({ 'width': Xwidth, 'height': (Xwidth * RatioIMG), 'left': 0, 'top': 0, 'bottom': 'auto' });
    //    $('.bg-home').css({ 'width': Xwidth, 'height': (Xwidth * RatioIMG) });
    //    $('.slide-bg, .slider-home').css({ 'width': '100%', 'height': (Xwidth * RatioIMG) });
    //} else {
    //    if (Xwidth <= 620) {
    //        $(img).css({ 'width': Xwidth + 300, 'height': (Xwidth + 300) * RatioIMG, 'left': -150, 'top': 0, 'bottom': 'auto' });
    //        $('.bg-home').css({ 'width': Xwidth, 'height': (Xwidth + 300) * RatioIMG });
    //        $('.slide-bg, .slider-home').css({ 'width': '100%', 'height': (Xwidth + 300) * RatioIMG });
    //    } else {
    //        $(img).css({ 'width': Xwidth + 300, 'height': (Xwidth + 300) * RatioIMG, 'left': -150, 'top': 0, 'bottom': 'auto' });
    //        $('.bg-home').css({ 'width': Xwidth, 'height': (Xwidth + 300) * RatioIMG });
    //        $('.slide-bg, .slider-home').css({ 'width': '100%', 'height': (Xwidth + 300) * RatioIMG });
    //    }
    //}

    $('.title-pages').fadeIn(600);
    //if ($('#currenturl').html() == '/San-pham') {
    //    window.location = $('.sub-nav-top li:first-child a').attr('href');
    //}
    setTimeout(function () { LoadSlide.FullBackground(); }, 100);
    //$('.container').animate({ 'opacity': 1 }, 200, 'linear', function () {
    //    $('.hidecontent').fadeOut(600, 'linear');
    //    if (!$('#home-pages').length) {
    //        $('.certificate').fadeOut(300);
    //        $('.video-home').fadeOut(300);
    //        $('.box-support').addClass('hide');
    //        $('.box-support').hover(function () {
    //            $(this).removeClass('hide');
    //        },
    //        function () {
    //            $(this).addClass('hide');
    //        });
    //        $('.box-support').delay(300).fadeIn(300).animate({ 'opacity': 1 }, 500, 'linear', function () { });
    //    }
    //    else {
    //        $('.over-dark').remove();
    //        $('.box-news-home').addClass('fadeinup');
    //        $('.certificate').delay(800).fadeIn(500);
    //        $('.video-home').delay(800).fadeIn(500);
    //        $('.box-support').fadeIn(500).removeClass('hide').animate({ 'opacity': 1 }, 500, 'linear');
    //    }
    //});

    //all pages
    $('.colum-box,.content-products,.about-products').css({ 'width': Xwidth, 'min-height': Yheight });
    $('.no-img').parent().css({ 'line-height': 0 });
    $('.support a').click(function (e) {
        e.preventDefault();
        scrollTab();
        $('.box-support').addClass('hide');
        $('.content-support').fadeIn(100, function () {
            $(this).animate({ 'opacity': 1, 'right': 0 }, 900, 'easeInOutExpo', function () {
                $('.close-support').fadeIn(600);

            });

        });
    });
    setTimeout(function () {
        $('.logo-m').click(function () {
            $('.link-home a').trigger('click');
        });
    }, 2200);


    $('.support-icon').hover(function () {
        $(this).css({ 'height': 65 });
    },
    function () {
        $(this).removeAttr('style');
    }
    );

    $('.support-icon').click(function (e) {
        e.preventDefault();
        $('.support-icon').removeClass('show');
        $('.support-icon').animate({}, 0, 'linear', function () {
            $('.box-support').addClass('hide');
            $('.content-support').fadeIn(100, function () {
                $('.content-support').animate({ 'opacity': 1, 'right': 0 }, 600, 'easeInOutQuart', function () {
                    $('.close-support').addClass('show');
                    scrollTab();
                });

            });
        })
    });
    //support
    $('.close-support').click(function (e) {

        $('.content-support').animate({ 'right': '-50%', 'opacity': 0 }, 600, 'easeInOutQuart', function () {
            $(this).fadeOut(100, 'linear', function () {
                $('.support-icon').addClass('show');
            });
            $('.close-support').removeClass('show');

        });

    });

    setTimeout(function () { $('hideicon').children().remove(); }, 500);

    $('.gotop').click(function () {
        $('html, body').stop().animate({ scrollTop: 0 }, 'slow');
    });


    if (Xwidth <= 1000) {
        $('.logo-m').fadeIn(300);
        $('.navigation ul li a').bind('click', function () {
            if ($(this).parent().hasClass('active')) {
                $(this).parent().find('ul.sub-nav').fadeOut(300);
                $(this).parent().removeClass('active');
            }
            else {
                var HeightSub = $(this).find('ul.sub-nav').innerHeight();
                $('ul.sub-nav').fadeOut(300);
                $(this).parent().find('ul.sub-nav').fadeIn(300).css({ 'height': HeightSub });
                $(this).parent().addClass('active');
            }


        });

        $('.lang').bind('click', function () {
            if ($(this).hasClass('active')) {
                $(this).find('.box-lang').fadeOut(300);
                $(this).removeClass('active');
            }
            else {

                $(this).find('.box-lang').fadeIn(300);
                $(this).addClass('active');
            }


        });
    }
    else {


        $('.navigation ul li').hover(function () {
            //   $(this).find('ul.sub-nav').fadeOut(300, 'linear', function () {
            var dataID = $(this).attr('data-id');
            var H1 = $('.all-sub[data-sub= "' + dataID + '"]').children().innerHeight();
            $(this).find('.all-sub').css({ 'height': H1 + 10 });
            if ($(this).find('.sub-nav-2').children().length) {
                $(this).find('.all-sub').fadeIn('fast').css({ 'width': 700 });
            }
            // alert($(this).children().height());
            //   });

        },
            function () {

                $(this).find('.all-sub').css({ 'height': '0', 'display': 'none' });
            }
        );


        $('ul.sub-nav li').hover(function () {
            // $(this).find('ul.sub-nav-2').fadeOut(300, 'linear', function () {
            $(this).find('ul.sub-nav-2').css({ 'display': 'block' });
            // });
        },
            function () {
                $(this).find('ul.sub-nav-2').css({ 'display': 'none' });
            }
        );

        //lang
        $('.list-lang').bind('click', function () {
            if ($(this).parent().hasClass('active')) {
                $(this).parent().find('.box-lang').fadeOut(300);
                $(this).parent().removeClass('active');

            }
            else {

                $(this).parent().find('.box-lang').fadeIn(300);
                $(this).parent().addClass('active');
            }


        });

    }
    //fix mobile
    if (Xwidth <= maxWidth) {

        $('.nav-open').css({ 'opacity': 1, 'display': 'block' });
        $('.logo').fadeOut();
    }
    else {
        $('.logo').fadeIn();
        $('.nav-open').css({ 'opacity': 0, 'display': 'none' }).removeClass('active');
        $('.header').removeClass('show');
        $('.container').removeClass('show');
        $('.nav-open').removeClass('active');
    }
}
function NavClick() {
    $('.nav-open').bind('click', function () {
        if ($(window).width() <= maxWidth) {
            if ($(this).hasClass('active')) {
                $('.header').removeClass('show');
                $('.container').removeClass('show');
                $('.nav-open').removeClass('active');
                $('body').removeClass('no-scroll');
                $('html').removeClass('no-scroll');
                $('.overlay-menu').fadeOut();
            }
            else {
                $('.header').addClass('show');
                $('.container').addClass('show');
                $('.nav-open').fadeIn(300).addClass('active');
                $('body').addClass('no-scroll');
                $('html').addClass('no-scroll');
                $('.overlay-menu').fadeIn();
            }
        }
    })
}
$(document).bind('scroll', function () {
    var scroll = $(document).scrollTop();
    // if ($(window).width() >= maxWidth) {
    if (scroll > 50) {
        $('.gotop').fadeIn(500, 'linear');
    } else {
        $('.gotop').fadeOut(500, 'linear');
    }
    // }
});
$(document).mousemove(function (e) {
    var yMouse = e.pageY;
    var xMouse = e.pageX;
    if (xMouse >= $(window).width() - 30) {
        $('.nicescroll-rails').css({ 'width': 12, 'background-color': 'rgba(0,0,0,0.5)' });
        $('.nicescroll-rails').find('div').css({ 'width': 12, 'background-color': 'rgba(255,255,255,0.5)' });
    } else {
        $('.nicescroll-rails').css({ 'width': 6, 'background-color': 'rgba(0,0,0,0.3)' });
        $('.nicescroll-rails').find('div').css({ 'width': 6, 'background-color': 'rgba(255,255,255,0.3)' });
    }
    return false;
});



//project


$(function () {
    $('.rows-project').on('click', function (e) {
        var id = $(this).data('id');
        scrollA();
        e.preventDefault();
        $('body').append('<div class="loadicon" style="display:block"></div>');
        $('body').append('<div class="content-project"></div>');
        $('.over-dark').fadeIn();
        $('body').addClass('no-scroll');
        $.ajax({
            url: '/load/project.aspx?id=' + id, cache: false, success: function (data) {
                $('.content-project').html(data).fadeIn();
                $('.content-project').animate({ 'opacity': 1 }, 400, 'easeOutBounce', function () {

                    $('.loadicon').remove();
                });

                scrollA();

                $('.close').on('click', function (e) {
                    $('.content-project').fadeOut().animate({ 'opacity': 0 }, 400, 'easeOutBounce', function () {
                        $('.content-project').remove();
                        $('.over-dark').fadeOut();

                        $('body').removeClass('no-scroll');
                    });
                });

                $('.over-dark').on('click', function () {
                    $('.close').trigger('click');
                });
            }
        });
    });
});

//check menu left
//var url = window.location.href;
//$('ul.menu-left li').each(function () {
//    alert($(this).find('a').attr('href'));
//    if (url.indexOf((this).children('a').attr('href')) != -1) {
//        $(this).addClass('current');
//    }
//});