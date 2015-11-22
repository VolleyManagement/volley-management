var counter = 1;

function addInput(startIndex, limit, alertMsg) {
    if (counter == limit) {
        alert(alertMsg);
    }
    else {
        divisionName = "Divisions[" + (counter + startIndex) + "].Name";
        divisionId = "Divisions_" + (counter + startIndex) + "__Name";
        var newElem = $('#Division0').clone().attr('id', 'Division' + (counter + startIndex));
        newElem.children(':first').attr('id', divisionId).attr('name', divisionName).attr('value', '');
        $("#Divisions").append(newElem);
        counter++;
    }
}