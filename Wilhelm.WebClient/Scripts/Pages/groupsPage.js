(() => {
    var shownAllTasks = false;
    var config = "";
    var userId = "";

    window.onload = function () {
        var dataDiv = document.getElementById("dataDiv");
        config = JSON.parse(dataDiv.dataset.config);
        userId = dataDiv.dataset.userid;

        var applyBtn = document.getElementById("applyGroup");
        var resetBtn = document.getElementById("resetGroup");
        var deleteBtn = document.getElementById("deleteGroup");
        var assignBtn = document.getElementById("assignToTask");

        applyBtn.onclick = () => ApplyCLick();
        resetBtn.onclick = () => ResetCLick();
        deleteBtn.onclick = () => DeleteCLick();
        assignBtn.onclick = function () { AssignClick(); };

        var newGroup = document.getElementById("newGroup");
        newGroup.onclick = NewGroupClick;
        newGroup.groupId = -1;

        NewGroupClick();
        LoadGroups();

        //menu
        var selectedMenu = document.getElementsByClassName("selectedMenu");
        if (selectedMenu.length > 0)
            selectedMenu[0].classList.remove("selectedMenu");
        document.getElementById("groupsButton").classList.add("selectedMenu");
    }
    //buttons
    function ApplyCLick() {
        var selectedGroupId = document.getElementsByClassName("activeGroup")[0].groupId;
        var tasksDivs = $("#groupTasks").find(".taskInGroup");
        var tasks = [];
        for (var i = 0; i < tasksDivs.length; i++) {
            var task = {
                Id: tasksDivs[i].taskId,
                Name: tasksDivs[i].firstChild.innerText,
                Description: tasksDivs[i].lastChild.innerText
            }
            tasks.push(task);
        }
        var group = {
            Archivized: false,
            Description: document.getElementById("groupDescription").value,
            Tasks: tasks,
            Id: selectedGroupId,
            Name: document.getElementById("groupName").value,
            OwnerId: userId,
        };
        if (selectedGroupId == -1) {
            group.Id = -1;
            config.Tasks.push(group);
        }
        else {
            for (var j = 0; j < config.Groups.length; j++) {
                if (config.Groups[j].Id == selectedGroupId) {
                    config.Groups[j] = group;
                    break;
                }
            }
        }
        sendNewConfig(userId);
        LoadGroups();
        NewGroupClick();
        shownAllTasks = false;
    }
    function ResetCLick() {
        var selectedGroupId = document.getElementsByClassName("activeGroup")[0].groupId;
        var group = config.Groups.filter(function (el) {
            return el.Id == selectedGroupId
        });
        ShowGroupDetails(group[0]);
        shownAllTasks = false;
    }
    function DeleteCLick() {
        var selectedGroupId = document.getElementsByClassName("activeGroup")[0].groupId;
        for (var j = 0; j < config.Tasks.length; j++) {
            if (config.Groups[j].Id == selectedGroupId) {
                config.Groups[j].Archivized = true;
                break;
            }
        }
        sendNewConfig(userId);
        LoadGroups();
        NewGroupClick();
        shownAllTasks = false;
    }
    function AssignClick() {
        if (shownAllTasks)
            return;
        shownAllTasks = true;
        ShowAllTasks();
    }
    // used in AssignClick
    function ShowAllTasks() {
        tasks = config.Tasks;
        var tasksDivs = $("#groupTasks").find(".groupTaskItem");
        for (let i = 0; i < tasksDivs.length; i++) {
            tasksDivs[i].onclick = function () { AddDeleteTaskFromGroup(tasksDivs[i]); };
            tasks = tasks.filter(function (el) { return el.Id != tasksDivs[i].taskId; });
        }

        var tasksDiv = document.getElementById("groupTasks");
        for (var i = 0; i < tasks.length; i++) {
            let task = document.createElement("div");
            task.taskId = tasks[i].Id;
            task.classList.add("groupTaskItem");
            var label = document.createElement("label");
            label.innerText = tasks[i].Name;
            var p = document.createElement("p");
            p.innerText = tasks[i].Description;
            task.appendChild(label);
            task.appendChild(p);
            task.onclick = function () { AddDeleteTaskFromGroup(task); };
            tasksDiv.appendChild(task);
        }
    }
    function AddDeleteTaskFromGroup(group) {
        if (group.classList.contains("taskInGroup"))
            group.classList.remove("taskInGroup");
        else
            group.classList.add("taskInGroup");
    }
    //~buttons

    //tasks
    function LoadGroups() {
        var groups = config.Groups;
        var groupsDiv = document.getElementById("groups");
        while (groupsDiv.children.length > 1) // 0 - New group
            groupsDiv.removeChild(groupsDiv.lastChild);
        for (var i = 0; i < groups.length; i++) {
            if (groups[i].Archivized === true)
                continue;
            let newButton = document.createElement("button");
            newButton.classList.add("groupButton");
            newButton.groupId = groups[i].Id;
            newButton.innerText = groups[i].Name;
            newButton.onclick = function () {
                shownAllTasks = false;
                var group = groups.filter(function (el) { return el.Id == newButton.groupId; })[0];
                ShowGroupDetails(group);
                SetActiveGroup(newButton);
            }
            groupsDiv.appendChild(newButton);
        }
    };
    function SetActiveGroup(group) {
        var selected = document.getElementsByClassName("activeGroup");
        if (selected.length > 0)
            selected[0].classList.remove("activeGroup");
        group.classList.add("activeGroup");
    };
    function ShowGroupDetails(el) {
        document.getElementById("groupName").value = el.Name;
        document.getElementById("groupDescription").value = el.Description;

        var tasksDiv = document.getElementById("groupTasks");
        while (tasksDiv.firstChild)
            tasksDiv.removeChild(tasksDiv.firstChild);

        var tasks = el.Tasks;
        for (var i = 0; i < tasks.length; i++) {
            var task = document.createElement("div");
            task.classList.add("taskInGroup");
            task.classList.add("groupTaskItem");
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
        SetActiveGroup(document.getElementById("newGroup"));

        document.getElementById("groupName").value = "New group";
        document.getElementById("groupDescription").value = "Your description";
        var tasksDiv = document.getElementById("groupTasks");
        while (tasksDiv.firstChild)
            tasksDiv.removeChild(tasksDiv.firstChild);
    }
    //~tasks

    //config
    function sendNewConfig(id) {
        var wqe = config;
        $.ajax({
            url: "http://localhost:8080/api/Configuration?userId=" + id,
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify(config),
            success: function () {
                location.reload();
            }
        })
    }
    //~config
})();
