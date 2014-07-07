/*
 *  FileBrowser - v2.0.1
 */

; (function ($, window, document, undefined) {

    var pluginName = "fileBrowser",
        defaults = {
            direcory: null,
            isEditor: true,
            createModal: true,
            newModalId: null
        };

    function Plugin(element, options, callback) {

        var self = this;

        if (!callback && typeof options === 'function') {
            callback = options;
            options = null;
        }

        this.element = element;
        this.callback = callback;
        this.settings = $.extend({}, defaults, options);
        this._defaults = defaults;
        this._name = pluginName;
        this.initialized = false;
        this.clickedElem = this.element;

        // Create objects for Ajax calls so we can abort them later
        this.browserAjax = null;
        this.folderAjax = null;

        this.$el = this.settings.createModal ? this.createModal() : $(this.element);

        // Each modal needs a unique Dropzone ID
        this.dropzoneId = this.$el.attr('id') + '_dropzone';

        // If createModal is set to false, it's assumed that this
        // plugin was called by a click event (i.e. CKEditor button)
        if (this.settings.createModal) {
            $(this.element).on('click', function () {
                self.clickedElem = this;
                self.init();
            });
        } else {
            self.init();
        }
    }

    Plugin.prototype.init = function () {
        if (this.initialized) {
            this.show();
        } else {
            this.show();
            common.showAjaxLoader(this.$el);
            this.initFileBrowserData(this);
        }

        this.initialized = true;
    };

    Plugin.prototype.show = function () {
        this.$el.reveal();
        this.events();
    };

    Plugin.prototype.events = function () {
        var self = this;

        // Click on  a folder in the folder list
        // Load list of files in that folder
        self.$el.on('click', '.directory', function () {

            var directory = $(this).attr('href');

            // Abort any existing folder AJAX calls
            self.folderAjax.abort();

            directory = directory.substr(directory.lastIndexOf('/') + 1);

            // Unhighlight all folders
            $('a', '.folders').removeClass('active');

            self.loadDirectoryFiles(directory);

            return false;

        });

        // Click on a file in the file list
        // Show thumbnail and settings, collapse for other files
        self.$el.on('click', '.files .files__list .file', function () {

            self.resetFileList();

            // Reveal thumbnail and settings toolbar
            $(this)
                .closest('li')
                .addClass('active')
                .find('.settings')
                .addClass('active');

            return false;

        });

        // Click on Insert on a file
        // Generate file object, hide modal, execute callback
        self.$el.on('click', '.insert', function () {

            var fileObject = {},
                $file = $(this).closest('.file-container'),
                isImage = $file.data('icon') === 'picture-o',
                media = {
                    path: $file.data('path'),
                    alt: $file.find('.alt-text').val(),
                    align: $('input[name=align]:checked', $file).val(),
                    linkText: $file.find('.link-text').val()
                };

            // Warn if user tries to insert a document without link text
            if (self.settings.isEditor && !isImage && !media.linkText) {
                alert('You didn\'t enter a link text');
                $file.find('.link-text').closest('label').addClass('input-error');
                return false;
            }

            // Create object to pass to callback
            if (isImage) {
                fileObject = {
                    type: 'image',
                    src: media.path,
                    alt: media.alt,
                    align: media.align,
                    elem: self.clickedElem
                };
            } else {
                fileObject = {
                    type: 'document',
                    href: media.path,
                    title: media.alt,
                    text: media.align,
                    elem: self.clickedElem
                };
            }

            // Unbind events, close modal
            self.terminateModal();

            if (self.callback && typeof self.callback === 'function') {
                self.callback(fileObject);
            }

        });

        // Click on the close button
        // Unbind all events, close the modal
        self.$el.on('click', '.close-reveal-modal', function () {
            self.terminateModal();
        });

    };

    Plugin.prototype.createModal = function () {

        return $("<div/>", {
            "id": this.settings.newModalId || "Modal_" + Math.random().toString(36).substring(2, 12),
            "class": "reveal-modal large file-browser"
        }).appendTo("body");

    };

    Plugin.prototype.initFileBrowserData = function () {

        var self = this;

        self.browserAjax = $.ajax({
            url: "/mediaupload/filebrowser/",
            type: "GET",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (res) {
                if (res && res.success) {
                    self.showFileBrowser(res);
                }
            }
        });

    };

    Plugin.prototype.showFileBrowser = function (data) {

        var self = this;

        self.$el
            .html(data.html);
        common.hideAjaxLoader(self.$el);

        if (self.settings.directory) {
            self.loadDirectoryFiles(self.settings.directory);
        } else {
            self.loadDirectoryFiles($('a', '.folders').first().data('directory'));
        }

    };

    Plugin.prototype.loadDirectoryFiles = function (dir) {

        var self = this;

        var $container = $(".file-browser.open > div.browser");
        common.showAjaxLoader($container);
        self.folderAjax = $.ajax({
            url: "/mediaupload/filebrowser/" + dir,
            type: "GET",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (res) {

                var $html, $noFiles;

                if (res && res.html) {
                    // Filebrowser partial
                    $html = $(res.html);
                    self.$files = $html.find('.file-container');
                    $noFiles = $html.find('.files');

                    $('.files', self.$el).html($noFiles.html());
                    
                    if (self.settings.isEditor) {
                        $('.files__list .file .insert').hide();
                    } else {
                        $('.files__list .settings .insert').hide();
                    }                    

                    // Add generated Dropzone ID
                    $('.files', self.$el).attr('id', self.dropzoneId);

                    // Reveal the list of files
                    $('.files__list', self.$el).addClass('active');

                    // Unhighlight all folders
                    $('a', '.folders').removeClass('active');

                    $('a[data-directory="' + dir + '"]', '.folders').addClass('active');

                    if (self.fileDropzone) {
                        self.fileDropzone.options.params.category = dir;
                    } else {
                        self.fileDropzone = new Dropzone('#' + self.dropzoneId, {
                            clickable: '.toolbar',
                            url: "/mediaupload/uploadfile/",
                            params: {
                                category: dir
                            },
                            init: function () {
                                this.on("success", function (file, data) {
                                    if (data && data.success) {
                                        self.loadDirectoryFiles(this.options.params.category);
                                    } else {
                                        noty({ text: data.error, type: 'error', timeout: 3000 });
                                    }
                                });
                            }
                        });
                    }
                }
                common.hideAjaxLoader($container);
            }
        });

    };    

    Plugin.prototype.terminateModal = function () {

        this.$el.off('click');
        this.$el.trigger('reveal:close');

    };

    Plugin.prototype.resetFileList = function () {

        // Hide thumbnails and settings toolbar for
        // previously clicked files
        $('.settings', '.files__list').removeClass('active');
        $('li', '.files__list').removeClass('active');

        // Reset radio buttons
        $('input[name=align]').prop('checked', false);

        // Remove any error classes
        $('.input-error').removeClass('input-error');

    };

    $.fn[pluginName] = function (options, callback) {

        // This allows the plugin to reuse modals once they've
        // been instantiated. Mostly for CKEditor.
        if (!$.data(this, "plugin_" + pluginName)) {
            $.data(this, "plugin_" + pluginName, new Plugin(this, options, callback));
        } else {
            $.data(this, "plugin_" + pluginName).show();
        }

        return this;
    };

})(jQuery, window, document);