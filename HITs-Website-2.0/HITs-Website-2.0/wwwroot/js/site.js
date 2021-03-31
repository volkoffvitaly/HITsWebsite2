// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function AssignNavbarParameters() {
    navbarWidth = $navbarContainer.width()
    startPoint = $("#blocks-container :header:first").first().offset().top
    topIndent = parseInt($($navbarPlace).offset().top - $("#blocks-container :header:first").first().offset().top)

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

$(document).ready(function () {

    $(".info-block").each(function (index, elem) {

        let title = $(elem).find("h2:first")
        $(title).before(`<a name="#${index}"></a>`)

        $navbar.find("ul").append(`<li><div><a href="#${index}">${$(title).text()}</a></div></li>`)
    })

    $navbar.on("click", "a", function (event) {
        event.preventDefault();
        var sc = $(this).attr("href")
        var dn = $(`a[name*='${sc}']`).first().offset().top;
        $('html, body').animate({ scrollTop: dn - 20 }, 700);
    });

});


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

