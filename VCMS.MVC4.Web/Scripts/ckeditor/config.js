/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
    // Define changes to default configuration here. For example:
	//config.language = 'vi';
    // config.uiColor = '#AADC6E';
    //config.extraPlugins = 'youtube';
    config.toolbar_Full = [
    { name: 'document', groups: ['mode', 'document', 'doctools'], items: ['Source'] },
    // On the basic preset, clipboard and undo is handled by keyboard.
    // Uncomment the following line to enable them on the toolbar as well.
    { name: 'clipboard',   groups: [ 'clipboard', 'undo' ], items: [ 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', 'Undo', 'Redo' ] },
    { name: 'editing', groups: ['find', 'selection', 'spellchecker'], items: ['Find', 'Replace', 'SelectAll', 'Scayt'] },
    { name: 'insert', items: ['CreatePlaceholder', 'Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak', 'Iframe','Youtube','InsertPre'] },
    '/',
    { name: 'basicstyles', groups: ['basicstyles', 'cleanup'], items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', 'RemoveFormat'] },
    { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align'], items: ['NumberedList', 'BulletedList', 'Outdent', 'Indent', 'Blockquote', 'CreateDiv', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', 'BidiLtr', 'BidiRtl'] },
    { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
    '/',
    { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize', 'lineheight'] },
    { name: 'colors', items: ['TextColor', 'BGColor'] },
    { name: 'tools', items: ['UIColor', 'Maximize', 'ShowBlocks'] },
    { name: 'about', items: ['About'] }
    ];

    config.toolbar_Basic = [
    { name: 'clipboard',   groups: [ 'clipboard', 'undo' ], items: [ 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', 'Undo', 'Redo' ] },
    { name: 'insert', items: [ 'Image', 'Flash', 'Youtube'] },
   
    { name: 'basicstyles', groups: ['basicstyles', 'cleanup'], items: ['Bold', 'Italic', 'Underline'] },
    { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align'], items: ['NumberedList', 'BulletedList', 'Outdent', 'Indent',  'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
    { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
   
    { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize', 'lineheight'] },
    { name: 'colors', items: ['TextColor', 'BGColor'] },
    { name: 'tools', items: ['UIColor', 'Maximize', 'ShowBlocks'] },
    ];
    config.toolbar = "Full";
};
