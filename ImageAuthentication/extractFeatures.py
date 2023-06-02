import cv2
import dlib
import numpy as np

# Load the detector
detector = dlib.get_frontal_face_detector()

# Load the predictor
predictor = dlib.shape_predictor("shape_predictor_68_face_landmarks.dat")

def extract_face_features(img):
    # read the image
    #img = cv2.imread(image_path)
    
    # Convert image into grayscale
    gray = cv2.cvtColor(src=img, code=cv2.COLOR_BGR2GRAY).astype('uint8')
    
    # Use detector to find faces
    #cv2.imshow("gray", gray)
    #cv2.waitKey(0)
    
    cnn_face_detector = dlib.cnn_face_detection_model_v1('mmod_human_face_detector.dat')
    faces = cnn_face_detector(gray)
    
    #faces = detector(gray)
    face_features = []
    
    if len(faces) > 0:
        print("Extracting.\n")
        # Assume the first face is the one we want
        face = faces[0]
        
        # get the rectangle object from the mmod_rectangle object
        rect = face.rect
        
        x1 = rect.left()  # left point
        y1 = rect.top()  # top point
        x2 = rect.right()  # right point
        y2 = rect.bottom()  # bottom point
        
        # Create landmark object
        landmarks = predictor(image=gray, box=rect)
        
        # Loop through all the points
        for n in range(0, 68):
            x = landmarks.part(n).x
            y = landmarks.part(n).y
            # append x and y separately
            face_features.extend([x, y])
            
            # Draw a circle
            cv2.circle(img=img, center=(x, y), radius=3, color=(0, 255, 0), thickness=-1)
    else:
        print("No faces detected in the image.")
    
    return face_features


# Test on an image
#features = extract_face_features('img.png')
#if features is not None:
#    print(features)
#else:
#    print("Feature extraction failed.")
