public class PostboxInitializer
{
    // NOTE THE VALUES ARE REPRESENTED IN MINUTES
    // AS FOR NOW THE DISTANCE BETWEEN EACH NEIGHBOURING POSTBOX IS 30 MIN

    private static final int DISTANCE_IN_MINUTES = 10;
    static public Postbox[] initializePostboxes(){
        Postbox[] postboxes = new Postbox[10];
        postboxes[0] = new Postbox(1);
        postboxes[0].timeToGetToPostbox_1 = 0;
        postboxes[0].timeToGetToPostbox_2 = DISTANCE_IN_MINUTES;
        postboxes[0].timeToGetToPostbox_3 = 2 * DISTANCE_IN_MINUTES;
        postboxes[0].timeToGetToPostbox_4 = 3 * DISTANCE_IN_MINUTES;
        postboxes[0].timeToGetToPostbox_5 = 4 * DISTANCE_IN_MINUTES;
        postboxes[0].timeToGetToPostbox_6 = 5 * DISTANCE_IN_MINUTES;
        postboxes[0].timeToGetToPostbox_7 = 6 * DISTANCE_IN_MINUTES;
        postboxes[0].timeToGetToPostbox_8 = 7 * DISTANCE_IN_MINUTES;
        postboxes[0].timeToGetToPostbox_9 = 8 * DISTANCE_IN_MINUTES;
        postboxes[0].timeToGetToPostbox_10 = 9 * DISTANCE_IN_MINUTES;

        postboxes[1] = new Postbox(2);
        postboxes[1].timeToGetToPostbox_1 = DISTANCE_IN_MINUTES;
        postboxes[1].timeToGetToPostbox_2 = 0;
        postboxes[1].timeToGetToPostbox_3 = DISTANCE_IN_MINUTES;
        postboxes[1].timeToGetToPostbox_4 = 2 * DISTANCE_IN_MINUTES;
        postboxes[1].timeToGetToPostbox_5 = 3 * DISTANCE_IN_MINUTES;
        postboxes[1].timeToGetToPostbox_6 = 4 * DISTANCE_IN_MINUTES;
        postboxes[1].timeToGetToPostbox_7 = 5 * DISTANCE_IN_MINUTES;
        postboxes[1].timeToGetToPostbox_8 = 6 * DISTANCE_IN_MINUTES;
        postboxes[1].timeToGetToPostbox_9 = 7 * DISTANCE_IN_MINUTES;
        postboxes[1].timeToGetToPostbox_10 = 8 * DISTANCE_IN_MINUTES;

        postboxes[2] = new Postbox(3);
        postboxes[2].timeToGetToPostbox_1 = 2 * DISTANCE_IN_MINUTES;
        postboxes[2].timeToGetToPostbox_2 = DISTANCE_IN_MINUTES;
        postboxes[2].timeToGetToPostbox_3 = 0;
        postboxes[2].timeToGetToPostbox_4 = DISTANCE_IN_MINUTES;
        postboxes[2].timeToGetToPostbox_5 = 2 * DISTANCE_IN_MINUTES;
        postboxes[2].timeToGetToPostbox_6 = 3 * DISTANCE_IN_MINUTES;
        postboxes[2].timeToGetToPostbox_7 = 4 * DISTANCE_IN_MINUTES;
        postboxes[2].timeToGetToPostbox_8 = 5 * DISTANCE_IN_MINUTES;
        postboxes[2].timeToGetToPostbox_9 = 6 * DISTANCE_IN_MINUTES;
        postboxes[2].timeToGetToPostbox_10 = 7 * DISTANCE_IN_MINUTES;

        postboxes[3] = new Postbox(4);
        postboxes[3].timeToGetToPostbox_1 = 3 * DISTANCE_IN_MINUTES;
        postboxes[3].timeToGetToPostbox_2 = 2 * DISTANCE_IN_MINUTES;
        postboxes[3].timeToGetToPostbox_3 = DISTANCE_IN_MINUTES;
        postboxes[3].timeToGetToPostbox_4 = 0;
        postboxes[3].timeToGetToPostbox_5 = DISTANCE_IN_MINUTES;
        postboxes[3].timeToGetToPostbox_6 = 2 * DISTANCE_IN_MINUTES;
        postboxes[3].timeToGetToPostbox_7 = 3 * DISTANCE_IN_MINUTES;
        postboxes[3].timeToGetToPostbox_8 = 4 * DISTANCE_IN_MINUTES;
        postboxes[3].timeToGetToPostbox_9 = 5 * DISTANCE_IN_MINUTES;
        postboxes[3].timeToGetToPostbox_10 = 6 * DISTANCE_IN_MINUTES;

        postboxes[4] = new Postbox(5);
        postboxes[4].timeToGetToPostbox_1 = 4 * DISTANCE_IN_MINUTES;
        postboxes[4].timeToGetToPostbox_2 = 3 * DISTANCE_IN_MINUTES;
        postboxes[4].timeToGetToPostbox_3 = 2 * DISTANCE_IN_MINUTES;
        postboxes[4].timeToGetToPostbox_4 = DISTANCE_IN_MINUTES;
        postboxes[4].timeToGetToPostbox_5 = 0;
        postboxes[4].timeToGetToPostbox_6 = DISTANCE_IN_MINUTES;
        postboxes[4].timeToGetToPostbox_7 = 2 * DISTANCE_IN_MINUTES;
        postboxes[4].timeToGetToPostbox_8 = 3 * DISTANCE_IN_MINUTES;
        postboxes[4].timeToGetToPostbox_9 = 4 * DISTANCE_IN_MINUTES;
        postboxes[4].timeToGetToPostbox_10 = 5 * DISTANCE_IN_MINUTES;

        postboxes[5] = new Postbox(6);
        postboxes[5].timeToGetToPostbox_1 = 5 * DISTANCE_IN_MINUTES;
        postboxes[5].timeToGetToPostbox_2 = 4 * DISTANCE_IN_MINUTES;
        postboxes[5].timeToGetToPostbox_3 = 3 * DISTANCE_IN_MINUTES;
        postboxes[5].timeToGetToPostbox_4 = 2 * DISTANCE_IN_MINUTES;
        postboxes[5].timeToGetToPostbox_5 = DISTANCE_IN_MINUTES;
        postboxes[5].timeToGetToPostbox_6 = 0;
        postboxes[5].timeToGetToPostbox_7 = DISTANCE_IN_MINUTES;
        postboxes[5].timeToGetToPostbox_8 = 2 * DISTANCE_IN_MINUTES;
        postboxes[5].timeToGetToPostbox_9 = 3 * DISTANCE_IN_MINUTES;
        postboxes[5].timeToGetToPostbox_10 = 4 * DISTANCE_IN_MINUTES;

        postboxes[6] = new Postbox(7);
        postboxes[6].timeToGetToPostbox_1 = 6 * DISTANCE_IN_MINUTES;
        postboxes[6].timeToGetToPostbox_2 = 5 * DISTANCE_IN_MINUTES;
        postboxes[6].timeToGetToPostbox_3 = 4 * DISTANCE_IN_MINUTES;
        postboxes[6].timeToGetToPostbox_4 = 3 * DISTANCE_IN_MINUTES;
        postboxes[6].timeToGetToPostbox_5 = 2 * DISTANCE_IN_MINUTES;
        postboxes[6].timeToGetToPostbox_6 = DISTANCE_IN_MINUTES;
        postboxes[6].timeToGetToPostbox_7 = 0;
        postboxes[6].timeToGetToPostbox_8 = DISTANCE_IN_MINUTES;
        postboxes[6].timeToGetToPostbox_9 = 2 * DISTANCE_IN_MINUTES;
        postboxes[6].timeToGetToPostbox_10 = 3 * DISTANCE_IN_MINUTES;

        postboxes[7] = new Postbox(8);
        postboxes[7].timeToGetToPostbox_1 = 7 * DISTANCE_IN_MINUTES;
        postboxes[7].timeToGetToPostbox_2 = 6 * DISTANCE_IN_MINUTES;
        postboxes[7].timeToGetToPostbox_3 = 5 * DISTANCE_IN_MINUTES;
        postboxes[7].timeToGetToPostbox_4 = 4 * DISTANCE_IN_MINUTES;
        postboxes[7].timeToGetToPostbox_5 = 3 * DISTANCE_IN_MINUTES;
        postboxes[7].timeToGetToPostbox_6 = 2 * DISTANCE_IN_MINUTES;
        postboxes[7].timeToGetToPostbox_7 = DISTANCE_IN_MINUTES;
        postboxes[7].timeToGetToPostbox_8 = 0;
        postboxes[7].timeToGetToPostbox_9 = DISTANCE_IN_MINUTES;
        postboxes[7].timeToGetToPostbox_10 = 2 * DISTANCE_IN_MINUTES;

        postboxes[8] = new Postbox(9);
        postboxes[8].timeToGetToPostbox_1 = 8 * DISTANCE_IN_MINUTES;
        postboxes[8].timeToGetToPostbox_2 = 7 * DISTANCE_IN_MINUTES;
        postboxes[8].timeToGetToPostbox_3 = 6 * DISTANCE_IN_MINUTES;
        postboxes[8].timeToGetToPostbox_4 = 5 * DISTANCE_IN_MINUTES;
        postboxes[8].timeToGetToPostbox_5 = 4 * DISTANCE_IN_MINUTES;
        postboxes[8].timeToGetToPostbox_6 = 3 * DISTANCE_IN_MINUTES;
        postboxes[8].timeToGetToPostbox_7 = 2 * DISTANCE_IN_MINUTES;
        postboxes[8].timeToGetToPostbox_8 = DISTANCE_IN_MINUTES;
        postboxes[8].timeToGetToPostbox_9 = 0;
        postboxes[8].timeToGetToPostbox_10 = DISTANCE_IN_MINUTES;

        postboxes[9] = new Postbox(10);
        postboxes[9].timeToGetToPostbox_1 = 9 * DISTANCE_IN_MINUTES;
        postboxes[9].timeToGetToPostbox_2 = 8 * DISTANCE_IN_MINUTES;
        postboxes[9].timeToGetToPostbox_3 = 7 * DISTANCE_IN_MINUTES;
        postboxes[9].timeToGetToPostbox_4 = 6 * DISTANCE_IN_MINUTES;
        postboxes[9].timeToGetToPostbox_5 = 5 * DISTANCE_IN_MINUTES;
        postboxes[9].timeToGetToPostbox_6 = 4 * DISTANCE_IN_MINUTES;
        postboxes[9].timeToGetToPostbox_7 = 3 * DISTANCE_IN_MINUTES;
        postboxes[9].timeToGetToPostbox_8 = 2 * DISTANCE_IN_MINUTES;
        postboxes[9].timeToGetToPostbox_9 = DISTANCE_IN_MINUTES;
        postboxes[9].timeToGetToPostbox_10 = 0;

        return postboxes;
    }
}
