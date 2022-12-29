using System;
using System.IO;
using System.Text;

namespace terrain_generator
{
  public static class Helpers
  {
    // Odwracanie bajtu
    public static byte[] Reverse(this byte[] b)
    {
      Array.Reverse(b);
      return b;
    }

    // Odczytywanie Inta 16 bitowego zapisanego w formacie Big Endian
    public static Int16 ReadInt16BE(this BinaryReader binRdr)
    {
      return BitConverter.ToInt16(binRdr.ReadBytes(sizeof(Int16)).Reverse(), 0);
    }

    // Wyciąganie wysokości z konkretnych koordynatów
    public static int getElement(int[] data, int[] coords)
    {
      int elementsInRow = (int)Math.Sqrt(data.Length);
      return data[(coords[1] * elementsInRow) + coords[1]];
    }

    // Zamiana danych binarnych na liniową tablicę wysokości
    public static int[] getData(string filePath)
    {
      Console.WriteLine("{0}", filePath);

      using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
      {
        int fsLength = (int)new FileInfo(filePath).Length;
        int[] heights = new int[fsLength / 2];

        BinaryReader reader = new BinaryReader(fs, Encoding.BigEndianUnicode);
        for (int i = 0; i < fsLength / 2; i++)
        {
          heights[i] = ReadInt16BE(reader);
        }

        return heights;
      }
    }
  }
}
