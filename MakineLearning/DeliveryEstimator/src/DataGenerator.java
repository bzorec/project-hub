import weka.core.Attribute;
import weka.core.DenseInstance;
import weka.core.Instances;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.List;
import java.util.Random;

public class DataGenerator {

    private static final int NUMBER_OF_DATA_INSTANCES = 10000;
    private static final int DELIVERY_START_HOUR = 8;
    private static final int DELIVERY_HOURS = 8;
    public static void main(String[] args) {

        Postbox[] postboxes = PostboxInitializer.initializePostboxes();

        try {
            generateAndSaveData("output_data.arff", postboxes);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private static void generateAndSaveData(String fileName, Postbox[] postboxes) throws IOException {
         // Create dataset
        ArrayList<Attribute> attributes = generateAttributes();
        Instances dataset = new Instances("DeliveryEstimate", attributes, 0);

        // Add instances to the dataset
        Random random = new Random();

        for (int i = 0; i < NUMBER_OF_DATA_INSTANCES; i++) {
            int p1 = random.nextInt(10) + 1;
            int p2 = random.nextInt(10) + 1;

            while (p1 == p2){
                p2 = random.nextInt(10) + 1;
            }
            int rTime = random.nextInt(DELIVERY_HOURS) + DELIVERY_START_HOUR;
            int rDay = random.nextInt(5) + 1;

            double timeCoefficient = getTimeCoefficient(rTime);
            double dayCoefficient = getDayCoefficient(rDay);

            String dayName = switch (rDay) {
                case 1 -> "MONDAY"; //mon
                case 2 -> "TUESDAY"; //tue
                case 3 -> "WEDNESDAY"; //wed
                case 4 -> "THURSDAY"; //thu
                case 5 -> "FRIDAY"; //fri
                default -> "NULL";
            };

            double timeNeededValue = switch (p2){
                case 2 -> postboxes[p1-1].timeToGetToPostbox_2;
                case 3 -> postboxes[p1-1].timeToGetToPostbox_3;
                case 4 -> postboxes[p1-1].timeToGetToPostbox_4;
                case 5 -> postboxes[p1-1].timeToGetToPostbox_5;
                case 6 -> postboxes[p1-1].timeToGetToPostbox_6;
                case 7 -> postboxes[p1-1].timeToGetToPostbox_7;
                case 8 -> postboxes[p1-1].timeToGetToPostbox_8;
                case 9 -> postboxes[p1-1].timeToGetToPostbox_9;
                case 10 -> postboxes[p1-1].timeToGetToPostbox_10;
                default -> postboxes[p1-1].timeToGetToPostbox_1;
            };

            timeNeededValue *= timeCoefficient;
            timeNeededValue *= dayCoefficient;

            // Create an instance and add it to the dataset
            DenseInstance instance = new DenseInstance(5);
            instance.setValue(attributes.get(0), "Postbox" + p1);
            instance.setValue(attributes.get(1), "Postbox" + p2);
            instance.setValue(attributes.get(2), rTime);
            instance.setValue(attributes.get(3), dayName);
            instance.setValue(attributes.get(4), (int)timeNeededValue);

            dataset.add(instance);
        }

        // Save dataset to .arff file
        BufferedWriter writer = new BufferedWriter(new FileWriter(fileName));
        writer.write(dataset.toString());
        writer.flush();
        writer.close();
    }

    private static ArrayList<Attribute> generateAttributes() {

        ArrayList<Attribute> attributes = new ArrayList<>();

        List<String> postboxValues = new ArrayList<>();
        postboxValues.add("Postbox1");
        postboxValues.add("Postbox2");
        postboxValues.add("Postbox3");
        postboxValues.add("Postbox4");
        postboxValues.add("Postbox5");
        postboxValues.add("Postbox6");
        postboxValues.add("Postbox7");
        postboxValues.add("Postbox8");
        postboxValues.add("Postbox9");
        postboxValues.add("Postbox10");

        List<String> dayValues = new ArrayList<>();
        dayValues.add("MONDAY");
        dayValues.add("TUESDAY");
        dayValues.add("WEDNESDAY");
        dayValues.add("THURSDAY");
        dayValues.add("FRIDAY");
        dayValues.add("NULL");

        Attribute postbox1 = new Attribute("postbox1",postboxValues);
        Attribute postbox2 = new Attribute("postbox2", postboxValues);
        Attribute timeOfDay = new Attribute("timeOfDay");
        Attribute dayOfWeek = new Attribute("dayOfWeek",dayValues);
        Attribute timeNeeded = new Attribute("timeNeeded");

        attributes.add(postbox1);
        attributes.add(postbox2);
        attributes.add(timeOfDay);
        attributes.add(dayOfWeek);
        attributes.add(timeNeeded);

        return attributes;
    }

    private static double getTimeCoefficient(int hour) {
        // Extract hour from the time string

        // Apply time coefficients
        if (hour < 10) {
            return 1.0;
        } else if (hour < 11) {
            return 1.1;
        }
        else if (hour <= 13) {
            return 1.3;
        }
        else if (hour == 14) {
            return 1.1;
        } else {
            return 1.0;
        }
    }

    private static double getDayCoefficient(int day) {
        // Apply day coefficients
        return switch (day) {
            case 1 -> 1.5; //mon 10 -> 15 min
            case 2 -> 1.4; //tue 10 -> 14 min
            case 3 -> 1.4; //wed 10 -> 14 min
            case 4 -> 1.1; //thu 10 -> 11 min
            case 5 -> 0.8; //fri 10 -> 8 min
            default -> 1.0; // 10 -> 10 min
        };
    }

}
