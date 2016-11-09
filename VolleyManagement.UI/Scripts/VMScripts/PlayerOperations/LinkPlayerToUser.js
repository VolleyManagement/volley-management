;$(document).ready(function () {
    $("#linkUser").click(function () {

        var playerId = $('#Model_Id').val();
        var playerFirstName = $('#playerFirstName').html();
        var playerLastName = $('#playerLastName').html();

        $.ajax({
            url: "/Players/LinkWithUser",
            datatype: 'json',
            data: { id: playerId },
            success: function (data) {
                $('#ajaxResultMessage')
                    .html('<br />' + data +
                    '<mark><strong><em> ' + playerFirstName +
                    ' ' + playerLastName + '</em></strong></mark>');
            },
            error: function () {
                $('#ajaxResultMessage').html('ERROR');
            }
        });
    });
});
