

// Highlight efekti için
    (function($) {
    "use strict";

    var path = window.location.href; // because the 'href' property of the DOM element is the absolute path
        $("#layoutSidenav_nav .sb-sidenav a.nav-link").each(function() {
            if (path.includes(this.href)) {
                $(this).addClass("active");
            }
            if ($('.nav-link.active').length == 2) {
                $('#homeNavLink').removeClass("active");
            }
        });

    // Toggle the side navigation
    $("#sidebarToggle").on("click", function(e) {
        e.preventDefault();
        $("body").toggleClass("sb-sidenav-toggled");
    });
})(jQuery);
