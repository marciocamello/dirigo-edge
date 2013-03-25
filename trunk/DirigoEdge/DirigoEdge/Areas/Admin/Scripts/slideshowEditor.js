/// ===========================================================================================
/// This currently serves as both the blog admin, user admin, and content admin Javascript area
/// ===========================================================================================

slideEditor_class = function () {
    this.inputBox; // Used to store input field when uploading slide images
};

slideEditor_class.prototype.initPageEvents = function () {
    this.initslideActions();
    this.initSaveEvent();
    this.initMediaModalEvent();

    

};

slideEditor_class.prototype.initMediaModalEvent = function() {
    var self = this;
    
    $("#SlideEditList a.revealUpload").click(function () {
        self.inputBox = $(this).parent().find("input.imageLocation");

        $("#MediaModal").reveal();
    });

};

// Callback for clicking on uploaded content image in media upload window
slideEditor_class.prototype.fInsertImage = function (imageName) {

    slideEditor.inputBox.attr("value", imageName);
    $("#MediaModal").trigger('reveal:close');
};

slideEditor_class.prototype.initSaveEvent = function () {
    $("#SaveSlideShow").click(function() {
        var data = {};
        data.entity = {};
        data.entity.SlideshowModuleId = $("#SlideEditList").attr("data-id");
        data.entity.SlideShowName = $("#ContentName").attr("value");
        data.entity.AdvanceSpeed = $("#AdvanceSpeed").attr("value");
        data.entity.Animation = $("#AnimationSelect option:selected").attr("value");
        data.entity.AnimationSpeed = $("#AnimationSpeed").attr("value");
        data.entity.UseTimer = $("#UseTimer").is(":checked");
        data.entity.PauseOnHover = $("#PauseOnHover").is(":checked");
        data.entity.UseDirectionalNav = $("#UseDirectionalNav").is(":checked");
        data.entity.ShowBullets = $("#ShowBullets").is(":checked");
        data.entity.Slides = [];

        $("#SlideEditList li.slideContainer").each(function () {
            var slide = {
                ImageLocation: $(this).find("input.imageLocation").attr("value"),
                Caption: $(this).find("input.caption").attr("value"),
                HtmlContent: "",
                Id : 1
            };
            
            data.entity.Slides.push(slide);
        });

        var $container = $("#SlideEditList");
        common.showAjaxLoader($container);
        $.ajax({
            url: "/SlideAdmin/SaveSlideshow",
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data, null, 2),
            success: function (data) {
                var noty_id = noty({ text: 'Slideshow saved successfully.', type: 'success', timeout: 3000 });
                common.hideAjaxLoader($container);
            },
            error: function (data) {
                var noty_id = noty({ text: 'There was an error processing your request.', type: 'error' });
                common.hideAjaxLoader($container);
            }
        });
    });
};

slideEditor_class.prototype.initslideActions = function () {
    // Move up
    $("#SlideEditList ul.actionList li a.moveUp").live("click", function() {
        var $currentRow = $(this).closest("li.slideContainer");
        $currentRow.prev("li.slideContainer").before($currentRow);
    });

    // Move Down
    $("#SlideEditList ul.actionList li a.moveDown").live("click", function() {
        var $currentRow = $(this).closest("li.slideContainer");
        $currentRow.next("li.slideContainer").after($currentRow);
    });

    // Delete Row
    $("#SlideEditList ul.actionList li a.delete").live("click", function() {
        $(this).closest("li.slideContainer").remove();
    });

    // Add Slide
    $("#AddSlide").click(function() {
        var $html = $("#SlideCopyHtml").html();
        $("#SlideEditList").append($html);
    });
};


// Keep at the bottom
$(document).ready(function () {
    slideEditor = new slideEditor_class();
    slideEditor.initPageEvents();
});