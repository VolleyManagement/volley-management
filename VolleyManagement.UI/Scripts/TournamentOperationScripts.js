function addDivision(maxDivisionsCount, divisionDefaultName, groupsTitle, groupDefaultName) {
    var divisionsCount = $("#Divisions").children().size();
    var newDivisionWrapper = $("<div></div>").attr("id", "Division_" + divisionsCount);
    var newDivisionIdHidden = $("#Divisions_0__Id").clone()
        .attr("id", "Divisions_" + divisionsCount + "__Id")
        .attr("name", "Divisions[" + divisionsCount + "].Id");
    var newDivisionNameWrapper = $("<div></div>").attr("class", "editor-label");
    var newDivisionNameEditor = $("#Divisions_0__Name").clone()
        .attr("id", "Divisions_" + divisionsCount + "__Name")
        .attr("name", "Divisions[" + divisionsCount + "].Name")
        .attr("value", divisionDefaultName + " " + (divisionsCount + 1));
    var newDivisionNameValidation = $("<span></span>")
        .attr("class", "field-validation-valid")
        .attr("data-valmsg-for", "Divisions[" + divisionsCount + "].Name")
        .attr("data-valmsg-replace", "true");
    var newDivisionGroupsWrapper = $("<div></div>").attr("class", "division-groups-wrapper");
    var newDivisionGroupsTitleWrapper = $("<div></div>").attr("class", "editor-label");
    var newDivisionGroupsTitle = $("<label>" + (groupsTitle) + "</label>")
        .attr("for", "Divisions_" + divisionsCount + "__Groups");
    var newDivisionGroupsListWrapper = $("<div></div>").attr("id", "Division_" + divisionsCount + "_Groups");
    var newGroupWrapper = cloneGroupWrapper(divisionsCount, 0, groupDefaultName);
    var newDivisionGroupsAddWrapper = $("<div></div>").attr("class", "add-group-button");
    var newDivisionGroupsAdd = $("#Add_Division_0_Group").clone()
        .attr("id", "Add_Division_" + divisionsCount + "_Group")
        .attr("onclick", "addGroup(" + divisionsCount + ", " + maxDivisionsCount + ", '" + groupDefaultName + "')")
        .show();

    $(newDivisionGroupsListWrapper).append(newGroupWrapper);
    $(newDivisionGroupsTitleWrapper).append(newDivisionGroupsTitle);
    $(newDivisionGroupsAddWrapper).append(newDivisionGroupsAdd);
    $(newDivisionGroupsWrapper).append(newDivisionGroupsTitleWrapper);
    $(newDivisionGroupsWrapper).append(newDivisionGroupsListWrapper);
    $(newDivisionGroupsWrapper).append(newDivisionGroupsAddWrapper);
    $(newDivisionNameWrapper).append(newDivisionNameEditor);
    $(newDivisionNameWrapper).append(newDivisionNameValidation);
    $(newDivisionWrapper).append(newDivisionIdHidden);
    $(newDivisionWrapper).append(newDivisionNameWrapper);
    $(newDivisionWrapper).append(newDivisionGroupsWrapper);
    $("#Divisions").append(newDivisionWrapper);

    if (divisionsCount + 1 == maxDivisionsCount) {
        $("#Add_Division").hide();
    }
}

function addGroup(divisionIdx, maxGroupsCount, groupDefaultName) {
    var divisionGroupsWrapper = "#Division_" + divisionIdx + "_Groups";
    var groupsCount = $(divisionGroupsWrapper).children().size();
    var newGroupWrapper = cloneGroupWrapper(divisionIdx, groupsCount, groupDefaultName);

    $(divisionGroupsWrapper).append(newGroupWrapper);

    if (groupsCount + 1 == maxGroupsCount) {
        $("#Add_Division_" + divisionIdx + "_Group").hide();
    }
}

function cloneGroupWrapper(divisionIdx, groupIdx, groupDefaultName) {
    var newGroupWrapper = $("#Division_0_Group_0").clone()
        .attr("id", "Division_" + divisionIdx + "_Group_" + groupIdx);

    $(newGroupWrapper.children()[0])
        .attr("id", "Divisions_" + divisionIdx + "__Groups_" + groupIdx + "__Id")
        .attr("name", "Divisions[" + divisionIdx + "].Groups[" + groupIdx + "].Id");
    $(newGroupWrapper.children()[1])
        .attr("id", "Divisions_" + divisionIdx + "__Groups_" + groupIdx + "__Name")
        .attr("name", "Divisions[" + divisionIdx + "].Groups[" + groupIdx + "].Name")
        .attr("value", groupDefaultName + " " + (groupIdx + 1));
    $(newGroupWrapper.children()[2])
        .attr("data-valmsg-for", "Divisions[" + divisionIdx + "].Groups[" + groupIdx + "].Name");

    return newGroupWrapper;
}
