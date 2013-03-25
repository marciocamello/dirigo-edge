/// ===========================================================================================
/// The Media Upload Widget allows users to upload and browse previously uploaded content
/// It is designed to be flexible - developers can utilize callbacks on upload / image select
/// functionality to decide how interaction should take place according to their requirements
/// ===========================================================================================

mediaUpload_class = function () {

};

mediaUpload_class.prototype.initPageEvents = function () {
    // Go ahead and kick it off on doc ready if it's on the screen
    $("#MediaUploader").kendoUpload({
        async: {
            saveUrl: "/MediaUpload/UploadFile",
            removeUrl: "/MediaUpload/RemoveFile",
            removeField: "media"
        }
    });

    $("#BrowseMedia").click(function () {
        var $container = $("#MediaListing");
        common.showAjaxLoader($container);
        $.ajax({
            url: "/MediaUpload/ViewDirectory",
            type: "POST",
            //data: data
            success: function (data) {
                $container.html(data.html);
                common.hideAjaxLoader($container);
            },
            error: function (data) {
                common.hideAjaxLoader($container);
            }
        });
    });

    $("#UploadedMediaListing li a.th").live("click", function () {
        var imgSrc = $(this).find("img").attr("src");

        // Check for callback on modal
        var callBack = $("#MediaModal").attr("data-callback");
        if (callBack.length > 1) {
            common.executeFunctionByName(callBack, window, imgSrc);
        }
    });
};


// Keep at the bottom
$(document).ready(function () {
    mediaUpload = new mediaUpload_class();
    mediaUpload.initPageEvents();
});