(function () {
    $('.widget').draggable();
    $('.widget').each(function(){
        var $widget = $(this);
        $widget.find('.panel-heading').click(function () {
            if ($widget.hasClass('minimize')) {
                $('.widget').addClass('minimize', 500);
            }
            $widget.toggleClass('minimize', 500);
        });

    });
    $('section.update-section').on('refresh', function () {
        var $this = $(this);
        var url = $this.data('url');
        $.ajax({
            url: url
        }).success(function (result) {
            console.log(result);
        });
    });
})();