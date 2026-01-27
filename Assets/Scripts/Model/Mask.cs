using System.Collections.Generic;
using UnityEngine;

public class Mask
{
    [SerializeField] private int _nCols;
    [SerializeField] private int _nRows;
    
    private int[] _mask;

    public Mask(int radious)
    {
        _nCols = radious * 2 + 1;
        _nRows = _nCols;
        
        _mask = new int[_nCols * _nRows];
    }

    public void ApplyMask(int iCols, int jRows)
    {
        
    }

    public void GenerateMask(int min, int max, int nCells)
    {
        nCells = Mathf.Clamp(nCells, 1, _mask.Length);

        List<int> randomCells= GenerateUniqueRandomNumbers(_mask.Length, nCells);

        foreach (int cellIndex in randomCells)
        {
            int randomNumber = Random.Range(min, max);

            while (randomNumber == 0)
            {
                randomNumber = Random.Range(min, max);
            }
            
            _mask[cellIndex] = randomNumber;
        }
    }
    
    private List<int> GenerateUniqueRandomNumbers(int max, int count)
    {
        List<int> numbers = new List<int>();
        for (int i = 0; i <= max; i++)
            numbers.Add(i);

        for (int i = numbers.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1); // Random.Range è inclusivo su min, esclusivo su max
            (numbers[i], numbers[j]) = (numbers[j], numbers[i]);
        }

        if (count > numbers.Count)
        {
            Debug.LogWarning("Count maggiore di n+1! Ridotto al massimo disponibile.");
            count = numbers.Count;
        }

        return numbers.GetRange(0, count);
    }

    public void PrintMask()
    {
        string s = "";
        for (int iCol = 0; iCol < _nCols; iCol++)
        {
            for (int jRow = 0; jRow < _nRows; jRow++)
            {
                s += "[" + _mask[jRow + iCol * _nRows] + "] ";
            }

            s += '\n';
        }
        Debug.Log(s);
    }
}
