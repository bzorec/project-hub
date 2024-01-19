public class PostboxInitializer
{
    // NOTE THE VALUES ARE REPRESENTED IN MINUTES
    // AS FOR NOW THE DISTANCE BETWEEN EACH NEIGHBOURING POSTBOX IS 30 MIN
    static public Postbox[] initializePostboxes(){
        Postbox[] postboxes = new Postbox[10];
        postboxes[0] = new Postbox(1);
        postboxes[0].timeToGetToPostbox_1 = 0;
        postboxes[0].timeToGetToPostbox_2 = 30;
        postboxes[0].timeToGetToPostbox_3 = 60;
        postboxes[0].timeToGetToPostbox_4 = 90;
        postboxes[0].timeToGetToPostbox_5 = 120;
        postboxes[0].timeToGetToPostbox_6 = 150;
        postboxes[0].timeToGetToPostbox_7 = 180;
        postboxes[0].timeToGetToPostbox_8 = 210;
        postboxes[0].timeToGetToPostbox_9 = 240;
        postboxes[0].timeToGetToPostbox_10 = 270;

        postboxes[1] = new Postbox(2);
        postboxes[1].timeToGetToPostbox_1 = 30;
        postboxes[1].timeToGetToPostbox_2 = 0;
        postboxes[1].timeToGetToPostbox_3 = 30;
        postboxes[1].timeToGetToPostbox_4 = 60;
        postboxes[1].timeToGetToPostbox_5 = 90;
        postboxes[1].timeToGetToPostbox_6 = 120;
        postboxes[1].timeToGetToPostbox_7 = 150;
        postboxes[1].timeToGetToPostbox_8 = 180;
        postboxes[1].timeToGetToPostbox_9 = 210;
        postboxes[1].timeToGetToPostbox_10 = 240;

        postboxes[2] = new Postbox(3);
        postboxes[2].timeToGetToPostbox_1 = 60;
        postboxes[2].timeToGetToPostbox_2 = 30;
        postboxes[2].timeToGetToPostbox_3 = 0;
        postboxes[2].timeToGetToPostbox_4 = 30;
        postboxes[2].timeToGetToPostbox_5 = 60;
        postboxes[2].timeToGetToPostbox_6 = 90;
        postboxes[2].timeToGetToPostbox_7 = 120;
        postboxes[2].timeToGetToPostbox_8 = 150;
        postboxes[2].timeToGetToPostbox_9 = 180;
        postboxes[2].timeToGetToPostbox_10 = 210;

        postboxes[3] = new Postbox(4);
        postboxes[3].timeToGetToPostbox_1 = 90;
        postboxes[3].timeToGetToPostbox_2 = 60;
        postboxes[3].timeToGetToPostbox_3 = 30;
        postboxes[3].timeToGetToPostbox_4 = 0;
        postboxes[3].timeToGetToPostbox_5 = 30;
        postboxes[3].timeToGetToPostbox_6 = 60;
        postboxes[3].timeToGetToPostbox_7 = 90;
        postboxes[3].timeToGetToPostbox_8 = 120;
        postboxes[3].timeToGetToPostbox_9 = 150;
        postboxes[3].timeToGetToPostbox_10 = 180;

        postboxes[4] = new Postbox(5);
        postboxes[4].timeToGetToPostbox_1 = 120;
        postboxes[4].timeToGetToPostbox_2 = 90;
        postboxes[4].timeToGetToPostbox_3 = 60;
        postboxes[4].timeToGetToPostbox_4 = 30;
        postboxes[4].timeToGetToPostbox_5 = 0;
        postboxes[4].timeToGetToPostbox_6 = 30;
        postboxes[4].timeToGetToPostbox_7 = 60;
        postboxes[4].timeToGetToPostbox_8 = 90;
        postboxes[4].timeToGetToPostbox_9 = 120;
        postboxes[4].timeToGetToPostbox_10 = 150;

        postboxes[5] = new Postbox(6);
        postboxes[5].timeToGetToPostbox_1 = 150;
        postboxes[5].timeToGetToPostbox_2 = 120;
        postboxes[5].timeToGetToPostbox_3 = 90;
        postboxes[5].timeToGetToPostbox_4 = 60;
        postboxes[5].timeToGetToPostbox_5 = 30;
        postboxes[5].timeToGetToPostbox_6 = 0;
        postboxes[5].timeToGetToPostbox_7 = 30;
        postboxes[5].timeToGetToPostbox_8 = 60;
        postboxes[5].timeToGetToPostbox_9 = 90;
        postboxes[5].timeToGetToPostbox_10 = 120;

        postboxes[6] = new Postbox(7);
        postboxes[6].timeToGetToPostbox_1 = 180;
        postboxes[6].timeToGetToPostbox_2 = 150;
        postboxes[6].timeToGetToPostbox_3 = 120;
        postboxes[6].timeToGetToPostbox_4 = 90;
        postboxes[6].timeToGetToPostbox_5 = 60;
        postboxes[6].timeToGetToPostbox_6 = 30;
        postboxes[6].timeToGetToPostbox_7 = 0;
        postboxes[6].timeToGetToPostbox_8 = 30;
        postboxes[6].timeToGetToPostbox_9 = 60;
        postboxes[6].timeToGetToPostbox_10 = 90;

        postboxes[7] = new Postbox(8);
        postboxes[7].timeToGetToPostbox_1 = 210;
        postboxes[7].timeToGetToPostbox_2 = 180;
        postboxes[7].timeToGetToPostbox_3 = 150;
        postboxes[7].timeToGetToPostbox_4 = 120;
        postboxes[7].timeToGetToPostbox_5 = 90;
        postboxes[7].timeToGetToPostbox_6 = 60;
        postboxes[7].timeToGetToPostbox_7 = 30;
        postboxes[7].timeToGetToPostbox_8 = 0;
        postboxes[7].timeToGetToPostbox_9 = 30;
        postboxes[7].timeToGetToPostbox_10 = 60;

        postboxes[8] = new Postbox(9);
        postboxes[8].timeToGetToPostbox_1 = 240;
        postboxes[8].timeToGetToPostbox_2 = 210;
        postboxes[8].timeToGetToPostbox_3 = 180;
        postboxes[8].timeToGetToPostbox_4 = 150;
        postboxes[8].timeToGetToPostbox_5 = 120;
        postboxes[8].timeToGetToPostbox_6 = 90;
        postboxes[8].timeToGetToPostbox_7 = 60;
        postboxes[8].timeToGetToPostbox_8 = 30;
        postboxes[8].timeToGetToPostbox_9 = 0;
        postboxes[8].timeToGetToPostbox_10 = 30;

        postboxes[9] = new Postbox(10);
        postboxes[9].timeToGetToPostbox_1 = 270;
        postboxes[9].timeToGetToPostbox_2 = 240;
        postboxes[9].timeToGetToPostbox_3 = 210;
        postboxes[9].timeToGetToPostbox_4 = 180;
        postboxes[9].timeToGetToPostbox_5 = 150;
        postboxes[9].timeToGetToPostbox_6 = 120;
        postboxes[9].timeToGetToPostbox_7 = 90;
        postboxes[9].timeToGetToPostbox_8 = 60;
        postboxes[9].timeToGetToPostbox_9 = 30;
        postboxes[9].timeToGetToPostbox_10 = 0;

        return postboxes;
    }
}
