/**
 *
 * @param {string} login
 * @param {string} password
* @returns {boolean}
 */
function VerifyUser(login, password) {
    //TODO
    if (login && (login == "qwe") && (password == "qwe"))
        return true;
    else
        return false;
}

function AddIncorrectSigningLabel() {
    var incorrect = document.getElementById("incorrect");
    incorrect.style.visibility = 'visible';
    setTimeout(function () { incorrect.style.visibility = 'collapse'; }, 4000);
}
function AddEmptySigningLabel() {
    var empty = document.getElementById("empty");
    empty.style.visibility = 'visible';
    setTimeout(function () { empty.style.visibility = 'collapse'; }, 4000);
}
function SetValidation() {
    "use strict";
    document.getElementById("sign_in_form").onsubmit = function () {
        var login = document.getElementById("login").value;
        var password = document.getElementById("password").value;
        if (!login || !password) {
            AddEmptySigningLabel();
            return false;
        }
        if (VerifyUser(login, password)) {
            return true;
        }
        else {
            AddIncorrectSigningLabel();
            return false;
        }
    };
}