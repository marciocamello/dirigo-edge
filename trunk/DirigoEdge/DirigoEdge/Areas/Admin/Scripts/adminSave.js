/// ===========================================================================================
/// Class allows a page's settings to be saved via AJAX based strictly on html5 data attributs
/// ===========================================================================================

save_class = function () {

};

save_class.prototype.initPageEvents = function () {
    var self = this;
    
    $("a.savePageButton").click(function() {
        var saveUrl = $(this).attr("data-url");
        var saveMessage = $(this).attr("data-saveMessage") || "Save Successful.";
        var data = self.getData();

        $("#SaveIndicator").show();
        $.ajax({
            url: saveUrl,
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data, null, 2),
            success: function (data) {
                var noty_id = noty({ text: 'Changes saved successfully.', type: 'success', timeout: 3000 });
                $("#SaveIndicator").hide();
            },
            error: function (data) {
                var noty_id = noty({ text: 'There was an error processing your request.', type: 'error' });
                $("#SaveIndicator").hide();
            }
        });
    });
};

save_class.prototype.getData = function() {
    var data = {
        entity : {
            
        }
    };

    // Text Fields
    $("input[type=text].saveField").each(function() {
        var field = $(this).attr("data-field");
        var value = $(this).attr("value");
        
        data.entity[field] = value;
    });

    // Checkboxes (booleans)
    $("input[type=checkbox].saveField").each(function () {
        var field = $(this).attr("data-field");
        var value = $(this).is(":checked");

        data.entity[field] = value;
    });
    
    // Select Boxes (single value)
    $("select.saveField").each(function () {
        var field = $(this).attr("data-field");
        var value = $(this).find("option:selected").val();

        data.entity[field] = value;
    });

    return data;
};


// Keep at the bottom
$(document).ready(function () {
    save = new save_class();
    save.initPageEvents();
});