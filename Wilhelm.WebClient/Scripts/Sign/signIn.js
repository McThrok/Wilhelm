(() => {
    window.onload = function () {
        document.getElementById("signInBtn").onclick = () => signInClick();
        document.getElementById("signUpBtn").onclick = () => {
            window.location.href = "/Sign/SignUpPage";
        }
    }

    function signInClick() {

        var loginInput = document.getElementById("login");
        var passwordInput = document.getElementById("password");

        $.get("http://localhost:8080/api/account?login=" + loginInput.value + "&password=" + passwordInput.value).done((data) => {
            var userId = data.Dto.Id;
            window.location.href = "/Pages/HomePage?userId=" + userId;
        });
    }
})();