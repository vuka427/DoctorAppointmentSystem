var submit = $('#login-form').submit(function (event) {
    event.preventDefault();
    var user = {
        username: $('#username').val(),
        password: $('#password').val()
    }

    console.log(user);
    $.ajax({
        url: '/Account/Login',
        method: 'POST',
        data: { model: user },
        success: function (res) {
            if (res.success == true) {
                window.location.href = '/Home/Index';
                $('#login-form').trigger('reset');
            } else {
                Swal.fire({
                    position: 'top',
                    icon: 'error',
                    title: 'Error',
                    text: res.message,
                    showConfirmButton: false,
                    timer: 1800,
                    width: '30em'
                })
            }
        },
        error: function (err) {
            Swal.fire({
                position: 'top',
                icon: 'error',
                title: 'Connect to Sever failed!',
                text: res.message,
                showConfirmButton: false,
                timer: 3000,
                width: '30em'
            })
        }
    });
})

var validData = $('#login-form').validate({
    onfocusout: function (element) {
        $(element).valid()
    },
    rules: {
        "Username": {
            required: true
        },
        "Password": {
            required: true
        }
    },
    messages: {
        "Username": {
            required: "Username is required."
        },
        "Password": {
            required: "Password is required."
        }
    },
    highlight: function (element) {
        var elem = $(element);
        if (elem.hasClass("select2-hidden-accessible")) {
            element = $(".select2-selection");

            element.addClass('border-2 border-danger')
        } else {
            elem.addClass('border-2 border-danger ')
        }

    },
    unhighlight: function (element) {
        var elem = $(element);
        if (elem.hasClass("select2-hidden-accessible")) {
            element = $(".select2-selection");

            element.removeClass('border-2 border-danger')
        } else {
            elem.removeClass('border-2 border-danger')
        }
    },
})

$(document).ready(function () {
    submit()
    validData()
})