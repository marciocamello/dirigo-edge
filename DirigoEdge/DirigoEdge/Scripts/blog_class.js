// Handles blog interaction such as AJAX loading 
blog_class = function () {



};

blog_class.prototype.initPageEvents = function () {

    var self = this;

    function getUrlVars() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    }

    // Ajax Load in more blog when clicking "Load More" button
    $("#LoadMoreBlogs").click(function () {
        var lastBlogId = $(this).attr("data-lastBlog");
        var blogCount = $(this).attr("data-blogCount");
        var user = $('.authorTitle').length ? $('.authorTitle a').html() : '';
        var date = getUrlVars()['date'];
        var idList = [];

        $('.bigBlogListContainer article').each(function() {
            idList.push($(this).data('id'));
        });

        var $button = $(this);
        common.showAjaxLoader($button);
        if (lastBlogId != 0) {
            $.ajax({
                url: "/BlogActions/LoadMoreBlogs/",
                type: "POST",
                data: {
                    lastBlogId: lastBlogId,
                    count: blogCount,
                    user: user,
                    date: typeof date != 'undefined' ? date : '',
                    idList : idList
                },
                success: function(data) {

                    $('.bigBlogListContainer').append(data.html);

                    // Update the button so we load the correct module
                    $button.attr("data-lastblog", data.lastBlogId);

                    if (data.lastBlogId == 0) {
                        // All blogs are loaded. Hide the button for now
                        $button.fadeOut();
                    }

                    common.hideAjaxLoader($button);
                },
                error: function(data) {

                    // All blogs are loaded. Hide the button for now
                    $button.fadeOut();

                    common.hideAjaxLoader($button);
                }
            });
        } else {
            $button.fadeOut();
        }
    });
    
    // Load more popular blogs
    // Ajax Load in more blog when clicking "Load More" button
    $("#LoadMorePopularBlogs").click(function () {
        var lastBlogId = $(this).attr("data-lastBlog");
        var blogCount = $(this).attr("data-blogCount");
        var idList = [];

        $('.popularBlogsContainer .blogItem').each(function () {
            idList.push($(this).data('id'));
        });

        var $button = $(this);
        common.showAjaxLoader($button);
        if (lastBlogId != 0) {
            $.ajax({
                url: "/BlogActions/LoadMorePopularBlogs/",
                type: "POST",
                data: { lastBlogId: lastBlogId, count: blogCount, idList: idList },
                success: function(data) {

                    $('.popularBlogsContainer ul').append(data.html);

                    // Update the button so we load the correct module
                    $button.attr("data-lastblog", data.lastBlogId);

                    if (data.lastBlogId == 0) {
                        // All blogs are loaded. Hide the button for now
                        $button.fadeOut();
                    }

                    common.hideAjaxLoader($button);
                },
                error: function(data) {

                    // All blogs are loaded. Hide the button for now
                    $button.fadeOut();

                    common.hideAjaxLoader($button);
                }
            });
        } else {
            $button.fadeOut();
        }
    });
    
    // Ajax Load in more blog when clicking "Load More" button
    $("#LoadMoreRelatedBlogs").click(function () {
        var lastBlogId = $(this).attr("data-lastBlog");
        var blogCount = $(this).attr("data-blogCount");
        var idList = [];

        $('.relatedPost article').each(function () {
            idList.push($(this).data('id'));
        });

        var $button = $(this);
        common.showAjaxLoader($button);
        $.ajax({
            url: "/BlogActions/LoadMoreRelatedBlogs/",
            type: "POST",
            data: { lastBlogId: lastBlogId, count: blogCount, idList: idList },
            success: function (data) {

                $('.relatedPostsInner').append(data.html);

                // Update the button so we load the correct module
                $button.attr("data-lastblog", data.lastBlogId);

                if (data.lastBlogId == 0) {
                    // All blogs are loaded. Hide the button for now
                    $button.fadeOut();
                }

                common.hideAjaxLoader($button);
            },
            error: function (data) {

                // All blogs are loaded. Hide the button for now
                $button.fadeOut();

                common.hideAjaxLoader($button);
            }
        });
    });
    
    // Ajax Load in more archives when clicking "Load More" button
    $("#LoadMoreArchives").click(function () {
        var lastMonth = $('.archiveContainer ul li:last-child .archive span').data('date');
        var blogCount = $(this).attr("data-blogCount");
        var user = $('.authorTitle').length ? $('.authorTitle a').html() : '';
        var date = getUrlVars()['date'];
        var idList = [];

        $('.bigBlogListContainer article').each(function () {
            idList.push($(this).data('id'));
        });

        var $button = $(this);
        common.showAjaxLoader($button);
        if (lastMonth != 0) {
            $.ajax({
                url: "/BlogActions/LoadMoreArchives/",
                type: "POST",
                data: {
                    lastMonth: lastMonth,
                    count: blogCount,
                    user: user,
                    date: typeof date != 'undefined' ? date : '',
                    idList: idList
                },
                success: function (data) {

                    $('.archiveContainer ul').append(data.html);

                    // Update the button so we load the correct module
                    $button.attr("data-lastMonth", $('.archiveContainer ul li:last-child .archive span').data('date'));

                    if (data.lastMonth == "0") {
                        // All blogs are loaded. Hide the button for now
                        $button.attr("data-lastMonth", 0);
                        $button.fadeOut();
                    }

                    common.hideAjaxLoader($button);
                },
                error: function (data) {

                    // All blogs are loaded. Hide the button for now
                    $button.fadeOut();

                    common.hideAjaxLoader($button);
                }
            });
        } else {
            $button.fadeOut();
        }
    });
};

// Keep at the bottom
$(document).ready(function () {
    blogActions = new blog_class();
    blogActions.initPageEvents();
});