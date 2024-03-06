document.addEventListener('DOMContentLoaded', function () {
    $("#VenueFilePath").change(function () {
        var file = this.files[0];
        if (file) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#VenueImage').attr('src', e.target.result);
            }
            reader.readAsDataURL(file);
        }
    });
    fnLoadData();
});


function fnAddUpdateVenue() {

    
    var fieldsFilled = false;

    if (!fieldsFilled) {
        if ($("#VenueName").val() === '' || $("#VenueCost").val() === '' || $("#VenueFilePath").val() === '') {
            alert("Please fill in all required fields");
            return;
        } else if ($("#VenueFilePath")[0].files.length === 0) {
            alert("Please choose a file for VenueFilePath");
            return;
        } else {
            fieldsFilled = true;
        }
    }
    var venuelist = [];
    var VenueID = $("#VenueID").attr('value');

    if (VenueID == "0") {
        venuelist.push(VenueID);
    }
    else {
        venuelist.push($("#VenueID").val());
    }

    venuelist.push($("#VenueName").val());
    venuelist.push($("#VenueCost").val());
    venuelist.push("");
    venuelist.push($("#VenueFilePath").val())
    venuelist.push($("#Createdate").val());

    var fileInput = document.getElementById("VenueFilePath");
    var files = fileInput.files;

    var formData = new FormData();
    formData.append("VenueFilePath", files[0]);

    $.each(venuelist, function (index, value) {
        formData.append("venuelist[]", value);
    });

    $.ajax({
        type: "POST",
        url: '/Venue/AddUpdateVenue',
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

                $("#btnSaveUpdateVenue").text("Save/Update");
                fnLoadData();
                fnLoadVenueDetails();
                $('#divVenueMaster').show();

                $("#VenueID").val("");
                $("#btnSaveUpdateVenue").removeClass("disable-ele-color");

                // Update image preview
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#venueImagePreview').attr('src', e.target.result);
                    $("#VenueImage").attr('src', e.target.result); // Set the src attribute of VenueImage
                }
                reader.readAsDataURL(files[0]);
            }
        },
        error: function (errormessage) {
            // Handle error
        }
    });
}


function fnAddNewMaster() {

    $('#divVenueMaster').hide();
    $('#divAddEditElements').show();
    $('#VenueID').val("");
    $('#VenueName').val("");
    $('#VenueCost').val("");
    $('#VenueFilename').val("");
    $('#venueFilenameLabel').text('');
    $('#VenueFilePath').val("");
    $('#VenueImage').attr('src', '');
}

function fnEditVenueData(VenueID) {
    $('#divVenueMaster').hide();

    $.ajax({
        type: "GET",
        url: "/Venue/LoadEditVenueData",
        data: { VenueID },
        success: function (data) {
            //$('#divAddEditElements').show();

            if (!data.isSuccess) {
                $('._CustomMessage').text(data.message);
                $('#errorPopup').modal('show');
            }
            else {
                fnAddNewMaster();
                if (data.dataList) {
                    $('#VenueID').val(data.dataList.venueID);
                    $("#VenueID").prop("disabled", true);
                    $('#VenueName').val(data.dataList.venueName);
                    $('#VenueCost').val(data.dataList.venueCost);
                    $('#VenueFilename').val(data.dataList.venueFilename);
                    $('#venueFilenameLabel').text(data.dataList.venueFilename);
                    //$('#VenueFilePath').text(data.dataList.venueFileP);
                    $('#VenueImage').attr('src', data.dataList.venueFilePath);
                    $('#Createdate').val(data.dataList.createdate);
   
                }
            }
        },
        error: function () {

        }
    });
}




function fnLoadData() {


    $.ajax({
        type: "POST",
        url: "/Venue/LoadVenue",
        data: {},
        success: function (obj) {

            fnLoadVenueGrid(obj.dataList);
            /*console.log(obj.dataList);*/
            $('#divAddEditElements').hide();
            $('#divVenueMaster').show();

        },
        error: function () {

        }
    });
}
function fnLoadVenueGrid(DataList) {
    DataList.sort(function (a, b) {
        return b.venueID - a.venueID;
    });

    $('#tblVenue').empty();
    $('#tblVenue').dataTable({

        "pageLength": 10,
        "Processing": true,
        "destroy": true,
        "aaData": DataList,
        "order": [[0, "DESC"]],
        "columns": [

            {
                "data": "venueID", "width": "30%", "title": "", "class": "text-center",
                "render": function (data) {
                    return `
            <div class="row" style="width:170px">
                <div class="col-xl-5">
                    <button type="button" class="dt-btn-approve btn btn-primary btn-block" onclick="fnEditVenueData('${data}')">Edit</button>
                </div>
                <div class="col-xl-5 d-flex justify-content-center"  style="width:auto">
                    <button type="button" class="dt-btn-reject btn btn-danger btn-block" onclick="fnDeleteVenueData('${data}')">Delete</button>
                </div>
            </div>`;
                }
            },

            { "data": "venueName", "title": "VenueName", "width": "70px" },
            { "data": "venueCost", "title": "VenueCost", "width": "70px" },
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
                }
            },

        ]
    });



    $('#tblVenue_wrapper').find(".row :first").prop("style", "margin-bottom:2% ;");
    $('#tblVenue_wrapper').find("select[name='tblVenue_length']").prop("style", "margin-top:4% ;width:35% !important;");

}

function fnLoadVenueDetails() {

    var id = $("#VenueID").attr('value');
    if (id != "") {
        $.ajax({
            type: "POST",
            url: "/Venue/LoadVenueDetails",
            data: { id },
            success: function (data) {

                if (!data.isSuccess) {
                    $('._CustomMessage').text(data.message);
                    $('#errorPopup').modal('show');
                }
                else {

                    $("#VenueID").val(data.venue.venueid);
                    $("#VenueName").val(data.venue.venuename);
                    $("#VenueCost").val(data.venue.venuecost);
                    $("#Createdate").val(data.venue.createdate);

                }
            },
            error: function () {

            }
        });
    }
    else {
        $('._CustomMessage').text("Please select Venue!");
        $('#errorPopup').modal('show');
    }
}

function fnDeleteVenueData(DeleteData) {
    // Create the dialog div using jQuery
    var confirmationDialog = $('<div id="confirmationDialog" class="modal modal-center" style="width:550px; hieght:150px">' +
        '<div class="modal-content" style="margin-left:200px">' +
        '<span class="close" style="margin-left:10px">&times;</span>' +
        '<p style="margin-left:10px">Do you want to delete content?</p>' +
        '<div class="row buttons"  style="margin-left:10px">' +
        '<div class="row" style="width:185px;margin-bottom:5px">'+
        '<div class= "col-xl-5 d-flex justify-content-center" style="width:auto"" >' +
        '<button type="button" class="dt-btn-reject btn btn-danger btn-block" id="deleteBtn">Delete</button>'+
        '</div >'+
        '<div class="col-xl-5 d-flex justify-content-center" style="width:auto">'+
        '<button type="button" class="dt-btn-approve btn btn-primary btn-block" id="cancelBtn">Cancel</button>'+
        '</div>'+
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
            url: "/Venue/DeleteVenueData",
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
