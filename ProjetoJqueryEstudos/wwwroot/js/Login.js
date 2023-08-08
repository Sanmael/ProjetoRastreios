
$(document).ready(function () {
    $("#loginButton").on("click", function (e) {
        $("#loginButton").prop("disabled", true);

        var formData = $("#loginForm").serialize();

        $.ajax({
            type: "POST",
            url: "User/Login",
            data: formData,
            dataType: "json",
            success: function (data) {
                if (data.success) {
                    window.location.href = data.redirectUrl;
                } else {
                    $("#loginMessage").text(data.message);
                }

                $("#loginButton").prop("disabled", false);
            },
            error: function () {
                alert("Erro ao processar a solicitação de login.");
                $("#loginButton").prop("disabled", false);
            }
        });
    });
});
