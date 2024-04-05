$("#register-btn").click(function() {
    $("#login").slideUp();
    $("#register").fadeIn();
})
$("#login-btn").click(function() {
    $("#register").slideUp();
    $("#login").slideDown();
})
$(document).ready(function() {
    var route = location.href.split("#")[1];
    var route2 = location.href.split("/")[4];
    if (route2) {
        if (route2 == "Register") {
            $("#login").slideUp();
            $("#register").fadeIn();
        } else {
            $("#register").slideUp();
            $("#login").slideDown();
        }
    }
    if (route) {
        if (route == "register") {
            $("#login").slideUp();
            $("#register").fadeIn();
        } else {
            $("#register").slideUp();
            $("#login").slideDown();
        }
    }
   
});
$(".fileUpload").click(function () {
    var isText = $('.fileUpload i').hasClass("fa fa-upload");
    if (isText) {
        $(".footer form input[type='text']").hide();
        $(".footer form input[type='text']").val('');
        $(".footer form input[type='file']").show();
        $(".fileUpload i").removeClass("fa-upload");
        $(".fileUpload i").addClass("fa-align-justify");
    } else {
        $(".footer form input[type='text']").show();
        $(".footer form input[type='file']").hide();
        $(".fileUpload i").addClass("fa-upload");
        $(".fileUpload i").removeClass("fa-align-justify");
    }
});