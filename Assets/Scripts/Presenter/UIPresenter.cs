using System;
using System.Collections.Generic;
using Logic;
using TMPro;
using UnityEngine;

// Contains UI data and manages UI events and updates
// Is a singleton
public class UIPresenter : MonoBehaviour
{
    
    // UI ELEMENTS
    [SerializeField] private TMP_Text _scoreText;
    
    // Sounds
    private MyAudioManager _myAudioManager;
    
    public static UIPresenter Istance { get; private set; }
    
    // UI DATA
    public int[,] CurrentMatrix { get; private set; }
    public int MatrixColumnsLength { get; private set; }

    public List<Mask> MasksList;
    public int ActiveMask => _loopLogic.CurrentMask;


    // UI Events
    public event Action UpdateMatrixUI;
    public event Action UpdateMaskUI;
    
    private GameLoopLogic _loopLogic;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Istance != null)
        {
            Destroy(gameObject);
        }

        Istance = this;
        _loopLogic = GameLoopLogic.Istance;
        _myAudioManager = MyAudioManager.Istance;
        
        _loopLogic.NextRoundEvent += UpdateUI;
    }
    
    public void UpdateUI()
    {
        UpdateCurrentMatrix();
        UpdateCurrentMasks();

        UpdateScore();
    }
    
    
    public void UpdateCurrentMatrix()
    {
        CurrentMatrix = _loopLogic.GetCurrentMatrix();
        MatrixColumnsLength = CurrentMatrix.GetLength(1);
        
        UpdateMatrixUI?.Invoke();
    }
    
    public void UpdateCurrentMasks()
    {
        MasksList = _loopLogic.GetMasks();
        UpdateMaskUI?.Invoke();
    }

    public void UpdateScore()
    {
        _scoreText.text = _loopLogic.Score.ToString();
    }
    
    
    
    
    
    
    // Events
    public void OnMatrixClick(int id)
    {
        _myAudioManager.PlayCellClickSound();
        int x = id / MatrixColumnsLength;
        int y = id % MatrixColumnsLength;
        
        _loopLogic.ApplyMask(x, y);
        
        UpdateUI();
    }

    public void OnMaskClick(int id)
    {
        _myAudioManager.PlayMaskClickSound();
        _loopLogic.CurrentMask = id;
        
        UpdateMaskUI?.Invoke();
    }

    public void OnUndo()
    {
        _loopLogic.Undo();
        
        UpdateUI();
    }

    public void OnExit()
    {
        print("OnExit");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    
    
    // Getters and Setters
    public int GetMatrixValueAt(int index)
    {
        return CurrentMatrix[index / MatrixColumnsLength, index % MatrixColumnsLength];
    }

    public int GetMaskApplications(int id)
    {
        return _loopLogic.GetMaskApplications(id);
    }

    public bool isOnBorder(int id)
    {
        int x = id / MatrixColumnsLength;
        int y = id % MatrixColumnsLength;

        if (x == 0 || y == 0 || x == CurrentMatrix.GetLength(0) - 1 || y == CurrentMatrix.GetLength(1) - 1)
        {
            return true;
        }
        return false;
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
