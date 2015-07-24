var modal = $('#login_modal');
var shade = $('#shade');
$('#start').on('click', function (e) {
    modal.css("display", 'block');
    var navbar = $("#navbar");
    var rt = ($(window).width() - (navbar.offset().left + navbar.outerWidth())).toString() + "px";
    modal.css("right", rt)
    e.preventDefault();
});
$('#close').on('click', function (e) {
    modal.css("display", 'none');
    e.preventDefault();
});
$(window).resize(function () {
    var navbar = $("#navbar");
    var rt = ($(window).width() - (navbar.offset().left + navbar.outerWidth())).toString() + "px";
    modal.css("right", rt)
});

function after_login(data) {
    if (data != null) {
        $("#user_field").css("display", "block");
        $('#user_field').html($('#user-field-template').tmpl(data));
        modal.css("display", 'none');
    }
}

function login_error(data) {
    window.location = data.responseText;
}

function register_error(data) {
    window.location = data.responseText;
}