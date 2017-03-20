$(document).ready(function () {
    console.log("test loaded settingscript");
    $(window).bind("load resize", function () {// this will remove the settings sidebar if the user is using a mobile device
        var topOffset = 50;
        var width = (this.window.innerWidth > 0) ? this.window.innerWidth : this.screen.width;
        if (width < 768) {
            $('.sidebar-settings').addClass('collapse');
            $('.settingsContentWrapper').css('margin-left', '0px');
            topOffset = 100; // 2-row-menu
        } else {
            $('.sidebar-settings').removeClass('collapse');
            $('.settingsContentWrapper').css('margin-left', '250px');
        }
    });
});