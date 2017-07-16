﻿$(document).ready(function () {
    $('#agree').click(function () {
        var apply = document.getElementById('apply');
        $(this).is(':checked') ? apply.disabled = false : apply.disabled = true;
    });
});

$(document).ready(function () {
    $("#apply").click(function () {
        var tournamentApply = {
            GroupId: $("#GroupId").val(),
        };

        $.ajax({
            url: "/Tournaments/ApplyForTournament",
            datatype: 'json',
            type: 'POST',
            data: { tournamentApply: tournamentApply, tournamentId: $("#tournamentId").val(), teamId: $("#TeamId").val()}
        })
                .done(function (data) {
                    $('#ajaxResultMessage')
                        .html('<br />' + data);
                })
                .fail(function (data) {
                    $('#ajaxResultMessage').html(data);
                });
    });
});

$(document).ready(function () {
    $('#apply-cancel').click(function () {
        window.history.back();
    });
});