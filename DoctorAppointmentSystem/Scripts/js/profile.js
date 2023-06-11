$(document).ready(function () {
    showProfile();

    $('#dateOfBirth').on('focus', function () {
        $(this).attr('type', 'date'); // Chuyển kiểu thành text
    });

    // Lắng nghe sự kiện blur
    $('#dateOfBirth').on('blur', function () {
        $(this).attr('type', 'text'); // Chuyển kiểu thành date
    });

    // Handle event click Edit button
    $('#btnEdit').click(function () {
        $('#btnSave').show();
        $('#btnCancel').show();
        $('input').prop('disabled', false);
        $(this).hide();
    });

    // Handle event click Cancel button
    $('#btnCancel').click(function () {
        $('#btnEdit').show();
        $('#btnSave').hide();
        showProfile();
        $('input').prop('disabled', true);
        $(this).hide();
    });

    // Handle event click Save button
    $('#btnSave').click(function () {
        $('input').prop('disabled', true);
        $('#btnEdit').show();
        $(this).hide();
        $('#btnCancel').hide();
    });
});


var showProfile = function () {
    $.ajax({
        url: '/Account/EditProfileAPI',
        type: 'GET',
        dataType: 'JSON',
        success: function (res) {
            setDataProfile(res);
            console.log(res);
        },
        error: function (err) {
            Swal.fire({
                position: 'top',
                icon: 'error',
                title: 'Error!',
                text: err.responseText,
                showConfirmButton: false,
                timer: 2000,
                width: '30em'
            })
        }
    })
}

var dataProfile = {
    fullName: $('#fullName').val(),
    email: $('#email').val(),
    nationalID: $('#nationalID').val(),
    dateOfBirth: $('#dateOfBirth').val(),
    gender: $('#gender').val(),
    phoneNumber: $('#phoneNumber').val(),
    address: $('#address').val()
}

var setDataProfile = function (res) {
    if (res.data != undefined) {
        $('#fullName').val(res.data.fullName);
        $('#email').val(res.data.email);
        $('#nationalID').val(res.data.nationalID);
        $('#dateOfBirth').val(res.data.dateOfBirth);
        $('#gender').val(res.data.gender);
        $('#phoneNumber').val(res.data.phoneNumber);
        $('#address').val(res.data.address);
    }
}

