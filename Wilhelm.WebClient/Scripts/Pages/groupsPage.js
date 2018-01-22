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
            Name: document.getElementById("taskName").value,
            OwnerId: userId,
        };
        if (selectedGroupId == -1) {
            group.Id = -1;
            config.Tasks.push(group);
        }
        else {
            for (var j = 0; j < config.Groups.length; j++) {
                if (config.Groups[j].Id == selectedGroupId) {
                    config.GRoups[j] = group;
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
        var group = config.GRoups.filter(function (el) {
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
        ShowAllTasks);
    }
    // used in AssignClick
    function ShowAllTasks() {
        tasks = config.Tasks;
        var tasksDivs = $("#groupTasks").find(".groupTaskItem");
        for (let i = 0; i < tasksDivs.length; i++) {
            tasksDivs[i].onclick = function () { AddDeleteGroupFromGroup(tasksDivs[i]); };
            tasks = tasks.filter(function (el) { return el.Id != tasksDivs[i].taskId; });
        }

        var groupsDiv = document.getElementById("taskGroups");
        for (var i = 0; i < tasks.length; i++) {
            let group = document.createElement("div");
            group.groupId = tasks[i].Id;
            group.classList.add("taskGroupItem");
            var label = document.createElement("label");
            label.innerText = tasks[i].Name;
            var p = document.createElement("p");
            p.innerText = tasks[i].Description;
            group.appendChild(label);
            group.appendChild(p);
            group.onclick = function () { AddDeleteGroupFromGroup(group); };
            groupsDiv.appendChild(group);
        }
    }
    function AddDeleteGroupFromGroup(group) {
        if (group.classList.contains("groupInTask"))
            group.classList.remove("groupInTask");
        else
            group.classList.add("groupInTask");
    }
    //~buttons

    //tasks
    function LoadGroups() {
        var tasks = config.Tasks;
        var tasksDiv = document.getElementById("tasks");
        while (tasksDiv.children.length > 1) // 0 - New task
            tasksDiv.removeChild(tasksDiv.lastChild);
        for (var i = 0; i < tasks.length; i++) {
            if (tasks[i].Archivized === true)
                continue;
            let newButton = document.createElement("button");
            newButton.classList.add("taskButton");
            newButton.taskId = tasks[i].Id;
            newButton.innerText = tasks[i].Name;
            newButton.onclick = function () {
                shownAllTasks = false;
                var task = tasks.filter(function (el) { return el.Id == newButton.taskId; })[0];
                ShowGroupDetails(task);
                SetactiveGroup(newButton);
            }
            tasksDiv.appendChild(newButton);
        }
    };
    function SetactiveGroup(task) {
        var selected = document.getElementsByClassName("activeGroup");
        if (selected.length > 0)
            selected[0].classList.remove("activeGroup");
        task.classList.add("activeGroup");
    };
    function ShowGroupDetails(el) {
        document.getElementById("taskName").value = el.Name;
        document.getElementById("taskDescription").value = el.Description;
        document.getElementById("taskStartDate").value = el.StartDate;
        document.getElementById("taskFrequency").value = el.Frequency;

        var groups = el.Groups;
        var groupsDiv = document.getElementById("taskGroups");
        while (groupsDiv.firstChild)
            groupsDiv.removeChild(groupsDiv.firstChild);
        for (var i = 0; i < groups.length; i++) {
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
    function NewGroupClick() {
        shownAllTasks = false;
        SetactiveGroup(document.getElementById("newTask"));

        document.getElementById("taskName").value = "New Task";
        document.getElementById("taskDescription").value = "Your description";
        document.getElementById("taskStartDate").value = new Date().toISOString().substr(0, 16);
        document.getElementById("taskFrequency").value = 1;
        var groupsDiv = document.getElementById("taskGroups");
        while (groupsDiv.firstChild)
            groupsDiv.removeChild(groupsDiv.firstChild);
    }
    //~tasks

    //config
    function sendNewConfig(id) {
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
