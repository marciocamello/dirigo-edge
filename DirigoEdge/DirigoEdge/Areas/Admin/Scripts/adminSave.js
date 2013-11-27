/// ===========================================================================================
/// Class allows a page's settings to be saved via AJAX based strictly on html5 data attributs
/// ===========================================================================================

save_class = function () {

};

save_class.prototype.saveAdminData = function ($this) {
    var saveUrl = $this.attr("data-url");
    var saveMessage = $this.attr("data-saveMessage") || "Save Successful.";
    var isListData = $this.data('list');
    var data = isListData ? this.getListData() : this.getData();

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
            $('form[data-list="true"] tbody tr.altered').removeClass('altered');
            $('.reveal-modal').trigger('reveal:close');
        },
        error: function (data) {
            var noty_id = noty({ text: 'There was an error processing your request.', type: 'error' });
            $("#SaveIndicator").hide();
        }
    });
};

save_class.prototype.initPageEvents = function () {
    var self = this;

    $("a.savePageButton").click(function () {
        self.saveAdminData($(this));
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

save_class.prototype.getListData = function () {
    var entities = {
        entity: {

        }
    };

    var data = [];

    $('form[data-list="true"] tbody tr.altered').each(function () {
        var $row = $(this);
        // Text Fields
        $row.find("input[type=text].saveField").each(function () {
            var field = $(this).attr("data-field");
            var value = $(this).attr("value");

            entities.entity[field] = value;
        });

        // Checkboxes (booleans)
        $row.find("input[type=checkbox].saveField").each(function () {
            var field = $(this).attr("data-field");
            var value = $(this).is(":checked");

            entities.entity[field] = value;
        });

        // Select Boxes (single value)
        $row.find("select.saveField").each(function () {
            var field = $(this).attr("data-field");
            var value = $(this).find("option:selected").val();

            entities.entity[field] = value;
        });

        // Hidden Fields
        $row.find("input[type=hidden].saveField").each(function () {
            var field = $(this).attr("data-field");
            var value = $(this).val();

            entities.entity[field] = value;
        });
        data.push(entities.entity);
        entities.entity = {};
    });
    return data;
};


// Keep at the bottom
$(document).ready(function () {
    save = new save_class();
    save.initPageEvents();
});