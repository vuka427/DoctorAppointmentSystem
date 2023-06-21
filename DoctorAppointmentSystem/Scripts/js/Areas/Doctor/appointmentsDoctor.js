


// Jquery datatable
function initJqueryDatatable() {
    var table = $('#appointment-table').DataTable({

        "sAjaxSource": "/Admin/AppointmentManage/LoadAppointmentData",
        "sServerMethod": "POST",
        "bServerSide": true,
        "bProcessing": true,
        "responsive": true,
        "bSearchable": true,
        "order": [[1, 'asc']],
        "language": {
            "emptyTable": "There are no appointments in the list.",
            "processing":
                '<i class="fa fa-spinner fa-spin fa-3x fa-fw" style="color:#2a2b2b;"></i><span class="sr-only">Loading...</span> '
        },
        "columns": [
            {
                "className": 'dt-control',
                "orderable": false,
                "data": null,
                "defaultContent": '',
            },
            {
                "data": "APPOINTMENTID",
                "title": 'Appointment ID',
                "searchable": true

            },
            {
                "data": "PATIENTNAME",
                "title": 'Patient Name',
                "searchable": true

            },
            {
                "data": "DOCTORNAME",
                "title": 'Doctor Name',
                "searchable": true

            },
            {
                "data": "APPOINTMENTDATE",
                "title": 'Appointment Date',
                "searchable": true

            },
            {
                "data": "APPOINTMENTTIME",
                "title": 'Appointment Time',
                "searchable": true
            },

            {
                "data": "APPOINTMENTDAY",
                "title": 'Appointment Day',
                "searchable": true
            },
            {
                "data": "CONSULTANTTIME",
                "title": 'Consultant Time',
                "searchable": true
            },
            {
                "data": "CLOSEDBY",
                "title": 'Closed By',
                "searchable": true
            },
            {
                "data": "CLOSEDDATE",
                "title": 'Closed Date',
                "searchable": true
            },
            {
                "data": "CREATEDBY",
                "title": "Create By",
                "searchable": true
            },
            {
                "data": "CREATEDDATE",
                "title": "Create Date",
                "searchable": true
            },
            {
                "data": "UPDATEDBY",
                "title": "Update By",
                "searchable": true
            },
            {
                "data": "UPDATEDDATE",
                "title": "Update Date",
                "searchable": true
            }
            ,
            {
                "data": "APPOIMENTSTATUS",
                "title": 'Appointment Status',
                "className": 'text-center',
                "responsivePriority": 1,
                "searchable": true,
                "render": function (data, type, row) {
                    if (data.toLowerCase() == 'pending') {
                        return '<span class="column-status column-status--pending">' + data + '</span>';

                    } else if (data.toLowerCase() == 'in process') {
                        return '<span class="column-status column-status--process">' + data + '</span>';

                    } else if (data.toLowerCase() == 'completed') {
                        return '<span class="column-status column-status--completed">' + data + '</span>';

                    } else {
                        return '<span class="column-status column-status--cancel">' + data + '</span>';

                    }

                }
            },
            {
                "data": null,
                "title": "Action",
                "responsivePriority": 1,
                "searchable": false,
                "render": function (data, type, row) {
                    console.log(data, type, row);
                    return "<btn class=\"btn-view-appointment btn btn-sm btn-outline-primary btn-action\" data-appointmentid=\"" + row.APPOINTMENTID + "\"  data-toggle=\"tooltip\" data-placement=\"top\" title=\"View Appointment\"   > <i class=\"fa-solid fa-eye\"></i> </btn>"
                        + "<btn class=\"btn-delete-appointment btn btn-sm btn-outline-danger btn-action  ml-2\" data-id=\"" + row.APPOINTMENTID + "\" data-patientname=\"" + row.PATIENTNAME + "\"   data-toggle=\"tooltip\" data-placement=\"top\" title=\"Delete Appointment\"> <i class=\"fa-solid fa-trash\"></i> </btn> "

                }

            },
        ]

    });

    $(window).trigger('resize');

}

$("document").ready(function () {
 
    initJqueryDatatable();
   

});