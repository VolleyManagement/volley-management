$(document).ready(function () {
    $('a.delete').click(OnDeleteClick);
    $('a.showFinishedTournamenst').click(OnShowFinishedTournamentsClick);
    $('#finished_table').hide();
    $('#FinishedHeader').hide();
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
    $('#finished_table').show();
    $(".dynamicData").remove();

    $.ajax({
        url: 'Tournaments/GetFinished',
        type: 'GET',
        dataType: 'json',
        success: function (resultJson) {
            $('a.showFinishedTournamenst').hide();
            $('#FinishedHeader').show();
            $.each(resultJson, function (i, item) {
                var $tr = $('<tr class="dynamicData">').append(
                    $('<td width="400">').append($('<a/>').attr('href','mvc/Tournaments/Details/' + item.Id).text(item.Name)),
                    $('<td width="100">').append($('<label>').text(DisplaySeason(item.Season)))
                ).appendTo('#finished_table');
            });
        }
    });

    return false;
}

function DisplaySeason(season) {
    return season + "/" + (season + 1);
}

$(".deleteResult_btn").click(function (e) {
    
    var confirmation = confirm(DELETE_CONFIRMATION_MESSAGE);

    if (!confirmation) {
        return;
    }
    
    $.ajax({
        type: "POST",
        url: "/GameResults/Delete",
        data: {
            id: $(this).data("id")
        },
        success: function (result) {
                location.reload();
        },
        error: function () {
            alert("Error");
        }
    });
});