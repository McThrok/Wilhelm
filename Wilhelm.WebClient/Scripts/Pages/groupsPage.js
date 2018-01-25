(() => {
    var shownAllTasks = false;
    var config = "";
    var userId = "";

    window.onload = function () {
        var dataDiv = document.getElementById("dataDiv");
        config = JSON.parse(dataDiv.dataset.config);
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
        LoadGroups();

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
        if (selectedGroupId == -1) {
            group.Id = -1;
            sendNewConfig("POST", group, tasks);
        }
        else {
            sendNewConfig("PUT", group, tasks);
        }
       
    }
    function ResetCLick() {
        var selectedGroupId = document.getElementsByClassName("activeTask")[0].groupId;
        var group = config.Groups.filter(function (el) {
            return el.Id == selectedGroupId
        });
        ShowGroupDetails(group[0]);
        shownAllTasks = false;
    }
    function DeleteCLick() {
        var selectedGroupId = document.getElementsByClassName("activeTask")[0].groupId;
        var tasksDivs = $("#taskGroups").find(".groupInTask");
        var tasks = [];
        for (var i = 0; i < tasksDivs.length; i++) {
            tasks.push(tasksDivs[i].taskId);
        }
        var group = {
            Archivized: true,
            Description: document.getElementById("taskDescription").value,
            Id: selectedGroupId,
            Name: document.getElementById("taskName").value,
            OwnerId: userId,
        };
        sendNewConfig("PUT", group, tasks);
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
        var tasksDivs = $("#taskGroups").find(".taskGroupItem");
        for (let i = 0; i < tasksDivs.length; i++) {
            tasksDivs[i].onclick = function () { AddDeleteTaskFromGroup(tasksDivs[i]); };
            tasks = tasks.filter(function (el) { return el.Id != tasksDivs[i].taskId; });
        }

        var tasksDiv = document.getElementById("taskGroups");
        for (var i = 0; i < tasks.length; i++) {
            let task = document.createElement("div");
            task.taskId = tasks[i].Id;
            task.classList.add("taskGroupItem");
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
        if (group.classList.contains("groupInTask"))
            group.classList.remove("groupInTask");
        else
            group.classList.add("groupInTask");
    }
    //~buttons

    //tasks
    function LoadGroups() {
        var groups = config.Groups;
        var groupsDiv = document.getElementById("tasks");
        while (groupsDiv.children.length > 1) // 0 - New group
            groupsDiv.removeChild(groupsDiv.lastChild);
        for (var i = 0; i < groups.length; i++) {
            if (groups[i].Archivized === true)
                continue;
            let newButton = document.createElement("button");
            newButton.classList.add("taskButton");
            newButton.groupId = groups[i].Id;
            newButton.innerText = groups[i].Name;
            newButton.onclick = function () {
                shownAllTasks = false;
                var group = groups.filter(function (el) { return el.Id == newButton.groupId; })[0];
                ShowGroupDetails(group);
                SetActiveGroup(newButton);
                Show();
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
    function sendNewConfig(ttype, group, tasks) {
        $.ajax({
            url: "http://localhost:8080/api/Configuration/group",
            type: ttype,
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify({ "Key": group, "Value": tasks }),
            success: function () {
                location.reload();
            }
        })
    }
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
    //~config
})();
