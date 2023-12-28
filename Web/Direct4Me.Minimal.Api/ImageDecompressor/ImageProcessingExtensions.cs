using System.Collections;
using System.Drawing;

namespace ImageCompressorDecompressor;

public static class ImageProcessingExtensions
{
    public static int MagnificationIndex = 76;

    // Check if the bit at the specified position is set
    public static bool IsBitSet(this int value, int position)
    {
        return position is < 0 or >= 32
            ? throw new ArgumentOutOfRangeException(nameof(position))
            : (value & (1 << position)) != 0;
    }

    // Set the bit at the specified position in the number
    public static void SetBit(ref int number, int position)
    {
        if (position is < 0 or >= 32) throw new ArgumentOutOfRangeException(nameof(position));

        number |= 1 << position;
    }

    // Reverse the prediction to reconstruct the image based on list E
    public static Bitmap ReversePredict(IReadOnlyList<int> e, int height, int width)
    {
        var image = new Bitmap(width, height);

        for (var x = 0; x < width; x++)
        for (var y = 0; y < height; y++)
        {
            var value = e[x * height + y];

            Color color;

            if (x == 0 && y == 0)
            {
                color = Color.FromArgb(Clamp(value), Clamp(value), Clamp(value));
            }
            else if (y == 0)
            {
                var x1 = Clamp(image.GetPixel(x - 1, 0).R - value);
                color = Color.FromArgb(x1, x1, x1);
            }
            else if (x == 0)
            {
                var y1 = Clamp(image.GetPixel(0, y - 1).R - value);
                color = Color.FromArgb(y1, y1, y1);
            }
            else
            {
                int leftNeighbour = image.GetPixel(x - 1, y).R;
                int upNeighbour = image.GetPixel(x, y - 1).R;
                int diagonalNeighbour = image.GetPixel(x - 1, y - 1).R;

                var max = Math.Max(leftNeighbour, upNeighbour);
                var min = Math.Min(leftNeighbour, upNeighbour);

                if (diagonalNeighbour >= max)
                {
                    color = Color.FromArgb(Clamp(min - value), Clamp(min - value),
                        Clamp(min - value));
                }
                else if (diagonalNeighbour <= min)
                {
                    color = Color.FromArgb(Clamp(max - value), Clamp(max - value),
                        Clamp(max - value));
                }
                else
                {
                    var tmp = leftNeighbour + upNeighbour - diagonalNeighbour;
                    color = Color.FromArgb(Clamp(tmp - value), Clamp(tmp - value), Clamp(tmp - value));
                }
            }

            image.SetPixel(x, y, color);
        }

        return image;
    }

    // Clamp the value between 0 and 255
    public static int Clamp(int value)
    {
        return Math.Max(0, Math.Min(value, 255));
    }

    // Set the header for the compressed image
    public static BitArray SetHeader(ushort height, byte c0, int cl, int n)
    {
        var bitArray = new BitArray(76);
        var index = 0;

        for (var j = 11; j >= 0; j--, index++) bitArray[index] = IsBitSet(height, j);

        for (var j = 7; j >= 0; j--, index++) bitArray[index] = IsBitSet(c0, j);

        for (var j = 31; j >= 0; j--, index++) bitArray[index] = IsBitSet(cl, j);

        for (var j = 23; j >= 0; j--, index++) bitArray[index] = IsBitSet(n, j);

        return bitArray;
    }

    // Get the header values from the compressed image
    public static void GetHeader(this BitArray bitArray, ref int height, ref int width, ref int c0, ref int cl,
        ref int n)
    {
        var index = 0;

        for (var j = 11; j >= 0; j--, index++)
            if (bitArray[index])
                SetBit(ref height, j);

        for (var j = 7; j >= 0; j--, index++)
            if (bitArray[index])
                SetBit(ref c0, j);

        for (var j = 31; j >= 0; j--, index++)
            if (bitArray[index])
                SetBit(ref cl, j);

        for (var j = 23; j >= 0; j--, index++)
            if (bitArray[index])
                SetBit(ref n, j);

        width = n / height;
    }

    // Decompress the input file and save the decompressed image
    public static Bitmap Decompress(this byte[] allBytes)
    { 
        var bitArray = new BitArray(allBytes);

        bitArray.ReorderBytes();

        int height = 0, width = 0, c0 = 0, cl = 0, n = 0;

        bitArray.GetHeader(ref height, ref width, ref c0, ref cl, ref n);

        var cList = new List<int>(new[] {c0});

        for (var i = 1; i < n - 1; i++) cList.Add(0);

        cList.Add(cl);

        bitArray.Decode(ref cList, 0, n - 1);

        var calculateN = cList.CalculateN(n);

        var eList = calculateN.DecodeE();

        var bitmap = ReversePredict(eList, height, width);

        bitmap.Save($"DecompressedImage.bmp");

        return bitmap;
    }
}