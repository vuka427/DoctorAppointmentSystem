$('#login-form').submit(function (event) {
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
                    title: 'Oops...',
                    text: res.message,
                    showConfirmButton: false,
                    timer: 1500,
                    width: '30em'
                })
            }
        },
        error: function (err) {
            Swal.fire({
                position: 'top',
                icon: 'error',
                title: 'Oops...',
                text: res.message,
                showConfirmButton: false,
                timer: 1500,
                width: '30em'
            })
        }
    });
})