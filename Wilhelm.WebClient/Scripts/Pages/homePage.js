(() => {
    var activities = "";
    var userId = "";
    var c = true;
    window.onload = function () {
        var applyBtn = document.getElementById("applyButton");
        activities = JSON.parse(applyBtn.dataset.activities);
        userId = applyBtn.dataset.userid;
        applyBtn.onclick = () => apply(activities, userId);

        //
        var homeDiv = document.getElementById("home");
        for (var i = 0; i < activities.length; i++) {
            let activity = document.createElement("div");
            activity.classList.add("homeItem");
            let checkbox = document.createElement("input");
            checkbox.type = "checkbox";
            checkbox.classList.add("c");
            checkbox.activityId = activities[i].Id;
            checkbox.checked = activities[i].IsDone;
            checkbox.onclick = function () {
                c = false;
            }
            var l2 = document.createElement("label")
            l2.innerText = activities[i].Task.Name;
            var p = document.createElement("p");
            p.innerText = activities[i].Task.Description;
            activity.appendChild(checkbox);
            activity.appendChild(l2);
            activity.appendChild(p);
            homeDiv.appendChild(activity);
            activity.onclick = function () {
                if (c)
                    activity.children[0].checked = !activity.children[0].checked;
                c = true;
            }
        }
    }
    //menu
    var selectedMenu = document.getElementsByClassName("selectedMenu");
    if (selectedMenu.length > 0)
        selectedMenu[0].classList.remove("selectedMenu");
    document.getElementById("homeButton").classList.add("selectedMenu");

    function apply(activities, id) {
        var checkBoxes = document.getElementsByClassName("c");
        for (var i = 0; i < checkBoxes.length; i++)
            for (var j = 0; j < activities.length; j++)
                if (Number(checkBoxes[i].activityId) === activities[j].Id) {
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