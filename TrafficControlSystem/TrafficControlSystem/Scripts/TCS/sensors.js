
(function () {
    var $addSensorForm = $('.form.add-sensor');
    var sensors = [];
    function showSensors(data) {
        $(sensors).each(function (i, sensor) {
            sensor.marker.setMap(null);
        });

        sensors = [];
        $(data).each(function (i, sensor) {

            var latLng = new google.maps.LatLng(sensor.Latitude, sensor.Longitude);
            sensor.marker = new google.maps.Marker({
                position: latLng,
                map: map,
                title: sensor.Name,
                icon: "/images/More-16.png"
            });
            sensors.push(sensor);
        });
    }
    function loadSensors() {
        $.ajax({
            url: '/api/sensors/all',
            method: 'GET',
            dataType: 'json'
        }).success(function (result) {
            showSensors(result);
        }).error(function (err) {
            console.log(err);
        });
    };
    var mapLoaded = false;
    if (map)
    {
        if (map.loaded)
        {
            mapLoaded = true;
            loadSensors();
        }
    }
    if (!mapLoaded)
    {
        $('body').on('map-loaded', function () {
            loadSensors();
        });
    }
   
    $addSensorForm.on('selectLocation', function () {
        var addSensorListener = google.maps.event.addListener(map, 'click', function (event) {
            google.maps.event.removeListener(addSensorListener);
            var inputs = $addSensorForm.data('inputs');
            var sensor = {};
            sensor.marker = new google.maps.Marker({
                position: event.latLng,
                map: map,
                title: "Sensor",
                icon: "/images/More-16.png"
            });
            sensors.push(sensor);
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
            url: '/api/sensors/add',
            method: 'POST',
            dataType: 'json',
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify(sensor)
        }).done(function (response) {
            var $updateSection = $addSensorForm.find('.update-section');
            var Url = $updateSection.data('url');
            $.get(Url, function (result) {
                $updateSection.html(result);
                loadSensors();
            });
        }
        );
    });

    $addSensorForm.on('delete', function (e, sensorId) {
        $.ajax({
            url: '/api/sensors/delete/' + sensorId,
            method: 'GET'
        }).done(function (response) {
            var $updateSection = $addSensorForm.find('.update-section');
            var Url = $updateSection.data('url');
            $.get(Url, function (result) {
                $updateSection.html(result);
                loadSensors();
            });
        }
        );
    });

})();
