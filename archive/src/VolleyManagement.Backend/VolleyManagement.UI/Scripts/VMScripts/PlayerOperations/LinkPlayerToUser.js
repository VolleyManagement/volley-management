; $(document).ready(function () {
  ; (function () {

    var messagePlace = $('#ajaxResultMessage');
    var linkButton = $("#linkUser");

    linkButton.click(function () {

      var currentPlayerId = $('#Model_Id').val();
      var linkMessage = $('#linkMessage').html();

      $.ajax({
        url: "/Players/LinkWithUser",
        datatype: 'json',
        data: { playerId: currentPlayerId },
        success: function (message) {
          messagePlace.html(message + linkMessage);
          linkButton.prop('disabled', true);
        },
        error: function () {
          messagePlace.html('ERROR');
        }
      });
    });

  })();
});



