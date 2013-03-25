// Handles blog interaction such as AJAX loading 
blog_class = function () {



};

blog_class.prototype.initPageEvents = function () {

    var self = this;

    // Ajax Load in more blog when clicking "Load More" button
    $("#LoadMoreBlogs").click(function () {
        var lastBlogId = $(this).attr("data-lastBlog");
        var blogCount = $(this).attr("data-blogCount");

        var $button = $(this);
        common.showAjaxLoader($button);
        $.ajax({
            url: "/BlogActions/LoadMoreBlogs/",
            type: "POST",
            data: { lastBlogId: lastBlogId, count: blogCount },
            success: function (data) {
                
                $button.before("<div class='newBlog inactive'>" + data.html + "</div>");

                $("div.newBlog.inactive").fadeIn().removeClass("inactive");

                // Update the button so we load the correct module
                $button.attr("data-lastblog", data.lastBlogId);

                common.hideAjaxLoader($button);
            },
            error: function (data) {

                // All blogs are loaded. Hide the button for now
                $button.fadeOut();

                common.hideAjaxLoader($button);
            }
        });
    });
};

// Keep at the bottom
$(document).ready(function () {
    blogActions = new blog_class();
    blogActions.initPageEvents();
});