(() => {

    var config = "";
    var userId = "";

    window.onload = function () {
        var applyBtn = document.getElementById("apply_task");
        var resetBtn = document.getElementById("reset_task");
        var deleteBtn = document.getElementById("delete_task");

        var dataDiv = document.getElementById("dataDiv");
        config = JSON.parse(dataDiv.dataset.config);
        userId = dataDiv.dataset.userId;

        applyBtn.onclick = () => applyCLick();
        resetBtn.onclick = () => resetCLick();
        deleteBtn.onclick = () => deleteCLick();
    }

    function applyCLick() {
    }

    function resetCLick() {
    }

    function deleteCLick() {
    }


    //function ConvertDatesToIso(config) {
    //    for (var i = 0; i < config.Tasks.length; i++) {
    //        config.Tasks[i].StartData = config.Tasks[i].StartData.substring(0, 10).split("-").join("/");
    //    }
    //}
    //function ConvertDatesFromIso{
    //    for (var i = 0; i < config.Tasks.length; i++) {
    //        config.Tasks[i].StartData = config.Tasks[i].StartData.split("/").join("-") + "";
    //    }
    //}
    function sendNewConfig(id) {

        $.ajax({
            url: "http://localhost:8080/api/ConfigurationActivities?userId=" + id,
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify(config),
        })
    }


})();