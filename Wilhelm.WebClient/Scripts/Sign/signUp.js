(() => {
    window.onload = function () {
        document.getElementById("signUpBtn").onclick = () => signUpClick();
        document.getElementById("signInBtn").onclick = () => {
            window.location.href = "/Sign/SignInPage";
        }
    }

    function signUpClick() {

        var loginInput = document.getElementById("login");
        var passwordInput = document.getElementById("password");
        var confirmPasswordInput = document.getElementById("confirmPassword");

        ////var jsonToSend = JSON.stringify(activities);
        $.get("http://localhost:8080/api/account?login=" + loginInput.value + "&password=" + passwordInput.value + "&confirmPassword=" + confirmPasswordInput.value)
            .done((data) => {
                var userId = data.Dto.Id;
                window.location.href = "/Pages/HomePage?userId=" + userId;
            });
    }
})();