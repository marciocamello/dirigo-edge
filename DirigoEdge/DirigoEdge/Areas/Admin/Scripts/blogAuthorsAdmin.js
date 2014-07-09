blog_authors_class = function () {

};

blog_authors_class.prototype.initPageEvents = function() {

    var self = this;

    this.sortAuthors();

    $("#CreateAuthorButton").click(function () {
        var data = {
            user: {
                DisplayName: $("#NewUserName").attr("value"),
                UserName: $("#NewUserName").attr("value"),
                UserImageLocation: $("#NewUserImage").attr("value"),
                IsActive: $("#NewUserIsActiveBox").hasClass("checked")
            }
        };

        var $container = $("#NewAuthorModal div.content");
        common.showAjaxLoader($container);
        $.ajax({
            url: "/Admin/AddBlogUser",
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data, null, 2),
            success: function (data) {
                // Close the dialog box
                $('#NewAuthorModal').trigger('reveal:close');

                common.hideAjaxLoader($container);

                //Refresh the inner content to show the new user
                self.refreshAuthorTable(noty({ text: 'Author Successfully Created.', type: 'success', timeout: 3000 }));
            },
            error: function (data) {
                $('#NewAuthorModal').trigger('reveal:close');
                common.hideAjaxLoader($container);
                var noty_id = noty({ text: 'There was an error processing your request.', type: 'error' });
            }
        });
    });

    // Manage Users Edit User
    $("#Main div.manageAuthors a.editUser.button").live("click", function () {
        var $row = $(this).parent().parent();
        var $el = $(this);
        self.EditUserId = $(this).attr("data-id");
        self.EditUserDisplayName = $row.find("td.displayName").text();
        self.EditUserIsActive = $row.find("td.isActive").text();
        self.EditUserImageLoc = $row.find("td.imageLocation img").attr("src");

        // Set the modal properties
        $("#ModUserName").attr("value", self.EditUserDisplayName);
        $("#ModUserImageLocation").attr("value", self.EditUserImageLoc);
        if ($row.find("td.isActive").text() == "True") {
            // Do Checkbox
            $("#ModUserIsActiveBoxOver").addClass("checked");
        }
        else {
            $("#ModUserIsActiveBoxOver").removeClass("checked");
        }

        $("#ModifyAuthorModal").reveal();
    });

    // Submit edit author
    $("#ModifyAuthorButton").click(function () {

        var data = {
            user: {
                DisplayName: $("#ModUserName").attr("value"),
                UserImageLocation: $("#ModUserImageLocation").attr("value"),
                IsActive: $("#ModUserIsActiveBoxOver").hasClass("checked"),
                UserID: self.EditUserId,
            }
        };

        var $container = $("#ModifyAuthorModal > div.content");
        common.showAjaxLoader($container);
        $.ajax({
            url: "/Admin/ModifyBlogUser",
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data, null, 2),
            success: function (data) {

                common.hideAjaxLoader($container);

                // Close the dialog box
                $('#ModifyAuthorModal').trigger('reveal:close');

                //Refresh the inner content to show the new user
                self.refreshAuthorTable(noty({ text: 'User Successfully Modified.', type: 'success', timeout: 3000 }));
            },
            error: function (data) {
                common.hideAjaxLoader($container);
                $('#ModifyAuthorModal').trigger('reveal:close');
                var noty_id = noty({ text: 'There was an error processing your request.', type: 'error' });
            }
        });
    });

    // Delete author and confirmation
    $("#Main div.manageAuthors a.deleteUser.button").live("click", function () {
        var $row = $(this).parent().parent();
        var $el = $(this);
        self.EditUserId = $(this).attr("data-id");
        self.EditUserDisplayName = $row.find("td.displayName").text();
        $("#DelUserName").text(self.EditUserDisplayName);
        $("#DeleteAuthorModal").reveal();
    });

    // Submit Delete Author
    $("#DeleteAuthorButton").click(function () {
        var data = { user: { UserID: self.EditUserId } };

        $.ajax({
            url: "/Admin/DeleteBlogUser",
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data, null, 2),
            success: function (data) {
                // Close the dialog box
                $('#DeleteAuthorModal').trigger('reveal:close');

                blog_authors.refreshAuthorTable(noty({ text: 'Author Successfully Deleted.', type: 'success', timeout: 3000 }));
            },
            error: function (data) {
                $('#ModifyUserModal').trigger('reveal:close');
                var noty_id = noty({ text: 'There was an error processing your request.', type: 'error' });
            }
        });
    });

    // Close Delete User Modal
    $("#CancelDeleteButton").click(function () {
        $('#DeleteUserModal').trigger('reveal:close');
    });
};

blog_authors_class.prototype.refreshAuthorTable = function (fSuccess) {
    //Refresh the inner content to show the new user

    var $container = $("#ManageAuthorTableContainer");
    common.showAjaxLoader($container);

    $("#ManageAuthorTableContainer").load("/admin/manageblogauthors/ #ManageAuthorTable", function (data) {
        var noty_id = fSuccess;

        // Sort the table again since the html has changed
        blog_authors.sortAuthors();

        common.hideAjaxLoader($container);
    });
};

blog_authors_class.prototype.sortAuthors = function () {
    $("#ManageAuthorTable").dataTable({
        "iDisplayLength": 25,
        "aoColumnDefs": [
            { "bSortable": false, "aTargets": ["actions"] } // No Sorting on actions
        ],
        "aaSorting": [[1, "desc"]] // Sort by User Name date on load
    });
};

// Keep at the bottom
$(document).ready(function () {
    blog_authors = new blog_authors_class();

    if ($("#ManageAuthorTableContainer").length > 0) {
        blog_authors.initPageEvents();
    }
});