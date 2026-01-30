using System;
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
    
    public static UIPresenter Instance { get; private set; }
    
    // UI Events
    public event Action UpdateMatrixUI;
    public event Action UpdateMaskUI;
    
    private GameLoopLogic _loopLogic;
    private IReadOnlyGameState _gameState;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            _loopLogic = GameLoopLogic.Instance;
            _gameState = GameState.Instance;
        
            _loopLogic.NextRoundEvent += UpdateUI;
        }
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
        int x = id / colsNumber;
        int y = id % colsNumber;
        
        _loopLogic.ApplyMask(x, y);
        
        UpdateUI();
    }

    public void OnMaskClick(int id)
    {
        _loopLogic.SetActiveMaskIndex(id);
        
        UpdateMaskUI?.Invoke();
    }

    public void OnUndo()
    {
        _loopLogic.Undo();
        
        UpdateUI();
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
