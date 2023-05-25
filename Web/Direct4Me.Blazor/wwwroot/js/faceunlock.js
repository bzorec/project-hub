let canvasElement;
let context;
let byteArrayResolver;

window.faceUnlock = {
    enable: function () {
        if (!navigator.mediaDevices || !navigator.mediaDevices.getUserMedia) {
            // Check if the browser supports media devices and getUserMedia
            alert("Sorry, your browser doesn't support camera access.");
            return;
        }

        // Access the user's camera and display the video stream
        navigator.mediaDevices.getUserMedia({video: true})
            .then(function (stream) {
                const videoElement = document.createElement("video");
                videoElement.srcObject = stream;
                videoElement.play();

                // Create a canvas element to draw the captured image
                canvasElement = document.createElement("canvas");
                canvasElement.width = videoElement.videoWidth;
                canvasElement.height = videoElement.videoHeight;

                // Draw the video frame on the canvas
                context = canvasElement.getContext("2d");
                context.drawImage(videoElement, 0, 0, canvasElement.width, canvasElement.height);

                // Show the modal
                const imageModal = new bootstrap.Modal(document.getElementById("imageModal"));
                imageModal.show();
            })
            .catch(function (error) {
                console.error("Error accessing camera:", error);
            });
    }, enableFaceUnlock: function () {
        return new Promise((resolve) => {
            byteArrayResolver = resolve;
            window.faceUnlock.enable();
        });
    }
};

function captureImage() {
    // Draw the current video frame on the canvas
    context.drawImage(videoElement, 0, 0, canvasElement.width, canvasElement.height);

    // Convert the canvas image to a data URL
    const imageUrl = canvasElement.toDataURL();

    // Convert the data URL to a byte array
    const byteArray = base64ToByteArray(imageUrl);

    // Close the modal
    const imageModal = new bootstrap.Modal(document.getElementById("imageModal"));
    imageModal.hide();

    // Resolve the byte array promise
    byteArrayResolver(byteArray);
}

function base64ToByteArray(base64String) {
    const binaryString = window.atob(base64String.split(",")[1]);
    const byteArray = new Uint8Array(binaryString.length);
    for (let i = 0; i < binaryString.length; i++) {
        byteArray[i] = binaryString.charCodeAt(i);
    }
    return byteArray;
}


