document.addEventListener('DOMContentLoaded', function () {
    $("#FlowerFilePath").change(function () {
        var file = this.files[0];
        if (file) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#FlowerImage').attr('src', e.target.result);
            }
            reader.readAsDataURL(file);
        }
    });
    fnLoadData();
});


function fnAddUpdateFlower() {

    var fieldsFilled = false;
    if (!fieldsFilled) {
        if ($("#FlowerName").val() === '' || $("#FlowerCost").val() === '' || $("#FlowerFilePath").val() === '') {
            alert("Please fill in all required fields");
            return;
        } else {
            fieldsFilled = true;
        }
    }
    var flowerlist = [];
    var FlowerID = $("#FlowerID").val();
    //FlowerID = $("#FlowerID").attr('value');

    if (FlowerID == "0") {
        flowerlist.push(FlowerID);
    }
    else {
        flowerlist.push($("#FlowerID").val());
    }

    /*Product.push($("#FlowerID").val());*/
    flowerlist.push($("#FlowerName").val());
    flowerlist.push($("#FlowerCost").val());
    flowerlist.push("");
    flowerlist.push($("#FlowerFilePath").val());
    flowerlist.push($("#Createdate").val());

    var fileInput = document.getElementById("FlowerFilePath");
    var files = fileInput.files;
    //console.log(files);
    var formData = new FormData();
    formData.append("FlowerFilePath", files[0]);

    $.each(flowerlist, function (index, value) {

        formData.append("flowerlist[]", value);
        // console.log(formData);
    });

    $.ajax({
        type: "POST",
        url: '/Flower/AddUpdateFlower',
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            //$('#divFlowerMaster').show();

            if (!data.isSuccess) {
                $('._CustomMessage').text(data.message);
                $('#errorPopup').modal('show');

            }
            else {
                $('._CustomMessage').text(data.message);
                $('#successPopup').modal('show');
               

                $("#btnSaveUpdateFlower").text("Save/Update");

                fnLoadData();
                fnLoadFlowerDetails();
                $('#divFlowerMaster').show();

                $("#FlowerID").val("");
                $("#btnSaveUpdateFlower").removeClass("disable-ele-color");

                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#flowerImagePreview').attr('src', e.target.result);
                    $("#FlowerImage").attr('src', e.target.result); // Set the src attribute of VenueImage
                }
                reader.readAsDataURL(files[0]);
            }
        },
        error: function (errormessage) {

        }
    });
}

function fnAddNewMaster() {
    $('#divFlowerMaster').hide();
    $('#divAddEditElements').show();
    $('#FlowerID').val("");
    $('#FlowerName').val("");
    $('#FlowerCost').val("");
    $('#FlowerFilename').val("");
    $('#flowerFilenameLabel').text('');
    $('#FlowerFilePath').val("");
    $('#FlowerImage').attr('src', '');

}

function fnEditFlowerData(FlowerID) {
    $('#divFlowerMaster').hide();

    $.ajax({
        type: "GET",
        url: "/Flower/LoadEditFlowerData",
        data: { FlowerID },
        success: function (data) {
            //$('#divAddEditElements').show();

            if (!data.isSuccess) {
                $('._CustomMessage').text(data.message);
                $('#errorPopup').modal('show');
            }
            else {
                fnAddNewMaster();
                if (data.dataList) {
                    $('#FlowerID').val(data.dataList.flowerID);
                    $("#FlowerID").prop("disabled", true);
                    $('#FlowerName').val(data.dataList.flowerName);
                    $('#FlowerCost').val(data.dataList.flowerCost);
                    $('#FlowerFilename').val(data.dataList.flowerFilename);
                    $('#flowerFilenameLabel').text(data.dataList.flowerFilename);
                    //$('#VenueFilePath').text(data.dataList.venueFileP);
                    $('#FlowerImage').attr('src', data.dataList.flowerFilePath);

                    $('#Createdate').val(data.dataList.createdate);
                    // var datalst = data.dataList;
                    /*window.location.href = '/Customer/AddProduct?dataList=' + Product.toString();*/
                    //window.location.href = '/Flower/LoadFlower';
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
        url: "/Flower/LoadFlower",
        data: {},
        success: function (obj) {

            fnLoadFlowerGrid(obj.dataList);
           // console.log(obj.dataList);
            $('#divAddEditElements').hide();
            $('#divFlowerMaster').show();

        },
        error: function () {

        }
    });
}
function fnLoadFlowerGrid(DataList) {

    DataList.sort(function (a, b) {
        return b.flowerID - a.flowerID;
    });

    $('#tblFlower').empty();
    $('#tblFlower').dataTable({
        "pageLength": 10,
        "Processing": true,
        "destroy": true,
        "aaData": DataList,
        "columns": [
            {
                "data": "flowerID", "width": "50%", "title": "", "class": "text-center", "render": function (data) {
                    return ` <div class="row" style="width:170px">
                        <div class="col-xl-5">
                            <button type="button" class="dt-btn-approve btn btn-primary btn-block" onclick="fnEditFlowerData('${data}')">Edit</button>
                        </div>
                        <div class="col-xl-5 d-flex justify-content-center"  style="width:auto">
                            <button type="button" class="dt-btn-reject btn btn-danger btn-block" onclick="fnDeleteFlowerData('${data}')">Delete</button>
                        </div>
                    </div>`;
                }
            },
            //{ "data": "flowerID", "title": "FlowerID", "width": "50px" },
            { "data": "flowerName", "title": "FlowerName", "width": "70px" },
            { "data": "flowerCost", "title": "FlowerCost", "width": "70px" },
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
    $('#tblFlower_wrapper').find(".row :first").prop("style", "margin-bottom:2%");
    $('#tblFlower_wrapper').find("select[name='tblFlower_length']").prop("style", "margin-top:4% ;width:35% !important;");

}

function fnLoadFlowerDetails() {
    var id = $("#FlowerID").attr('value');
    if (id != "") {
        $.ajax({
            type: "POST",
            url: "/Flower/LoadFlowerDetails",
            data: { id },
            success: function (data) {

                if (!data.isSuccess) {
                    $('._CustomMessage').text(data.message);
                    $('#errorPopup').modal('show');
                }
                else {

                    $("#FlowerID").val(data.Flower.flowerid);
                    $("#FlowerName").val(data.Flower.flowername);
                    $("#FlowerCost").val(data.Flower.flowercost);
                    $("#Createdate").val(data.Flower.createdate);

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


function fnDeleteFlowerData(DeleteData) {

    var confirmationDialog = $('<div id="confirmationDialog" class="modal modal-center" style="width:550px; hieght:150px">' +
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

  
    $('body').append(confirmationDialog);

  
    $("#confirmationDialog").css("display", "block");
    
    $("#deleteBtn").on("click", function () {
      
        $("#confirmationDialog").css("display", "none");
        $.ajax({
            type: "POST",
            url: "/Flower/DeleteFlowerData",
            data: { DeleteData },
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
             
            }
        });
    });
    
    $("#cancelBtn, .close").on("click", function () {
        $("#confirmationDialog").css("display", "none");
    });
}
