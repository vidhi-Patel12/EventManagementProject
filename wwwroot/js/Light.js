document.addEventListener('DOMContentLoaded', function () {
    $("#LightFilePath").change(function () {
        var file = this.files[0];
        if (file) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#LightImage').attr('src', e.target.result);
            }
            reader.readAsDataURL(file);
        }
    });
    fnLoadData();
});


function fnAddUpdateLight() {
    var fieldsFilled = false;
    if (!fieldsFilled) {
        if ($("#LightName").val() === ''  || $("#LightCost").val() === '' || $("#LightFilePath").val() === '') {
            alert("Please fill in all required fields");
            return;
        } else {
            fieldsFilled = true;
        }
    }
    var lightlist = [];
    var LightID = $("#LightID").val();
   // LightID = $("#LightID").val();
    //alert($("#LightID").val());
    if (LightID == "0") {
        lightlist.push(LightID);
    }
    else {
        lightlist.push($("#LightID").val());
    }

    /*foodlist.push($("#FoodID").val());*/
    lightlist.push($('input[name="LightType"]:checked').val());
    lightlist.push($("#LightName").val());
    lightlist.push($("#LightCost").val());
    lightlist.push("");
    lightlist.push($("#LightFilePath").val());
    lightlist.push($("#Createdate").val());

    var fileInput = document.getElementById("LightFilePath");
    var files = fileInput.files;
    //console.log(files);
    var formData = new FormData();
    formData.append("LightFilePath", files[0]);

    $.each(lightlist, function (index, value) {

        formData.append("lightlist[]", value);
        // console.log(formData);
    });

    $.ajax({
        type: "POST",
        url: '/Light/AddUpdateLight',
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            /* $('#divDataMaster').show();*/

            if (!data.isSuccess) {
                $('._CustomMessage').text(data.message);
                $('#errorPopup').modal('show');

            }
            else {
                $('._CustomMessage').text(data.message);
                $('#successPopup').modal('show');
               
               
                $("#btnSaveUpdateLight").text("Save/Update");
              
                fnLoadData();
                fnLoadLightDetails();
                $('#divDataMaster').show();
            
                $("#LightID").val("");
                $("#btnSaveUpdateLight").removeClass("disable-ele-color");

                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#lightImagePreview').attr('src', e.target.result);
                    $("#LightImage").attr('src', e.target.result); // Set the src attribute of LightImage
                }
                reader.readAsDataURL(files[0]);
            }
        },
        error: function (errormessage) {

        }
    });
}

function fnAddNewMaster() {
    $('#divDataMaster').hide();
    $('#divAddEditElements').show();
    $('#LightID').val("");
    //$('input[name="LightType"]').prop('checked', false);
    $('#LightName').val("");
    $('#LightCost').val("");
    $('#LightFilenameLabel').text("");
    $('#LightFilePath').val("");
    $('#LightImage').attr('src', '');

}

function fnEditLightData(LightID) {
    $('#divDataMaster').hide();
   // alert(LightID);
    $.ajax({
        type: "GET",
        url: "/Light/LoadEditLightData",
        data: { LightID },
        success: function (data) {
            //$('#divAddEditElements').show();

            if (!data.isSuccess) {
                $('._CustomMessage').text(data.message);
                $('#errorPopup').modal('show');
            }
            else {
                fnAddNewMaster();
                if (data.dataList) {
                   /* alert(LightID);*/
                    $('#LightID').val(data.dataList.lightID);
                    //alert(data.dataList.lightID);
                    $("#LightID").prop("disabled", true);
                    $('#LightName').val(data.dataList.lightName);
                    $('input[name="LightType"][value="' + data.dataList.lightType + '"]').prop('checked', true); 
                    $('#LightCost').val(data.dataList.lightCost);
                    $('#LightFilename').val(data.dataList.lightFilename );
                    $('#LightFilenameLabel').text(data.dataList.lightFilename);
                    $('#LightImage').attr('src', data.dataList.lightFilePath);

                    $('#Createdate').val(data.dataList.createdate);
                   
                    // var datalst = data.dataList;
                    /*window.location.href = '/Customer/AddProduct?dataList=' + Product.toString();*/
                    //window.location.href = '//Load';
                }
            }
        },
        error: function () {

        }
    });
}
function fnLoadData() {
   
    /*  alert("Hi");*/
    $.ajax({
        type: "POST",
        url: "/Light/LoadLight",
        data: {},
        success: function (obj) {

            //if (!obj.isSuccess) {
            //    /*   alert("Hello");*/
            //    $('._CustomMessage').text(obj.message);
            //    $('#errorPopup').modal('show');
            //}
            //else {
                fnLoadLightGrid(obj.dataList);
                $('#divAddEditElements').hide();
                $('#divDataMaster').show();
            
        },
        error: function () {

        }
    });
}
function fnLoadLightGrid(DataList) {
    DataList.sort(function (a, b) {
        return b.lightID - a.lightID;
    });
    $('#tblLight').empty();
    $('#tblLight').dataTable({
        "pageLength": 10,
        "Processing": true,
        "destroy": true,
        "aaData": DataList,
        "columns": [
            {
                "data": "lightID", "width": "50%", "title": "", "class": "text-center",
                "render": function (data) {
                    return ` <div class="row" style="width:170px">
                        <div class="col-xl-5">
                            <button type="button" class="dt-btn-approve btn btn-primary btn-block" onclick="fnEditLightData('${data}')">Edit</button>
                        </div>
                        <div class="col-xl-5 d-flex justify-content-center"  style="width:auto">
                            <button type="button" class="dt-btn-reject btn btn-danger btn-block" onclick="fnDeleteLightData('${data}')">Delete</button>
                        </div>
                    </div>`;
                }
            },
            //{ "data": "lightID", "title": "LightId", "width": "50px" },
            { "data": "lightName", "title": "LightName", "width": "70px" },
            { "data": "lightCost", "title": "LightCost", "width": "70px" },
            {
                "data": "createdate", "title": "Createdate", "width": "70px",
                "render": function (data, type, row) {
                    if (data) {

                        var createdate = new Date(data);

                        var day = createdate.getDate();
                        var monthIndex = createdate.getMonth();
                        var year = createdate.getFullYear();

                        var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

                        return (day < 10 ? '0' : '') + day + '-' + monthNames[monthIndex] + '-' + year;
                    } else {
                        // If the data is null or undefined, return an empty string
                        return "";
                    }
                }            },

        ]
    });
    $('#tblLight_wrapper').find(".row :first").prop("style", "margin-bottom:2%");
    $('#tblLight_wrapper').find("select[name='tblLight_length']").prop("style", "margin-top:4% ;width:35% !important;"); 
}

function fnLoadLightDetails() {
    var id = $("#LightID").attr('value');
    if (id != "") {
        $.ajax({
            type: "POST",
            url: "/Light/LoadLightDetails",
            data: { id },
            success: function (data) {

                if (!data.isSuccess) {
                    $('._CustomMessage').text(data.message);
                    $('#errorPopup').modal('show');
                }
                else {
                    alert($("#LightID").val());
                    $("#LightID").val(data.light.lightid);
                    $("#LightName").val(data.light.lightname);
                    $("#LightCost").val(data.light.lightcost);
                    $("#Createdate").val(data.light.createdate);

                }
            },
            error: function () {

            }
        });
    }
    else {
        $('._CustomMessage').text("Please select Light!");
        $('#errorPopup').modal('show');
    }
}


function fnDeleteLightData(LightID) {
    var confirmationDialog = $('<div id="confirmationDialog" class="modal modal-center" style="width:550px; height:150px">' +
        '<div class="modal-content" style="margin-left:200px">' +
        '<span class="close" style="margin-left:10px">&times;</span>' +
        '<p style="margin-left:10px">Do you want to delete content?</p>' +
        '<div class="row buttons"  style="margin-left:10px">' +
        '<div class="row" style="width:185px;margin-bottom:5px">' +
        '<div class= "col-xl-5 d-flex justify-content-center" style="width:auto"" >' +
        '<button type="button" class="dt-btn-reject btn btn-danger btn-block" id="deleteBtn">Delete</button>' +
        '</div >' +
        '<div class="col-xl-5 d-flex justify-content-center" style="width:auto">' +
        '<button type="button" class="dt-btn-approve btn btn-primary btn-block" id="cancelBtn">Cancel</button>' +
        '</div>' +
        '</div > ' +
        '</div>' +
        '</div>' +
        '</div>');

    // Append the dialog div to the body
    $('body').append(confirmationDialog);

    // Show the dialog
    $("#confirmationDialog").css("display", "block");

    // Handle delete button click
    $("#deleteBtn").on("click", function () {
        // Close the dialog
        $("#confirmationDialog").css("display", "none");

        // Make AJAX request to delete the data and corresponding files
        $.ajax({
            type: "POST",
            url: "/Light/DeleteLightData",
            data: { LightID: LightID },
            success: function (data) {
                if (!data.isSuccess) {
                    $('._CustomMessage').text(data.message);
                    $('#errorPopup').modal('show');
                } else {
                    $('._CustomMessage').text(data.message);
                    $('#successPopup').modal('show');
                    fnLoadData();
                }
            },
            error: function () {
                // Handle error
            }
        });
    });

    // Handle cancel button click
    $("#cancelBtn, .close").on("click", function () {
        // Close the dialog
        $("#confirmationDialog").css("display", "none");
    });
}