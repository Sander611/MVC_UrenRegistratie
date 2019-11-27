// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/* When the user clicks on the button,
toggle between hiding and showing the dropdown content */
function myFunction() {
    document.getElementById("myDropdown").classList.toggle("show");
}

// Close the dropdown menu if the user clicks outside of it
window.onclick = function (event) {
    if (!event.target.matches('.navbarbutton')) {
        var dropdowns = document.getElementsByClassName("dropdown-content");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}

function onlySelectOne(clickedId) {
    if (clickedId == "checkTrainee") {
        document.getElementById("checkSeniorDeveloper").checked = false;
        document.getElementById("checkQienEmployee").checked = false;
    }
    else if (clickedId == "checkSeniorDeveloper") {
        document.getElementById("checkQienEmployee").checked = false;
        document.getElementById("checkTrainee").checked = false;
    }
    else if (clickedId == "checkQienEmployee") {
        document.getElementById("checkSeniorDeveloper").checked = false;
        document.getElementById("checkTrainee").checked = false;
    }
}

function showMessageActiveStatus(){
    if (document.getElementById("createAccountActiveCheck").checked == true) {
        document.getElementById("ExtraInfoActiveStatus").style.display = "block";
        document.getElementById("WarningSign").style.display = "block";
    }
    else {
        document.getElementById("ExtraInfoActiveStatus").style.display = "none";
        document.getElementById("WarningSign").style.display = "none";
    }
}