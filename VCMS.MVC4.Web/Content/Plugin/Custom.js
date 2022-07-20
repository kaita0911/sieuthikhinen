$(function () {
    var timer = null;
    /*
    * Add To Cart
    */
    $(".addToCart").on("click", function () {
        var _this = $(this);
        var url = _this.data("submit") != null ? _this.data("submit") : _this.attr("href");

        $.ajax({
            url: url,
            type: 'post',
            data: {
                id: _this.data("id"),
                num: 1
            },
            dataType: 'json',
            beforeSend: function () {
                _this.addClass("submit");
                clearTimeout(timer);
                $("#sys-notification").animate({ "opacity": "0","z-index":"1"}, 500);
               
            },
            complete: function () {
                _this.removeClass("submit");
            },
            success: function (json) {
                if (json.Status == 0) {
                    $("#number").html(json.Qyt);
                    $("#number-mobile").html(json.Qyt);
                    $("#totals").html(json.Amount);
                    $("#sys-notification").html('<div class="alert alert-success"><i class="fa fa-exclamation-circle"></i> ' + json.Message + '<button type="button" class="close">&times;</button></div>').animate({ "opacity": "1", "z-index": "9000" }, 500);
                    $("#sys-notification button.close").on("click", function () {
                        $("#sys-notification").animate({ "opacity": "0", "z-index": "1" }, 500);
                        
                    });
                    $("#shop-cart .content,#show-mobile .carts-mobile").load(_this.data("load"));
                    timer = setTimeout(function () {
                        $("#sys-notification").animate({ "opacity": "0", "z-index": "1" }, 500);
                       
                    }, 5000);
                }
                return false;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
                return false;
            }
        });
        return false;
    });

    /*
    * Delete item in cart
    */
    $(".mini-cart-info .delete").on("click", function () {
        var _this = $(this);
        var _parent = _this.parents("li.item");
        $.ajax({
            url: _this.data("submit"),
            type: 'post',
            data: {
                id: _this.data("id"),
            },
            dataType: 'json',
            success: function (json) {
                if (json.Status == 0) {
                    _parent.animate({ "opacity": "0" }, 300);
                    $(".mini-cart-info .price-total").html(json.Amount);
                    $("#number").html(json.Qty);
                    $("#number-mobile").html(json.Qyt);
                    $("#shop-cart .content,#show-mobile .carts-mobile").load(_this.data("load"));
                }
                return false;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
                return false;
            }
        });
        return false;
    });
    /*
    * Product Detail Add To Cart
    */
    $("#addcart").on("click", function () {
        var _this = $(this);
        var parent = _this.parents("#cart");
        var number = parent.find(".number-cart").val() != null ? parent.find(".number-cart").val() : 1;
        alert(number);
        $.ajax({
            url: _this.data("submit"),
            type: 'post',
            data: {
                id: _this.data("id"),
                num: number
            },
            dataType: 'json',
            beforeSend: function () {
                _this.addClass("submit");
                clearTimeout(timer);
                $("#sys-notification").animate({ "opacity": "0", "z-index": "1" }, 500);
            },
            complete: function () {
                _this.removeClass("submit");
            },
            success: function (json) {
                if (json.Status == 0) {
                    $("#number").html(json.Qyt);
                    $("#number-mobile").html(json.Qyt);
                    $("#totals").html(json.Amount);
                    $("#sys-notification").html('<div class="alert alert-success"><i class="fa fa-exclamation-circle"></i> ' + json.Message + '<button type="button" class="close">&times;</button></div>').animate({ "opacity": "1", "z-index": "9000" }, 500);
                    $("#sys-notification button.close").on("click", function () {
                        $("#sys-notification").animate({ "opacity": "0", "z-index": "1" }, 500);
                    });
                    $("#shop-cart .content,#show-mobile .carts-mobile").load(_this.data("load"));
                    timer = setTimeout(function () {
                        $("#sys-notification").animate({ "opacity": "0", "z-index": "1" }, 500);
                    }, 5000);
                }
                return false;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
                return false;
            }
        });

    });

    /*
    * Newsletter
    */
    $("#button-newsletter").on("click", function () {
        var _this = $(this);
        var email = $('input[name=\'email\']').val();
        $.ajax({
            type: 'post',
            url: _this.data("submit"),
            data: { email: email },
            dataType: 'json',
            beforeSend: function () {
                _this.button('loading');
                clearTimeout(timer);
                $("#sys-notification").animate({ "opacity": "0" }, 500);
            },
            complete: function () {
                _this.button('reset');
            },
            success: function (json) {
                if (json['error']) {
                    $("#sys-notification").html('<div class="alert alert-danger"><i class="fa fa-exclamation-circle"></i> ' + json['error']['warning'] + '<button type="button" class="close" data-dismiss="alert">&times;</button></div>').animate({ "opacity": "1" }, 800);
                } else if (json['done']) {
                    $("#sys-notification").html('<div class="alert alert-success"><i class="fa fa-exclamation-circle"></i> ' + json['done']['message'] + '<button type="button" class="close" data-dismiss="alert">&times;</button></div>').animate({ "opacity": "1" }, 800);
                }
                timer = setTimeout(function () {
                    $("#sys-notification").animate({ "opacity": "0" }, 500);
                }, 5000);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    });

    $("#sys-notification button.close").on("click", function () {
        $("#sys-notification").animate({ "opacity": "0" }, 500);
    });
});