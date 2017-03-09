$(document).ready(function () {
    $('#submitbutton').click(checkIfstartIsBeforeEnddate);
    $('#submitbutton').click(closeStoringIfAllDatesAreValid);
    $('#smsbericht').keyup(setRemainingCount);
    $('#emailTitle').keyup(function () {
        var sourceText = $("#emailTitle").val().replace(/\r?\n/g, '<br/>');
        $("#previewTitle").val(sourceText);

    });
    $('#emailbody').keyup(function () {
        var sourceText = $("#emailbody").val().replace(/\r?\n/g, '<br/>');
        $("#previewUserInput").html(sourceText);
    });

    function closeStoringIfAllDatesAreValid() {
        if ($('#isGeslotenCheckbox').is(":checked"))
        {
            var begindate = new Date($('#begindatum').val());
            var beginTime = $('#begintijd').val();
            var enddate = new Date($('#einddatum').val());
            var endTime = $('#eindtijd').val();
            if (begindate !== "Invalid Date" && beginTime !== "" && enddate !== "Invalid Date" && endTime !== "")
            {
                return true
            }
            else
            {
                $('#isGeslotenCheckbox').after("<p class='errortext'>Vul alle waarden in om de storing te sluiten</p>");
                return false;
            }
        }
        else
        {
            return true;
        }
    }
    function checkIfstartIsBeforeEnddate() {
        $('.errortext').remove();
        var begindate = new Date($('#begindatum').val());
        var beginTime = $('#begintijd').val();
        var beginTimeSplit = beginTime.split(':');
        var enddate = new Date($('#einddatum').val());
        var endTime = $('#eindtijd').val();
        var endTimeSplit = endTime.split(':');
        if (Date.parse(enddate) && enddate == "") // if only date is entered
        {
            $('#eindtijd').after("<p class='errortext'>Vul een eindtijd in</p>");
            return false
        }
        else if (!Data.parse(enddate) && endTime != "")// if only time is entered
        {
            $('#eindtijd').after("<p class='errortext'>Vul een einddatum in</p>");
            return false
        }
        if (Date.parse(begindate)) // check if begintime is earlier than the enddate
        {
            if (begindate < enddate)
            {
                console.log("date correct")
                if (endTime === "")
                {
                    $('#eindtijd').after("<p class='errortext'>Vul een einddatum in</p>");
                    return false
                }
                else
                {
                    return true;
                }
            }
            else if (begindate > enddate)
            {
                $('#einddatum').after("<p class='errortext'>Einddatum mag niet eerder zijn dan de begindatum</p>");
                return false;
            }
            else // dates are the same
            {
                if (beginTime === "" || endTime === "")
                {
                    $('#eindtijd').after("<p class='errortext'>Vul een einddatum in</p>");
                    return false;

                }
                else if (beginTimeSplit[0] > endTimeSplit[0]) 
                {
                    $('#eindtijd').after("<p class='errortext'>Vul een correcte einddatum in</p>");
                    return false;
                }
                else if (beginTimeSplit[0] === endTimeSplit[0])
                {
                    if (beginTimeSplit[1] >= endTimeSplit[1])
                    {
                        $('#eindtijd').after("<p class='errortext'>De eindtijd kan niet minder of hetelfde zijn als de begintijd</p>");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }    
        }
        else {
            $('#eindtijd').after("<p class='errortext'>Vul een einddatum in</p>");
            return false
        }

    };

    function setRemainingCount() {
        var count = $('#smsbericht').val().length;
        var remaining = 160 - count;
        $('#totalCharacters').text(remaining.toString() + "/160 karakters");
    };
});
