(function () {
    $('body').on('click', '.tree .tree-heading', function () {
        $(this).parents('.tree').first().toggleClass('minimize', 500);
    });
})();