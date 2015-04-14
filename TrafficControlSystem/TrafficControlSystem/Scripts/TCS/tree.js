(function () {
    $('body').on('click', '.tree .tree-heading', function () {
        var $tree = $(this).parents('.tree').first();
        if ($tree.parents('.tree-group').first().length > 0) {
            if ($tree.hasClass('minimize')) {
                $tree.parents('.tree-group').first().find('.tree').addClass('minimize', 500);
                $tree.removeClass('minimize');
            } else {
                $tree.addClass('minimize');
            }
            
           
        } else {
            $tree.toggleClass('minimize', 500);
        }
    });
})();