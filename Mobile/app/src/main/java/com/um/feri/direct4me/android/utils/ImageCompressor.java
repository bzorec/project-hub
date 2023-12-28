package com.um.feri.direct4me.android.utils;

import android.content.ContentValues;
import android.content.Context;
import android.graphics.Bitmap;
import android.net.Uri;
import android.provider.MediaStore;
import android.util.Log;

import com.um.feri.direct4me.android.utils.sub.utils.ImageProcessingExtension;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStream;

public class ImageCompressor {
    private static final String TAG = "ImageCompressor";

    public static byte[] compressImage(Context context, Bitmap bmp) {
        byte[] compressedImageArray = null;
        try {
            long startCompressionTime = System.currentTimeMillis();
            compressedImageArray = ImageProcessingExtension.compress(bmp, bmp.getHeight(), bmp.getWidth());
            long endCompressionTime = System.currentTimeMillis();

            // Debug logs
            Log.d(TAG, "Compression time: " + (endCompressionTime - startCompressionTime) + " ms");

            // Save compressed image to a file
            saveCompressedImageToFile(context, compressedImageArray);

        } catch (Exception e) {
            e.printStackTrace();
            // Debug logs for errors
            Log.e(TAG, "Error compressing image: " + e.getMessage());
        }

        return compressedImageArray;
    }

    private static void saveCompressedImageToFile(Context context, byte[] data) {
        try {
            ContentValues values = new ContentValues();
            values.put(MediaStore.Images.Media.DISPLAY_NAME, "compressed.bin");
            values.put(MediaStore.Images.Media.MIME_TYPE, "application/octet-stream");

            Uri contentUri = MediaStore.Images.Media.EXTERNAL_CONTENT_URI;
            Uri itemUri = context.getContentResolver().insert(contentUri, values);

            if (itemUri != null) {
                try (OutputStream os = context.getContentResolver().openOutputStream(itemUri)) {
                    if (os != null) {
                        os.write(data);
                    }
                }

                // Debug logs
                Log.d(TAG, "Compressed image saved to: " + itemUri.toString());
            } else {
                // Debug logs for errors
                Log.e(TAG, "Error creating itemUri for compressed image");
            }

        } catch (IOException e) {
            e.printStackTrace();
            // Debug logs for errors
            Log.e(TAG, "Error saving compressed image: " + e.getMessage());
        }
    }


}
