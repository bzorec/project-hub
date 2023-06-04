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
    }, closeModal() {
        $('#updateModal').modal('hide');
    }, openModal() {
        $('#updateModal').modal('show');
    },
    closeHistoryModal() {
        $('#grantAccessModal').modal('hide');
    }, openHistoryModal() {
        $('#grantAccessModal').modal('show');
    }, captureImageFromCamera: function () {
        return new Promise((resolve, reject) => {
            const videoElement = document.getElementById("camera-stream");
            const canvasElement = document.createElement("canvas");
            const context = canvasElement.getContext("2d");

            // Access the user's camera and display the video stream
            navigator.mediaDevices.getUserMedia({video: true})
                .then(function (stream) {
                    videoElement.srcObject = stream;
                    videoElement.play();

                    // Draw the video frame on the canvas
                    context.drawImage(videoElement, 0, 0, videoElement.videoWidth, videoElement.videoHeight);

                    // Convert the canvas image to a data URL
                    const imageUrl = canvasElement.toDataURL();

                    // Stop the video stream and release resources
                    videoElement.srcObject = null;
                    stream.getVideoTracks().forEach(track => track.stop());

                    resolve(imageUrl);
                })
                .catch(function (error) {
                    reject(error);
                });
        });
    },

    authenticateImage: function (byteArray) {
        return new Promise((resolve, reject) => {
            const formData = new FormData();
            formData.append("image", new Blob([byteArray]));

            // Send the POST request to the API endpoint for image authentication
            fetch("/imgAuthenticate", {
                method: "POST",
                body: formData
            })
                .then(response => response.json())
                .then(data => {
                    const userId = data.user_id;
                    const confidence = data.confidence;

                    if (userId !== null) {
                        console.log(`Predicted user_id: ${userId} with confidence ${confidence}`);
                        resolve(true);
                    } else {
                        console.log("Prediction failed.");
                        resolve(false);
                    }
                })
                .catch(error => {
                    reject(error);
                });
        });
    },

    base64ToByteArray: function (base64String) {
        const binaryString = window.atob(base64String.split(",")[1]);
        const byteArray = new Uint8Array(binaryString.length);
        for (let i = 0; i < binaryString.length; i++) {
            byteArray[i] = binaryString.charCodeAt(i);
        }
        return byteArray;
    }
}

function closeAnimation() {
}