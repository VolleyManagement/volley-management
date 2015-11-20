$(document).ready(function () {
    'use strict';
    
    // register namespace
    var editScope = $("div[vmscope='team_edit']"),
        handlers = VM.addNamespace("team.handlers"),
        privateScope = {};

    // PRIVATE 
    
    privateScope.teamId = (function () {

        var teamIdField = $("[name='Id']", editScope);

        return null;

    })();
    privateScope.selectedPlayers = [];

    //Autocompleter
    privateScope.getAutocompleteUrl = function (config) {

        var searchString,
            isCaptain = "false",
            selectedPlayers = "null",
            result = "";

        if (config.searchString) {
            searchString = encodeURI(config.searchString);

            if (config.isCaptain === true) {
                isCaptain = "true";
            }
            
            if (privateScope.selectedPlayers.length > 0) {
                selectedPlayers = privateScope.selectedPlayers.join();
            }

            result = "/Mvc/Players/GetFreePlayers?searchString=" + searchString + "&isCaptain=" + isCaptain + "&selectedPlayers=" + selectedPlayers;
        }

        return result;
    };

    privateScope.executeCompleter = function(url, responseHandler) {

        var processedData = [];
        
        if (url) {
            $.getJSON(url, function (data, status, xhr) {
                $.each(data, function(item) {
                    processedData.push({
                        label: item.FullName,
                        value: item.Id
                    });
                });
            });
        }
        
        responseHandler(processedData);
    };

    privateScope.playersCompleterFunc = function (requestObj, responseHandler) {
        
        var url = privateScope.getAutocompleteUrl({
            searchString: requestObj.term,
            isCaptain: false
        });

        privateScope.executeCompleter(url, responseHandler);
    };

    privateScope.onCaptainSelect = function (selectedItem) {
        var selectedId = selectedItem.value;
        
        if ($.inArray(selectedId) === -1) {
            selectedItem.push(selectedId);
        }
    };
    
    privateScope.onPlayerSelect = function (selectedItem) {
        var selectedId = selectedItem.value;

        if ($.inArray(selectedId) === -1) {
            selectedItem.push(selectedId);
        }
    };

    privateScope.captainCompleterFunc = function(requestObj, responseHandler) {
        'use strict';

        var url = privateScope.getAutocompleteUrl({
            searchString: requestObj.term,
            isCaptain: true
        });

        privateScope.executeCompleter(url, responseHandler);
    };

    privateScope.allPlayersAreSet = function (rows) {

        var result = true;
        
        for (var i = 0; i < rows.length; i++) {
            if (rows[i].value === "") {
                result = false;
            }
        }

        return result;
    };


    // Register handlers
    handlers.deleteTeam = function (teamId, teamName) {
        var message = $("#DeleteConfirmationMessage").val();
        var confirmation = confirm(message + ' "' + teamName + '" ?');
        if (confirmation) {
            $.ajax({
                url: 'Teams/Delete',
                type: 'POST',
                data: { id: teamId },
                dataType: 'json',
                success: function (resultJson) {
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

    handlers.changePage = function (newPage, numberOfPages) {
        ajaxPlayersRefresh(newPage);
        printPagesBar(newPage, numberOfPages);
    };

    handlers.ajaxPlayersRefresh = function (pageNumber) {

        $.get("ChoosePlayers",
            { page: pageNumber },
            function (data) { $("#currentPlayersPage").html(data); },
            "html");
    };

    handlers.addTeamPlayersRow = function () {
        'use strict';

        var teamPlayersTable = $("#teamRoster", editScope),
                rows = $('.teamPlayer', teamPlayersTable),
                currentCount = rows.length,
                template;

        if (privateScope.allPlayersAreSet(rows)) {
            template = "<tr><td><input type='text' name='Roster[" + currentCount + "].FullName' class='teamPlayer'/></td><td></td></tr>";
            $('#teamRoster > tbody:last', editScope).append(template);
            $('#teamRoster > tbody:last', editScope).autocomlete({
                minLength: 2,
                source: privateScope.playersCompleterFunc,
                select: privateScope.onPlayerSelect,
                delay: 500
            });
        }

        //var newElementId = "player_" + id;
        //$("#" + newElementId).hide();

        //var indexOfNewPlayer = window.opener.$("#teamRoster tr.rosterPlayer").length;

        //if (window.name == "ChoosingRosterWindow") {
        //    if (!(window.opener.$("#" + newElementId).get(0))) {
        //        var fullNameInput = "<input type='text' name='Roster[" + indexOfNewPlayer + "].FullName' value='" + fullName + "' readonly />";
        //        var idInput = "<input id='" + newElementId + "' type='text' name='Roster[" + indexOfNewPlayer + "].Id' value='" + id + "' hidden />";

        //        var newPlayer = "<tr class='rosterPlayer'><td>" + fullNameInput + "</td><td>" + idInput + "<td></tr>";
        //        window.opener.$("#teamRoster").children().append(newPlayer);
        //    }
        //} else {
        //    window.opener.$("#captainFullName").val(fullName);
        //    window.opener.$("#captainId").val(id);
        //    window.close();
        //}
    };

    handlers.deleteTeamPlayersRow = function(deletedId) {

        var indexOfItem = $.inArray(deletedId),
            rows = $('#teamRoster > tbody:tr', editScope),
            length = rows.length,
            row,
            i;

        if (indexOfItem !== -1) {
            selectedItem.splice(indexOfItem, 1);
        }

        for (var i = 0; i < length; i++) {
            row = rows[i];
            if ($('.idValueInput', row).val() === deletedId) {
                row.remove();
                break;
            }
        }

        ;
    };

    handlers.showCreateTeamErrors = function showCreateTeamErrors() {
    };

    /// Bindings
    $(".addPlayerToTeamButton", editScope).bind('click', handlers.addRowToTeamPlayersTable);
})





