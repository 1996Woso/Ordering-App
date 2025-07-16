function allowDigits(event) {
    let inputChar = String.fromCharCode(event.which);
    let digits = /[0-9]/;
    if (!digits.test(inputChar)) {
        event.preventDefault();
    }
}
function pasteDigits(event, inputId, maxLength) {
    event.preventDefault();
    let pastedText = (event.originalEvent || event).clipboardData.getData('text/plain');
    let only_digits = pastedText.replace(/[^0-9]/g, '');
    if (only_digits.length > maxLength) {
        only_digits = only_digits.slice(0, maxLength);
    }
    document.getElementById(inputId).value = only_digits;
    return false;
}