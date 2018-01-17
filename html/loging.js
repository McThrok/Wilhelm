/**
 *
 * @param {string} login
 * @param {string} password
* @returns {boolean}
 */
function VerifyUser(login, password) {
    console.log(login);
    console.log(password);
    if ((login == "qwe") && (password == "qwe"))
        return true;
    else
        return false;
}

function AddIncorrectLogingLabel() {
    var incorrect = document.getElementById("incorrect");
    incorrect.style.visibility = 'visible';
}

function SetValidation() {
    "use strict";
    document.getElementById("sign_in_form").onsubmit = function () {
        var login = document.getElementById("login").nodeValue;
        var password = document.getElementById("password").nodeValue;

        return VerifyUser(login, password);
        //if (VerifyUser(login, password)) {
        //    console.log("ok");
        //    return false;
        //}
        //else {
        //    AddIncorrectLogingLabel();
        //    return false;
        //}
    };
}