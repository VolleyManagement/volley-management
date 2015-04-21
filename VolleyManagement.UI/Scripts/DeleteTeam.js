function OnDelete(e) {
    var teamId = e.id;
    if (e.HasError)
    {
        $.alert(e.Message);
        return;
    }
    else
    {
        var teamName = e.Message;
        var message = document.getElementById("DeleteConfirmationMessage").getAttribute("value");
        var flag = confirm(message + ' ' + teamName + ' ?');
        if (flag) {
            $.ajax({
                url: 'Teams/Delete',
                type: 'POST',
                data: { id: teamId, confirm: true },
                dataType: 'json',
                success: function (resultJson) {
                    alert(resultJson.Message);
                    if (resultJson.HasDeleted) {
                        $("#" + teamId).parent().parent().remove();
                    } else {
                        window.location.pathname = "Mvc/Teams";
                    }
                }
            });
        }
        return false;
    }
}