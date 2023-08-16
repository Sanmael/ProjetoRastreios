
let buttonModal = $('#PersonCreateModalButton');

let reallyRemovePerson = $('#reallyRemovePerson');

let buttonModalClose = $('#closeModalCreate');

let cancelRemovePerson = $('#cancelRemovePerson');


cancelRemovePerson.on("click", function () {
    $("#personRemoveModal").modal("hide");
})

buttonModalClose.on("click", function () {
    $("#PersonCreateModal").modal("hide");
})

reallyRemovePerson.on("click", function () {
    $("#personRemoveModal").modal("hide");
    var personId = $('#personIdRemove').val();

    $.ajax({
        url: "PersonDeleteAction",
        data: {
            personId: personId
        },
        method: "POST",
        success: function (result) {
            if (result.success) {
                alert(result.message)
                window.location.reload();
            }
            else
                alert(result.message)
        }
    });
})


function OpenModalRemove(id) {
    $("#personRemoveModal").modal("show");
    $('#personIdRemove').val(id);
}

function OpenModalAddressDetail(id) {
    $.getJSON("GetAddressById/" + id, function (data) {

        if (data.success) {
            $("#addressDetailModal").modal("show");

            $("#publicPlaceInputDetail").val(data.data.publicPlace);
            $("#postalCodeDetail").val(data.data.postalCode);
            $("#neighborhoodInputDetail").val(data.data.neighborhood);
            $("#cityInputDetail").val(data.data.city);
            $("#stateInputDetail").val(data.data.state);
        }
        else
            alert(data.message)
    });
}

$(document).on("click", "#modalDeCadastroRastreio .btn-primary", function () {
    var codigoDeRastreio = $("#codigoDeRastreio").val();
    var subPersonId = $("#subPersonIdInput").val();

    $.ajax({
        url: "https://localhost:7186/Tracking/NewTrackingCode",
        data: {
            code: codigoDeRastreio, subPersonId: subPersonId
        },
        method: "POST",
        success: function (result) {
            if (result.success) {
                alert("Codigo de rastreio adicionado com sucesso!");
                window.location.reload();
            }
            else if (result.message != null)
                alert(result.message)
        }
    });
});

function AbrirModalComId(id)
{
    $("#subPersonIdInput").val(id); 
    $("#modalDeCadastroRastreio").modal("show");
}