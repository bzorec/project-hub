package org.example.models;

public class PickupDecision implements JsonTemplate {
    private int packageId;
    private String decision;
    private double confidence;

    PickupDecision(int packageId, String decision, double confidence) {
        this.packageId = packageId;
        this.decision = decision;
        this.confidence = confidence;
    }
}