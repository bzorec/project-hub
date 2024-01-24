package org.example;

import javafx.animation.KeyFrame;
import javafx.animation.KeyValue;
import javafx.animation.Timeline;
import javafx.application.Application;
import javafx.collections.FXCollections;
import javafx.geometry.Insets;
import javafx.geometry.Pos;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.control.ComboBox;
import javafx.scene.control.Label;
import javafx.scene.effect.Glow;
import javafx.scene.layout.HBox;
import javafx.scene.layout.VBox;
import javafx.stage.FileChooser;
import javafx.stage.Modality;
import javafx.stage.Stage;
import javafx.util.Duration;
import weka.classifiers.Classifier;
import weka.classifiers.Evaluation;
import weka.classifiers.bayes.NaiveBayes;
import weka.classifiers.functions.Logistic;
import weka.classifiers.functions.MultilayerPerceptron;
import weka.classifiers.functions.SMO;
import weka.classifiers.lazy.IBk;
import weka.classifiers.meta.AdaBoostM1;
import weka.classifiers.rules.DecisionTable;
import weka.classifiers.rules.ZeroR;
import weka.classifiers.trees.J48;
import weka.classifiers.trees.REPTree;
import weka.classifiers.trees.RandomForest;
import weka.core.Instances;
import weka.core.converters.ArffLoader;

import java.io.File;

public class WekaClassifierApp extends Application {

    private Instances data;
    private Classifier selectedClassifier;
    private final Label resultLabel = new Label(); // Define resultLabel here

    @Override
    public void start(Stage primaryStage) {
        primaryStage.setTitle("Weka Classifier");

        FileChooser fileChooser = new FileChooser();
        Button loadButton = new Button();
        ComboBox<String> classifierComboBox = new ComboBox<>();
        Button classifyButton = new Button();

        //style
        resultLabel.setStyle("-fx-background-color: #333333; -fx-text-fill: white; -fx-padding: 10px;");
        resultLabel.setMinSize(400.0, 250.0);
        setGlowOnHover(loadButton, classifierComboBox, classifyButton);

        loadButton.setText("Load ARFF File");
        loadButton.setStyle("-fx-font-size: 14px; -fx-background-color: #4CAF50; -fx-text-fill: white; -fx-padding: 10px 20px;");

        classifierComboBox.setPromptText("Select Classifier");
        classifierComboBox.setStyle("-fx-font-size: 14px; -fx-background-color: #2196F3; -fx-text-fill: white; -fx-padding: 10px 20px;");

        classifyButton.setText("Classify");
        classifyButton.setStyle("-fx-font-size: 14px; -fx-background-color: #D3D3D3; -fx-text-fill: white; -fx-padding: 10px 20px;");

        // Load ARFF file
        loadButton.setOnAction(e -> {
            File file = fileChooser.showOpenDialog(primaryStage);
            if (file != null) {
                try {
                    data = loadArff(file);
                    showPopup("ARFF file '" + file.getName() + "' loaded successfully.", primaryStage);
                    loadButton.setText(file.getName());
                } catch (Exception ex) {
                    ex.printStackTrace();
                    showPopup("Error loading ARFF file.", primaryStage);
                    loadButton.setText("Load ARFF File");
                }
            }
        });

        // Classifier selection
        String[] algorithms = {
                "J48", "Logistic", "IBk", "AdaBoostM1",
                "DecisionTable", "ZeroR", "NaiveBayes", "SMO",
                "RandomForest", "MultilayerPerceptron", "REPTree"
        };

        classifierComboBox.setItems(FXCollections.observableArrayList(algorithms));

        // Classification button
        classifyButton.setOnAction(e -> {
            String selectedAlgorithm = classifierComboBox.getValue();
            selectedClassifier = getClassifier(selectedAlgorithm);
            boolean classificationResult = classify();

            String popupMessage = classificationResult ?
                    "Classification successful!" :
                    "Error during classification.";

            showPopup(popupMessage, primaryStage);
        });

        // Set preferred size for buttons and combo box
        double buttonWidth = 250.0;
        double buttonHeight = 50.0;
        loadButton.setPrefWidth(buttonWidth);
        loadButton.setPrefHeight(buttonHeight);
        classifyButton.setPrefWidth(buttonWidth);
        classifyButton.setPrefHeight(buttonHeight);
        classifierComboBox.setPrefWidth(buttonWidth);
        classifierComboBox.setPrefHeight(buttonHeight);

        HBox firstRow = new HBox(10.0);
        firstRow.setAlignment(Pos.TOP_CENTER);
        firstRow.getChildren().addAll(loadButton, classifierComboBox);

        VBox layout = new VBox(10.0);
        layout.setAlignment(Pos.TOP_CENTER);
        layout.setPadding(new Insets(50.0)); // Adjust padding as needed
        layout.getChildren().addAll(firstRow, classifyButton, resultLabel);

