document.addEventListener('DOMContentLoaded', function () {
    fnLightlst();
    // fnVenuelst();
    // showSuccessMessage();
});

function fnLightlst() {
    var Light = $("#LightID");
    Light.empty().append('<option selected="true" value="0" disabled = "disabled">Loading.....</option>');
    $.ajax({
        type: "POST",
        url: "/BookingLight/GetLightList",
        data: '{}',
        success: function (response) {

            Light.empty().append('<option selected="true" value="0">Light</option>');

            $.each(response.list, function (Key, value) {
                $("#LightID").append('<option  selected="true" value="' + value.value + '">' + value.text + '</option>');

            });

            var checkboxesHTML = '';
            $.each(response.list, function (key, value) {
                checkboxesHTML += '<label><input type="checkbox" id="LightID' + value.foodID + '" name="chklight" value="' + value.lightID + '" data-img="' + value.lightFilePath + '">' + value.lightName + '</label><br>';             
                //checkboxesHTML += '<label><input type="checkbox" id=chk_"' + item.value + '" name="light" value="' + item.value + '">' + item.text + '</label><br>';
            });
            $('#LightID').html(checkboxesHTML);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
};

function fnBookLight() {
   
    var BookLightlst = [];
    var lightitem = []
    $('#LightID input[type="checkbox"]').each(function () {
        if ($(this).is(':checked')) {

            BookLightlst.push($(this).val());
        }
    }).change(changeImage);

    if (BookLightlst.length === 0) {
        alert('Please select at least one light to book.');
        return; // Exit the function if no light is selected
    }
    $.ajax({
        type: "POST",
        url: "/BookingLight/BookLight",
        data: { "BookLightlst": JSON.stringify(BookLightlst) },
        success: function (data) {
            window.location.href = '/BookingLight/Success';
        
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
        url: "/BookingLight/BookingID",
        data: {},
        success: function () {
            fnBookLight();
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
