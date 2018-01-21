(() => {

    var config = "";
    var userId = "";

    window.onload = function () {
        var applyBtn = document.getElementById("apply_task");
        var resetBtn = document.getElementById("reset_task");
        var deleteBtn = document.getElementById("delete_task");

        var dataDiv = document.getElementById("dataDiv");
        config = JSON.parse(dataDiv.dataset.config);
        userId = dataDiv.dataset.userId;

        applyBtn.onclick = () => applyCLick();
        resetBtn.onclick = () => resetCLick();
        deleteBtn.onclick = () => deleteCLick();

        var taskButtons = $("#tasks").find(".taskButton")
        // var taskButtons = document.getElementsByClassName("taskButton");
        for (let i = 0; i < taskButtons.length; i++) {
            taskButtons[i].onclick = function () {
                var task = config.Tasks.filter(function (el) {
                    return el.Id == taskButtons[i].dataset.taskid
                });
                ShowTaskDetails(task[0]);
                var selected = document.getElementsByClassName("activeTask");
                if (selected.length > 0)
                    selected[0].classList.remove("activeTask");
                taskButtons[i].classList.add("activeTask");
            };
        }

        var assignToGroup = document.getElementById("assignToGroup");
        assignToGroup.onclick = function () {
            var groupsDiv = document.getElementById("taskGroups");

            ShowGroups(config.Groups);
            var selectedTaskId = document.getElementsByClassName("activeTask")[0].taskid;
            var groupsDivs = $("#groupsDiv").find(".taskGroupItem");
            for (var j = 0; j < groupsDiv.length; j++) {
                 groupsDiv[i].groupId
            }
        }

        var selectedMenu = document.getElementsByClassName("selectedMenu");
        if (selectedMenu.length > 0)
            selectedMenu[0].classList.remove("selectedMenu");
        document.getElementById("tasksButton").classList.add("selectedMenu");
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

        ShowGroups(el.Groups);
    }

    function applyCLick() {
    }

    function resetCLick() {
    }

    function deleteCLick() {
    }

    function ShowGroups(groups) {
        var groupsDiv = document.getElementById("taskGroups");
        while (groupsDiv.firstChild)
            groupsDiv.removeChild(groupsDiv.firstChild);
        for (var i = 0; i < groups.length; i++) {
            var group = document.createElement("div");
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
    //function ConvertDatesToIso(config) {
    //    for (var i = 0; i < config.Tasks.length; i++) {
    //        config.Tasks[i].StartData = config.Tasks[i].StartData.substring(0, 10).split("-").join("/");
    //    }
    //}
    //function ConvertDatesFromIso{
    //    for (var i = 0; i < config.Tasks.length; i++) {
    //        config.Tasks[i].StartData = config.Tasks[i].StartData.split("/").join("-") + "";
    //    }
    //}
    function sendNewConfig(id) {

        $.ajax({
            url: "http://localhost:8080/api/ConfigurationActivities?userId=" + id,
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify(config),
        })
    }

})();