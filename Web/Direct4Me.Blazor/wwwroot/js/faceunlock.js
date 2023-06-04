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

window.captureImageFromVideo = function () {
    var video = document.getElementById('camera-stream');
    if (!video || video.paused || video.ended) {
        console.log('Video is not ready');
        return null;
    }

    var canvas = document.createElement('canvas');
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    var context = canvas.getContext('2d');
    context.drawImage(video, 0, 0, video.videoWidth, video.videoHeight);

    // Convert image to byte array
    var dataUrl = canvas.toDataURL('image/jpeg');
    var base64 = dataUrl.split(',')[1];
    var binary = atob(base64);
    var array = [];
    for (var i = 0; i < binary.length; i++) {
        array.push(binary.charCodeAt(i));
    }
    return new Uint8Array(array);
};
