/**
 * HACK: $(document).ready only fires once, but that's ok because this function is only called on postback to show errors
 */
function showOkModalOnPostback(message, title) {
    $(document).ready(function () {
        $('#OkModal .modal-title').text(title);
        $('#OkModal .modal-body').text(message);
        $('#OkModal').modal('show');
    });
}
