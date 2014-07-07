/*
 *  Schema / Field Editor integrated into Module Editor 
*/

fieldEditor_class = function() {
    this.moduleId = $("#Main > div.editContent").attr("data-id");
};

fieldEditor_class.prototype.initPageEvents = function () {
    var self = this;
    
    // Load Schema into Field editor on Schema Change
    $("#SchemaSelector").change(function() {
        var schemaId = $("#SchemaSelector option:selected").attr("data-id");

        // Check for No Schema
        if (schemaId == -1) {
            $("#FieldEntry").html("<p>No Schema assigned.</p>");
            return;
        }

        // If there is a schema, load it
        self.loadSchemaIntoFields(schemaId, self.moduleId);
        
        // Update the edit schema link
        $("#EditSchemaLink").attr("href", "/admin/editschema/" + schemaId);
    }).trigger("change");

    // Refresh the schema by simply triggering a change on the drop down
    $("#RefreshSchemaLink").click(function() {
        $("#SchemaSelector").trigger("change");
    });
};

fieldEditor_class.prototype.loadSchemaIntoFields = function (schemaId, moduleId) {
    var self = this;

    var data = {
        schemaId: schemaId,
        moduleId: moduleId,
        isPage: $("#PageTitle").length > 0
    };

    // Set loading spinner
    var $container = $("#FieldEntry");
    common.showAjaxLoader($container);
    $("#RefreshSchemaLink i.fa-refresh").addClass("fa-spin");
    
    // Kick off ajax to get the schema and values
    $.ajax({
        url: "/contentadmin/getschemahtml",
        type: "POST",
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data, null, 2),
        success: function (data) {
            var obj = jQuery.parseJSON(data.data);

            // Clear the html
            $("#EditorTab").html("<div id='FieldEditorContainer'></div>");
            
            // Build out the fields into html and then place them on the page
            var $content = self.buildSchemaContent(obj.fields);
            $("#FieldEditorContainer").append($content);
            
            // Set the user values into the newly placed fields
            var schemaData = jQuery.parseJSON(data.values);

            if (schemaData != null) {
                self.loadSchemaValues(schemaData);
            }
            
            // Set up Image Uploaders
            $('#FieldEntry a.upload').fileBrowser({
                    directory: ""
                },
                function(object) {
                    if (object.type === 'image' && object.src) {

                        var $button = $(object.elem);
                        var $input = $button.parent().find("input[type=text]");
                        $input.val(object.src);
                    }
                }
            );
            
            // Setup CKEditors
            $('#FieldEntry textarea.editor').each(function() {
                CKEDITOR.replace($(this).attr("id"));
            });
                 

            // End Loading
            $("#RefreshSchemaLink i.fa-refresh").removeClass("fa-spin");
            common.hideAjaxLoader($container);
        },
        error: function (data) {
            alert("An Error Occurred. Please refresh and try again.");
        }
    });
};

fieldEditor_class.prototype.loadSchemaValues = function (schemaValues) {

    $.each(schemaValues, function (key, value) {

        var inputName = key.toLowerCase().replace(/ /g, '-');

        // Currently Only handles text inputs and textareas
        $("#FieldEntry input[name='" + inputName + "']").val(value);
        
        $("#FieldEntry textarea[name='" + inputName + "']").val(value);        
    });
};

fieldEditor_class.prototype.buildSchemaContent = function(fields) {

    var $content = $("<form id='FieldEntry'>");

    // loop through the fields
    $.each(fields, function (index, value) {
        var oItem = value;
        var label = oItem.label;
        var inputName = label.toLowerCase().replace(/ /g, '-'); // lowercase, replace spaces with dashes
        var type = oItem.field_type;
        var bRequired = oItem.required;
        var sRequiredTokend = (bRequired) ? " <span class='requiredToken'>*</span>" : "";
        var cId = oItem.cid;
        var requiredClass = (bRequired) ? "required" : "";
        var options = oItem.field_options;
        var description = options.description || "";
        var sizeClass = options.size || "";


        // Text Fields
        if (type == "text") {

            var $formContainer = $("<div class='formContainer text " + sizeClass + "'>");

            $formContainer.append("<label>" + label + sRequiredTokend + "</label>");
            $formContainer.append("<input name='" + inputName + "' data-label='" + label + "' type='text' class='" + requiredClass + "' data-id='" + cId + "' autocomplete='off' />");

            if (description.length > 0) {
                $formContainer.append("<p class='labelDescription'>" + description + "</p>");
            }

            $content.append($formContainer);
        }
        
        // Paragraph / Text Area Fields
        if (type == "paragraph") {
            var $formContainer = $("<div class='formContainer paragraph " + sizeClass + "'>");

            $formContainer.append("<label>" + label + sRequiredTokend + "</label>");
            $formContainer.append("<textarea name='" + inputName + "' data-label='" + label + "' type='text' class='" + requiredClass + "' data-id='" + cId + "' autocomplete='off' ></textarea>");

            if (description.length > 0) {
                $formContainer.append("<p class='labelDescription'>" + description + "</p>");
            }

            $content.append($formContainer);
        }
        
        // Image Uploader Fields
        if (type == "image") {

            var $formContainer = $("<div class='formContainer image " + sizeClass + "'>");

            $formContainer.append("<label>" + label + sRequiredTokend + "</label>");
            $formContainer.append("<input name='" + inputName + "' data-label='" + label + "' type='text' class='" + requiredClass + "' data-id='" + cId + "' autocomplete='off' />");
            $formContainer.append("<a href='#' class='upload secondary button small'><i class='fa fa-picture-o'></i> Select Image...</a>");

            if (description.length > 0) {
                $formContainer.append("<p class='labelDescription'>" + description + "</p>");
            }

            $content.append($formContainer);
        }
        
        // WYSIWYG Editor 
        if (type == "wysiwyg") {
            var $formContainer = $("<div class='formContainer wysiwyg " + sizeClass + "'>");

            $formContainer.append("<label>" + label + sRequiredTokend + "</label>");
            $formContainer.append("<textarea name='" + inputName + "' id='" +cId + "' data-label='" + label + "' type='text' class='editor " + requiredClass + "' data-id='" + cId + "' autocomplete='off' ></textarea>");

            if (description.length > 0) {
                $formContainer.append("<p class='labelDescription'>" + description + "</p>");
            }

            $content.append($formContainer);
        }
        
        // Multiple Choice / Radio Buttons
        if (type == "radio") {

            var $formContainer = $("<div class='formContainer radioContainer" + sizeClass + "' data-label='" + label + "'>");
            $formContainer.append("<label>" + label + "</label>");

            // Add Radio Options
            $.each(options.options, function(key, value) {
                var radioLabel = value.label;
                var radioId = value.label.toLowerCase().replace(/ /g, '-'); // lowercase, replace spaces with dashes
                var checked = value.checked == true ? "checked=checked" : "";
                var radioName = label.toLowerCase().replace(/ /g, '-'); 

                var $radioContainer = $("<div class='radioSingle' />");

                $radioContainer.append("<input id='" + radioId + "'  name='" + radioName + "' type='radio' data-label='" + radioLabel + "' class='" + requiredClass + "' value='test' data-id='" + cId + "' autocomplete='off' " + checked + " />");
                $radioContainer.append("<label for='" + radioId + "' class='radioLabel inline'>" + radioLabel + "</label>");

                $formContainer.append($radioContainer);
            });
            
            $content.append($formContainer);
        }

    });

    return $content;
};

// Keep at the bottom
$(document).ready(function () {
    fieldEditor = new fieldEditor_class();
    fieldEditor.initPageEvents();
});