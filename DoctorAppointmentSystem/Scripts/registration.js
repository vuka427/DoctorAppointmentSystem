// const { type } = require("jquery");

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

function loadData() {

}

function registerUser(username, password, passwordRecoveryQue1, passwordRecoveryQue2, passwordRecoveryQue3, passwordRecoveryAns1, passwordRecoveryAns2, passwordRecoveryAns3) {
    this.username = username;
    this.password = password;
    this.passwordRecoveryQue1 = passwordRecoveryQue1;
    this.passwordRecoveryQue2 = passwordRecoveryQue2;
    this.passwordRecoveryQue3 = passwordRecoveryQue3;
    this.passwordRecoveryAns1 = passwordRecoveryAns1;
    this.passwordRecoveryAns2 = passwordRecoveryAns2;
    this.passwordRecoveryAns3 = passwordRecoveryAns3;
}

function registerPatient(fullName, email, nationalID, dateOfBirth, gender, phoneNumber, address, profilePicture) {
    this.fullName = fullName;
    this.email = email;
    this.nationalID = nationalID;
    this.dateOfBirth = dateOfBirth;
    this.gender = gender;
    this.phoneNumber = phoneNumber;
    this.address = address;
    this.profilePicture = profilePicture;
}

// Get registration data of User and Patient on form to send to AccountController
$('.register-form').submit(function (event) {
    event.preventDefault();

    // Get User's info
    var username = $('#username').val();
    var password = $('#password').val();
    var confirmPassword = $('#confirmPassword').val();
    var passwordRecoveryQue1 = $('#authQuestion1').val();
    var passwordRecoveryQue2 = $('#authQuestion2').val();
    var passwordRecoveryQue3 = $('#authQuestion3').val();
    var passwordRecoveryAns1 = $('#answer1').val();
    var passwordRecoveryAns2 = $('#answer2').val();
    var passwordRecoveryAns3 = $('#answer3').val();

    // Get Patient's info
    var fullName = $('#fullName').val();
    var email = $('#email').val();
    var nationalID = $('#nationalID').val();
    var dateOfBirth = $('#dateOfBirth').val();
    var gender = $('#gender').val();
    var phoneNumber = $('#phoneNumber').val();
    var address = $('#address').val();
    var profilePicture = $('#profilePicture').val();


    var user = new registerUser(username, password, passwordRecoveryQue1, passwordRecoveryQue2, passwordRecoveryQue3, passwordRecoveryAns1, passwordRecoveryAns2, passwordRecoveryAns3)

    var patient = new registerPatient(fullName, email, nationalID, dateOfBirth, gender, phoneNumber, address, profilePicture)

    console.log(user, patient);

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
                alert('successfully! ' + res.message);
            },
            error: function (err) {
                alert('failed! ' +err);
            }
        });
    }
    alert('...');
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