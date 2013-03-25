common_class = function() {

};

common_class.prototype.initPageEvents = function() {


};

// Shows an ajax spinner over the specified element
// Ex: common.showAjaxLoader($("#myElement"));
common_class.prototype.showAjaxLoader = function($el) {
    var width = $el.width();
    var height = $el.height();

    var $html = $('<div class="ajaxContainer"><div class="ajaxLoader"></div></div>');
    $html.css({ "height": height, "width": width });

    // Element must be relative for the graphic to overlay properly
    $el.css("position", "relative").append($html);
};

common_class.prototype.hideAjaxLoader = function($el) {
    $el.find(".ajaxContainer").remove();
};

common_class.prototype.insertParam = function(url, key, value) {
    return url + (url.indexOf("?") < 0 ? "?" : "&") + key + "=" + value;
};

common_class.prototype.executeFunctionByName = function(functionName, context /*, args */) {
    var args = Array.prototype.slice.call(arguments).splice(2);
    var namespaces = functionName.split(".");
    var func = namespaces.pop();
    for (var i = 0; i < namespaces.length; i++) {
        context = context[namespaces[i]];
    }
    return context[func].apply(this, args);
};

// Keep at the bottom
$(document).ready(function () {
    common = new common_class();
    common.initPageEvents();
});