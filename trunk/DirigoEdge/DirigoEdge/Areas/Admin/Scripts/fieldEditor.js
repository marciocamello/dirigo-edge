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
            // Timeout needed for CKEditor Bug
            setTimeout(function () {
                $('#FieldEntry textarea.editor').each(function () {
                    var id = $(this).attr("id");
                    var $el = $("#" + id);

                    if (!$el.hasClass("activated")) {
                        CKEDITOR.replace(id);
                        $el.addClass("activated");
                    }
                });
            }, 300);
                 
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

        var label = key;
        var inputName = key.toLowerCase().replace(/ /g, '-');

        //text inputs
        $("#FieldEntry input[name='" + inputName + "']").val(value);
        
        // Text Area / Paragraphs
        $("#FieldEntry textarea[name='" + inputName + "']").val(value);

        // Select Boxes
        $("#FieldEntry select[name='" + inputName + "']").val(value);

        // Checkboxes
        $("#FieldEntry div.checkContainer[data-label='" + label + "']").each(function() {
            var nValues = value;
            var $this = $(this);

            $.each(nValues, function(key, val) {

                var $input = $this.find("input[data-label='" + val + "']");
                if ($input.length > 0) {
                    $input.prop("checked", true);
                }
                else {
                    // Other / Specify
                    $this.find("input.otherInput").prop("checked", true);
                    $this.find("input.otherSpecifyInput").val(val);
                }
            });
        });

        // Radio Buttons
        $("#FieldEntry div.radioContainer[data-label='" + label + "']").each(function () {
            var $input = $(this).find("input[data-label='" + value + "']");
            if ($input.length > 0) {
                $input.prop("checked", true);
            }
            else {
                // Other / Specify
                $(this).find("input.otherInput").prop("checked", true);
                $(this).find("input.otherSpecifyInput").val(value);
            }
        });
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
        var includeOther = options.include_other_option || false;
        
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

                var inputHtml = "<input id='" + radioId + "'  name='" + radioName + "' type='radio' data-label='" + radioLabel + "' class='" + requiredClass + "' data-id='" + cId + "' autocomplete='off' " + checked + " />";
                $radioContainer.append("<label for='" + radioId + "' class='radioLabel'>" + inputHtml + " " + radioLabel + "</label>");

                $formContainer.append($radioContainer);
            });

            // Add Other choice if necessary
            if (includeOther) {
                var checkId = inputName + "OtherRadio";
                var checkName = label.toLowerCase().replace(/ /g, '-');
                var checkLabel = "Other";
                var checked = "";

                var $checkContainer = $("<div class='radioSingle' />");

                var inputHtml = "<input id='" + checkId + "'  name='" + checkName + "' type='radio' data-label='" + checkLabel + "' class='otherInput " + requiredClass + "' data-id='" + cId + "' autocomplete='off' " + checked + " />";
                $checkContainer.append("<label for='" + checkId + "' class='radioLabel'>" + inputHtml + " " + checkLabel + "</label>");
                $checkContainer.append('<input type="text" class="otherSpecifyInput" autocomplete="off" />');

                $formContainer.append($checkContainer);
            }
            
            $content.append($formContainer);
        }

        // CheckBoxes
        if (type == "checkboxes") {

            var $formContainer = $("<div class='formContainer checkContainer" + sizeClass + "' data-label='" + label + "'>");
            $formContainer.append("<label>" + label + "</label>");

            // Add CheckBox Options
            $.each(options.options, function (key, value) {
                var checkLabel = value.label;
                var checkId = value.label.toLowerCase().replace(/ /g, '-'); // lowercase, replace spaces with dashes
                var checked = value.checked == true ? "checked=checked" : "";
                var checkName = label.toLowerCase().replace(/ /g, '-');

                var $checkContainer = $("<div class='checkSingle' />");

                var inputHtml = "<input id='" + checkId + "'  name='" + checkName + "' type='checkbox' data-label='" + checkLabel + "' class='" + requiredClass + "' value='test' data-id='" + cId + "' autocomplete='off' " + checked + " />";
                $checkContainer.append("<label for='" + checkId + "' class='radioLabel'>" + inputHtml + " " + checkLabel + "</label>");

                $formContainer.append($checkContainer);
            });

            // Add Other choice if necessary
            if (includeOther) {
                var checkId = inputName + "OtherCheck";
                var checkName = inputName + "Check1";
                var checkLabel = "Other";
                var checked = "";

                var $checkContainer = $("<div class='checkSingle' />");

                var inputHtml = "<input id='" + checkId + "'  name='" + checkName + "' type='checkbox' data-label='" + checkLabel + "' class='otherInput " + requiredClass + "' value='test' data-id='" + cId + "' autocomplete='off' " + checked + " />";
                $checkContainer.append("<label for='" + checkId + "' class='radioLabel'>" + inputHtml + " " + checkLabel + "</label>");
                $checkContainer.append('<input type="text" class="otherSpecifyInput"/>');

                $formContainer.append($checkContainer);
            }

            $content.append($formContainer);
        }

        // Select Boxes / Dropdown
        if (type == "dropdown") {
            var $formContainer = $("<div class='formContainer dropdown'>");

            $formContainer.append("<label>" + label + sRequiredTokend + "</label>");
            var $select = $("<select name='" + inputName + "' data-label='" + label + "' ></select>");

            $.each(options.options, function(key, value) {

                var label = value.label;
                var checked = value.checked;
                var selectedText = checked ? "selected=selected" : "";

                $select.append("<option " + selectedText +" >" +label + "</option>");
            });

            $formContainer.append($select);

            if (description.length > 0) {
                $formContainer.append("<p class='labelDescription'>" + description + "</p>");
            }

            $content.append($formContainer);
        }
    });

    // Wrap each form container in list items for better viewing
    $content.find("div.formContainer").wrapAll("<ol></ol>").wrapInner("<li></li>");

    return $content;
};

// Keep at the bottom
$(document).ready(function () {
    fieldEditor = new fieldEditor_class();
    fieldEditor.initPageEvents();
});