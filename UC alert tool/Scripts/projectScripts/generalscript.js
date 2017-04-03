$(document).ready(function () {
    $('#submitbutton').click(checkIfstartIsBeforeEnddate);
    $('#submitbutton').click(closeStoringIfAllDatesAreValid);
    $('.readMoreButton').click(showFullArticle);
    $('.readLessButton').click(showOnlyFirstPartOfArticle);
    $('#smsbericht').keyup(setRemainingCount);

    $('#emailTitle').keyup(function () { 
        var sourceText = $("#emailTitle").val().replace(/\r?\n/g, '<br/>');
        $("#previewTitle").val(sourceText);

    });
    $('#emailbody').keyup(function () {
        var sourceText = $("#emailbody").val().replace(/\r?\n/g, '<br/>');
        $("#previewUserInput").html(sourceText);
    });
    $('#ProductID').change(function () {
        var urlName = location.pathname.split('/').slice(-1)[0];
        var selectedProduct = $('#ProductID').find(":selected").text();
        if (urlName == "meldingmetsms") {
            var url = '/rapporteren/recipientSMSCount?productname=' + selectedProduct.toString();
            $.ajax({
                type: 'POST',
                url: url,
                success: onRecipientReceiveSMS,
                Error: failedRecipientReceive
            });
        }
        else if (urlName == "meldingmetemail") {
            var url = '/rapporteren/recipientEmailCount?productname=' + selectedProduct.toString();
            $.ajax({
                type: 'POST',
                url: url,
                success: onRecipientReceiveEmail,
                Error: failedRecipientReceive
            });
        }
    });
    // functions
    function onRecipientReceiveSMS(response) {
        $('#Summeryfield').text("Het aantal ontvangers dat een sms krijgen: " + response);
    }
    function onRecipientReceiveEmail(response) {
        $('#Summeryfield').text("Het aantal ontvangers die een email krijgen: " + response);
    }
    function failedRecipientReceive(response) {
        alert('Error tijdens het ophalen van aantal ontvangers - ' + response);
    }

    function showFullArticle() {
        $(this).parent().css(
            'display','none'
        );
        $(this).parent().parent().find(".fullText").css(
            "display", "inline"
        );
    }
    function showOnlyFirstPartOfArticle() {
        $(this).parent().css(
            "display", "none"
        );
        $(this).parent().parent().find(".previewText").css(
            "display", "inline"
        );
    }


    function setRemainingCount() {
        var count = $('#smsbericht').val().length;
        var remaining = 160 - count;

        var ModuloMoreMessages = count % 153; // total used characters in the last message
        var remainingMoreMessages = 153 - ModuloMoreMessages;
        if (remainingMoreMessages == 153 ) { // modulo 153%153 = 0 and 153-0 = 153 characters remaining - this is wrong infomation for the user so it will be set to 0 (chacters remaining)
            remainingMoreMessages = 0;
        }
        if (count > 160) {
            $('#totalCharacters').text(remainingMoreMessages.toString() + "/153 karakters"); 
            $('#totalSMSForOneMessage').text(Math.ceil((count / 153)) + " berichten"); 
        }
        else {
            $('#totalSMSForOneMessage').text("1 bericht");
            $('#totalCharacters').text(remaining.toString() + "/160 karakters"); 
        }
    };





    function checkIfstartIsBeforeEnddate() {
        $('.errortext').remove();
        var begindate = new Date($('#begindatum').val());
        var beginTime = $('#begintijd').val();
        var beginTimeSplit = beginTime.split(':');
        var enddate = new Date($('#einddatum').val());
        var endTime = $('#eindtijd').val();
        var endTimeSplit = endTime.split(':');
        if (!Date.parse($('#einddatum').val()) && endTime == "")// no enddate and endtime
        {
            return true;
        }
        if (Date.parse($('#einddatum').val()) && endTime == "") // if only date is entered
        {
            $('#eindtijd').after("<p class='errortext'>Vul een eindtijd in</p>");
            return false
        }
        else if (!Date.parse($('#einddatum').val()) && endTime != "")// if only time is entered
        {
            $('#eindtijd').after("<p class='errortext'>Vul een einddatum in</p>");
            return false
        }
        if (Date.parse(begindate)) // check if begintime is earlier than the enddate
        {
            if (begindate < enddate) {
                if (endTime === "") {
                    $('#eindtijd').after("<p class='errortext'>Vul een eindtijd in</p>");
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
                if (beginTime === "" || endTime === "") {
                    $('#eindtijd').after("<p class='errortext'>Vul een einddatum in</p>");
                    return false;

                }
                else if (beginTimeSplit[0] > endTimeSplit[0]) {
                    $('#eindtijd').after("<p class='errortext'>Vul een correcte einddatum in</p>");
                    return false;
                }
                else if (beginTimeSplit[0] === endTimeSplit[0]) {
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
        else {
            $('#eindtijd').after("<p class='errortext'>Vul een einddatum in plz</p>");
            return false
        }
    };
    function closeStoringIfAllDatesAreValid() {
        if ($('#isGeslotenCheckbox').is(":checked")) {
            var begindate = new Date($('#begindatum').val());
            var beginTime = $('#begintijd').val();
            var enddate = new Date($('#einddatum').val());
            var endTime = $('#eindtijd').val();
            if (begindate !== "Invalid Date" && beginTime !== "" && enddate !== "Invalid Date" && endTime !== "") {
                return true
            }
            else {
                $('#isGeslotenCheckbox').after("<p class='errortext'>Vul alle waarden in om de storing te sluiten</p>");
                return false;
            }
        }
        else {
            return true;
        }
    }
});
