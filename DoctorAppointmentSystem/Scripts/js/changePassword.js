$(document).on('submit', '#formVerifyAccount', function (event) {
    event.preventDefault();

    var answers = {
        ans1: $('#authQuestion1').val(),
        ans2: $('#authQuestion2').val(),
        ans3: $('#authQuestion3').val()
    }

    if ($('#formVerifyAccount').valid()) {
        $.ajax({
            url: '/Account/VerifyAnswers',
            method: 'GET',
            data: answers,
            dataType: 'JSON',
            success: function (res) {
                if (res.success) {
                    showChangePassForm();
                } else {
                    Swal.fire({
                        position: 'top',
                        icon: 'warning',
                        title: 'Failed!',
                        text: res.message,
                        showConfirmButton: false,
                        timer: 2000,
                        width: '30em'
                    })
                }
            },
            error: function (err) {
                console.log(err.responseText);
            }
        })
    }
})

$(document).on('submit', '#formChangePassword', function (event) {
    event.preventDefault();

    var data = {
        currPass: $('#currPass').val(),
        newPass: $('#newPass').val(),
    }

    if ($('#formChangePassword').valid()) {
        $.ajax({
            url: '/Account/ChangePasswordAPI',
            method: 'POST',
            data: data,
            dataType: 'JSON',
            success: function (res) {
                console.log(res);
                if (res.success) {
                    Swal.fire({
                        position: 'top',
                        icon: 'success',
                        title: 'Congratulation!',
                        text: res.message,
                        showConfirmButton: false,
                        timer: 2000,
                        width: '30em'
                    });
                    setTimeout(function () {
                        $('#formChangePassword').trigger('reset');
                        logoutUser();
                    }, 2000);
                } else {
                    Swal.fire({
                        position: 'top',
                        icon: 'warning',
                        title: 'Failed!',
                        text: res.message,
                        showConfirmButton: false,
                        timer: 2000,
                        width: '30em'
                    })
                }
            },
            error: function (err) {
                console.log(err.responseText);
            }
        })
    }
})

function logoutUser() {
    $.ajax({
        url: '/Account/LogoutAPI',
        method: 'GET',
        dataType: 'JSON',
        success: function (res) {
            if (res.success) {
                window.location.href = '/Account/Login';
            }
        },
        error: function (err) {
            console.log(err.responseText);
        }
    });
}

function showChangePassForm() {
    $('#formVerifyAccount').hide();
    $('#formChangePassword').show();
    $('#formVerifyAccount').trigger('reset');
    $('#formChangePassword').trigger('reset');
}

function cancel() {
    $('#formVerifyAccount').trigger('reset');
    $('#formVerifyAccount').trigger('reset');
}

var validVerifyAccountForm = $('#formVerifyAccount').validate({
    onfocusout: function (element) {
        $(element).valid()
    },
    rules: {
        "authQuestion1": {
            required: true
        },
        "authQuestion2": {
            required: true
        },
        "authQuestion3": {
            required: true
        }
    },
    messages: {
        "authQuestion1": {
            required: 'This value is required.'
        },
        "authQuestion2": {
            required: 'This value is required.'
        },
        "authQuestion3": {
            required: 'This value is required.'
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

function validChangePasswordForm() {
    jQuery.validator.addMethod('passwordFormat', function (value) {
        var regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,50}$/;
        return value.trim().match(regex);
    });

    $('#formChangePassword').validate({
        onfocusout: function (element) {
            $(element).valid()
        },
        rules: {
            "currPass": {
                required: true,
                passwordFormat: true
            },
            "newPass": {
                required: true,
                passwordFormat: true
            },
            "confirmPass": {
                required: true,
                passwordFormat: true,
                equalTo: "#newPass"
            }
        },
        messages: {
            "currPass": {
                required: 'Password is required.',
                passwordFormat: "Password charater at least one uppercase letter, one lowercase letter, one number and one special character"
            },
            "newPass": {
                required: 'New password is required.',
                passwordFormat: "Password charater at least one uppercase letter, one lowercase letter, one number and one special character"
            },
            "confirmPass": {
                required: 'Confirm password value is required.',
                passwordFormat: "Password charater at least one uppercase letter, one lowercase letter, one number and one special character",
                equalTo: "Does not match the password."
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
}
    

$(document).ready(function () {
    validVerifyAccountForm.call;
    validChangePasswordForm();
})