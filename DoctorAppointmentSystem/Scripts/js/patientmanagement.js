
 //show on off form
function setbtnonffoform() {
    $("#create-patient-page").hide();
    $("#update-patient-page").hide();
    $("#list-patient-page").show();

    //close form create
    $("#btn-close-form").on("click", function () {
        $("#create-patient-page").hide();
        $("#update-patient-page").hide();
        $("#list-patient-page").show();
     
    });
    // close from update
    $("#btn-close-form-update").on("click", function () {
        $("#create-patient-page").hide();
        $("#update-patient-page").hide();
        $("#list-patient-page").show();
       
    });
    
    //open from create
    $("#btn-open-form").on("click", function () {
        $("#create-patient-page").show();
        $("#update-patient-page").hide();
        $("#list-patient-page").hide();
       
    });
    //open from update
    $(".btn-update-patient").on("click", function () {
        $("#create-patient-page").hide();
        $("#create-patient-page").hide();
        $("#list-patient-page").show();
     

    });
   
}

//create patient
function setSubmitFormByAjax() {
    $("#form-create-patient").submit(function (event) {
      
        var formData = {

            PATIENTNAME: $("#patientname").val() ,
            USERNAME: $("#username").val(),
            PASSWORD: $("#password").val(),
            EMAIL: $("#email").val(), 
            PATIENTNATIONALID: $("#nationalid").val(),
            PATIENTGENDER: $("#gender").val(),
            PATIENTMOBILENO: $("#mobile").val(),
            PATIENTDATEOFBIRTH: $("#brithofdate").val(),
            PATIENTADDRESS: $("#address").val(),

           
        };
        console.log("Create =>patient");
        $.ajax({
            type: "POST",
            url: "/Admin/PatientManage/CreatePatient",
            data: formData,
            dataType: "json",
            encode: true,
        }).done(function (data) {
            console.log(data);
            if (data.error == 1) {
                //showMessage(data.msg, "Error !");
                Swal.fire(
                    'Failed!',
                    data.msg,
                    'error'
                )
            }
            if (data.error == 0) {
                
                $('#PatientTable').DataTable().ajax.reload();
                $("#create-patient-page").hide();
                $("#list-patient-page").show();
                //showMessage("Create patient is success ", "Success !")
                Swal.fire(
                    'Success!',
                    'Create patient is success !',
                    'success'
                )

            }
        });

        event.preventDefault();
    });
}

//show update from
function SetEventUpdatePatientFoBtn() {
    var table = $('#PatientTable').DataTable();

    table.on('draw', function () {

        $(".btn-update-patient").on("click", function () {
            $("#create-patient-page").hide();
            $("#list-patient-page").hide();
            $("#update-patient-page").show();

            var Button = $(this);
            var id = Button.data("id");
            console.log(id);
            LoadDataToForm(id);

        });

    });

}

//load data to form update
function LoadDataToForm(patientid) {

    var formData = {
        PatientId: patientid,
    };

    $.ajax({
        type: "POST",
        url: "/Admin/PatientManage/LoadPatient",
        data: formData,
        dataType: "json",
        encode: true,
    }).done(function (data) {
        console.log(data);
        if (data.error == 1) {
            //showMessageFormUpdate(data.msg);
            Swal.fire(
                'Failed!',
                data.msg,
                'error'
            )
        }
        if (data.error == 0) {

            console.log(data.doctor);

            $("#upatientid").val(data.patient.PATIENTID);
            $("#upatientname").val(data.patient.PATIENTNAME);
            $("#uusername").val(data.patient.USERNAME);
            $("#unationalid").val(data.patient.PATIENTNATIONALID);
            $("#uaddress").val(data.patient.PATIENTADDRESS);
            $("#ubrithofdate").val(data.patient.PATIENTDATEOFBIRTH);
            $("#umobile").val(data.patient.PATIENTMOBILENO);
            $("[name=ugender]").val([data.patient.PATIENTGENDER]);
            $("#uemail").val(data.patient.EMAIL);


        }
    });


}

//Update patient
function setSubmitFormUdateByAjax() {
    $("#form-update-patient").submit(function (event) {

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
            showCancelButton: true,
            confirmButtonText: 'Yes, update it!',
            cancelButtonText: 'No, cancel!',
            reverseButtons: false
        }).then((result) => {
            if (result.isConfirmed) {

                var formData = {

                    PATIENTID: $("#upatientid").val(),
                    PATIENTNAME: $("#upatientname").val(),
                    EMAIL: $("#uemail").val(),
                    PATIENTNATIONALID: $("#unationalid").val(),
                    PATIENTGENDER: $("#ugender").val(),
                    PATIENTMOBILENO: $("#umobile").val(),
                    PATIENTDATEOFBIRTH: $("#ubrithofdate").val(),
                    PATIENTADDRESS: $("#uaddress").val(),
                };

                $.ajax({
                    type: "POST",
                    url: "/Admin/PatientManage/UpdatePatient",
                    data: formData,
                    dataType: "json",
                    encode: true,
                }).done(function (data) {
                    console.log(data);
                    if (data.error == 1) {

                        //showMessage(data.msg, "Error !");
                        Swal.fire(
                            'Failed!',
                            data.msg,
                            'error'
                        )
                    }
                    if (data.error == 0) {
                        $('#PatientTable').DataTable().ajax.reload();

                        $("#create-patient-page").hide();
                        $("#update-patient-page").hide();
                        $("#list-patient-page").show();
                        // showMessage("Update patient is success ", "Success !")

                        Swal.fire(
                            'Success!',
                            'Update patient is success!',
                            'success'
                        )

                    }
                });



            } else if (
                /* Read more about handling dismissals below */
                result.dismiss === Swal.DismissReason.cancel
            ) {

            }
        })
        
       

        event.preventDefault();
    });
}

//show delete dialog
function SetEventDeletePatientFoBtn() {
    var table = $('#PatientTable').DataTable();

    table.on('draw', function () {

        $(".btn-delete-patient").on("click", function () {
            var Button = $(this);
            var id = Button.data("id");
            console.log("db=>" + id);

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
                showCancelButton: true,
                confirmButtonText: 'Yes, delete it!',
                cancelButtonText: 'No, cancel!',
                reverseButtons: false
            }).then((result) => {
                if (result.isConfirmed) {

                    DeletePatientByAjax(id);// delete patient by id

                } else if (
                    /* Read more about handling dismissals below */
                    result.dismiss === Swal.DismissReason.cancel
                ) {
                    
                }
            })
        });
    });
    
}


// delete patient
function DeletePatientByAjax(patientid) {

    var formData = {
        PatientId: patientid,
    };

    $.ajax({
        type: "POST",
        url: "/Admin/PatientManage/DeletePatient",
        data: formData,
        dataType: "json",
        encode: true,
    }).done(function (data) {
        console.log(data);
        if (data.error == 1) {
            Swal.fire(
                'Failed!',
                data.msg,
                'error'
            )
        }
        if (data.error == 0) {
            $('#PatientTable').DataTable().ajax.reload();
            //showMessage("Delete patient  is success ", "Success !")
            Swal.fire(
                'Deleted!',
                'Delete Patient is success!',
                'success'
            )
        }
    });
}


//show message create from
function showMessage(msg, title) {
    $("#messageModalLabel").text(title);
    $("#messageContent").text(msg);
    $('#messageModal').modal('show');

    console.log(msg);
    document.body.scrollTop = 0; // For Safari
    document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera


}

// Jquery datatable
function initJqueryDatatable() {
    var table = $('#PatientTable').DataTable({
        
        "sAjaxSource": "/Admin/PatientManage/LoadPatientData",
        "sServerMethod": "POST",
        "bServerSide": true,
        "bProcessing": true,
        "responsive": true,
        "bSearchable": true,
        "order": [[1, 'asc']],
        "language": {
            "emptyTable": "No record found.",
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
                "data": "PATIENTID",
                "title": 'ID',

                "searchable": true

            },
            {
                "data": "PATIENTNAME",
                "title": "Name",

                "searchable": true

            },
            {
                "data": "USERNAME",
                "title": "User Name",

                "searchable": true

            },
           
            {
                "data": "PATIENTGENDER",
                "title": "Gender",

                "searchable": true
            },
           
            {
                "data": "PATIENTDATEOFBIRTH",
                "title": "BrithDate",

                "searchable": true
            },
            {
                "data": "PATIENTNATIONALID",
                "title": "Nation ID",

                "searchable": true
            },
            {
                "data": "PATIENTMOBILENO",
                "title": "Mobile NO",

                "searchable": true
            },
            {
                "data": "EMAIL",
                "title": "Email",

                "searchable": true
            },
            {
                "data": "PATIENTADDRESS",
                "title": "Address",

                "searchable": true
            },
            
            {
                "data": "LOGINLOCKDATE",
                "title": "Login lock date",

                "searchable": true
            },
            {
                "data": "LOGINRETRYCOUNT",
                "title": "Login retry count",

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
            },
            {
                "data": null,
                "title": "Task",
                "responsivePriority": 1,
                "searchable": true,
                "render": function (data, type, row) {
                    console.log(data, type, row);
                    return "<btn class=\"btn-update-patient btn btn-sm btn-primary \" data-id=\"" + row.PATIENTID + "\" data-partientname=\"" + row.PATIENTNAME + "\"> Edit </btn>"
                        + "<btn class=\"btn-delete-patient btn btn-sm btn-danger ml-2\" data-id=\"" + row.PATIENTID + "\" data-partientname=\"" + row.PATIENTNAME + "\" > Delete </btn> "
                        
                }

            },
        ]

    });

    $(window).trigger('resize');

}

// show hihe pass
function ShowPass() {
    
      
        $(".paswd-on-off").each(function () {

             var inp = this;

            if (inp.type == "password"){
                inp.type = "text";
            }
            else {
                inp.type = "password";
            }

         });

    }
        

        


$("document").ready(function () {
    setbtnonffoform();
    setSubmitFormByAjax();
    setSubmitFormUdateByAjax();
    initJqueryDatatable();
    SetEventUpdatePatientFoBtn();
    SetEventDeletePatientFoBtn();
    
});