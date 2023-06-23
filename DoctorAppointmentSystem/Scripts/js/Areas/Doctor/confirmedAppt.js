var table = $('#confirmedApptForm').DataTable({
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
            data: 'patientName',
            title: 'Patient Name',
            className: 'text-nowrap',
            autoWidth: true,
            searchable: true,
            render: function (data, type, row) {
                return '<a class="btn-viewAppt" href="/Doctor/ConfirmedAppt/MemberDetails?id=' + row.appointmentID + '">' + data + '</a>';
            }
        },
        {
            data: 'patientGender',
            title: 'Gender',
            autoWidth: true,
            searchable: true,
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
})