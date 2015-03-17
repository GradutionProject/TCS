
(function () {
    //var actions = {
    //    removeSensor: function (sensorId) {
    //        sensors[sensorId].marker.setMap(null);
    //        $('.senors-grid .sens-' + sensorId).remove();
    //    },
    var $addSensorForm = $('.form.add-sensor');
    $addSensorForm.on('selectLocation', function () {
        var addSensorListener = google.maps.event.addListener(map, 'click', function (event) {
            google.maps.event.removeListener(addSensorListener);
            var inputs = $addSensorForm.data('inputs');
            var marker = new google.maps.Marker({
                position: event.latLng,
                map: map,
                title: "Sensor",
                icon: "/images/More-16.png"
            });
            inputs["Latitude"].val(event.latLng.lat());
            inputs["Longitude"].val(event.latLng.lng());
            map.setOptions({
                draggableCursor: "url('https://maps.gstatic.com/mapfiles/openhand_8_8.cur'), default"
            });
        });
        map.setOptions({
            draggableCursor: "crosshair"
        });
    });
    $addSensorForm.on('submit', function () {
        var inputs = $addSensorForm.data('inputs');
        var sensor = {};
        for (var input in inputs) {
            sensor[input] = inputs[input].val();
        }
        $.ajax({
            url: '/api/sensors/',
            method: 'POST',
            dataType: 'json',
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify(sensor)
        }).done(function (response) {
            var Url = $('.update-section').data('url');
            $.get(Url, function (result) {
                $('.update-section').html(result);
            });
        }
        );
    });
    $('.form.add-sensor').each(function () {
        var $form = $(this);
        var inputs = {};
        $form.find('.form-control').each(function () {
            var $input = $(this);
            inputs[$input.attr("name")] = $input;
        });
        $form.data('inputs', inputs);
        $form.find('.action').each(function () {
            var $action = $(this);
            $action.click(function () {
                var command = $action.data("action-click");
                $form.trigger(command);
            })
        });
    });
})();
