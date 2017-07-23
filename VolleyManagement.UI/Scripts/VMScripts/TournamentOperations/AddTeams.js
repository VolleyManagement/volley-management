﻿$(document).ready(function () {
    'use strict';

    var currNs = VM.addNamespace("tournament.addTeams"),
        privates = {};
    var divisionCounter = 0;
    var groupCounter = 0;
    var deleteRowButtonCounter = 0;
    var MAX_TEAMS_NUMBER = 0;
    var teamCounter = 0;

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
        var actualDivisionCounter = $(document.activeElement).attr('counter');
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
            TournamentId: $("[id='TournamentId']").val(),
            TeamsList: [],
            GroupsList: []
        };

        var selectedTeams = $("select[name='teams'] :selected");
        var selectedGroups = $("select[name='groups'] :selected");
        var selectedDivisions = $("select[name='divisions'] :selected");

        for (var i = 0; i < selectedTeams.length; i++) {
            if (selectedTeams[i].value !== "0") {
                result.TeamsList.push({
                    Id: selectedTeams[i].value                   
                });
            }
        }

        for (var j = 0; j < selectedGroups.length; j++) {
            if (selectedGroups[j].value !== "0") {
                result.GroupsList.push({
                    Id: selectedGroups[j].value,
                    DivisionId: selectedDivisions[j].value
                });
            }
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

    privates.getTornamentDivisionRowMarkup = function (responseOptions, isDisabled) {
        var disabled = "";

        if (isDisabled) {
            disabled = "disabled";
        }

        var result = "<td><select " + disabled + " name = 'divisions' counter = '" + divisionCounter + "'>" + responseOptions + "</select></td>"
            + "<td class = 'markup' counter = '" + divisionCounter +"'></td>"
            + "<td><button class='deleteTeamButton' counter = '" + divisionCounter +"'>Delete</button></td>";
        return result;
    };

    privates.renderNewTournamentDivisionsRow = function (responseOptions, isDisabled) {
        var teamTableData = $("select[name='teams']:last").parent().parent();
        teamTableData.append(privates.getTornamentDivisionRowMarkup(responseOptions, isDisabled));
    };

    privates.getTornamentGroupRowMarkup = function (responseOptions, isDisabled, actualDivisionCounter) {
        var disabled = "";
        deleteRowButtonCounter = actualDivisionCounter;
        groupCounter = actualDivisionCounter;
        if (isDisabled) {
            disabled = "disabled";
        }

        var result = "<td><select " + disabled + " name='groups' counter = '" + groupCounter + "' >" + responseOptions + "</select></td>"
            + "<td><button class='deleteTeamButton' counter = '" + deleteRowButtonCounter + "'>Delete</button></td>";
        return result;
    };

    privates.renderNewTournamentGroupsRow = function (responseOptions, isDisabled) {
        var actualDivisionCounter = $(document.activeElement).attr('counter');
        $("select[name ='divisions'][counter='" + actualDivisionCounter + "']:last", privates.tornamentTeamsTable).parent().parent().append(privates.getTornamentGroupRowMarkup(responseOptions, isDisabled, actualDivisionCounter));
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
                $('select[name="divisions"][counter="' + divisionCounter + '"]').on('change', privates.CheckIfEmptyGroupRowDraw);
            }            
        });
    };

    privates.CheckIfEmptyGroupRowDraw = function () {
        var actualDivisionCounter = $(document.activeElement).attr('counter');
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
       
            privates.RemoveGroupsAndDeleteRowButtonMarkup();
            privates.renderNewTournamentGroupsRow(responseOptions, isDisabled);
            $(".deleteTeamButton").bind("click", currNs.onDeleteTeamButtonClick);
        });

    };

    privates.RemoveGroupsAndDeleteRowButtonMarkup = function () {
        var actualDivisionCounter = $(document.activeElement).attr('counter');
        var group = actualDivisionCounter;
        var deleteButton = actualDivisionCounter;

        $('select[name = "groups"][counter = "' + group + '"]').parent().remove();
        $('[class = "markup"][counter = "' + group + '"]').remove();
        $('[class = "deleteTeamButton"][counter = "' + deleteButton +'"]').parent().remove();
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

        if (teamData.TeamsList.length === teamData.GroupsList.length) {      
            $.post("/Tournaments/AddTeamsToTournament", teamData)
                .done(privates.handleTeamsAddSuccess);
        } else {
            alert("Not all parameters was selected!");
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
