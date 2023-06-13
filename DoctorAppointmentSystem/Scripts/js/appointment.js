/*---------------------------------------------------------------------------------------------------*/
/*                                     Schedule of Doctors                                           */
/*---------------------------------------------------------------------------------------------------*/

$(document).ready(function () {
    loadSchedule();
});

/*Using AJAX to display data of DEPARTMENT TABLE*/
var table = $("#scheduleTbl").DataTable({
    "ajax": {
        "responsive": true,
        "url": '/Appointment/LoadSchedules',
        "type": "GET",
        "datatype": "json"
    },
    "columns": [
        {
            "className": 'dt-control',
            "orderable": false,
            "data": null,
            "defaultContent": '',
        },
        {
            "data": "doctorName",
            "title": 'Doctor Name',
            "autoWidth": true,
            "searchable": true
        },
        {
            "data": "speciality",
            "title": 'Speciality',
            "autoWidth": true,
            "searchable": true
        },
        {
            "data": "gender",
            "title": 'Gender',
            "autoWidth": true,
            "searchable": true
        },
        {
            "data": "workingDay",
            "title": 'Appointment Day',
            "autoWidth": true,
            "searchable": true
        },
        {
            "data": "availableTime",
            "title": 'Available Time',
            "autoWidth": true,
            "searchable": true
        },
        {
            "data": "consultantTime",
            "title": 'Consultant Time',
            "autoWidth": true,
            "searchable": true
        },
        {
            "responsivePriority": 1,
            "data": "scheduleID",
            "title": "Action",
            "autoWidth": true,
            "searchable": true,
            "render": function (data, type, row) {
                return type === 'display' ? '<a data-doctorid="' + row.doctorID + '" data-scheduleid="' + data + '" class="btn btn-outline-primary btn-sm ml-1 btn-appointment" role="button" data-toggle="modal" data-target="#makeAppointmentModal"><i class="fa-regular fa-calendar-plus"></i></div>' : data;
            }
        }
    ]
});

// Get schedule of doctors to display on screen
function loadSchedule() {
    table.ajax.reload();
}


/*---------------------------------------------------------------------------------------------------*/
/*                                           Appointment                                             */
/*---------------------------------------------------------------------------------------------------*/

// Get doctorID and scheduleID to handle
var selectedDoctorID = 0;
var selectedScheduleID = 0;
$(document).on('click', '.btn-appointment', function () {
    var btnAppointment = $(this);
    selectedDoctorID = btnAppointment.data('doctorid');
    selectedScheduleID = btnAppointment.data('scheduleid');
    loadAppointment(selectedDoctorID, selectedScheduleID)
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
            console.log(res);
        },
        error: function (err) {
            console.log(err.responseText);
        }
    })
}
