window.initializeCamera = function () {
    return new Promise(function (resolve, reject) {
        // Check for camera availability
        if (!navigator.mediaDevices || !navigator.mediaDevices.getUserMedia) {
            reject('Camera not available');
            return;
        }

        // Request permission to use the camera
        navigator.mediaDevices.getUserMedia({video: true})
            .then(function (stream) {
                // If permission is granted, assign the video stream to the video element
                var video = document.getElementById('camera-stream');
                if ('srcObject' in video) {
                    video.srcObject = stream;
                } else {
                    // Older browsers may not have srcObject
                    video.src = window.URL.createObjectURL(stream);
                }

                video.onloadedmetadata = function () {
                    video.play();
                    resolve(); // The stream is ready
                };
            })
            .catch(function (err) {
                // An error occurred
                reject('An error occurred: ' + err);
            });
    });
};

window.playVideo = function (videoElement) {
    videoElement.play();
};

window.captureImageFromVideo = async function () {
    try {
        var video = document.getElementById('camera-stream');
        if (!video || video.paused || video.ended) {
            console.log('Video is not ready');
            return null;
        }

        // Create a temporary canvas to hold the resized image
        var tempCanvas = document.createElement('canvas');
        var tempContext = tempCanvas.getContext('2d');

        // Set the desired dimensions for the resized image
        var maxWidth = 800; // Set your desired maximum width
        var maxHeight = 600; // Set your desired maximum height

        // Calculate the scaled dimensions
        var videoWidth = video.videoWidth;
        var videoHeight = video.videoHeight;
        var scaleFactor = Math.min(maxWidth / videoWidth, maxHeight / videoHeight);
        var scaledWidth = videoWidth * scaleFactor;
        var scaledHeight = videoHeight * scaleFactor;

        // Resize the image using the temporary canvas
        tempCanvas.width = scaledWidth;
        tempCanvas.height = scaledHeight;
        tempContext.drawImage(video, 0, 0, scaledWidth, scaledHeight);

        // Convert the resized image to a byte array
        var resizedDataUrl = tempCanvas.toDataURL('image/jpeg');
        var resizedBase64 = resizedDataUrl.split(',')[1];
        var resizedBinary = atob(resizedBase64);
        var resizedArray = new Uint8Array(resizedBinary.length);
        for (var i = 0; i < resizedBinary.length; i++) {
            resizedArray[i] = resizedBinary.charCodeAt(i);
        }

        return resizedArray;
    } catch (e) {
        console.log(e);
        return null;
    }
};
