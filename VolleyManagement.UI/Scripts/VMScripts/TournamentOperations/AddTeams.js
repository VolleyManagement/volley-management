$(document).ready(function () {
    'use strict';

    var currNs = VM.addNamespace("tournament.addTeams"),
     privates = {};

    privates.tornamentTeamsTable = $("#tornamentRoster");

    privates.getAllTeamsOptions = function (callback) {
        $.getJSON("/Teams/GetAllTeams", callback);
    }

    privates.handleTeamsAddSuccess = function (data, status, xhr) {
        if (data.Message) {
            alert(data.Message);
            return false;
        }
        location.reload();
    };

    privates.getJsonForTournamentTeamsSave = function () {
        var result = {
            TournamentId: location.search.split('tournamentId=')[1],
            List: []
        }
        var selectedTeams = $("select :selected");

        for (var i = 0; i < selectedTeams.length; i++) {
            if (selectedTeams[i].value != 0) {
                result.List.push({
                    Id: selectedTeams[i].value
                });
            }
        }

        return result;
    }

    privates.getTornamentTeamsRowMarkup = function (responseOptions) {
        var result = "<tr><td><select>" + responseOptions + "</select></td>" +
                    "<td><button class='deleteTeamButton'>Delete</button></td></tr>";
        return result;
    }

    privates.renderNewTournamentTeamsRow = function (responseOptions) {
        $("tr:last", privates.tornamentTeamsTable).after(privates.getTornamentTeamsRowMarkup(responseOptions));
    }

    privates.addTournamentTeamsRow = function () {
        var context = $("tr:last", privates.tornamentTeamsTable);
        var elem = $("select :selected", context);
        if (elem.val() !== "0") {
            privates.getAllTeamsOptions(function (options) {
                var responseOptions = "<option value = '0'>" + currNs.teamIsNotSelectedMessage + "</option>";
                $.each(options, function (key, value) {
                    responseOptions += "<option value='" + value.Id + "'>" + value.Name + "</option>";
                })
                privates.renderNewTournamentTeamsRow(responseOptions);
                $(".deleteTeamButton").bind("click", currNs.onDeleteTeamButtonClick);
            });
        }
        return false;
    }

    currNs.onAddTeamToTournamentButtonClick = function () {
        privates.addTournamentTeamsRow();
    }

    currNs.onAddTeamsButtonButtonClick = function () {
        var data = privates.getJsonForTournamentTeamsSave();        
        if (data.List.length > 0) {
            $.post("/Tournaments/AddTeamsToTournament", data)
                .done(privates.handleTeamsAddSuccess);
        }
    }

    currNs.onDeleteTeamButtonClick = function (eventData) {
        var currentRow = eventData.target.parentElement.parentElement;
        currentRow.remove();
    }

    currNs.onDeleteTeamFromTournamentButtonClick = function (eventData) {
        var teamId = eventData.target.id;
        var tournamentId =  location.search.split('tournamentId=')[1];
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
    }

    $("#addTeamToTournamentButton").bind("click", currNs.onAddTeamToTournamentButtonClick);
    $("#addTeamsButton").bind("click", currNs.onAddTeamsButtonButtonClick);
    $(".deleteTeamFromTournamentButton").bind("click", currNs.onDeleteTeamFromTournamentButtonClick);
});