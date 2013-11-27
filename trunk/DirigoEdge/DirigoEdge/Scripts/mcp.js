mcp_class = function () {


};

mcp_class.prototype.initPageEvents = function() {
    // Lazy Load background images
    $("div.lazyLoad").removeClass("lazyLoad");
    
    if ($("#ContactForm").length > 0) {
        this.initContactPageEvents();
    }

    // Generic form handler for custom forms
    $("form.customForm").live("submit", function (e) {

        // prevent default if not ajax form
        e.preventDefault();

        var $modal = $("#ContactSuccessModal");

        $(this).validate({
            submitHandler: function($form) {

                var $container = $($form);

                common.showAjaxLoader($container);

                $($form).ajaxSubmit({
                    success: function() {
                        $modal.reveal();
                        common.hideAjaxLoader($container);
                    },
                    error: function () {
                        common.hideAjaxLoader($container);
                    }
                });

                common.hideAjaxLoader($el);

                return false;
            }
        });

        // Kick off the submit
        $(this).submit();
    });
};

mcp_class.prototype.initContactPageEvents = function () {
    // Contact Form Submit
    this.initValidateForm($("#ContactForm"), $('#ContactSuccessModal'));
};

// Generic Form Validation Handler
// Will validate form and do ajax get if successful
mcp_class.prototype.initValidateForm = function ($form, $modal) {
    $form.validate({
        submitHandler: function (form) {
            var $el = $form;
            common.showAjaxLoader($el);
            $(form).ajaxSubmit({
                success: function () {
                    $modal.reveal();
                    common.hideAjaxLoader($el);
                }
            });

            return false;
        }
    });
};

// Keep at the bottom
$(document).ready(function () {
    mcp = new mcp_class();
    mcp.initPageEvents();
});