$(document).ready(function () {
    $("#linkUser").click(function () {
        

        $.ajax({
            url: "/Players/LinkWithUser",
            datatype: 'json',
            data: { id: '@Model.Model.Id' },
            success: function (data) {
                $('#ajaxResultMessage')
                    .html(
                    '<br />' + data +
                    '<mark><strong><em> @Model.Model.FirstName @Model.Model.LastName</em></strong></mark>'
                    );
            },
            error: function () {
                $('#ajaxResultMessage').html('ERROR');
            }
        });
    });
});