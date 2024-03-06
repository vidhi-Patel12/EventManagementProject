document.addEventListener('DOMContentLoaded', function () {
    fnEquipmentlst();
    //fnVenuelst();
   // showSuccessMessage();
});


function fnEquipmentlst() {
   
    $.ajax({
        type: "POST",
        url: "/BookingEquipment/GetEquipmentList",
        data: '{}',
        success: function (data) {
          //  $("#VenueID").append('<option value="' + value.venueID + '" data-img="' + value.venueFilePath + '">' + value.venueName + '</option>');          
           
                var checkboxesHTML = '';
            $.each(data.list, function (key, value) {
                /*  checkboxesHTML += '<label><input type="checkbox" id="EquipmentID' + value.equipmentID + '" name="chkequipment" value="' + value.equipmentID + '" data-img="' + value.equipmentFilePath + '">' + value.equipmentName + '</label><br>';*/
                checkboxesHTML += '<label><input type="checkbox" id="EquipmentID' + value.equipmentID + '" name="chkfood" value="' + value.equipmentID + '" data-img="' + value.equipmentFilePath + '">' + value.equipmentName + '</label><br>';  
                });
            $('#EquipmentID').html(checkboxesHTML);
           // console.log(checkboxesHTML);
            }, 
        
        error: function (response) {
            alert(response.responseText);
        }
    });
};

function fnBookEquipment() {

    var checkedEquipmentCount = $('#EquipmentID input[type="checkbox"]:checked').length;
    if (checkedEquipmentCount === 0) {
        alert('Please select at least one equipment.');
        return;
    }
    var BookEquipmentlst = [];

    $('#EquipmentID input[type="checkbox"]').each(function () {
        if ($(this).is(':checked')) {
            // If checked, push its value into the BookEquipmentlst array
            BookEquipmentlst.push($(this).val());
        }
    }).change(changeImage);
   
    //BookEquipmentlst.push($("#BookingDate").val());
    BookEquipmentlst.push($("#BookingID").val());


    $.ajax({
        type: "POST",
        url: "/BookingEquipment/BookingEquipment",
        data: { "BookEquipmentlst": JSON.stringify(BookEquipmentlst) },
        success: function (data) {
            window.location.href = '/BookingEquipment/Success';
            // showSuccessMessage();
            // window.location.href = '/Login/Login?name=' + data.name;
            if (!data.isSuccess) {
                $('._CustomMessage').text(data.message);
                $('#errorPopup').modal('show');

            }

        },
        error: function (errormessage) {
           
        }
    });
}


function fnBookingId() {

    $.ajax({
        type: 'POST',
        url: "/BookingEquipment/BookingID",
        data: {},
        success: function () {
            fnBookEquipment();
            // fnBookingDetails();
            //alert("hii");
            //alert(data.roleid);
            //console.log(data);
            //alert(data);

        },
        error: function (errormessage) {

        }
    });
}


function showSuccessMessage() {
    // Use the window.prompt method to display a message
    window.prompt("Success!", "Venue Booked successfully. \n Next Process Book Equipment.");
    alert(window.location.href = '/BookingEquipment/BookEquipment');
}
