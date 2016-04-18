$(document).ready(function () {
    $('a.deleteGame').click(OnDeleteClick);
});
function OnDeleteClick(e) {
    var gameId = e.target.id;
    var tournamentId = document.getElementById("tournamentId").getAttribute("value");
    var message = document.getElementById("DeleteConfirmationMessage").getAttribute("value");
  
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
                } else {
                    window.location.pathname = "/Tournaments/ShowSchedule/" + tournamentId;
                }
            }
        });
    }
    return false;
}