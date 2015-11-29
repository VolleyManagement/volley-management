var counter = 1;

function addInput(startIndex, limit, alertMsg) {
    if (counter == 1) {
        counter += startIndex;
    }
    if (counter == limit) {
        alert(alertMsg);
    }
    else {
        divisionName = "Divisions[" + (counter) + "].Name";
        divisionId = "Divisions_" + (counter) + "__Name";
        var newElem = $('#Division0').clone().attr('id', 'Division' + (counter));
        newElem.children(':first').attr('id', divisionId).attr('name', divisionName).attr('value', '');
        newElem.children(':last').children(':last').attr('data-division', counter);
        $("#Divisions").append(newElem);

        groupName = "Divisions[" + (counter) + "].Groups[0].Name"; //Divisions[0].Groups[0].Name
        groupId = "Divisions_" + (counter) + "__Groups_0__Name"; //Divisions_0__Groups_0__Name
        groupsId = "Group" + (counter);
        var newGroups = $("#Group0").clone().attr('id', groupsId);
        newGroups.empty();
        var newGroup = $('#Group00').clone().attr('id', 'Group' + (counter) + 0);
        newGroup.children(':first').attr('id', groupId).attr('name', groupName).attr('value', '');
        newGroups.append(newGroup);
        $("#Divisions").append(newGroups);
        counter++;
    }
}

function addGroup(buttonInfo, limit, alertMsg) {

    var divisionNum = $(buttonInfo).data("division");
    groupsId = "#Group" + (divisionNum);

    var groupCounter = $(groupsId).children("div").size();
    if (groupCounter == limit) {
        alert(alertMsg);
    }

    groupName = "Divisions[" + (divisionNum) + "].Groups[" + (groupCounter) + "].Name"; //Divisions[0].Groups[0].Name
    groupId = "Divisions_" + (divisionNum) + "__Groups_" + (groupCounter) + "__Name"; //Divisions_0__Groups_0__Name
    var newGroup = $('#Group00').clone().attr('id', 'Group' + (divisionNum) + groupCounter);
    newGroup.children(':first').attr('id', groupId).attr('name', groupName).attr('value', '');
    $(groupsId).append(newGroup);
}