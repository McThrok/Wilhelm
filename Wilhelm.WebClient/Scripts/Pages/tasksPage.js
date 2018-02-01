(() => {
    var shownAllGroups = false;
    var userId = "";
    var offset = 0;
    var count = 8;

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

        var newTask = document.getElementById("newTask");
        newTask.onclick = NewTaskClick;
        newTask.taskId = -1;

        NewTaskClick();
        GetTasks();
        //menu
        var selectedMenu = document.getElementsByClassName("selectedMenu");
        if (selectedMenu.length > 0)
            selectedMenu[0].classList.remove("selectedMenu");
        document.getElementById("tasksButton").classList.add("selectedMenu");
    }
    //buttons
    function ApplyCLick() {
        var selectedTaskId = document.getElementsByClassName("activeTask")[0].taskId;
        var groupsDivs = $("#taskGroups").find(".groupInTask");
        var groups = [];
        for (var i = 0; i < groupsDivs.length; i++) {
            groups.push(groupsDivs[i].groupId);
        }
        var task = GetTask(false);
        if (selectedTaskId === -1) {
            task.Id = -1;
            SendTask("POST", task, groups);
        }
        else
            SendTask("PUT", task, groups);
    }
    function ResetCLick() {
        var selectedTaskId = document.getElementsByClassName("activeTask")[0].taskId;
        GetTaskDetails(selectedTaskId);
        shownAllGroups = false;
    }
    function DeleteCLick() {
        var selectedTaskId = document.getElementsByClassName("activeTask")[0].taskId;
        DeleteTask(selectedTaskId);
    }
    function AssignClick() {
        if (shownAllGroups)
            return;
        shownAllGroups = true;
        GetGroups();
    }
    // used in AssignClick
    function ShowAllGroups(groups) {
        var groupsDivs = $("#taskGroups").find(".taskGroupItem");
        for (let i = 0; i < groupsDivs.length; i++) {
            groupsDivs[i].onclick = function () { AddDeleteGroupFromTask(groupsDivs[i]); };
            groups = groups.filter(function (el) { return el.m_Item1 !== groupsDivs[i].groupId; });
        }

        var groupsDiv = document.getElementById("taskGroups");
        for (var i = 0; i < groups.length; i++) {
            let group = document.createElement("div");
            group.groupId = groups[i].m_Item1;
            group.classList.add("taskGroupItem");
            var label = document.createElement("label");
            label.innerText = groups[i].m_Item2;
            var p = document.createElement("p");
            p.innerText = groups[i].m_Item3;
            group.appendChild(label);
            group.appendChild(p);
            group.onclick = function () { AddDeleteGroupFromTask(group); };
            groupsDiv.appendChild(group);
        }
    }
    function AddDeleteGroupFromTask(group) {
        if (group.classList.contains("groupInTask"))
            group.classList.remove("groupInTask");
        else
            group.classList.add("groupInTask");
    }
    //~buttons

    //tasks
    function AddTasks(tasks, addButton) {
        var tasksDiv = document.getElementById("tasks");
        while (tasksDiv.children.length > 1) // 0 - New task
            tasksDiv.removeChild(tasksDiv.lastChild);
        for (var i = 0; i < tasks.length; i++) {
            if (tasks[i].Archivized === true)
                continue;
            let newButton = document.createElement("button");
            newButton.classList.add("taskButton");
            newButton.taskId = tasks[i].Key;
            newButton.innerText = tasks[i].Value;
            newButton.onclick = function () {
                shownAllGroups = false;
                SetActiveTask(newButton);
                Show();
                GetTaskDetails(newButton.taskId);
            }
            tasksDiv.appendChild(newButton);
        }
        if (addButton)
            tasksDiv.appendChild(GetButton());
    };
    function SetActiveTask(task) {
        var selected = document.getElementsByClassName("activeTask");
        if (selected.length > 0)
            selected[0].classList.remove("activeTask");
        task.classList.add("activeTask");
    };
    function ShowTaskDetails(el) {
        document.getElementById("taskName").value = el.Name;
        document.getElementById("taskDescription").value = el.Description;
        document.getElementById("taskStartDate").value = el.StartDate;
        document.getElementById("taskFrequency").value = el.Frequency;

        var groups = el.Groups;
        var groupsDiv = document.getElementById("taskGroups");
        while (groupsDiv.firstChild)
            groupsDiv.removeChild(groupsDiv.firstChild);
        for (var i = 0; i < groups.length; i++) {
            if (groups[i].Archivized)
                continue;
            var group = document.createElement("div");
            group.classList.add("groupInTask");
            group.classList.add("taskGroupItem");
            group.groupId = groups[i].Id;
            var label = document.createElement("label");
            label.innerText = groups[i].Name;
            var p = document.createElement("p");
            p.innerText = groups[i].Description;
            group.appendChild(label);
            group.appendChild(p);
            groupsDiv.appendChild(group);
        }
    }
    function NewTaskClick() {
        shownAllGroups = false;
        SetActiveTask(document.getElementById("newTask"));

        document.getElementById("taskName").value = "New Task";
        document.getElementById("taskDescription").value = "Your description";
        var tzoffset = (new Date()).getTimezoneOffset() * 60000;
        document.getElementById("taskStartDate").value = (new Date(Date.now() - tzoffset)).toISOString().slice(0, -1).substr(0, 16);
        document.getElementById("taskFrequency").value = 1;
        var groupsDiv = document.getElementById("taskGroups");
        while (groupsDiv.firstChild)
            groupsDiv.removeChild(groupsDiv.firstChild);
        Hide();
    }
    //~tasks

    //config
    function SendTask(ttype, task, groups) {
        $.ajax({
            url: "http://localhost:8080/api/tasks",
            type: ttype,
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify({ "Key": task, "Value": groups }),
            success: function () {
                NewTaskClick();
                GetTasks();
            }
        })
    }
    function GetTasks() {
        $.ajax({
            url: "http://localhost:8080/api/tasks/names?userId=" + userId + "&offset=" + count * offset + "&amount=" + count,
            type: 'GET',
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                AddTasks(data.m_Item2, data.m_Item1);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }
    function GetTaskDetails(taskId) {
        $.ajax({
            url: "http://localhost:8080/api/tasks/details?taskId=" + taskId,
            type: 'GET',
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                ShowTaskDetails(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }
    function GetGroups() {
        $.ajax({
            url: "http://localhost:8080/api/tasks/groups?userId=" + userId,
            type: 'GET',
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                ShowAllGroups(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }
    function DeleteTask(taskId) {
        $.ajax({
            url: "http://localhost:8080/api/tasks?taskId=" + taskId,
            type: 'DELETE',
            success: function () {
                NewTaskClick();
                GetTasks();
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
    function GetTask(archivized) {
        var selectedTaskId = document.getElementsByClassName("activeTask")[0].taskId;
        return task = {
            Archivized: archivized,
            Description: document.getElementById("taskDescription").value,
            Frequency: document.getElementById("taskFrequency").value,
            Id: selectedTaskId,
            Name: document.getElementById("taskName").value,
            OwnerId: userId,
            StartDate: document.getElementById("taskStartDate").value,
        };
    }

    function GetButton() {
        var b = document.createElement("button");
        b.id = "load";
        b.innerHTML = "Load";
        b.onclick = function () {
            offset++;
            GetTasks();
        }
        return b;
    }
})();
