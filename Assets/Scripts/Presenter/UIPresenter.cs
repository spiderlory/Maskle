using System;
using System.Collections.Generic;
using Logic;
using UnityEngine;

// Contains UI data and manages UI events and updates
// Is a singleton
public class UIPresenter : MonoBehaviour
{
    public static UIPresenter Istance { get; private set; }
    
    // UI DATA
    public int[,] CurrentMatrix { get; private set; }
    public int MatrixColumnsLength { get; private set; }

    public List<Mask> MasksList;
    public int ActiveMask => loopLogic.CurrentMask;


    // UI Events
    public event Action UpdateMatrixUI;
    public event Action UpdateMaskUI;
    
    private GameLoopLogic loopLogic;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Istance != null)
        {
            Destroy(gameObject);
        }

        Istance = this;
        loopLogic = GameLoopLogic.Istance;
        
        loopLogic.NextRound += UpdateUI;
    }
    
    public void UpdateUI()
    {
        print("GAME STARTED");
        print("UPDATE  MATRIX");
        UpdateCurrentMatrix();
        print("UPDATE MASK");
        UpdateCurrentMasks();
    }
    
    
    public void UpdateCurrentMatrix()
    {
        CurrentMatrix = loopLogic.GetCurrentMatrix();
        MatrixColumnsLength = CurrentMatrix.GetLength(1);
        
        UpdateMatrixUI?.Invoke();
    }
    
    public void UpdateCurrentMasks()
    {
        MasksList = loopLogic.GetMasks();
        UpdateMaskUI?.Invoke();
    }
    
    
    
    
    
    
    // Events
    public void OnMatrixClick(int id)
    {
        int x = id / MatrixColumnsLength;
        int y = id % MatrixColumnsLength;
        
        loopLogic.ApplyMask(x, y);
        
        UpdateCurrentMatrix();
    }

    public void OnMaskClick(int id)
    {
        print("OnMaskClick: " + id);
        loopLogic.CurrentMask = id;
        
        UpdateMaskUI?.Invoke();
    }

    public void OnUndo()
    {
        print("OnUndo");
        loopLogic.Undo();
        UpdateCurrentMatrix();
        
        UpdateMatrixUI?.Invoke();
    }
    
    
    
    
    
    // Getters and Setters
    public int GetMatrixValueAt(int index)
    {
        return CurrentMatrix[index / MatrixColumnsLength, index % MatrixColumnsLength];
    }
    
    public int[] GetMaskValues(int id)
    {
        int[] values = new int[9];
        Mask m = MasksList[id];

        int index = 0;
        for (int i = 0; i < m.NRows; i++)
        {
            for (int j = 0; j < m.NCols; j++)
            {
                values[index] = -m[i, j];
                index++;
            }
        }
        
        return values;
    }
    
}
