sidebar_class = function() {

};

sidebar_class.prototype.initPageEvents = function () {
    var self = this;
    
    // Sidebar menu
    $("#sidebar ul > li.has-dropdown > a").click(function (e) {
        e.preventDefault();
        
        var $li = $(this).parent();
        
        $li.find("ul.dropdown").slideToggle("fast", function() {
            if ($(this).is(":visible")) {
                $(this).parent().addClass("active");                
            }
            else {
                $(this).parent().removeClass("active");
            }
        });
    });

    // Keep sidebar open if on manage page
    var path = window.location.pathname;
    $("#sidebar ul > li.has-dropdown > ul > li > a[href='" + path + "']").closest("li.has-dropdown").addClass("active").find('ul.dropdown').show();

    // Toggle sidebar
    $("#ToggleSidebar").click(function () {
        $("body").toggleClass("sideBarClosed");
        
        if ($("body").hasClass("sideBarClosed")) {
            self.saveMenuState("sideBarClosed");
        } 
        else {
            self.saveMenuState("");
        }
    });

    self.timeout = {};
    $("body #sidebar").hover(function () {

        if ($(window).width() < 1025) {
            return;
        }

        clearTimeout(self.timeout);
        
        $("body").addClass("sidebarOpen");
    }, function () {

        if ($(window).width() < 1025) {
            return;
        }
        
        self.timeout = setTimeout(function () {
            $("body").removeClass("sidebarOpen");
        }, 300);
    });

    self.getMenuState();
};

sidebar_class.prototype.saveMenuState = function(state) {
    if (Modernizr.localstorage) {
        localStorage.setItem("menuState", state);
    }
};

sidebar_class.prototype.getMenuState = function () {
    if (Modernizr.localstorage) {
        var bClass = localStorage["menuState"];
        $("body").addClass(bClass);
    }
};


$(document).ready(function () {
    sidebarClass = new sidebar_class();
    sidebarClass.initPageEvents();
});