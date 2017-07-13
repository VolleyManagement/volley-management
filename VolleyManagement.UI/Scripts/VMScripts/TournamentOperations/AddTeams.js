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
        var divId = $('select[name="divisions"] :selected').val();
        if (divId === undefined) {
            divId = 0;
        }
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
        var selectedTeams = $("select[name=\"teams\"] :selected");
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
            DivisionId: $('select[name="divisions"] :selected').val(),
            List: []
        };

        var selectedGroups = $("select[name=\"groups\"] :selected");
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
        var result = "<tr><td><select name=\"teams\">" + responseOptions + "</select></td>";
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
        var result = "<td><select " + disabled + " name=\"divisions\">" + responseOptions + "</select></td>";
        return privates.getDeleteRowButtonMarkup(result);
    };

    privates.renderNewTournamentDivisionsRow = function (responseOptions, isDisabled) {
        $("td:last", privates.tornamentTeamsTable).after(privates.getTornamentDivisionRowMarkup(responseOptions, isDisabled));
    };

    privates.getTornamentGroupRowMarkup = function (responseOptions, isHidden, isDisabled) {
        var hidden = "";
        var disabled = "";
        if (isHidden) {
            hidden = "hidden";
        }
        if (isDisabled) {
            disabled = "disabled";
        }
        var result = "<td name=\"groups\"><select " + hidden + " " + disabled + " name=\"groups\">" + responseOptions + "</select></td>";
        return result;
    };

    privates.renderNewTournamentGroupsRow = function (responseOptions, isHidden, isDisabled) {
        $("td:last", privates.tornamentTeamsTable).prev().after(privates.getTornamentGroupRowMarkup(responseOptions, isHidden, isDisabled));
    };

    privates.getDeleteRowButtonMarkup = function (markup) {
        markup += "<td><button class='deleteTeamButton'>Delete</button></td></tr>";
        return markup;
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
                responseOptions += "<option value = '0'>" + "No division is selected" + "</option>";
            }
            $.each(options, function (key, value) {
                responseOptions += "<option value='" + value.Id + "'>" + value.Name + "</option>";
            });
            privates.renderNewTournamentDivisionsRow(responseOptions, isDisabled);
            $(".deleteTeamButton").bind("click", currNs.onDeleteTeamButtonClick);
            if (isDisabled) {
                privates.addTournamentGroupsRow();
            } else {
                $('select[name="divisions"]').on('change', privates.addTournamentGroupsRow);
            }
        });
    };

    privates.addTournamentGroupsRow = function () {

        var selectedGroups = $("select :selected");
        for (var i = 0; i < selectedGroups.length; i++) {
            if (selectedGroups[i].value === 0) {
                return false;
            }
        }

        privates.getAllGroupsOptions(function (options) {
            var isHidden = false;
            var isDisabled = false;
            var responseOptions = "";
            if (options.length === 0) {
                isHidden = true;
            } else if (options.length === 1) {
                isDisabled = true;
                privates.DeleteGroupsMarkup();
            } else {
                privates.DeleteGroupsMarkup();
                responseOptions += "<option value = '0'>" + "No group is selected" + "</option>";
            }
            $.each(options, function (key, value) {
                responseOptions += "<option value='" + value.Id + "'>" + value.Name + "</option>";
            });
            privates.renderNewTournamentGroupsRow(responseOptions, isHidden, isDisabled);
            $('select[name="divisions"]').on('change', privates.DeleteGroupsMarkup);
        });

    };

    privates.DeleteGroupsMarkup = function () {
        var result = $('td[name="groups"]');
        result.remove();
    };

    currNs.onAddTeamToTournamentButtonClick = function () {
        privates.addTournamentTeamsRow();
        privates.addTournamentDivisionsRow();
        privates.addTournamentGroupsRow();
    };

    currNs.onAddTeamsButtonButtonClick = function () {
        var teamData = privates.getJsonForTournamentTeamsSave();
        var groupData = privates.getJsonForTournamentGroupsSave();

        if (teamData.List.length > 0 && groupData.List.length > 0) {
            $.post("/Tournaments/AddTeamsToTournament", { teams: teamData, groups: groupData })
                .done(privates.handleTeamsAddSuccess);
        }

    };

    currNs.onDeleteTeamButtonClick = function (eventData) {
        var currentRow = eventData.target.parentElement.parentElement;
        currentRow.remove();
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
    $("#addTeamsButton").bind("click", currNs.onAddTeamsButtonButtonClick);
    $(".deleteTeamFromTournamentButton").bind("click", currNs.onDeleteTeamFromTournamentButtonClick);
});
