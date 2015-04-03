(function () {
    var $form = $('.form.locations');
    var locations = {};
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
            $updateSection.html(result);
        });
        loadLocationsData();
    
    }

    function loadLocationsData()
    {
        $.get('/api/locations/all', function (result) {
            $.each(result, function (i, loc) {

                loc.LocationSensors = loc.LocationSensors.sort(function (ls1, ls2) { return ls1.Sensor.Name > ls2.Sensor.Name; });
                drawLocation(loc);
            });
        });
    }
    var mapLoaded = false;
    if (map) {
        if (map.loaded) {
            mapLoaded = true;
            loadLocationsData();
        }
    }
    if (!mapLoaded) {
        $('body').on('map-loaded', function () {
            loadLocationsData();
        });
    }

    $form.on('delete', function (e, parameter) {
        $.ajax({
            url: '/api/locations/delete/' + parameter,
            method: 'GET',
            dataType: 'json'
        }).done(function (response) {
            loadLoactions();
        }
        );
    });
    $form.on('click', '.assign-sensor', function () {
        var $this = $(this);
        var $parent = $this.parent('.select-sensor-panel');
        var $sensor = $parent.find('.select-sensor');
        var $type = $parent.find('.select-sensor-type');
        $.ajax({
            url: '/api/locations/assignSensor/?locationId=' + $this.data('location-id') + '&sensorId=' + $sensor.val() + '&input=' + $type.val(),
            method: 'GET',
            dataType: 'json'
        }).done(function (response) {
            var $sensors = $parent.parents('.location').find('.location-sensor');
            var html = "";
            response.LocationSensors = response.LocationSensors.sort(function (ls1, ls2) { return ls1.Sensor.Name > ls2.Sensor.Name; });
            for (var i = 0; i < response.LocationSensors.length; i++) {
                var locSensor = response.LocationSensors[i];
                var data = {
                    SensorName: locSensor.Sensor.Name,
                    InputOrOutput: locSensor.InputOrOutput ? "Input" : "Output"
                };
                var template = $('#locationSensorTemplate').html();
                for (var prop in data) {
                    template = template.replace('{' + prop + '}', data[prop]);
                }
                html += template;
            }
            $sensors.html(html);
            drawLocation(response);
        });

    });

    function drawLocation(location) {
        var paths = [];
        for (var i = 0; i < location.LocationSensors.length; i++) {
            var sensor = location.LocationSensors[i].Sensor;
            paths.push(new google.maps.LatLng(sensor.Latitude, sensor.Longitude));
        }
        if (location.LocationSensors.length > 2) {
            location.overlay = new google.maps.Polygon({
                map: map,
                paths: paths,
                fillColor: "#FF0000"
            });
        } else {
            location.overlay = new google.maps.Polyline({
                map: map,
                paths: paths,
                fillColor: "#FF0000"
            });
        }
        if (locations[location.LocationId]) {
            locations[location.LocationId].overlay.setMap(null);
        }
        locations[location.LocationId] = location;

    }
    $('body').on('sensor-added', function () {
        loadLoactions();
    });
})();