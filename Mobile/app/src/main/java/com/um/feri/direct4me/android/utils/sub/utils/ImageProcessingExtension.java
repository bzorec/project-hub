package com.um.feri.direct4me.android.utils.sub.utils;

import android.graphics.Bitmap;
import java.util.ArrayList;
import java.util.BitSet;
import java.util.List;

public class ImageProcessingExtension {
    private static int MagnificationIndex = 0;

    public static boolean isBitSet(int value, int position) {
        if (position < 0 || position >= 32) {
            throw new IllegalArgumentException("Position is out of range");
        }
        return (value & (1 << position)) != 0;
    }

    public static void setBit(int[] number, int position) {
        if (position < 0 || position >= 32) {
            throw new IllegalArgumentException("Position is out of range");
        }
        number[0] |= 1 << position;
    }

    public static List<Integer> predict(Bitmap image, int height, int width) {
        List<Integer> e = new ArrayList<>();

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {

                int pixel = (int) image.getColor(x,y).red();

                if (x == 0 && y == 0) {
                    e.add(pixel);
                } else if (y == 0) {
                    e.add((int) (image.getColor(x - 1, 0).red() - pixel));
                } else if (x == 0) {
                    e.add((int) (image.getColor(0, y - 1).red() - pixel));
                } else {
                    int left = (int) image.getColor(x - 1, y).red();
                    int up = (int) image.getColor(x, y - 1).red();
                    int diagonal = (int) image.getColor(x - 1, y - 1).red();

                    int max = Math.max(left, up);
                    int min = Math.min(left, up);

                    if (diagonal >= max) {
                        e.add(min - pixel);
                    } else if (diagonal <= min) {
                        e.add(max - pixel);
                    } else {
                        e.add(left + up - diagonal - pixel);
                    }
                }
            }
        }

        return e;
    }

    public static int clamp(int value) {
        return Math.max(0, Math.min(value, 255));
    }

    public static BitSet setHeader(int height, int c0, int cl, int n) {
        BitSet bitArray = new BitSet();
        int index = 0;

        for (int j = 11; j >= 0; j--, index++) {
            bitArray.set(index, isBitSet(height, j));
        }

        for (int j = 7; j >= 0; j--, index++) {
            bitArray.set(index, isBitSet(c0, j));
        }

        for (int j = 31; j >= 0; j--, index++) {
            bitArray.set(index, isBitSet(cl, j));
        }

        for (int j = 23; j >= 0; j--, index++) {
            bitArray.set(index, isBitSet(n, j));
        }

        return bitArray;
    }

    public static byte[] compress(Bitmap image, int height, int width) {
        MagnificationIndex = 76;
        List<Integer> eList = predict(image, height, width);
        int n = width * height;
        List<Integer> codedN = ImageCompression.codeN(eList);
        List<Integer> calcC = ImageCompression.calculateC(codedN);

        BitSet bitArray = setHeader((short) height, calcC.get(0), calcC.get(calcC.size() - 1), n);
        bitArray = ImageCompression.code(bitArray, calcC, 0, n - 1, 76,0).object;

        byte[] bytes = ImageCompression.toBytes(bitArray);
        ImageCompression.fixBytesOrder(bytes);

        return bytes;
    }
}