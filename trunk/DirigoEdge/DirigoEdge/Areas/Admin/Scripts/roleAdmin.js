/// ===========================================================================================
/// User Role Management
/// ===========================================================================================

role_class = function () {

};

role_class.prototype.initPageEvents = function () {
    // Save delegate resources by only triggering on the correct page
    if ($("#ManageUserRolesTable").length > 0) {

        this.manageUserRoleAdminEvents();

        this.initPermissionEvents();

        this.initEditUsersEvents();

        this.initRegistrationEvents();

        this.sortRoles();
    }
};

role_class.prototype.manageUserRoleAdminEvents = function () {
    var self = this;
    
    // Create User Role
    $("#CreateUserRoleButton").click(function() {

        var roleName = $("#RoleName").val();
        if (roleName.length < 1) {
            alert("Please enter a Role Name.");
            return false;
        }
        
        var data = {
            role: {
                RoleName: roleName,
                Permissions: {}
            }
        };

        // Add the Roles
        $("#NewUserRoleModal ul.rolePermissionsList input[type=checkbox]").each(function () {
             data.role.Permissions[$(this).data("key")] = $(this).is(":checked");
        });
        
        var $container = $("#NewUserRoleModal div.content");
        common.showAjaxLoader($container);
        $.ajax({
            url: "/Admin/AddUserRole",
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data, null, 2),
            success: function (data) {
                // Close the dialog box
                $('#NewUserRoleModal').trigger('reveal:close');

                common.hideAjaxLoader($container);

                //Refresh the inner content to show the new user
                self.refreshUserRoleTable(noty({ text: 'User Successfully Created.', type: 'success', timeout: 3000 }));
            },
            error: function (data) {
                $('#NewUserModal').trigger('reveal:close');
                common.hideAjaxLoader($container);
                var noty_id = noty({ text: 'There was an error processing your request.', type: 'error' });
            }
        });
    });
    
    // Delete user role and confirmation
    $("#ManageUserRolesTable a.deleteUserRole.button").live("click", function () {
        var $row = $(this).parent().parent();
        var $el = $(this);
        self.EditUserRoleId = $(this).attr("data-id");
        self.EditUserRoleDisplayName = $row.find("td.roleName").text();
        $("#DelUserRole").text(self.EditUserRoleDisplayName);
        $("#DeleteUserRoleModal").reveal();
    });

    // Submit Delete user
    $("#DeleteUserRoleButton").click(function () {
        var data = {
            role: {
                RoleId: self.EditUserRoleId
            }
        };

        var $container = $("#DeleteUserRoleModal > .content");
        common.showAjaxLoader($container);
        $.ajax({
            url: "/Admin/DeleteRole",
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data, null, 2),
            success: function (data) {
                // Close the dialog box
                common.hideAjaxLoader($container);
                $('#DeleteUserRoleModal').trigger('reveal:close');

                self.refreshUserRoleTable(noty({ text: 'User Successfully Deleted.', type: 'success', timeout: 3000 }));
            },
            error: function (data) {
                common.hideAjaxLoader($container);
                $('#DeleteUserRoleModal').trigger('reveal:close');
                noty({ text: 'There was an error processing your request.', type: 'error' });
            }
        });
    });

    // Close Delete User Modal
    $("#CancelDeleteButton").click(function () {
        $('#DeleteUserRoleModal').trigger('reveal:close');
    });        
};

role_class.prototype.initRegistrationEvents = function () {
    var self = this;
    
    $("#ManageUserRolesTable a.regCodes").live("click", function () {
        var $row = $(this).parent().parent();
        self.EditUserRoleId = $(this).attr("data-id");
        self.EditUserRoleDisplayName = $row.find("td.roleName").text();

        $("#RegCodeInput").val($(this).attr("data-code"));

        $("#EditRoleRegistrationModal").reveal();
    });

    // Save RegCode
    $("#ModifyRegCodeButton").click(function() {
        var data = {
            role: {
                RoleId : self.EditUserRoleId,
                RegistrationCode : $("#RegCodeInput").val()
            }
        };

        var $container = $("#EditRoleRegistrationModal > .content");
        common.showAjaxLoader($container);
        $.ajax({
            url: "/Admin/UpdateRoleCode",
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data, null, 2),
            success: function (data) {
                // Close the dialog box
                common.hideAjaxLoader($container);
                $('#EditRoleRegistrationModal').trigger('reveal:close');

                self.refreshUserRoleTable(noty({ text: 'Registration Code Updated.', type: 'success', timeout: 3000 }));
            },
            error: function (data) {
                common.hideAjaxLoader($container);
                $('#EditRoleRegistrationModal').trigger('reveal:close');
                noty({ text: 'There was an error processing your request.', type: 'error' });
            }
        });
    });
};


role_class.prototype.initPermissionEvents = function () {
    var self = this;
    
    // Show user permissions
    $("#ManageUserRolesTable a.showPermissions").live("click", function () {
        var $row = $(this).parent().parent();
        self.EditUserRoleId = $(this).attr("data-id");
        self.EditUserRoleDisplayName = $row.find("td.roleName").text();
        $("#EditUserRole").text(self.EditUserRoleDisplayName);

        // Set the permissions to edit accordingly
        var nCurrentRolePermissions = $row.find("td.permsList").text().split(", ");

        $("#EditUserRolePermissionsModal ul.rolePermissionsList li").each(function () {
            var currentRole = $(this).find("span.key").text();

            if ($.inArray(currentRole, nCurrentRolePermissions) != -1) {
                $(this).find("span.checkbox").addClass("checked");
            }
            else {
                $(this).find("span.checkbox").removeClass("checked");
            }
        });

        $("#EditUserRolePermissionsModal").reveal();
    });

    // Edit Role Permissions
    $("#ModifyPermissionsButton").click(function () {

        var data = {
            role: {
                RoleName: self.EditUserRoleDisplayName,
                RoleId: self.EditUserRoleId,
                Permissions: {}
            }
        };

        // Add the roles
        $("#EditUserRolePermissionsModal ul.rolePermissionsList li").each(function () {
            var key = $(this).find("input[type='checkbox']").data("key");
            var isChecked = $(this).find(".checkbox").hasClass("checked");

            data.role.Permissions[key] = isChecked;
        });

        var $container = $("#EditUserRolePermissionsModal div.content");
        common.showAjaxLoader($container);
        $.ajax({
            url: "/Admin/ModifyRolePermissions",
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data, null, 2),
            success: function (data) {

                common.hideAjaxLoader($container);

                // Close the dialog box
                $('#EditUserRolePermissionsModal').trigger('reveal:close');

                //Refresh the inner content to show the new user
                self.refreshUserRoleTable(noty({ text: 'User Successfully Modified.', type: 'success', timeout: 3000 }));
            },
            error: function () {
                common.hideAjaxLoader($container);
                $('#EditUserRolePermissionsModal').trigger('reveal:close');
                noty({ text: 'There was an error processing your request.', type: 'error' });
            }
        });
    });
};


role_class.prototype.initEditUsersEvents = function () {
    var self = this;

    // Show modal
    $("#ManageUserRolesTable a.showUsers").live("click", function () {
        
        var $row = $(this).parent().parent();
        self.EditUserRoleId = $(this).attr("data-id");
        self.EditUserRoleDisplayName = $row.find("td.roleName").text();
        $("#EditUsersNRole").text(self.EditUserRoleDisplayName);
        
        // Hide how many users changed
        $("#UsersChangedContainer").hide();

        // Show modal
        $("#EditUsersInRoleModal").reveal();
        
        // Ajax in the Users for the current role
        var $container = $("#EditUsersInRoleModal > div.content");
        common.showAjaxLoader($container);
        var data = { RoleName: self.EditUserRoleDisplayName };
        $("#UserListing").load("/Admin/GetRoleUsers/", data, function () {
            
            // Success
            common.hideAjaxLoader($container);
        });
    });
    
    // Toggle checkboxes
    $("#UserListing ul.userList li").live("click", function (e) {

        // If we clicked on the checkbox, proceed as usual 
        if ($(e.target).attr("type") == "checkbox") {
            return;
        }

        // Otherise toggle the checkbox
        var $check = $(this).find("input[type=checkbox]");
        $check.prop("checked", !$check.prop("checked"));
        $check.trigger("change");
    });
    
    // Toggling checkboxes changes user changed count and marks for update
    $("#UserListing ul.userList li input[type=checkbox]").live("change", function(e) {
        e.preventDefault();
        
        // Update user count
        $(this).closest("li").addClass("changed");

        var changedCount = $("#UserListing ul.userList li.changed").length;

        $("#UsersChangedContainer").show();
        $("#UserModCount").text(changedCount);
    });

    // Add Remove User Submission
    $("#ModifyUserInRoleButton").click(function () {

        // compile the user count
        var addUsers = [];
        var removeUsers = [];

        $("#UserListing ul.userList li.changed").each(function () {

            var $checked = $(this).find("input[type=checkbox]").is(":checked");
            
            if ($checked) {
                addUsers.push($(this).attr("data-id"));
            }
            else {
                removeUsers.push($(this).attr("data-id"));
            }
        });

        var data = {
            RemoveUsers: removeUsers,
            AddUsers: addUsers,
            RoleID: self.EditUserRoleId
        };
        var $container = $("#EditUsersInRoleModal div.content");
        common.showAjaxLoader($container);
        $.ajax({
            url: "/Admin/ModifyUsersInRole",
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data, null, 2),
            success: function (data) {

                common.hideAjaxLoader($container);

                // Close the dialog box
                $('#EditUsersInRoleModal').trigger('reveal:close');

                //Refresh the inner content to show the new user
                self.refreshUserRoleTable(noty({ text: 'Users Successfully Modified.', type: 'success', timeout: 3000 }));
            },
            error: function () {
                common.hideAjaxLoader($container);
                $('#EditUsersInRoleModal').trigger('reveal:close');
                noty({ text: 'There was an error processing your request.', type: 'error' });
            }
        });

    });

};

role_class.prototype.sortRoles = function () {
    $("#ManageUserRolesTable").dataTable({
        "iDisplayLength": 25,
        "aoColumnDefs": [
            { "bSortable": false, "aTargets": ["actions"] } // No Sorting on actions
        ],
        "aaSorting": [[0, "asc"]] // Sort by Role Name date on load
    });
};

role_class.prototype.refreshUserRoleTable = function (fSuccess) {

    var $container = $("#ManageUserTableContainer");
    
    common.showAjaxLoader($container);
    //Refresh the inner content to show the new user
    $("#ManageUserTableContainer").load("/admin/manageuserroles/ #ManageUserRolesTable", function (data) {
        var noty_id = fSuccess;

        // Sort the table again since the html has changed
        roleAdmin.sortRoles();
        
        common.hideAjaxLoader($container);
    });
};

// Keep at the bottom
$(document).ready(function () {
    roleAdmin = new role_class();
    roleAdmin.initPageEvents();
});