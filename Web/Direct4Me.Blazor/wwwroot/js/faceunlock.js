window.faceUnlock = {
    enable: function () {
        if (!navigator.mediaDevices || !navigator.mediaDevices.getUserMedia) {
            // Check if the browser supports media devices and getUserMedia
            alert("Sorry, your browser doesn't support camera access.");
            return null;
        }

        // Access the user's camera and capture an image
        return navigator.mediaDevices.getUserMedia({video: true})
            .then(function (stream) {
                const videoElement = document.createElement("video");
                videoElement.srcObject = stream;
                videoElement.play();

                // Create a canvas element to draw the captured image
                const canvasElement = document.createElement("canvas");
                canvasElement.width = videoElement.videoWidth;
                canvasElement.height = videoElement.videoHeight;

                // Draw the video frame on the canvas
                const context = canvasElement.getContext("2d");
                context.drawImage(videoElement, 0, 0, canvasElement.width, canvasElement.height);

                // Convert the image data to a byte array
                const imageData = context.getImageData(0, 0, canvasElement.width, canvasElement.height);
                const byteArray = new Uint8Array(imageData.data.buffer);

                // Stop the camera stream
                stream.getTracks().forEach(track => track.stop());

                return byteArray;
            })
            .catch(function (error) {
                console.error("Error accessing camera:", error);
                return null;
            });
    }
};
