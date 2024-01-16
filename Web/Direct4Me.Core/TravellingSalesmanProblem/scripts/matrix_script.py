import googlemaps
import json

# Replace with your Google Maps API key
API_KEY = ''

# Initialize Google Maps client
gmaps = googlemaps.Client(key=API_KEY)

# Load cities from JSON file
with open('cities.json', 'r') as file:
    data = json.load(file)
    cities = data['cities']

# Function to process a batch of cities
def process_batch(origins, destinations):
    try:
        matrix = gmaps.distance_matrix(origins, destinations, mode='driving')
        distances = [[element.get('distance', {}).get('value', 0) for element in row['elements']] for row in matrix['rows']]
        times = [[element.get('duration', {}).get('value', 0) for element in row['elements']] for row in matrix['rows']]
        return distances, times
    except Exception as e:
        print(f"Error processing batch: {e}")
        return [], []

# Split cities into batches and process
batch_size = 10
distance_matrix = []
time_matrix = []

for i in range(0, len(cities), batch_size):
    batch_origins = [(city['cordX'], city['cordY']) for city in cities[i:i+batch_size]]
    distances, times = process_batch(batch_origins, batch_origins)
    distance_matrix.extend(distances)
    time_matrix.extend(times)

# Add matrices to the JSON data
data['distanceMatrix'] = distance_matrix
data['timeMatrix'] = time_matrix

# Save the complete data to a new JSON file
with open('complete_data.json', 'w') as file:
    json.dump(data, file, indent=4)
