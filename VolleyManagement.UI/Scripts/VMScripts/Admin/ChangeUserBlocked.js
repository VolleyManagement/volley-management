function changeUserBlocked(userId, isBlock) {
    $.ajax({
        url: "/Admin/Users/ChangeUserBlocked",
        data: {
            id: userId,
            toBlock: isBlock,
            backTo: returnUrl
        },
        success: function (result) {
            $(".text-danger").remove();

            if (!result.success) {
                showError(result.message);
            } else {
                location.reload();
            }
        }
    });
}

function showError(errorMessage) {
    $(".panel.panel-primary").before("<span class = 'text-danger'> Error: " + errorMessage + "</span>");
}