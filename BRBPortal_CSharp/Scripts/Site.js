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
