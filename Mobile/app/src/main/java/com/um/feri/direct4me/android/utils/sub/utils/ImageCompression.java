package com.um.feri.direct4me.android.utils.sub.utils;

import java.util.ArrayList;
import java.util.BitSet;
import java.util.List;

public class ImageCompression {
    public static class Pair<Integer, T> {
        public final Integer offset;
        public final T object;

        public Pair(Integer offset, T bitSet) {
            this.offset = offset;
            this.object = bitSet;
        }
    }

    public static Pair<Integer, BitSet> code(BitSet bitArray, List<Integer> cList, int low, int high, int index, int offset) {

        while (true) {
            // Check base case for recursion
            if (high - low <= 1 || cList.get(high).equals(cList.get(low))) return new Pair<>(index, bitArray);

            // Calculate midpoint and the number of bits required
            int midpoint = (low + high) / 2;
            int requiredBits = (int) Math.ceil(Math.log(cList.get(high) - cList.get(low) + 1) / Math.log(2));
            int delta = cList.get(midpoint) - cList.get(low);

            // Extend the length of BitSet
            //int newLength = bitArray.length() + requiredBits;

            offset = 0;

            // Set bits in BitSet based on the calculated delta
            for (int i = 0; i < requiredBits; i++) {
                if(ImageProcessingExtension.isBitSet(delta, requiredBits - 1 - i)){
                    bitArray.set(index, true);
                    offset = 0;

                } else {
                    bitArray.set(index, false);
                    offset++;
                }
                index++;
            }

            // Recursively code the left and right halves
            if (low < midpoint) {
                Pair<Integer, BitSet> pair = code(bitArray, cList, low, midpoint, (bitArray.length() >= index)? bitArray.length() + offset: index, offset);

                bitArray = pair.object;
                index = pair.offset;
            }
            if (midpoint < high) {
                low = midpoint;
                //index = bitArray.length() + offset;
                continue;
            }

            break;
        }

        return new Pair<>(index, bitArray);
    }


    public static List<Integer> codeN(List<Integer> eList) {
        List<Integer> nList = new ArrayList<>();
        nList.add(eList.get(0));

        for (int i = 1; i < eList.size(); i++) {
            int eValue = eList.get(i);
            int encodedValue = (eValue >= 0) ? 2 * eValue : 2 * Math.abs(eValue) - 1;
            nList.add(encodedValue);
        }

        return nList;
    }

    public static List<Integer> calculateC(List<Integer> nList) {
        List<Integer> cList = new ArrayList<>();
        cList.add(nList.get(0));

        for (int i = 1; i < nList.size(); i++) {
            cList.add(cList.get(i - 1) + nList.get(i));
        }

        return cList;
    }

    public static byte[] toBytes(BitSet bitArray) {
        byte[] bytes = new byte[(bitArray.length() - 1) / 8 + 1];
        bytes = bitArray.toByteArray();
        return bytes;
    }

    public static void fixBytesOrder(byte[] bytes) {
        for (int i = 0; i < bytes.length; i++) {
            byte originalByte = bytes[i];
            byte reversedByte = 0;

            for (int bit = 0; bit < 8; bit++) {
                if ((originalByte & (1 << bit)) != 0) {
                    reversedByte |= (byte) (1 << (7 - bit));
                }
            }

            bytes[i] = reversedByte;
        }
    }
}