
(function () {
    var form = $('.form.add-sensor').form()[0];

    var sensorLayer;
    function loadSensors() {
        map.infowindow.close();
        sensorLayer.forEach(function (sensor) {
            sensorLayer.remove(sensor);
        });
        sensorLayer.loadGeoJson("/api/sensors/geo");
    };

    var mapLoaded = false;
    if (map) {
        if (map.loaded) {
            mapLoaded = true;
            init();
        }
    }
    if (!mapLoaded) {
        $('body').on('map-loaded', init);
    }
    function init() {
        sensorLayer = new google.maps.Data({
            map: map
        });
        google.maps.event.addListener(sensorLayer, 'click', function (e) {
            var feature = e.feature;
            var info = {};
            feature.forEachProperty(function (val, name) {
                info[name] = val;
            });
            map.infowindow.setContent($.template($('#sensorInfoWindow'), info));
            map.infowindow.setPosition(feature.getGeometry().get());
            map.infowindow.open(map);
        });
        loadSensors();
    }

    var tempSensor;
    // Select Location from map
    form.on('selectLocation', function () {
        $('body').trigger('select-sensor');
        var addSensorListener = google.maps.event.addListener(map, 'click', function (event) {
            google.maps.event.removeListener(addSensorListener);
            var inputs = form.getInputs();
            var sensor = {};
            if (tempSensor) {
                tempSensor.setMap(null);
                tempSensor = null;
            }
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
    form.on('submit', function () {
        form.hideError();
        var inputs = form.getInputs();
        var sensor = {};
        for (var input in inputs) {
            sensor[input] = inputs[input].val();
        }
        var validations = [];
        if (sensor.Name == "") { validations.push("Sensor name is missing"); }
        if (sensor.Latitude == "") { validations.push("Sensor position is missing"); }
        if (sensor.TypeId == "") { validations.push("Sensor type is missing"); }
        if (validations.length > 0) {
            form.showError(validations);
            return;
        }

        if (tempSensor) {
            tempSensor.setMap(null);
            tempSensor = null;
        }
        $('.loading').show();
        $.ajax({
            url: '/api/sensors/add',
            method: 'POST',
            dataType: 'json',
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify(sensor)
        }).done(function (response) {
            var $updateSection = form.$form.find('.update-section');
            $updateSection.off('refresh-complete').on('refresh-complete', function () {
                loadSensors();
            });
            $updateSection.trigger('refresh');
            $('body').trigger('sensor-changed');

        }).error(function (err) {
            form.handleAJAXError(err);
            $('.loading').hide();
        });
    });
    // Delete sensor
    form.on('delete', function (e, params) {
        form.hideError();
        var sensorId = params.parameter;
        $('.loading').show();
        $.ajax({
            url: '/api/sensors/delete/' + sensorId,
            method: 'GET'
        }).done(function (response) {
            $('body').trigger('sensor-changed');
            loadSensors();
            var $updateSection = form.$form.find('.update-section');
            $updateSection.off('refresh-complete').on('refresh-complete', function () {
                $('.loading').hide();
            });
            $updateSection.trigger('refresh');
        }).error(function (err) {
            form.showError([JSON.parse(err.responseText).ExceptionMessage]);
            $('.loading').hide();
        });
    });
    // Zoom to sensor
    form.on('zoom', function (e, params) {
        form.hideError();
        var sensorId = params.parameter;
        var sensor;
        sensorLayer.forEach(function (feature) {
            if (feature.getProperty('id') == sensorId) {
                sensor = feature;
                return false;
            }
        });
        if (sensor) {
            $('body').trigger('point-zoom', { lat: sensor.getGeometry().get().lat(), lng: sensor.getGeometry().get().lng() });
        }
    });
    // flash sensor on map
    form.on('flash', function (e, params) {
        form.hideError();
        var sensorId = params.parameter;
        var sensor;
        sensorLayer.forEach(function (feature) {
            if (feature.getProperty('id') == sensorId) {
                sensor = feature;
                return false;
            }
        });
        if (sensor) {
            var flashCount = 0;

            function flash() {
                if (flashCount % 2 == 0) {
                    sensorLayer.remove(sensor);
                } else {
                    sensorLayer.add(sensor);
                }
                flashCount++;
                if (flashCount < 6)
                { window.setTimeout(flash, 500); }
            }
            flash();
        }
    });
    // Clear Form 
    form.on('cancel', function () {
        if (tempSensor) {
            tempSensor.setMap(null);
            tempSensor = null;
        }
        $('body').trigger('select-sensor-cancel');
        form.clear();
    });
    $('body').on('sensorZoom', function (e, params) {
        form.trigger('zoom', params);
    });
    $('body').on('sensorFlash', function (e, params) {
        form.trigger('flash', params);
    });
    // toggle sensors layer visibility
    form.on('toggleSensorsVisibility', function (e, params) {
        var status = params.context.data('status');

        if (status) {
            params.context.html('Show Sensors');
            sensorLayer.setStyle({
                visible: false
            });
            status = 0;
        } else {
            params.context.html('Hide Sensors');
            sensorLayer.setStyle({
                visible: true
            });
            status = 1;
        }
        params.context.data('status', status);
    });


})();
