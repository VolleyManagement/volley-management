$(document).ready(function () {
    'use strict';

    // register namespace
    var currentScope = $("div[vm_scope='team_list']"),
        teamDeleteMessage = $("#DeleteConfirmationMessage").val(),
        handlers = VM.addNamespace("team.listHandlers"),
        privates = {};

    privates.getTeamData = function (teamRowElement) {
        var result = {
            teamId: 0,
            teamName: ""
        };

        if (teamRowElement.attr) {
            result.teamId = parseInt(teamRowElement.attr("vm_teamId"));
            result.teamName = teamRowElement.attr("vm_teamName");
        } else {
            result.teamId = parseInt(teamRowElement.attributes["vm_teamId"].value);
            result.teamName = teamRowElement.attributes["vm_teamName"].value;
        }

        return result;
    };

    handlers.deleteTeam = function (eventObj) {
        var teamRow = eventObj.target.parentElement.parentElement,
            teamData = privates.getTeamData(teamRow),
            teamName = teamData.teamName,
            teamId = teamData.teamId,
            confirmation = confirm(teamDeleteMessage + ' "' + teamName + '" ?');

        if (confirmation) {
            $.ajax({
                url: '../Teams/Delete',
                type: 'POST',
                data: {
                    id: teamId
                },
                dataType: 'json',
                success: function (resultJson) {
                    if (resultJson.OperationSuccessful) {
                        teamRow.remove();
                        $('#teamMessagePlace').html(resultJson.Message);
                    }
                    else {
                        $('[vm_teamId=' + teamId + ']')
                            .append(resultJson.Message + '<br/> ' +
                                'The possible reason - cascade delete is prohibited.');
                    }
                }
            });
        }
    };

    /// BINDINGS
    (function () {
        $(".deleteTeamButton", currentScope).bind('click', handlers.deleteTeam);
    })();
});


