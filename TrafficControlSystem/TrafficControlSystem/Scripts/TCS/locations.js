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
            loadLoactions();
        }
        );
    });
    function loadLoactions()
    {
        var $updateSection = $form.find('.update-section');
        var Url = $updateSection.data('url');
        $.get(Url, function (result) {
            $updateSection.html(result);
        });
    }

    $form.on('delete', function (e , parameter) {
        $.ajax({
            url: '/api/locations/delete/' + parameter,
            method: 'GET',
            dataType: 'json'
        }).done(function (response) {
            loadLoactions();
        }
        );
    });
    $form.on('click', '.asiign-sensor',function(){

    });
    $form.on('assignSensor', function (e, parameter) {
        
        $.ajax({
            url: '/api/locations/delete/' + parameter,
            method: 'GET',
            dataType: 'json'
        }).done(function (response) {
            loadLoactions();
        }
        );
    });

    $('body').on('sensor-added', function () {
        loadLoactions();
    });
})();