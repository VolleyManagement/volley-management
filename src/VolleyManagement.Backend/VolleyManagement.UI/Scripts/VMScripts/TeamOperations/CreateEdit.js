$(document).ready(function () {
  'use strict';

  // register namespace
  var currNs = VM.addNamespace("team.createEdit"),
    privates = {};

  privates.teamUnderEdit = document.location.pathname.search("Edit") > 0 || document.location.pathname.search("edit") > 0;
  privates.teamId = privates.teamUnderEdit ? $("[name='Id']").val() : 0;
  privates.playersData = [];

  privates.playerIdAttributeName = "data-vm-playerid";
  privates.selectedPlayers = [];
  privates.teamPlayersTable = $("#teamRoster");
  privates.DeletedPlayers = [];
  privates.AddedPlayers = [];
  var teamPlayerCounter = 0;
  var fullnameRegExp = /[a-zA-Zа-яА-ЯёЁіІїЇєЄ]{2,}[\s][a-zA-Zа-яА-ЯёЁіІїЇєЄ]{2,}/g;
  var fullNameCorrectValueCheck = /[\d`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]+/gi;
  var firstNameLastNameSplitter = /[\s]/g;
  var fullnameError = "\nUsername must have first and last name!";
  var captainHasSuchNameError = "\nName was used by captain!";
  var requiredFieldError = "\nRequired!";
  var anotherPlayerHasSuchName = "\nName was used by another player!";
  var fullNameContainsSpecialSymbols = "\nFullname can't contain special symbols or digits!";
  var processedFoundedPlayerData = [];

  // Draws markup for new team players - 'Roster'
  privates.getTeamPlayerRowMarkup = function (config) {
    var playerName = config.playerName || "",
      playerId = config.playerId || 0,
      customAttributes = privates.playerIdAttributeName + " ='" + playerId + "'",
      inputName = playerId;
    teamPlayerCounter++;

    var result = "<tr class='teamPlayer'>" +
      "<td><div><input type='text' name='" + inputName + "' class='teamPlayerInput' counter='" + teamPlayerCounter + "' value='" + playerName + "' "
      + customAttributes + "/></div></td><td><button class='deleteTeamPlayerButton'>Delete</button></td></tr>";

    return result;
  };

  // Gets actual team playerId/captainId
  privates.getPlayerId = function (inputElement) {
    var result;

    if (inputElement.attr) {
      result = parseInt(inputElement.attr(privates.playerIdAttributeName));
    } else {
      result = parseInt(inputElement.attributes[privates.playerIdAttributeName].value);
    }

    return result || 0;
  };

  // Sets actual team playerId/captainId
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

  // Sets handlers for player input field
  privates.setTeamPlayerInputListeners = function (inputElement) {
    var temporaryPlayerName = inputElement.val().toLowerCase();
    var temporaryPlayerId = privates.getPlayerId(inputElement);

    inputElement.bind("input keyup", function () {
      var $this = $(this);
      var delay = 500;

      clearTimeout($this.data('timer'));
      $this.data('timer', setTimeout(function () {
        $this.removeData('timer');

        privates.checkIfFullNameCorrect(inputElement);
        privates.checkIfPlayerHasCaptainName(inputElement);
        privates.checkIfAnotherPlayerAlreadyHasSuchName(inputElement);
        privates.checkIfFullnameContainsSpecSymbols(inputElement);
        privates.setsPlayerIdWithFoundedPlayerId(temporaryPlayerName, temporaryPlayerId, inputElement);
      }, delay));
    });

    inputElement.autocomplete({
      minLength: 2,
      source: privates.teamPlayerCompleter,
      select: function (event, ui) {
        privates.onTeamPlayerSelect(event, ui);
        inputElement.val(ui.item.value);
        privates.checkIfFullNameCorrect(inputElement);
        privates.checkIfFullnameContainsSpecSymbols(inputElement);
      },
      delay: 500
    });
  };

  privates.setsPlayerIdWithFoundedPlayerId = function (temporaryPlayerName, temporaryPlayerId, inputElement) {
    var fullName = inputElement.val().toLowerCase().trim();
    var defaultPlayerId = 0;

    if (privates.teamUnderEdit) {
      if (temporaryPlayerName !== fullName) {
        privates.setPlayerId(inputElement, defaultPlayerId);
      } else {
        privates.setPlayerId(inputElement, temporaryPlayerId);
      }
    }

    if (fullName === processedFoundedPlayerData[0].value.toLowerCase()) {
      privates.setPlayerId(inputElement, processedFoundedPlayerData[0].id);
    } else {
      privates.setPlayerId(inputElement, defaultPlayerId);
    }
  };

  // Check if fullname correct
  privates.checkIfFullNameCorrect = function (inputElement) {
    var incorrectFullNameCounter = inputElement.attr('counter');

    if (inputElement.val().match(fullnameRegExp) === null) {
      $(".incorrectFullName[incorrectFullNameCounter='" + incorrectFullNameCounter + "']").remove();
      inputElement.after("<span class = 'incorrectFullName' incorrectFullNameCounter = '" + incorrectFullNameCounter + "'>" + fullnameError + "</span>");
    } else {
      $(".incorrectFullName[incorrectFullNameCounter='" + incorrectFullNameCounter + "']").remove();
    }
  };

  privates.checkIfFullnameContainsSpecSymbols = function (inputElement) {
    var incorrectFullNameCounter = inputElement.attr('counter');

    if (inputElement.val().match(fullNameCorrectValueCheck) !== null) {
      $(".incorrectFullName[fullNameContainsSpecSymbolsCounter='" + incorrectFullNameCounter + "']").remove();
      inputElement.after("<span class = 'incorrectFullName' incorrectFullNameCounter = '" + incorrectFullNameCounter + "'>" + fullNameContainsSpecialSymbols + "</span>");
    } else {
      $(".incorrectFullName[fullNameContainsSpecSymbolsCounter='" + incorrectFullNameCounter + "']").remove();
    }
  }

  // Check if player input has captain value
  privates.checkIfPlayerHasCaptainName = function (inputElement) {
    var hasSuchNameCounter = inputElement.attr('counter');

    if ($.trim(inputElement.val()) === $.trim($("#Captain_FullName").val())) {
      $(".hasSuchName[playerCaptainCounter='" + hasSuchNameCounter + "']").remove();
      inputElement.after("<br/><span class = 'hasSuchName' playerCaptainCounter = '" + hasSuchNameCounter + "'>" + captainHasSuchNameError + "</span>");
      inputElement.css("border-color", "red");
    } else {
      inputElement.css("border-color", "initial");
      $(".hasSuchName[playerCaptainCounter='" + hasSuchNameCounter + "']").remove();
    }
  };

  // Check if player input has value of another player
  privates.checkIfAnotherPlayerAlreadyHasSuchName = function (inputElement) {
    var hasSuchNameCounter = inputElement.attr('counter');
    var actualTeamPlayers = [];

    if (teamPlayerCounter > 1) {
      for (var j = 1; j <= teamPlayerCounter; j++) {
        if ($(".teamPlayerInput[counter='" + j + "']").val() !== "" && j !== parseInt(inputElement.attr('counter'))) {
          actualTeamPlayers.push($(".teamPlayerInput[counter='" + j + "']").val());
        }
      }

      for (var i = 0; i < actualTeamPlayers.length; i++) {
        if ($.trim(inputElement.val()) === $.trim(actualTeamPlayers[i])) {
          inputElement.css("border-color", "red");
          $(".hasSuchName[playersCounter='" + hasSuchNameCounter + "']").remove();
          inputElement.after("<span class = 'hasSuchName' playersCounter = '" + hasSuchNameCounter + "'>" + anotherPlayerHasSuchName + "</span>");
          break;
        } else {
          inputElement.css("border-color", "initial");
          $(".hasSuchName[playersCounter='" + hasSuchNameCounter + "']").remove();
        }
      }
    }
  };

  // Sets handlers for delete button in teamPlayerRow
  privates.setRowOptionsListeners = function (teamPlayerRow) {
    $(".deleteTeamPlayerButton", teamPlayerRow).bind('click', currNs.deleteTeamPlayersRow);
  };

  // Adds row for player
  privates.addTeamPlayersRow = function (config) {
    config = config || {};

    $('tr:last', privates.teamPlayersTable).after(privates.getTeamPlayerRowMarkup(config));
    privates.setRowOptionsListeners($('tr:last', privates.teamPlayersTable));

    var teamPlayerInput = $('.teamPlayerInput[counter="' + teamPlayerCounter + '"]', privates.teamPlayersTable);
    privates.setTeamPlayerInputListeners(teamPlayerInput);

    return false;
  };

  // Grabs all actual data before 'Create'/'Edit' operation
  privates.getJsonForTeamSave = function () {
    var captainFullname = $("#Captain_FullName").val().trim().split(firstNameLastNameSplitter, 2);
    var result = {
      Name: $("#Name").val(),
      Coach: $("#Coach").val(),
      Achievements: $("#Achievements").val(),
      Captain: {
        Id: privates.getPlayerId($("#Captain_FullName")),
        FirstName: captainFullname[0],
        LastName: captainFullname[1]
      },
      Roster: [],
      AddedPlayers: [],
      DeletedPlayers: [],
      IsCaptainChanged: Boolean(0)
    };
    result.AddedPlayers = privates.AddedPlayers;
    result.DeletedPlayers = privates.DeletedPlayers;
    result.Roster.push({
      FirstName: captainFullname[0],
      LastName: captainFullname[1],
      Id: privates.getPlayerId($("#Captain_FullName"))
    });

    if (privates.teamUnderEdit) {
      result.Id = privates.teamId;
    }
    for (var i = 0; i < currNs.teamRoster.length; i++) {
      if (currNs.teamRoster[i].isCaptain === true) {
        if (currNs.teamRoster[i].name !== $("#Captain_FullName").val().trim()) {
          result.IsCaptainChanged = true;
        }
      }
    }
    var playersWhichWasInTeam = currNs.teamRoster || [];
    for (var j = 1; j <= teamPlayerCounter; j++) {
      var inputTeamPlayer = $(".teamPlayerInput[counter='" + j + "']");
      if (inputTeamPlayer.val() !== "" && inputTeamPlayer.val() !== undefined) {
        var fullName = inputTeamPlayer.val().trim().split(firstNameLastNameSplitter, 2);
        var playerId = privates.getPlayerId(inputTeamPlayer);

        var isThisPlayerIsNewForThisTeam =
          checkIfPlayerExists(playersWhichWasInTeam, playerId);
        if (isThisPlayerIsNewForThisTeam === false) {
          result.AddedPlayers.push({
            FirstName: fullName[0],
            LastName: fullName[1],
            Id: playerId
          });
        } else {
          result.Roster.push({
            FirstName: fullName[0],
            LastName: fullName[1],
            Id: playerId
          });
        }
      }
    }

    return result;
  };

  function checkIfPlayerExists(playersWhichWasInTeam, playerId) {
    var ifexist = false;
    for (var i = 0; i < playersWhichWasInTeam.length; i++) {
      if (+playersWhichWasInTeam[i].id === playerId) {
        ifexist = true;
      }
    }
    return ifexist;
  }


  // Check if form valid
  privates.teamIsValid = function () {
    var isDataValid = $("form").valid();
    if ($(".hasSuchName").length > 0 || $(".incorrectFullName").length > 0) {
      isDataValid = false;
    }
    return isDataValid;
  };

  // Builds specific url for 'TeamsController' method 'GetFreePlayers'
  privates.getAutocompleteUrl = function (config) {
    var selectedPlayers,
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

  // Gets Json with specific team player or captain from 'GetFreePlayers' method in 'PlyersController'
  privates.executeCompleter = function (url, responseHandler) {

    processedFoundedPlayerData = [];

    if (url) {
      $.getJSON(url, function (data) {
        $.each(data, function (key, value) {
          processedFoundedPlayerData.push({
            id: value.Id,
            value: value.FirstName + " " + value.LastName
          });
        });

        responseHandler(processedFoundedPlayerData);
      });
    }
  };

  // Sets url for getJSON and get founded team player with callback 'responseHandler'
  privates.teamPlayerCompleter = function (requestObj, responseHandler) {
    var url = privates.getAutocompleteUrl({
      searchString: requestObj.term,
    });

    privates.executeCompleter(url, responseHandler);
  };

  // Sets url for getJSON and get founded captain with callback 'responseHandlern
  privates.captainCompleter = function (requestObj, responseHandler) {

    var url = privates.getAutocompleteUrl({
      searchString: requestObj.term,
    });

    privates.executeCompleter(url, responseHandler);
  };

  // Sets team player id and update its input
  privates.onTeamPlayerSelect = function (eventObj, selectedItem) {
    privates.setPlayerId(eventObj.target, selectedItem.item.id);
  };


  // Replaces old captain id with founded new after founded captain was selected
  privates.onCaptainSelect = function (eventObj, selectedItem) {
    var selectedId = selectedItem.item.id;
    privates.setPlayerId(eventObj.target, selectedId);
  };

  // Handles event of 'Create'/'Edit'
  privates.handleTeamCreateEditEvent = function (data) {
    if (data.Success === "False") {
      alert(data.Message);
      return false;
    }
    if (privates.teamUnderEdit) {
      window.history.go(-2);
    } else {
      window.location.href = "../Teams";
    }
  };

  // Adds required field to required inputs
  privates.AddRequiredValue = function (inputElement) {
    if (inputElement.val() === "") {
      $(".required").remove();
      inputElement.css("border-color", "red");
      inputElement.after("<span class='required'>" + requiredFieldError + "</span>");
      inputElement.focus();
    } else {
      $(".required").remove();
      inputElement.css("border-color", "initial");
    }
  };


  // NAMESPACE PUBLIC METHODS

  // Adds new team player table row
  currNs.onAddPlayerToTeamButtonClick = function () {
    if (privates.teamIsValid()) {
      privates.addTeamPlayersRow("", "");
    }

    return false;
  };

  // Deletes player`s row
  currNs.deleteTeamPlayersRow = function (eventData) {
    var currentRow = eventData.target.parentElement.parentElement;
    var currentInput = currentRow.children[0].children[0].children[0];
    var playerId = parseInt(currentInput.name);
    if (playerId > 0) {
      privates.DeletedPlayers.push(playerId);
    }
    currentRow.remove();
    return false;
  };

  // Creates new team in 'Create' mode
  currNs.createTeam = function () {
    var teamData = privates.getJsonForTeamSave();

    if (privates.teamIsValid()) {
      $.post("/Teams/Create", teamData)
        .always(function (data) {
          privates.handleTeamCreateEditEvent(data);
        });
    }

    return false;
  };

  // Saves team in 'Edit' mode
  currNs.saveTeam = function () {
    var teamData = privates.getJsonForTeamSave();

    if (privates.teamIsValid()) {
      $.post("/Teams/Edit", teamData)
        .always(function (data) {
          privates.handleTeamCreateEditEvent(data);
        });
    }

    return false;
  };

  // Adds existed team players in 'Edit' team mode
  (function () {
    var playersData = currNs.teamRoster || [];

    // Team players rows
    if (playersData.length > 0) {
      $.each(playersData, function (ind, val) {
        if (!val.isCaptain) {
          privates.addTeamPlayersRow({
            playerName: val.name,
            playerId: val.id
          });
        }
      });
    }
  })();


  // Adds handlers for captain field
  (function () {
    var captainNameInput = $("#Captain_FullName");
    var temporaryPlayerName = captainNameInput.val().toLowerCase();
    var temporaryPlayerId = privates.getPlayerId($("#Captain_FullName"));

    captainNameInput.bind('blur input keyup', function () {
      privates.AddRequiredValue(captainNameInput);
      privates.checkIfFullNameCorrect(captainNameInput);
      privates.checkIfFullnameContainsSpecSymbols(captainNameInput);
      privates.setsPlayerIdWithFoundedPlayerId(temporaryPlayerName, temporaryPlayerId, captainNameInput);
    });

    captainNameInput.bind("change", function () {
      var $this = $(this);
      var delay = 500;

      clearTimeout($this.data('timer'));
      $this.data('timer', setTimeout(function () {
        $this.removeData('timer');
        privates.checkIfFullNameCorrect(captainNameInput);
        privates.checkIfFullnameContainsSpecSymbols(captainNameInput);
        privates.setsPlayerIdWithFoundedPlayerId(temporaryPlayerName, temporaryPlayerId, captainNameInput);
      }, delay));
    });

    captainNameInput.autocomplete({
      minLength: 2,
      source: privates.captainCompleter,
      select: function (event, ui) {
        privates.onCaptainSelect(event, ui);
        captainNameInput.val(ui.item.value);
        privates.checkIfFullNameCorrect(captainNameInput);
        privates.checkIfFullnameContainsSpecSymbols(captainNameInput);
      },
      delay: 500
    });

  })();

  // Adds player to team button
  $("#addPlayerToTeamButton").bind('click', currNs.onAddPlayerToTeamButtonClick);

  // Creates team button in 'Create' team mode
  $("#createTeamButton").bind("click", currNs.createTeam);

  // Check is team name empty
  $("input:first").bind('blur change input keyup', function () {
    privates.AddRequiredValue($("input:first"));
  });

  // Saves team in 'Edit' team mode
  $("#saveTeamButton").bind("click", currNs.saveTeam);
});
