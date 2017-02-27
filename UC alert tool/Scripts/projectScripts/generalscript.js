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
    $('#submitbutton').click(checkIfstartIsBeforeEnddate);

    function checkIfstartIsBeforeEnddate() {
        $('.errortext').remove();
        var begindate = new Date($('#begindatum').val());
        var beginTime = $('#begintijd').val();
        var beginTimeSplit = beginTime.split(':');
        var enddate = new Date($('#einddatum').val());
        var endTime = $('#eindtijd').val();
        var endTimeSplit = endTime.split(':');
        if (enddate != "Invalid Date") {
            if (begindate < enddate)
            {
                console.log("date correct")
                if (endTime == "") {
                    $('#eindtijd').after("<p class='errortext'>Vul een einddatum in</p>");
                    return false
                }
                else {
                    return true;
                }
            }
            else if (begindate > enddate) {
                $('#einddatum').after("<p class='errortext'>Einddatum mag niet eerder zijn dan de begindatum</p>");
                return false;
            }
            else // dates are the same
            {
                if (beginTime == "" || endTime == "") {
                    $('#eindtijd').after("<p class='errortext'>Vul een einddatum in</p>");
                    return false;

                }
                else if (beginTimeSplit[0] > endTimeSplit[0]) 
                {
                    $('#eindtijd').after("<p class='errortext'>Vul een correcte einddatum in</p>");
                    return false;
                }
                else if (beginTimeSplit[0] == endTimeSplit[0]) {
                    if (beginTimeSplit[1] >= endTimeSplit[1]) {
                        $('#eindtijd').after("<p class='errortext'>De eindtijd kan niet minder of hetelfde zijn als de begintijd</p>");
                        return false;
                    }
                    else {
                        return true;
                    }
                }
                else {
                    return true;
                }
            }    
        }

    };
});
