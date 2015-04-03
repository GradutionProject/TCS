(function () {

    var $form = $('.form.locations');
    var locationLayer;
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
    function loadLoactions() {
        var $updateSection = $form.find('.update-section');
        var Url = $updateSection.data('url');
        $.get(Url, function (result) {
            $('.loading').hide();
            $updateSection.html(result);
        });
        loadLocationsData();

    }

    function loadLocationsData() {
        $('.loading').hide();
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

    $form.on('delete', function (e, params) {
        $('.loading').show();
        $.ajax({
            url: '/api/locations/delete/' + params.parameter,
            method: 'GET',
            dataType: 'json'
        }).done(function (response) {
            loadLoactions();
        }
        );
    });
    $form.on('click', '.assign-sensor', function () {
        $('loading').show();
        var $this = $(this);
        var $parent = $this.parent('.select-sensor-panel');
        var $sensor = $parent.find('.select-sensor');
        var $type = $parent.find('.select-sensor-type');
        $.ajax({
            url: '/api/locations/AssignSensor/?locationId=' + $this.data('location-id') + '&sensorId=' + $sensor.val() + '&input=' + $type.val(),
            method: 'GET',
            dataType: 'json'
        }).done(function (response) {
            var $sensors = $parent.parents('.location').find('.location-sensor');
            bindSenosrs($sensors, response);
            $('.loading').hide();
        });

    });

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
            $('.loading').hide();
        });
    });

    $form.on('moveUp', function (e, parms) {
        $('.loading').show();
        var $link = parms.context;
        var locSensorId = $link.data('location-sensor-id');
        $.ajax({
            url: '/api/locations/MoveUpSensor/?locationSensorId=' + locSensorId,
            method: 'GET',
            dataType: 'json'
        }).done(function (response) {
            var $sensors = $link.parents('.location').find('.location-sensor');
            bindSenosrs($sensors, response);
            $('.loading').hide();
        });
    });


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
            $('.loading').hide();
        });
    });

    function bindSenosrs($sensors, location) {
        var html = "";
        location.LocationSensors = location.LocationSensors.sort(function (ls1, ls2) { return ls1.Order > ls2.Order; });
        for (var i = 0; i < location.LocationSensors.length; i++) {
            var locSensor = location.LocationSensors[i];
            var data = {
                SensorName: locSensor.Sensor.Name,
                InputOrOutput: locSensor.InputOrOutput ? "Input" : "Output",
                Id: locSensor.LocationSensorsId
            };
            var template = $('#locationSensorTemplate').html();
            for (var prop in data) {
                template = template.replace(new RegExp('{' + prop + '}', 'g'), data[prop]);
            }
            html += template;
        }
        $sensors.html(html);
        loadLocationsData(location);

    }


    $('body').on('sensor-changed', function () {
        loadLoactions();
    });
})();