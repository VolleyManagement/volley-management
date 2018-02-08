function SwapRounds(currentRound) {
    var e = document.getElementById("list_rounds1");
    e.options[currentRound - 1].selected = true;

    $("#dialog-swap").dialog(
        {
            modal: true,
            buttons: {
                "Swap": function () {
                    var context = this;

                    var listOfRound1 = document.getElementById("list_rounds1");
                    var listOfRound2 = document.getElementById("list_rounds2");
                    var round1 = listOfRound1.options[listOfRound1.selectedIndex].value;
                    var round2 = listOfRound2.options[listOfRound2.selectedIndex].value;

                    var tournamentId = document.getElementById("tournamentId").getAttribute("value");

                    if (round1 != round2) {
                        $.ajax({
                            url: '/Tournaments/SwapRounds',
                            type: 'POST',
                            data: {
                                tournamentId: tournamentId,
                                firstRoundNumber: round1,
                                secondRoundNumber: round2
                            },
                            error: function (request, status, error) {
                                alert(request.responseText);
                            },
                            success: function () {
                                $(context).dialog("close");
                                window.location.href = "/Tournaments/ShowSchedule?tournamentId=" + tournamentId + "#swap_round_" + round1;
                                window.location.reload();
                            }
                        })
                    }
                    else
                    {
                        document.getElementById("error").innerHTML = "Rounds can not be similar.";
                    }
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            }
        });
    return false;
}