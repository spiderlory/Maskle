using System;
using System.Collections.Generic;
using Logic;
using Model;
using Model.Interfaces;
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
    
    // UI Events
    public event Action UpdateMatrixUI;
    public event Action UpdateMaskUI;
    
    private GameLoopLogic _loopLogic;
    private IReadOnlyGameState _gameState;
    
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
        _gameState = GameState.Istance;
        
        _loopLogic.NextRoundEvent += UpdateUI;
    }
    
    public void UpdateUI()
    {
        UpdateMatrixUI?.Invoke();
        UpdateMaskUI?.Invoke();
        
        UpdateScore();
    }
    

    public void UpdateScore()
    {
        _scoreText.text = _gameState.GetCurrentScore().ToString();
    }
    
    
    // Events
    public void OnMatrixClick(int id)
    {
        int colsNumber = _gameState.GetCurrentMatrix().GetLength(1);
        _myAudioManager.PlayCellClickSound();
        int x = id / colsNumber;
        int y = id % colsNumber;
        
        _loopLogic.ApplyMask(x, y);
        
        UpdateUI();
    }

    public void OnMaskClick(int id)
    {
        _myAudioManager.PlayMaskClickSound();
        _loopLogic.SetActiveMaskIndex(id);
        
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

    public bool isOnBorder(int id)
    {
        int[,] currentMatrix = _gameState.GetCurrentMatrix();
        int colsNumber = currentMatrix.GetLength(1);
        
        int x = id / colsNumber;
        int y = id % colsNumber;

        if (x == 0 || y == 0 || x == currentMatrix.GetLength(0) - 1 || y == currentMatrix.GetLength(1) - 1)
        {
            return true;
        }
        return false;
    }
}
