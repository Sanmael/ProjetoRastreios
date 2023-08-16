$(document).ready(function () {

    $("#submitCreateSubPerson").on("click", function (e) {
        $("#submitCreateSubPerson").prop("desabled", true);

        let email = $('#emailInputRegister').val();        

        let emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailPattern.test(email)) {
            alert('Email inválido!');
            return;
        }

        let formData = $("#registerPersonForm").serialize();

        $.ajax({
            type: "POST",
            url: "https://localhost:7186/Person/PersonCreateAction",
            data: formData,
            dataType: "json",
            success: function (data) {
                $("#submitCreateSubPerson").prop("desabled", false);

                if (data.success) {
                    window.location.href = data.redirectUrl;
                } else {
                    $("#registerMessage").text(data.message);
                }
            },
            error: function () {
                alert("Erro ao processar a solicitação de registro.");
                $("#submitCreateSubPerson").prop("desabled", false);
            }
        });
    });
});