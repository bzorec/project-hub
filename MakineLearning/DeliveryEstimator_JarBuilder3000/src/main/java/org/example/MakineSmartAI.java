package org.example;

import com.formdev.flatlaf.json.Json;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import org.example.utils.JsonUtil;
import weka.classifiers.trees.RandomForest;
import weka.core.*;
import weka.core.converters.JSONLoader;

import java.io.*;
import java.net.URL;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;

public class MakineSmartAI {
        private static final String JSON_MAKINE_STRING = "MAKINE";

    public static void main(String[] args) throws IOException {
        if (args.length != 2) {
            System.err.println("Usage: java MakineSmartAI <path_to_json> <jsonSavePath - absPath/file.json>");
            System.exit(1);
        }

        JsonObject object = JsonUtil.loadJsonObject(args[0]);
        JsonUtil.JSON_OUTPUT_PATH = args[1];

        if(!object.has(JSON_MAKINE_STRING)){
            System.err.println("No MAKINE field in json");
            System.exit(0);
        }

        switch (object.get(JSON_MAKINE_STRING).getAsInt()){
            case 1:
                //zorec ai
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
