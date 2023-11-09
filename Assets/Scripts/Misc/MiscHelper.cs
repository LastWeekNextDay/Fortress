using System;

public class MiscHelper
{
    public static T[] ConvertTo1DArray<T>(T[,] array)
    {
        // Step 1: get total size of 2D array, and allocate 1D array.
        int size = array.Length;
        T[] result = new T[size];

        // Step 2: copy 2D array elements into a 1D array.
        int write = 0;
        for (int i = 0; i <= array.GetUpperBound(0); i++)
        {
            for (int z = 0; z <= array.GetUpperBound(1); z++)
            {
                result[write++] = array[i, z];
            }
        }
        // Step 3: return the new array.
        return result;
    }
}
