$.fn.pager = function (config)
{
    $(this).each(function () {
        
        $(this).empty();
        var itemcount = config.itemCount;
        var pageSize = config.pageSize;
        var pagecount = Math.floor(itemcount / pageSize) + (itemcount % pageSize > 0 ? 1 : 0);
        var pageIndex = 1;
        if (config.pageIndex !=undefined ) pageIndex = config.pageIndex;
        var length = 5;
        if (config.length != undefined) length = config.length;
        $(this).attr("pager-count", pagecount);
        if (pagecount == 1) return;
        
        var start = pageIndex - Math.floor(length / 2);
        if (start <= 0) start = 1;
        var end = start + length - 1;
        
        if (end > pagecount) {
            end = pagecount;
            start = end - length + 1; if (start <= 0) start = 1;
        }
       
        
        var pagerId = config.pagerId;
        if (start > 1) {
            var href = config.href;
            href = href.replace(/pageIndex=\d+/, "pageIndex=" + 1);
            var item = $("<a>");
            item.attr("href", href);
            item.html('First');
            item.attr({ "data-ajax": "true", "data-ajax-method": "get", "data-ajax-mode": "replace", "data-ajax-update": config.updateId });
            item.attr({ 'data-ajax-success': 'pagerSuccess(data, status, xhr, {PageIndex:' + 1 + ', PagerId: "' + config.pagerId + '"},' + config.callback + ')', id: 'page_' + 1 });
            if (i == 1)
                item.addClass('active');
            item.addClass("first");
            $(this).append(item);
        }
        for (i = start; i <= end; i++) {
            
            var href = config.href;
            href = href.replace(/pageIndex=\d+/, "pageIndex=" + (i));
            var item = $("<a>");
            item.attr("href", href);
            item.html(i);
            item.attr({ "data-ajax": "true", "data-ajax-method": "get", "data-ajax-mode": "replace", "data-ajax-update": config.updateId });
            item.attr({ 'data-ajax-success': 'pagerSuccess(data, status, xhr, {PageIndex:' + i + ', PagerId: "' + config.pagerId + '"},' + config.callback + ')', id: 'page_' + i });
            if ( i == pageIndex)
                item.addClass('active');
            item.addClass("item");
            $(this).append(item);
        }
        //if (end < pagecount)
        //{
        //    var href = config.href;
        //    href = href.replace(/pageIndex=\d+/, "pageIndex=" + pagecount);
        //    var item = $("<a>");
        //    item.attr("href", href);
        //    item.html('Last');
        //    item.attr({ "data-ajax": "true", "data-ajax-method": "get", "data-ajax-mode": "replace", "data-ajax-update": config.updateId });
        //    item.attr({ 'data-ajax-success': 'pagerSuccess(data, status, xhr, {PageIndex:' + pagecount + ', PagerId: "' + config.pagerId + '"},' + config.callback + ')' , id:'page_' + pagecount});
        //    if (i == 1)
        //        item.addClass('active');
        //    item.addClass("last");
        //    $(this).append(item);
        //}
    });
   
}


function pagerSuccess(data, status, xhr, json, callback) {
    var pagerId = json.PagerId;
    var index = json.PageIndex;
    var pagers = $("div[pager-id='" + pagerId + "']");
    pagers.each(function () {
        var count = parseInt($(this).attr('pager-count'));
        var items = $("a.item", this);
        items.removeClass('active');

        var start = index - Math.floor(items.length / 2);
        if (start <= 0) start = 1;
        var end = start + items.length - 1;
        if (end > count) {
            end = count;
            start = end - items.length + 1;
        }
        for (i = 0; i < items.length; i++) {
            var item = $(items[i]);
            var href = item.attr('href');
            href = href.replace(/pageIndex=\d+/, "pageIndex=" + (start + i));
            item.attr('href', href).text(start + i);
            item.attr('data-ajax-success', 'pagerSuccess(data, status, xhr, {PageIndex:' + (start + i) + ', PagerId: "' + pagerId + '"},' + callback + ')');
            if (start + i == index)
                item.addClass('active');
        }
        //show/hide last
        if (end >= count)
            $("a.last", this).hide();
        else
            $("a.last", this).show();
        //show/hide first
        if (start == 1)
            $("a.first", this).hide();
        else
            $("a.first", this).show();

        if (callback) callback(data, status, xhr);
    })


}