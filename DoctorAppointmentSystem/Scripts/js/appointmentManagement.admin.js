//show on off form
function setButtonOnOffForm() {
    $("#appointment-info").hide();
    $("#list-appointment-page").show();

    //close form 
    $("#btn-close-form").on("click", function () {
        $("#appointment-info").hide();
        $("#list-appointment-page").show();

    });
   
    //open from 
    $("#btn-open-form-create").on("click", function () {
        $("#appointment-info").show();
        $("#list-appointment-page").hide();

    });
    
}


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
                "responsivePriority": 1,
                "searchable": true
            },
            {
                "data": null,
                "title": "Action",
                "responsivePriority": 1,
                "searchable": false,
                "render": function (data, type, row) {
                    console.log(data, type, row);
                    return "<btn class=\"btn-update-schedule btn btn-sm btn-outline-primary btn-action\" data-id=\"" + row.SCHEDULEID + "\"  data-toggle=\"tooltip\" data-placement=\"top\" title=\"Edit schedule\"> <i class=\"fa-solid fa-user-pen\"></i> </btn>"
                        + "<btn class=\"btn-delete-schedule btn btn-sm btn-outline-danger btn-action  ml-2\" data-id=\"" + row.SCHEDULEID + "\"  data-toggle=\"tooltip\" data-placement=\"top\" title=\"Delete schedule\"> <i class=\"fa-solid fa-trash\"></i> </btn> "

                }

            },
        ]

    });

    $(window).trigger('resize');

}



$("document").ready(function () {
    setButtonOnOffForm();
    initJqueryDatatable();
    
});
