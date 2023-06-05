from keras.models import load_model
from keras.preprocessing import image
from mtcnn import MTCNN
from keras.models import Model
import numpy as np
import cv2
from keras_vggface.vggface import VGGFace
from keras_vggface.utils import preprocess_input

# Load the VGGFace model
base_model = VGGFace(model='resnet50', include_top=False)
# Remove the last layer to get features instead of predictions
model = Model(inputs=base_model.inputs, outputs=base_model.layers[-2].output)


# Instantiate a MTCNN detector for face detection and alignment
mtcnn_detector = MTCNN()

def create_vggface_model():
    # load the VGGFace model (for example, VGG16)
    base_model = VGGFace(model='vgg16', include_top=False)
    model = Model(inputs=base_model.inputs, outputs=base_model.layers[-2].output)
    return model


def extract_face_features(image):
    # Convert the image from BGR to RGB
    image_rgb = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)

    # Detect faces in the image
    faces = mtcnn_detector.detect_faces(image_rgb)

    if len(faces) == 0:
        print("No faces detected in the image.")
        return None

    print("extracting")
    # Assume the first face is the one we want
    face = faces[0]

    # Extract the bounding box from the requested face
    x, y, width, height = face['box']

    # Extract the face from the image using the bounding box coordinates
    face_img = image_rgb[y:y + height, x:x + width]

    # Resize the image to 224x224 for the VGGFace model
    face_img = cv2.resize(face_img, (224, 224))

    # Preprocess the image for the VGGFace model
    face_img = face_img.astype('float64')
    face_img = np.expand_dims(face_img, axis=0)
    face_img = preprocess_input(face_img, version=2)  # version 2 is used for RESNET50

    # Extract features from the face
    face_features = model.predict(face_img)[0]

    return face_features



