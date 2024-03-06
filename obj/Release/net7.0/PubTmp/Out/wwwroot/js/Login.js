document.addEventListener('DOMContentLoaded', function () {

});

function fnLogin() {
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
    var login = [];

    login.push(username);
    login.push(password);
  //  login.push($("#RoleID").val());
    

    $.ajax({
        type: 'POST',
        url: "/Login/Login",
        data: { "login": JSON.stringify(login) },
        success: function (data) {
            //alert("hii");
            //alert(data.roleid);
            //console.log(data);
            //alert(data);
            if (data.roleID == "Admin") {
               
                window.location.href = '/Admin/Dashboard?username=' + data.username;
            }
            else if (data.roleID == "Customer") {
                window.location.href = '/Customer/Dashboard?username=' + data.username;
            }
            else if (data.roleID == "SuperAdmin") {
                window.location.href = '/SuperAdmin/Dashboard?username=' + data.username;
            }
            else {
                alert("Username or password is incorrect. Please check your credentials.");
            }

        },
        error: function (errormessage) {
            alert("Username or password is incorrect. Please check your credentials.");
        }
    });
}