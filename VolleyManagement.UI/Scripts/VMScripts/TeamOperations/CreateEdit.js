$(document).ready(function() {
    'use strict';

    // register namespace
    var currentScope = $("div[vm_scope='team_edit']"),
        teamUnderEdit = false,
        handlers = VM.addNamespace("team.editHandlers"),
        privates = {};

    teamUnderEdit = document.location.pathname.search("Create") > 0 ||
        document.location.pathname.search("create") > 0 ||
        document.location.pathname.search("Edit") > 0 ||
        document.location.pathname.search("edit") > 0;

    privates.teamId = (function() {

        var teamIdField = $("[name='Id']", currentScope);

        return null;
    })();

    privates.selectedPlayers = [];
    privates.teamPlayersTable = $("#teamRoster", currentScope);
    privates.captainRow = null; // set on render

    // HELPERS
    privates.updateSelectedPlayers = function() {
        privates.selectedPlayers = [];
        $('.teamPlayerInput', privates.teamPlayersTable).each(function(key, value) {
            var playerId = privates.getPlayerId(value);
            if (playerId) {
                privates.selectedPlayers.push(playerId);
            }
        });
    };

    privates.allPlayersAreSet = function() {

        var inputs = $('.teamPlayerInput', privates.teamPlayersTable),
            length = inputs.length,
            result = true,
            item,
            i;

        for (i = 0; i < length; i++) {
            item = inputs[i];
            if (!item.classList.contains('captain')) {
                if (!privates.getPlayerId(item)) {
                    result = false;
                    break;
                }
            }
        }

        return result;
    };

    privates.getTeamPlayerRowMarkup = function(config) {
        var playerName = config.playerName || "",
            playerId = config.playerId || 0,
            isCaptain = config.isCaptain || false,
            rowClass = isCaptain ? "teamPlayer captain" : "teamPlayer",
            customAttributes = "vm_playerid ='" + playerId + "'",
            inputClasses = isCaptain ? "teamPlayerInput captain" : "teamPlayerInput",
            rowOptions = "<button class='viewTeamPlayerDetailsButton'>Details</button>",
            result;

        if (teamUnderEdit) {
            rowOptions += (isCaptain ? "" : " <button class='deleteTeamPlayerButton'>Delete</button>");
        }

        result = "<tr class='" + rowClass + "'>" +
            "<td><input type='text' class='" + inputClasses + "' value='" + playerName + "' " + customAttributes + "/></td>" +
            "<td>" + rowOptions + "</td></tr>"

        return result;
    };

    privates.getPlayerId = function(inputElement) {
        var result;

        if (inputElement.attr) {
            result = parseInt(inputElement.attr("vm_playerid"));
        } else {
            result = parseInt(inputElement.attributes["vm_playerid"].value);
        }

        return result || 0;
    };

    privates.setPlayerId = function(inputElement, id) {
        var attr;

        id = id || 0;

        if (inputElement.attr) {
            inputElement.attr("vm_playerid", id);
        } else {
            attr = document.createAttribute("vm_playerid");
            attr.value = id;
            inputElement.attributes.setNamedItem(attr);
        }
    };

    privates.setCaptainRowValues = function(name, id) {
        var inputElement = $(".teamPlayerInput", privates.captainRow);
        inputElement.val(name);
        privates.setPlayerId(inputElement, id);

        if (!name) {
            privates.captainRow.hide();
        } else {
            privates.captainRow.show();
        }
    };

    privates.renderNewTeamPlayerRow = function(config) {
        if (!config) {
            config = {};
        };

        config.isCaptain = false;
        $('tr:last', privates.teamPlayersTable).after(privates.getTeamPlayerRowMarkup(config));
        privates.setRowOptionsListeners($('tr:last', privates.teamPlayersTable));
    };

    privates.setTeamPlayerInputListeners = function(inputElement) {
        inputElement.bind('change', handlers.onTeamPlayerInputChange);
        inputElement.bind('blur', handlers.onTeamPlayerInputBlur);
        inputElement.autocomplete({
            minLength: 2,
            source: privates.teamPlayerCompleter,
            select: privates.onTeamPlayerSelect,
            delay: 500
        });
    };

    privates.setRowOptionsListeners = function(teamPlayerRow) {
        $(".deleteTeamPlayerButton", teamPlayerRow).bind('click', handlers.deleteTeamPlayersRow);
        $(".viewTeamPlayerDetailsButton", teamPlayerRow).bind('click', handlers.viewTeamPlayerDetails);
    };

    privates.getCaptainEditInput = function() {
        return $("#Captain_FullName", currentScope);
    };

    privates.addTeamPlayersRow = function(config) {
        var teamPlayerInput = null;

        privates.renderNewTeamPlayerRow(config);

        if (teamUnderEdit) {
            teamPlayerInput = $('.teamPlayerInput:last', privates.teamPlayersTable);
            privates.setTeamPlayerInputListeners(teamPlayerInput);
        }

        return false;
    };

    privates.getJsonForTeamSave = function() {
        var result = {
            //Id: privates.teamId,
            Name: $("#Name", currentScope).val(),
            Coach: $("#Coach", currentScope).val(),
            Achievements: $("#Achievements", currentScope).val(),
            Captain: {
                Id: privates.getPlayerId(privates.getCaptainEditInput())
            },
            Roster: []
        };

        $.each(privates.selectedPlayers, function(key, value) {
            result.Roster.push({
                Id: value
            });
        });

        return result;
    };


    if (teamUnderEdit) {


        privates.teamIsValid = function(teamData) {
            return $("form", currentScope).valid();
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

        privates.teamPlayerCompleter = function(requestObj, responseHandler) {

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

        privates.onTeamPlayerSelect = function(eventObj, selectedItem) {
            privates.setPlayerId(eventObj.target, selectedItem.item.id)
            privates.updateSelectedPlayers();
        };

        privates.onCaptainSelect = function(eventObj, selectedItem) {
            var selectedId = selectedItem.item.id,
                selectedName = selectedItem.item.value;

            privates.setPlayerId(eventObj.target, selectedId);

            privates.setCaptainRowValues(selectedName, selectedId);
            privates.updateSelectedPlayers();
        };

        privates.handleTeamCreateEditSuccess = function(data, status, xhr) {
            document.location = "../Teams/Index";
        };

        privates.handleTeamCreateEditFail = function(data, status, xhr) {
            alert("Create/Edit failed");
        };
    }


    // HANDLERS

    if (teamUnderEdit) {

        handlers.onAddPlayerToTeamButtonClick = function() {
            if (privates.allPlayersAreSet()) {
                privates.addTeamPlayersRow();
            }
            return false;
        };

        handlers.onTeamPlayerInputChange = function(eventData) {
            privates.setPlayerId(eventData.target, "");
            privates.updateSelectedPlayers();
            return false;
        };

        handlers.deleteTeamPlayersRow = function(eventData) {
            var currentRow = eventData.target.parentElement.parentElement;
            currentRow.remove();
            privates.updateSelectedPlayers();

            return false;
        };

        handlers.onTeamPlayerInputBlur = function(eventData) {
            var inputElement = eventData.target,
                playerId = privates.getPlayerId(inputElement);

            if (!playerId) {
                if (inputElement.value !== "") {
                    alert("You can choose only existing player!");
                    inputElement.focus();
                }
            }

            return false;
        };

        handlers.onCaptainBlur = function(eventData) {
            var inputElement = eventData.target,
                captainId = privates.getPlayerId(inputElement);

            if (!captainId) {
                if (inputElement.value !== "") {
                    alert("You can choose only existing player!");
                    inputElement.focus();
                } else {
                    privates.setCaptainRowValues("", "");
                }
            }

            return false;
        };

        handlers.createTeam = function() {
            var teamData = privates.getJsonForTeamSave();

            if (privates.teamIsValid(teamData)) {
                $.post("/Teams/Create", teamData)
                    .done(privates.handleTeamCreateEditSuccess)
                    .fail(privates.handleTeamCreateEditFail);
            }

            return false;
        };

        handlers.saveTeam = function() {
            var teamData = privates.getJsonForTeamSave();

            if (privates.validateTeamData(teamData)) {
                $.post("/Teams/Edit", teamData)
                    .done(privates.handleTeamCreateEditSuccess)
                    .fail(privates.handleTeamCreateEditFail);
            }

            return false;
        };
    }

    handlers.viewTeamPlayerDetails = function(eventData) {
        // TODO: add necessary actions
        return false;
    };


    // INIT RENDER
    // STRICTLY BEFORE BINDINGS to avoid wrong page state
    (function() {

        var teamPlayersJson = $(".dataForTeamPlayersTable").val(),
            playersData = [],
            inputElement;

        // "Add player to team" button
        if (teamUnderEdit) {
            privates.teamPlayersTable.before("<button id='addPlayerToTeamButton'>Add player to team</button>");
        }

        // Team captain row
        $('tr:first', privates.teamPlayersTable).after(privates.getTeamPlayerRowMarkup({
            isCaptain: true
        }));
        privates.captainRow = $('tr.captain', privates.teamPlayersTable);
        inputElement = privates.getCaptainEditInput();
        privates.setCaptainRowValues(inputElement.value, privates.getPlayerId(inputElement));

        // Team players rows
        if (teamPlayersJson) {
            playersData = JSON.stringify(teamPlayersJson);
            if (playersData) {
                privates.addTeamPlayersRow({
                    playerName: playersData.FullName,
                    playerid: playersData.Id,
                });
            }
        }

        privates.updateSelectedPlayers();

    })();


    /// BINDINGS
    (function() {

        var captainNameInput;

        if (teamUnderEdit) {

            captainNameInput = privates.getCaptainEditInput();

            // Captain input
            privates.captainRow.bind('click', handlers.viewTeamPlayerDetails);
            captainNameInput.bind('blur', handlers.onCaptainBlur);
            captainNameInput.bind('change', handlers.onTeamPlayerInputChange);
            captainNameInput.autocomplete({
                minLength: 2,
                source: privates.captainCompleter,
                select: privates.onCaptainSelect,
                delay: 500
            });

            //BUTTONS
            // Add player to team button
            $("#addPlayerToTeamButton", currentScope).bind('click', handlers.onAddPlayerToTeamButtonClick);

            // Create team button
            $("#createTeamButton", currentScope).bind("click", handlers.createTeam);
        }

    })();

});
