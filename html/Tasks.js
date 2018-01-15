//jakies tam taski

$(function () {
    var tasks = CreateTasks(); // TODO willl be load tasks from server
    LoadTasks(tasks);
});

function LoadTasks(tasks) {
    var tasksDiv = document.getElementById("tasks");
    tasks.forEach(function (el) {
        tasksDiv.appendChild(ObjectToButton(el));
    });
};

function ObjectToButton(el) {
    var button = document.createElement("button");
    var node = document.createTextNode(el.name);
    button.appendChild(node);
    button.onclick = function () {
        var selected = document.getElementsByClassName("active_task");
        selected[0].classList.remove("active_task");
        button.classList.add("active_task");
        TaskCLick(el);
    };
    return button;
}

function TaskCLick(el) {
    var name = $("#active_task_details").find("#name");
    name[0].value = el.name;
    var description = $("#active_task_details").find("#description");
    description[0].value = el.description;
    var startDate = $("#active_task_details").find("#start_date");
    startDate[0].value = el.startDate.toISOString().substr(0,10);
    var frequency = $("#active_task_details").find("#frequency");
    frequency[0].value = el.frequency;

    var groups = document.getElementById("task_groups");
    while (groups.firstChild)
        groups.removeChild(groups.firstChild);
    for (var i = 0; i < el.groups.length; i++) {
        var group = document.createElement("div");
        group.classList.add("column_group");
        var label = document.createElement("label");
        var labelNode = document.createTextNode(el.groups[i].name);
        label.appendChild(labelNode);
        var p = document.createElement("p");
        var pNode = document.createTextNode(el.groups[i].description);
        p.appendChild(pNode);
        group.appendChild(label);
        group.appendChild(p);
        groups.appendChild(group);
    }
}

function CreateTasks() {
    var tasks = [];
    for (var i = 0; i < 5; i++) {
        var groups = [];
        for (var j = 0; j < i+1; j++) {
            var group = {
                name: "Group" + j,
                description: "Group " + j + " description"
            }
            groups.push(group);
        }
        var task = {
            name: "Task " + i,
            description: "Task " + i + " description",
            startDate: new Date(),
            frequency: i,
            groups: groups
        };
        tasks.push(task);
    }
    return tasks;
}