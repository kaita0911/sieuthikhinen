jQuery(document).ready(function(){
    var scripts = document.getElementsByTagName("script");
    var jsFolder = "";
    for (var i= 0; i< scripts.length; i++)
    {
        if( scripts[i].src && scripts[i].src.match(/initcarousel-adv\.js/i))
            jsFolder = scripts[i].src.substr(0, scripts[i].src.lastIndexOf("/") + 1);
    }
   
    jQuery("#amazingcarousel-adv").amazingcarousel({
        jsfolder:jsFolder,
        width:300,
        height:135,
        skinsfoldername:"",
        interval:3000,
        itembottomshadowimagetop:100,
        donotcrop:false,
        random:true,
        showhoveroverlay:false,
        height: 135,
        arrowheight:8,
        showbottomshadow:false,
        itembackgroundimagewidth:100,
        imageheight: 135,
        skin:"fade",
        responsive:true,
        lightboxtitlebottomcss:"{color:#333; font-size:14px; font-family:Armata,sans-serif,Arial; overflow:hidden; text-align:left;}",
        enabletouchswipe:true,
        navstyle:"none",
        backgroundimagetop:-40,
        arrowstyle: "",
        bottomshadowimagetop:100,
        transitionduration:1000,
        itembackgroundimagetop:0,
        hoveroverlayimage:"hoveroverlay-64-64-4.png",
        itembottomshadowimage:"itembottomshadow-100-100-5.png",
        lightboxshowdescription:false,
        width:300,
        navswitchonmouseover:false,
        showhoveroverlayalways:false,
        transitioneasing:"easeOutExpo",
        lightboxshownavigation:false,
        showitembackgroundimage:false,
        itembackgroundimage:"",
        playvideoimagepos:"center",
        circular:true,
        arrowimage:"arrows-32-32-4.png",
        scrollitems:1,
        direction:"vertical",
        lightboxdescriptionbottomcss:"{color:#333; font-size:12px; font-family:Arial,Helvetica,sans-serif; overflow:hidden; text-align:left; margin:4px 0px 0px; padding: 0px;}",
        supportiframe:false,
        navimage:"bullet-24-24-0.png",
        backgroundimagewidth:220,
        showbackgroundimage:false,
        lightboxbarheight:64,
        showplayvideo:true,
        spacing:0,
        lightboxthumbwidth:80,
        navdirection:"vertical",
        itembottomshadowimagewidth:100,
        backgroundimage:"",
        lightboxthumbtopmargin:12,
        autoplay: true,
        arrowwidth:24,
        transparent:false,
        bottomshadowimage:"bottomshadow-110-100-5.png",
        scrollmode:"page",
        navmode:"page",
        lightboxshowtitle:true,
        lightboxthumbbottommargin:8,
        arrowhideonmouseleave:1000,
        showitembottomshadow:false,
        lightboxthumbheight:100,
        navspacing:0,
        pauseonmouseover:true,
        imagefillcolor:"FFFFFF",
        playvideoimage:"playvideo-64-64-0.png",
        visibleitems:1,
        imagewidth:300,
        usescreenquery:false,
        bottomshadowimagewidth:110,
        screenquery:{
	mobile: {
		screenwidth: 600,
		visibleitems: 1
	}
},
        navwidth:24,
        loop:0,
        navheight:24
    });
});