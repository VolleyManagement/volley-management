var counter = 1;

function addInput(startIndex, limit, alertMsg) {
    if (counter == limit) {
        alert(alertMsg);
    }
    else {
        divisionInput = $("#Divisions_0__Name");
        divisionName = "Divisions[" + (counter + startIndex) + "].Name";
        divisionId = "Divisions_" + (counter + startIndex) + "__Name";
        divisionInput.attr("id", divisionId).attr("name", divisionName);
        $("#Divisions").appendChild(divisionInput);
    }
}