var map;

function initialize() {
    var mapOptions = {
        center: { lat: 30.099408866368016, lng: 31.32052463912964 },
        zoom: 15
    };
    map = new google.maps.Map(document.getElementById('map-canvas'),
        mapOptions);
    map.infowindow = new google.maps.InfoWindow();
    google.maps.event.addListenerOnce(map, 'tilesloaded', function (event) {
        map.loaded = true;
        $('body').trigger('map-loaded');
        $('body').on('point-zoom', function (e, latLng) {
            map.setZoom(20);
            map.setCenter(new google.maps.LatLng(Number(latLng.lat), Number(latLng.lng)));
        });
        $('.loading').hide();
    });
}

google.maps.event.addDomListener(window, 'load', initialize);