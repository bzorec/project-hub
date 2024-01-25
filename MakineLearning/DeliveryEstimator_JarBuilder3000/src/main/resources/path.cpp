#include <iostream>
#include <cstdlib>
#include <ctime>
#include <fstream>
#include <vector>

const int numPackageBoxes = 10;
const int numVisits = 10;
const int startHour = 6;
const int endHour = 15;
const int deliveryTimePerPackage = 1;
const int numInstances = 1000;

// Funkcija za generiranje naključnega zaporedja indeksov paketnikov
void generateRandomSequence(std::vector<int>& sequence, int size) {
    for (int i = 0; i < size; ++i) {
        sequence.push_back(rand() % numPackageBoxes + 1); // +1, da začnemo z 1
    }
}

int main() {
    srand(time(0));

    std::ofstream arffFile("delivery_data.arff");
    if (!arffFile.is_open()) {
        std::cerr << "Napaka pri odpiranju datoteke ARFF." << std::endl;
        return 1;
    }

    // Zapis glave datoteke ARFF
    arffFile << "@relation delivery_data" << std::endl;

    // Zapis atributov za vsak paketnik (p1 do p10) in numPeopleHome
    for (int i = 1; i <= 10; ++i) {
        arffFile << "@attribute p" << i << " numeric" << std::endl;
    }
    arffFile << "@attribute numPeopleHome {0,1,2,3}" << std::endl;

    arffFile << "@data" << std::endl;

    // Za vsako instanco
    for (int instance = 0; instance < numInstances; ++instance) {
        std::vector<int> deliverySequence;
        generateRandomSequence(deliverySequence, numVisits);

        int totalDeliveries = 0;

        // Zapis podatkov za vsak obisk
        for (int i = 0; i < numVisits; ++i) {
            int currentTime = startHour + i;
            int packageBoxIndex = deliverySequence[i];

            // Preveri, ali je oseba doma na tem paketniku
            int numPeopleHome = rand() % 2; // Naključno generiraj 0 ali 1

            // Posodobi skupno število dostav
            totalDeliveries += numPeopleHome;

        }
        // Podatke zapiši v formatu ARFF
        // Zapis zaporedja obiskov in skupnega števila dostav za vsako instanco
        for (int j = 0; j < numVisits; ++j) {
            arffFile << deliverySequence[j] << (j == numVisits - 1 ? "" : ",");
        }
        arffFile << ",";
        // Dodaj diskretiziran atribut numPeopleHome
        if (totalDeliveries >= 0 && totalDeliveries <= 3) {
            arffFile << "0";
        }
        else if (totalDeliveries > 3 && totalDeliveries <= 6) {
            arffFile << "1";
        }
        else if (totalDeliveries > 6 && totalDeliveries <= 9) {
            arffFile << "2";
        }
        else {
            arffFile << "3";
        }
        arffFile << std::endl;

        // Izhodna statistika za vsako instanco
        std::cout << "Instanca " << instance + 1 << ": Skupno število dostav = " << totalDeliveries << std::endl;
        std::cout << "-------------------------------------------" << std::endl;
    }

    arffFile.close();
    return 0;
}