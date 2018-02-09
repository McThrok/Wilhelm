(() => {
    var shownAllTasks = false;
    var userId = "";

    window.onload = function () {
        var dataDiv = document.getElementById("dataDiv");
        userId = dataDiv.dataset.userid;

        var applyBtn = document.getElementById("applyTask");
        var resetBtn = document.getElementById("resetTask");
        var deleteBtn = document.getElementById("deleteTask");
        var assignBtn = document.getElementById("assignToGroup");

        applyBtn.onclick = () => ApplyCLick();
        resetBtn.onclick = () => ResetCLick();
        deleteBtn.onclick = () => DeleteCLick();
        assignBtn.onclick = function () { AssignClick(); };

        var newGroup = document.getElementById("newTask");
        newGroup.onclick = NewGroupClick;
        newGroup.groupId = -1;

        NewGroupClick();
        GetGroups();

        //menu
        var selectedMenu = document.getElementsByClassName("selectedMenu");
        if (selectedMenu.length > 0)
            selectedMenu[0].classList.remove("selectedMenu");
        document.getElementById("groupsButton").classList.add("selectedMenu");
    }
    //buttons
    function ApplyCLick() {
        var selectedGroupId = document.getElementsByClassName("activeTask")[0].groupId;
        var tasksDivs = $("#taskGroups").find(".groupInTask");
        var tasks = [];
        for (var i = 0; i < tasksDivs.length; i++) {
            tasks.push(tasksDivs[i].taskId);
        }
        var group = {
            Archivized: false,
            Description: document.getElementById("taskDescription").value,
            Id: selectedGroupId,
            Name: document.getElementById("taskName").value,
            OwnerId: userId,
        };
        if (selectedGroupId === -1) {
            group.Id = -1;
            SendGroup("POST", group, tasks);
        }
        else {
            SendGroup("PUT", group, tasks);
        }
    }
    function ResetCLick() {
        var selectedGroupId = document.getElementsByClassName("activeTask")[0].groupId;
        GetGroupDetails(selectedGroupId);
        shownAllTasks = false;
    }
    function DeleteCLick() {
        var selectedGroupId = document.getElementsByClassName("activeTask")[0].groupId;
        DeleteGroup(selectedGroupId);
    }
    function AssignClick() {
        if (shownAllTasks)
            return;
        shownAllTasks = true;
        GetTasks();
    }
    // used in AssignClick
    function ShowAllTasks(tasks) {
        var tasksDivs = $("#taskGroups").find(".taskGroupItem");
        for (let i = 0; i < tasksDivs.length; i++) {
            tasksDivs[i].onclick = function () { AddDeleteTaskFromGroup(tasksDivs[i]); };
            tasks = tasks.filter(function (el) { return el.m_Item1 !== tasksDivs[i].taskId; });
        }

        var tasksDiv = document.getElementById("taskGroups");
        for (var i = 0; i < tasks.length; i++) {
            let task = document.createElement("div");
            task.taskId = tasks[i].m_Item1;
            task.classList.add("taskGroupItem");
            var label = document.createElement("label");
            label.innerText = tasks[i].m_Item2;
            var p = document.createElement("p");
            p.innerText = tasks[i].m_Item3;
            task.appendChild(label);
            task.appendChild(p);
            task.onclick = function () { AddDeleteTaskFromGroup(task); };
            tasksDiv.appendChild(task);
        }
    }
    function AddDeleteTaskFromGroup(group) {
        if (group.classList.contains("groupInTask"))
            group.classList.remove("groupInTask");
        else
            group.classList.add("groupInTask");
    }
    //~buttons

    //tasks
    function ShowGroups(groups) {
        var groupsDiv = document.getElementById("tasks");
        while (groupsDiv.children.length > 1) // 0 - New group
            groupsDiv.removeChild(groupsDiv.lastChild);
        for (var i = 0; i < groups.length; i++) {
            if (groups[i].Archivized === true)
                continue;
            let newButton = document.createElement("button");
            newButton.classList.add("taskButton");
            newButton.groupId = groups[i].Key;
            newButton.innerText = groups[i].Value;
            newButton.onclick = function () {
                shownAllTasks = false;
                SetActiveGroup(newButton);
                Show();
                GetGroupDetails(newButton.groupId);
            }
            groupsDiv.appendChild(newButton);
        }
    };
    function SetActiveGroup(group) {
        var selected = document.getElementsByClassName("activeTask");
        if (selected.length > 0)
            selected[0].classList.remove("activeTask");
        group.classList.add("activeTask");
    };
    function ShowGroupDetails(el) {
        document.getElementById("taskName").value = el.Name;
        document.getElementById("taskDescription").value = el.Description;

        var tasksDiv = document.getElementById("taskGroups");
        while (tasksDiv.firstChild)
            tasksDiv.removeChild(tasksDiv.firstChild);

        var tasks = el.Tasks;
        for (var i = 0; i < tasks.length; i++) {
            if (tasks[i].Archivized)
                continue;
            var task = document.createElement("div");
            task.classList.add("groupInTask");
            task.classList.add("taskGroupItem");
            task.taskId = tasks[i].Id;
            var label = document.createElement("label");
            label.innerText = tasks[i].Name;
            var p = document.createElement("p");
            p.innerText = tasks[i].Description;
            task.appendChild(label);
            task.appendChild(p);
            tasksDiv.appendChild(task);
        }
    }
    function NewGroupClick() {
        shownAllTasks = false;
        SetActiveGroup(document.getElementById("newTask"));

        document.getElementById("taskName").value = "New group";
        document.getElementById("taskDescription").value = "Your description";
        var tasksDiv = document.getElementById("taskGroups");
        while (tasksDiv.firstChild)
            tasksDiv.removeChild(tasksDiv.firstChild);
        Hide();
    }
    //~tasks

    //config
    function SendGroup(ttype, group, tasks) {
        $.ajax({
            url: "http://localhost:8080/api/Configuration/group",
            type: ttype,
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify({ "Key": group, "Value": tasks }),
            success: function () {
                NewGroupClick();
                GetGroups();
            }
        })
    }
    function GetGroups() {
        $.ajax({
            url: "http://localhost:8080/api/groups/names?userId=" + userId,
            type: 'GET',
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                ShowGroups(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }
    function GetGroupDetails(groupId) {
        $.ajax({
            url: "http://localhost:8080/api/groups/details?groupId=" + groupId,
            type: 'GET',
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                ShowGroupDetails(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }
    function GetTasks() {
        $.ajax({
            url: "http://localhost:8080/api/groups/tasks?userId=" + userId,
            type: 'GET',
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                ShowAllTasks(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }
    function DeleteGroup(groupId) {
        $.ajax({
            url: "http://localhost:8080/api/groups?groupId=" + groupId,
            type: 'DELETE',
            success: function () {
                NewGroupClick();
                GetGroups();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }
    //~config
    function Hide() {
        var resetBtn = document.getElementById("resetTask");
        resetBtn.style.visibility = "hidden";
        var deleteBtn = document.getElementById("deleteTask");
        deleteBtn.style.visibility = "hidden";
    }
    function Show() {
        var resetBtn = document.getElementById("resetTask");
        resetBtn.style.visibility = "visible";
        var deleteBtn = document.getElementById("deleteTask");
        deleteBtn.style.visibility = "visible";
    }
})();
