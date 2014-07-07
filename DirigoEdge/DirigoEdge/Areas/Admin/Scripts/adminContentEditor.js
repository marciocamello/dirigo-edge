adminEditor_class = function() {
    // We don't want CKEditor to pick up edit fields right away
    CKEDITOR.disableAutoInline = true;
};

adminEditor_class.prototype.initPageEvents = function() {
    // Allows the admin links to be positioned properly
    $("a.adminEdit").parent().addClass("relative");

    this.initEditModulePopupEvent();
    this.initSaveModuleEvent();
    this.initKeyboardShortcuts(); // Press a key to hide / show admin tips
};

adminEditor_class.prototype.initKeyboardShortcuts = function() {

    $(document).keypress(function (e) {
        // If we're in an input field don't do anything
        if ($(e.target).is('input')) {
            return;
        }

        // 'N' Key hide / show admin edit links
        if (e.which == 110) {
            $("a.adminEdit, div.adminButtons").toggle();
        }
        // 'B' Key hide / show grid overlay
        else if (e.which == 98) {
            $("#grid-displayer").toggle();
        }
    });
};

adminEditor_class.prototype.initEditModulePopupEvent = function () {
    var self = this;
    
    // Pop up module / content editor on screen if screen is wide enough
    $("a.adminEdit.onScreen").live("click", function (e) {
        var winWidth = $(document).width();
        self.dataEditingId = $(this).attr("data-id");
        var thisHref = $(this).attr("href");

        // If window is large enough, let's use edit in place
        // Otherwise, open in new tab
        if (winWidth > 1100) {
            e.preventDefault();

            $.post('/admin/getModuleData', { id: $(this).attr("data-id") }, "json")
                .done(function (result) {
                    $html = result.html;
                    $title = result.title;
                    
                    // Destroy the editor if it exists so we can create a new one
                    if (CKEDITOR.instances.ModuleContentEditContainer != null) {
                        CKEDITOR.instances.ModuleContentEditContainer.destroy();
                    }
                    // Create the new editor
                    $("#ModuleContentEditContainer").html($html);
                    $('.moduleTitle').html($title);
                    
                    CKEDITOR.config.contentsCss = '/Content/Themes/Base/Site.css';
                    self.editContentEditor = CKEDITOR.replace('ModuleContentEditContainer');
            });
            
            // Set the link to "open in admin interface" in case user does not want to edit inline
            $("#OpenInAdminLink").attr("href", thisHref);

            $('#ContentEditorLink, #WysiwygEditorLink').attr({ 'data-id': self.dataEditingId, 'href': thisHref }).show();
            $("#AdminEditContentModal").reveal();
        }
    });
    
    // Pop up module / content editor on screen if screen is wide enough
    $(document).on("click", "a.adminRawEdit.onScreen", function (e) {
        if ($(this).attr('id') != 'ContentEditorLink') {
            // The user did not come from the WYSIWYG
            $('#WysiwygEditorLink').hide();
        }
        var winWidth = $(document).width();
        self.dataEditingId = $(this).attr("data-id");
        var thisHref = $(this).attr("href");

        // If window is large enough, let's use edit in place
        // Otherwise, open in new tab
        if (winWidth > 1024) {
            e.preventDefault();

            $.post('/admin/getModuleData', { id: $(this).attr("data-id") }, "json")
                .done(function (result) {
                    $html = result.html;
                    $js = result.js;
                    $css = result.css;
                    $title = result.title;
                    
                    $('.editContent').attr('data-id', self.dataEditingId);
                    $("#SaveContentButton").attr('data-name', result.title);
                    $('.moduleTitle').html($title);

                    if (typeof(self.content) == 'undefined') {
                        self.content = new content_modal_class();
                        self.content.manageContentAdminEvents();
                        self.content.initCodeEditorEvents($html, $css, $js);
                        self.content.initWordWrapEvents();
                        self.content.initContentImageUploadEvents();
                    } else {
                        self.content.initUpdateEditor($html, $css, $js);
                    }
                });

            // Set the link to "open in admin interface" in case user does not want to edit inline
            $("#OpenInAdminLink").attr("href", thisHref);

            $("#AdminEditRawContentModal").reveal({ "opened": function() {
                
            } });
        }
    });
};

adminEditor_class.prototype.initSaveModuleEvent = function() {
    var self = this;
    
    // Save changes when editing a module on screen
    $("#ConfirmAdminContentUpdate").click(function () {

        var html = CKEDITOR.instances.ModuleContentEditContainer.getData();
        var $editContainer = $("#cke_ModuleContentEditContainer");

        // Save the content via ajax first
        common.showAjaxLoader($editContainer);
        $.ajax({
            url: "/BlogAdmin/UpdateModuleShort",
            type: "POST",
            data: {
                id: self.dataEditingId,
                html: html
            },
            success: function (data) {

                self.refreshModuleContent(self.dataEditingId);

                common.hideAjaxLoader($editContainer);
                
                $("#AdminEditContentModal").trigger('reveal:close');
            },
            error: function (data) {
                common.hideAjaxLoader($editContainer);
                alert("An error occurred. Please refresh and try again.");
            }
        });
    });
};

adminEditor_class.prototype.refreshModuleContent = function (id) {
    /**
     * Reload the div with new html keeping the edit buttons
     */
    $.post('/admin/getModuleData', { id: id }, "json").done(function (result) {
        $html = result.html;
        var $parent = $('div.adminButtons[data-id=' + id + ']').parent();

        var $adminEditHtml = $parent.find("div.adminButtons");

        $parent.html($html).prepend($adminEditHtml);
    });
};

// Keep at the bottom
$(document).ready(function () {
    adminEdit = new adminEditor_class();
    adminEdit.initPageEvents();
});