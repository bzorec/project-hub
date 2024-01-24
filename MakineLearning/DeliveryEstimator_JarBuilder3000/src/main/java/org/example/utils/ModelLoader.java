package org.example.utils;

import org.example.MakineSmartAI;
import weka.classifiers.trees.RandomForest;

import java.io.*;
import java.net.URL;

public class ModelLoader {
    public static RandomForest loadModel(String modelResourceName) throws Exception {
        ClassLoader classLoader = MakineSmartAI.class.getClassLoader();
        URL modelUrl = classLoader.getResource(modelResourceName);

        if (modelUrl != null) {
            try (InputStream modelStream = new FileInputStream(new File(modelUrl.getFile()));
                 ObjectInputStream objectInputStream = new ObjectInputStream(modelStream)) {

                return (RandomForest) objectInputStream.readObject();
            }
        } else {
            throw new FileNotFoundException("Model file not found in resources: " + modelResourceName);
        }
    }
}
