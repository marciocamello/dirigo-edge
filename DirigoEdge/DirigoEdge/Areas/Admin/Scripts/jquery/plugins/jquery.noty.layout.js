; (function ($) {
    $.noty.layouts.top = {
		name: 'top',
		options: {},
		container: {
			object: '<ul id="noty_top_layout_container" />',
			selector: 'ul#noty_top_layout_container',
			style: function() {
				$(this).css({
					top: 0,
					left: '5%',
					position: 'fixed',
					width: '90%',
					height: 'auto',
					margin: 0,
					padding: 0,
					listStyleType: 'none',
					zIndex: 9999999
				});
			}
		},
		parent: {
			object: '<li />',
			selector: 'li',
			css: {}
		},
		css: {
			display: 'none'
		},
		addClass: ''
	};

    $.noty.layouts.bottomRight = {
        name: 'bottomRight',
        options: { // overrides options

        },
        container: {
            object: '<ul id="noty_bottomRight_layout_container" />',
            selector: 'ul#noty_bottomRight_layout_container',
            style: function () {
                $(this).css({
                    bottom: 20,
                    right: 20,
                    position: 'fixed',
                    width: '310px',
                    height: 'auto',
                    margin: 0,
                    padding: 0,
                    listStyleType: 'none',
                    zIndex: 10000000
                });

                if (window.innerWidth < 600) {
                    $(this).css({
                        right: 5
                    });
                }
            }
        },
        parent: {
            object: '<li />',
            selector: 'li',
            css: {}
        },
        css: {
            display: 'none',
            width: '310px'
        },
        addClass: ''
    };

})(jQuery);