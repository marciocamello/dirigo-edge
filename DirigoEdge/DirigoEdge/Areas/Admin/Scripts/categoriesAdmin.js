category_class = function() {

};

category_class.prototype.initPageEvents = function() {
    this.initAddCategoryEvent();
    this.initDeleteCategoryEvent();
    this.initConfirmCatDeleteEvent();
};

category_class.prototype.initConfirmCatDeleteEvent = function() {
    var self = this;

    // Confirm deleteion of category from cat table
    $("#ConfirmDeleteCategory").click(function() {
        var catIdToDelete = self.$catRowToDelete.find("td.id").text();
        
        var $container = $("#DeleteCategoryModal").find("div.wrapper");
        common.showAjaxLoader($container);
        $.ajax({
            url: "/Category/DeleteCategory",
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
                $('#DeleteCategoryModal').trigger('reveal:close');
            },
            error: function (data) {
                // Close Modal
                common.hideAjaxLoader($container);
                $('#DeleteCategoryModal').trigger('reveal:close');
                var noty_id = noty({ text: 'There was an error processing your request.', type: 'error' });
            }
        });
    });
};

category_class.prototype.initDeleteCategoryEvent = function () {
    var self = this;
    self.$catRowToDelete;
    

    // Delete Category from category listing table
    $("#CategoriesTable td a.deleteCategoryButton").live("click", function () {
        var catId = $(this).attr("data-id");

        // Store the row to be removed so the dialog box can access is
        self.$catRowToDelete = $(this).closest("tr");
        
        // Set the dialog's box's text to give the user some context
        $("#popTitle").text("'" + self.$catRowToDelete.find("td.name").text() + "'");

        // Show confirmation pop up
        $("#DeleteCategoryModal").reveal();
    });
};

category_class.prototype.initAddCategoryEvent = function() {

    // Add Category Button
    $("#ConfirmAddCategory").click(function () {
        var name = $("#CategoryNameInput").attr("value");
        if (name.length < 1) {
            return false;
        }

        var $container = $("#AddCategoryModal").find("div.wrapper");
        common.showAjaxLoader($container);
        $.ajax({
            url: "/Category/AddCategory",
            type: "POST",
            data: {
                name: name
            },
            success: function (data) {

                // Notify user of success
                var noty_id = noty({ text: 'Category Successfully Created.', type: 'success', timeout: 2000 });

                // Add the row
                $("#CategoriesTable").append('<tr><td class="id">' + data.id + '</td><td class="name">' + name + '</td><td><a data-id="' + data.id + '" href="javascript:void(0);" class="deleteCategoryButton button small">Delete</a></td></tr>');

                // Hide loader
                common.hideAjaxLoader($container);

                // Close Modal
                $('#AddCategoryModal').trigger('reveal:close');
            },
            error: function (data) {
                // Close Modal
                common.hideAjaxLoader($container);
                $('#AddCategoryModal').trigger('reveal:close');
                var noty_id = noty({ text: 'There was an error processing your request.', type: 'error' });
            }
        });
    });
};


// Keep at the bottom
$(document).ready(function () {
    categoryAdmin = new category_class();
    categoryAdmin.initPageEvents();
});