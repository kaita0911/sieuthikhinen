
<head>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
</head>
<div class="bg-error">
    <h2><span>404</span> File or directory not found</h2>
    <div class="title">
        <span>The resource you are looking for might have been removed, had its name changed, or is <br />temporarily unavailable.</span>
    </div>
    <%--<div class="search hidden-md hidden-sm hidden-xs">
        @using (Html.BeginForm("Search", "Home", FormMethod.Get, new { accept_charset = "utf-8", @id = "search-forms" }))
        {
            <input id="key" type="text" name="keyword" class="txt-search" placeholder="Enter your keyword here..." />
            <button type="submit" class="btn-search"></button>
        }
    </div>--%>
    <div class="back-home">
        <a href="/">Back to Home</a>
    </div>
</div>
<link rel="stylesheet" href="Content/bootstrap.css" />
<link rel="stylesheet" href="Content/error.css" />
<script src='Scripts/jquery-1.8.2.min.js'></script>
<script>
    $(document).ready(function () {
        $("head").append("<meta charset=utf-8 />");
    });
    $("#search-forms").submit(function () {
        if ($("#key").val() == "" || $("#key").val() == " ") {
            alert("Please enter your keywords.");
            $("#key").focus();
            return false;
        }
        return true;
    });
</script>




