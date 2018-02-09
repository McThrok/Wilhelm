(() => {
    var c = true;

    window.onload = function () {
        var userId = document.getElementById("config").dataset.userid;
        GetActivities(userId);
    }

    function ShowTodayActivities(activities) {
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
                SaveActivity(activities[i].Id, checkbox.checked);
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
                    SaveActivity(activities[i].Id, checkbox.checked);
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

    function SaveActivity(id, value) {
        $.ajax({
            url: "http://localhost:8080/api/ArchiveActivities?activityId=" + id + "&value=" + value,
            type: "PUT",
        })
    }

    function GetActivities(userId) {
        $.ajax({
            url: "http://localhost:8080/api/ActiveActivities?userId=" + userId,
            type: 'GET',
            success: function (data) {
                ShowTodayActivities(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }

})();