$(function () {
    var tasks = CreateTasks(); // TODO willl be load tasks from server
    LoadTasks(tasks);

    function LoadTasks(tasks) {
        var tasksDiv = document.getElementById("tasks");
        tasks.forEach(function (el) {
            tasksDiv.appendChild(ObjectToButton(el));
        });
    };

    function ObjectToButton(el) {
        var button = document.createElement("button");
        button.taskId = el.id;
        var node = document.createTextNode(el.name);
        button.appendChild(node);
        button.onclick = function () {
            var selected = document.getElementsByClassName("active_task");
            if (selected.length > 0)
                selected[0].classList.remove("active_task");
            button.classList.add("active_task");
            ShowStats(el);
        };
        return button;
    };



    function CreateTasks() {
        var tasks = [];
        for (var i = 0; i < 5; i++) {
            var groups = [];
            for (var j = 0; j < i + 1; j++) {
                var group = {
                    name: "Group" + j,
                    description: "Group " + j + " description"
                }
                groups.push(group);
            }
            var task = {
                id: i,
                name: "Task " + i,
                description: "Task " + i + " description",
                startDate: "2018-01-18",
                frequency: i,
                groups: groups
            };
            tasks.push(task);
        }
        return tasks;
    };
});