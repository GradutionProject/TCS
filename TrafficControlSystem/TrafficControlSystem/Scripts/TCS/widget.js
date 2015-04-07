(function () {
    //$('.widget').draggable();
    $('.widget').each(function () {
        var $widget = $(this);
        $widget.find('.panel-heading').first().click(function () {
            if ($widget.hasClass('minimize')) {
                $('.widget').addClass('minimize', 100);
                $widget.removeClass('minimize', 500, 'easeInQuint', function () {
                    if (!$widget.find('.dataTable').data('dt-initialized')) {
                        $widget.find('.dataTable').dataTable({
                            "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]]
                        });
                    }
                    $widget.find('.dataTable').data('dt-initialized', true);
                });

            } else {
                $widget.addClass('minimize', 500, 'easeInQuint');
            }


        });

    });
    $('.widget-trigger').click(function () {
        var $trigger = $(this);
        $('.widget.' + $trigger.data('widget')).find('.panel-heading').first().trigger('click');
    });
    $('section.update-section').on('refresh', function () {
        var $this = $(this);
        var url = $this.data('url');
        $.get(url, function (result) {
            $this.html(result);
            $this.trigger('refresh-complete');
            $this.find('.dataTable').dataTable({
                "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]]
            });
        });
    });

})();