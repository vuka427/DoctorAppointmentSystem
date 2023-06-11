$(document).ready(function () {
    loadData();
});

/*Using AJAX to display data of DEPARTMENT TABLE*/
var table = $("#makeAppointmentTbl").DataTable({
    "ajax": {
        "responsive": true,
        "url": '/Appointment/GetData',
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
            "data": "doctorID",
            "title": "Action",
            "autoWidth": true,
            "searchable": true,
            "render": function (data, type) {
                return type === 'display' ? '<a data-doctorID="' + data + '" data-workingDay="' + data + '" id="btn-make-appointment" class="btn btn-outline-primary btn-sm ml-1 btn-action" role="button" data-toggle="tooltip" data-placement="top" title="Tooltip on top" href="@Url.Action(" Delete")" data-toggle="modal" data-target="#modal-delete-department"><i class="fa-solid fa-briefcase-medical"></i></a></div>' : data;
            }
        }
    ]
});

function loadData() {
    table.ajax.reload();
}