document.addEventListener('DOMContentLoaded', function () {
    $("#FoodFilePath").change(function () {
        var file = this.files[0];
        if (file) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#FoodImage').attr('src', e.target.result);
            }
            reader.readAsDataURL(file);
        }
    });
    fnLoadData();
});


function fnAddUpdateFood() {

    var fieldsFilled = false;

    if (!fieldsFilled) {
        if ($("#FoodType").val() === '' || $("#MealType").val() === '' || $("#DishType").val() === '' || $("#FoodName").val() === '' || $("#FoodCost").val() === '' || $("#FoodFilePath").val() === '') {
            alert("Please fill in all required fields");
            return;
        } else {
            fieldsFilled = true;
        }
    }
    var foodlist = [];
    var FoodID = $("#FoodID").val();
    FoodID = $("#FoodID").attr('value');

    if (FoodID == "0") {
        foodlist.push(FoodID);
    }
    else {
        foodlist.push($("#FoodID").val());
    }

    /*foodlist.push($("#FoodID").val());*/
    foodlist.push($('input[name="FoodType"]:checked').val());
    foodlist.push($('input[name="MealType"]:checked').val());
    foodlist.push($("#DishType").val());
    foodlist.push($("#FoodName").val());
    foodlist.push($("#FoodCost").val());
    foodlist.push("");
    foodlist.push($("#FoodFilePath").val());
    foodlist.push($("#Createdate").val());

    var fileInput = document.getElementById("FoodFilePath");
    var files = fileInput.files;
    //console.log(files);
    var formData = new FormData();
    formData.append("FoodFilePath", files[0]);

    $.each(foodlist, function (index, value) {

        formData.append("foodlist[]", value);
        // console.log(formData);
    });

    $.ajax({
        type: "POST",
        url: '/Food/AddUpdateFood',
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
              
                $("#btnSaveUpdateFood").text("Save/Update");
                
                fnLoadData();
                fnLoadFoodDetails();

                $('#divDataMaster').show();
                //$('#divAddEditMaster').hide();
                $("#FoodID").val("");
                $("#btnSaveUpdateFood").removeClass("disable-ele-color");
             
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#foodImagePreview').attr('src', e.target.result);
                    $("#FoodImage").attr('src', e.target.result); // Set the src attribute of VenueImage
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
    $('#FoodID').val("");
    $('input[name="FoodType"]').prop('checked', false);
    $('input[name="MealType"]').prop('checked', false);
    $('#DishType').val("");
    $('#FoodName').val("");
    $('#FoodCost').val("");
    $('#FoodFilenameLabel').text("");
    $('#FoodFilePath').val("");
    $('#FoodImage').attr('src', '');
}

function fnEditFoodData(FoodID) {
    $('#divDataMaster').hide();

    $.ajax({
        type: "GET",
        url: "/Food/LoadEditFoodData",
        data: { FoodID },
        success: function (data) {
            //$('#divAddEditElements').show();

            if (!data.isSuccess) {
                $('._CustomMessage').text(data.message);
                $('#errorPopup').modal('show');
            }
            else {
                fnAddNewMaster();
                if (data.dataList) {
                    $('#FoodID').val(data.dataList.foodID);
                    $("#FoodID").prop("disabled", true);
                    $('#FoodName').val(data.dataList.foodName);
                    $('#FoodCost').val(data.dataList.foodCost);
                    $('#FoodFilenameLabel').text(data.dataList.foodFilename);
                    $('#FoodImage').attr('src', data.dataList.foodFilePath);

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
        url: "/Food/LoadFood",
        data: {},
        success: function (obj) {

            if (!obj.isSuccess) {
                /*   alert("Hello");*/
                $('._CustomMessage').text(obj.message);
                $('#errorPopup').modal('show');
            }
            else {
                fnLoadFoodGrid(obj.dataList);
                $('#divAddEditElements').hide();
                $('#divDataMaster').show();
            }
        },
        error: function () {

        }
    });
}
function fnLoadFoodGrid(DataList) {

    DataList.sort(function (a, b) {
        return b.foodID - a.foodID;
    });

    $('#tblFood').empty();
    $('#tblFood').dataTable({
        "pageLength": 10,
        "Processing": true,
        "destroy": true,
        "aaData": DataList,
        "columns": [
            {
                "data": "foodID", "width": "50%", "title": "", "class": "text-center",
                "render": function (data) {
                    return `
                    <div class="row" style="width:170px">
                        <div class="col-xl-5">
                            <button type="button" class="dt-btn-approve btn btn-primary btn-block" onclick="fnEditFoodData('${data}')">Edit</button>
                        </div>
                        <div class="col-xl-5 d-flex justify-content-center"  style="width:auto">
                            <button type="button" class="dt-btn-reject btn btn-danger btn-block text-center" onclick="fnDeleteFoodData('${data}')">Delete</button>
                        </div>
                    </div>`;
                }
            },
            //{ "data": "foodID", "title": "FoodId", "width": "50px" },
            { "data": "foodName", "title": "FoodName", "width": "70px" },
            { "data": "foodCost", "title": "FoodCost", "width": "70px" },
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
    $('#tblFood_wrapper').find(".row :first").prop("style", "margin-bottom:2%");
    $('#tblFood_wrapper').find("select[name='tblFood_length']").prop("style", "margin-top:4% ;width:35% !important;");

}

function fnLoadFoodDetails() {
    var id = $("#FoodID").attr('value');
    if (id != "") {
        $.ajax({
            type: "POST",
            url: "/Food/LoadFoodDetails",
            data: { id },
            success: function (data) {

                if (!data.isSuccess) {
                    $('._CustomMessage').text(data.message);
                    $('#errorPopup').modal('show');
                }
                else {

                    $("#FoodID").val(data.food.foodid);
                    $("#FoodName").val(data.food.foodname);
                    $("#FoodCost").val(data.food.foodcost);
                    $("#Createdate").val(data.food.createdate);

                }
            },
            error: function () {

            }
        });
    }
    else {
        $('._CustomMessage').text("Please select Food!");
        $('#errorPopup').modal('show');
    }
}


function fnDeleteFoodData(DeleteData) {

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

    // Append the dialog div to the body
    $('body').append(confirmationDialog);

    // Show the dialog
    $("#confirmationDialog").css("display", "block");

    // Handle delete button click
    $("#deleteBtn").on("click", function () {
        // Close the dialog
        $("#confirmationDialog").css("display", "none");

        // Make AJAX request to delete the data
        $.ajax({
            type: "POST",
            url: "/Food/DeleteFoodData",
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
