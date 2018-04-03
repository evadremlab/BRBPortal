
function showOKModal(message, title, delay) {
    $(document).ready(function () {
        setTimeout(function () {
            $('#OkModal .modal-title').html(title);
            $('#OkModal .modal-body').html(message);
            $('#OkModal').modal('show');
        }, delay || 100);
    });
}

function showErrorModal(message, title, delay) {
    $(document).ready(function () {
        setTimeout(function () {
            $('#ErrorModal .modal-title').html(title);
            $('#ErrorModal .modal-body').html(message);
            $('#ErrorModal').modal('show');
        }, delay || 100);
    });
}

function submitPaymentForm(cartID) {
    $(document).ready(function () {
        $('#aspForm') // hijack the asp.net form
            .prop('action', 'https://staging.officialpayments.com/pc_entry_cobrand.jsp')
            .find('input[name="cde-Cart-17"]').val(cartID)
            .find('.aspNetHidden').remove(); // delete ASP.NET generated fields

        setTimeout(function () {
            $('#aspForm').submit();
        }, 0);
    });
}
