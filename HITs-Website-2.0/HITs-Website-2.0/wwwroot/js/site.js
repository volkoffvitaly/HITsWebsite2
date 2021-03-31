// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function AssignNavbarParameters() {
    navbarWidth = $navbarContainer.width()
    startPoint = $("#info-block :header:first").first().offset().top
    topIndent = parseInt($($navbarPlace).offset().top - $("#info-block :header:first").first().offset().top)

    console.log(topIndent)
}

function ChangeToFixed() {
    $navbar.removeClass("default").addClass("fixed");
    $navbar.css('top', `${topIndent}px`)
    $navbar.css('width', `${navbarWidth}px`)
}

function ChangeToDefault() {
    $navbar.removeClass("fixed").addClass("default");
    $navbar.css('top', ``)
    $navbar.css('width', ``)
}

var $navbar = $("#side-navbar");
var $navbarContainer = $("#side-navbar-container");
var $navbarPlace = $("#side-navbar-place");

var navbarWidth
var startPoint
var topIndent

AssignNavbarParameters()

$(window).scroll(function () {
    if ($(this).scrollTop() > startPoint && $navbar.hasClass("default")) {
        ChangeToFixed()      
    } else if ($(this).scrollTop() <= startPoint && $navbar.hasClass("fixed")) {
        ChangeToDefault()
    }
});//scroll

$(window).resize(function () {
    AssignNavbarParameters()

    if ($(this).scrollTop() > startPoint) {
        ChangeToFixed()
    } else if ($(this).scrollTop() <= startPoint) {
        ChangeToDefault()
    }
});//resize



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

