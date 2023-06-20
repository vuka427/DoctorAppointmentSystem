
var table = $('#historyTbl').DataTable({
    language: {
        emptyTable: "You haven't booked any appointments yet."
    },
    order: [[1, 'asc']],
    columns: [
        {
            orderable: false,
            className: 'dt-control',
            data: null,
            defaultContent: '',
        },
        {
            data: 'appointmentID',
            title: 'Appt No.',
            autoWidth: true,
            searchable: true
        },
        {
            data: 'doctorName',
            title: 'Doctor Name',
            className: 'text-nowrap',
            autoWidth: true,
            searchable: true
        },
        {
            data: 'dateOfConsultation',
            title: 'Date of Consultation',
            className: 'text-center',
            autoWidth: true,
            searchable: true,
            orderData: [3]
        },
        {
            data: 'consultationTime',
            title: 'Time',
            autoWidth: true,
            searchable: true
        },
        {
            data: 'consultationDay',
            title: 'Day',
            autoWidth: true,
            searchable: true
        },
        {
            data: 'appointmentStatus',
            title: 'Status',
            className: 'text-center',
            autoWidth: true,
            searchable: true,
            render: function (data, type, row) {
                return '<span class="column-status">' + data + '</span>';
            }
        },
        {
            responsivePriority: 1,
            data: 'appointmentID',
            title: "Action",
            autoWidth: true,
            orderable: false,
            searchable: true,
            render: function (data, type, row) {
                return type === 'display' ?
                    '<div class="d-flex justify-content-round">'
                    + '<a data-appointmentid="' + data + '" class="btn btn-outline-info btn-sm btn-action btn-viewAppt" role="button" data-toggle="modal" data-target="#makeAppointmentModal">'
                    + '<i class="fa-solid fa-eye"></i></a>'
                    + '<a data-appointmentid="' + data + '" class="btn btn-outline-danger btn-sm btn-action btn-cancelAppt" role="button">'
                    + '<i class="fa-solid fa-circle-xmark"></i></a>'
                    + '</div>'
                    : data;
            }
        }
    ]
});

function loadData() {
    $.ajax({
        url: '/Appointment/ViewHistory',
        method: 'GET',
        dataType: 'JSON',
        success: function (res) {
            table.clear(res.data).draw();
            table.rows.add(res.data).draw();
            styleForStatus();
        },
        error: function (err) {
            console.log(err.responseText)
        }
    })
}

$(document).ready(function () {
    loadData();
    table.on('draw', function () {
        styleForStatus();
    });
})


function styleForStatus() {
    var elements = document.getElementsByClassName("column-status");
    for (var element of elements) {
        if (element.textContent.toLowerCase() == 'pending') {
            element.classList.add("column-status--pending");
        } else if (element.textContent.toLowerCase() == 'in process') {
            element.classList.add("column-status--process");
        } else if (element.textContent.toLowerCase() == 'completed') {
            element.classList.add("column-status--completed");
        } else {
            element.classList.add("column-status--cancel");
        }
    }
}

$(document).on('click', '.btn-viewAppt', function () {
    var btnViewAppt = $(this);
    var appointmentID = btnViewAppt.data('appointmentid');
    $.ajax({
        url: '/Appointment/ViewAppointment',
        method: 'GET',
        data: { appointmentID: appointmentID },
        dataType: 'JSON',
        success: function (res) {
            var data = res.data;
            console.log(res.data);
            $('#doctorName').text(data.doctorName);
            $('#doctorGender').text(data.doctorGender);
            $('#speciality').text(data.doctorSpeciality);
            $('#patientName').text(data.patientName);
            $('#dateOfBirth').text(data.patientDateOfBirth);
            $('#patientGender').text(data.patientGender);
            $('#modeOfConsultant').val(data.modeOfConsultant);
            $('#consultantType').val(data.consultantType);
            $('#dateOfConsultation').text(data.dateOfConsultation);
            $('#consultationTime').text(data.consultationTime);
            $('#appointmentDate').text(data.appointmentDate);
            $('#appointmentTime').text(data.appointmentTime);
            $('#symtoms').val(data.symtoms);
            $('#existingIllness').val(data.existingIllness);
            $('#drugAlergies').val(data.drugAlergies);
        },
        error: function (err) {
            console.log(err.responseText);
        }
    })
})

$(document).on('click', '.btn-cancelAppt', function () {
    var btnCancelAppt = $(this);
    var appointmentID = btnCancelAppt.data('appointmentid');
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success mr-2',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })
    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        position: 'top',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, cancel it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Appointment/CancelAppointment',
                method: 'POST',
                data: { appointmentID: appointmentID },
                dataType: 'JSON',
                success: function (res) {
                    if (res.success) {
                        Swal.fire({
                            title: 'Cancelled!',
                            text: 'Your appointment has been cancelled.',
                            icon: 'success',
                            position: 'top',
                            showConfirmButton: false,
                            timer: 2000
                        })
                        loadData();
                    } else {
                        console.log(res.message)
                    }

                },
                error: function (err) {
                    console.log(err.responseText);
                }
            })
        }
    })

})