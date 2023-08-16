$(document).ready(function () {

    $("#submitRegister").on("click", function (e) {
        $("#submitRegister").prop("desabled", true);

        let email = $('#emailInput').val();
        let taxNumber = $('#taxNumberInput').val();

        const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailPattern.test(email)) {
            alert('Email inválido!');
            return;
        }

        if (!validateCPF(taxNumber)) {
            alert('CPF inválido!');
            return;
        }

        var formData = $("#registerForm").serialize();

        $.ajax({
            type: "POST",
            url: "https://localhost:7186/User/Register",
            data: formData,
            dataType: "json",
            success: function (data) {
                $("#submitRegister").prop("desabled", false);

                if (data.success) {
                    window.location.href = data.redirectUrl;
                } else {
                    $("#registerMessage").text(data.message);
                }
            },
            error: function () {
                alert("Erro ao processar a solicitação de registro.");
                $("#submitRegister").prop("desabled", false);
            }
        });
    });
});

$(document).ready(function () {
    $("#postalCodeId").blur(function () {

        var cep = $("#postalCodeId").val().replace(/\D/g, '');

        if (cep != "") {

            var validateCep = /^[0-9]{8}$/;

            if (validateCep.test(cep)) {

                $("#publicPlaceInput").val("");
                $("#neighborhoodInput").val("");
                $("#cityInput").val("");
                $("#stateInput").val("");
                $("#validateFormId").val("false");

                $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?", function (data) {
                    if (!("erro" in data)) {
                        $("#publicPlaceInput").val(data.logradouro);
                        $("#neighborhoodInput").val(data.bairro);
                        $("#cityInput").val(data.localidade);
                        $("#stateInput").val(data.uf);
                        $("#validateFormId").val("true");
                    }
                    else {
                        resetForm();
                        alert("CEP não encontrado.");
                    }
                });
            }
            else {
                resetForm();
                alert("Formato de CEP inválido.");
            }
        }
        else {
            resetForm();
        }
    });
});



function resetForm() {
    $("#publicPlaceInput").val("");
    $("#neighborhoodInput").val("");
    $("#cityInput").val("");
    $("#stateInput").val("");
    $("#validateFormId").val("false");
}

function validateCPF(cpf) {
    const cpfValue = document.getElementById('taxNumberInput').value;
    const cpfPattern = /^\d{3}\.\d{3}\.\d{3}-\d{2}$/;

    if (!cpfPattern.test(cpfValue)) {
        return false;
    }

    const cpfDigits = cpfValue.replace(/\D/g, '');

    if (cpfDigits === cpfDigits[0].repeat(11)) {
        return false;
    }

    let sum = 0;
    for (let i = 0; i < 9; i++) {
        sum += parseInt(cpfDigits[i]) * (10 - i);
    }

    let remainder = (sum * 10) % 11;
    if (remainder === 10 || remainder === 11) {
        remainder = 0;
    }

    if (remainder !== parseInt(cpfDigits[9])) {
        return false;
    }

    sum = 0;
    for (let i = 0; i < 10; i++) {
        sum += parseInt(cpfDigits[i]) * (11 - i);
    }

    remainder = (sum * 10) % 11;
    if (remainder === 10 || remainder === 11) {
        remainder = 0;
    }

    if (remainder !== parseInt(cpfDigits[10])) {
        return false;
    }

    return true;
}