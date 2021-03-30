// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


var $navbar = $("#side-navbar");
var startPoint = $("#info-block :header:first").first().offset().top
var lenght = parseInt($($navbar).offset().top - $("#info-block :header:first").first().offset().top)

$(window).scroll(function () {

    if ($(this).scrollTop() > startPoint && $navbar.hasClass("default")) {
        $navbar.removeClass("default").addClass("fixed");
        $('.fixed').css('top', `${lenght}px`)
    } else if ($(this).scrollTop() <= startPoint && $navbar.hasClass("fixed")) {
        $navbar.removeClass("fixed").addClass("default");
    }
});//scroll



function YandexReadyHandler() {
    var map = new ymaps.Map("ymap", {
        center: [56.46910481095714, 84.94797934925934],
        zoom: 16,
        controls: [],
        type: "yandex#map"
    }, {
        suppressMapOpenBlock: true
    });
    map.geoObjects.add(new ymaps.GeoObject({
        geometry: {
            type: "Point",
            coordinates: [56.46953536079353, 84.94760383999729],
            hideIconOnBalloonOpen: false
        },
        properties: {
            balloonContent: decodeURIComponent("%D0%A2%D0%BE%D1%87%D0%BA%D0%B0"),
            iconCaption: decodeURIComponent(""),
            hintCaption: decodeURIComponent(""),
        }
    }, {
        preset: "islands#blueIcon",
    })
    ); return map;
}

