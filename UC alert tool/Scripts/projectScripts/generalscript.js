$(document).ready(function () {
    $('#submitbutton').click(checkIfstartIsBeforeEnddate);
    $('#submitbutton').click(closeStoringIfAllDatesAreValid);
    $('#emailTitle').keyup(function () {
        var sourceText = $("#emailTitle").val();
        $("#previewTitle").val(sourceText);

    });
    $('#emailbody').keyup(function () {
        var sourceText = $("#emailbody").val();
        $("#previewUserInput").text(sourceText);
    });

    function copyDataToPreviewField(event) // element is the targeted element example, .previewUserInput
    {
        var sourceText = $(event.data.sourceElement).val();
        var currentTargetText = $(event.data.targetElement).text();
        $(event.data.targetElement).text = (sourceText);
    }

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
        if (enddate !== "Invalid Date") {
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

    };
});
