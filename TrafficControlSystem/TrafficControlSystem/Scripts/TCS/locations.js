(function () {

    var $form = $('.form.locations');
    var locationLayer;

    function loadLoactions() {
        var $updateSection = $form.find('.update-section');
        var Url = $updateSection.data('url');
        $.get(Url, function (result) {
            $('.loading').hide();
            $updateSection.html(result);
        });

    }

    // Load location data - Redraw locations on map
    function loadLocationsData() {
        $('.loading').hide();
        locationLayer.setStyle({ visible: true });
        locationLayer.forEach(function (location) {
            locationLayer.remove(location);
        });
        locationLayer.loadGeoJson("/api/locations/geo");
    }


    var mapLoaded = false;
    if (map) {
        if (map.loaded) {
            mapLoaded = true;
            init();
        }
    }
    if (!mapLoaded) {
        $('body').on('map-loaded', function () {
            init();
        });
    }
    function init() {
        locationLayer = new google.maps.Data({ map: map });
        locationLayer.setStyle(function (feature) {
            var count = feature.getProperty('count');
            var color = count > 20 ? 'red' : 'blue';
            return {
                fillColor: color,
                strokeWeight: 1
            };
        });
        loadLocationsData();
    }

    // Add new location
    $form.on('submit', function () {
        $('.loading').show();
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
            $('.loading').hide();
            loadLoactions();
        });
    });

    // Delete location
    $form.on('delete', function (e, params) {
        $('.loading').show();
        $.ajax({
            url: '/api/locations/delete/' + params.parameter,
            method: 'GET',
            dataType: 'json'
        }).done(function (response) {
            loadLoactions();
            loadLocationsData();
        }
        );
    });

    //assign sensor to location
    $form.on('assign', function (e, params) {
        $('.loading').show();
        var $this = params.context;
        var $parent = $this.parents('.select-sensor-panel').first();
        var $sensor = $parent.find('.select-sensor');
        var $type = $parent.find('.select-sensor-type');
        $.ajax({
            url: '/api/locations/AssignSensor/?locationId=' + $this.data('location-id') + '&sensorId=' + $sensor.val() + '&input=' + $type.val(),
            method: 'GET',
            dataType: 'json'
        }).done(function (response) {
            var $sensors = $parent.parents('.location').find('.location-sensor');
            bindSenosrs($sensors, response);
            loadLocationsData();
            $('.loading').hide();
        });

    });

    // Unassign sensor from location
    $form.on('unassign', function (e, parms) {
        $('.loading').show();
        var $link = parms.context;
        var locSensorId = $link.data('location-sensor-id');
        $.ajax({
            url: '/api/locations/UnassignSensor/?locationSensorId=' + locSensorId,
            method: 'GET',
            dataType: 'json'
        }).done(function (response) {
            var $sensors = $link.parents('.location').find('.location-sensor');
            bindSenosrs($sensors, response);
            loadLocationsData();
            $('.loading').hide();
        });
    });

    // Move sensor up in order
    $form.on('moveUp', function (e, parms) {
        $('.loading').show();
        var $link = parms.context;
        var locSensorId = $link.data('location-sensor-id');
        $.ajax({
            url: '/api/locations/MoveUpSensor/?locationSensorId=' + locSensorId,
            method: 'GET',
            dataType: 'json'
        }).done(function (response) {
            loadLocationsData();
            var $sensors = $link.parents('.location').find('.location-sensor');
            bindSenosrs($sensors, response);
            $('.loading').hide();
        });
    });

    // Move sensor down in order
    $form.on('moveDown', function (e, parms) {
        $('.loading').show();
        var $link = parms.context;
        var locSensorId = $link.data('location-sensor-id');
        $.ajax({
            url: '/api/locations/MoveDownSensor/?locationSensorId=' + locSensorId,
            method: 'GET',
            dataType: 'json'
        }).done(function (response) {
            var $sensors = $link.parents('.location').find('.location-sensor');
            bindSenosrs($sensors, response);
            loadLocationsData();
            $('.loading').hide();
        });
    });

    // Update location sensors
    function bindSenosrs($sensors, location) {
        var html = "";
        location.LocationSensors = location.LocationSensors.sort(function (ls1, ls2) { return ls1.Order > ls2.Order; });
        var sensors = [];
        for (var i = 0; i < location.LocationSensors.length; i++) {
            var locSensor = location.LocationSensors[i];
            var sensor = {
                SensorName: locSensor.Sensor.Name,
                InputOrOutput: locSensor.InputOrOutput ? "Input" : "Output",
                Id: locSensor.LocationSensorsId,
                SensorId : locSensor.SesnorId
            };
            sensors.push(sensor);
        }
        $sensors.template($('#locationSensorTemplate'), sensors);
    }

    // Zoom to sensor
    $form.on('sensor-zoom', function (e, params)
    {
        $('body').trigger('sensorZoom', params);
    });
    // Zoom to sensor
    $form.on('sensor-flash', function (e, params) {
        $('body').trigger('sensorFlash', params);
    });
    // Sensors changed
    $('body').on('sensor-changed', function () {
        locationLayer.setStyle({ visible: true });
        loadLoactions();
    });

    $('body').on('select-sensor', function () {
        locationLayer.setStyle({ visible: false });
    });
})();