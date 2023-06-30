﻿var table = $('#confirmedApptForm').DataTable({
    language: {
        emptyTable: "You haven't booked any appointments yet."
    },
    order: [[1, 'asc']],
    responsive: true,
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
            data: 'patientName',
            title: 'Patient Name',
            className: 'text-nowrap',
            autoWidth: true,
            searchable: true,
            render: function (data, type, row) {
                return '<a class="btn-viewAppt" href="/Doctor/ConfirmedAppt/AppointmentDetails?id=' + row.appointmentID + '">' + data + '</a>';
            }
        },
        {
            data: 'dateOfConsultation',
            title: 'Consultation Date',
            className: 'text-center',
            autoWidth: true,
            searchable: true,
        },
        {
            data: 'appointmentDate',
            title: 'Appointment Date',
            autoWidth: true,
            searchable: true,
            render: function (data, type, row) {
                var today = new Date();
                var apptDate = new Date(data)
                if (apptDate < today) {
                    return '<div class="text-center text-light" style="background-color: #997473"  data-toggle="popover" '
                        + 'data-trigger="hover" data-placement="top"  data-content="Appointment time exceeded.">' + data + '</div>';
                }
                return '<div class="text-center ">' + data + '</div>';
            }
        },
        {
            data: 'appointmentTime',
            title: 'Time',
            autoWidth: true,
            searchable: true
        },
        {
            data: 'appointmentDay',
            title: 'Day',
            autoWidth: true,
            searchable: true
        },
        {
            responsivePriority: 1,
            data: 'appointmentStatus',
            title: 'Status',
            className: 'text-center',
            autoWidth: true,
            searchable: true,
            render: function (data, type, row) {
                return '<span class="column-status">' + data + '</span>';
            }
        },
    ]
})

function loadData() {
    $.ajax({
        url: '/Doctor/ConfirmedAppt/LoadData',
        method: 'GET',
        data: {status: 'confirm'},
        dataType: 'JSON',
        success: function (res) {
            if (res.success) {
                table.clear(res.data).draw();
                table.rows.add(res.data).draw();
                styleForStatus();
            }
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
        } else if (element.textContent.toLowerCase() == 'confirm') {
            element.classList.add("column-status--confirm");
        } else if (element.textContent.toLowerCase() == 'completed') {
            element.classList.add("column-status--completed");
        } else {
            element.classList.add("column-status--cancel");
        }
    }
}

$(document).on('click', '.btn-viewAppt', function (event) {

    var btnViewAppt = $(this);
    var appointmentID = btnViewAppt.data('appointmentid');
    var status = btnViewAppt.data('status');
    var previousPage = document.referrer;
    $('#btnGoToDashboard').attr('href', previousPage);
})
$(document).on('click', '#btnGoToDashboard', function () {
    var previousPage = document.referrer;
    console.log(previousPage);
    $('#btnGoToDashboard').attr('href', previousPage);
})