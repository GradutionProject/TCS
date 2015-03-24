(function () {
    var $form = $('.form.locations');
    $form.on('submit', function () {
        var inputs = $form.data('inputs');
        var location = {};
        for (var input in inputs) {
            location[input] = inputs[input].val();
        }
        $.ajax({
            url: '/api/locations/add',
            method: 'POST',
            dataType: 'json',
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify(location)
        }).done(function (response) {
            var $updateSection = $form.find('.update-section');
            var Url = $updateSection.data('url');
            $.get(Url, function (result) {
                $updateSection.html(result);
                loadSensors();
            });
        }
        );
    });
})();