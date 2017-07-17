function addDivision(
            maxDivisionsCount, maxGroupsCount, minDivisionsCount, minGroupsCount,
            divisionDefaultName, removeDivisionDefaultName,
            groupsTitle, groupDefaultName,
            addGroupDefaultName, removeGroupDefaultName) {
    var divisionsCount = $("#Divisions").children().size() - 1;
    var newDivisionWrapper = $("<div></div>").attr("id", "Division_" + divisionsCount + "_Id");
    var newDivisionIdHidden = $("<input></input>").attr("data-val", "true")
        .attr("data-val-number", "The field Id must be a number.")
        .attr("data-val-required", "The Id field is required.")
        .attr("id", "Divisions_" + divisionsCount + "__Id")
        .attr("name", "Divisions[" + divisionsCount + "].Id")
        .attr("type", "hidden")
        .attr("value", "0");
    var newDivisionNameWrapper = $("<div></div>").attr("class", "editor-label");
    var newDivisionNameEditor = $("<input></input>").attr("class", "text-box single-line")
        .attr("data-val", "true")
        .attr("data-val-maxlength", "Division Name can not contain more than 60 symbols")
        .attr("data-val-maxlength-max", "60")
        .attr("data-val-required", "Division Name can not be empty")
        .attr("id", "Divisions_" + divisionsCount + "__Name")
        .attr("name", "Divisions[" + divisionsCount + "].Name")
        .attr("type", "text")
        .attr("value", divisionDefaultName + " " + (divisionsCount + 1));
    var newDivisionNameValidation = $("<span></span>")
        .attr("class", "field-validation-valid")
        .attr("data-valmsg-for", "Divisions[" + divisionsCount + "].Name")
        .attr("data-valmsg-replace", "true");
    var newDivisionRemoveLink = $("<a> " + removeDivisionDefaultName + " </a>").attr("id", "Remove_Division_" + divisionsCount + "_Id")
        .attr("class", "link-button")
        .attr("onclick", "removeDivision(" + divisionsCount + ", " + minDivisionsCount + ", " + maxGroupsCount + ", " + minGroupsCount + ", '" + groupDefaultName + "', '" + removeGroupDefaultName + "')");
    var newDivisionGroupsWrapper = $("<div></div>").attr("class", "division-groups-wrapper");
    var newDivisionGroupsTitleWrapper = $("<div></div>").attr("class", "editor-label");
    var newDivisionGroupsTitle = $("<label>" + (groupsTitle) + "</label>")
        .attr("for", "Divisions_" + divisionsCount + "__Groups");
    var newDivisionGroupsListWrapper = $("<div></div>").attr("id", "Division_" + divisionsCount + "_Groups");
    var newGroupWrapper = createGroup(divisionsCount, 0, minGroupsCount, groupDefaultName, removeGroupDefaultName);
    var newDivisionGroupsAddWrapper = $("<div></div>").attr("class", "add-group-button");
    var newDivisionGroupsAdd = $("<a> " + addGroupDefaultName + " </a>").attr("class", "link-button")
        .attr("id", "Add_Division_" + divisionsCount + "_Group")
        .attr("onclick", "addGroup(" + divisionsCount + ", " + maxGroupsCount + ", " + minGroupsCount + ", '" + groupDefaultName + "', '" + removeGroupDefaultName + "')")
        .show();

    $(newDivisionGroupsListWrapper).append(newGroupWrapper);
    $(newDivisionGroupsTitleWrapper).append(newDivisionGroupsTitle);
    $(newDivisionGroupsAddWrapper).append(newDivisionGroupsAdd);
    $(newDivisionGroupsWrapper).append(newDivisionGroupsTitleWrapper);
    $(newDivisionGroupsWrapper).append(newDivisionGroupsListWrapper);
    $(newDivisionGroupsWrapper).append(newDivisionGroupsAddWrapper);
    $(newDivisionNameWrapper).append(newDivisionNameEditor);
    $(newDivisionNameWrapper).append(newDivisionNameValidation);
    $(newDivisionNameWrapper).append(newDivisionRemoveLink);
    $(newDivisionWrapper).append(newDivisionIdHidden);
    $(newDivisionWrapper).append(newDivisionNameWrapper);
    $(newDivisionWrapper).append(newDivisionGroupsWrapper);
    $("#Divisions").append(newDivisionWrapper);

    divisionsCount++;

    var divisionId = "#" + $(newDivisionGroupsListWrapper).attr("id");
    var groupsCount = $(divisionId).children().size();
    hideRemoveLink(divisionId, groupsCount, minGroupsCount);

    hideRemoveLink("#Divisions", divisionsCount + 1, minDivisionsCount + 1);

    if (divisionsCount - minDivisionsCount == 1) {
        var removeLinkId = "#Remove_" + $("#Divisions").children()[1].id;

        $(removeLinkId).show();
    }

    if (divisionsCount == maxDivisionsCount) {
        $("#Add_Division").hide();
    }
}

function createGroup(divisionIdx, groupIdx, minGroupsCount, groupDefaultName, removeGroupDefaultName) {
    var newGroupWrapper = $("<div></div>").attr("id", "Division_" + divisionIdx + "_Group_" + groupIdx)
        .attr("class", "editor-field group-editor");
    var newGroupHidden = $("<input></input>").attr("data-val", "true")
        .attr("data-val-number", "The field Id must be a number.")
        .attr("data-val-required", "The Id field is required.")
        .attr("id", "Divisions_" + divisionIdx + "__Groups_" + groupIdx + "__Id")
        .attr("name", "Divisions[" + divisionIdx + "].Groups[" + groupIdx + "].Id")
        .attr("type", "hidden")
        .attr("value", "0");
    var newGroupNameEditor = $("<input></input>").attr("class", "text-box single-line")
        .attr("data-val", "true")
        .attr("data-val-maxlength", "Group name can not contain more than 60 symbols")
        .attr("data-val-maxlength-max", "60")
        .attr("data-val-required", "Group name can not be empty")
        .attr("id", "Divisions_" + divisionIdx + "__Groups_" + groupIdx + "__Name")
        .attr("name", "Divisions[" + divisionIdx + "].Groups[" + groupIdx + "].Name")
        .attr("type", "text")
        .attr("value", groupDefaultName + " " + (groupIdx + 1));
    var newGroupValidation = $("<span></span>").attr("class", "field-validation-valid")
        .attr("dara-valmsg-for", "Divisions[" + divisionIdx + "].Groups[" + groupIdx + "].Name")
        .attr("data-valmsg-replace", "true");
    var newRemoveGroupLink = $("<a> " + removeGroupDefaultName + "</a>").attr("id", "Remove_Division_" + divisionIdx + "_Group_" + groupIdx)
        .attr("class", "link-button")
        .attr("onclick", "removeGroup(" + divisionIdx + ", " + groupIdx + ", " + minGroupsCount + ")");

    $(newGroupWrapper).append(newGroupHidden);
    $(newGroupWrapper).append(newGroupNameEditor);
    $(newGroupWrapper).append(newGroupValidation);
    $(newGroupWrapper).append(newRemoveGroupLink);

    return newGroupWrapper;
}

function addGroup(divisionIdx, maxGroupsCount, minGroupsCount, groupDefaultName, removeGroupDefaultName) {
    var divisionGroupsWrapper = "#Division_" + divisionIdx + "_Groups";
    var groupsCount = $(divisionGroupsWrapper).children().size();
    var newGroupWrapper = createGroup(divisionIdx, groupsCount, minGroupsCount, groupDefaultName, removeGroupDefaultName);

    $(divisionGroupsWrapper).append(newGroupWrapper);

    if (groupsCount == minGroupsCount) {
        var removeLinkId = "#Remove_" + $(divisionGroupsWrapper).children()[0].id;

        $(removeLinkId).show();
    }

    if (groupsCount + 1 == maxGroupsCount) {
        $("#Add_Division_" + divisionIdx + "_Group").hide();
    }
}

function removeDivision(divisionIdx, minDivisionsCount, maxGroupsCount, minGroupsCount, groupDefaultName, removeGroupDefaultName) {
    var divisionsId = "#Divisions";
    var divisionWrapperId = "#Division_" + divisionIdx + "_Id";
    var divisionsCount = $(divisionsId).children().size() - 1;

    $(divisionWrapperId).remove();

    divisionsCount--;

    updateDivisions($(divisionsId), divisionIdx, divisionsCount, minDivisionsCount, maxGroupsCount, minGroupsCount, groupDefaultName, removeGroupDefaultName);

    hideRemoveLink(divisionsId, divisionsCount + 1, minDivisionsCount + 1);
}

function removeGroup(divisionIdx, groupIdx, minGroupsCount) {
    var divisionGroupsWrapperId = "#Division_" + divisionIdx + "_Groups";
    var groupWrapperId = "#Division_" + divisionIdx + "_Group_" + groupIdx;
    var groupsCount = $(divisionGroupsWrapperId).children().size();

    $(groupWrapperId).remove();

    groupsCount--;

    updateGroups(divisionGroupsWrapperId, divisionIdx, groupIdx, groupsCount, minGroupsCount);

    hideRemoveLink(divisionGroupsWrapperId, groupsCount, minGroupsCount);
}

function hideRemoveLink(elementsPlacement, elementsAmount, minElementsNumber) {
    if (elementsAmount <= minElementsNumber) {
        for (var i = 0; i < elementsAmount; i++){
            var element = $(elementsPlacement).children()[i];
            if (element != undefined) {
                var linkToHide = "#Remove_" + element.id;

                $(linkToHide).hide();
            }
        }
    }
}

function updateDivisions(divisionsId, removedDivisionIdx, divisionsCount, minDivisionsCount, maxGroupsCount, minGroupsCount, groupDefaultName, removeGroupDefaultName) {
    for (var i = removedDivisionIdx; i < divisionsCount; i++) {
        var divisionToUpdate = $(divisionsId).children()[i + 1];

        var divisionHidden = $(divisionToUpdate).children()[0];

        var divisionNameWrapper = $(divisionToUpdate).children()[1];
        var divisionGroupsWrapper = $(divisionToUpdate).children()[2];

        var divisionNameEditor = $(divisionNameWrapper).children()[0];
        var divisionNameValidation = $(divisionNameWrapper).children()[1];
        var divisionRemoveLink = $(divisionNameWrapper).children()[2];

        var divisionGroupsTitleWrapper = $(divisionGroupsWrapper).children()[0];
        var divisionGroupsListWrapper = $(divisionGroupsWrapper).children()[1];
        var divisionGroupsAddWrapper = $(divisionGroupsWrapper).children()[2];

        var divisionGroupsTitle = $(divisionGroupsTitleWrapper).children()[0];

        var divisionGroupsAdd = $(divisionGroupsAddWrapper).children()[0];

        $(divisionToUpdate).attr("id", "Division_" + i + "_Id");

        $(divisionHidden).attr("id", "Divisions_" + i + "__Id");
        $(divisionHidden).attr("name", "Divisions[" + i + "].Id");

        $(divisionNameEditor).attr("id", "Divisions_" + i + "__Name");
        $(divisionNameEditor).attr("name", "Divisions[" + i + "].Name");

        $(divisionNameValidation).attr("data-valmsg-for", "Divisions[" + i + "].Name");

        $(divisionRemoveLink).attr("id", "Remove_Division_" + i + "_Id");
        $(divisionRemoveLink).attr("onclick", "removeDivision(" + i + ", " + minDivisionsCount + ", " + maxGroupsCount + ", " + minGroupsCount + ", '" + groupDefaultName + "', '" + removeGroupDefaultName + "')");

        $(divisionGroupsListWrapper).attr("id", "Division_" + i + "_Groups");

        updateGroups("#" + divisionGroupsListWrapper.id, i, 0, $(divisionGroupsListWrapper).children().size(), minGroupsCount);

        $(divisionGroupsTitle).attr("for", "Divisions_" + i + "__Groups");

        $(divisionGroupsAdd).attr("id", "Add_Division_" + i + "_Group");
        $(divisionGroupsAdd).attr("onclick", "addGroup(" + i + ", " + maxGroupsCount + ", " + minGroupsCount + ", '" + groupDefaultName + "', '" + removeGroupDefaultName + "')");
    }
}

function updateGroups(divisionGroupsWrapperId, divisionIdx, startingGroupIdx, groupsCount, minGroupsCount) {
    for (var i = startingGroupIdx; i < groupsCount; i++) {
        var groupToUpdate = $(divisionGroupsWrapperId).children()[i];
        var groupHidden = $(groupToUpdate).children()[0];
        var groupNameEditor = $(groupToUpdate).children()[1];
        var groupValidation = $(groupToUpdate).children()[2];
        var groupRemoveLink = $(groupToUpdate).children()[3];

        $(groupToUpdate).attr("id", "Division_" + divisionIdx + "_Group_" + i);

        $(groupHidden).attr("id", "Divisions_" + divisionIdx + "__Groups_" + i + "__Id");
        $(groupHidden).attr("name", "Divisions[" + divisionIdx + "].Groups[" + i + "].Id");

        $(groupNameEditor).attr("id", "Divisions_" + divisionIdx + "__Groups_" + i + "__Name");
        $(groupNameEditor).attr("name", "Divisions[" + divisionIdx + "].Groups[" + i + "].Name");

        $(groupValidation).attr("dara-valmsg-for", "Divisions[" + divisionIdx + "].Groups[" + i + "].Name");

        $(groupRemoveLink).attr("id", "Remove_Division_" + divisionIdx + "_Group_" + i);
        $(groupRemoveLink).attr("onclick", "removeGroup(" + divisionIdx + ", " + i + ", " + minGroupsCount + ")");
    }
}