// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function AssignNavbarParameters() {
    //navbarWidth = $navbarContainer.width()
    topIndent = parseInt($('.box').first().offset().top - $("#blocks-container .info-block:first").first().offset().top)
    $navbar.css('margin-top', `${topIndent}px`)
    $navbar.css('top', `${topIndent}px`)

    //startPoint = $("#blocks-container .info-block:first").first().offset().top
    //finishPoint = $("#blocks-container").height() + $("#blocks-container").offset().top - $navbar.height() - topIndent
}
function InvalidMsg(textbox, message) {
    if (textbox.value === '') {
        textbox.setCustomValidity(message);
    } 
    else {
        textbox.setCustomValidity('');
    }

    return true;
}
//function ChangeToFixed() {
//    $navbar.css('top', `${topIndent}px`)
//    $navbar.css('width', `${navbarWidth}px`)
//    $navbar.addClass("fixed").removeClass("default").removeClass("press-down");
//}

//function ChangeToDefault() {
//    $navbar.css('top', ``)
//    $navbar.css('width', ``)
//    $navbar.removeClass("fixed").addClass("default");
//}

//function ChangeToPressDown() {
//    $navbar.css('top', ``)
//    $navbar.removeClass("fixed").addClass("press-down");
//}

function GetPostionTop(elem) {
    return $(elem).offset().top - $(this).scrollTop()
}

function HandlePageResizing(scrollTop) {
    AssignNavbarParameters()

    //if (scrollTop > startPoint && scrollTop < finishPoint) {
    //    ChangeToFixed()
    //} else if (scrollTop <= startPoint) {
    //    ChangeToDefault()
    //} else {
    //    ChangeToPressDown()
    //}
}

function SelectCurrentNavbarItem() {
    let currElem = $(".info-block > a").first()

    $(".info-block > a").each(function (index, elem) {
        if (GetPostionTop(elem) <= 1) {
            currElem = elem
        }
    })

    let sc = $(currElem).attr("name")
    let currentDiv = $(`a[href*='${sc}']`).first().parent()
    if (currentDiv != $navbar.find(".side-navbar-selected").first()) {
        $navbar.find(".side-navbar-selected").removeClass("side-navbar-selected")
        $(currentDiv).addClass("side-navbar-selected")
    }
}

var $navbar = $("#side-navbar");
var $navbarContainer = $("#side-navbar-container");
var $navbarPlace = $("#side-navbar-place");
var buttons = [$("#teachers-btn"), $("#students-btn"), $("#graduates-btn")];

var navbarWidth
var startPoint
var finishPoint
var topIndent

window.onscroll = function () {

    //let scrollTop = $(this).scrollTop()

    //if (scrollTop <= startPoint && $navbar.hasClass("fixed")) {
    //    ChangeToDefault()
    //} else if ((scrollTop > startPoint && scrollTop < finishPoint)
    //    && ($navbar.hasClass("default") || $navbar.hasClass("press-down"))) {
    //    ChangeToFixed()
    //} else if (scrollTop >= finishPoint && $navbar.hasClass("fixed")) {        
    //    ChangeToPressDown()
    //}    

    if ($navbar.hasClass("inAnimState")) {
        return true
    }

    SelectCurrentNavbarItem()

};//scroll

$(window).resize(function () {
    HandlePageResizing($(this).scrollTop())

    ChangePhotosWidth()
});//resize

buttons.forEach(btn => btn.click(function () {
    HandlePageResizing($(window).scrollTop())
}))

$(document).ready(function () {

    //Добавляем пункты в навбар
    $(".info-block").each(function (index, elem) {
        let title = $(elem).find("h2:first")
        $(elem).prepend(`<a name="#${index}"></a>`)
        $navbar.find("ul").append(`<li><div><a href="#${index}">${$(title).text()}</a></div></li>`)
    })

    //Делаем переход по якорям плавным

    let animationTimeout
    let animationTime = 700;
    $navbar.on("click", "a", function (event) {
        event.preventDefault();
        let sc = $(this).attr("href")
        let dn = $(`a[name*='${sc}']`).first().offset().top
        $('html, body').stop()                                                              //Остановка текущей анимации
        $('html, body').animate({ scrollTop: dn }, animationTime)                           //Запуск анимации
        $navbar.addClass("inAnimState")                                                     //Добваления класса, показываеющего что находится в состоянии анимации
        clearTimeout(animationTimeout);                                                     //Остановка прошлого таймаута
        animationTimeout = setTimeout('$navbar.removeClass("inAnimState")', animationTime)  //Запуск таймаута на удаления класса
        $navbar.find(".side-navbar-selected").removeClass("side-navbar-selected")
        $(this).parent().addClass("side-navbar-selected")
    });

    //Выставляем позицию навбара
    AssignNavbarParameters()

    //if ($(this).scrollTop() > startPoint && $(this).scrollTop() < finishPoint) {
    //    ChangeToFixed()
    //} else if ($(this).scrollTop() <= startPoint) {
    //    ChangeToDefault()
    //} else {
    //    ChangeToPressDown()
    //}

    //Выделяем в навбаре наблюдаемый блок
    SelectCurrentNavbarItem()


    ChangePhotosWidth()
});




function ChangePhotosWidth() {
    $('.photos-container').find('.photo').each(function () {
        $(this).height($(this).width() * 0.65)
    })
}


//var $addCards = $('.adding-card')

//$addCards.each(function () {
//    let container = $(this).find('.btn-add-container').first()
//    let addBtn = $(container).find('.add-btn').first()
//    let form = $(this).find('form').first()
//    let cancelBtn = $(this).find('.cancel-adding').first()

//    addBtn.click(function () {
//        $(form).removeClass('hidden')
//        $(container).addClass('hidden')

//        HandlePageResizing($(window).scrollTop())
//    })

//    cancelBtn.click(function () {
//        $(container).removeClass('hidden')
//        $(form).addClass('hidden')

//        HandlePageResizing($(window).scrollTop())
//    })
//})


//var $titleBlocks = $('.title-block');
//$titleBlocks.each(function () {
//    let container = $(this).find('h2').first()
//    let editBtn = $(container).find('.edit-block-name').first()
//    let form = $(this).find('form').first()
//    let cancelBtn = $(this).find('.cancel-editing').first()

//    editBtn.click(function () {
//        $(form).removeClass('hidden')
//        $(container).addClass('hidden')

//        HandlePageResizing($(window).scrollTop())
//    })

//    cancelBtn.click(function () {
//        $(container).removeClass('hidden')
//        $(form).addClass('hidden')

//        HandlePageResizing($(window).scrollTop())
//    })
//})



$('.popup-bg').click(function () {
    $('.popup').slideUp(300)
    $(this).fadeOut(300)
})

var $popup = $('.popup');
$popup.each(function () {
    let popup = $(this)
    let closeBtn = $(this).find('.close-popup').first()

    closeBtn.click(function () {
        hide()
    })

    function hide() {
        $(popup).slideUp(300)
        $('.popup-bg').fadeOut(300)
    }
})

$('.open-popup').click(function () {
    let popup_id = $('#' + $(this).attr("rel")); // Связываем rel и popup_id
    $(popup_id).slideDown('slow'); // Открываем окно
    $('.popup-bg').fadeIn('slow'); // Открываем блок заднего фона
})

$('.load-input').change(function () {
    if ($(this).val() != '') $(this).prev().text($(this).val());
    else $(this).prev().text('Выберите файлы');
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

