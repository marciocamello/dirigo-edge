media_class = function () {

    this.el = '.manageMedia';
    this.$el = $(this.el);

};

media_class.prototype.initPageEvents = function() {
    if (this.$el.length > 0) {
        this.initPageEvents();
        this.initDropzone();
    }
};

media_class.prototype.initPageEvents = function () {

    var self = this;

    $('#toggle-thumbnails', this.el).on('change', this.methods.toggleThumbnails).change();

    // Events to trigger add folder modal
    // and call method to send to server
    $('.create-media-folder').click(function () {
        $("#AddFolderModal").reveal();
        $('input, #AddFolderModal').focus();
        return false;
    });

    $('input', '#AddFolderModal').on('keyup', function (e) {
        var key = e.keyCode || e.which;
        if (key === 13) {
            self.methods.addMediaFolder();
        }
    });

    $('#ConfirmFolderAdd').on('click', this.methods.addMediaFolder);
    
    // Events to trigger delete folder
    // confirmation and call method to
    // send to server
    $('body').on('click', '.delete-media-folder', function () {
        var folder = $(this).closest('li, tr').attr('data-folder');
        $("#ConfirmFolderDelete").attr('data-folder', folder);
        $("#DeleteFolderModal").reveal();
        return false;
    });
    
    $('#ConfirmFolderDelete').on('click', this.methods.deleteMediaFolder);
    
    // Events to trigger delete file
    // confirmation and call method to
    // send to server
    self.$el.on('click', '.delete:not(.disabled)', function () {

        var file = $(this).attr('data-src'),
            $modal = $('#DeleteMediaModal');

        $modal.find('#ConfirmMediaDelete').attr('data-src', file);

        $modal.find('.file').text(file.split('/').pop());
        
        $modal.reveal();

    });
    
    $('#ConfirmMediaDelete').on('click', function () {

        var $button = $(this),
            data = {
                filename : $button.attr("data-src")
            };

        var $container = $('#DeleteMediaModal .content');
        common.showAjaxLoader($container);
        $.ajax({
            url : "/mediaupload/removefile/",
            type : "POST",
            dataType : 'json',
            contentType : 'application/json; charset=utf-8',
            data : JSON.stringify(data, null, 2),
            success : function (res) {

                if (res && res.success) {

                    noty({ text : 'File successfully deleted.', type : 'success', timeout : 3000 });

                    // If DataTables are missing, remove HTML from table
                    // otherwise use DataTables API
                    if (!window.oTable) {
                        $('.manageTable .delete[data-src="' + data.filename + '"]').closest('tr')[0].remove();
                    } else {
                        oTable.fnDeleteRow($('.manageTable .delete[data-src="' + data.filename + '"]').closest('tr')[0]);
                    }

                } else {

                    noty({ text : res.response, type : 'error', timeout : 3000 });

                }
                
                common.hideAjaxLoader($container);
                
                $('#DeleteMediaModal').trigger('reveal:close');
            }
        });

        return false;
    });

};

media_class.prototype.initZClip = function ($element) {
    return $element.zclip({
        path: '/scripts/jquery/plugins/zclip/ZeroClipboard.swf',
        copy: function () { return $(this).parent().find("input").val() },
        afterCopy: function () {
            $(this).parent().find("input").select();
        }
    });

};

media_class.prototype.methods = {
    toggleThumbnails: function () {

        var _this,
            $image = $('.thumbnail .image');

        if ($(this).prop('checked')) {
            $image.each(function () {
                _this = $(this);
                _this
                    .attr('style', 'background-image:url(' + _this.attr('data-thumb') + ')');
            });
        } else {
            $image
                .attr('style', '');
        }

    },
    
    addMediaFolder: function () {

        var $input     = $('#AddFolderModal').find('input').first(),
            folderName = $input.val(),
            data       = {
                folder : folderName
            };

        var $container = $('#AddFolderModal div.content');
        common.showAjaxLoader($container);
        $.ajax({
            url         : "/mediaupload/addfolder/",
            type        : "POST",
            dataType    : 'json',
            contentType : 'application/json; charset=utf-8',
            data        : JSON.stringify(data, null, 2),
            success     : function (res) {
                
                $input.val('');

                if (res && res.success) {
                    
                    $('.media-menu').prepend(
                        '<li data-folder="' + folderName + '">' +
                            '<a href="/admin/managemedia/' + folderName + '">' +
                                '<i class="fa fa-folder-open"></i>' +
                                '<span>' + folderName + '</span>' +
                                '<div class="delete-media-folder">' +
                                    '<i class="fa fa-times"></i>' +
                                '</div>' +
                            '</a>' +
                        '</li>'
                    );
                    
                    $('#AddFolderModal').trigger('reveal:close');
                    
                } else {
                    
                    noty({ text: res.error, type: 'error', timeout: 3000 });
                    
                }

                common.hideAjaxLoader($container);
            }
        });

        return false;
    },
    
    deleteMediaFolder : function () {

        var $this  = $(this),
            folder = $(this).attr('data-folder'),
            data   = {
                folder : folder
            };

        $.ajax({
            url         : "/mediaupload/deletefolder/",
            type        : "POST",
            dataType    : 'json',
            contentType : 'application/json; charset=utf-8',
            data        : JSON.stringify(data, null, 2),
            success     : function (res) {
                
                $('#DeleteFolderModal').trigger('reveal:close');

                if (res && res.success) {
                    
                    // If user is in folder that just got deleted, redirect
                    // them to the dashboard. Otherwise just remove
                    // the folder.
                    var rgx = new RegExp('^(?:/admin)\/managemedia\/(?:' + folder + '/?)$', 'ig');
                    if (location.pathname.match(rgx)) {
                        window.location = '/admin';
                    } else {
                        $('.dropdown [data-folder="' + folder + '"], tr[data-folder="' + folder + '"]').remove();
                    }

                } else {
                    noty({ text: res.error, type: 'error', timeout: 3000 });
                }

            }
        });

        return false;
        
    }
};

// Keep at the bottom
$(document).ready(function () {
    media = new media_class();
    media.initPageEvents();
});
