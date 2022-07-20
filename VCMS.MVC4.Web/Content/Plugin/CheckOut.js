$(window).load(function () {
    var _this = $("#check-out");
    $.ajax({
        url: _this.data("submit"),
        type: "POST",
        dataType: "json",
        success: function (e) {
            if (e.Status == 0) {
                $.ajax({
                    url: _this.data("customer"),
                    dataType: 'html',
                    success: function (html) {
                        $("#collapse-payment-address .panel-body").html(html);
                        $('#collapse-checkout-option').parent().find('.panel-heading').addClass("ok");
                        $('#collapse-checkout-option').parent().find('.panel-heading .panel-title').html('<span class="text">Đăng nhập</span><i class="fa fa-user"></i><span class="bar"><span></span></span>');

                        $('#collapse-payment-address').parent().find('.panel-heading').addClass("active");
                        $('#collapse-payment-address').parent().find('.panel-heading .panel-title').html('<a href="#collapse-payment-address" data-toggle="collapse" data-parent="#accordion" class="accordion-toggle"><span class="text">Thông tin khách hàng</span><i class="fa fa-newspaper-o"></i><span class="bar"><span></span></span></a>');
                        $('a[href=\'#collapse-payment-address\']').trigger('click');
                        return false;
                    }
                });
            }
            else {
                $.ajax({
                    url: _this.data("login"),
                    dataType: 'html',
                    success: function (html) {
                        $("#collapse-checkout-option .panel-body").html(html);
                        $('#collapse-checkout-option').parent().find('.panel-heading').addClass("active");
                        $('#collapse-checkout-option').parent().find('.panel-heading .panel-title').html('<a href="#collapse-checkout-option" data-toggle="collapse" data-parent="#accordion" class="accordion-toggle"><span class="text">Đăng nhập</span><i class="fa fa-user"></i><span class="bar"><span></span></span></a>');
                        $('a[href=\'#collapse-checkout-option\']').trigger('click');
                        return false;
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
                    }
                });
            }
            return false;
        }
    });
});

//$(document).on('change', 'input[name=\'checkregister\']', function () {
//    if ($('#collapse-payment-address').parent().find('.panel-heading .panel-title > *').is('a')) {
//        if (this.value == '0') {
//            $('#collapse-payment-address').parent().find('.panel-heading .panel-title').html('<a href="#collapse-payment-address" data-toggle="collapse" data-parent="#accordion" class="accordion-toggle">Bước 2: Đăng ký tài khoản <i class="fa fa-caret-down"></i></a>');
//        } else {
//            $('#collapse-payment-address').parent().find('.panel-heading .panel-title').html('<a href="#collapse-payment-address" data-toggle="collapse" data-parent="#accordion" class="accordion-toggle">Bước 2: Thông tin khách hàng <i class="fa fa-caret-down"></i></a>');
//        }
//    }
//    else {
//        if (this.value == '0') {
//            $('#collapse-payment-address').parent().find('.panel-heading .panel-title .text').html('Đăng ký tài khoản');
//        } else {
//            $('#collapse-payment-address').parent().find('.panel-heading .panel-title .text').html('Thông tin khách hàng');
//        }
//    }
//});

// Checkout
$(document).delegate('#button-account', 'click', function () {
    var _this = $(this);
    var status = $('input[name=\'checkregister\']:checked').val();
    var url = status == 0 ? _this.data("register") : _this.data("guest");

    $.ajax({
        url: url,
        dataType: 'html',
        beforeSend: function () {
            _this.button('loading');
        },
        complete: function () {
            _this.button('reset');
        },
        success: function (html) {
            $('#collapse-payment-address .panel-body').html(html);
            $('#collapse-checkout-option').parent().find('.panel-heading').addClass("ok");
            $('#collapse-payment-address').parent().find('.panel-heading').addClass("active");

            $('#collapse-checkout-option').parent().find('.panel-heading').removeClass("active");
            if ($('input[name=\'checkregister\']:checked').val() == 'register') {
                $('#collapse-payment-address').parent().find('.panel-heading .panel-title').html('<a href="#collapse-payment-address" data-toggle="collapse" data-parent="#accordion" class="accordion-toggle"><span class="text">Đăng ký tài khoản</span><i class="fa fa-newspaper-o"></i><span class="bar"><span></span></span></a>');
            } else {
                $('#collapse-payment-address').parent().find('.panel-heading .panel-title').html('<a href="#collapse-payment-address" data-toggle="collapse" data-parent="#accordion" class="accordion-toggle"><span class="text">Thông tin khách hàng</span><i class="fa fa-newspaper-o"></i><span class="bar"><span></span></span></a>');
            }

            $('a[href=\'#collapse-payment-address\']').trigger('click');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
        }
    });
});

// Login
$(document).delegate('#button-login', 'click', function () {
    var _this = $(this);
    $.ajax({
        url: _this.data("submit"),
        type: 'post',
        data: $('#check-out form').serialize(),
        dataType: 'json',
        beforeSend: function () {
            _this.button('loading');
        },
        complete: function () {
            _this.button('reset');
        },
        success: function (json) {
            $('.alert, .text-danger').remove();
            $('.form-group').removeClass('has-error');
            if (json['redirect']) {
                location = json['redirect'];
            } else if (json['error']) {
                if (json['error']['warning'])
                    $('#collapse-checkout-option .panel-body').prepend('<div class="alert alert-danger"><i class="fa fa-exclamation-circle"></i> ' + json['error']['warning'] + '<button type="button" class="close" data-dismiss="alert">&times;</button></div>');

                $('input[name=\'UserName\']').parent().addClass('has-error');
                $('input[name=\'Password\']').parent().addClass('has-error');

                for (i in json['error']) {
                    var element = $('#' + i);

                    if ($(element).parent().hasClass('form-group')) {
                        $(element).parent().append('<div class="text-danger">' + json['error'][i] + '</div>');
                    } else {
                        $(element).after('<div class="text-danger">' + json['error'][i] + '</div>');
                    }
                }
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

// Register
$(document).delegate('#button-register', 'click', function () {
    var _this = $(this);
    $.ajax({
        url: _this.data("submit"),
        type: 'post',
        data: $('#check-out form').serialize(),
        dataType: 'json',
        beforeSend: function () {
            _this.button('loading');
        },
        complete: function () {
            _this.button('reset');
        },
        success: function (json) {
            $('.alert, .text-danger').remove();
            $('.form-group').removeClass('has-error');
            if (json['redirect']) {
                location = json['redirect'];
            } else if (json['error']) {
                if (json['error']['warning']) {
                    $('#collapse-payment-address .panel-body').prepend('<div class="alert alert-danger"><i class="fa fa-exclamation-circle"></i> ' + json['error']['warning'] + '<button type="button" class="close" data-dismiss="alert">&times;</button></div>');
                }
                for (i in json['error']) {
                    var element = $('.register #' + i);
                    if ($(element).parent().hasClass('form-group')) {
                        $(element).parent().append('<div class="text-danger">' + json['error'][i] + '</div>');
                    } else {
                        $(element).after('<div class="text-danger">' + json['error'][i] + '</div>');
                    }
                }
                $('.text-danger').parent().addClass('has-error');
            }
            return false;
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
        }
    });
    return false;
});

// Guest
$(document).delegate('#button-guest', 'click', function () {
    var _this = $(this);
    $.ajax({
        url: _this.data("submit"),
        type: 'post',
        data: $('#check-out form').serialize(),
        dataType: 'json',
        beforeSend: function () {
            _this.button('loading');
        },
        complete: function () {
            _this.button('reset');
        },
        success: function (json) {
            $('.alert, .text-danger').remove();
            $('.form-group').removeClass('has-error');
            if (json['redirect']) {
                location = json['redirect'];
            } else if (json['error']) {
                if (json['error']['warning']) {
                    $('#collapse-payment-address .panel-body').prepend('<div class="alert alert-warning">' + json['error']['warning'] + '<button type="button" class="close" data-dismiss="alert">&times;</button></div>');
                }

                for (i in json['error']) {
                    var element = $('#' + i);

                    if ($(element).parent().hasClass('input-group')) {
                        $(element).parent().append('<div class="text-danger">' + json['error'][i] + '</div>');
                    } else {
                        $(element).after('<div class="text-danger">' + json['error'][i] + '</div>');
                    }
                }

                $('.text-danger').parent().addClass('has-error');
            } else if (json['done']) {
                $('#collapse-payment-address').parent().find('.panel-heading').addClass("ok").removeClass("active");
                $('#collapse-checkout-note').parent().find('.panel-heading').addClass("active");

                $('#collapse-shipping-address').parent().find('.panel-heading .panel-title').html('<a href="#collapse-shipping-address" data-toggle="collapse" data-parent="#accordion" class="accordion-toggle"><span class="text">Thông tin khách hàng</span><i class="fa fa-newspaper-o"></i><span class="bar"><span></span></span></a>');

                $('a[href=\'#collapse-shipping-address\']').trigger('click');

                $('#collapse-checkout-note').parent().find('.panel-heading .panel-title').html('<a href="#collapse-checkout-note" data-toggle="collapse" data-parent="#accordion" class="accordion-toggle"><span class="text">Thời gian giao hàng</span><i class="fa fa-calendar"></i><span class="bar"><span></span></span></a>');

                $('a[href=\'#collapse-checkout-note\']').trigger('click');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
        }
    });
    return false;
});

// Payment Address
$(document).delegate('#button-payment-address', 'click', function () {
    var _this = $(this);
    $.ajax({
        url: _this.data("submit"),
        type: 'post',
        data: $('#check-out form').serialize(),
        dataType: 'json',
        beforeSend: function () {
            _this.button('loading');
        },
        complete: function () {
            _this.button('reset');
        },
        success: function (json) {
            $('.alert, .text-danger').remove();
            $('.form-group').removeClass('has-error');

            if (json['error']) {
                if (json['error']['warning']) {
                    $('#collapse-payment-address .panel-body').prepend('<div class="alert alert-warning">' + json['error']['warning'] + '<button type="button" class="close" data-dismiss="alert">&times;</button></div>');
                }

                for (i in json['error']) {
                    var element = $('#' + i);

                    if ($(element).parent().hasClass('form-group')) {
                        $(element).parent().append('<div class="text-danger">' + json['error'][i] + '</div>');
                    } else {
                        $(element).after('<div class="text-danger">' + json['error'][i] + '</div>');
                    }
                }
                $('.text-danger').parent().addClass('has-error');
            } else if (json.Status == 1) {
                $('#collapse-checkout-note').parent().find('.panel-heading').addClass("active");
                $('#collapse-payment-address').parent().find('.panel-heading').removeClass("active").addClass("ok");
                

                $('#collapse-shipping-address').parent().find('.panel-heading .panel-title').html('<a href="#collapse-shipping-address" data-toggle="collapse" data-parent="#accordion" class="accordion-toggle"><span class="text">Thông tin khách hàng</span><i class="fa fa-newspaper-o"></i><span class="bar"><span></span></span></a>');

                $('a[href=\'#collapse-shipping-address\']').trigger('click');

                $('#collapse-checkout-note').parent().find('.panel-heading .panel-title').html('<a href="#collapse-checkout-note" data-toggle="collapse" data-parent="#accordion" class="accordion-toggle"><span class="text">Thời gian giao hàng</span><i class="fa fa-calendar"></i><span class="bar"><span></span></span></a>');

                $('a[href=\'#collapse-checkout-note\']').trigger('click');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
        }
    });
    return false;
});

// Checkout note
$(document).delegate('#button-checkout-note', 'click', function () {
    var _this = $(this);
    $.ajax({
        url: _this.data("submit"),
        type: 'post',
        data: $('#check-out form').serialize(),
        dataType: 'json',
        beforeSend: function () {
            _this.button('loading');
        },
        complete: function () {
            _this.button('reset');
        },
        success: function (json) {
            $('.alert, .text-danger').remove();
            $('.form-group').removeClass('has-error');
            if (json['redirect']) {
                location = json['redirect'];
            } else if (json['error']) {
                if (json['error']['warning']) {
                    $('#collapse-checkout-note .panel-body').prepend('<div class="alert alert-danger">' + json['error']['warning'] + '<button type="button" class="close" data-dismiss="alert">&times;</button></div>');
                }
                $('input[name=\'DateDelivery\']').parent().addClass('has-error');
                $('.text-danger').parent().parent().addClass('has-error');
            }
            else if (json['done']) {
                //var url = json['done']['Value'] == 0 ? _this.data("success") : _this.data("success-none-shipping");
                $.ajax({
                    url: _this.data("success"),
                    dataType: 'html',
                    success: function (html) {
                        $('#collapse-confirm-cart').prepend('<input name="ShippingPrice" value="' + json['done']['Value'] + '" type="hidden" />');
                        $('#collapse-confirm-cart').prepend('<input name="TotalPrice" value="' + json['done']['Total'] + '" type="hidden" />');


                        $('#collapse-confirm-cart .panel-body').html(html);


                        $('#collapse-checkout-note').parent().find('.panel-heading').addClass("ok").removeClass("active");
                        $('#collapse-confirm-cart').parent().find('.panel-heading').addClass("active");

                        $('#collapse-confirm-cart').parent().find('.panel-heading .panel-title').html('<a href="#collapse-confirm-cart" data-toggle="collapse" data-parent="#accordion" class="accordion-toggle"><span class="text">Xem lại đơn hàng</span><i class="fa fa-cube"></i><span class="bar"><span></span></span></a>');

                        $('a[href=\'#collapse-confirm-cart\']').trigger('click');
                        $('a[href=\'#collapse-checkout-note\']').trigger('click');
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
                    }
                });

            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
        }
    });
    return false;
});

$(document).delegate('#button-checkout', 'click', function () {
    var _this = $(this);
    $.ajax({
        url: _this.data("submit"),
        type: 'post',
        data: $('#check-out form').serialize(),
        dataType: 'json',
        beforeSend: function () {
            $('#button-checkout').button('loading');
        },
        complete: function () {
            $('#button-checkout').button('reset');
        },
        success: function (json) {
            $('.alert, .text-danger').remove();
            $('.form-group').removeClass('has-error');
            if (json['redirect']) {
                location = json['redirect'];
            } else if (json['error']) {
                if (json['error']['warning']) {
                    $('#collapse-payment-method .panel-body').prepend('<div class="alert alert-warning">' + json['error']['warning'] + '<button type="button" class="close" data-dismiss="alert">&times;</button></div>');
                }
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
        }
    });
    return false;
});

$(document).delegate('#button-reorder', 'click', function () {
    var _this = $(this);
    $.ajax({
        type: 'post',
        url: _this.data("submit"),
        dataType: 'json',
        beforeSend: function () {
            $('#sys-notification').animate({ "opacity": "0" }, 25).html("");
            $(".imgload").removeClass("hidden");
        },
        complete: function () {
            $(".imgload").addClass("hidden");
        },
        success: function (json) {
            if (json['error']) {
                if (json['error']['warning'])
                    $('#sys-notification').animate({ "opacity": "1", "margin-top": "0" }, 25).prepend('<div class="alert alert-danger"><i class="fa fa-exclamation-circle"></i> ' + json['error']['warning'] + '<button type="button" class="close" data-dismiss="alert">&times;</button></div>');
            }
            else if (json['done']) {
                if (json['done']['message'])
                    $('#sys-notification').animate({ "opacity": "1" }, 25).prepend('<div class="alert alert-success"><i class="fa fa-exclamation-circle"></i> ' + json['done']['message'] + '<button type="button" class="close" data-dismiss="alert">&times;</button></div>');
            }
            $("#shopcart-info,#shopcart-info-mobile").load(_this.data("load"));
            return false;
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            return false;
        }
    });
    return false;
});

$(document).delegate('#button-cancel', 'click', function () {
    var _this = $(this);
    $.ajax({
        type: 'post',
        url: _this.data("submit"),
        dataType: 'json',
        success: function (json) {
            if (json['error']) {
                if (json['error']['warning'])
                    $('#sys-notification').animate({ "opacity": "1", "margin-top": "0" }, 25).prepend('<div class="alert alert-danger"><i class="fa fa-exclamation-circle"></i> ' + json['error']['warning'] + '<button type="button" class="close" data-dismiss="alert">&times;</button></div>');
            } if (json['redirect']) {
                location = json['redirect'];
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
$(document).delegate('#button-payment', 'click', function () {
    var _this = $(this);
    $('#collapse-confirm-cart').parent().find('.panel-heading').addClass("ok").removeClass("active");
    $('#collapse-checkout-payment').parent().find('.panel-heading').addClass("active");

    $("#orderSummary #shipping-price").html($("input[name=\'ShippingPrice\']").val());

    $('#collapse-checkout-payment').parent().find('.panel-heading .panel-title').html('<a href="#collapse-checkout-payment" data-toggle="collapse" data-parent="#accordion" class="accordion-toggle"><span class="text">Thanh toán</span><i class="fa fa-paypal"></i><span class="bar"><span></span></span></a>');

    $('a[href=\'#collapse-checkout-payment\']').trigger('click');
    $('a[href=\'#collapse-confirm-cart\']').trigger('click');
    return false;
});

$(document).delegate('#shipping-time .slot:not(.disable)', 'click', function () {
    var _this = $(this);
    $("#shipping-time .slot").removeClass("active");
    _this.addClass("active");
    $("#DateDelivery").val(_this.data("date"));
    $("#ShippingTime").val(_this.data("id"));
    var str = FormattedDate(_this.data("date"), _this.text());
    $(".shipping-time-info").html(str);

    return false;
});
function FormattedDate(d, text) {
    var monthNames = [
        "Tháng 1", "Tháng 2", "Tháng 3",
        "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7",
        "Tháng 8", "Tháng 9", "Tháng 10",
        "Tháng 11", "Tháng 12"
    ];
    var date = new Date(d);
    var day = date.getDate();
    var monthIndex = date.getMonth();
    var year = date.getFullYear();
    var str = "Ngày " + day + " " + monthNames[monthIndex] + ", " + year + ". Khoảng thời gian " + text;
    return str;
}
