# Delivery Estimator

The Delivery Estimator is an AI-powered model designed to predict the delivery time between two postboxes based on historical data of delivery routes. This README provides an overview of the project, including a mock data generator, the learning model, and the implementation details.

Our plan is to incorporate this into the TSP solution we have already implemented.

## Mock Data Generator

### Description
The mock data generator is a tool designed to create simulated data for training the delivery time prediction model. It assigns rough time estimations for delivery routes between 10 postboxes (100 routes). The current values used can be checked in the (`PostboxInitializer.java`). The generator incorporates time of day coefficients to simulate rush hour traffic and day of the week coefficients to account for busier days.

### Usage
To generate mock data, run the provided Java application (`DataGenerator.java`). This will create a .arff file containing discretized data of hourOfDay and estimated delivery time. The generator utilizes a simple algorithm to simulate varying conditions affecting delivery times.

## Model Learning

### Training
To train the model, the generated data needs to be discretized. Then the data is fed into the J48 classifier. The model builds a decision tree to predict delivery times based on input features such as postbox IDs, time of day, and day of the week.

## Implementation within the Project

### Description
The trained model is integrated into the project to provide accurate delivery time predictions. Notifications are sent to users at the beginning of each workday, informing them of the proposed delivery times. This implementation enhances efficiency and helps users plan their day accordingly.
