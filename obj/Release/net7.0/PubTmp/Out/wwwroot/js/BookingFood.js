document.addEventListener('DOMContentLoaded', function () {
    fnFoodlst();
   // fnVenuelst();
    // showSuccessMessage();
});

function fnFoodlst() {
    var Food = $("#FoodID");
    Food.empty().append('<option selected="true" value="0" disabled = "disabled">Loading.....</option>');
    $.ajax({
        type: "POST",
        url: "/BookingFood/GetFoodList",
        data: '{}',
        success: function (response) {

            Food.empty().append('<option selected="true" value="0">Food</option>');

            $.each(response.list, function (Key, value) {
                $("#FoodID").append('<option  selected="true" value="' + value.value + '">' + value.text + '</option>');

            });

            var checkboxesHTML = '';
            $.each(response.list, function (key, value) {
         checkboxesHTML += '<label><input type="checkbox" id="FoodID' + value.foodID + '" name="chkfood" value="' + value.foodID + '" data-img="' + value.foodFilePath + '">' + value.foodName + '</label><br>';             
               // checkboxesHTML += '<label><input type="checkbox" id=chk_"' + item.value + '" name="DJ" value="' + item.value + '">' + item.text + '</label><br>';
            });
            $('#FoodID').html(checkboxesHTML);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
};

function fnBookFood() {

    if (!$('#FoodID input[type="checkbox"]').is(':checked')) {
       
        alert('Please select at least one food item.');
        return; 
    }
    
    if (!$('input[name="FoodType"]').is(':checked')) {
        alert('Please select Food Type.');
        return; 
    }

    if (!$('input[name="MealType"]').is(':checked')) {
       
        alert('Please select Meal Type.');
        return; 
    }
    
    if ($("#DishType").val() === "") {
        
        alert('Please select Dish Type.');
        return; 
    }


    var BookFoodlst = [];
    var fooditem = []
    $('#FoodID input[type="checkbox"]').each(function () {
        if ($(this).is(':checked')) {
           
            fooditem.push($(this).val());
        }
    }).change(changeImage);
   
    console.log(fooditem);
   
    BookFoodlst.push($('input[name="FoodType"]:checked').val());

    BookFoodlst.push($('input[name="MealType"]:checked').val());
    BookFoodlst.push($("#DishType").val());
  
    

    $.ajax({
        type: "POST",
        url: "/BookingFood/BookFood",
        data: { "BookFoodlst": JSON.stringify(BookFoodlst),"Fooditem": JSON.stringify(fooditem) },
        success: function (data) {
            window.location.href = '/BookingFood/Success';
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
        url: "/BookingFood/BookingID",
        data: {},
        success: function () {
            fnBookFood();
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
