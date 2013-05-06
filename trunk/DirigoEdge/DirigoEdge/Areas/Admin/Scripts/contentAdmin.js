/// ===========================================================================================
/// This currently serves as both the blog admin, user admin, and content admin Javascript area
/// ===========================================================================================

content_class = function() {

};

content_class.prototype.initPageEvents = function() {

    this.manageContentAdminEvents();

    if ($("div.editContent").length > 0 && typeof(ace) != "undefined") {
        this.initCodeEditorEvents();
        this.initWordWrapEvents();
    }

    if ($("div.manageContent").length > 0) {
        this.initDeleteContentEvent();
    }

    if ($("div.manageModule").length > 0) {
        this.initDeleteModuleEvent();
    }
    
    // Insert Image Functionality on Modules and Pages
    if ($("#InsertImageModal").length > 0) {
        this.initContentImageUploadEvents();
    }
};


content_class.prototype.initWordWrapEvents = function () {
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


content_class.prototype.initDeleteModuleEvent = function() {
    var self = this;

    // Delete Module
    $("div.manageModule table.manageTable td a.delete").click(function() {
        self.managePageId = $(this).attr("data-id");

        self.$managePageRow = $(this).parent().parent();
        var title = '"' + self.$managePageRow.find("td.title a").text() + '"';
        $("#popTitle").text(title);
        $("#DeleteModal").reveal();
    });

    // Confirm Delete Content
    $("#ConfirmModuleDelete").click(function() {
        var id = self.managePageId;
        $.ajax({
            url: "/Admin/DeleteModule",
            type: "POST",
            data: {
                id: self.managePageId
            },
            success: function(data) {
                var noty_id = noty({ text: 'Module Successfully Deleted.', type: 'success', timeout: 2000 });
                self.$managePageRow.remove();
                $('#DeleteModal').trigger('reveal:close');
            },
            error: function(data) {
                $('#DeleteModal').trigger('reveal:close');
                var noty_id = noty({ text: 'There was an error processing your request.', type: 'error' });
            }
        });
    });

};

content_class.prototype.initDeleteContentEvent = function () {
    var self = this;
    $("div.manageContent table.manageTable td a.delete").click(function () {
        self.managePageId = $(this).attr("data-id");
        
        self.$managePageRow = $(this).parent().parent();
        var title = '"' + self.$managePageRow.find("td.title a").text() + '"';
        $("#popTitle").text(title);
        $("#DeleteModal").reveal();
    });

    // Confirm Delete Content
    $("#ConfirmContentDelete").click(function () {
        var id = self.managePageId;
        $.ajax({
            url: "/Admin/DeleteContent",
            type: "POST",
            data: {
                id: self.managePageId
            },
            success: function (data) {
                var noty_id = noty({ text: 'Content Page Successfully Deleted.', type: 'success', timeout: 2000 });
                self.$managePageRow.remove();
                $('#DeleteModal').trigger('reveal:close');
            },
            error: function (data) {
                $('#DeleteModal').trigger('reveal:close');
                var noty_id = noty({ text: 'There was an error processing your request.', type: 'error' });
            }
        });
    });

}

content_class.prototype.initCodeEditorEvents = function() {
    var self = this;

    // Init Code Editor
    var theme = $("#EditorTheme :selected").attr("value");
    self.htmlEditor = ace.edit("HTMLContent");
    self.htmlEditor.setTheme(theme);
    self.htmlEditor.getSession().setMode("ace/mode/html");
    self.htmlEditor.getSession().setUseSoftTabs(true);
    self.htmlEditor.getSession().setUseWrapMode(true);
    self.htmlEditor.setShowInvisibles(true);

    // Switch to CSS
    self.htmlEditor.commands.addCommand({
        name: 'switchTab',
        bindKey: { win: 'Ctrl-2', mac: 'Command-2' },
        exec: function(editor) {
            $("a[href=#CSS]").trigger("click");
            $("#CSSContent textarea").focus();
        }
    });
    // Switch to JS
    self.htmlEditor.commands.addCommand({
        name: 'switchTab',
        bindKey: { win: 'Ctrl-3', mac: 'Command-3' },
        exec: function(editor) {
            $("a[href=#JS]").trigger("click");
            $("#JSContent textarea").focus();
        }
    });
    // Save
    self.htmlEditor.commands.addCommand({
        name: 'Save',
        bindKey: { win: 'Ctrl-S', mac: 'Command-S' },
        exec: function(editor) {
            $("#SaveContentButton").trigger("click");
        }
    });
    //self.htmlEditor.commands.addCommand({
    //    name: 'Fullscreen',
    //    bindKey: { win: 'Ctrl-F', mac: 'Command-F' },
    //    exec: function (editor) {
    //        $("#ContentRowContainer").toggleClass("fullscreen");
    //    },
    //    readOnly: true // false if this command should not apply in readOnly mode
    //});


    self.cssEditor = ace.edit("CSSContent");
    self.cssEditor.setTheme(theme);
    self.cssEditor.getSession().setMode("ace/mode/css");
    // Temp test
    self.cssEditor.commands.addCommand({
        name: 'switchTab1',
        bindKey: { win: 'Ctrl-1', mac: 'Command-1' },
        exec: function(editor) {
            $("a[href=#Html]").trigger("click");
            $("#HTMLContent textarea").focus();
        },
        readOnly: true // false if this command should not apply in readOnly mode
    });

    self.jsEditor = ace.edit("JSContent");
    self.jsEditor.setTheme(theme);
    self.jsEditor.getSession().setMode("ace/mode/javascript");

    // Change editor Theme
    $("#EditorTheme").change(function() {
        var theme = $(this).attr("value");
        self.htmlEditor.setTheme(theme);
        self.cssEditor.setTheme(theme);
        self.jsEditor.setTheme(theme);
    });
};

content_class.prototype.initContentImageUploadEvents = function() {
    var self = this;
    
    Dropzone.options.myAwesomeDropzone = {
        url: "/admin/fileUpload/",
        init: function () {
            this.on("success", function (file, data) {

                var imgTag = "<img src='" + data.path + "' alt='' />";
                // Insert an img tag into the editor
                self.htmlEditor.insert(imgTag);

                // Highlight the newly placed tag
                self.htmlEditor.find(imgTag, { backwards: true, });

                // Close the dialog box
                setTimeout(function () {
                    $("#InsertImageModal").trigger('reveal:close');
                }, 400);
            });
        }
    };
};

content_class.prototype.manageContentAdminEvents = function() {
    var self = this;

    // Preview Content Window
    $("#PreviewContentButton").click(function() {
        var title = $("#ContentName").attr("value").toLowerCase();
        title = title.replace(/ /g, '-');
        var url = $(this).attr("data-url") || '/Content/';
        var host = 'http://' + window.location.host + url;

        // Open the blog in a new window
        window.open(host + title + '/' + '?debug=true');
    });

    // WYSIWYG Editor
    if ($('#CKEDITPAGE').length > 0) {
        self.CKPageEditor = CKEDITOR.replace('CKEDITPAGE');
    } else if ($('#CKEDITCONTENT').length > 0) {
        self.CKPageEditor = CKEDITOR.replace('CKEDITCONTENT', {
            // options here
            height: 430
        });
    }

    // Save Content Button
    $("#SaveContentButton").click(function () {
        var htmlContent;
        var cssContent;
        var jsContent;
        var template = $("#ContentTemplateSelect option:selected").attr("value");
        var isBasic;
        
        // basic editor
        if (typeof(self.htmlEditor) == "undefined") {
            htmlContent = CKEDITOR.instances.CKEDITCONTENT.getData();
            isBasic = true;
        } 
        else {
            // advanced editor
            htmlContent = self.htmlEditor.getValue();
            cssContent = self.cssEditor.getValue();
            jsContent = self.jsEditor.getValue();
            isBasic = false;
        }

        var data = {
            entity: {
                ContentModuleId: $("div.editContent").attr("data-id"),
                ContentPageId: $("div.editContent").attr("data-id"),
                DisplayName: $("#ContentName").attr("value"),
                ModuleName: $("#ContentName").attr("value"),
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
            success: function(data) {
                var noty_id = noty({ text: 'Changes saved successfully.', type: 'success', timeout: 1200 });
                $("#SaveSpinner").hide();
            },
            error: function(data) {
                var noty_id = noty({ text: 'There was an error processing your request.', type: 'error' });
                $("#SaveSpinner").hide();
            }
        });
    });
};

// Keep at the bottom
$(document).ready(function () {
    content = new content_class();
    content.initPageEvents();
});