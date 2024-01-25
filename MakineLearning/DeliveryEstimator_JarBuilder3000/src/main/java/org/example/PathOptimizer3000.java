package org.example;

import com.google.gson.*;
import org.example.models.DeliveryJsonModel;
import org.example.models.HowManyPeopleHome;
import org.example.utils.JsonUtil;
import org.example.utils.ModelLoader;
import weka.classifiers.trees.RandomForest;
import weka.core.Attribute;
import weka.core.DenseInstance;
import weka.core.Instances;

import java.util.ArrayList;
import java.util.List;

public class PathOptimizer3000 {
    private static final String MODEL_NAME = "path.model";
    private static final String TOUR = "delivery";
    private static final int TOUR_SIZE = 10;

    public static void evaluateTour(JsonObject json){
        try {
            // Load Weka model
            RandomForest treeModel = ModelLoader.loadModel(MODEL_NAME);

            if(!json.has(TOUR)){
                System.err.println("No <delivery> in Json");
                System.exit(0);
            }

            JsonArray tourArray = json.getAsJsonArray(TOUR);

            if(tourArray.size() != TOUR_SIZE){
                System.err.println("Tour size is invalid!");
                System.exit(0);
            }

            // Create a new dataset with the relevant attributes
            final Attribute p1 = new Attribute("p1");
            final Attribute p2 = new Attribute("p2");
            final Attribute p3 = new Attribute("p3");
            final Attribute p4 = new Attribute("p4");
            final Attribute p5 = new Attribute("p5");
            final Attribute p6 = new Attribute("p6");
            final Attribute p7 = new Attribute("p7");
            final Attribute p8 = new Attribute("p8");
            final Attribute p9 = new Attribute("p9");
            final Attribute p10 = new Attribute("p10");

            final List<String> classValues = new ArrayList<>(){
                {
                    add("1");
                    add("2");
                    add("3");
                    add("4");
                }
            };

            // This is actually how many packages were successfully delivered
            Attribute numPeopleHome = new Attribute("numPeopleHome", classValues);

            ArrayList<Attribute> attributes = new ArrayList<>() {
                {
                    add(p1);
                    add(p2);
                    add(p3);
                    add(p4);
                    add(p5);
                    add(p6);
                    add(p7);
                    add(p8);
                    add(p9);
                    add(p10);
                    add(numPeopleHome);
                }
            };

            Instances newUnpredictedInstance = new Instances("HowManyPeopleHome", attributes, 1);
            newUnpredictedInstance.setClassIndex(newUnpredictedInstance.numAttributes() -1);

            // Create a new instance with the unpredicted tour data
            DenseInstance unpredictedInstance = new DenseInstance(newUnpredictedInstance.numAttributes()){
                {
                    setDataset(newUnpredictedInstance);
                    setValue(p1, tourArray.get(0).getAsInt() -1);
                    setValue(p2, tourArray.get(1).getAsInt() -1);
                    setValue(p3, tourArray.get(2).getAsInt() -1);
                    setValue(p4, tourArray.get(3).getAsInt() -1);
                    setValue(p5, tourArray.get(4).getAsInt() -1);
                    setValue(p6, tourArray.get(5).getAsInt() -1);
                    setValue(p7, tourArray.get(6).getAsInt() -1);
                    setValue(p8, tourArray.get(7).getAsInt() -1);
                    setValue(p9, tourArray.get(8).getAsInt() -1);
                    setValue(p10, tourArray.get(9).getAsInt() -1);
                    setClassMissing();
                }
            };


            //predict
            double resClass = treeModel.classifyInstance(unpredictedInstance);

            HowManyPeopleHome howManyPeopleHome = new HowManyPeopleHome(howManyPackagesWereDelivered(resClass));

            Gson gson = new Gson();
            JsonUtil.saveJsonString(gson.toJson(howManyPeopleHome, HowManyPeopleHome.class));


        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private static String howManyPackagesWereDelivered(double classID){
        return switch ((int) classID) {
            case 0 -> "0 - 3 packages were successfully delivered";
            case 1 -> "3 - 6 packages were successfully delivered";
            case 2 -> "6 - 9 packages were successfully delivered";
            case 3 -> "9+ packages were successfully delivered";
            default -> " we don't know how many packages were successfully delivered";
        };
    }
}
