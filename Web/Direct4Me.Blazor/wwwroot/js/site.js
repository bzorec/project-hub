window.jsInterop = {
    getBoundingClientRect: function (element) {
        return element.getBoundingClientRect();
    },
    setToken: function (token) {
        document.cookie = "JWTToken=" + token + "; expires=" + (new Date(Date.now() + 5 * 60 * 1000)).toUTCString() + "; path=/; secure; samesite=none";
    },

    getToken: function () {
        var cookies = document.cookie.split("; ");
        for (var i = 0; i < cookies.length; i++) {
            var cookie = cookies[i].split("=");
            if (cookie[0] === "JWTToken") {
                return cookie[1];
            }
        }
        return null;
    },

    removeToken: function () {
        document.cookie = "JWTToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/; secure; samesite=none";
    },

    isAuthenticated: function () {
        let token = document.cookie.replace(/(?:^|.*;\s*)JWTToken\s*=\s*([^;]*).*$|^.*$/, "$1");

        return token !== ""; // Change this based on your authentication logic
    },

    getUsername: function () {
        var token = document.cookie.replace(/(?:^|.*;\s*)JWTToken\s*=\s*([^;]*).*$|^.*$/, "$1");

        if (token === "") return null; // No token available

        var jwtData = token.split('.')[1];
        var decodedJwt = window.atob(jwtData);
        var jwtClaims = JSON.parse(decodedJwt);

        var username = jwtClaims.name;

        return username || null; // Return the username or null if not found
    },

    getEmail: function () {
        var token = document.cookie.replace(/(?:^|.*;\s*)JWTToken\s*=\s*([^;]*).*$|^.*$/, "$1");

        if (token === "") return null; // No token available

        var jwtData = token.split('.')[1];
        var decodedJwt = window.atob(jwtData);
        var jwtClaims = JSON.parse(decodedJwt);

        var email = jwtClaims.email;

        return email || null; // Return the emaič or null if not found
    },

    playAndCloseAnimation(audioData) {
        var audio = document.createElement("audio");

        var blob = new Blob([audioData], {type: "audio/mp3"});

        audio.src = URL.createObjectURL(blob);

        document.body.appendChild(audio);

        audio.play();

        audio.addEventListener("ended", function () {
            closeAnimation();
        });
    },

    closeModal() {
        $('#updateModal').modal('hide');
    },

    openModal() {
        $('#updateModal').modal('show');
    },

    closeHistoryModal() {
        $('#grantAccessModal').modal('hide');
    },

    openHistoryModal() {
        $('#grantAccessModal').modal('show');
    },

    initMap: initMap,
    addMarker: addMarker,
    drawPath: drawPath,

    showSpinner() {
        document.getElementById('map-spinner').style.display = 'block';
    },

    hideSpinner() {
        document.getElementById('map-spinner').style.display = 'none';
    },

    initBestPathMap: initBestPathMap
};
let globalMap;
let markers = [];
let polyline;

function initBestPathMap(tour, zoomLevel) {
    const mapContainer = document.getElementById('map');
    if (!mapContainer) {
        console.error('Map container not found');
        return;
    }

    // Remove existing markers
    markers.forEach(marker => {
        globalMap.removeLayer(marker);
    });

    // Remove existing polyline if it exists
    if (polyline) {
        globalMap.removeLayer(polyline);
    }

    if (globalMap) {
        addMarkersAndDrawPath(tour);
        return;
    } else {
        let startCity = tour.path[0];
        let map = L.map('map').setView([startCity.lat, startCity.lng], zoomLevel);

        L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
            maxZoom: 19,
            attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
        }).addTo(map);

        globalMap = map;
    }

    addMarkersAndDrawPath(tour);
}

function addMarkersAndDrawPath(tour) {
    let latlngs = [];

    tour.path.forEach(city => {
        let latlng = L.latLng(city.lat, city.lng);
        latlngs.push(latlng);

        let marker = L.marker(latlng).addTo(globalMap);
        markers.push(marker);
    });

    polyline = L.polyline(latlngs, {color: 'blue'}).addTo(globalMap);
    globalMap.fitBounds(polyline.getBounds());
}
function initMap(latitude, longitude, zoomLevel) {
    const mapContainer = document.getElementById('map');
    if (!mapContainer) {
        console.error('Map container not found');
        return;
    }
    if (globalMap) {
        return;
    }

    let map = L.map('map').setView([latitude, longitude], zoomLevel);

    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map);

    globalMap = map;
}

function addMarker(latitude, longitude) {
    L.marker([latitude, longitude]).addTo(globalMap);
}

function drawPath(latlngs) {
    let polyline = L.polyline(latlngs, {color: 'blue'}).addTo(globalMap);
    globalMap.fitBounds(polyline.getBounds());
}

function showSpinner() {
}

function hideSpinner() {
}