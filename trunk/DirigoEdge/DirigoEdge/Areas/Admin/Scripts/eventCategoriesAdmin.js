event_category_class = function() {

};

event_category_class.prototype.initPageEvents = function () {
    this.initAddCategoryEvent();
    this.initDeleteCategoryEvent();
    this.initConfirmCatDeleteEvent();
};

event_category_class.prototype.initConfirmCatDeleteEvent = function () {
    var self = this;

    // Confirm deleteion of category from cat table
    $("#ConfirmDeleteEventCategory").click(function() {
        var catIdToDelete = self.$catRowToDelete.find("td.id").text();
        
        var $container = $("#DeleteEventCategoryModal").find("div.wrapper");
        common.showAjaxLoader($container);
        $.ajax({
            url: "/EventCategory/DeleteCategory",
            type: "POST",
            data: {
                id: catIdToDelete
            },
            success: function (data) {

                // Notify user of success
                var noty_id = noty({ text: 'Category Successfully Deleted.', type: 'success', timeout: 2000 });

                // Remove the row
                self.$catRowToDelete.remove();

                // Hide loader
                common.hideAjaxLoader($container);

                // Close Modal
                $('#DeleteEventCategoryModal').trigger('reveal:close');
            },
            error: function (data) {
                // Close Modal
                common.hideAjaxLoader($container);
                $('#DeleteEventCategoryModal').trigger('reveal:close');
                var noty_id = noty({ text: 'There was an error processing your request.', type: 'error' });
            }
        });
    });
};

event_category_class.prototype.initDeleteCategoryEvent = function () {
    var self = this;
    self.$catRowToDelete;
    

    // Delete Category from category listing table
    $("#EventCategoriesTable td a.deleteCategoryButton").live("click", function () {
        var catId = $(this).attr("data-id");

        // Store the row to be removed so the dialog box can access is
        self.$catRowToDelete = $(this).closest("tr");
        
        // Set the dialog's box's text to give the user some context
        $("#popTitle").text("'" + self.$catRowToDelete.find("td.name").text() + "'");

        // Show confirmation pop up
        $("#DeleteEventCategoryModal").reveal();
    });
};

event_category_class.prototype.initAddCategoryEvent = function () {

    // Add Category Button
    $("#ConfirmAddEventCategory").click(function () {
        var name = $("#EventCategoryNameInput").attr("value");
        if (name.length < 1) {
            return false;
        }

        var $container = $("#AddEventCategoryModal").find("div.wrapper");
        common.showAjaxLoader($container);
        $.ajax({
            url: "/EventCategory/AddCategory",
            type: "POST",
            data: {
                name: name
            },
            success: function (data) {

                // Notify user of success
                var noty_id = noty({ text: 'Category Successfully Created.', type: 'success', timeout: 2000 });

                // Add the row
                $("#EventCategoriesTable").append('<tr><td class="id">' + data.id + '</td><td class="name">' + name + '</td><td><a data-id="' + data.id + '" href="javascript:void(0);" class="deleteCategoryButton button small">Delete</a></td></tr>');

                // Hide loader
                common.hideAjaxLoader($container);

                // Close Modal
                $('#AddEventCategoryModal').trigger('reveal:close');
            },
            error: function (data) {
                // Close Modal
                common.hideAjaxLoader($container);
                $('#AddEventCategoryModal').trigger('reveal:close');
                var noty_id = noty({ text: 'There was an error processing your request.', type: 'error' });
            }
        });
    });
};


// Keep at the bottom
$(document).ready(function () {
    eventCategoryAdmin = new event_category_class();
    eventCategoryAdmin.initPageEvents();
});