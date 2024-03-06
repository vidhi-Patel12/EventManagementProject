document.addEventListener('DOMContentLoaded', function () {
    fnEventlst();
    fnVenuelst();
   // showSuccessMessage();
});

function fnEventlst() {
    var Event = $("#EventID");
    Event.empty().append('<option selected="true" value="0" disabled = "disabled">Loading.....</option>');
    $.ajax({
        type: "POST",
        url: "/BookingVenue/GetEventList",
        data: '{}',
        success: function (response) {

            Event.empty().append('<option selected="true" value="0">Event</option>');

            $.each(response.list, function (Key, value) {
                $("#EventID").append('<option value="' + value.value + '">' + value.text + '</option>');

            });
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
};


function fnVenuelst() {
    var Venue = $("#VenueID");
    Venue.empty().append('<option selected="true" value="0" disabled = "disabled">Loading.....</option>');
    $.ajax({
        type: "POST",
        url: "/BookingVenue/GetVenueList",
        data: '{}',
        success: function (response) {

            Venue.empty().append('<option selected="true" value="0">Venue</option>');

            $.each(response.list, function (Key, value) {
               
                //$("#VenueID").append('<option value="' + value.value + '">' + value.text + '</option>');
                $("#VenueID").append('<option value="' + value.venueID + '" data-img="' + value.venueFilePath + '">' + value.venueName + '</option>');          
            });
            
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
};

function fnBookingId() {

    $.ajax({
        type: 'POST',
        url: "/BookingVenue/BookingDetailsForID",
        data: {},
        success: function () {
            fnBookVenue();
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

function fnBookingDetails() {

    var BookVenuelst = [];
   
    //BookVenuelst.push($("#BookingNo").val());
    BookVenuelst.push($("#BookingDate").val());
    BookVenuelst.push($("#Createdby").val());
    BookVenuelst.push($("#CreatedDate").val());
    BookVenuelst.push($("#BookingApproval").attr('value'));

    $.ajax({
        type: "POST",
        url: "/BookingVenue/BookingDetails",
        data: { "BookVenuelst": JSON.stringify(BookVenuelst) },
        success: function (data) {
            fnBookingId();
    
           // showSuccessMessage();
            // window.location.href = '/Login/Login?name=' + data.name;
            if (!data.isSuccess) {
                $('._CustomMessage').text(data.message);
                $('#errorPopup').modal('show');

            }

        },
        error: function (errormessage) {
            alert("hii");
        }
    });
}

function fnBookVenue() {

    if ($("#EventID").val() === '' || $("#VenueID").val() === '' || $("#GuestNo").val() === '' || $("#BookingDate").val() === '') {
        // Show error message or handle validation failure
        alert("Please fill in all required fields");
        return; // Exit function if validation fails
    }
    var BookVenuelst = [];
    

    BookVenuelst.push($("#EventID").val());
    BookVenuelst.push($("#VenueID").val());
    BookVenuelst.push($("#GuestNo").val());
    BookVenuelst.push($("#BookingDate").val());
    BookVenuelst.push($("#BookingID").val());
  
  
    $.ajax({
        type: "POST",
        url: "/BookingVenue/BookVenue",
        data: { "BookVenuelst": JSON.stringify(BookVenuelst) },
        success: function (data) {
            window.location.href = '/BookingVenue/Success';
           // showSuccessMessage();
           // window.location.href = '/Login/Login?name=' + data.name;
            if (!data.isSuccess) {
                $('._CustomMessage').text(data.message);
                $('#errorPopup').modal('show');

            }

        },
        error: function (errormessage) {
           // alert("hii");
        }
    });
}

function showSuccessMessage() {
    // Use the window.prompt method to display a message
    window.prompt("Success!", "Venue Booked successfully. \n Next Process Book Equipment.");
    alert(window.location.href = '/BookingEquipment/BookEquipment');
}
