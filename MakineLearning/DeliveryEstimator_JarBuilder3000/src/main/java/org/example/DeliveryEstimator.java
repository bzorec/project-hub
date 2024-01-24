package org.example;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import org.example.models.DeliveryJsonModel;
import org.example.utils.JsonUtil;
import org.example.utils.ModelLoader;
import weka.classifiers.trees.RandomForest;
import weka.core.Attribute;
import weka.core.DenseInstance;
import weka.core.Instances;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class DeliveryEstimator {
    private static final String MODEL_NAME = "randumTree.model";
    private static final String DAY_OF_WEEK = "dayOfWeek";
    private static final String DELIVERY = "delivery";
    private static final int DELIVERY_STARTING_TIME = 6;
    private static final int CLASS_INITIAL_VALUE = 62;
    private static final int CLASS_VALUE_INCREMENT = 32;

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
            System.out.println("Model Information: " + model.toString());

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
                    add("1");
                    add("2");
                    add("3");
                    add("4");
                    add("5");
                    add("6");
                    add("7");
                    add("8");
                    add("9");
                    add("10");
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

            for(int i = 1; i < deliverys.size()-1; i++){

                // Create a new instance with the relevant features
                int finalI = i;
                DenseInstance unpredictedInstance = new DenseInstance(dataUnpredicted.numAttributes()){
                    {
                        setDataset(dataUnpredicted);
                        setValue(postbox1, deliverys.get(finalI-1).getAsInt());
                        setValue(postbox2, deliverys.get(finalI).getAsInt());
                        setValue(timeOfDay, 8);
                        setValue(dayOfWeek, 1);
                        setClassMissing();
                    }
                };
                //newInstance.setValue(0, deliveryPath.get(i-1)); //postboxID1
                //newInstance.setValue(1, deliveryPath.get(i)); //postboxID2
                //newInstance.setValue(2, deliveryTime + DELIVERY_STARTING_TIME); //timeOfDay
                //newInstance.setValue(2, 8); //timeOfDay
                //newInstance.setValue(3, dayIndex); //dayOfWeek

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
        return (int) (CLASS_INITIAL_VALUE + (discretizedValue - 1) * CLASS_VALUE_INCREMENT);
    }
}
