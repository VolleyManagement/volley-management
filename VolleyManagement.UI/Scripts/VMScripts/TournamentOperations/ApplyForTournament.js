$(document).ready(function () {
    $('#agree').click(function () {
        var apply = document.getElementById('apply');
        $(this).is(':checked') ? apply.disabled = false : apply.disabled = true;
    });
});

$(document).ready(function () {
    $("#apply").click(function () {
        var tournamentId = $('#tournamentId').val();
        var teamId = $("#TeamId").val();
        $.ajax({
            url: "/Tournaments/ApplyForTournament",
            datatype: 'json',
            type: 'POST',
            data: { tournamentId: tournamentId, teamId: teamId},
            success: function (data) {
                $('#ajaxResultMessage')
                    .html('<br />' + data +
                    'Your request');
            },
            error: function () {
                $('#ajaxResultMessage').html('ERROR');
            }
        });
    });
});
