content_modal_class = function () {

};

content_modal_class.prototype.initWordWrapEvents = function () {
    var self = this;

    $("#WordWrap").change(function () {
        var wrapWords = $(this).is(":checked");

        self.htmlEditor.getSession().setUseWrapMode(wrapWords);

        // Let's help out our fellow coders and save their settings so they don't check/uncheck every time
        $.ajax({
            url: "/contentadmin/setwordwrap",
            type: "POST",
            data: {
                wordWrap: wrapWords
            }
        });
    });
};

content_modal_class.prototype.initUpdateEditor = function ($html) {

    this.htmlEditor.getSession().setValue($html);
    $('#HTMLContent textarea').val($html);
};

content_modal_class.prototype.initCodeEditorEvents = function ($html) {
    var self = this;

    // Init Code Editor
    self.htmlEditor = ace.edit("HTMLContent");
    var theme = $("#EditorTheme :selected").attr("value");
    self.htmlEditor.setTheme(theme);
    self.htmlEditor.getSession().setMode("ace/mode/html");
    self.htmlEditor.getSession().setUseSoftTabs(true);
    self.htmlEditor.getSession().setUseWrapMode(true);
    self.htmlEditor.setShowInvisibles(true);
    self.htmlEditor.getSession().setValue($html);

    // Save
    self.htmlEditor.commands.addCommand({
        name: 'Save',
        bindKey: { win: 'Ctrl-S', mac: 'Command-S' },
        exec: function (editor) {
            $("#SaveContentButton").trigger("click");
        }
    });

    // Change editor Theme
    $("#EditorTheme").change(function () {
        var theme = $(this).attr("value");
        self.htmlEditor.setTheme(theme);
    });
};

content_modal_class.prototype.initContentImageUploadEvents = function () {
    var self = this;

    var afterImageInsert = function (imgTag) {
        $("#InsertImageModal").trigger('reveal:close');
        $("#AdminEditRawContentModal").reveal({
            "opened": function () {
                // Insert an img tag into the editor
                self.htmlEditor.insert(imgTag);

                // Highlight the newly placed tag
                self.htmlEditor.find(imgTag, { backwards: true, });
            }
        });
    };

    $("#modal-dropzone").addClass('dropzone').dropzone({
        url: "/admin/fileUpload/",
        init: function () {
            this.on("success", function (file, data) {
                var imgTag = "<img src='" + data.path + "' alt='' />";

                // Close the dialog box
                setTimeout(afterImageInsert(imgTag), 400);
            });
        }
    });
};

content_modal_class.prototype.manageContentAdminEvents = function () {
    var self = this;

    // Save Content Button
    $("#SaveContentButton").click(function () {
        var htmlContent;
        var template = $("#ContentTemplateSelect option:selected").attr("value");
        var isBasic;
        var id = $("div.editContent").attr("data-id");
        var contentName = $("#SaveContentButton").attr("data-name");

        // advanced editor
        htmlContent = self.htmlEditor.getValue();
        isBasic = false;
      
        var data = {
            entity: {
                ContentModuleId: id,
                ContentPageId: id,
                DisplayName: contentName,
                ModuleName: contentName,
                HTMLContent: htmlContent,
                Template: template,
                PublishDate: $("#PublishDate").attr("value"),
                Title: $("#PageTitle").attr("value"),
                MetaDescription: $("#MetaDescription").attr("value"),
                OGTitle: $("#OGTitle").attr("value"),
                OGImage: $("#OGImage").attr("value"),
                OGType: $("#OGType").attr("value"),
                OGUrl: $("#OGUrl").attr("value"),
                Canonical: $("#Canonical").attr("value")
            },
            // Let Ajax handler know if we're using an advanced editor or basic editor
            // Basic editor does not send over JS / CSS rules so we should leave the content as is in the controller
            isBasic: isBasic
        };

        var url = $(this).attr("data-url") || 'ModifyContent';
        $.ajax({
            url: "/blogadmin/" + url,
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data, null, 2),
            success: function (data) {
                self.refreshModuleContent(id);
                var noty_id = noty({ text: 'Changes saved successfully.', type: 'success', timeout: 1200 });
                $("#SaveSpinner").hide();
                $("#AdminEditRawContentModal").trigger('reveal:close');
            },
            error: function (data) {
                var noty_id = noty({ text: 'There was an error processing your request.', type: 'error' });
                $("#SaveSpinner").hide();
            }
        });
    });

    // Cancel button on the modal
    $(document).on('click', '#cancelModal', function () {
        $('#AdminEditRawContentModal').trigger('reveal:close');
    });
};

content_modal_class.prototype.refreshModuleContent = function (id) {
    /**
     * Reload the div with new html keeping the edit buttons
     */
    $.post('/admin/getModuleData', { id: id }, "json")
        .done(function (result) {
            $html = result.html;
            var $parent = $('div.adminButtons[data-id=' + id + ']').parent();
            var $adminEditHtml = $parent.find("div.adminButtons");

            $parent.html($html).prepend($adminEditHtml);
        });
};

// Keep at the bottom
$(document).ready(function () {
    
});