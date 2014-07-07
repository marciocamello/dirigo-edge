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

content_modal_class.prototype.initUpdateEditor = function ($html, $css, $js) {

    this.htmlEditor.getSession().setValue($html);
    $('#HTMLContent textarea').val($html);
    
    this.jsEditor.getSession().setValue($js);
    $('#JSContent textarea').val($js);
    
    this.cssEditor.getSession().setValue($css);
    $('#CSSContent textarea').val($css);
};

content_modal_class.prototype.initCodeEditorEvents = function ($html, $css, $js) {
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

    // Switch to CSS
    self.htmlEditor.commands.addCommand({
        name: 'switchTab',
        bindKey: { win: 'Ctrl-2', mac: 'Command-2' },
        exec: function (editor) {
            $("a[href=#CSS]").trigger("click");
            $("#CSSContent textarea").focus();
        }
    });
    // Switch to JS
    self.htmlEditor.commands.addCommand({
        name: 'switchTab',
        bindKey: { win: 'Ctrl-3', mac: 'Command-3' },
        exec: function (editor) {
            $("a[href=#JS]").trigger("click");
            $("#JSContent textarea").focus();
        }
    });
    // Save
    self.htmlEditor.commands.addCommand({
        name: 'Save',
        bindKey: { win: 'Ctrl-S', mac: 'Command-S' },
        exec: function (editor) {
            $("#SaveContentButton").trigger("click");
        }
    });

    self.cssEditor = ace.edit("CSSContent");
    self.cssEditor.setTheme(theme);
    self.cssEditor.getSession().setMode("ace/mode/css");
    self.cssEditor.getSession().setValue($css);

    // Temp test
    self.cssEditor.commands.addCommand({
        name: 'switchTab1',
        bindKey: { win: 'Ctrl-1', mac: 'Command-1' },
        exec: function (editor) {
            $("a[href=#Html]").trigger("click");
            $("#HTMLContent textarea").focus();
        },
        readOnly: true // false if this command should not apply in readOnly mode
    });

    self.jsEditor = ace.edit("JSContent");
    self.jsEditor.setTheme(theme);
    self.jsEditor.getSession().setMode("ace/mode/javascript");
    self.jsEditor.getSession().setValue($js);

    // Change editor Theme
    $("#EditorTheme").change(function () {
        var theme = $(this).attr("value");
        self.htmlEditor.setTheme(theme);
        self.cssEditor.setTheme(theme);
        self.jsEditor.setTheme(theme);
    });
};

content_modal_class.prototype.manageContentAdminEvents = function () {
    var self = this;

    // Save Content Button
    $("#SaveContentButton").click(function () {
        var htmlContent;
        var cssContent;
        var jsContent;
        var template = $("#ContentTemplateSelect option:selected").attr("value");
        var isBasic;
        var id = $("div.editContent").attr("data-id");
        var contentName = $("#SaveContentButton").attr("data-name");

        // advanced editor
        htmlContent = self.htmlEditor.getValue();
        cssContent = self.cssEditor.getValue();
        jsContent = self.jsEditor.getValue();
        isBasic = false;
      
        var data = {
            entity: {
                ContentModuleId: id,
                ContentPageId: id,
                DisplayName: contentName,
                ModuleName: contentName,                
                HTMLContent: htmlContent,
                JSContent: jsContent,
                CSSContent: cssContent,
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
            url: "/BlogAdmin/" + url,
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