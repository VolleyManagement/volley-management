$(document).ready(function () {
    $('a.delete').click(OnDeleteClick);
    $('a.showFinishedTournamenst').click(OnShowFinishedTournamentsClick);
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
            success: function (resultJson) {
                alert(resultJson.Message);
                if (resultJson.HasDeleted) {
                    $("#" + playerId).parent().parent().remove();
                } else {
                    window.location.pathname = "Mvc/Players";
                }
            }
        });
    }
    return false;
}

function OnShowFinishedTournamentsClick(e) {
    var response = [{
        "id": 1, "name": "First name", "description": "first description", "season": 2012, "scheme": "1", "regulationsLink": ""
    }, {
        "id": 2, "name": "Second name", "description": "second description", "season": 2013, "scheme": "1", "regulationsLink": "Volley.dp.ua"
    }, {
        "id": 3, "name": "Third name", "description": "Third description", "season": 2014, "scheme": "2", "regulationsLink": "123123"
    }
    ]

    response = $.parseJSON(response);

    $.each(response, function (i, item) {
        var $tr = $('<tr>').append(
            $('<td>').text(item.name),
            $('<td>').text(item.season)
        ); //.appendTo('#records_table');
        console.log($tr.wrap('<p>').html());
    });
}