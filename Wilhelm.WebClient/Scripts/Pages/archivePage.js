(() => {
    var c = true;
    var offset = 0;
    var count = 8;
    var userId = "";

    window.onload = function () {
        userId = document.getElementById("config").dataset.userid;

        GetNewActivities(userId, offset, count);

    }

    //menu
    var selectedMenu = document.getElementsByClassName("selectedMenu");
    if (selectedMenu.length > 0)
        selectedMenu[0].classList.remove("selectedMenu");
    document.getElementById("archivesButton").classList.add("selectedMenu");

    function SaveActivities(id, value) {
        $.ajax({
            url: "http://localhost:8080/api/ArchiveActivities?activityId=" + id + "&value=" + value,
            type: "PUT",
        })
    }

    function GetNewActivities(userId, offset, count) {
        $.ajax({
            url: "http://localhost:8080/api/ArchiveActivities?userId=" + userId + "&offset=" + count * offset + "&amount=" + count,
            type: 'GET',
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                AddActivities(data.m_Item2, data.m_Item1);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }
    function AddActivities(activities, addButton) {
        var archiveDiv = document.getElementById("home");
        if (archiveDiv.children.length > 0)
            archiveDiv.removeChild(archiveDiv.lastChild);

        for (let i = 0; i < activities.length; i++) {
            let activity = document.createElement("div");
            activity.classList.add("homeItem");
            let checkbox = document.createElement("input");
            checkbox.type = "checkbox";
            checkbox.classList.add("c");
            checkbox.activityId = activities[i].Id;
            checkbox.checked = activities[i].IsDone;
            checkbox.onclick = function () {
                SaveActivities(activities[i].Id, checkbox.checked);
                c = false;
            }
            var l1 = document.createElement("label")
            l1.innerText = activities[i].Date.substring(0, 10);
            var l2 = document.createElement("label")
            l2.innerText = activities[i].Task.Name;
            var p = document.createElement("p");
            p.innerText = activities[i].Task.Description;
            activity.appendChild(checkbox);
            activity.appendChild(l1);
            activity.appendChild(l2);
            activity.appendChild(p);
            archiveDiv.appendChild(activity);
            activity.onclick = function () {
                if (c) {
                    checkbox.checked = !checkbox.checked;
                    SaveActivities(activities[i].Id, checkbox.checked);
                }
                c = true;
            }
        }

        if (addButton)
            archiveDiv.appendChild(GetButton());
    }

    function GetButton() {
        var b = document.createElement("button");
        b.id = "load";
        b.innerHTML = "Load";
        b.onclick = function () {
            offset++;
            GetNewActivities(userId, offset, count);
        }
        return b;
    }
})();