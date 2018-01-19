function LoadHome() {
    LoadTodayTasks();
}

function LoadTodayTasks() {
    var homeDiv = document.getElementById("home");
    // TODO load from server
    for (var i = 0; i < 5; i++) {
        var newDiv = document.createElement("div");
        newDiv.setAttribute("class", "column");

        var input = document.createElement("input")
        input.type = "checkbox";
        input.name = "checkbox";
        input.id = i;
        var label = document.createElement("label");
        var labelNode = document.createTextNode("label "+i);
        label.appendChild(labelNode);
        label.htmlFor = i;
        var p = document.createElement("p");
        var pNode = document.createTextNode("Foooo " + i);
        p.appendChild(pNode);

        newDiv.appendChild(input);
        newDiv.appendChild(label);
        newDiv.appendChild(p);

        homeDiv.appendChild(newDiv);
    }
}