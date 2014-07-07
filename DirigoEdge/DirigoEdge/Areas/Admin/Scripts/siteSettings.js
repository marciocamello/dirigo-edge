/// ===========================================================================================
/// This currently serves as both the blog admin, user admin, and content admin Javascript area
/// ===========================================================================================

siteSettings_class = function() {

};

siteSettings_class.prototype.initPageEvents = function () {
    var self = this;
    
    self.updateRevisionRetensionVisibility();

    // Update revision retension when content revisions are enabled / disabled
    $("input[data-field='ContentPageRevisionsEnabled']").change(function() {
        self.updateRevisionRetensionVisibility();
    });
};

siteSettings_class.prototype.updateRevisionRetensionVisibility = function () {
    // Hide Revision Retension Policy if not enabled on page load
    if (!$("input[data-field='ContentPageRevisionsEnabled']").is(":checked")) {
        $("select.saveField[data-field='ContentPageRevisionsRetensionCount']").closest("div.row.retension").addClass("hide");
    } else {
        $("select.saveField[data-field='ContentPageRevisionsRetensionCount']").closest("div.row.retension").removeClass("hide");
    }
};

// Keep at the bottom
$(document).ready(function () {
    siteSettings = new siteSettings_class();
    siteSettings.initPageEvents();
});