package org.example;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import org.example.models.DeliveryJsonModel;
import org.example.utils.JsonUtil;
import org.example.utils.ModelLoader;
import weka.classifiers.trees.J48;
import weka.classifiers.trees.RandomForest;
import weka.core.Attribute;
import weka.core.DenseInstance;
import weka.core.Instances;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class DeliveryEstimator {
    private static final String MODEL_NAME = "randumTree.model";
    //private static final String MODEL_NAME = "j48.model";
    private static final String DAY_OF_WEEK = "dayOfWeek";
    private static final String DELIVERY = "delivery";
    private static final int DELIVERY_STARTING_TIME = 8;
    private static final int CLASS_INITIAL_VALUE = 10;
    private static final int CLASS_VALUE_INCREMENT = 4;

    public static void evaluatePath(JsonObject json){
        int dayIndex;

        if(!json.has(DAY_OF_WEEK)){
            System.err.println("No <dayOfWeek> in Json");
            System.exit(0);
        }

        dayIndex = switch (json.get(DAY_OF_WEEK).getAsString().toLowerCase()) {
            case "tuesday" -> 2;
            case "wednesday" -> 3;
            case "thursday" -> 4;
            case "friday" -> 5;
            default -> 1;
        };

        try {
            // Load Weka model
            RandomForest model = ModelLoader.loadModel(MODEL_NAME);

            if(!json.has(DELIVERY)){
                System.err.println("No <delivery> in Json");
                System.exit(0);
            }

            JsonArray deliverys = json.getAsJsonArray(DELIVERY);

            HashMap<Integer, Integer> deliveryEstimate = new HashMap<>();
            deliveryEstimate.put(deliverys.get(0).getAsInt(), 0);

            Integer deliveryTime = 0;

            // Create a new dataset with the relevant attributes
            final Attribute postbox1 = new Attribute("postbox1");
            final Attribute postbox2 = new Attribute("postbox2");
            final Attribute timeOfDay = new Attribute("timeOfDay");
            final Attribute dayOfWeek = new Attribute("dayOfWeek");

            final List<String> classValues = new ArrayList<>(){
                {
                    for(int i=1; i <= 50; i++){
                        add(String.valueOf(i));
                    }
                }
            };
            Attribute attributeClass = new Attribute("timeNeeded", classValues);

            ArrayList<Attribute> attributes = new ArrayList<>() {
                {
                    add(postbox1);
                    add(postbox2);
                    add(timeOfDay);
                    add(dayOfWeek);
                    add(attributeClass);
                }
            };

            Instances dataUnpredicted = new Instances("deliveryEstimate", attributes, 1);
            dataUnpredicted.setClassIndex(dataUnpredicted.numAttributes() -1);

            for(int i = 1; i < deliverys.size(); i++){

                // Create a new instance with the relevant features
                int finalI = i;
                Integer finalDeliveryTime = deliveryTime;
                DenseInstance unpredictedInstance = new DenseInstance(dataUnpredicted.numAttributes()){
                    {
                        setDataset(dataUnpredicted);
                        setValue(postbox1, deliverys.get(finalI-1).getAsInt());
                        setValue(postbox2, deliverys.get(finalI).getAsInt());
                        setValue(timeOfDay, whatClassOfTIme(finalDeliveryTime) -1);
                        setValue(dayOfWeek, dayIndex-1);
                        setClassMissing();
                    }
                };

                try {
                    // Classify the new instance
                    double classValue = model.classifyInstance(unpredictedInstance);

                    // Reverse the process
                    deliveryTime += reverseProcess(classValue);

                    deliveryEstimate.put(deliverys.get(i).getAsInt(), deliveryTime);
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }

            JsonUtil.saveToJsonDeliveryEstimate(new DeliveryJsonModel(deliveryEstimate));

        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private static Integer reverseProcess(double discretizedValue) {
        return (int) (discretizedValue * CLASS_VALUE_INCREMENT);
    }

    private static int whatClassOfTIme(int minutes){
        int hours = minutes/60;

        hours += DELIVERY_STARTING_TIME;

        if (hours < DELIVERY_STARTING_TIME) hours = DELIVERY_STARTING_TIME;
        if (hours > 15) hours = 15;

        return switch (hours) {
            case 8 -> 1;
            case 9 -> 8;
            case 10 -> 15;
            case 11 -> 22;
            case 12 -> 29;
            case 13 -> 36;
            case 14 -> 43;
            case 15 -> 50;
            default -> 1;
        };
    }
}
