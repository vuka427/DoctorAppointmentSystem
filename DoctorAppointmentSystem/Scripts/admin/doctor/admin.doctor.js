function setbtnonffoform() {
    $("#l-form-doctor").hide();
    $("#form-update-doctor").hide();
    $("#table-list-doctor").show();
    //close form create
    $("#btn-close-form").on("click", function () {
        $("#l-form-doctor").hide();
        $("#table-list-doctor").show();
        $("#form-update-doctor").hide();
    });
    // close from update
    $("#btn-close-form-update").on("click", function () {
        $("#l-form-doctor").hide();
        $("#table-list-doctor").show();
        $("#form-update-doctor").hide();
    });
    //open from create
    $("#btn-open-form").on("click", function () {
        $("#l-form-doctor").show();
        $("#table-list-doctor").hide();
        $("#form-update-doctor").hide();
    });

    $(".btn-update-doctor").on("click", function () {
        $("#l-form-doctor").hide();
        $("#table-list-doctor").hide();
        $("#form-update-doctor").show();

    });
} //show on off form
//create doctor
function setSubmitFormByAjax() {
    $("#form-create-doctor").submit(function (event) {
        var gender = $(".radio-gender:checked").val();

        var formData = {

            DOCTORNAME: $("#doctorname").val(),
            USERNAME: $("#username").val(), 
            PASSWORD: $("#password").val(),
            DOCTORNATIONALID: $("#nationalid").val(), 
            DOCTORGENDER: gender,
            DOCTORADDRESS: $("#address").val(),
            DEPARTMENTID: $("#department").val(),
            DOCTORDATEOFBIRTH: $("#brithofdate").val(),
            DOCTORMOBILENO: $("#mobile").val(),
            EMAIL: $("#email").val(), 
            SPECIALITY: $("#specialy").val(), 
            LOGINLOCKDATE: $("#loginlockdate").val(), 
            WORKINGENDDATE: $("#workingenddate").val(),
            WORKINGSTARTDATE: $("#workingstartdate").val(),
        };
        console.log($("#doctorname").val());
        $.ajax({
            type: "POST",
            url: "/Admin/DoctorManage/CreateDoctor",
            data: formData,
            dataType: "json",
            encode: true,
        }).done(function (data) {
            console.log(data);
            if (data.error == 1) { showMessage(data.msg, "Error !"); }
            if (data.error == 0) {
                //$("#form-create-doctor").trigger('reset');
                $('#DoctorTable').DataTable().ajax.reload();
                $("#l-form-doctor").hide();
                $("#table-list-doctor").show();
                showMessage("Create doctor is success ", "Success !")



            }
        });

        event.preventDefault();
    });
}
//Update doctor
function setSubmitFormUdateByAjax() {
    $("#form-edit-doctor").submit(function (event) {
        var gender = $(".uradio-gender:checked").val();

        var formData = {
            DOCTORID: $("#udoctorid").val(),
            DOCTORNAME: $("#udoctorname").val(),
            DOCTORGENDER: gender,
            DOCTORADDRESS: $("#uaddress").val(),
            DEPARTMENTID: $("#udepartment").val(),
            DOCTORDATEOFBIRTH: $("#ubrithofdate").val(),
            DOCTORMOBILENO: $("#umobile").val(),
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
//show message create from
function showMessage(msg, title) {
    $("#messageModalLabel").text(title);
    $("#messageContent").text(msg);
    $('#messageModal').modal('show');

    console.log(msg);
    document.body.scrollTop = 0; // For Safari
    document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera


}

//show delete dialog
function SetEventDeleteDoctorFoBtn() {
    var table = $('#DoctorTable').DataTable();

    table.on('draw', function () {

        $(".btn-delete-doctor").on("click", function () {
            var Button = $(this);
            var ButtonAccept = $("#btn-accept-delete-doctor");
            var doctorname = Button.data("doctorname");
            var id = Button.data("id");
            ButtonAccept.attr("data-id", id);
            $("#doctorname-delete").html(doctorname);
        });
    });

    $("#btn-accept-delete-doctor").on("click", function () {
        var Button = $(this);
        var id = Button.attr("data-id");
        DeleteDocTorByAjax(id);
    });

}
//show edit from
function SetEventUpdateDoctorFoBtn() {
    var table = $('#DoctorTable').DataTable();

    table.on('draw', function () {

        $(".btn-update-doctor").on("click", function () {
            $("#l-form-doctor").hide();
            $("#table-list-doctor").hide();
            $("#form-update-doctor").show();

            var Button = $(this);
            var id = Button.data("id");
            console.log(id);
            LoadDataToForm(id);

        });

    });

}
// load data to form edit
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
            $("#uaddress").val(data.doctor.DOCTORADDRESS);
            $("#udepartment").val(String(data.doctor.DEPARTMENTID));
            $("#ubrithofdate").val(data.doctor.DOCTORDATEOFBIRTH);
            $("#umobile").val(data.doctor.DOCTORMOBILENO);
            $("#uworkingenddate").val(data.doctor.WORKINGENDDATE);
            $("#uworkingstartdate").val(data.doctor.WORKINGSTARTDATE);

            $("[name=ugender]").val([data.doctor.DOCTORGENDER]);


        }
    });


}
// ajax delete doctor
function DeleteDocTorByAjax(doctorid) {

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

// Jquery datatable
function initJqueryDatatable() {
    var table = $('#DoctorTable').DataTable({
        
        "sAjaxSource": "/Admin/DoctorManage/LoadDoctorData",
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
                "data": "DOCTORID",
                "title": 'ID',

                "searchable": true

            },
            {
                "data": "DOCTORNAME",
                "title": "Name",

                "searchable": true

            },
            {
                "data": "USERNAME",
                "title": "User Name",

                "searchable": true

            },
            {
                "data": "DEPARTMENT",
                "title": "Deparment",

                "searchable": true
            },
            {
                "data": "DOCTORGENDER",
                "title": "Gender",

                "searchable": true
            },
           
            {
                "data": "DOCTORDATEOFBIRTH",
                "title": "BrithDate",

                "searchable": true
            },
            {
                "data": "DOCTORNATIONALID",
                "title": "Nation ID",

                "searchable": true
            },
            {
                "data": "DOCTORMOBILENO",
                "title": "Mobile NO",

                "searchable": true
            },
            {
                "data": "EMAIL",
                "title": "Email",

                "searchable": true
            },
            {
                "data": "DOCTORADDRESS",
                "title": "Address",

                "searchable": true
            },
            {
                "data": "SPECIALITY",
                "title": "Speciality",

                "searchable": true
            },
            {
                "data": "QUALIFICATION",
                "title": "Qualification",

                "searchable": true
            },
            {
                "data": "WORKINGSTARTDATE",
                "title": "Working Start Date",

                "searchable": true
            },
            {
                "data": "WORKINGENDDATE",
                "title": "Working End Date",

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
                    return "<btn class=\"btn-update-doctor btn btn-sm btn-primary \" data-id=\"" + row.DOCTORID + "\" data-doctorname=\"" + row.DOCTORNAME + "\"> Edit </btn>"
                        + "<btn class=\"btn-delete-doctor btn btn-sm btn-danger ml-2\" data-id=\"" + row.DOCTORID + "\" data-doctorname=\"" + row.DOCTORNAME + "\" data-toggle=\"modal\" data-target=\"#accept-delete-doctor\"> Delete </btn> "
                }

            },
        ]

    });
    $(window).trigger('resize');

}


$("document").ready(function () {
    setbtnonffoform();
    setSubmitFormByAjax();
    setSubmitFormUdateByAjax();
    initJqueryDatatable();
    SetEventUpdateDoctorFoBtn();
    SetEventDeleteDoctorFoBtn();
});