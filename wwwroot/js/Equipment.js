document.addEventListener('DOMContentLoaded', function () {
    $("#EquipmentFilePath").change(function () {
        var file = this.files[0];
        if (file) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#EquipmentImage').attr('src', e.target.result);
            }
            reader.readAsDataURL(file);
        }
    });
    fnLoadData();
});


function fnAddUpdateEquipment() {

    var fieldsFilled = false;

    if (!fieldsFilled) {
        if ($("#EquipmentName").val() === '' || $("#EquipmentCost").val() === '' || $("#EquipmentFilePath").val() === '') {
            alert("Please fill in all required fields");
            return;
        } else {
            fieldsFilled = true;
        }
    }
    var equipmentlist = [];
  
    var EquipmentID = $("#EquipmentID").attr('value');
    


    if (EquipmentID == "0") {
        equipmentlist.push(EquipmentID);
    }
    else {
        equipmentlist.push($("#EquipmentID").val());
    }

    /*Product.push($("#EquipmentID").val());*/
    equipmentlist.push($("#EquipmentName").val());
    equipmentlist.push($("#EquipmentCost").val());
    equipmentlist.push("");
    equipmentlist.push($("#EquipmentFilePath").val())
    equipmentlist.push($("#Createdate").val());

    var fileInput = document.getElementById("EquipmentFilePath");
    var files = fileInput.files;
    //console.log(files);
    var formData = new FormData();
    formData.append("EquipmentFilePath", files[0]);

    $.each(equipmentlist, function (index, value) {

        formData.append("equipmentlist[]", value);
       // console.log(formData);
    });

    $.ajax({
        type: "POST",
        url: '/Equipment/AddUpdateEquipment',
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
           
            if (!data.isSuccess) {
                $('._CustomMessage').text(data.message);
                $('#errorPopup').modal('show');

            }
            else {
                $('._CustomMessage').text(data.message);
                $('#successPopup').modal('show');
               
                $("#btnSaveUpdateEquipment").text("Save/Update");
                                        
                fnLoadData();
                fnLoadEquipmentDetails();
                $('#divDataMaster').show();
                $("#EquipmentID").val("");

                $("#btnSaveUpdateEquipment").removeClass("disable-ele-color");

                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#equipmentImagePreview').attr('src', e.target.result);
                    $("#EquipmentImage").attr('src', e.target.result); 
                }
                reader.readAsDataURL(files[0]);
            }
        },
        error: function (errormessage) {

        }
    });
}

function fnAddNewMaster() {
    $('#divAddEditElements').show();
    $('#divDataMaster').hide();
    $('#EquipmentID').val("");
    $('#EquipmentName').val("");
    $('#EquipmentCost').val("");
    $('#EquipmentFilenameLabel').text("");
    $('#EquipmentFilePath').val("");
    $('#EquipmentImage').attr('src', '');

}

function fnEditEquipmentData(EquipmentID) {
    $('#divDataMaster').hide();
    $('#divAddEditElements').show();
    $.ajax({
        type: "GET",
        url: "/Equipment/LoadEditEquipmentData",
        data: { EquipmentID },
        success: function (data) {
            //$('#divAddEditElements').show();

            if (!data.isSuccess) {
                $('._CustomMessage').text(data.message);
                $('#errorPopup').modal('show');
            }
            else {
                fnAddNewMaster();
                if (data.dataList) {
                    $('#EquipmentID').val(data.dataList.equipmentID);
                    $("#EquipmentID").prop("disabled", true);
                    $('#EquipmentName').val(data.dataList.equipmentName);
                    $('#EquipmentCost').val(data.dataList.equipmentCost);
                    $('#EquipmentFilename').val(data.dataList.equipmentFilename);
                    $('#EquipmentFilenameLabel').text(data.dataList.equipmentFilename);
                    $('#EquipmentImage').attr('src', data.dataList.equipmentFilePath);

                    $('#Createdate').val(data.dataList.createdate);
                    //$('#divAddEditElements').show();


                    // var datalst = data.dataList;
                    /*window.location.href = '/Customer/AddProduct?dataList=' + Product.toString();*/
                    // window.location.href = '/Customer/AddUpdateProduct';
                }
            }
        },
        error: function () {

        }
    });
}
function fnLoadData() {
   /* $('#divDataMaster').hide();*/
    /*  alert("Hi");*/
    $.ajax({
        type: "POST",
        url: "/Equipment/LoadEquipment",
        data: {},
        success: function (obj) {

            if (!obj.isSuccess) {
                /*   alert("Hello");*/
                $('._CustomMessage').text(obj.message);
                $('#errorPopup').modal('show');
            }
            else {
               // $('#divDataMaster').show();
                fnLoadEquipmentGrid(obj.dataList);
                $('#divAddEditElements').hide();
                $('#divDataMaster').show();
            }
        },
        error: function () {

        }
    });
}
function fnLoadEquipmentGrid(DataList) {

    DataList.sort(function (a, b) {
        return b.equipmentID - a.equipmentID;
    });

    $('#tblEquipment').empty();
    $('#tblEquipment').dataTable({
        "pageLength": 10,
        "Processing": true,
        "destroy": true,
        "aaData": DataList,
        "columns": [
            {
                "data": "equipmentID", "width": "100%", "title": "", "class": "text-center",
                "render": function (data) {
                    return `
            <div class="row" style="width:170px">
                <div class="col-xl-5">
                    <button type="button" class="dt-btn-approve btn btn-primary btn-block" onclick="fnEditEquipmentData('${data}')">Edit</button>
                </div>
                <div class="col-xl-5 d-flex justify-content-center"  style="width:auto">
                    <button type="button" class="dt-btn-reject btn btn-danger btn-block" onclick="fnDeleteEquipmentData('${data}')">Delete</button>
                </div>
            </div>`;
                   
                }
              
            },
            //{ "data": "equipmentID", "title": "EquipmentId", "width": "90px" },
            { "data": "equipmentName", "title": "EquipmentName", "width": "90px" },
            { "data": "equipmentCost", "title": "EquipmentCost", "width": "90px" },
            {
                "data": "createdate", "title": "Createdate", "width": "90px",
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
    $('#tblEquipment_wrapper').find(".row :first").prop("style", "margin-bottom:2%");
    $('#tblEquipment_wrapper').find("select[name='tblEquipment_length']").prop("style", "margin-top:4% ;width:35% !important;");

}

function fnLoadEquipmentDetails() {
    var id = $("#EquipmentID").attr('value');
    if (id != "") {
        $.ajax({
            type: "POST",
            url: "/Equipment/LoadEquipmentDetails",
            data: { id },
            success: function (data) {

                if (!data.isSuccess) {
                    $('._CustomMessage').text(data.message);
                    $('#errorPopup').modal('show');
                }
                else {

                    $("#EquipmentID").val(data.equipment.equipmentid);
                    $("#EquipmentName").val(data.equipment.equipmentname);
                    $("#EquipmentCost").val(data.equipment.equipmentcost);
                    $("#Createdate").val(data.equipment.createdate);

                }
            },
            error: function () {

            }
        });
    }
    else {
        $('._CustomMessage').text("Please select Equipment!");
        $('#errorPopup').modal('show');
    }
}


function fnDeleteEquipmentData(EquipmentID) {
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
            url: "/Equipment/DeleteEquipmentData",
            data: { EquipmentID: EquipmentID },
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