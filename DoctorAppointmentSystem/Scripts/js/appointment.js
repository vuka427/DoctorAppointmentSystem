/*---------------------------------------------------------------------------------------------------*/
/*                                           Appointment                                             */
/*---------------------------------------------------------------------------------------------------*/

// It is mandatory to choose Date of Consultation and Time before choosing Doctor
function validateAppointment() {
    var dateOfConsultation = document.getElementById("dateOfConsultation").value;
    var time = document.getElementById("time").value;
    var doctorSelector = document.getElementById("doctorSelector");

    if (dateOfConsultation === "" || time === "") {
        Swal.fire({
            position: 'top',
            icon: 'error',
            title: 'Failed!',
            text: "Please select Date of Consultation and Time before choosing a doctor.",
            showConfirmButton: false,
            timer: 2000
        })
        doctorSelector.selectedIndex = 0;
    }
}


// Get a list of doctors with corresponding work schedules and load data for Doctors select list
function sendDataToServer() {
    var dateOfConsultation = document.getElementById("dateOfConsultation").value;
    var time = document.getElementById("time").value;

    if (dateOfConsultation !== "" && time !== "") {
        $.ajax({
            url: "/Appointment/GetDoctors",
            type: "GET",
            dataType: 'JSON',
            data: {
                dateOfConsultation: dateOfConsultation,
                time: time
            },
            success: function (response) {
                var data = response.data;
                var select = document.getElementById('doctorSelector');
                $("#doctorSelector option:not(:first)").remove();
                for (var item of data) {
                    var option = document.createElement('option');
                    option.value = item.doctorID;
                    option.text = item.doctorName;
                    option.dataset.scheduleid = item.scheduleID;

                    select.appendChild(option);
                }
            },
            error: function (error) {
                console.log("FAILED! ", error.responseText);
            }
        });
    }
}

var dateOfConsultationInput = document.getElementById("dateOfConsultation");
dateOfConsultationInput.addEventListener("change", sendDataToServer);

var timeInput = document.getElementById("time");
timeInput.addEventListener("change", sendDataToServer);



var doctorSelector = document.getElementById("doctorSelector");
doctorSelector.addEventListener("click", validateAppointment);

var selectedDoctorID = 0;
var selectedScheduleID = 0;

// Scroll down form content
$(document).on('change', '#doctorSelector', function () {
    var select = $(this);
    var selectedOption = select.find(':selected');
    selectedScheduleID = selectedOption.data('scheduleid');
    selectedDoctorID = selectedOption.val();
    var targetOffset = $(".modal-body").offset().top;
    $("html, body").animate({ scrollTop: targetOffset }, "slow");
    loadAppointment(selectedDoctorID, selectedScheduleID);
})

var appointmentDate = null;
var time = null;

$(document).on('change', '#dateOfConsultation', function () {
    appointmentDate = $('#dateOfConsultation').val();
    resetPTag();
    var selectTag = document.getElementById('doctorSelector');
    selectTag.selectedIndex = 0;
})

$(document).on('change', '#time', function () {
    time = $('#time').val();
    resetPTag();
    var selectTag = document.getElementById('doctorSelector');
    selectTag.selectedIndex = 0;
})

// Get data to display on appointment form
function loadAppointment(selectedDoctorID, selectedScheduleID) {
    $.ajax({
        url: '/Appointment/LoadAppointment',
        method: 'GET',
        data: {
            selectedDoctorID,
            selectedScheduleID
        },
        dataType: 'JSON',
        success: function (res) {
            $('#doctorName').text(res.data.doctorName);
            $('#doctorGender').text(res.data.doctorGender);
            $('#speciality').text(res.data.doctorSpeciality);
            $('#patientName').text(res.data.patientName);
            $('#patientGender').text(res.data.patientGender);
            $('#dateOfBirth').text(res.data.patientDateOfBirth);
            $('#appointmentDate').text(appointmentDate);
            $('#appointmentTime').text(time);
        },
        error: function (err) {
            console.log(err.responseText);
        }
    })
}

function resetInputTag() {
    var inputTag = document.getElementsByTagName('input');
    for (var item of inputTag)
        item.value = '';
}

function resetSelectTag() {
    var selectTag = document.getElementsByTagName('select');
    for (var item of selectTag)
        item.selectedIndex = 0;
}

function resetPTag() {
    var pTag = document.getElementsByTagName('p');
    for (var item of pTag)
        item.textContent = ''
}

$('#btnCancel').click(function () {
    resetInputTag();
    resetSelectTag();
    resetPTag();
})

$('#appointmentForm').submit(function (event) {
    event.preventDefault();

    if ($('#appointmentForm').valid()) {
        if ($('#doctorName').text().trim() == null) {
            Swal.fire({
                position: 'top',
                icon: 'error',
                title: 'Failed!',
                text: 'Please choose a doctor before making an appointment.',
                showConfirmButton: false,
            })
        } else {
            var data = {
                doctorID: selectedDoctorID,
                scheduleID: selectedScheduleID,
                consultantType: $('#consultantType').val(),
                modeOfConsultant: $('#modeOfConsultant').val(),
                symtoms: $('#symtoms').val(),
                existingIllness: $('#existingIllness').val(),
                drugAllargies: $('#drugAlergies').val(),
                appointmentDate: $('#appointmentDate').text(),
                appointmentTime: $('#appointmentTime').text()
            }

            $.ajax({
                url: '/Appointment/MakeAppointmentAPI',
                method: 'POST',
                data: data,
                dataType: 'JSON',
                success: function (res) {
                    if (res.success) {
                        Swal.fire({
                            position: 'top',
                            icon: 'success',
                            title: 'Congratulations!',
                            text: res.message,
                            showConfirmButton: false,
                            timer: 2000
                        })
                        // Reset all input tag
                        resetInputTag()

                        // Reset all select tag
                        resetSelectTag()

                        // Reset all p tag
                        resetPTag()
                        var pTag = document.getElementsByTagName('p');
                        for (var item of pTag)
                            item.textContent = ''
                    } else {
                        Swal.fire({
                            position: 'top',
                            icon: 'error',
                            title: 'Failed!',
                            text: res.message,
                            showConfirmButton: false,
                            timer: 2000
                        })
                    }
                },
                error: function (err) {
                    console.log(err.responseText);
                }
            })
        }
    } else {
        Swal.fire({
            position: 'top',
            icon: 'error',
            title: 'Failed!',
            text: 'Please choose mode of consultant and type of consultant.',
            showConfirmButton: false,
            timer: 2000
        })
    }
})


/*---------------------------------------------------------------------------------------------------*/
/*                                        Validation Data                                            */
/*---------------------------------------------------------------------------------------------------*/

var validateData = function () {
    $('#appointmentForm').validate({
        onfocusout: function (element) {
            $(element).valid()
        },
        rules: {
            "consultantType": {
                required: true
            },
            "modeOfConsultant": {
                required: true
            }
        },
        messages: {
            "consultantType": {
                required: "Please choose mode of consultant."
            },
            "modeOfConsultant": {
                required: "Please choose mode of consultant."
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
    validateData();
})