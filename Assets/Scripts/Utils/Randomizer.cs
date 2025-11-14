using System;

namespace Utils
{
    public static class Randomizer
    {
        public static void ShuffleArray<T>(this Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1) 
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
}
