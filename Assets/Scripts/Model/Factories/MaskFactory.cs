using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Model
{
    public static class MaskFactory
    {
        public static Mask CreateRandomMask(int radius, int minValue, int maxValue, int count)
        {
            Mask outMask = new Mask(radius);
        
            if (maxValue - minValue <= 1)
            {
                return outMask;
            }

            maxValue++;
        
            count = Mathf.Clamp(count, 1, outMask.Length);

            List<int> randomCells = SelectRandomIndexes(outMask.Length, count);

            foreach (int cellIndex in randomCells)
            {
                int randomNumber = Random.Range(minValue, maxValue);

                while (randomNumber == 0)
                {
                    randomNumber = Random.Range(minValue, maxValue);
                }
            
                int rowIndex = cellIndex / outMask.NCols;
                int colIndex = cellIndex % outMask.NCols;
            
                outMask[rowIndex, colIndex] = randomNumber;
            }
            
            return outMask;
        }
        
        private static List<int> SelectRandomIndexes(int len, int count)
        {
            List<int> numbers = Enumerable.Range(0, len).ToList();

            count = Mathf.Clamp(count, 0, len);
        
            numbers.Shuffle();

            return numbers.GetRange(0, count);
        }
    }
}