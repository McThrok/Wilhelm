(() => {
    var shownAllGroups = false;
    var newTaskId = -1;
    var config = "";
    var userId = "";

    window.onload = function () {
        var applyBtn = document.getElementById("apply_task");
        var resetBtn = document.getElementById("reset_task");
        var deleteBtn = document.getElementById("delete_task");

        var newTask = document.getElementById("newTask");
        newTask.taskId = -1;


        var dataDiv = document.getElementById("dataDiv");
        config = JSON.parse(dataDiv.dataset.config);
        userId = dataDiv.dataset.userid;

        LoadTasks();
        NewTaskClick();
        applyBtn.onclick = () => applyCLick();
        resetBtn.onclick = () => resetCLick();
        deleteBtn.onclick = () => deleteCLick();



        var assignToGroup = document.getElementById("assignToGroup");
        assignToGroup.onclick = function () {
            if (shownAllGroups)
                return;
            shownAllGroups = true;
            ShowGroups(config.Groups);
        }

        var selectedMenu = document.getElementsByClassName("selectedMenu");
        if (selectedMenu.length > 0)
            selectedMenu[0].classList.remove("selectedMenu");
        document.getElementById("tasksButton").classList.add("selectedMenu");
    }

    function LoadTasks() {
        var tasksDiv = document.getElementById("tasks");
        while (tasksDiv.children.length > 1)
            tasksDiv.removeChild(tasksDiv.lastChild);
        for (var i = 0; i < config.Tasks.length; i++) {
            if (config.Tasks[i].Archivized == true)
                continue;
            var newButton = document.createElement("button");
            newButton.classList.add("taskButton");
            newButton.taskId = config.Tasks[i].Id;
            newButton.innerText = config.Tasks[i].Name;
            tasksDiv.appendChild(newButton);
        }
        var taskButtons = $("#tasks").find(".taskButton")
        // var taskButtons = document.getElementsByClassName("taskButton");
        for (let i = 0; i < taskButtons.length; i++) {
            taskButtons[i].onclick = function () {
                shownAllGroups = false;
                if (taskButtons[i].taskId == -1) {
                    NewTaskClick();
                }
                else {
                    var task = config.Tasks.filter(function (el) {
                        return el.Id == taskButtons[i].taskId
                    });
                    ShowTaskDetails(task[0]);
                }
                var selected = document.getElementsByClassName("activeTask");
                if (selected.length > 0)
                    selected[0].classList.remove("activeTask");
                taskButtons[i].classList.add("activeTask");
            };
        }
    }

    function ShowTaskDetails(el) {
        var name = $("#taskDetails").find("#taskName");
        name[0].value = el.Name;
        var description = $("#taskDetails").find("#taskDescription");
        description[0].value = el.Description;
        var startDate = $("#taskDetails").find("#taskStartDate");
        startDate[0].value = el.StartDate;
        var frequency = $("#taskDetails").find("#taskFrequency");
        frequency[0].value = el.Frequency;

        var groups = el.Groups;
        var groupsDiv = document.getElementById("taskGroups");
        while (groupsDiv.firstChild)
            groupsDiv.removeChild(groupsDiv.firstChild);
        for (var i = 0; i < groups.length; i++) {
            var group = document.createElement("div");
            group.classList.add("groupInTask");
            group.groupId = groups[i].Id;
            group.classList.add("taskGroupItem");
            var label = document.createElement("label");
            var labelNode = document.createTextNode(groups[i].Name);
            label.appendChild(labelNode);
            var p = document.createElement("p");
            var pNode = document.createTextNode(groups[i].Description == null ? "" : groups[i].Description);
            p.appendChild(pNode);
            group.appendChild(label);
            group.appendChild(p);
            groupsDiv.appendChild(group);
        }
    }

    function applyCLick() {
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
            task.Id = newTaskId;
            newTaskId--;
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

    function resetCLick() {
        var selectedTaskId = document.getElementsByClassName("activeTask")[0].taskId;
        var task = config.Tasks.filter(function (el) {
            return el.Id == selectedTaskId
        });
        ShowTaskDetails(task[0]);
    }

    function deleteCLick() {
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

    function NewTaskClick() {
        SetNewTaskValues();
        var groupsDiv = document.getElementById("taskGroups");
        while (groupsDiv.firstChild)
            groupsDiv.removeChild(groupsDiv.firstChild);

        var newTask = document.getElementById("newTask");
        var selected = document.getElementsByClassName("activeTask");
        if (selected.length > 0)
            selected[0].classList.remove("activeTask");
        newTask.classList.add("activeTask");
    }

    function ShowGroups(groups) {
        var groupsDivs = $("#taskGroups").find(".taskGroupItem");
        for (let i = 0; i < groupsDivs.length; i++) {
            groupsDivs[i].onclick = function () {
                if (groupsDivs[i].classList.contains("groupInTask"))
                    groupsDivs[i].classList.remove("groupInTask");
                else
                    groupsDivs[i].classList.add("groupInTask");
            };
            groups = groups.filter(function (el) {
                return el.Id != groupsDivs[i].groupId;
            });
        }

        var groupsDiv = document.getElementById("taskGroups");
        for (var i = 0; i < groups.length; i++) {
            let group = document.createElement("div");
            group.groupId = groups[i].Id;
            group.classList.add("taskGroupItem");
            var label = document.createElement("label");
            var labelNode = document.createTextNode(groups[i].Name);
            label.appendChild(labelNode);
            var p = document.createElement("p");
            var pNode = document.createTextNode(groups[i].Description == null ? "" : groups[i].Description);
            p.appendChild(pNode);
            group.appendChild(label);
            group.appendChild(p);
            group.onclick = function () {
                if (group.classList.contains("groupInTask"))
                    group.classList.remove("groupInTask");
                else
                    group.classList.add("groupInTask");
            };
            groupsDiv.appendChild(group);
        }
    }
    function SetNewTaskValues() {
        $("#taskDetails").find("#taskName")[0].value = "New Task";
        $("#taskDetails").find("#taskDescription")[0].value = "";
        $("#taskDetails").find("#taskStartDate")[0].value = new Date().toISOString().substr(0, 16);
        $("#taskDetails").find("#taskFrequency")[0].value = 1;
    }

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
})();