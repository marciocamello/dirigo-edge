/// ===========================================================================================
/// User Management
/// ===========================================================================================

user_class = function() {

};

user_class.prototype.initPageEvents = function() {
    // Save delegate resources by only triggering on the correct page
    if ($("#ManageUserTable").length > 0) {
        this.manageUserAdminEvents();
        this.manageChangePasswordEvents();
        this.sortUsers();
    }
};

user_class.prototype.sortUsers = function() {
    $("#ManageUserTable").dataTable({
        "iDisplayLength": 25,
        "aoColumnDefs": [
            { "bSortable": false, "aTargets": ["actions"] } // No Sorting on actions
        ],
        "aaSorting": [[1, "desc"]] // Sort by User Name date on load
    });
};

user_class.prototype.manageUserAdminEvents = function() {
    var self = this;

    // Add User click event
    $("#CreateUserButton").click(function() {
        var data = {
            user: {
                DisplayName: $("#NewUserName").attr("value"),
                UserName: $("#NewUserName").attr("value"),
                UserImageLocation: $("#NewUserImage").attr("value"),
                IsActive: $("#NewUserIsActiveBox").hasClass("checked"),
                Password: $("#NewUserPassword").attr("value"),
                Roles: []
            }
        };
        
        // Add the Roles
        $("#NewUserModal div.roleListing input[type=checkbox]").each(function () {

            if ($(this).is(":checked")) {
                var oRole = {
                    RoleName: $(this).data("role"),
                };

                data.user.Roles.push(oRole);
            }
        });

        var $container = $("#NewUserModal div.content");
        common.showAjaxLoader($container);
        $.ajax({
            url: "/Admin/AddUser",
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data, null, 2),
            success: function(data) {
                // Close the dialog box
                $('#NewUserModal').trigger('reveal:close');

                common.hideAjaxLoader($container);

                //Refresh the inner content to show the new user
                self.refreshUserTable(noty({ text: 'User Successfully Created.', type: 'success', timeout: 3000 }));
            },
            error: function(data) {
                $('#NewUserModal').trigger('reveal:close');
                common.hideAjaxLoader($container);
                var noty_id = noty({ text: 'There was an error processing your request.', type: 'error' });
            }
        });
    });

    // Manage Users Edit User
    $("#Main div.manageUsers a.editUser.button").live("click", function() {
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
        
        // Set the Roles
        $("#ModifyUserModal div.roleListing input[type=checkbox]").each(function () {
            var role = $(this).data("role");

            if ($row.find("td.roles:contains(" + role + ")").length > 0) {
                $(this).prop('checked', true).next().addClass("checked");
            }
            else {
                $(this).prop('checked', false).next().removeClass("checked");
            }
        });

        $("#ModifyUserModal").reveal();
    });

    // Submit edit user
    $("#ModifyUserButton").click(function () {

        // populate roles
        var roles = [];
        $(".roleListing input[type='checkbox']:checked").each(function () {
            var oRole = {
                RoleName: $(this).data("role"),
            };
            
            roles.push(oRole);
        });


        var data = {
            user: {
                UserName: $("#ModUserName").attr("value"),
                UserImageLocation: $("#ModUserImageLocation").attr("value"),
                IsActive: $("#ModUserIsActiveBoxOver").hasClass("checked"),
                UserID: self.EditUserId,
                Roles: roles
            }
        };

        var $container = $("#ModifyUserModal > div.content");
        common.showAjaxLoader($container);
        $.ajax({
            url: "/Admin/ModifyUser",
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data, null, 2),
            success: function (data) {
                
                common.hideAjaxLoader($container);
                
                // Close the dialog box
                $('#ModifyUserModal').trigger('reveal:close');

                //Refresh the inner content to show the new user
                self.refreshUserTable(noty({ text: 'User Successfully Modified.', type: 'success', timeout: 3000 }));
            },
            error: function (data) {
                common.hideAjaxLoader($container);
                $('#ModifyUserModal').trigger('reveal:close');
                var noty_id = noty({ text: 'There was an error processing your request.', type: 'error' });
            }
        });
    });

    // Delete user and confirmation
    $("#Main div.manageUsers a.deleteUser.button").live("click", function() {
        var $row = $(this).parent().parent();
        var $el = $(this);
        self.EditUserId = $(this).attr("data-id");
        self.EditUserDisplayName = $row.find("td.displayName").text();
        $("#DelUserName").text(self.EditUserDisplayName);
        $("#DeleteUserModal").reveal();
    });

    // Submit Delete user
    $("#DeleteUserButton").click(function() {
        var data = {
            user: {
                UserID: self.EditUserId
            }
        };

        $.ajax({
            url: "/Admin/DeleteUser",
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data, null, 2),
            success: function(data) {
                // Close the dialog box
                $('#DeleteUserModal').trigger('reveal:close');

                self.refreshUserTable(noty({ text: 'User Successfully Deleted.', type: 'success', timeout: 3000 }));
            },
            error: function(data) {
                $('#ModifyUserModal').trigger('reveal:close');
                var noty_id = noty({ text: 'There was an error processing your request.', type: 'error' });
            }
        });
    });

    // Close Delete User Modal
    $("#CancelDeleteButton").click(function() {
        $('#DeleteUserModal').trigger('reveal:close');
    });
};

user_class.prototype.manageChangePasswordEvents = function () {
    var self = this;

    $("#ChangeUserPassword").click(function () {
        // set up modal info before showing modal
        $("#ChngPasswdUname").text(self.EditUserDisplayName);

        //
        $("#ChangePasswordModal").reveal();
    });
    
    // Change Password Submit
    $("#ChangeUserPasswordButton").click(function() {
        //self.EditUserId
        var newPassword = $("#NewUserChangePassword").val();
        var newPasswordRepeated = $("#RepeatNewUserChangePassword").val();

        // Passwords must match
        if (newPassword != newPasswordRepeated) {
            alert("Passwords must match");
            return;
        }
        
        // Min lenfth on password
        if (newPassword.length < 1) {
            alert("Passwords much be at least one character long.");
            return;
        }
        
        // We're good - send off the ajax call
        var $container = $("#ChangePasswordModal .content");
        common.showAjaxLoader($container);
        var data = { user: { UserID: self.EditUserId }, newPassword: newPassword };
        $.ajax({
            url: "/Admin/ChangeUserPassword",
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data, null, 2),
            success: function (data) {
                // Close the dialog box
                common.hideAjaxLoader($container);
                $('#ChangePasswordModal').trigger('reveal:close');

                self.refreshUserTable(noty({ text: 'Password Successfully Updated.', type: 'success', timeout: 3000 }));
            },
            error: function (data) {
                common.hideAjaxLoader($container);                
                var noty_id = noty({ text: 'There was an error processing your request.', type: 'error' });
            }
        });
    });
};

user_class.prototype.refreshUserTable = function(fSuccess) {
    //Refresh the inner content to show the new user
    $("#ManageUserTableContainer").load("/Admin/ManageUsers #ManageUserTable", function(data) {
        var noty_id = fSuccess;

        // Sort the table again since the html has changed
        user.sortUsers();
    });
};

// Keep at the bottom
$(document).ready(function () {
    user = new user_class();
    user.initPageEvents();
});