$(document).ready(function () {
  $('a.deleteGame').click(OnDeleteClick);
});
function OnDeleteClick(e) {
  var gameId = e.target.id;
  var tournamentId = document.getElementById("tournamentId").getAttribute("value");
  var message = document.getElementById("DeleteConfirmationMessage").getAttribute("value");
  var numberOfRound = document.getElementById("numberOfRound_" + e.target.id).getAttribute("value");

  if (confirm(message)) {
    $.ajax({
      url: '/GameResults/Delete',
      type: 'POST',
      data: { id: gameId },
      dataType: 'json',
      success: function (resultJson) {
        alert(resultJson.Message);
        if (resultJson.HasDeleted) {
          $("#" + gameId).remove();
          if ($('#round_' + numberOfRound).has("div").length == 0) {
            document.getElementById("round_" + numberOfRound).innerHTML = "No games scheduled yet"
          }
        } else {
          window.location.pathname = "/Tournaments/ShowSchedule/" + tournamentId;
        }
      }
    });
  }
  return false;
}