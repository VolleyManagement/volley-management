$(document).ready(function() {
    'use strict';

    // register namespace
    var editScope = $("div[vmscope='team_edit']"),
        handlers = VM.addNamespace("team.handlers"),
        privates = {};


    privates.teamId = (function() {

        var teamIdField = $("[name='Id']", editScope);

        return null;
    })();
    privates.selectedPlayers = [];
    privates.captainNameInput = $("#captainFullName", editScope);
    privates.teamPlayersTable = $("#teamRoster", editScope);
    privates.captainRow = null; // set on render

    // HELPERS
    privates.updateSelectedPlayers = function() {
        privates.selectedPlayers = [];
        $('input .teamPlayerId', privates.teamPlayersTable).each(function(item) {
            var value = item.val();
            if (value) {
                privates.selectedPlayers.push(value);
            }
        });
    };

    privates.allPlayersAreSet = function() {

        var rows = $('tr .teamPlayer', privates.teamPlayersTable),
            result = true,
            item,
            i;

        for (i = 0; i < rows.length; i++) {
            item = rows[i];
            if (!item.hasClass('captain')) {
                if (item.value === "") {
                    result = false;
                    break;
                }
            }
        }

        return result;
    };

    privates.getTeamPlayerRowMarkup = function(config) {
        var rowNumber = config.rowNumber,
            playerName = config.playerName || "",
            playerId = config.playerId || "",
            isCaptain = config.isCaptain || false,
            hiddenRow = "",
            rowClass = "teamPlayer",
            playerNameInput = "",
            playerIdInput = "<input type='text name='Roster[" + rowNumber + "].Id' value='" + playerId + "' class='teamPlayerId' hidden />",
            deleteButton = '';

        if (config.isHidden) {
            hiddenRow = "hidden";
        }

        if (isCaptain) {
            rowClass += " captain";
            playerNameInput = "<input type='text' name='Roster[" + rowNumber + "].FullName' class='teamPlayerName' value='" + playerName + "' readonly/>";
        } else {
            playerNameInput = "<input type='text' name='Roster[" + rowNumber + "].FullName' class='teamPlayerName' value='" + playerName + "'/>";
            buttonTemplate = "<button class='deleteTeamPlayerButton>Delete</button>'";
        }

        return "<tr class='" + rowClass + "' "+ hiddenRow + ">" +
               "<td>" + playerNameInput + "</td>" +
               "<td>" + playerIdInput + "</td>" +
               "<td>" + deleteButton + "</td></tr>";
    };

    // AUTOCOMPLETE
    privates.getAutocompleteUrl = function(config) {

        var searchString,
            isCaptain = "false",
            selectedPlayers = "",
            result = "";

        if (config.searchString) {
            searchString = encodeURI(config.searchString);

            if (config.isCaptain === true) {
                isCaptain = "true";
            }

            if (privates.selectedPlayers.length > 0) {
                selectedPlayers = "&selectedPlayers=" + privates.selectedPlayers.join();
            }

            result = "/Players/GetFreePlayers?searchString=" + searchString + "&isCaptain=" + isCaptain + selectedPlayers;
        }

        return result;
    };

    privates.executeCompleter = function(url, responseHandler) {

        var processedData = [];

        if (url) {
            $.getJSON(url, function(data, status, xhr) {
                $.each(data, function(key, value) {
                    processedData.push({
                        id: value.Id,
                        value: value.FullName
                    });
                });

                responseHandler(processedData);
            });
        }
    };

    privates.playersCompleter = function(requestObj, responseHandler) {

        var url = privates.getAutocompleteUrl({
            searchString: requestObj.term,
            isCaptain: false
        });

        privates.executeCompleter(url, responseHandler);
    };

    privates.captainCompleter = function(requestObj, responseHandler) {

        var url = privates.getAutocompleteUrl({
            searchString: requestObj.term,
            isCaptain: true
        });

        privates.executeCompleter(url, responseHandler);
    };

    privates.onPlayerSelect = function(eventObj, selectedItem) {
        var selectedId = selectedItem.item.id,
            selectedName = selectedItem.item.value;

        // TODO: id update logic

        privates.updateSelectedPlayers();
    };

    privates.onCaptainSelect = function(eventObj, selectedItem) {
        var selectedId = selectedItem.item.id,
            selectedName = selectedItem.item.value;

        // TODO: id update logic

        privates.updateSelectedPlayers();
    };



    // HANDLERS

    handlers.onCaptainChange = function(obj) {
        console.log(obj);
        // TODO: check if player was chosen correctly
        // TODO: Hide first row if captain empty
        $('tr .captain > input .teamPlayerName', privates.teamPlayersTable).val("TEST");
    };

    handlers.onPlayerChange = function(obj) {
        console.log(obj);
        // TODO: check if player was chosen correctly
    };

    handlers.deleteTeam = function(teamId, teamName) {
        var message = $("#DeleteConfirmationMessage").val();
        var confirmation = confirm(message + ' "' + teamName + '" ?');
        if (confirmation) {
            $.ajax({
                url: 'Teams/Delete',
                type: 'POST',
                data: {
                    id: teamId
                },
                dataType: 'json',
                success: function(resultJson) {
                    alert(resultJson.Message);
                    if (resultJson.OperationSuccessful) {
                        $("#team" + teamId).remove();
                    } else {
                        window.location.pathname = "Mvc/Teams";
                    }
                }
            });
        }
    };

    handlers.addTeamPlayersRow = function(config) {
        if (privates.allPlayersAreSet()) {

            config.rowNumber = privates.getTeamPlayerRowMarkup(privates.selectedPlayers.length + 1);
            config.isCaptain = config.isCaptain || false;

            $('tbody:last', privates.teamPlayersTable).append(config);

            // TODO: check if append returns value, so it is possible to increase perfomance

            $('tbody:last', privates.teamPlayersTable).autocomlete({
                minLength: 2,
                source: privates.playersCompleter,
                select: privates.onPlayerSelect,
                delay: 500
            });
        }
    };

    handlers.deleteTeamPlayersRow = function(obj) {
        console.log(obj);
        // TODO: delete parent row
    };

    handlers.showCreateTeamErrors = function() {};


    // RENDER TEAM PLAYERS
    (function () {
        var captainRowIsHidden = false,
            teamPlayersJson = $(".dataForTeamPlayersTable").val(),
            playersData = [];

        $('tbody:first-child', privates.teamPlayersTable).prepend(privates.getTeamPlayerRowMarkup({
            rowNumber: 1,
            isHidden: captainRowIsHidden,
            isCaptain: true
        }));

        privates.captainRow = $('tbody:first-child', privates.teamPlayersTable);

        if (teamPlayersJson) {
            playersData = JSON.stringify(teamPlayersJson);
            if (playersData) {
                handlers.addTeamPlayersRow({
                    playerName: playersData.fullName,
                    playerid: playersData.Id,
                });
            }
        }

        privates.updateSelectedPlayers();

    })();


    /// Bindings
    $(".addPlayerToTeamButton", editScope).bind('click', handlers.addRowToTeamPlayersTable);
    privates.captainNameInput.bind('change', handlers.onCaptainChange);
    privates.captainNameInput.autocomplete({
                minLength: 2,
                source: privates.captainCompleter,
                select: privates.onCaptainSelect,
                delay: 500
    });
})
