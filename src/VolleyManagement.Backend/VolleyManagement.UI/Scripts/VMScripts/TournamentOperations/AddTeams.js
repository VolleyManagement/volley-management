$(document).ready(function () {
    'use strict';

    var currNs = VM.addNamespace("tournament.addTeams"),
        privates = {};
    var divisionCounter = 0;
    var teamCounter = 0;
    var MAX_TEAMS_NUMBER = 0;
    var groupVisibility = "hidden";
    var divisionVisibility = "hidden";

    privates.tornamentTeamsTable = $("#tournamentRoster");

    privates.getAllTeamsOptions = function (callback) {
        var id = $("[id='TournamentId']").val();
        $.getJSON("/Tournaments/GetAllAvailableTeams", { tournamentId: id }, callback);
    };

    privates.getAllDivisionsOptions = function (callback) {
        var id = $("[id='TournamentId']").val();
        $.getJSON("/Tournaments/GetAllAvailableDivisions", { tournamentId: id }, callback);
    };

    privates.getAllGroupsOptions = function (callback) {
        var actualDivisionCounter = $(document.getElementsByName('divisions')).attr('counter');
        var divId = $("select[name='divisions'][counter='" + actualDivisionCounter + "'] :selected").val();
        $.getJSON("/Tournaments/GetAllAvailableGroups", { divisionId: divId }, callback);
    };

    privates.handleTeamsAddSuccess = function (data, status, xhr) {
        if (data.Message) {
            alert(data.Message);
            return false;
        }
        location.reload();
    };

    privates.getJsonForTournamentTeamsSave = function () {
        var result = {
            GroupTeamList: []
        };

        var selectedTeams = $("select[name='teams'] :selected");
        var selectedGroups = $("select[name='groups'] :selected");

        if (selectedTeams.length !== selectedGroups.length) {
            return alert("Cannot save. Data invalid. Try removing empty row.");
        }

        for (var i = 0; i < selectedTeams.length; i++) {
            if (selectedTeams[i].value !== "0" && selectedGroups[i].value !== "0") {
                result.GroupTeamList.push({
                    GroupId: selectedGroups[i].value,
                    TeamId: selectedTeams[i].value
                });
            }
        }

        if (result.GroupTeamList.length === 0) {
            return null;
        }

        return result;
    };

    privates.getTornamentTeamsRowMarkup = function (responseOptions) {
        var result = "<tr><td><select name='teams'>" + responseOptions + "</select></td><tr>";
        return result;
    };

    privates.renderNewTournamentTeamsRow = function (responseOptions) {
        $("tr:last", privates.tornamentTeamsTable).after(privates.getTornamentTeamsRowMarkup(responseOptions));
    };

    privates.getTornamentDivisionRowMarkup = function (responseOptions) {
        var result = "<td><select name = 'divisions' counter = '" + divisionCounter + "'" + divisionVisibility + ">" + responseOptions + "</select></td>"
            + "<td class = 'markup' counter = '" + divisionCounter + "'></td>"
            + "<td></td><td><button class='deleteTeamButton' counter = '" + divisionCounter + "'>Delete</button></td>";
        return result;
    };

    privates.renderNewTournamentDivisionsRow = function (responseOptions) {
        var teamTableData = $("select[name='teams']:last").parent().parent();
        teamTableData.append(privates.getTornamentDivisionRowMarkup(responseOptions));
    };

    privates.getTornamentGroupRowMarkup = function (responseOptions, actualDivisionCounter) {
        var result = "<td><select name='groups' counter = '" + actualDivisionCounter + "'" + groupVisibility + ">" + responseOptions + "</select></td>"
            + "<td><button class='deleteTeamButton' counter = '" + actualDivisionCounter + "'>Delete</button></td>";

        return result;
    };

    privates.renderNewTournamentGroupsRow = function (responseOptions) {
        var actualDivisionCounter = $(document.getElementsByName('divisions')).attr('counter');
        $("select[name ='divisions'][counter='" + actualDivisionCounter + "']:last", privates.tornamentTeamsTable).parent()
            .parent().append(privates.getTornamentGroupRowMarkup(responseOptions, actualDivisionCounter));
    };

    privates.addTournamentTeamsRow = function (callbackDivisions) {

        var selectedTeams = $("select :selected");

        for (var i = 0; i < selectedTeams.length; i++) {
            if (selectedTeams[i].value === 0) {
                return false;
            }
        }

        privates.getAllTeamsOptions(function (options) {
            MAX_TEAMS_NUMBER = options.length;
            var responseOptions = "<option value = '0'>" + currNs.teamIsNotSelectedMessage + "</option>";

            $.each(options, function (key, value) {
                responseOptions += "<option value='" + value.Id + "'>" + value.Name + "</option>";
            });

            privates.renderNewTournamentTeamsRow(responseOptions);

            callbackDivisions();
        });
    };

    privates.addTournamentDivisionsRow = function () {

        divisionCounter++;
        var selectedDivisions = $("select :selected");
        for (var i = 0; i < selectedDivisions.length; i++) {
            if (selectedDivisions[i].value === 0) {
                return false;
            }
        }

        privates.getAllDivisionsOptions(function (options) {
            var responseOptions = "";

            if (options.length > 1) {
                responseOptions = "<option value = '0'>" + currNs.divisionIsNotSelectedMessage + "</option>";
                divisionVisibility = "visible";
            }

            $.each(options, function (key, value) {
                responseOptions += "<option value='" + value.Id + "'>" + value.Name + "</option>";
            });
            privates.renderNewTournamentDivisionsRow(responseOptions);

            if (options.length === 1) {
                privates.CheckIfEmptyGroupRowDraw();
            }

            $(".deleteTeamButton").bind("click", currNs.onDeleteTeamButtonClick);
            $('select[name="divisions"][counter="' + divisionCounter + '"]').bind('change', privates.CheckIfEmptyGroupRowDraw);
        });
    };

    privates.CheckIfEmptyGroupRowDraw = function () {
        var actualDivisionCounter = $(document.getElementsByName('divisions')).attr('counter');
        var divId = $("select[name='divisions'][counter='" + actualDivisionCounter + "'] :selected").val();

        if (divId === "0") {
            privates.RemoveGroupsAndDeleteRowButtonMarkup();
            $("select[name = 'divisions'][counter='" + actualDivisionCounter + "']", privates.tornamentTeamsTable).parent().parent()
                .append("<td class = 'markup' counter = '" + actualDivisionCounter
                + "'></td><td><button class='deleteTeamButton' counter = '" + actualDivisionCounter + "'>Delete</button></td>");
            $(".deleteTeamButton").bind("click", currNs.onDeleteTeamButtonClick);
        } else {
            privates.addTournamentGroupsRow();
        }
    };

    privates.addTournamentGroupsRow = function () {

        var selectedGroups = $("select :selected");

        for (var i = 0; i < selectedGroups.length; i++) {
            if (selectedGroups[i].value === 0) {
                return false;
            }
        }

        privates.getAllGroupsOptions(function (options) {
            var responseOptions = "";
            groupVisibility = "hidden";

            if (options.length > 1) {
                groupVisibility = "visible";
                divisionVisibility = "visible";
                responseOptions = "<option value = '0'>" + currNs.groupIsNotSelectedMessage + "</option>";
            }
            $.each(options, function (key, value) {
                responseOptions += "<option value='" + value.Id + "'>" + value.Name + "</option>";
            });
            privates.RemoveGroupsAndDeleteRowButtonMarkup();
            privates.renderNewTournamentGroupsRow(responseOptions);
            $(".deleteTeamButton").bind("click", currNs.onDeleteTeamButtonClick);
        });

    };

    privates.RemoveGroupsAndDeleteRowButtonMarkup = function () {
        var actualDivisionCounter = $(document.getElementsByName('divisions')).attr('counter');
        var group = actualDivisionCounter;
        var deleteButton = actualDivisionCounter;

        $('select[name = "groups"][counter = "' + group + '"]').parent().remove();
        $('[class = "markup"][counter = "' + group + '"]').remove();
        $('[class = "deleteTeamButton"][counter = "' + deleteButton + '"]').parent().remove();
    };

    privates.HasDisabledAddTeamToTournamentButton = function () {
        if (teamCounter === MAX_TEAMS_NUMBER) {
            $("#addTeamToTournamentButton").prop('disabled', true);
        } else {
            $("#addTeamToTournamentButton").prop('disabled', false);
        }
    };

    currNs.onAddTeamToTournamentButtonClick = function () {
        privates.addTournamentTeamsRow(privates.addTournamentDivisionsRow);
    };

    currNs.onAddTeamsButtonButtonClick = function () {
        var teamData = privates.getJsonForTournamentTeamsSave();

        if (teamData !== null) {
            $.post("/Tournaments/AddTeamsToTournament", teamData)
                .done(privates.handleTeamsAddSuccess);
        }
    };

    currNs.onDeleteTeamButtonClick = function (eventData) {
        var currentRow = eventData.target.parentElement.parentElement.remove();
        teamCounter--;
        $('tr:empty').remove();
        privates.HasDisabledAddTeamToTournamentButton();
    };

    currNs.onDeleteTeamFromTournamentButtonClick = function (eventData) {
        var teamId = eventData.target.id;
        var tournamentId = $("[id='TournamentId']").val();
        var confirmDeleteTeamResult = confirm(currNs.deleteConfirmMessage);

        if (confirmDeleteTeamResult) {
            $.post("/Tournaments/DeleteTeamFromTournament",
                {
                    teamId: teamId,
                    tournamentId: tournamentId
                })
                .done(function (data) {
                    alert(data.Message);
                    if (data.HasDeleted) {
                        $("#" + teamId).parent().parent().remove();
                    }
                });
        }
    };

    $("#addTeamToTournamentButton").bind("click", function () {
        currNs.onAddTeamToTournamentButtonClick();
        teamCounter++;
        privates.HasDisabledAddTeamToTournamentButton();
    });
    $("#addTeamsButton").bind("click", currNs.onAddTeamsButtonButtonClick);
    $(".deleteTeamFromTournamentButton").bind("click", currNs.onDeleteTeamFromTournamentButtonClick);
});
