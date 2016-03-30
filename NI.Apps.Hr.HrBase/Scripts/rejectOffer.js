var offerID = $("#Offer_ID").val();

$('#dialog-confirm-reject').dialog({
    title: "Want to reject offer?",
    autoOpen: false,
    resizable: false,
    height: 340,
    width: 340,
    modal: true,
    buttons: {
        "Yes": function () {
            //var url='@Url.Action("Reject", "Offer",new { offerID="_offerID_"})';
            var url = '@Url.Action("Reject", "Offer")';
            alert(url);
            window.location.href = url+"/"+offerID;
        },
        "Cancel": function () {
            $(this).dialog("close");
        }
    }
});

$("#RejectBtn").click(function () {
    $('#dialog-confirm-reject').dialog("open");
});