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

app = FastAPI()

# Constants
#DB_URL = "mongodb+srv://bzorec:mHUURWihVWohmxBa@db.8rqwamq.mongodb.net/"
DB_URL = "mongodb+srv://miselcucek:LKAQmmOTJu06QRzn@raimc.l14qt4n.mongodb.net/?retryWrites=true&w=majority"
DB_NAME = "raiMC"  # Use the name of your database
COLLECTION_NAME = "imgAuth"  # Use the name of your collection
IMG_SIZE = 96  # The size to resize images to

# Establish database connection
client = MongoClient(DB_URL)
# Send a ping to confirm a successful connection
try:
    client.admin.command('ping')
    print("Pinged your deployment. You successfully connected to MongoDB!")
except Exception as e:
    print(e)

db = client[DB_NAME]
collection = db[COLLECTION_NAME]

# Load the model
model = load_model('face_recognition_model.h5')

# Function to predict the user from an image file
def predict_user(image):
    face_features = extract_face_features(image)
    if face_features is not None:
        face_features = np.expand_dims(face_features, axis=0)
        predictions = model.predict(face_features)
        predicted_class = np.argmax(predictions)
        confidence = predictions[0, predicted_class]
        return predicted_class, confidence
    return None, None


@app.post("/getImageContents")
async def getImage(imgPath: str):
    # Read image file
    with open(imgPath, "rb") as file:
        file_content = file.read()

    # Decode image file content as image
    nparr = np.frombuffer(file_content, np.uint8)
    img = cv2.imdecode(nparr, cv2.IMREAD_COLOR)

    # Process the image as needed
    # ...

    # Return image bytes as raw response
    ret, image_bytes = cv2.imencode('.png', img)
    return Response(content=image_bytes.tobytes(), media_type="image/png")
    
#    encoded_image = cv2.imencode(".jpg", img)
#    encoded_image_str = encoded_image.tostring()
#    file_like = BytesIO(encoded_image_str)
#    image_file = UploadFile(file=file_like, filename="image.jpg")
#    return image_file


@app.post("/uploadImage")
async def upload_image(userId: str, image: UploadFile = File(...)):
    # Read image file SUPPORTED 720 X 720
    contents = await image.read()
    nparr = np.fromstring(contents, np.uint8)
    img = cv2.imdecode(nparr, cv2.IMREAD_COLOR)
    
    # Check if an entry for this user already exists
    #user_entry = collection.find_one({'userId': userId})
    
    #if user_entry is not None:
        # If the user entry exists, append the new image to the images array
        #updated_images = user_entry['images'] + [img.tolist()]
        #collection.update_one({'userId': userId}, {'$set': {'images': updated_images}})
    #else:
        # If the user entry does not exist, create a new entry
    collection.insert_one({'userId': userId, 'images': [img.tolist()]})
    
    return {"status": "success"}


@app.post("/imgAuthenticate")
async def img_authenticate(image: UploadFile = File(...)):
    # Read image file
    contents = await image.read()
    nparr = np.fromstring(contents, np.uint8)
    img = cv2.imdecode(nparr, cv2.IMREAD_COLOR)

    user_id, confidence = predict_user(img)

    if user_id is not None:
        print(f'Predicted user_id: {user_id} with confidence {confidence}')
    else:
        print('Prediction failed.')

    return {"user_id": user_id, "confidence": confidence}


@app.get("/helloWorld")
async def hello():
    return "helloWorld"
