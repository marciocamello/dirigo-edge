CKEDITOR.plugins.add('insertimage', {
    icons: 'insertimage',
    init: function (editor) {

        editor.ui.addButton('InsertImage', {
            label: 'Insert Image',
            toolbar: 'insert',
            click: function (e) { }
        });

        // Detach the default click event on the Insert Image button
        // Click event will be replaced by Filebrowser events
        var $button = $(".cke_button__insertimage");

        $button.off('click');

        $button.fileBrowser(function (data) {
            if (data.type === 'image' && data.src) {
                editor.insertHtml('<img src="' + data.src + '" alt="' + data.alt + '" class="' + data.align + '">');
            } else {
                editor.insertHtml('<a href="' + data.href + '" title="' + data.title + '">' + data.text + '</a>');
            }
        });
    }
});