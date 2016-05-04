$(document).ready(function () {
    'use strict';

    // register namespace
    var currNs = VM.addNamespace("team.createEdit"),
        privates = {};

    privates.teamUnderEdit = document.location.pathname.search("Edit") > 0 || document.location.pathname.search("edit") > 0;
    privates.teamId = privates.teamUnderEdit ? $("[name='Id']").val() : 0;
    privates.playersData = [];

    privates.playerIdAttributeName = "data-vm-playerid";
    privates.notValidPlayerInputMessage = "You can choose only existing player!";
    privates.selectedPlayers = [];
    privates.teamPlayersTable = $("#teamRoster");
    privates.defaultTeamPlayerInputClasses = "teamPlayerInput existingPlayerRequired";
    privates.captainRow = null; // set on render

    // HELPERS
    privates.updateSelectedPlayers = function () {
        privates.selectedPlayers = [];
        $('.teamPlayerInput', privates.teamPlayersTable).each(function (key, value) {
            var playerId = privates.getPlayerId(value);
            if (playerId) {
                privates.selectedPlayers.push(playerId);
            }
        });
    };

    privates.areAllPlayersSetToInputs = function () {

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

    privates.getTeamPlayerRowMarkup = function (config) {
        var playerName = config.playerName || "",
            playerId = config.playerId || 0,
            isCaptain = config.isCaptain || false,
            rowClass = isCaptain ? "teamPlayer captain" : "teamPlayer",
            customAttributes = privates.playerIdAttributeName + " ='" + playerId + "'",
            inputClasses = privates.defaultTeamPlayerInputClasses,
            inputName = playerId,
            rowOptions = "<button class='viewTeamPlayerDetailsButton'>Details</button>",
            validationSpan = "<span class='field-validation-valid' data-valmsg-for='" + playerId + "' data-valmsg-replace='true'></span>",
            result;

        inputClasses += isCaptain ? " captain" : "";
        rowOptions += (isCaptain ? "" : " <button class='deleteTeamPlayerButton'>Delete</button>");

        result = "<tr class='" + rowClass + "'>" +
            "<td><div><input type='text' name='" + inputName + "' class='" + inputClasses + "' value='" + playerName + "' " + customAttributes + "/>" + validationSpan + "</div></td>" +
            "<td>" + rowOptions + "</td></tr>";

        return result;
    };

    privates.getPlayerId = function (inputElement) {
        var result;

        if (inputElement.attr) {
            result = parseInt(inputElement.attr(privates.playerIdAttributeName));
        } else {
            result = parseInt(inputElement.attributes[privates.playerIdAttributeName].value);
        }

        return result || 0;
    };

    privates.setPlayerId = function (inputElement, id) {
        var attr;

        id = id || 0;

        if (inputElement.attr) {
            inputElement.attr(privates.playerIdAttributeName, id);
        } else {
            attr = document.createAttribute(privates.playerIdAttributeName);
            attr.value = id;
            inputElement.attributes.setNamedItem(attr);
        }
    };

    privates.updateCaptainRowValues = function (name, id) {
        var inputElement = $(".teamPlayerInput", privates.captainRow),
            sourceInput;

        if (name === undefined || id === undefined) {
            sourceInput = privates.getCaptainEditInput();
            id = privates.getPlayerId(sourceInput);
            name = sourceInput.val();
        }

        inputElement.val(name);
        inputElement.attr("name", id);
        $(inputElement.parent()).children().last().attr("data-valmsg-for", id);
        privates.setPlayerId(inputElement, id);

        if (!name) {
            privates.captainRow.hide();
        } else {
            privates.captainRow.show();
        }
    };

    privates.renderNewTeamPlayerRow = function (config) {
        config = config || {};

        config.isCaptain = false;
        $('tr:last', privates.teamPlayersTable).after(privates.getTeamPlayerRowMarkup(config));
        privates.setRowOptionsListeners($('tr:last', privates.teamPlayersTable));
    };

    privates.setTeamPlayerInputListeners = function (inputElement) {
        inputElement.bind('change', currNs.onTeamPlayerInputChange);
        inputElement.bind('blur', currNs.onTeamPlayerInputBlur);
        inputElement.autocomplete({
            minLength: 2,
            source: privates.teamPlayerCompleter,
            select: privates.onTeamPlayerSelect,
            delay: 500
        });
    };

    privates.setRowOptionsListeners = function (teamPlayerRow) {
        $(".deleteTeamPlayerButton", teamPlayerRow).bind('click', currNs.deleteTeamPlayersRow);
        $(".viewTeamPlayerDetailsButton", teamPlayerRow).bind('click', currNs.viewTeamPlayerDetails);
    };

    privates.getCaptainEditInput = function () {
        return $("#Captain_FullName");
    };

    privates.addTeamPlayersRow = function (config) {
        var teamPlayerInput = null;

        privates.renderNewTeamPlayerRow(config);

        teamPlayerInput = $('.teamPlayerInput:last', privates.teamPlayersTable);
        privates.setTeamPlayerInputListeners(teamPlayerInput);


        return false;
    };

    privates.getJsonForTeamSave = function () {
        var result = {
            Name: $("#Name").val(),
            Coach: $("#Coach").val(),
            Achievements: $("#Achievements").val(),
            Captain: {
                Id: privates.getPlayerId(privates.getCaptainEditInput())
            },
            Roster: []
        };

        if (privates.teamUnderEdit) {
            result.Id = privates.teamId;
        }

        $.each(privates.selectedPlayers, function (key, value) {
            result.Roster.push({
                Id: value
            });
        });

        return result;
    };


    privates.teamIsValid = function (teamData) {
        return $("form").valid();
    };

    // AUTOCOMPLETE

    privates.getAutocompleteUrl = function (config) {

        var searchString,
            isCaptain = "false",
            selectedPlayers,
            result = "";

        if (config.searchString) {
            result = "/Players/GetFreePlayers?searchString=" + encodeURI(config.searchString);

            if (privates.selectedPlayers.length > 0) {
                selectedPlayers = privates.selectedPlayers.join();

                if (config.isCaptain) {
                    result += ("&includeList=" + selectedPlayers);
                } else {
                    result += ("&excludeList=" + selectedPlayers);
                }

                if (privates.teamUnderEdit) {
                    result += ("&includeTeam=" + privates.teamId);
                }
            }
        }

        return result;
    };

    privates.executeCompleter = function (url, responseHandler) {

        var processedData = [];

        if (url) {
            $.getJSON(url, function (data, status, xhr) {
                $.each(data, function (key, value) {
                    processedData.push({
                        id: value.Id,
                        value: value.FullName
                    });
                });

                responseHandler(processedData);
            });
        }
    };

    privates.teamPlayerCompleter = function (requestObj, responseHandler) {

        var url = privates.getAutocompleteUrl({
            searchString: requestObj.term,
            isCaptain: false
        });

        privates.executeCompleter(url, responseHandler);
    };

    privates.captainCompleter = function (requestObj, responseHandler) {

        var url = privates.getAutocompleteUrl({
            searchString: requestObj.term,
            isCaptain: true
        });

        privates.executeCompleter(url, responseHandler);
    };

    privates.onTeamPlayerSelect = function (eventObj, selectedItem) {
        privates.setPlayerId(eventObj.target, selectedItem.item.id);
        privates.updateSelectedPlayers();
    };

    privates.onCaptainSelect = function (eventObj, selectedItem) {
        var selectedId = selectedItem.item.id,
            selectedName = selectedItem.item.value;

        privates.setPlayerId(eventObj.target, selectedId);

        if (selectedId) {
            $("tr .teamPlayerInput", privates.teamPlayersTable).each(function (ind, el) {
                if (!$(el).hasClass("captain") && privates.getPlayerId(el) === selectedId) {
                    el.parentNode.parentNode.remove();
                }
            });
        }

        privates.updateCaptainRowValues(selectedName, selectedId);
        privates.updateSelectedPlayers();
    };

    privates.handleTeamCreateEditSuccess = function(data, status, xhr) {
        document.location = "/Teams/Index";
    };

    privates.handleTeamCreateEditFail = function (data, status, xhr) {
        alert("Create/Edit failed");
    };


    // NAMESPACE PUBLIC METHODS

    currNs.onAddPlayerToTeamButtonClick = function () {
        if (privates.areAllPlayersSetToInputs()) {
            privates.addTeamPlayersRow();
        }
        return false;
    };

    currNs.onTeamPlayerInputChange = function (eventData) {
        privates.setPlayerId(eventData.target, "");
        privates.updateSelectedPlayers();
        return false;
    };

    currNs.deleteTeamPlayersRow = function (eventData) {
        var currentRow = eventData.target.parentElement.parentElement;
        currentRow.remove();
        privates.updateSelectedPlayers();

        return false;
    };

    currNs.onTeamPlayerInputBlur = function (eventData) {
        var inputElement = eventData.target,
            playerId = privates.getPlayerId(inputElement);

        if (playerId) {
            eventData.target.name = playerId;
            eventData.target.parentElement.lastChild.dataset["valmsgFor"] = playerId;
        }
        else {
            if (inputElement.value !== "") {
                eventData.target.classList.add("existingPlayerRequired");
            }
            else {
                eventData.target.classList.remove("existingPlayerRequired");
            }
        }

        return false;
    };

    currNs.onCaptainBlur = function (eventData) {
        var inputElement = eventData.target,
            captainId = privates.getPlayerId(inputElement);

        if (!captainId) {
            if (inputElement.value === "") {
                privates.updateCaptainRowValues("", "");
            }
        }

        return false;
    };

    currNs.createTeam = function () {
        var teamData = privates.getJsonForTeamSave();

        if (privates.teamIsValid(teamData)) {
            $.post("/Teams/Create", teamData)
                .done(privates.handleTeamCreateEditSuccess)
                .fail(privates.handleTeamCreateEditFail);
        }

        return false;
    };

    currNs.saveTeam = function () {
        var teamData = privates.getJsonForTeamSave();

        if (privates.teamIsValid(teamData)) {
            $.post("/Teams/Edit", teamData)
                .done(privates.handleTeamCreateEditSuccess)
                .fail(privates.handleTeamCreateEditFail);
        }

        return false;
    };


    currNs.viewTeamPlayerDetails = function (eventData) {
        // TODO: add necessary actions
        return false;
    };


    // INIT RENDER
    // STRICTLY BEFORE BINDINGS to avoid wrong page state
    (function () {

        var playersData = currNs.teamRoster || [],
            inputElement;

        // Team captain row
        $('tr:first', privates.teamPlayersTable).after(privates.getTeamPlayerRowMarkup({
            isCaptain: true
        }));

        privates.captainRow = $('tr.captain', privates.teamPlayersTable);
        privates.updateCaptainRowValues();

        // Team players rows
        if (playersData.length > 0) {
            $.each(playersData, function (ind, val) {
                if (!val.isCaptain) {
                    privates.addTeamPlayersRow({
                        playerName: val.name,
                        playerId: val.id,
                    });
                }
            });
        }

        privates.updateSelectedPlayers();
    })();


    /// BINDINGS
    (function () {

        var captainNameInput;

        captainNameInput = privates.getCaptainEditInput();

        // Captain input
        privates.captainRow.bind('click', currNs.viewTeamPlayerDetails);
        captainNameInput.bind('blur', currNs.onCaptainBlur);
        captainNameInput.bind('change', currNs.onTeamPlayerInputChange);
        captainNameInput.autocomplete({
            minLength: 2,
            source: privates.captainCompleter,
            select: privates.onCaptainSelect,
            delay: 500
        });

        //BUTTONS
        // Add player to team button
        $("#addPlayerToTeamButton").bind('click', currNs.onAddPlayerToTeamButtonClick);

        // Create team button
        $("#createTeamButton").bind("click", currNs.createTeam);

        $("#saveTeamButton").bind("click", currNs.saveTeam);

        //Validation
        $.validator.addMethod("PlayerExist", function (value, element) {
            return +element.dataset["vmPlayerid"];
        }, privates.notValidPlayerInputMessage);

        $.validator.addClassRules("existingPlayerRequired", { PlayerExist: true });

    })();

});
