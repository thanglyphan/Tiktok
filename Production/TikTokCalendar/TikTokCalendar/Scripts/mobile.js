$(function() {

    google.maps.event.addDomListener(window, "load", init);
    var map;

    function init() {
        var mapOptions = {
            center: new google.maps.LatLng(59.919839, 10.756072),
            zoom: 13,
            zoomControl: false,
            zoomControlOptions: {
                style: google.maps.ZoomControlStyle.DEFAULT,
            },
            disableDoubleClickZoom: true,
            mapTypeControl: false,
            scaleControl: false,
            scrollwheel: true,
            panControl: false,
            streetViewControl: false,
            draggable: true,
            overviewMapControl: false,
            overviewMapControlOptions: {
                opened: false,
            },
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            styles: [
                { "featureType": "landscape.man_made", "elementType": "geometry", "stylers": [{ "color": "#f7f1df" }] },
                { "featureType": "landscape.natural", "elementType": "geometry", "stylers": [{ "color": "#d0e3b4" }] },
                {
                    "featureType": "landscape.natural.terrain",
                    "elementType": "geometry",
                    "stylers": [{ "visibility": "off" }]
                }, { "featureType": "poi", "elementType": "labels", "stylers": [{ "visibility": "off" }] },
                { "featureType": "poi.business", "elementType": "all", "stylers": [{ "visibility": "off" }] },
                { "featureType": "poi.medical", "elementType": "geometry", "stylers": [{ "color": "#fbd3da" }] },
                { "featureType": "poi.park", "elementType": "geometry", "stylers": [{ "color": "#bde6ab" }] },
                { "featureType": "road", "elementType": "geometry.stroke", "stylers": [{ "visibility": "off" }] },
                { "featureType": "road", "elementType": "labels", "stylers": [{ "visibility": "off" }] },
                { "featureType": "road.highway", "elementType": "geometry.fill", "stylers": [{ "color": "#ffe15f" }] },
                {
                    "featureType": "road.highway",
                    "elementType": "geometry.stroke",
                    "stylers": [
                    {
                        "color":
                            "#efd151"
                    }
                    ]
                },
                { "featureType": "road.arterial", "elementType": "geometry.fill", "stylers": [{ "color": "#ffffff" }] },
                { "featureType": "road.local", "elementType": "geometry.fill", "stylers": [{ "color": "black" }] },
                {
                    "featureType": "transit.station.airport",
                    "elementType": "geometry.fill",
                    "stylers": [{ "color": "#cfb2db" }]
                }, { "featureType": "water", "elementType": "geometry", "stylers": [{ "color": "#a2daf2" }] }
            ],
        };
        var mapElement = document.getElementById("map");
        var map = new google.maps.Map(mapElement, mapOptions);
        var locations = [
            [
                "Teknologi", "Teknologi", "undefined", "undefined", "undefined", 59.2131164, 9.608698199999935,
                "https://mapbuildr.com/assets/img/markers/solid-pin-black.png"
            ]
        ];
        for (i = 0; i < locations.length; i++) {
            if (locations[i][1] == "undefined") {
                description = "";
            } else {
                description = locations[i][1];
            }
            if (locations[i][2] == "undefined") {
                telephone = "";
            } else {
                telephone = locations[i][2];
            }
            if (locations[i][3] == "undefined") {
                email = "";
            } else {
                email = locations[i][3];
            }
            if (locations[i][4] == "undefined") {
                web = "";
            } else {
                web = locations[i][4];
            }
            if (locations[i][7] == "undefined") {
                markericon = "";
            } else {
                markericon = locations[i][7];
            }
            marker = new google.maps.Marker({
                icon: markericon,
                position: new google.maps.LatLng(locations[i][5], locations[i][6]),
                map: map,
                title: locations[i][0],
                desc: description,
                tel: telephone,
                email: email,
                web: web
            });
            link = "";
        }

    }
});