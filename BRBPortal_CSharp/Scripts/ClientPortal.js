
//Show Popup Message with OK Button
function ShowPopupOK(message, title) {

    $(function () {
        $("#dialog2").html(message);
        $("#dialog2").dialog({
            title: title,
            buttons: {
                Close: function () {
                    $(this).dialog('close');
                }
            },
            width: 400,
            modal: true
        });
    });

};

//Show Popup Message with Yes No buttons that trigger VB events
function ShowPopupYN(message, title) {
    $(function () {
        $("#dialog2").html(message);
        $("#dialog2").dialog({
            closeOnEscape: false,   // prevents user closing with the Esc key
            title: title,
            buttons: [
            {
                id: "Yes",
                text: "Yes",
                click: function () {
                    $("[id*=btnDialogResponseYes]").attr("rel", "delete");
                    $("[id*=btnDialogResponseYes]").click();
                }
            },
            {
                id: "No",
                text: "No",
                click: function () {
                    $("[id*=btnDialogResponseNo]").attr("rel", "delete");
                    $("[id*=btnDialogResponseNo]").click();
                }
            }
            ],
            open: function (event, ui) {
                $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
            },  // suppresses close button on title bar - user must enter Yes or No
            width: 400,
            modal: true
        });
    });
    $("[id*=btnDialogResponse]").click(function () {
        if ($(this).attr("rel") !== "delete") {
            $('#dialog').dialog('open');
            return false;
        } else {
            __doPostBack(this.name, '');
        }
    });
};