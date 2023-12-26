FROM python:3.7

# Set the working directory in the container to /app
WORKDIR /application

# Update package lists and install necessary libraries
RUN apt-get update && apt-get install -y libgl1-mesa-glx

# Add the current directory contents into the container at /app
ADD . /application

# Install any needed packages specified in requirements.txt
#RUN pip install --no-cache-dir -r requirements.txt
RUN pip install -r requirements.txt

# Make port 8000 available to the world outside this container
EXPOSE 8000

# Ensure models are available
COPY models/face_recognition_model.h5 /application
COPY models/mmod_human_face_detector.dat /application
COPY models/shape_predictor_68_face_landmarks.dat /application
COPY models/user_neuron_mapping.pkl /application

# Run app.py when the container launches
#CMD ["python", "trainModel.py"]
#CMD ["python", "extractFeatures.py"]
CMD ["python", "-m", "uvicorn", "imageAuthenticationApi:app", "--host", "0.0.0.0", "--port", "8000"]
