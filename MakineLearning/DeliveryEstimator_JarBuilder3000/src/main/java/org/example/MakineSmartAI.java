package org.example;

import com.google.gson.JsonObject;
import org.example.utils.JsonUtil;

import java.io.*;

public class MakineSmartAI {
        private static final String JSON_MAKINE_STRING = "MAKINE";
        private static final String OUTPUT = "output";

    public static void main(String[] args) throws IOException {
        if (args.length != 1) {
            System.err.println("Usage: java MakineSmartAI <path_to_json>");
            System.exit(1);
        }

        JsonObject object = JsonUtil.loadJsonObject(args[1]);

        if(!object.has(JSON_MAKINE_STRING)){
            System.err.println("No MAKINE field in json");
            System.exit(0);
        }

        if(!object.has(OUTPUT)){
            System.err.println("No output field in json");
            System.exit(0);
        }

        JsonUtil.JSON_OUTPUT_PATH = object.get(OUTPUT).getAsString();

        switch (object.get(JSON_MAKINE_STRING).getAsInt()){
            case 1:
                //zorec ai
                PackageOptimizer3000.evaluatePickups(object);
                break;

            case 2:
                //bezo ai
                break;

            default:
                //cucek ai
                DeliveryEstimator.evaluatePath(object);
                break;
        }

    }
}
