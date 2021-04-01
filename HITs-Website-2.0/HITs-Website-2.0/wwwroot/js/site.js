// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function AssignNavbarParameters() {
    navbarWidth = $navbarContainer.width()
    topIndent = parseInt($($navbarPlace).offset().top - $("#blocks-container .info-block:first").first().offset().top)
    startPoint = $("#blocks-container .info-block:first").first().offset().top
    finishPoint = $navbarContainer.height() + $navbarContainer.offset().top - $navbar.height() - topIndent
}

function ChangeToFixed() {
    $navbar.addClass("fixed").removeClass("default").removeClass("press-down");
    $navbar.css('top', `${topIndent}px`)
    $navbar.css('width', `${navbarWidth}px`)
}

function ChangeToDefault() {
    $navbar.removeClass("fixed").addClass("default");
    $navbar.css('top', ``)
    $navbar.css('width', ``)
}

function ChangeToPressDown() {
    $navbar.css('top', ``)
    $navbar.removeClass("fixed").addClass("press-down");
}

function GetPostionTop(elem) {
    return $(elem).offset().top - $(this).scrollTop()
}

function SelectCurrentNavbarItem() {
    let currElem = $(".info-block > a").first()

    $(".info-block > a").each(function (index, elem) {
        if (GetPostionTop(elem) <= 1) {
            currElem = elem
        }
    })

    $navbar.find(".side-navbar-selected").removeClass("side-navbar-selected")
    let sc = $(currElem).attr("name")
    $(`a[href*='${sc}']`).first().parent().addClass("side-navbar-selected")
}

var $navbar = $("#side-navbar");
var $navbarContainer = $("#side-navbar-container");
var $navbarPlace = $("#side-navbar-place");

var navbarWidth
var startPoint
var finishPoint
var topIndent

$(window).scroll(function () {

    if ($(this).scrollTop() <= startPoint && $navbar.hasClass("fixed")) {
        ChangeToDefault()
    } else if ($(this).scrollTop() > startPoint && $(this).scrollTop() < finishPoint && ($navbar.hasClass("default") || $navbar.hasClass("press-down"))) {
        ChangeToFixed()
    } else if ($(this).scrollTop() >= finishPoint && $navbar.hasClass("fixed")) {
        ChangeToPressDown()
    }    

    if ($navbar.hasClass("inAnimState")) {
        return true
    }

    SelectCurrentNavbarItem()
});//scroll

$(window).resize(function () {
    AssignNavbarParameters()

    if ($(this).scrollTop() > startPoint && $(this).scrollTop() < finishPoint) {
        ChangeToFixed()
    } else if ($(this).scrollTop() <= startPoint) {
        ChangeToDefault()
    } else {
        ChangeToPressDown()
    }
});//resize

$(document).ready(function () {

    //Добавляем пункты в навбар
    $(".info-block").each(function (index, elem) {
        let title = $(elem).find("h2:first")
        $(title).before(`<a name="#${index}"></a>`)
        $navbar.find("ul").append(`<li><div><a href="#${index}">${$(title).text()}</a></div></li>`)
    })

    //Делаем переход по якорям плавным
    $navbar.on("click", "a", function (event) {
        event.preventDefault();
        let sc = $(this).attr("href")
        let dn = $(`a[name*='${sc}']`).first().offset().top
        let animationTime = 700;
        $('html, body').animate({ scrollTop: dn }, animationTime)
        $navbar.addClass("inAnimState")
        setTimeout('$navbar.removeClass("inAnimState")', animationTime)
        $navbar.find(".side-navbar-selected").removeClass("side-navbar-selected")
        $(this).parent().addClass("side-navbar-selected")
    });

    //Выставляем позицию навбара
    AssignNavbarParameters()
    if ($(this).scrollTop() > startPoint && $(this).scrollTop() < finishPoint) {
        ChangeToFixed()
    } else if ($(this).scrollTop() <= startPoint) {
        ChangeToDefault()
    } else {
        ChangeToPressDown()
    }

    //Выделяем в навбаре наблюдаемый блок
    SelectCurrentNavbarItem()
});



//Функция для выставление позици в Яндекс Картах и удаления элементов меню
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

