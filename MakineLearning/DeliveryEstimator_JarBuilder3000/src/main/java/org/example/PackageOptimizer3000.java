package org.example;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import org.example.utils.ModelLoader;
import weka.classifiers.bayes.NaiveBayesMultinomialText;
import weka.core.Attribute;
import weka.core.DenseInstance;
import weka.core.Instance;
import weka.core.Instances;

import java.io.FileWriter;
import java.util.ArrayList;
import java.util.Arrays;

public class PackageOptimizer3000 {
    private static final String MODEL_NAME = "package_pickup.model";

    public static void evaluatePickups(JsonObject json) {
        try {
            NaiveBayesMultinomialText model = ModelLoader.loadModel(MODEL_NAME);
            System.out.println("Model Information: " + model.toString());

            if (!json.has("delivery")) {
                System.err.println("No <Delivery> in Json");
                System.exit(0);
            }

            JsonArray deliveries = json.getAsJsonArray("delivery");
            ArrayList<Attribute> attributes = createAttributes();

            Instances dataUnpredicted = new Instances("PackagePickup", attributes, deliveries.size());
            dataUnpredicted.setClassIndex(dataUnpredicted.numAttributes() - 1);

            JsonArray resultsArray = new JsonArray();
            for (int i = 0; i < deliveries.size(); i++) {
                JsonObject delivery = deliveries.get(i).getAsJsonObject();
                Instance instance = createInstance(delivery, dataUnpredicted);

                try {
                    double classValue = model.classifyInstance(instance);
                    String pickupDecision = instance.classAttribute().value((int) classValue);
                    JsonObject result = createResultObject(delivery, pickupDecision);
                    resultsArray.add(result);
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }

            String outputPath = json.get("output").getAsString();
            saveResultsToJsonFile(resultsArray, outputPath);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private static ArrayList<Attribute> createAttributes() {
        return new ArrayList<>() {
            {
                add(new Attribute("boxId"));
                add(new Attribute("numPeopleHome"));
                add(new Attribute("paymentType", Arrays.asList("manual", "prepaid")));
                add(new Attribute("needSignature", Arrays.asList("yes", "no")));
                add(new Attribute("packageType", Arrays.asList("perishable", "electronics", "clothing", "others")));
                add(new Attribute("deliveryUrgency", Arrays.asList("standard", "expedited", "immediate")));
                add(new Attribute("dayOfWeek", Arrays.asList("monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday")));
                add(new Attribute("repeatDelivery", Arrays.asList("yes", "no")));
                add(new Attribute("postboxCapacity"));
                add(new Attribute("packagePickup", Arrays.asList("yes", "no")));
            }
        };
    }


    private static Instance createInstance(JsonObject delivery, Instances dataset) {
        Instance instance = new DenseInstance(dataset.numAttributes());
        instance.setDataset(dataset);

        instance.setValue(dataset.attribute("boxId"), delivery.get("PostBoxId").getAsInt());
        instance.setValue(dataset.attribute("numPeopleHome"), delivery.get("NumPeopleHome").getAsInt());
        instance.setValue(dataset.attribute("paymentType"), delivery.get("PaymentType").getAsString());
        instance.setValue(dataset.attribute("needSignature"), delivery.get("NeedSignature").getAsString());
        instance.setValue(dataset.attribute("packageType"), delivery.get("PackageType").getAsString());
        instance.setValue(dataset.attribute("deliveryUrgency"), delivery.get("DeliveryUrgency").getAsString());
        instance.setValue(dataset.attribute("dayOfWeek"), delivery.get("DayOfWeek").getAsString());
        instance.setValue(dataset.attribute("repeatDelivery"), delivery.get("RepeatDelivery").getAsString());
        instance.setValue(dataset.attribute("postboxCapacity"), delivery.get("PostboxCapacity").getAsDouble());
        instance.setMissing(dataset.attribute("packagePickup")); // Target attribute

        return instance;
    }

    private static JsonObject createResultObject(JsonObject delivery, String pickupDecision) {
        JsonObject result = new JsonObject();
        result.addProperty("PackageId", delivery.get("PackageId").getAsInt());
        result.addProperty("PickupDecision", pickupDecision);
        return result;
    }

    private static void saveResultsToJsonFile(JsonArray results, String outputPath) {
        try (FileWriter writer = new FileWriter(outputPath)) {
            writer.write(results.toString());
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
