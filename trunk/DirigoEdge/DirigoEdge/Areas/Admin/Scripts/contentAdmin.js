/// ===========================================================================================
/// Content Admin Class. In charge of manage content, edit content for Modules and Pages
/// ===========================================================================================

content_class = function() {

};

content_class.prototype.initPageEvents = function() {

    this.manageContentAdminEvents();

    if ($("div.editContent").length > 0 && typeof(ace) != "undefined") {
        this.initCodeEditorEvents();
        this.initWordWrapEvents();
        this.initContentImageUploadEvents();
        this.initModuleUploadEvents();
    }

    if ($("div.manageContent").length > 0) {
        this.initDeleteContentEvent();
    }

    if ($("div.manageModule").length > 0) {
        this.initDeleteModuleEvent();
    }
    
    // View / Act on Revisions
    this.initRevisionEvents();
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

content_class.prototype.initDeleteContentEvent = function() {
    var self = this;
    $("div.manageContent table.manageTable td a.delete").click(function() {
        self.managePageId = $(this).attr("data-id");

        self.$managePageRow = $(this).parent().parent();
        var title = '"' + self.$managePageRow.find("td.title a").text() + '"';
        $("#popTitle").text(title);
        $("#DeleteModal").reveal();
    });

    // Confirm Delete Content
    $("#ConfirmContentDelete").click(function() {
        var id = self.managePageId;
        $.ajax({
            url: "/Admin/DeleteContent",
            type: "POST",
            data: {
                id: self.managePageId
            },
            success: function(data) {
                var noty_id = noty({ text: 'Content Page Successfully Deleted.', type: 'success', timeout: 2000 });
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

    $('#InsertContentImage').fileBrowser(function (object) {
        if (object.type === 'image' && object.src) {

            var imgTag = "<img src='" + object.src + "' alt='' />";
            // Insert an img tag into the editor
            self.htmlEditor.insert(imgTag);

            // Highlight the newly placed tag
            self.htmlEditor.find(imgTag, { backwards: true, });
        }
    });
};

content_class.prototype.initModuleUploadEvents = function () {

    $('#ChangeImageThumbnail').fileBrowser(function (object) {
        if (object.type === 'image' && object.src) {
                
            $("#ModuleThumbnail").attr("value", object.src);
            $("#ImageModuleThumbnail").attr("src", object.src);
        }
    });
    
    // Key up refreshes thumbnail
    $("#ModuleThumbnail").keyup(function () {
        var src = $(this).attr("value");
        $("#ImageModuleThumbnail").attr("src", src);
    });
};

content_class.prototype.manageContentAdminEvents = function() {
    var self = this;

    // WYSIWYG Editor
    if ($('#CKEDITPAGE').length > 0) {
        self.CKPageEditor = CKEDITOR.replace('CKEDITPAGE');
    }
    else if ($('#CKEDITCONTENT').length > 0) {
        self.CKPageEditor = CKEDITOR.replace('CKEDITCONTENT', {
            // options here
            height: 430
        });
    }

    // Save Content Button
    $("#SaveContentButton").click(function () {
       
        // Make sure Page Title exists
        if ($("#PageTitle").attr("value").length < 1) {
            alert("You must enter a page title before saving.");
            return false;
        }

        var data = self.getPageData();

        $("#SaveSpinner").show();
        var url = $(this).attr("data-url") || 'ModifyContent';
        $.ajax({
            url: "/blogadmin/" + url,
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data, null, 2),
            success: function(data) {
                noty({ text: 'Changes saved successfully.', type: 'success', timeout: 1200 });
                $("#SaveSpinner").hide();

                self.setPublishedStatusState(true);

                $("#PublishedDate").text(data.publishDate);

                // Update "Preview" button to use the just-saved / published id
                $("#PreviewContentButton").attr("href", common.updateURLParameter($("#PreviewContentButton").attr("href"), "id", $("div.editContent").attr("data-id")));
                
                // Refresh Revisions list
                self.refreshRevisionListing();
            },
            error: function(data) {
                noty({ text: 'There was an error processing your request.', type: 'error' });
                $("#SaveSpinner").hide();
            }
        });
    });
    
    // Save Module Button
    $("#SaveModuleButton").click(function () {

        var data = self.getPageData();

        $("#SaveSpinner").show();
        var url = $(this).attr("data-url") || 'ModifyContent';
        $.ajax({
            url: "/blogadmin/" + url,
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data, null, 2),
            success: function (data) {
                noty({ text: 'Changes saved successfully.', type: 'success', timeout: 1200 });
                $("#SaveSpinner").hide();
            },
            error: function (data) {
                noty({ text: 'There was an error processing your request.', type: 'error' });
                $("#SaveSpinner").hide();
            }
        });
    });
    
    // Save a draft
    $("#SaveDraftButton").click(function() {

        var data = self.getPageData();
        
        $("#SaveSpinner").show();
        $.ajax({
            url: "/blogadmin/savedraft",
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data, null, 2),
            success: function (data) {
                
                $("#SaveSpinner").hide();

                // Update "Preview" button to use the just-saved id
                $("#PreviewContentButton").attr("href", common.updateURLParameter($("#PreviewContentButton").attr("href"), "id", data.id));

                self.setPublishedStatusState(false);                

                // Refresh Revisions list
                self.refreshRevisionListing();
            },
            error: function (data) {
                noty({ text: 'There was an error processing your request.', type: 'error' });
                $("#SaveSpinner").hide();
            }
        });
    });
    
    // Toggle Draft Status Container
    $("#StatusLabel, #CloseDraftStatus").click(function () {
        $("#DraftStatusContainer").slideToggle();
    });
    
    // Change Draft Status
    $("#DraftStatusSelector").change(function () {
        var isActive = $("#DraftStatusSelector option:selected").val() == "published";
        var data = {
            entity: {
                ContentPageId: $("div.editContent").attr("data-id"),
                IsActive : isActive
            }
        };

        $("#SaveSpinner").show();
        $.ajax({
            url: "/blogadmin/changedraftstatus",
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data, null, 2),
            success: function (data) {
                $("#SaveSpinner").hide();

                self.setPublishedStatusState(isActive);
                
                // Update publish date if just switched to publish
                if (isActive) {
                    $("#PublishedDate").text(data.publishDate);
                }
            },
            error: function (data) {
                noty({ text: 'There was an error processing your request.', type: 'error' });
                $("#SaveSpinner").hide();
            }
        });
    });
    
    // View Permalink in new tab
    $("#ViewPermaLink").click(function (e) {
        e.preventDefault();
        window.open($("#SiteUrl").text() + $("#PermaLinkEnd").text() + "/",
            '_blank');
    });
};

content_class.prototype.getPageData = function() {
    var self = this;
    
    var htmlContent;
    var cssContent;
    var jsContent;
    var template = $("#ContentTemplateSelect option:selected").attr("value");
    var isBasic;

    // basic editor
    if (typeof (self.htmlEditor) == "undefined") {
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
            Permalink: $("#PermaLinkEnd").text().toLowerCase(),
            ModuleName: $("#ModuleName").attr("value"),
            Description: $("#ModuleDescription").val(),
            ThumbnailLocation: $("#ModuleThumbnail").val(),
            HTMLContent: self.parseSchemaContent(htmlContent),
            HTMLUnparsed : self.htmlEditor.getValue(),
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
            Canonical: $("#Canonical").attr("value"),
            SchemaId: $("#SchemaSelector option:selected").attr("data-id"),
            SchemaEntryValues: JSON.stringify(self.getSchemaValues())
        },
        // Let Ajax handler know if we're using an advanced editor or basic editor
        // Basic editor does not send over JS / CSS rules so we should leave the content as is in the controller
        isBasic: isBasic
    };

    return data;
};

// Returns an object with values
content_class.prototype.getSchemaValues = function () {

    // Key Value Pairs of labels / selected value
    var oValues = {};

    // Text , Image, and Paragraph / TextArea Values, DropDown
    $("#FieldEntry div.formContainer.text input[type=text], "  +
        "#FieldEntry div.formContainer.image input[type=text], " +
        "#FieldEntry div.formContainer.dropdown select, " +
        "#FieldEntry div.formContainer.paragraph textarea").each(function () {
        
        var label = $(this).attr("data-label");
        var value = $(this).val();
        oValues[label] = value;
    });
    
    // WYSIWYG Editors
    $("#FieldEntry div.formContainer.wysiwyg textarea").each(function () {
        var id = $(this).attr("id");
        var label = $(this).attr("data-label");
        var value = CKEDITOR.instances[id].getData();
        oValues[label] = value;
    });

    // Radio Boxes
    $("#FieldEntry div.radioContainer").each(function () {
        var label = $(this).attr("data-label");
        var $radioChecked = $(this).find("input[type=radio]:checked");
        var value = $(this).find("input[type=radio]:checked").attr("data-label");

        if ($radioChecked.hasClass("otherInput")) {
            value = $radioChecked.closest(".radioSingle").find("input.otherSpecifyInput").val();
        }
        
        oValues[label] = value;
    });

    // Check Boxes
    $("#FieldEntry div.checkContainer").each(function () {
        var label = $(this).attr("data-label");
        var values = [];
        $(this).find("input[type='checkbox']:checked").each(function() {

            var inputValue = "";
            if ($(this).hasClass("otherInput")) {
                inputValue = $(this).closest(".checkSingle").find("input.otherSpecifyInput").val();
            }
            else {
                inputValue = $(this).attr("data-label");
            }

            values.push(inputValue);
        });

        oValues[label] = values;
    });    

    return oValues;
};

content_class.prototype.parseSchemaContent = function (htmlContent) {
    // Use mustache to set variables to be replaced / operated on in editor

    return Mustache.to_html(htmlContent, this.getSchemaValues());
};

// Set publish button text, drop down selected
content_class.prototype.setPublishedStatusState = function (isActive) {
    
    if (isActive) {
        $("#SaveContentButton").text("Update");
        $("#StatusLabel").text("Published");
        $("#DraftStatusSelector").val("published");
    }
    else {
        $("#SaveContentButton").text("Publish");
        $("#StatusLabel").text("Draft");
        $("#DraftStatusSelector").val("draft");
    }
    
    // Just saved a draft or published, can now remove version notice
    this.updateVersionNotice();
};

// If there is a "there is a newer version available" message, close and remove it.
content_class.prototype.updateVersionNotice = function() {
    $("#VersionInfoContainer").fadeOut(function () {
        $(this).remove();
    });
};

content_class.prototype.initRevisionEvents = function () {

    var self = this;

    // Show the revision modal and insert proper html into the text area
    $("#RevisionsList ul li a").live("click", function () {
        var revisionId = $(this).attr("data-id");
        
        $("#RevisionDetailModal").reveal();
        
        // Update the switch revision link to point to the new revision id we just loaded
        var isBasic = typeof (self.htmlEditor) == "undefined";
        var editContentUrl = isBasic ? "/admin/editcontentbasic/" : "/admin/editcontent/";

        $("#SwitchRevision").attr("href", editContentUrl + revisionId + "/");

        self.setRevisionModalHtml(revisionId);
    });
};

content_class.prototype.refreshRevisionListing = function() {
    var $listContainer = $("#RevisionsList");
    var pageId = $("#Main div.editContent").attr("data-id");

    if ($listContainer.length < 1 || pageId < 1) { return; }

    common.showAjaxLoader($listContainer);

    $.get('/contentadmin/getrevisionlist/' + pageId + '/', function (data) {
        $listContainer.html(data.html);
        common.hideAjaxLoader($listContainer);
    });
};

content_class.prototype.setRevisionModalHtml = function (revisionId) {

    $("#RevisionDetailModal textarea").val("Loading...");
    $.ajax({
        url: "/contentadmin/getrevisionhtml",
        type: "POST",
        data: { revisionId: revisionId },
        success: function (data) {
            $("#RevisionDetailModal textarea").val(data.html);
        },
        error: function (data) {
           noty({ text: 'There was an error processing your request.', type: 'error' });
        }
    });
};

content_class.prototype.formatContentLink = function (value) {
    // replace spaces with dashes
    value = value.replace(/ /g, '-');

    // Strip bad characters
    return value.replace(/[^a-zA-Z0-9-_]/g, '').toLowerCase();
};

// Keep at the bottom
$(document).ready(function () {
    content = new content_class();
    content.initPageEvents();
});