$(function () {
    if (typeof ("CKEDITOR") != "undefined") {
        
        var fileman = "/Scripts/ckeditor/plugins/fileman/";
        $("textarea.fck-small").each(function () {
            var num = Math.round(Math.random() * 100);
            CKEDITOR.replace(this.id, {
                toolbar: 'Basic',
                filebrowserUploadUrl: "/Editor/ImageUpload?editorType=CKEDITOR" ,
                filebrowserBrowseUrl: fileman,
                filebrowserImageBrowseUrl: fileman + '?type=image',
                removeDialogTabs: 'link:upload;image:upload',
                extraPlugins: 'image2,youtube,lineheight'
            });
        });
        $("textarea.fck").each(function () {
            CKEDITOR.replace(this.id, {
                toolbar: 'Full',
                filebrowserUploadUrl: "/Editor/ImageUpload?editorType=CKEDITOR",
                filebrowserImageUploadUrl: "/Editor/ImageUpload?editorType=CKEDITOR&type=image",
                filebrowserBrowseUrl: fileman,
                filebrowserImageBrowseUrl: fileman + '?type=image',
                removeDialogTabs: 'link:upload;image:upload',
                extraPlugins: 'image2,youtube,lineheight'
            });
        });
    }
    else {
        $("textarea.fck-small").editable({
            inlineMode: false,
            height: 200,
            buttons: ["bold", "italic", "underline", "strikeThrough", "fontFamily", "fontSize", "color", "formatBlock", "align", "insertOrderedList", "insertUnorderedList", "outdent", "indent", "createLink", "insertImage", "undo", "redo", "html"],
            imageUploadURL: "/Editor/ImageUpload",
            imagesLoadURL: "/Editor/ImageList",
            defaultImageWidth: ""
        }).addClass("editor");
        $("textarea.fck").editable({
            inlineMode: false,
            height: 400,
            imageUploadURL: "/Editor/ImageUpload",
            imagesLoadURL: "/Editor/ImageList",
            defaultImageWidth: "",
            textNearImage: true
        }).addClass("editor");
    }
});

var nowTemp = new Date();
var now = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);
var files;
var storedFiles = [];
var upc = 0;
var check = 0;
var create = 0;


$(function () {
    $("input[id^='multiple_file']").change(function (e) {
        doReCreate(e);
    });
});


function doReCreate(e) {
    upc = upc + 1;
    handleFileSelect(e);
    $("input[id^='multiple_file']").hide();
    $('<input>').attr({
        type: 'file',
        id: 'multiple_file' + upc,
        multiple: 'multiple',
        name: 'multiple_file',
        onchange: "doReCreate(event)",
        title: '  ',
    }).appendTo('.file-image');
    $(".file #number").html(storedFiles.length + " File được chọn").show();
}


function handleFileSelect(e) {
    selDiv = document.querySelector("#previewFiles");
    if (!e.target.files) {
        return;
    }
    files = e.target.files;
    for (var i = 0; i < files.length; i++) {
        var status = 0;
        var f = files[i];
        if (check == 0) {
            storedFiles.push(f.name);
            check = 1;

            var reader = new FileReader();
            reader.onload = (function (theFile) {
                return function (e) {
                    var div = document.createElement('div');
                    div.setAttribute('onclick', 'removeAtt(this)');
                    div.innerHTML = ["<i style='background-image:url(", e.target.result, ");'/> " + theFile.name + ""].join("");
                    selDiv.insertBefore(div, null);
                };
            })(f);
            reader.readAsDataURL(f);
        }
        $.each(storedFiles, function (index, value) {
            if (value == f.name) {
                status = 1;
                return false;
            }
        });
        if (status == 0) {
            storedFiles.push(f.name);

            var reader = new FileReader();
            reader.onload = (function (theFile) {
                return function (e) {
                    var div = document.createElement('div');
                    div.setAttribute('onclick', 'removeAtt(this)');
                    div.innerHTML = ["<i style='background-image:url(", e.target.result, ");' /> " + theFile.name + ""].join("");
                    selDiv.insertBefore(div, null);
                };
            })(f);

            reader.readAsDataURL(f);
        }

    }
    $('#Upload').val(storedFiles);
}


function removeAtt(t) {
    var serEle = $(t).text();
    var index = storedFiles.indexOf(serEle.trim());
    if (index !== -1) {
        storedFiles.splice(index, 1);
    }
    $(t).remove();
    $('#Upload').val(storedFiles);
    $(".file #number").html(storedFiles.length + " File được chọn");
    if (storedFiles.length == 0) {
        $(".file #number").html("Không tệp tin nào được chọn");
    }
}

function getUrlParam(paramName) {
    var reParam = new RegExp('(?:[\?&]|&)' + paramName + '=([^&]+)', 'i');
    var match = window.location.search.match(reParam);

    return (match && match.length > 1) ? match[1] : null;
}
