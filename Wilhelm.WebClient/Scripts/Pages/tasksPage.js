(() => {
    var shownAllGroups = false;
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

        var newTask = document.getElementById("newTask");
        newTask.onclick = NewTaskClick;
        newTask.taskId = -1;

        NewTaskClick();
        LoadTasks();

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
            var group = {
                Id: groupsDivs[i].groupId,
                Name: groupsDivs[i].firstChild.innerText,
                Description: groupsDivs[i].lastChild.innerText
            }
            groups.push(group);
        }
        var task = {
            Archivized: false,
            Description: document.getElementById("taskDescription").value,
            Frequency: document.getElementById("taskFrequency").value,
            Groups: groups,
            Id: selectedTaskId,
            Name: document.getElementById("taskName").value,
            OwnerId: userId,
            StartDate: document.getElementById("taskStartDate").value,
        };
        if (selectedTaskId == -1) {
            task.Id = -1;
            config.Tasks.push(task);
        }
        else {
            for (var j = 0; j < config.Tasks.length; j++) {
                if (config.Tasks[j].Id == selectedTaskId) {
                    config.Tasks[j] = task;
                    break;
                }
            }
        }
        sendNewConfig(userId);
        LoadTasks();
        NewTaskClick();
        shownAllGroups = false;
    }
    function ResetCLick() {
        var selectedTaskId = document.getElementsByClassName("activeTask")[0].taskId;
        var task = config.Tasks.filter(function (el) {
            return el.Id == selectedTaskId
        });
        ShowTaskDetails(task[0]);
        shownAllGroups = false;
    }
    function DeleteCLick() {
        var selectedTaskId = document.getElementsByClassName("activeTask")[0].taskId;
        for (var j = 0; j < config.Tasks.length; j++) {
            if (config.Tasks[j].Id == selectedTaskId) {
                config.Tasks[j].Archivized = true;
                break;
            }
        }
        sendNewConfig(userId);
        LoadTasks();
        NewTaskClick();
        shownAllGroups = false;
    }
    function AssignClick() {
        if (shownAllGroups)
            return;
        shownAllGroups = true;
        ShowAllGroups(config.Groups);
    }
    // used in AssignClick
    function ShowAllGroups(groups) {
        var groupsDivs = $("#taskGroups").find(".taskGroupItem");
        for (let i = 0; i < groupsDivs.length; i++) {
            groupsDivs[i].onclick = function () { AddDeleteGroupFromTask(groupsDivs[i]); };
            groups = groups.filter(function (el) { return el.Id != groupsDivs[i].groupId; });
        }

        var groupsDiv = document.getElementById("taskGroups");
        for (var i = 0; i < groups.length; i++) {
            let group = document.createElement("div");
            group.groupId = groups[i].Id;
            group.classList.add("taskGroupItem");
            var label = document.createElement("label");
            label.innerText = groups[i].Name;
            var p = document.createElement("p");
            p.innerText = groups[i].Description;
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
    function LoadTasks() {
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
                shownAllGroups = false;
                var task = tasks.filter(function (el) { return el.Id == newButton.taskId; })[0];
                ShowTaskDetails(task);
                SetActiveTask(newButton);
            }
            tasksDiv.appendChild(newButton);
        }
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
