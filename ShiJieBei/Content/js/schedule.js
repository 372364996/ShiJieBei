$(function () {
    var htmlH = $("html").outerHeight(true);
    var navH = $("#nav").outerHeight(true);
    var bannerH = $("#banner").outerHeight(true);
    var listH = htmlH - navH - bannerH - 12;
    $("#list").attr("style","overflow:scroll; height: " + listH + "px;");
})


