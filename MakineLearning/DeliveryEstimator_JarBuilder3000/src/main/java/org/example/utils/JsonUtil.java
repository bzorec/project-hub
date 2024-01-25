package org.example.utils;

import com.google.gson.Gson;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import org.example.models.DeliveryJsonModel;
import org.example.models.JsonTemplate;

import java.io.FileWriter;
import java.io.IOException;
import java.io.Reader;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.HashMap;
import java.util.Map;

public class JsonUtil {
    public static String JSON_OUTPUT_PATH = "";
    public static JsonObject loadJsonObject(String absFilePath) {
        Path path = Paths.get(absFilePath);
        try(Reader reader = Files.newBufferedReader(path, StandardCharsets.UTF_8)){
            return JsonParser.parseReader(reader).getAsJsonObject();

        } catch (Exception e){
            e.printStackTrace();
        }

        return null;
    }

    public static void saveToJsonDeliveryEstimate(DeliveryJsonModel obj) throws IOException {
        Gson gson = new Gson();
        String s = gson.toJson(obj.postboxTimeNeededPairs);

        saveJsonString(s);
    }

    public static void saveJsonObject(JsonObject object){
        Gson gson = new Gson();

        try {
            gson.toJson(object, new FileWriter(JSON_OUTPUT_PATH));
        } catch (IOException e) {
            throw new RuntimeException(e);
        }

    }

    public static void saveJsonString(String jsonString){
        try(FileWriter writer = new FileWriter(JSON_OUTPUT_PATH)){
            writer.write(jsonString);
        } catch (Exception e) {
            e.printStackTrace();
        }

    }
}
