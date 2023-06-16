var table = $('#historyTbl').DataTable({
    responsive: true,
    columns: [
        {
            className: 'dt-control',
            orderable: false,
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
            data: 'appointmentDate',
            title: 'Date of Consultation',
            className: 'text-center',
            autoWidth: true,
            searchable: true
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
            searchable: true,
            render: function (data, type, row) {
                return type === 'display' ?
                    '<div class="d-flex justify-content-round">'
                    + '<a id="btn-view" data-appointmentid="' + data + '" class="btn btn-outline-info btn-sm btn-action" role="button" data-toggle="modal" data-target="#makeAppointmentModal">'
                    + '<i class="fa-solid fa-eye"></i></a>'
                    +'<a id="btn-cancel" data-appointmentid="' + data + '" class="btn btn-outline-danger btn-sm btn-action" role="button">'
                    + '<i class="fa-solid fa-circle-xmark"></i></a>'
                    +'</div>'
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