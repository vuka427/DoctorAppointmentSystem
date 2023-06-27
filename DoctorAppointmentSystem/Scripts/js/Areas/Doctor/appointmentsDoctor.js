


// Jquery datatable
function initJqueryDatatable() {
    var table = $('#appointment-table').DataTable({

        "sAjaxSource": "/Doctor/Appointments/LoadAppointmentData",
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
                "title": 'No.',
                "searchable": true

            },
            {
                "data": "PATIENTNAME",
                "title": 'Patient Name',
                
                "searchable": true,
                "render": function (data, type, row) {
                    return '<a href="#' + row.APPOINTMENTID + '" class="">' + data + '</a>';
                }
            },
            {
                "data": "DATEOFCONSULTANT",
                "title": 'Date Of Consultant',
                "searchable": true,
                "render": function (data, type, row) {
                    if (row.LATE) return '<div class="text-center late-appointment" data-toggle="popover" '
                        + 'data-trigger="hover" data-placement="top"  data-content="Appointment is lated">' + data + '</div>';

                    return '<div class="text-center ">' + data + '</div>';


                     
                }
            },
            {
                "data": "DATEOFCONSULTANTTIME",
                "title": 'Time',
                "searchable": true
            },

            {
                "data": "DATEOFCONSULTANTDAY",
                "title": 'Day',
                "searchable": true
            },
            {
                "data": "CONSULTANTTIME",
                "title": 'Consultant Time',
                "searchable": true
            } ,
            {
                "data": "APPOIMENTSTATUS",
                "title": 'Status',
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
            }
        ]

    });

    $(window).trigger('resize');

}

function setEventHover() {
    var table = $('#appointment-table').DataTable();

    table.on('draw', function () {
        $('[data-toggle="popover"]').popover({
            html: true,
            placement: 'top',
            container: "body",
            delay: { "show": 300, "hide": 200 },
            trigger: 'hover',
            template: '<div class="popover fc-med-popover" role="tooltip"><div class="arrow"></div> <h3  class="popover-header"></h3><div class="popover-body"></div></div>'

        })

    });

} 


$("document").ready(function () {
 
    initJqueryDatatable();
    // Initialize popover component
    setEventHover();
   
   

});
$(document).on('click', '#btnGoToDashboard', function () {
    var previousPage = document.referrer;
    console.log(previousPage);
    $('#btnGoToDashboard').attr('href', previousPage);
})