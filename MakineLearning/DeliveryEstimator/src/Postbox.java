public class Postbox {
    private int id;

    // In minutes
    public int timeToGetToPostbox_1;
    public int timeToGetToPostbox_2;
    public int timeToGetToPostbox_3;
    public int timeToGetToPostbox_4;
    public int timeToGetToPostbox_5;
    public int timeToGetToPostbox_6;
    public int timeToGetToPostbox_7;
    public int timeToGetToPostbox_8;
    public int timeToGetToPostbox_9;
    public int timeToGetToPostbox_10;

    // Constructor
    public Postbox(int id) {
        this.id = id;
    }

    // Getter methods
    public int getId() {
        return id;
    }


    // Setter methods (if needed)
    public void setId(int id) {
        this.id = id;
    }


    // Other methods or overrides can be added as needed
}
