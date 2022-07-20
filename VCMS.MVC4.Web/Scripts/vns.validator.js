$.validator.addMethod("image", function (value, element) {
    if ($(element).attr('type') == "file" && ($(element).hasClass('required') || $(element).get(0).value.length > 0))
        return value.match(/\.(png|gif|jpg|jpeg|bmp|swf)$/i);
    else
        return true;

}, "Invalid image file type");
$.validator.addMethod("attach", function (value, element) {
    if ($(element).attr('type') == "file" && ($(element).hasClass('required') || $(element).get(0).value.length > 0))
        return value.match(/\.(png|gif|jpg|jpeg|bmp|zip|rar|doc|docx|xls|xlsx|ppt|pptx|pdf|txt|csv)$/i);
    else
        return true;

}, "Invalid attach file type");
$.validator.unobtrusive.adapters.addBool("image");
$.validator.unobtrusive.adapters.addBool("attach");