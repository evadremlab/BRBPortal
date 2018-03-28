function showDialogOK(message, title) {
    $(document).ready(function () {
        $('#OkModal .modal-title').html(title);
        $('#OkModal .modal-body').html(message);
        $('#OkModal').modal('show');
    });
}

function submitPaymentForm(billingCode, cartID, paymentAmount) {
    $('#btnSubmit').click(function (evt) { // hijack the asp.net form
        evt.preventDefault();

        $('#aspForm')
            .prop('action', 'https://staging.officialpayments.com/pc_entry_cobrand.jsp')
            .find('input[name="cde-CartID-17"]').val(cartID)
            .find('input[cde-BillingCode-1]').val(billingCode)
            .find('input[paymentAmount]').val(paymentAmount)
            .find('.aspNetHidden').remove() // delete ASP.NET generated fields
            .submit();
    });
}
