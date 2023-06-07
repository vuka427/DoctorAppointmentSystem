var firstOptions = document.getElementsByClassName('gender');
for (var i = 0; i < firstOptions.length; i++) {
    var option = firstOptions[i].getElementsByTagName('option')[0];
    option.setAttribute('hidden', true);
    option.setAttribute('selected', true);
    option.setAttribute('disabled', true);
}

// Processing event select value of Authetication Question
$(document).ready(function () {
    var selectedValues = [];
    $('.authQuestion').on('change', function () {
        var selectedValue = {
            value: $(this).val(),
            selector: $(this).attr('id')
        }
        if (selectedValues.length === 0) {
            selectedValues.push(selectedValue)
        } else {
            var flag = 0;
            for (var i = 0; i < selectedValues.length; i++) {
                flag = 0;
                if (selectedValue.selector === selectedValues[i].selector) {
                    selectedValues[i].value = selectedValue.value;
                    flag = 1;
                    break;
                }
            }
            if (flag === 0) selectedValues.push(selectedValue);
        }
        console.log(selectedValues);
        for (var i = 0; i < selectedValues.length; i++) {
            if (selectedValues[i].selector === $(this).attr('id')) {
                $('select').not(this).find('option[value="' + selectedValues[i].value + '"]').prop('selected', false);
            }
        }
    });
});

// Get registration data of User and Patient on form to send to AccountController
$('.register-form').submit(function (event) {
    event.preventDefault();
    var confirmPassword = $('#confirmPassword').val();
    var password = $('#password').val();

    // Get User's info
    var user = {
        username : $('#username').val(),
        password : password,
        email : $('#email').val(),
        passwordRecoveryQue1 : $('#authQuestion1').val(),
        passwordRecoveryQue2 : $('#authQuestion2').val(),
        passwordRecoveryQue3 : $('#authQuestion3').val(),
        passwordRecoveryAns1 : $('#answer1').val(),
        passwordRecoveryAns2 : $('#answer2').val(),
        passwordRecoveryAns3 : $('#answer3').val(),
        profilePicture : $('#profilePicture').val()
    }
    
    // Get Patient's info
    var patient = {
        fullName : $('#fullName').val(),
        nationalID : $('#nationalID').val(),
        dateOfBirth : $('#dateOfBirth').val(),
        gender : $('#gender').val(),
        phoneNumber : $('#phoneNumber').val(),
        address : $('#address').val()
    }

    if (password !== confirmPassword) {
        alert("Confirm password do not matched with password!");
        $('#confirmPassword').focus();
    } else {
        $.ajax({
            url: '/Account/RegisterAPI',
            method: 'POST',
            data: {
                user: user,
                patient: patient
            },
            success: function (res) {
                if (res.success == true) {
                    window.location.href = '/Account/Login';
                    $('.register-form').trigger('reset');
                    Swal.fire({
                        position: 'top',
                        icon: 'success',
                        title: 'Congratulations!',
                        text: res.message,
                        showConfirmButton: false,
                        timer: 1500,
                        width: '30em'
                    })
                } else {
                    Swal.fire({
                        position: 'top',
                        icon: 'error',
                        title: 'Error!',
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
    }
})

// Focus on the answer when selected value onchange
$('#authQuestion1').change(function () {
    $('#answer1').val('');
    $('#answer1').focus();
})

$('#authQuestion2').change(function () {
    $('#answer2').val('');
    $('#answer2').focus();
})

$('#authQuestion3').change(function () {
    $('#answer3').val('');
    $('#answer3').focus();
})