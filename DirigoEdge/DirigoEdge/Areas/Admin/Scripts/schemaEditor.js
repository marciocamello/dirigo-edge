/// ===========================================================================================
/// Schema Editor
/// ===========================================================================================

schemaEditor_class = function () {
    this.id = $(".editContent").data("id");

    // No likey autosave
    Formbuilder.options.AUTOSAVE = false;
    
    this.schema = new Formbuilder({
        selector: '#FormBuilder',
        autosave: false,
        bootstrapData: ODATA.fields
    });
};


schemaEditor_class.prototype.initPageEvents = function () {
    var self = this;

    // Set theme to match Foundation
    $(".js-save-form").addClass("button").text("Save Schema");

    // Set up Save event
    self.schema.on('save', function (data) {
        $.ajax({
            url: "/contentadmin/saveschema",
            type: "POST",
            data: {
                name : $("#SchemaName").val(),
                data: data,
                id : self.id
            }
        });
    });
    
    // Sticky Edit Window
    var $sidebar = $("#FormBuilder .fb-left"),
        $window = $("#Main"),
        offset = $sidebar.offset(),
        topPadding = 30;

    $window.scroll(function () {
        
        if ($window.scrollTop() > offset.top - topPadding) {
            $sidebar.addClass('fixed');
        } else {
            $sidebar.removeClass('fixed');
        }
    });


};

// Keep at the bottom
$(document).ready(function () {
    schemaEditor = new schemaEditor_class();
    schemaEditor.initPageEvents();
});