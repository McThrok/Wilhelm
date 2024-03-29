(() => {
    window.onload = function () {
        document.getElementById("js-button").onclick = function() {
            var loginInput = document.getElementById("login");
            var passwordInput = document.getElementById("password");
            var confirmPasswordInput = document.getElementById("confirmPassword");

            var check = true;
            if (validate(loginInput) == false) {
                showValidate(loginInput, "Username is required");
                check = false;
            }
            if (validate(passwordInput) == false) {
                showValidate(passwordInput, "Password is required");
                check = false;
            }
            if (validate(confirmPasswordInput) == false) {
                showValidate(confirmPasswordInput, "Confirm password is required");
                check = false;
            }
            if (check) {
                $.get("http://localhost:8080/api/account?login=" + loginInput.value + "&password=" + passwordInput.value + "&confirmPassword=" + confirmPasswordInput.value)
                    .done((data) => {
                        if (data.Dto === null) {
                            showValidate(loginInput, "Username or password or confirm password is not correct");
                        }
                        window.location.href = "/Pages/HomePage?userId=" + data.Dto.Id;
                    });
            }
        }
        document.getElementById("js-link").onclick = () => {
            window.location.href = "/Sign/SignInPage";
        }

        $('.validateForm .inputField').each(function () {
            $(this).focus(function () {
                hideValidate(this);
            });
        });
    }

    function validate(input) {
        if ($(input).val().trim() == '')
            return false;
        else
            return true;
    }

    function showValidate(input, message) {
        var thisAlert = $(input).parent();
        thisAlert[0].dataset.validate = message;
        $(thisAlert).addClass('alertValidate');
    }

    function hideValidate(input) {
        var thisAlert = $(input).parent();
        $(thisAlert).removeClass('alertValidate');
    }

})();