; $(document).ready(function () {

    ;(function () {
        $("#linkUser").click(function () {

            var playerId = $('#Model_Id').val();
            var linkMessage = $('#linkMessage').html();
            $.ajax({
                url: "/Players/LinkWithUser",
                datatype: 'json',
                data: { id: playerId },
                success: function (data) {
                    $('#ajaxResultMessage').html(data + linkMessage);
                },
                error: function () {
                    $('#ajaxResultMessage').html('ERROR');
                }
            });
        });
    })();

});



