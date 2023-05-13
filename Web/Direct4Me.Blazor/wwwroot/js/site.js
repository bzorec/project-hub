window.jsInterop = {
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
    }, isAuthenticated: function () {
        let token = document.cookie.replace(/(?:^|.*;\s*)JWTToken\s*=\s*([^;]*).*$|^.*$/, "$1");

        return token !== ""; // Change this based on your authentication logic
    }, getUsername: function () {
        var token = document.cookie.replace(/(?:^|.*;\s*)JWTToken\s*=\s*([^;]*).*$|^.*$/, "$1");

        if (token === "") return null; // No token available

        var jwtData = token.split('.')[1];
        var decodedJwt = window.atob(jwtData);
        var jwtClaims = JSON.parse(decodedJwt);

        var username = jwtClaims.name;

        return username || null; // Return the username or null if not found
    },
}