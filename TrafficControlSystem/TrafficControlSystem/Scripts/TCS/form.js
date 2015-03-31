(function () {
    $('.form').each(function () {
        var $form = $(this);
        var inputs = {};
        $form.find('.form-control').each(function () {
            var $input = $(this);
            inputs[$input.attr("name")] = $input;
        });
        $form.data('inputs', inputs);
        $form.on('click', '[data-action-click]', function (e) {
            
            var $action = $(this);
            var command = $action.data("action-click");
            var parameter = $action.data("action-parameter");
            $form.trigger(command, parameter);
            return false;
        });
    });
})();