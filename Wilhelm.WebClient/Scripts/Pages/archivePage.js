(() => {
    window.onload = function () {
        var applyBtn = document.getElementById("applyButton");
        applyBtn.onclick = () => apply(JSON.parse(applyBtn.dataset.activities), applyBtn.dataset.userId);
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
            url: "http://localhost:8080/api/ArchiveActivities?userId=" + id,
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify(activities),
        })
    }

})();