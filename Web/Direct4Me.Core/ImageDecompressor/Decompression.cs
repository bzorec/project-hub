using System.Collections;

namespace Direct4Me.Core.ImageDecompressor;

internal static class Decompression
{
    // Decode the BitArray and update the list C based on the given parameters
    public static void Decode(this BitArray bits, ref List<int> cList, int low, int high)
    {
        while (high - low > 1)
        {
            if (cList[high] != cList[low])
            {
                var midpoint = (low + high) / 2;
                var requiredBits = (int)Math.Ceiling(Math.Log(cList[high] - cList[low] + 1, 2));
                var buffer = 0;

                for (var i = 0; i < requiredBits; i++)
                {
                    var indexToCheck = ImageProcessingExtensions.MagnificationIndex + i;

                    if (indexToCheck < 0 || indexToCheck >= bits.Length || i >= bits.Length) continue;

                    if (bits[indexToCheck]) ImageProcessingExtensions.SetBit(ref buffer, requiredBits - 1 - i);
                }

                cList[midpoint] = cList[low] + buffer;
                ImageProcessingExtensions.MagnificationIndex += requiredBits;

                if (low < midpoint) Decode(bits, ref cList, low, midpoint);

                if (midpoint < high)
                {
                    low = midpoint;
                    continue;
                }
            }
            else if (cList[high] == cList[low])
            {
                for (var i = low; i <= high && i < cList.Count; i++) cList[i] = cList[low];
            }

            break;
        }
    }

    public static void ReorderBytes(this BitArray bits)
    {
        for (var i = 0; i < bits.Length; i += 8)
        for (var z = 0; z < 4; z++)
        {
            var oppositeIndex = i + 7 - z;

            if (oppositeIndex < bits.Length && i + z < bits.Length)
                (bits[oppositeIndex], bits[i + z]) = (bits[i + z], bits[oppositeIndex]);
        }
    }


    // Calculate list N based on the list C and the given length n
    public static List<int> CalculateN(this List<int> cList, int n)
    {
        var calculateN = new List<int> { cList[0] };

        for (var i = 1; i < n; i++) calculateN.Add(cList[i] - cList[i - 1]);

        return calculateN;
    }

    // Decode list N and return list E
    public static List<int> DecodeE(this List<int> nList)
    {
        var eList = new List<int> { nList[0] };
        eList.AddRange(nList.Skip(1).Select(i => i % 2 == 0 ? i / 2 : -(i + 1) / 2));
        return eList;
    }
}