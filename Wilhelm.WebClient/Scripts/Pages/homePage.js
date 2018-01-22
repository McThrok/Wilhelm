(() => {
    window.onload = function () {
        var applyBtn = document.getElementById("applyButton");
        applyBtn.onclick = () => apply(JSON.parse(applyBtn.dataset.activities), applyBtn.dataset.userid);

        var selectedMenu = document.getElementsByClassName("selectedMenu");
        if (selectedMenu.length > 0)
            selectedMenu[0].classList.remove("selectedMenu");
        document.getElementById("homeButton").classList.add("selectedMenu");
    }

    function apply(activities, id) {
        var checkBoxes = document.getElementsByClassName("qwe");

        for (var i = 0; i < checkBoxes.length; i++)
            for (var j = 0; j < activities.length; j++)
                if (Number(checkBoxes[i].dataset.activityid) === activities[j].Id) {
                    activities[j].IsDone = checkBoxes[i].checked;
                    break;
                }

        $.ajax({
            url: "http://localhost:8080/api/ActiveActivities?userId=" + id,
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify(activities),
        })
    }

})();