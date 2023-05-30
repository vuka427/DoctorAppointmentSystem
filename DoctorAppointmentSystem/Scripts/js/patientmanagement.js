
 //show on off form
function setbtnonffoform() {
    $("#create-patient-page").hide();
    $("#update-patient-page").hide();
    $("#list-patient-page").show();

    //close form create
    $("#btn-close-form").on("click", function () {
        $("#create-patient-page").hide();
        $("#update-patient-page").show();
        $("#list-patient-page").hide();
     
    });
    // close from update
    $("#btn-close-form-update").on("click", function () {
        $("#create-patient-page").hide();
        $("#update-patient-page").show();
        $("#list-patient-page").hide();
       
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
        var gender = $(".radio-gender:checked").val();

        var formData = {

            
            USERNAME: $("#username").val(), 
            PASSWORD: $("#password").val(),
           
            EMAIL: $("#email").val(), 
            SPECIALITY: $("#specialy").val(), 
            WORKINGENDDATE: $("#workingenddate").val(),
            WORKINGSTARTDATE: $("#workingstartdate").val(),
        };
       
        $.ajax({
            type: "POST",
            url: "",
            data: formData,
            dataType: "json",
            encode: true,
        }).done(function (data) {
            console.log(data);
            if (data.error == 1) { showMessage(data.msg, "Error !"); }
            if (data.error == 0) {
                
                $('#DoctorTable').DataTable().ajax.reload();
                $("#create-patient-page").hide();
                $("#list-patient-page").show();
                showMessage("Create patient is success ", "Success !")

            }
        });

        event.preventDefault();
    });
}

// load data to form update
function LoadDataToForm(doctorid) {

    var formData = {
        DoctorId: doctorid,
    };

    $.ajax({
        type: "POST",
        url: "",
        data: formData,
        dataType: "json",
        encode: true,
    }).done(function (data) {
        console.log(data);
        if (data.error == 1) { showMessageFormUpdate(data.msg); }
        if (data.error == 0) {

            console.log(data.doctor);


            $("#udoctorid").val(data.doctor.DOCTORID);
            $("#udoctorname").val(data.doctor.DOCTORNAME);
            $("#uusername").val(data.doctor.USERNAME);
            $("#upassword").val(data.doctor.PASSWORD);
            $("#unationalid").val(data.doctor.DOCTORNATIONALID);
            $("#uaddress").val(data.doctor.DOCTORADDRESS);
            $("#udepartment").val(String(data.doctor.DEPARTMENTID));
            $("#ubrithofdate").val(data.doctor.DOCTORDATEOFBIRTH);
            $("#umobile").val(data.doctor.DOCTORMOBILENO);
            $("#uworkingenddate").val(data.doctor.WORKINGENDDATE);
            $("#uworkingstartdate").val(data.doctor.WORKINGSTARTDATE);
            $("[name=ugender]").val([data.doctor.DOCTORGENDER]);
            $("#uemail").val(data.doctor.EMAIL);
            $("#uspecialy").val(data.doctor.SPECIALITY);


        }
    });


}

//Update patient
function setSubmitFormUdateByAjax() {
    $("#form-edit-doctor").submit(function (event) {
        var gender = $(".uradio-gender:checked").val();

        var formData = {
            DOCTORID: $("#udoctorid").val(),
            DOCTORNAME: $("#udoctorname").val(),
            DOCTORNATIONALID: $("#unationalid").val(),
            DOCTORGENDER: gender,
            DOCTORADDRESS: $("#uaddress").val(),
            DEPARTMENTID: $("#udepartment").val(),
            DOCTORDATEOFBIRTH: $("#ubrithofdate").val(),
            DOCTORMOBILENO: $("#umobile").val(),
            EMAIL: $("#uemail").val(),
            SPECIALITY: $("#uspecialy").val(),
            WORKINGENDDATE: $("#uworkingenddate").val(),
            WORKINGSTARTDATE: $("#uworkingstartdate").val(),


        };
        console.log($("#udoctorname").val());
        $.ajax({
            type: "POST",
            url: "",
            data: formData,
            dataType: "json",
            encode: true,
        }).done(function (data) {
            console.log(data);
            if (data.error == 1) { showMessage(data.msg, "Error !"); }
            if (data.error == 0) {
                $('#DoctorTable').DataTable().ajax.reload();

                $("#l-form-doctor").hide();
                $("#table-list-doctor").show();
                $("#form-update-doctor").hide();
                showMessage("Update doctor is success ", "Success !")

            }
        });

        event.preventDefault();
    });
}

//show delete dialog
function SetEventDeletePatientFoBtn() {
    var table = $('#DoctorTable').DataTable();

    table.on('draw', function () {

        $(".btn-delete-doctor").on("click", function () {
            var Button = $(this);
            var ButtonAccept = $("#btn-accept-delete-doctor");
            var doctorname = Button.data("doctorname");
            var id = Button.data("id");
            ButtonAccept.attr("data-id", id);
            $("#doctorname-delete").html(doctorname);
            console.log("db=>" + id);
        });
    });
    
    $("#btn-accept-delete-doctor").on("click", function () {
        var Button = $(this);
        var id = Button.attr("data-id");
        console.log("dl=>" + id);
        DeleteDocTorByAjax(id);// delete doctor by id
    });

}

// delete patient
function DeletePatientByAjax(doctorid) {

    var formData = {
        DoctorId: doctorid,
    };

    $.ajax({
        type: "POST",
        url: "",
        data: formData,
        dataType: "json",
        encode: true,
    }).done(function (data) {
        console.log(data);
        if (data.error == 1) {

        }
        if (data.error == 0) {
            $('#DoctorTable').DataTable().ajax.reload();
            showMessage("Delete doctor is success ", "Success !")
        }
    });
}

//show edit from
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
                        + "<btn class=\"btn-delete-patient btn btn-sm btn-danger ml-2\" data-id=\"" + row.PATIENTID + "\" data-partientname=\"" + row.PATIENTNAME + "\" data-toggle=\"modal\" data-target=\"#accept-delete-patient\"> Delete </btn> "
                        
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
    SetEventResetPaswdPatientFoBtn();
});