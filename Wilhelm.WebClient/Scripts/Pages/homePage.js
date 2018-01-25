(() => {
    var c = true;

    window.onload = function () {
        var dataDiv = document.getElementById("config");
        var activities = JSON.parse(dataDiv.dataset.activities);
        var userId = dataDiv.dataset.userid;

        //
        var homeDiv = document.getElementById("home");
        for (let i = 0; i < activities.length; i++) {
            if (activities[i].Task.Archivized)
                continue;
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
                if (c) {
                    checkbox.checked = !checkbox.checked;
                    SaveActivities(activities[i].Id, checkbox.checked);
                }
                c = true;
            }
        }
    }
    //menu
    var selectedMenu = document.getElementsByClassName("selectedMenu");
    if (selectedMenu.length > 0)
        selectedMenu[0].classList.remove("selectedMenu");
    document.getElementById("homeButton").classList.add("selectedMenu");

    function SaveActivities(id, value) {
        $.ajax({
            url: "http://localhost:8080/api/ArchiveActivities?activityId=" + id + "&value=" + value,
            type: "PUT",
        })
    }

})();