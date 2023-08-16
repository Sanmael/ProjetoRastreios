function OpenModalGetTracking(id) {
    $.getJSON("https://localhost:7186/Tracking/GetTrackingBySubPerson?subPersonId=" + id, function (data) {
        if (data.success) {
            var trackingButtonsRow = $("#trackingButtonsRow");
            trackingButtonsRow.empty();

            var currentRow = $("<div>").addClass("row");
            trackingButtonsRow.append(currentRow);

            $.each(data.data, function (index, item) {
                var trackingBox = $("<div>").addClass("col-md-6 mb-4");
                var codeButton = $("<button>").addClass("btn btn-primary btn-block").text(item.code);


                if (item.status == 0 || item.status == 1) {
                    codeButton.click(function () {
                        OpenTrackingEventsModal(item.trackingCodeId);
                    });
                }
                else
                {
                    codeButton.click(function () {
                        $.ajax({
                            type: "POST",
                            url: "https://localhost:7186/Tracking/DeleteTrackingCode",
                            data: { trackingCodeId: item.trackingCodeId },
                            dataType: "json",
                            success: function (data) {
                                if (data.success) {
                                    alert("Codigo Invalido!");
                                    $("#trackingModal").modal("hide");
                                } 
                            }                          
                        });
                    });
                }
                
                var lastUpdate = $("<p>").addClass("text-black").text("Ultima consulta: " + formatDateAndTime(item.updateDate));

                trackingBox.append(codeButton, lastUpdate);
                currentRow.append(trackingBox);

                if ((index + 1) % 2 === 0) {
                    trackingButtonsRow.append("<hr>");
                    currentRow = $("<div>").addClass("row");
                    trackingButtonsRow.append(currentRow);
                }

                if (index % 2 === 0 && index > 0) {
                    trackingButtonsRow.append("<div class='w-100 d-md-none'></div>"); 
                }
            });

            $("#trackingModal").modal("show");
        } else {
            
        }
    });
}







function OpenTrackingEventsModal(trackingCodeId) {
    $.getJSON("https://localhost:7186/Tracking/GetTrackingEvents?trackingCodeId=" + trackingCodeId, function (data) {
        if (data.success) {
            $("#trackingModal").modal("hide");

            var modalBody = $("#trackingEventsModalBody");
            modalBody.empty();

            $.each(data.data, function (index, item) {
                if (index > 0) {
                    modalBody.append($("<hr>").addClass("event-separator"));
                }

                var eventCard = $("<div>").addClass("card text-white" + returnBg(item.status) + "mb-3 bg-subtle");
                var cardHeader = $("<div>").addClass("card-header").text("Data: " + formatDateAndTime(item.creationDate));
                var cardBody = $("<div>").addClass("card-body");
                var eventStatus = $("<h5>").addClass("card-title").text("Status: " + item.status);
                var eventLocal = $("<p>").addClass("card-text").text("Local: " + item.local);

                cardBody.append(eventStatus, eventLocal);
                eventCard.append(cardHeader, cardBody);
                modalBody.append(eventCard);
            });

            $("#trackingEventsModal").modal("show");
        }
    });
}

function formatDateAndTime(dateTime) {
    var date = new Date(dateTime);
    var formattedDate = date.toLocaleDateString();
    var hours = date.getHours();
    var minutes = date.getMinutes();
    return formattedDate + " " + (hours < 10 ? "0" : "") + hours + ":" + (minutes < 10 ? "0" : "") + minutes;
}


function returnBg(status) {
    if (status.includes("Encaminhado para fiscaliz")) {
        return "card text-white bg-danger mb-3 bg-subtle";
    }

    if (status.includes("Objeto encaminhado")) {
        return "card text-white bg-secondary mb-3 bg-subtle";
    }

    if (status.includes("entregue ao destinat") || status.includes("aduaneira finalizada")) {
        return "card text-white bg-success mb-3 bg-subtle";
    }

    if (status.includes("Tentativa de entrega n")) {
        return "card text-white bg-warning mb-3 bg-subtle";
    }

    return "card text-white bg-info mb-3 bg-subtle";
}