mcp_class = function () {
    


};

mcp_class.prototype.initPageEvents = function() {
    // Lazy Load background images
    $("div.lazyLoad").removeClass("lazyLoad");
};

// Keep at the bottom
$(document).ready(function () {
    mcp = new mcp_class();
    mcp.initPageEvents();
});