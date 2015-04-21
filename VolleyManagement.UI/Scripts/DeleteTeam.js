$(document).ready(function ()
{
    $('.delete').click(OnDelete);
});
function OnDelete(e)
{
    var teamId = e.target.id;
    var message = document.getElementById("DeleteConfirmationMessage").getAttribute("value");
    var teamName = document.getElementById("TeamName " + teamId).innerHTML.trim();
    var confirmation = confirm(message + ' "' + teamName + '" ?');
    if (confirmation)
    {
        $.ajax({
            url: 'Teams/Delete',
            type: 'POST',
            data: { id: teamId },
            dataType: 'json',
            success: function (resultJson)
            {
                alert(resultJson.Message);
                if (resultJson.HasDeleted)
                {
                    $("#" + teamId).parent().parent().remove();
                } else
                {
                    window.location.pathname = "Mvc/Teams";
                }
            }
        });
    }
    return false;
}