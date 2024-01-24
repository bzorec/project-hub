package org.example.models;

import java.util.HashMap;

public class DeliveryJsonModel implements JsonTemplate {
    public HashMap<Integer, Integer> postboxTimeNeededPairs;

    public DeliveryJsonModel(HashMap<Integer, Integer> postboxTimeNeededPairs) {
        this.postboxTimeNeededPairs = postboxTimeNeededPairs;
    }
}