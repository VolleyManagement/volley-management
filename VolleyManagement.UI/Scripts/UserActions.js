$(document).ready(function () {
    $('a.delete').click(OnDeleteClick);
});
function OnDeleteClick(e) {
    var playerId = e.target.id;
    var playerName = document.getElementById(playerId + "playerName").textContent;
    var message = document.getElementById("DeleteConfirmationMessage").getAttribute("value");
    var flag = confirm(message + ' ' + playerName + ' ?');
    if (flag) {
        $.ajax({
            url: 'Players/Delete',
            type: 'POST',
            data: { id: playerId },
            dataType: 'json',
            success: function (result) { alert(result); $("#" + playerId).parent().parent().remove(); }
        });
    }
    return false;
}