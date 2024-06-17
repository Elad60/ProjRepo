$(document).ready(function () {

    $("#registerForm").submit(Register);
    $("#loginForm").submit(Login);
});

const createUser = (id, name, email, password, isAdmin, isActive) => ({ id, name, email, password, isAdmin, isActive });

//---------login---------//
function Login() {

    let api = `https://localhost:7020/api/User/login?email=` + encodeURIComponent($('#emailTB').val());
    ajaxCall("POST", api, JSON.stringify($('#passwordTB').val()), postLSCBF, postLECBF);
    return false;
}

function postLSCBF(data) {
    if (data == null) {
        alert("User not found");
    }
    else {
        localStorage.setItem("loggedUser", data.id);
        alert("Logged in succefully");
        window.opener.location.reload();
        window.close();
    }
    console.log(data);
}
function postLECBF(err) {
    alert('User not found')
    console.log(err);
}


//---------register---------//

function Register() {
    let newUser = createUser(0, $('#nameTB').val(), $('#emailTB').val(), $('#passwordTB').val(), false, true);
    let api = `https://localhost:7020/api/User/register`;
    ajaxCall("POST", api, JSON.stringify(newUser), postRSCBF, postRECBF);
    return false;

}

function postRSCBF(data) {
    console.log(data);
    if (data != null) {
        localStorage.setItem("loggedUser", data.id);
        alert("The user has been registered");
        alert("The user has been logged in");
        window.opener.location.reload();
        window.close();

    }
    else {
        alert("The user is already registered");
        document.getElementById('registerForm').reset();
    }
}
function postRECBF(err) {
    alert('error')
    console.log(err);
}
