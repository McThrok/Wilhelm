(() => {
    //menu
    var selectedMenu = document.getElementsByClassName("selectedMenu");
    if (selectedMenu.length > 0)
        selectedMenu[0].classList.remove("selectedMenu");
    document.getElementById("reportsButton").classList.add("selectedMenu");
})();