$(document).ready(function () {
    $('#isGeslotenCheckbox').change(function () {
        var attr = $('#isGeslotenCheckbox').attr('disabled');
        console.log($('#einddatum').val().length);
        console.log($('#eindtijd').val().length);
        if (($('#einddatum').val().length) == 0 && $('#eindtijd').val().length == 0)
        {
            $('einddatum').show("input-validation-error");
            console.log("Nope");
        }

    });
});
