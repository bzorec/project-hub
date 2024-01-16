import googlemaps
import json

# Replace with your Google Maps API key
API_KEY = ''

# Initialize Google Maps client
gmaps = googlemaps.Client(key=API_KEY)

# List of addresses to geocode
addresses = [
    "Poljanski nasip 30, Ljubljana, Slovenia",
    "Pražakova ulica 3, Ljubljana, Slovenia",
    "Zaloška cesta 57, Ljubljana, Slovenia",
    "Dunajska cesta 141, Ljubljana, Slovenia",
    "Riharjeva ulica 38, Ljubljana, Slovenia",
    "Vodnikova cesta 235, Ljubljana, Slovenia",
    "Resljeva cesta 14, Ljubljana, Slovenia",
    "Prušnikova ulica 2, Ljubljana Šentvid, Slovenia",
    "Glavarjeva cesta 39, Komenda, Slovenia",
    "Dunajska cesta 361, Ljubljana Črnuče, Slovenia",
    "Trdinov trg 8 A, Mengeš, Slovenia",
    "Ljubljanska cesta 14 A, Kamnik, Slovenia",
    "Vegova ulica 1, Moravče, Slovenia",
    "Zadobrovška cesta 14, Ljubljana Polje, Slovenia",
    "Partizanska cesta 7, Grosuplje, Slovenia",
    "Ploščad Osvobodilne fronte 4, Ivančana Gorica, Slovenia",
    "Kolodvorska ulica 2, Ribnica, Slovenia",
    "Ljubljanska cesta 23, Kočevje, Slovenia",
    "Podpeška cesta 20, Brezovica pri Ljubljani, Slovenia",
    "Molkov trg 12, Borovnica, Slovenia",
    "Poštna ulica 2, Vrhnika, Slovenia",
    "Cesta zmage 28, Zagorje ob Savi, Slovenia",
    "Trg revolucije 27, Trbovlje, Slovenia",
    "Trg Franca Fakina 4, Trbovlje, Slovenia",
    "Istrska ulica 49, Maribor, Slovenia",
    "Šarhova ulica 59 A, Maribor, Slovenia",
    "Sladki Vrh 3 A, Sladki Vrh, Slovenia",
    "Malečnik 56, Malečnik, Slovenia",
    "Partizanska cesta 3, Lenart v Slovenskih goricah, Slovenia",
    "Čolnikov trg 9, Benedikt, Slovenia",
    "Cesta k Dravi 5, Spodnji Duplek, Slovenia",
    "Mariborska Cesta 19, Ptuj, Slovenia",
    "Ivanjkovci 9 B, Ivanjkovci, Slovenia",
    "Poštna ulica 2, Ormož, Slovenia",
    "Gorišnica 79, Gorišnica, Slovenia",
    "Videm pri Ptuju 51 A, Videm pri Ptuju, Slovenia",
    "Mariborska ulica 26, Zgornja Polskava, Slovenia",
    "Makole 37, Makole, Slovenia",
    "Kopališka ulica 2, Kidričevo, Slovenia",
    "Ljubljanska cesta 14, Rače, Slovenia",
    "Cesta v Rošpoh 18, Kamnica, Slovenia",
    "Slovenski trg 4, Selnica ob Dravi, Slovenia",
    "Obrobna ulica 1, Brestrnica, Slovenia",
    "Glavni trg 31, Muta, Slovenia",
    "Trg 4. julija 1, Dravograd, Slovenia",
    "Trg svobode 19, Ravne na Koroškem, Slovenia",
    "Trg 32 A, Prevalje, Slovenia",
"Center 16, Črna na Koroškem, Slovenia",
"Krekov trg 9, Celje, Slovenia",
"Aškerčev trg 26, Šmarje pri Jelšah, Slovenia",
"Ulica heroja Staneta 1, Žalec, Slovenia",
"Malteška cesta 38, Polzela, Slovenia",
"Savinjska cesta 3, Mozirje, Slovenia",
"Dražgoška ulica 8, Kranj, Slovenia",
"Škofjeloška cesta 17, Kranj, Slovenia",
"Jezerska cesta 41, Kranj, Slovenia",
"Ulica Lojzeta Hrovata 2, Kranj, Slovenia",
"Glavna cesta 24, Naklo, Slovenia",
"Gasilska cesta 2 A, Šenčur, Slovenia",
"Kapucinski trg 14, Škofja Loka, Slovenia",
"Trg svobode 1, Žiri, Slovenia",
"Na Kresu 1, Železniki, Slovenia",
"Alpska cesta 37 B, Lesce, Slovenia",
"Trg svobode 2 C, Bohinjska Bistrica, Slovenia",
"Cesta Cirila Tavčarja 8, Jesenice, Slovenia",
"Žirovnica 4, Žirovnica, Slovenia",
"Borovška cesta 92, Kranjska Gora, Slovenia",
"Predilniška cesta 10, Tržič, Slovenia",
"Kidričeva ulica 19, Nova Gorica, Slovenia",
"Industrijska cesta 9, Nova Gorica, Slovenia",
"Trg maršala Tita 10, Tolmin, Slovenia",
"Goriška cesta 24, Ajdovščina, Slovenia",
"Vodnikova ulica 1, Idrija, Slovenia",
"Vrtojbenska cesta 19 C, Šempeter pri Gorici, Slovenia",
"Kolodvorska cesta 9, Koper, Slovenia",
"Partizanska cesta 48 A, Sežana, Slovenia",
"Ulica 1. maja 2 A, Postojna, Slovenia",
"Cankarjev drevored 1, Izola, Slovenia",
"Ulica Slavka Gruma 7, Novo mesto, Slovenia",
"Novi trg 7, Novo mesto, Slovenia",
"Goliev trg 11, Trebnje, Slovenia",
"Cesta na Fužine 3, Mirna, Slovenia",
"Ulica stare pravde 34, Brežice, Slovenia",
"Ulica bratov Gerjovičev 52, Dobova, Slovenia",
"Jesenice 9, Jesenice na Dolenjskem, Slovenia",
"Trg Matije Gubca 1, Krško, Slovenia",
"Trg svobode 9, Sevnica, Slovenia",
"Prvomajska cesta 3, Šentjernej, Slovenia",
"Naselje Borisa Kidriča 2, Metlika, Slovenia",
"Kolodvorska cesta 30 A, Črnomelj, Slovenia",
"Trg Zmage 6, Murska Sobota, Slovenia",
"Gornji Petrovci 40 E, Petrovci, Slovenia",
"Trg ljudske pravice 7, Lendava, Slovenia",
"Ulica Prekmurske čete 14, Črenšovci, Slovenia",
"Cesta na Stadion 1, Gornja Radgona, Slovenia",
"Panonska cesta 5, Radenci, Slovenia"
]

# Geocoding addresses
cities = []
for index, address in enumerate(addresses):
    geocode_result = gmaps.geocode(address)
    if geocode_result:
        location = geocode_result[0]['geometry']['location']
        city = {
            "index": index,
            "cordX": location['lat'],
            "cordY": location['lng']
        }
        cities.append(city)

# Save the geocoded addresses to a JSON file
with open('cities.json', 'w') as file:
    json.dump({"cities": cities}, file, indent=4)
