function checkFormData() {
    var checked = true;

    for (var i = 0; i <= 12; i++) {

        //will check the classname if it is checked
        if ($(`input[name=group${i}[]]`).is(':checked'))
            checked = true;
        else
            checked = false;

        // will break the loop if the returned check is false in the checked options
        if (checked == false)
            break;

    };

    if (checked == false)
        alert("form not submitted.");

    return checked;
}