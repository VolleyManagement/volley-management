;$(document).ready(function () {
    $("#linkUser").click(function () {

        var playerId = $('#Model_Id').val();
        var linkMessage = $('#linkMessage').html();  
        $.ajax({
            url: "/Players/LinkWithUser",
            datatype: 'json',
            data: { id: playerId },
            success: function () {
                $('#ajaxResultMessage')
                    .html(linkMessage);
            },
            error: function () {
                $('#ajaxResultMessage').html('ERROR');
            }
        });
    });
});
