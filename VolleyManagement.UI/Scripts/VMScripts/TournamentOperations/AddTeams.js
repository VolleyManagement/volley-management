$(document).ready(function () {
    'use strict';

    var currNs = VM.addNamespace("tournament.addTeams"),
        privates = {};

    privates.tornamentTeamsTable = $("#tornamentRoster");

    privates.getAllTeamsOptions = function (callback) {
        var id = $("[id='TournamentId']").val();
        $.getJSON("/Tournaments/GetAllAvailableTeams", { tournamentId: id }, callback);
    };

    privates.getAllDivisionsOptions = function (callback) {
        var id = $("[id='TournamentId']").val();
        $.getJSON("/Tournaments/GetAllAvailableDivisions", { tournamentId: id }, callback);
    };

    privates.getAllGroupsOptions = function (callback) {
        var divId = $("select[name='divisions'] :selected").val();
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
            TournamentId: $("[id='TournamentId']").val(),
            List: []
        };
        var selectedTeams = $("select[name='teams'] :selected");

        for (var i = 0; i < selectedTeams.length; i++) {
            if (selectedTeams[i].value !== 0) {
                result.List.push({
                    Id: selectedTeams[i].value
                });
            }
        }

        return result;
    };

    privates.getJsonForTournamentGroupsSave = function () {
        var result = {
            DivisionId: $("select[name='divisions'] :selected").val(),
            List: []
        };

        var selectedGroups = $("select[name='groups'] :selected");

        for (var i = 0; i < selectedGroups.length; i++) {
            if (selectedGroups[i].value !== 0) {
                result.List.push({
                    Id: selectedGroups[i].value
                });
            }
        }

        return result;
    };

    privates.getTornamentTeamsRowMarkup = function (responseOptions) {
        var result = "<tr><td id = 'team'><select name='teams'>" + responseOptions + "</select></td><tr>";
        return result;
    };

    privates.renderNewTournamentTeamsRow = function (responseOptions) {
        $("tr:last", privates.tornamentTeamsTable).after(privates.getTornamentTeamsRowMarkup(responseOptions));
    };

    privates.getTornamentDivisionRowMarkup = function (responseOptions, isDisabled) {
        var disabled = "";

        if (isDisabled) {
            disabled = "disabled";
        }

        var result = "<td id ='division'><select " + disabled + " name = 'divisions'>" + responseOptions + "</select></td>"
            + "<td id = 'group'></td>"
            + "<td id = 'delete-team-button'><button class='deleteTeamButton'>Delete</button></td>";
        return result;
    };

    privates.renderNewTournamentDivisionsRow = function (responseOptions, isDisabled) {
        var teamTableData = $("#team", privates.tornamentTeamsTable).parent();
        teamTableData.append(privates.getTornamentDivisionRowMarkup(responseOptions, isDisabled));
    };

    privates.getTornamentGroupRowMarkup = function (responseOptions, isDisabled) {
        var disabled = "";

        if (isDisabled) {
            disabled = "disabled";
        }

        var result = "<td id='group'><select " + disabled + " name='groups'>" + responseOptions + "</select></td>"
                    + "<td id = 'delete-team-button'><button class='deleteTeamButton'>Delete</button></td>";
        return result;
    };

    privates.renderNewTournamentGroupsRow = function (responseOptions, isDisabled) {
        $("td[id ='division']:last", privates.tornamentTeamsTable).parent().append(privates.getTornamentGroupRowMarkup(responseOptions, isDisabled));
    };

    privates.addTournamentTeamsRow = function () {

        var selectedTeams = $("select :selected");

        for (var i = 0; i < selectedTeams.length; i++) {
            if (selectedTeams[i].value === 0) {
                return false;
            }
        }

        privates.getAllTeamsOptions(function (options) {
            var responseOptions = "<option value = '0'>" + currNs.teamIsNotSelectedMessage + "</option>";

            $.each(options, function (key, value) {
                responseOptions += "<option value='" + value.Id + "'>" + value.Name + "</option>";
            });

            privates.renderNewTournamentTeamsRow(responseOptions);
        });
    };

    privates.addTournamentDivisionsRow = function () {

        var selectedDivisions = $("select :selected");
        for (var i = 0; i < selectedDivisions.length; i++) {
            if (selectedDivisions[i].value === 0) {
                return false;
            }
        }

        privates.getAllDivisionsOptions(function (options) {
            var isDisabled = false;
            var responseOptions = "";

            if (options.length === 1) {
                isDisabled = true;
            } else {
                responseOptions += "<option value = '0'>" + currNs.divisionIsNotSelectedMessage + "</option>";
            }

            $.each(options, function (key, value) {
                responseOptions += "<option value='" + value.Id + "'>" + value.Name + "</option>";
            });            
            privates.renderNewTournamentDivisionsRow(responseOptions, isDisabled);
            $(".deleteTeamButton").bind("click", currNs.onDeleteTeamButtonClick);
            if (isDisabled) {
                privates.addTournamentGroupsRow();
            } else {
                $('select[name="divisions"]').on('change', privates.CheckIfEmptyGroupRowDraw);                
            }            
        });
    };

    privates.CheckIfEmptyGroupRowDraw = function () {
        var divisionId = $('select[name="divisions"] :selected').val();

        if (divisionId === "0") {
            privates.DeleteGroupsMarkup();
            $("td[id = 'division'] :last", privates.tornamentTeamsTable).parent().parent().after("<td id = 'group'></td><td id = 'delete-team-button'><button class='deleteTeamButton'>Delete</button></td>");
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
            var isDisabled = false;
            var responseOptions = "";

            if (options.length === 1) {
                isDisabled = true;
            } else {
                responseOptions += "<option value = '0'>" + currNs.groupIsNotSelectedMessage + "</option>";
            }

            $.each(options, function (key, value) {
                responseOptions += "<option value='" + value.Id + "'>" + value.Name + "</option>";
            });

            privates.DeleteGroupsMarkup();
            privates.renderNewTournamentGroupsRow(responseOptions, isDisabled);
            $(".deleteTeamButton").bind("click", currNs.onDeleteTeamButtonClick);
        });

    };

    privates.DeleteGroupsMarkup = function () {
        $('#group').remove();
        $('#delete-team-button').remove();
    };

    currNs.onAddTeamToTournamentButtonClick = function () {
        privates.addTournamentTeamsRow();
        privates.addTournamentDivisionsRow();
    };

    currNs.onAddTeamsButtonButtonClick = function () {
        var teamData = privates.getJsonForTournamentTeamsSave();
        var groupData = privates.getJsonForTournamentGroupsSave();
        var teamId = $('select[name="teams"] :selected').val();
        var divisionId = $('select[name="divisions"] :selected').val();
        var groupId = $('select[name="groups"] :selected').val();

        if (divisionId === "0" || teamId === "0" || groupId === "0") {
            alert("Not all parameters was selected!");
        } else {
            $.post("/Tournaments/AddTeamsToTournament", { teams: teamData, groups: groupData })
                .done(privates.handleTeamsAddSuccess);
        }
    };

    currNs.onDeleteTeamButtonClick = function () {
        location.reload();
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

    $("#addTeamToTournamentButton").bind("click", currNs.onAddTeamToTournamentButtonClick);
    $("#addTeamToTournamentButton").bind("click", function () {
        $("#addTeamToTournamentButton").prop('disabled', true);
    });
    $("#addTeamsButton").bind("click", currNs.onAddTeamsButtonButtonClick);
    $("#addTeamsButton").bind("click", function () {
        $("#addTeamToTournamentButton").prop('disabled', false);
    });
    $(".deleteTeamFromTournamentButton").bind("click", currNs.onDeleteTeamFromTournamentButtonClick);
});
