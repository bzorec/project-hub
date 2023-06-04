let canvasElement;
let context;
let byteArrayResolver;
let capturedImages = [];

window.faceUnlock = {
    enable: function (userId) {
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
    },
    enableFaceUnlock: function (userId) {
        return new Promise((resolve) => {
            byteArrayResolver = resolve;
            window.faceUnlock.enable(userId);
        });
    }
};

function captureImage(userId) {
    // Draw the current video frame on the canvas
    context.drawImage(videoElement, 0, 0, canvasElement.width, canvasElement.height);

    // Convert the canvas image to a data URL
    const imageUrl = canvasElement.toDataURL();

    // Convert the data URL to a byte array
    const byteArray = base64ToByteArray(imageUrl);

    // Save the byte array to the capturedImages array
    capturedImages.push(byteArray);

    if (capturedImages.length < 5) {
        // Show the modal again to capture more images
        const imageModal = new bootstrap.Modal(document.getElementById("imageModal"));
        imageModal.show();
    } else {
        // Close the modal and send the captured images to the API
        const imageModal = new bootstrap.Modal(document.getElementById("imageModal"));
        imageModal.hide();

        sendCapturedImages(userId);
    }
}

function sendCapturedImages(userId) {

    for (let i = 0; i < capturedImages.length; i++) {
        const imageData = capturedImages[i];

        // Create a new FormData object to send the image data
        const formData = new FormData();
        formData.append("userId", userId);
        formData.append("image", new Blob([imageData]));

        // Send the POST request to the API endpoint
        fetch("/uploadImage", {
            method: "POST",
            body: formData
        })
            .then(response => response.json())
            .then(data => {
                console.log(data); // Handle the API response if needed
            })
            .catch(error => {
                console.error("Error uploading image:", error);
            });
    }

    // Clear the capturedImages array
    capturedImages = [];
}

function base64ToByteArray(base64String) {
    const binaryString = window.atob(base64String.split(",")[1]);
    const byteArray = new Uint8Array(binaryString.length);
    for (let i = 0; i < binaryString.length; i++) {
        byteArray[i] = binaryString.charCodeAt(i);
    }
    return byteArray;
}
