from keras.models import Sequential
from keras.layers import Dense, Dropout
from keras.optimizers import Adam
from keras.preprocessing.image import ImageDataGenerator
from keras.callbacks import EarlyStopping, ModelCheckpoint
from sklearn.preprocessing import LabelEncoder
from sklearn.model_selection import train_test_split
from keras.utils import to_categorical
import numpy as np
import pickle
from pymongo import MongoClient
from extractFeatures import extract_face_features
import matplotlib.pyplot as plt
import os
import cv2
import shutil

#DB_URL = "mongodb+srv://bzorec:mHUURWihVWohmxBa@db.8rqwamq.mongodb.net/"
DB_URL = "mongodb+srv://miselcucek:LKAQmmOTJu06QRzn@raimc.l14qt4n.mongodb.net/?retryWrites=true&w=majority"
DB_NAME = "raiMC" 
COLLECTION_NAME = "imgAuth"


def load_data_from_mongodb():
    client = MongoClient(DB_URL)
    db = client[DB_NAME]
    collection = db[COLLECTION_NAME]

    features = []
    labels = []

    # create a set of all unique userIds
    unique_user_ids = set(doc['userId'] for doc in collection.find({}, {"_id": 0, "userId": 1}))

    # create a directory for each unique userId if it doesn't already exist
    for user_id in unique_user_ids:
        if os.path.exists(str(user_id)):
            shutil.rmtree(str(user_id))
        os.makedirs(str(user_id))

    # save each image in its corresponding userId directory
    for entry in collection.find():
        imageId = entry['_id']
        for idx, image in enumerate(entry['images']):
            img_np = np.array(image, dtype=np.uint8)
            cv2.imwrite(os.path.join(str(entry['userId']), f'image_{imageId}.jpg'), img_np)

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

    for user_id in unique_user_ids:
        for filename in os.listdir(str(user_id)):
            img_np = cv2.imread(os.path.join(str(user_id), filename))
            img_np = img_np.reshape((1,) + img_np.shape)  # reshape image for the ImageDataGenerator
            augmentations_per_image = 30

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


features, labels = load_data_from_mongodb()

label_encoder = LabelEncoder()
labels = label_encoder.fit_transform(labels)
labels = to_categorical(labels)

user_ids = label_encoder.classes_
user_to_neuron = {user_id: i for i, user_id in enumerate(user_ids)}
with open('user_neuron_mapping.pkl', 'wb') as f:
    pickle.dump((user_ids, user_to_neuron), f)

features = features.reshape(features.shape[0], -1)
features_train, features_test, labels_train, labels_test = train_test_split(features, labels, test_size=0.2, random_state=42)

# Model modification
model = Sequential()
model.add(Dense(256, activation='relu', input_shape=(features_train.shape[1],)))
model.add(Dropout(0.4)) # reduced dropout
model.add(Dense(128, activation='relu')) # increased neurons
model.add(Dropout(0.4)) # reduced dropout
model.add(Dense(labels_train.shape[1], activation='softmax'))

# Adam optimizer with reduced learning rate
model.compile(optimizer=Adam(lr=0.0001), loss='categorical_crossentropy', metrics=['accuracy'])

callbacks = [
    #EarlyStopping(monitor='val_loss', patience=7), # patience increased
    ModelCheckpoint(filepath='best_model.h5', monitor='val_loss', save_best_only=True)
]

# Increased epochs and modified batch size
history = model.fit(features_train, labels_train, epochs=30, batch_size=64, validation_split=0.2, callbacks=callbacks)
# Save the model
model.save('face_recognition_model.h5')

# Save history object as a pickle file
with open('train_history.pkl', 'wb') as f:
    pickle.dump(history.history, f)

# Evaluate on the test set
_, test_acc = model.evaluate(features_test, labels_test)
print(f'Test Accuracy: {test_acc}')

# Summarize history for accuracy
plt.figure(figsize=[8,6])
plt.plot(history.history['acc'])
plt.plot(history.history['val_acc'])
plt.title('Model Accuracy')
plt.ylabel('Accuracy')
plt.xlabel('Epoch')
plt.legend(['Training', 'Validation'], loc='upper left')
plt.savefig('accuracy_plot.png')
plt.show()

# Summarize history for loss
plt.figure(figsize=[8,6])
plt.plot(history.history['loss'])
plt.plot(history.history['val_loss'])
plt.title('Model Loss')
plt.ylabel('Loss')
plt.xlabel('Epoch')
plt.legend(['Training', 'Validation'], loc='upper left')
plt.savefig('loss_plot.png')
plt.show()

