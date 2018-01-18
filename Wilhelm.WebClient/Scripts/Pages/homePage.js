(() => {
    window.onload = function () {
        var applyBtn = document.getElementById("applyButton");
        applyBtn.onclick = () => apply(JSON.parse(applyBtn.dataset.activities));
    }

    function apply(activities) {
        var checkBoxes = document.getElementsByClassName("qwe");

        for (var i = 0; i < checkBoxes.length; i++)
            for (var j = 0; j < activities.length; j++)
                if (Number(checkBoxes[i].dataset.activityid) === activities[j].Id) {
                    activities[j].IsDone = checkBoxes[i].checked;
                    break;
                }

        //var jsonToSend = JSON.stringify(activities);
        //$.get("http://localhost:8080/api/ActiveActivities?userId=1").done(() => {
        //    console.log("qwe");
        //});

        $.ajax({
            url: "http://localhost:8080/api/ActiveActivities?userId=1",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify(activities),
        })
        //$.post("http://localhost:8080/api/ActiveActivities?userId=1", JSON.stringify(activities), function () { }, "application/json;charset=utf-8");

        //$.ajax({
        //    type: "POST",
        //    url: "http://localhost:8080/api/ActiveActivities",
        //    data: activities,
        //    crossDomain: true,
        //});
    }

})();