from keras.preprocessing.image import ImageDataGenerator
import os
import shutil
from fastapi import FastAPI, UploadFile, File, Response
from keras.models import load_model, Model
from keras.layers import Dense, Dropout
from keras.optimizers import Adam
from keras.utils import to_categorical
import numpy as np
import pickle
from pymongo import MongoClient
from extractFeatures import extract_face_features
import cv2
from sklearn.preprocessing import LabelEncoder


app = FastAPI()

# Constants
DB_URL = "mongodb+srv://bzorec:mHUURWihVWohmxBa@db.8rqwamq.mongodb.net/"
#DB_URL = "mongodb+srv://miselcucek:LKAQmmOTJu06QRzn@raimc.l14qt4n.mongodb.net/?retryWrites=true&w=majority"
DB_NAME = "db"  # Use the name of your database
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


def load_new_user_data(user_id):
    client = MongoClient(DB_URL)
    db = client[DB_NAME]
    collection = db[COLLECTION_NAME]

    features = []
    labels = []

    # create a directory for the user if it doesn't already exist
    if os.path.exists(str(user_id)):
        shutil.rmtree(str(user_id))
    os.makedirs(str(user_id))

    # save each image in the user's directory
    for entry in collection.find({'userId': user_id}):
        imageId = entry['_id']
        for idx, image in enumerate(entry['images']):
            img_np = np.array(image, dtype=np.uint8)
            cv2.imwrite(os.path.join(str(user_id), f'image_{imageId}.jpg'), img_np)

    datagen = ImageDataGenerator(
        rotation_range=20,
        width_shift_range=0.15,
        height_shift_range=0.15,
        brightness_range=[0.5, 1.0],
        shear_range=0.15,
        zoom_range=0.3,
        horizontal_flip=True,
        fill_mode='nearest'
    )

    for filename in os.listdir(str(user_id)):
        img_np = cv2.imread(os.path.join(str(user_id), filename))
        img_np = img_np.reshape((1,) + img_np.shape)  # reshape image for the ImageDataGenerator
        augmentations_per_image = 10

        for batch in datagen.flow(img_np, batch_size=1):
            augmented_image = batch[0].astype('uint8')  # make sure to cast the augmented image back to uint8
            face_features = extract_face_features(augmented_image)
            if face_features is not None:
                features.append(np.array(face_features))
                labels.append(user_id)

            augmentations_per_image -= 1
            if augmentations_per_image <= 0:
                break

    return np.array(features), np.array(labels)

def update_user_neuron_mapping(new_user_id):
    # Load the current mapping
    with open('user_neuron_mapping.pkl', 'rb') as f:
        user_ids, user_to_neuron = pickle.load(f)

    # Check if the new user is already in the mapping
    if new_user_id not in user_to_neuron:
        # The new user is not in the mapping, so we need to add it
        # We will assign the new user the next available neuron (which is just the current count of users)
        user_ids = np.append(user_ids, new_user_id)
        user_to_neuron[new_user_id] = len(user_to_neuron)

        # Save the updated mapping
        with open('user_neuron_mapping.pkl', 'wb') as f:
            pickle.dump((user_ids, user_to_neuron), f)

    return user_ids, user_to_neuron


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

    confidence_threshold = 110
    encoded_user_id, confidence = predict_user(img, model, user_ids, user_to_neuron)

    #temp_file.close()  # Don't forget to close the file

    if confidence < confidence_threshold:
        user_id = 'unknown'
        confidence = None
        print('User is unknown.')
    else:
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


@app.post("/transferLearning")
async def transfer_learning(user_id: str):
    # Load the model and mappings
    model = load_model('face_recognition_model.h5')
    with open('user_neuron_mapping.pkl', 'rb') as f:
        user_ids, user_to_neuron = pickle.load(f)

    update_user_neuron_mapping(user_id)

    # Load the new user data
    new_features, new_labels = load_new_user_data(user_id)

    # Check if user_id is already in the mapping
    if user_id not in user_to_neuron:
        user_to_neuron[user_id] = len(user_to_neuron)
        user_ids = np.append(user_ids, user_id)

    label_encoder = LabelEncoder()
    # Encode labels
    new_labels = label_encoder.transform(new_labels)
    new_labels = to_categorical(new_labels)

    # Preprocess features
    new_features = new_features.reshape(new_features.shape[0], -1)

    # Define a new model with an additional neuron in the output layer
    x = model.layers[-2].output  # get the output of the second last layer
    output = Dense(new_labels.shape[1], activation='softmax')(x)  # add a new output layer
    new_model = Model(inputs=model.input, outputs=output)  # define the new model

    new_model.compile(optimizer=Adam(lr=0.0001), loss='categorical_crossentropy', metrics=['accuracy'])

    # Perform the transfer learning on the new user data
    history = new_model.fit(new_features, new_labels, epochs=10, batch_size=32)

    # Save the updated model
    new_model.save('face_recognition_model.h5')

    # Update and save the mappings
    with open('user_neuron_mapping.pkl', 'wb') as f:
        pickle.dump((user_ids, user_to_neuron), f)

    return {"status": "success"}
