document.addEventListener('DOMContentLoaded', function () {
    
    $(function () {
        var Country = $("#Country");
        Country.empty().append('<option selected="true" value="0" disabled = "disabled">Loading.....</option>');
        $.ajax({
            type: "POST",
            url: "/Registration/GetCountryList",
            data: '{}',
            success: function (response) {

                Country.empty().append('<option selected="true" value="0">Country</option>');
                
                $.each(response.list, function (Key, value) {
                    $("#Country").append('<option value="' + value.value + '">' + value.text + '</option>');
                
                });
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    });
});

document.getElementById("Country").addEventListener("change", function () {
    //var CountryID = $('#Country').val();
    var CountryID = this.value;
    //alert(countryID);
    GetStatesByCountryId(CountryID);
   
});
document.getElementById("State").addEventListener("change", function () {
    var StateID = this.value;
   // alert(stateID);
    GetCityByStateId(StateID);
});



function fnRegister() {

    var fields = ['Name', 'Address', 'Country', 'State', 'City', 'Mobileno', 'EmailID', 'Username', 'Password', 'ConfirmPassword', 'Gender', 'Birthdate', 'RoleID'];
    var emptyFields = [];
    fields.forEach(function (field) {
        if (!$('#' + field).val()) {
            emptyFields.push(field);
        }
    });

    // If any required field is empty, show a message and return
    if (emptyFields.length > 0) {
        var emptyFieldsMsg = emptyFields.join(', ');
        alert("Please fill out all required fields: " + emptyFieldsMsg);
        return;
    }


    var username = $("#Username").val();
    var password = $("#Password").val();

   
    var usernameRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$/;; 
    if (!usernameRegex.test(username)) {
        alert("Username must be at least 6 characters long and contain only alphanumeric characters.");
        return;
    }
    
    var passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$/; // At least 6 characters, one uppercase, one lowercase, one number, one special character
    if (!passwordRegex.test(password)) {
        alert("Password must be at least 6 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
        return;
    }

    var Register = [];

    Register.push($("#Name").val());
    Register.push($("#Address").val());
    Register.push($("#Country option:selected").text()); 
    Register.push($("#State option:selected").text());
    Register.push($("#City option:selected").text());
    Register.push($("#Mobileno").val());
    Register.push($("#EmailID").val());
    Register.push(username);
    Register.push(password);
    Register.push($("#ConfirmPassword").val());
    Register.push($('input[name="Gender"]:checked').val());
    Register.push($("#Birthdate").val());
    Register.push($("#RoleID option:selected").text());
    Register.push($("#CreatedOn").val());
    
    $.ajax({
        type: 'POST',
        url: "/Registration/Register",
        data: { "Register": JSON.stringify(Register) },
        success: function (data) {
            if (data.isSuccess) {
                window.location.href = '/Login/Login?name=' + data.Name;
            } else {
                alert("Username or Email already exists. Please choose different ones.");
            } 
            //window.location.href = '/Login/Login?name=' + data.name ;
            if (!data.isSuccess) {
                $('._CustomMessage').text(data.message);
                $('#errorPopup').modal('show');
               
            }

        },
        error: function (errormessage) {

        }
    });
}





function GetStatesByCountryId(CountryID) {
    $.ajax({
        type: "GET",
        url: '/Registration/GetStateList',
        data: { "CountryID": CountryID },
        success: function (data) {
            //alert(data);
                   // console.log(data);
                    $("#State").empty().append('<option selected="true" value="">Select State</option>');
                    $.each(data.statelist, function (key, value) {
                        $("#State").append('<option value="' + value.value + '">' + value.text + '</option>');
                       // alert(data.statelist);
                    });
                
           
        },
        error: function (errormessage) {
        }
    });
}


function GetCityByStateId(StateID) {
    $.ajax({
        type: "GET",
        url: '/Registration/GetCityList',
        data: { "StateID": StateID },
        success: function (data) {
           // console.log(data.StateID);
                    $("#City").empty().append('<option selected="true" value="">Select City</option>');
                    $.each(data.citylist, function (key, value) {
                        $("#City").append('<option value="' + value.value + '">' + value.text + '</option>');
                       // alert(data.citylist);
                    });       
        },
        error: function (errormessage) {
        }
    });
}