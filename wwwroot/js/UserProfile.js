document.addEventListener('DOMContentLoaded', function () {
    fnLoadData();
});



function fnLoadData() {

    /*  alert("Hi");*/
    $.ajax({
        type: "POST",
        url: "/Registration/LoadUserProfile",
        data: {},
        success: function (obj) {

            fnLoadUserProfileGrid(obj.dataList);
            // console.log(obj.dataList);
            //$('#divAddEditElements').hide();
            //$('#divDataMaster').show();

        },
        error: function () {

        }
    });
}
function fnLoadUserProfileGrid(DataList) {
    //console.log(DataList);
    $('#tblRegistration').empty();
    $('#tblRegistration').dataTable({
        "pageLength": 10,
        "Processing": true,
        "destroy": true,
        "aaData": DataList,
        "columns": [
            //{
            //    "data": "flowerID", "width": "50%", "title": "Action to Perform", "class": "text-center", "render": function (data) {
            //        return ` <button type="button" class="dt-btn-approve btn-primary" onclick="fnEditFlowerData('` + data.toString() + `')">Edit | </button>
            //          <button type="button" class="dt-btn-reject btn-danger"  onclick="fnDeleteFlowerData('` + data.toString() + `')">Delete</button>`
            //    }
            //},
            { "data": "username", "title": "UserName", "width": "50px" },
            { "data": "mobileno", "title": "MobileNo", "width": "70px" },
            { "data": "emailID", "title": "EmailId", "width": "70px" },
            { "data": "gender", "title": "Gender", "width": "70px" },
            {
                "data": "birthdate", "title": "Birthdate", "width": "50px",
                "render": function (data, type, row) {
                if (data) {
                      
                        var birthdate = new Date(data);
                        
                        var day = birthdate.getDate();
                        var monthIndex = birthdate.getMonth();
                        var year = birthdate.getFullYear();
                        
                        var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
                       
                        return (day < 10 ? '0' : '') + day + '-' + monthNames[monthIndex] + '-' + year;
                    } else {
                        // If the data is null or undefined, return an empty string
                        return "";
                    }
                }
            },
            { "data": "country", "title": "Country", "width": "70px" },
            { "data": "state", "title": "State", "width": "70px" },
            { "data": "city", "title": "City", "width": "70px" },
            { "data": "address", "title": "Address", "width": "70px" },
            {
                "data": "createdOn", "title": "CreatedOn", "width": "70px",
                "render": function (data, type, row) {
                    if (data) {

                        var createdOn = new Date(data);

                        var day = createdOn.getDate();
                        var monthIndex = createdOn.getMonth();
                        var year = createdOn.getFullYear();

                        var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

                        return (day < 10 ? '0' : '') + day + '-' + monthNames[monthIndex] + '-' + year;
                    } else {
                        // If the data is null or undefined, return an empty string
                        return "";
                    }
                }            },

        ]
    });
    $('#tblRegistration_wrapper').find(".row :first").prop("style", "margin-bottom:2%");
    $('#tblRegistration_wrapper').find("select[name='tblRegistration_length']").prop("style", "margin-top:4% ;width:35% !important;");
}

function fnLoadUserProfileDetails() {
    var id = $("#RegistrationID").attr('value');
    if (id != "") {
        $.ajax({
            type: "POST",
            url: "/Registration/LoadUserProfileDetails",
            data: { id },
            success: function (data) {

                if (!data.isSuccess) {
                    $('._CustomMessage').text(data.message);
                    $('#errorPopup').modal('show');
                }
                else {
                    alert(data.Registration.emailID);
                    $("#Username").val(data.Registration.username);
                    $("#Mobileno").val(data.Registration.mobileNo);
                    $("#EmailID").val(data.Registration.emailID);
                    $("#Gender").val(data.Registration.gender);
                    $("#Birthdate").val(data.Registration.birthdate);
                    $("#Country").text(data.Registration.country);
                    $("#State").text(data.Registration.state);
                    $("#City").text(data.Registration.city);
                    $("#Address").val(data.Registration.adress);
                    $("#CreatedOn").val(data.Registration.createdon);
                   
                }
            },
            error: function () {

            }
        });
    }
    else {
        $('._CustomMessage').text("Please select Flower!");
        $('#errorPopup').modal('show');
    }
}