(function () {
    $.fn.template = function ($template, obj, pattern) {
        obj = [].concat(obj);
        $(this).each(function () {
            var html = $.template($template, obj, pattern);
            $(this).html(html);
        });

    };
    $.template = function ($template, obj, pattern) {
        obj = [].concat(obj);
        var html = '';
        $(obj).each(function (i, element) {
            var template = $($template).html();
            if (typeof (element) === "string") {
                template = template.replace(new RegExp("{" + pattern + "}", 'g'), element);
            } else if (typeof (element) === "object") {
                for (var property in element) {
                    template = template.replace(new RegExp("{" + property + "}", 'g'), element[property]);
                }
            }
            html += template;
        });
        return html;
    };

    $.fn.showError = function (errors) {
        $(this).each(function () {
            var $errSummary = $(this).find('.err-summary .alert').first();
            $errSummary.template($('#errorTemplate'), errors, 'error');
            $errSummary.show();
        });
    };
    $.fn.hideError = function () {
        $(this).each(function () {
            $(this).find('.err-summary .alert').first().hide();
        });
    };
})();