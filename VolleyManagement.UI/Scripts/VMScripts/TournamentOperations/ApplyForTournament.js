$(document).ready(function () {
    $('#agree').click(function () {
        var apply = document.getElementById('apply');
        $(this).is(':checked') ? apply.disabled = false : apply.disabled = true;
    });
});

$(document).ready(function () {
    $("#apply").click(function () {
        var teamId = $('#TeamId').val();
        var divisionId = $('select[name="divisions"] :selected').val();
        var groupId = $('select[name="groups"] :selected').val();

        if (divisionId === "0" || groupId === "0") {
            alert("Not all parameters was selected!");
        } else {
            $.ajax({
                url: "/Tournaments/ApplyForTournament",
                datatype: 'json',
                type: 'POST',
                data: { GroupId: groupId, TeamId: teamId }
            })
                .done(function (data) {
                    $('#ajaxResultMessage')
                        .html('<br />' + data);
                })
                .fail(function (data) {
                    $('#ajaxResultMessage').html(data);
                });
        }
    });
});

$(document).ready(function () {
    $('#apply-cancel').click(function () {
        window.history.back();
    });
});


$(document).ready(function () {
    'use strict';

    var privates = {};

    privates.tornamentTeamsTable = $("#tournamentRoster");

    privates.getAllDivisionsOptions = function (callback) {
        var id = $("[id='tournamentId']").val();
        $.getJSON("/Tournaments/GetAllAvailableDivisions", { tournamentId: id }, callback);
    };

    privates.getAllGroupsOptions = function (callback) {
        var divId = $('select[name="divisions"] :selected').val();
        $.getJSON("/Tournaments/GetAllAvailableGroups", { divisionId: divId }, callback);
    };

    privates.getTornamentDivisionRowMarkup = function (responseOptions, isDisabled) {
        var disabled = "";

        if (isDisabled) {
            disabled = "disabled";
        }

        var result = "<td id ='division'><select " + disabled + " name = 'divisions'>" + responseOptions + "</select></td>"
            + "<td id = 'group'></td>";
        return result;
    };

    privates.renderNewTournamentDivisionsRow = function (responseOptions, isDisabled) {
        $("#team", privates.tornamentTeamsTable).parent().append(privates.getTornamentDivisionRowMarkup(responseOptions, isDisabled));
    };

    privates.getTornamentGroupRowMarkup = function (responseOptions, isDisabled) {
        var disabled = "";

        if (isDisabled) {
            disabled = "disabled";
        }

        var result = "<td id='group'><select " + disabled + " name='groups'>" + responseOptions + "</select></td>";
        return result;
    };

    privates.renderNewTournamentGroupsRow = function (responseOptions, isDisabled) {
        $("td[id = 'division']:last", privates.tornamentTeamsTable).after(privates.getTornamentGroupRowMarkup(responseOptions, isDisabled));
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
                responseOptions += "<option value = '0'>No division is selected</option>";
            }

            $.each(options, function (key, value) {
                responseOptions += "<option value='" + value.Id + "'>" + value.Name + "</option>";
            });

            privates.renderNewTournamentDivisionsRow(responseOptions, isDisabled);

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
            $("td[id = 'division']:last", privates.tornamentTeamsTable).parent().parent().after("<td id = 'group'></td>");
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
                responseOptions += "<option value = '0'>No group is selected</option>";
            }

            $.each(options, function (key, value) {
                responseOptions += "<option value='" + value.Id + "'>" + value.Name + "</option>";
            });

            privates.DeleteGroupsMarkup();
            privates.renderNewTournamentGroupsRow(responseOptions, isDisabled);
        });

    };

    privates.DeleteGroupsMarkup = function () {
        var result = $('#group').remove();
    };

    privates.addTournamentDivisionsRow();
});