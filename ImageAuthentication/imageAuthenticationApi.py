from fastapi import FastAPI, UploadFile, File, Response
from typing import List
from pymongo.mongo_client import MongoClient
from pymongo.server_api import ServerApi
import cv2
import numpy as np
import io
from keras.models import load_model
from extractFeatures import extract_face_features
from io import BytesIO
import pickle
import tempfile  # To create temporary files

app = FastAPI()

# Constants
#DB_URL = "mongodb+srv://bzorec:mHUURWihVWohmxBa@db.8rqwamq.mongodb.net/"
DB_URL = "mongodb+srv://miselcucek:LKAQmmOTJu06QRzn@raimc.l14qt4n.mongodb.net/?retryWrites=true&w=majority"
DB_NAME = "raiMC"  # Use the name of your database
COLLECTION_NAME = "imgAuth"  # Use the name of your collection
IMG_SIZE = 96  # The size to resize images to

# Establish database connection
client = MongoClient(DB_URL)
try:
    client.admin.command('ping')
    print("Pinged your deployment. You successfully connected to MongoDB!")
except Exception as e:
    print(e)

db = client[DB_NAME]
collection = db[COLLECTION_NAME]

# Move model and mappings loading into the functions where they're used.

@app.post("/getImageContents")
async def getImage(imgPath: str):
    with open(imgPath, "rb") as file:
        file_content = file.read()

    nparr = np.frombuffer(file_content, np.uint8)
    img = cv2.imdecode(nparr, cv2.IMREAD_COLOR)

    ret, image_bytes = cv2.imencode('.png', img)
    return Response(content=image_bytes.tobytes(), media_type="image/png")


@app.post("/uploadImage")
async def upload_image(userId: str, image: UploadFile = File(...)):
    contents = await image.read()
    nparr = np.frombuffer(contents, np.uint8)
    img = cv2.imdecode(nparr, cv2.IMREAD_COLOR)

    collection.insert_one({'userId': userId, 'images': [img.tolist()]})

    return {"status": "success"}


@app.post("/imgAuthenticate")
async def img_authenticate(image: UploadFile = File(...)):
    # Load the model and mappings here so it won't fail on import if there's an issue
    model = load_model('face_recognition_model.h5')
    with open('user_neuron_mapping.pkl', 'rb') as f:
        user_ids, user_to_neuron = pickle.load(f)

    contents = await image.read()
    nparr = np.frombuffer(contents, np.uint8)
    img = cv2.imdecode(nparr, cv2.IMREAD_COLOR)

    # Save the image to a temporary file
    #temp_file = tempfile.NamedTemporaryFile(delete=False, suffix='.png')
    #cv2.imwrite(temp_file.name, img)

    encoded_user_id, confidence = predict_user(img, model, user_ids, user_to_neuron)

    #temp_file.close()  # Don't forget to close the file

    if encoded_user_id is not None:
        user_id = user_ids[encoded_user_id]
        print(f'Predicted user_id: {user_id} with confidence {confidence}')
    else:
        print('Prediction failed.')
        user_id = None

    if isinstance(user_id, np.int64):
        user_id = int(user_id)
    if isinstance(confidence, np.float32):
        confidence = float(confidence)

    return {"user_id": user_id, "confidence": confidence}


@app.get("/helloWorld")
async def hello():
    return "helloWorld"


def predict_user(image, model, user_ids, user_to_neuron):
    face_features = extract_face_features(image)
    if face_features is not None:
        face_features = np.expand_dims(face_features, axis=0)
        flat_data = np.reshape(face_features, (face_features.shape[0], -1))
        predictions = model.predict(flat_data)
        predicted_class = np.argmax(predictions)
        confidence = predictions[0, predicted_class]
        return predicted_class, confidence
    return None, None
