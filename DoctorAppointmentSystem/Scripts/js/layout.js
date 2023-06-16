$(function () {
    $(".navbar-toggler").on("click", function (e) {
        var headerElement = $('header');
        var windowWidth = $(window).width();
        if (windowWidth >= 992) {
            if (!headerElement.hasClass('hidden')) {
                $(".custom-header").toggleClass("hidden");
                $('.custom-main').css('margin', '0 auto');
                $('.custom-header').css('transition', 'all 0.3s ease');
            } else {
                $(".custom-header").removeClass("hidden");
                $('.custom-main').css('margin', '0 0 0 290px');
                $('.custom-header').css('transition', 'all 0.3s ease');
            }
        } else {
            if (headerElement.hasClass('hidden')) {
                $(".custom-header").removeClass("hidden");
                $('.custom-header').css('transition', 'all 0.3s ease');
            }
            $(".custom-header").toggleClass("show");
            e.stopPropagation();
        }
    });

    $("html").click(function (e) {
        var header = document.getElementById("custom-header");

        if (!header.contains(e.target)) {
            $(".custom-header").removeClass("show");
        }
    });

    $("#custom-nav .nav-link").click(function (e) {
        $(".custom-header").removeClass("show");
    });
});