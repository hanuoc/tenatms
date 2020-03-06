/**
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

var CURRENT_URL = window.location.href.split('?')[0],
$BODY = $('body'),
$MENU_TOGGLE = $('#menu_toggle'),
$SIDEBAR_MENU = $('#sidebar-menu'),
$SIDEBAR_FOOTER = $('.sidebar-footer'),
$LEFT_COL = $('.left_col'),
$RIGHT_COL = $('.right_col'),
$NAV_MENU = $('.nav_menu'),
$FOOTER = $('footer');

// Sidebar
$(document).ready(function() {
// TODO: This is some kind of easy fix, maybe we can improve this
var setContentHeight = function () {
    // reset height
    $RIGHT_COL.css('min-height', $(window).height());

    var bodyHeight = $BODY.outerHeight(),
        footerHeight = $BODY.hasClass('footer_fixed') ? 0 : $FOOTER.height(),
        leftColHeight = $LEFT_COL.eq(1).height() + $SIDEBAR_FOOTER.height(),
        contentHeight = bodyHeight < leftColHeight ? leftColHeight : bodyHeight;

    // normalize content
    contentHeight -= $NAV_MENU.height() + footerHeight;
    $RIGHT_COL.css('min-height', contentHeight);
}; 
$(window).smartresize(function(){  
    setContentHeight();
});
setContentHeight();

// fixed sidebar
if ($.fn.mCustomScrollbar) {
    $('.menu_fixed').mCustomScrollbar({
        autoHideScrollbar: true,
        theme: 'minimal',
        mouseWheel:{ preventDefault: true }
    });
}
});
// /Sidebar

// Progressbar
// if ($(".progress .progress-bar")[0]) {
// $('.progress .progress-bar').progressbar();
// }
// /Progressbar

// Switchery
$(document).ready(function() {
if ($(".js-switch")[0]) {
    var elems = Array.prototype.slice.call(document.querySelectorAll('.js-switch'));
    elems.forEach(function (html) {
        var switchery = new Switchery(html, {
            color: '#26B99A'
        });
    });
}
});
// /Switchery

// iCheck
$(document).ready(function() {
if ($("input.flat")[0]) {
    $(document).ready(function () {
        $('input.flat').iCheck({
            checkboxClass: 'icheckbox_flat-green',
            radioClass: 'iradio_flat-green'
        });
    });
}
});
// /iCheck

// Table
$('table input').on('ifChecked', function () {
checkState = '';
$(this).parent().parent().parent().addClass('selected');
countChecked();
});
$('table input').on('ifUnchecked', function () {
checkState = '';
$(this).parent().parent().parent().removeClass('selected');
countChecked();
});

var checkState = '';

$('.bulk_action input').on('ifChecked', function () {
checkState = '';
$(this).parent().parent().parent().addClass('selected');
countChecked();
});
$('.bulk_action input').on('ifUnchecked', function () {
checkState = '';
$(this).parent().parent().parent().removeClass('selected');
countChecked();
});
$('.bulk_action input#check-all').on('ifChecked', function () {
checkState = 'all';
countChecked();
});
$('.bulk_action input#check-all').on('ifUnchecked', function () {
checkState = 'none';
countChecked();
});

function countChecked() {
if (checkState === 'all') {
    $(".bulk_action input[name='table_records']").iCheck('check');
}
if (checkState === 'none') {
    $(".bulk_action input[name='table_records']").iCheck('uncheck');
}

var checkCount = $(".bulk_action input[name='table_records']:checked").length;

if (checkCount) {
    $('.column-title').hide();
    $('.bulk-actions').show();
    $('.action-cnt').html(checkCount + ' Records Selected');
} else {
    $('.column-title').show();
    $('.bulk-actions').hide();
}
}
$(document).ready(function(){
window.addEventListener('storage',function(e){

    if(e.key=='logout')
    {
     
       localStorage.clear();
       this.window.location.href="http://localhost:4200";

    }
  })
})
//Accordion
$(document).ready(function() {
$(".expand").off('click').on("click", function () {
    $(this).next().slideToggle(200);
    $expand = $(this).find(">:first-child");

    if ($expand.text() == "+") {
        $expand.text("-");
    } else {
        $expand.text("+");
    }
});
});

// NProgress
if (typeof NProgress != 'undefined') {
$(document).ready(function () {
    NProgress.start();
});

$(window).on('load', function() {
    NProgress.done();
});
}
//Include tinymce by nvthang
// tinymce.init({
//     selector: "textarea"
// });
/**
* Resize function without multiple trigger
* 
* Usage:
* $(window).smartresize(function(){  
*     // code here
* });
*/
(function($,sr){
// debouncing function from John Hann
// http://unscriptable.com/index.php/2009/03/20/debouncing-javascript-methods/
var debounce = function (func, threshold, execAsap) {
  var timeout;

    return function debounced () {
        var obj = this, args = arguments;
        function delayed () {
            if (!execAsap)
                func.apply(obj, args);
            timeout = null; 
        }

        if (timeout)
            clearTimeout(timeout);
        else if (execAsap)
            func.apply(obj, args);

        timeout = setTimeout(delayed, threshold || 100); 
    };
};

// smartresize 
jQuery.fn[sr] = function(fn){  return fn ? this.bind('resize', debounce(fn)) : this.trigger(sr); };

})(jQuery,'smartresize');
// $(document).ready(function(){
//     $(".ajs-input")
// });

var debug = true;//true: add debug logs when cloning
var evenMoreListeners = true;//demonstrat re-attaching javascript Event Listeners (Inline Event Listeners don't need to be re-attached)
if (evenMoreListeners) {
    var allFleChoosers = $("input[type='file']");
    addEventListenersTo(allFleChoosers);
    function addEventListenersTo(fileChooser) {
    }
}
var clone = {};

// FileClicked()
function fileClicked(event) {
    var fileElement = event.target;
    if (fileElement.value != "") {
        clone[fileElement.id] = $(fileElement).clone(); //'Saving Clone'
    }
}

// FileChanged()
function fileChanged(event) {
    var fileElement = event.target;
    if (fileElement.value == "") {
        clone[fileElement.id].insertBefore(fileElement); //'Restoring Clone'
        $(fileElement).remove(); //'Removing Original'
        if (evenMoreListeners) { addEventListenersTo(clone[fileElement.id]) }
    }
}


// $(document).on('click','.toggle-password',function(){
//     $(this).toggleClass("fa-eye fa-eye-slash");
//     var input = $($(this).attr("toggle"));
//     if (input.attr("type") == "password") {
//         input.attr("type", "text");
//     } else {
//         input.attr("type", "password");
//     }
// });
$(document).on('mousedown','.toggle-password',function(){
    $(this).toggleClass("fa-eye fa-eye-slash");
    var input = $($(this).attr("toggle"));
    if (input.attr("type") == "password") {
        input.attr("type", "text");
    } else {
        input.attr("type", "password");
    }
});
$(document).on('mouseup','.toggle-password',function(){
    $(this).toggleClass("fa-eye fa-eye-slash");
    var input = $($(this).attr("toggle"));
    if (input.attr("type") == "text") {
        input.attr("type", "password");
    } else {
        input.attr("type", "text");
    }
});

