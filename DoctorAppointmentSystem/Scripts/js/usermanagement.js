'use strict'
 //show on off form
function setButtonOnOffForm() {
    $("#reset-password-page").hide();
    $("#table-list-user").show();

    
    // close from reset password
    $("#btn-close-form-reset-password").on("click", function () {
        $("#reset-password-page").hide();
        $("#table-list-user").show();
    });
  
    //open from reset password
    $(".btn-reset-password-user").on("click", function () { 
        $("#reset-password-page").show();
        $("#table-list-user").hide();
    });
}


//reset password
function setSubmitFormResetPasswordByAjax() {
    $("#form-reset-password-user").submit(function (event) {
        if ($("#form-reset-password-user").valid()) {
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
                confirmButtonText: 'Yes, reset password!',
                cancelButtonText: 'No, cancel!',
                reverseButtons: false
            }).then((result) => {
                if (result.isConfirmed) {

                    var formData = {
                        USERID: $("#userid").val(),
                        PASSWORD: $("#password").val(),
                    };
                    
                    $.ajax({
                        type: "POST",
                        url: "/Admin/UserManage/ResetPassword",
                        data: formData,
                        dataType: "json",
                        encode: true,
                    }).done(function (data) {
                        
                        if (data.error == 1) {
                            $("#form-reset-password-user").trigger('reset');
                            //showMessage(data.msg, "Error !");
                            Swal.fire(
                                'Failed!',
                                data.msg,
                                'error'
                            )
                        }
                        if (data.error == 0) {
                            $('#DoctorTable').DataTable().ajax.reload();

                            $("#reset-password-page").hide();
                            $("#table-list-user").show();
                            //showMessage("Update doctor is success ", "Success !")
                            $("#form-reset-password-user").trigger('reset');
                            Swal.fire(
                                'Success!',
                                'Reset password for user is success !',
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
        }


        event.preventDefault();
    });
}

//set event show form reset password
function setEventResetPasswordForBtn() {
    var table = $('#UserTable').DataTable();

    table.on('draw', function () {

        $(".btn-reset-password-user").on("click", function () {
            $("#reset-password-page").show();
            $("#table-list-user").hide();

            var Button = $(this);
            var id = Button.data("id");
            $("#userid").val(id);
            
        });

       
    });

}

//delete dialog
function setEventDeleteUserForBtn() {
    var table = $('#UserTable').DataTable();

    table.on('draw', function () {

        $(".btn-delete-user").on("click", function () {

            var Button = $(this);
            var username = Button.data("username");
            var id = Button.data("id");
            

            const swalWithBootstrapButtons = Swal.mixin({
                customClass: {
                    confirmButton: 'btn btn-success mr-2',
                    cancelButton: 'btn btn-danger'
                },
                buttonsStyling: false
            })
            swalWithBootstrapButtons.fire({
                title: 'Are you sure delete user ' + username + ' ?',
                text: "You won't be able to revert this!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes, delete it!',
                cancelButtonText: 'No, cancel!',
                reverseButtons: false
            }).then((result) => {
                if (result.isConfirmed) {

                    DeleteUserByAjax(id) // delete user by  id
                    

                } else if (
                    /* Read more about handling dismissals below */
                    result.dismiss === Swal.DismissReason.cancel
                ) {

                }
            })
        });
    });
    
    

}

//delete user
function deleteUserByAjax(doctorid) {

    var formData = {
        USERID: doctorid,
    };

    $.ajax({
        type: "POST",
        url: "/Admin/UserManage/DeleteUser",
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
            $('#UserTable').DataTable().ajax.reload();
            //showMessage("Delete doctor is success ", "Success !")
           Swal.fire(
                'Deleted!',
                'Delete doctor is success !',
                'success'
            )
        }
    });
}


// Jquery datatable
function initJqueryDatatable() {
    var table = $('#UserTable').DataTable({
        
        "sAjaxSource": "/Admin/UserManage/LoadUserData",
        "sServerMethod": "POST",
        "bServerSide": true,
        "bProcessing": true,
        "responsive": true,
        "bSearchable": true,
        "order": [[1, 'asc']],
        "language": {
            "emptyTable": "There are no doctors in the list.",
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
                "data": "USERID",
                "title": 'ID',
                "searchable": true

            },
            {
                "data": "FULLNAME",
                "title": "Full name",
                "searchable": true

            },
            {
                "data": "USERNAME",
                "title": "User Name",

                "searchable": true

            },
            {
                "data": "USERTYPE",
                "title": 'User Type',
                "searchable": true

            },
           
            {
                "data": "GENDER",
                "title": "Gender",

                "searchable": true
            },
           
            {
                "data": "MOBILENO",
                "title": "Mobile NO",

                "searchable": true
            },
            {
                "data": "EMAIL",
                "title": "Email",

                "searchable": true
            },
            {
                "data": "LASTLOGIN",
                "title": "Last login",

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
                    return "<btn class=\"btn-reset-password-user btn btn-sm btn-success \" data-id=\"" + row.USERID + "\" data-username=\"" + row.USERNAME + "\"> Reset password </btn>"
                        + "<btn class=\"btn-delete-user btn btn-sm btn-danger ml-2\" data-id=\"" + row.USERID + "\" data-username=\"" + row.USERNAME + "\"> Delete </btn> "
                        
                }

            },
        ]

    });

    $(window).trigger('resize');

}

// show hihe pass
function showPass() {
    
      
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

//Validate form
function validateFormResetPassword() {
    

    jQuery.validator.addMethod('valid_password', function (value) {
        var regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,50}$/;
        return value.trim().match(regex);
    });
    

    //Validate reset password
    $("#form-reset-password-user").validate({ 
        onfocusout: function (element) {
            $(element).valid()
        },
        rules: {
            "password": {
                required: true,
                maxlength: 50,
                minlength: 6,
                valid_password: true

            },
            "passwordconfirm": {
                required: true,
                equalTo: "#password",

            }
        },
        messages: {
            "password": {
                required: "Password is required",
                maxlength: "Password charater lenght is 6 to 50!",
                minlength: "Password charater lenght is 6 to 50!",
                valid_password: "Password charater at least one uppercase letter, one lowercase letter, one number and one special character: [a-z],[A-Z],[0-9],[@$!%*?&]"
            },
             "passwordconfirm": {
                required: "Password is required",
                 equalTo: "Password does not match !",
            }
        },
        highlight: function (element) {
            var elem = $(element);
            if (elem.hasClass("select2-hidden-accessible")) {
                element = $(".select2-selection");

                element.addClass('border-2 border-danger')
            } else {
                elem.addClass('border-2 border-danger ')
            }
           
        },
        unhighlight: function (element) {
            var elem = $(element);
            if (elem.hasClass("select2-hidden-accessible")) {
                element = $(".select2-selection");

                element.removeClass('border-2 border-danger')
            } else {
                elem.removeClass('border-2 border-danger')
            }
            
        },
        errorPlacement: function (error, element) {
            var elem = $(element);
            if (elem.hasClass("select2-hidden-accessible")) {
                element = $("#department").parent();

                error.insertAfter(element);
            } else {
                error.insertAfter(element);
            }
        }
       
    });

   
}

     


$("document").ready(function () {

    setButtonOnOffForm();
    initJqueryDatatable();
    setEventResetPasswordForBtn();
    setSubmitFormResetPasswordByAjax();
    validateFormResetPassword();
    setEventDeleteUserForBtn();
   
});