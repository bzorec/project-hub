package org.example.utils;

import org.example.MakineSmartAI;
import weka.classifiers.trees.RandomForest;

import java.io.*;
import java.net.URL;

public class ModelLoader {
    public static <T> T loadModel(String modelResourceName) throws Exception {
        ClassLoader classLoader = MakineSmartAI.class.getClassLoader();
        InputStream modelStream = classLoader.getResourceAsStream(modelResourceName);

        if (modelStream != null) {
            try (ObjectInputStream objectInputStream = new ObjectInputStream(modelStream)) {
                return (T) objectInputStream.readObject();
            }
        } else {
            throw new FileNotFoundException("Model file not found in resources: " + modelResourceName);
        }
    }
}
