function openChoosingCaptainWindow() {
    chosingWindow = window.open("/Mvc/Players/ChoosePlayers",
                                "ChoosingCaptainWindow",
                                "width=400, height=400, status=0, location=0, resizable=1");
}

function openChoosingRosterWindow() {
    chosingWindow = window.open("/Mvc/Players/ChoosePlayers",
                                "ChoosingRosterWindow",
                                "width=400, height=400, status=0, location=0, resizable=1");
}

function choosePlayer() {
    alert("choosePlayer");
}

function setCaptain() {
    $("body").prepend("setCaptain");
}

function setRoster() {
    alert("setRoster");
}

function finishChoosing() {
    alert("finishChoosing");
    window.opener.setCaptain();
    window.close();
    //if (window.name == "ChoosingCaptainWindow") {
    //    window.opener.setCaptain(chosen.fullName, chosen.id);
    //}
    //else {
    //    window.opener.setRoster(chosen.fullName, chosen.id);
    //}
}