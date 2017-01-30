$(document).ready(function () {
    $('#isGeslotenCheckbox').change(function () {
        var attr = $('#einddatum').attr('disabled');
        console.log(attr);
        if (typeof attr !== typeof undefined && attr !== false) {
            $('#einddatum').removeProp('disabled');
            $('#eindtijd').removeProp('disabled');
        }
        else {
            $('#einddatum').prop('disabled', true);
            $('#eindtijd').prop('disabled', true);
        }

    });
});
