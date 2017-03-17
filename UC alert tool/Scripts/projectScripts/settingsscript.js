$(document).ready(function () {
    $(window).bind("load resize", function () {// this will remove the settings sidebar if the user is using a mobile device
        var topOffset = 50;
        var width = (this.window.innerWidth > 0) ? this.window.innerWidth : this.screen.width;
        if (width < 768) {
            $('div.sidebar-settings').addClass('collapse');
            $('div.settingsContentWrapper').css('margin-left', '0px');
            topOffset = 100; // 2-row-menu
        } else {
            $('div.sidebar-settings').removeClass('collapse');
            $('div.settingsContentWrapper').css('margin-left', '250px');
        }
    });
});