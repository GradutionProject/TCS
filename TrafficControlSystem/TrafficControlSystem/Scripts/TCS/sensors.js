
(function () {
    var $form = $('.form.add-sensor');

    var sensorLayer;
    function loadSensors() {
        $('.loading').hide();
        sensorLayer.forEach(function (sensor) {
            sensorLayer.remove(sensor);
        });
        sensorLayer.loadGeoJson("/api/sensors/geo");
    };
    var mapLoaded = false;
    if (map)
    {
        if (map.loaded)
        {
            mapLoaded = true;
            init();
        }
    }
    if (!mapLoaded)
    {
        $('body').on('map-loaded', init);
    }
    function init()
    {
        sensorLayer = new google.maps.Data({
            map: map
        });
        loadSensors();
    }
    var tempSensor;
   // Select Location from map
    $form.on('selectLocation', function () {
        var addSensorListener = google.maps.event.addListener(map, 'click', function (event) {
            google.maps.event.removeListener(addSensorListener);
            var inputs = $form.data('inputs');
            var sensor = {};
            tempSensor = new google.maps.Marker({
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
    // Add new sensor
    $form.on('submit', function () {
        $('.loading').show();
        var inputs = $form.data('inputs');
        var sensor = {};
        for (var input in inputs) {
            sensor[input] = inputs[input].val();
        }
        $.ajax({
            url: '/api/sensors/add',
            method: 'POST',
            dataType: 'json',
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify(sensor)
        }).done(function (response) {
            if (tempSensor)
            {
                tempSensor.setMap(null);
                tempSensor = null;
            }
            var $updateSection = $form.find('.update-section');
            var Url = $updateSection.data('url');
            $('body').trigger('sensor-changed');
            $.get(Url, function (result) {
                $updateSection.html(result);
                loadSensors();
            });
        }
        );
    });
    // Delete sensor
    $form.on('delete', function (e, params) {
        var sensorId = params.parameter;
        $('.loading').show();
        $.ajax({
            url: '/api/sensors/delete/' + sensorId,
            method: 'GET'
        }).done(function (response) {
            $('body').trigger('sensor-changed');
            sensorLayer.forEach(function (sensor) {
                if (sensor.getProperty('id') == sensorId) {
                    sensorLayer.remove(sensor);
                }
            });
            var $updateSection = $form.find('.update-section');
            var Url = $updateSection.data('url');
            $.get(Url, function (result) {
                $updateSection.html(result);
                $('.loading').hide();
            });
        }
        );
    });

    // toggle sensors layer visibility
    $form.on('toggleSensorsVisibility', function (e, params) {
        var status = params.context.data('status');
        
        if (status)
        {
            params.context.html('Show Sensors');
            sensorLayer.setStyle({
                visible: false
            });
            status = 0;
        } else
        {
            params.context.html('Hide Sensors');
            sensorLayer.setStyle({
                visible: true
            });
            status = 1;
        }
        params.context.data('status', status);
    });
})();
