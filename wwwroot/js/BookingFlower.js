document.addEventListener('DOMContentLoaded', function () {
    fnFlowerlst();
    // fnVenuelst();
    // showSuccessMessage();
});

function fnFlowerlst() {
    var Flower = $("#FlowerID");
    Flower.empty().append('<option selected="true" value="0" disabled = "disabled">Loading.....</option>');
    $.ajax({
        type: "POST",
        url: "/BookingFlower/GetFlowerList",
        data: '{}',
        success: function (response) {

            //Flower.empty().append('<option selected="true" value="0">Flower</option>');

            //$.each(response.list, function (Key, value) {
            //    $("#FlowerID").append('<option  selected="true" value="' + value.value + '">' + value.text + '</option>');

            //});

            var checkboxesHTML = '';
            $.each(response.list, function (key, value) {
                checkboxesHTML += '<label><input type="checkbox" id="FlowerID' + value.flowerID + '" name="chkflower" value="' + value.flowerID + '" data-img="' + value.flowerFilePath + '">' + value.flowerName + '</label><br>';             
                //checkboxesHTML += '<label><input type="checkbox" id=chk_"' + item.value + '" name="Flower" value="' + item.value + '">' + item.text + '</label><br>';
            });
            $('#FlowerID').html(checkboxesHTML);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
};

function fnBookFlower() {

    var BookFlowerlst = [];
    var flowerSelected = false;
   
    $('#FlowerID input[type="checkbox"]').each(function () {
        if ($(this).is(':checked')) {

        
            BookFlowerlst.push($(this).val());
            flowerSelected = true;
        }
    }).change(changeImage);

    if (!flowerSelected) {
        alert("Please select at least one flower to book.");
        return; 
    }

    $.ajax({
        type: "POST",
        url: "/BookingFlower/BookFlower",
        data: { "BookFlowerlst": JSON.stringify(BookFlowerlst) },
        success: function (data) {
            window.location.href = '/BookingFlower/Success';
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
        url: "/BookingFlower/BookingID",
        data: {},
        success: function () {
            fnBookFlower();
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
