using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Logic
{
    public class GameLoopLogic : MonoBehaviour
    {
        public static GameLoopLogic Instance { get; private set; }
        
        // Params
        [SerializeField] private GamePreset _presetEasy;
        [SerializeField] private GamePreset _presetMedium;
        [SerializeField] private GamePreset _presetHard;
        
        private GamePreset _currentPreset;

        // Game State
        private GameState _gameState;
        private GameStateHistory _gameStateHistory;

        [SerializeField] private float _timer;
        
        
        // Other Game Objects
        private UIPresenter _presenter;
        private MySceneManager _sceneManager;
        
        // Events
        public event Action NextRoundEvent;
        
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;

                _presenter = UIPresenter.Instance;
                _sceneManager = MySceneManager.Instance;
            
                _gameStateHistory = new GameStateHistory();
            }
        }

        private void Start()
        {
            _gameState = (GameState) GameState.Instance;
            _gameState.SetTimeLeft(_timer);
            _gameState.Reset();
            
            StartCoroutine(LateStart());
        }

        private IEnumerator LateStart()
        {
            yield return null;
            ResetRound();
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            _timer = Mathf.Max(0.0f, _timer);
            
            _gameState.SetTimeLeft(_timer);
            
            if (_timer <= 0)
            {
                _sceneManager.GoToMenu();
            }
        }

        // -------------------------------
        // ---- Round Flow Management ----
        // -------------------------------
        
        public void NextRound()
        {
            _gameState.IncreaseScore(7);
            _gameState.IncreaseRound();
            
            UpdateState();
            InitRound();
            
            NextRoundEvent?.Invoke();
        }
        
        public void ResetRound()
        {
            InitRound();
            NextRoundEvent?.Invoke();
        }

        private void InitRound()
        {
            UpdateState();
                
            // Clear previous data
            int[,] initialMatrix = new int[_currentPreset.rows, _currentPreset.cols];
            Array.Clear(initialMatrix, 0, initialMatrix.Length);

            _gameStateHistory.ClearHistory();
            
            // Generate Masks
            Mask tempMask;
            for (int i = 0; i < _currentPreset.nMasks; i++)
            {
                tempMask = MaskFactory.CreateRandomMask(_currentPreset.maskRadius, _currentPreset.min, _currentPreset.max, _currentPreset.count);
                _gameState.AddMask(tempMask);
            }
            
            // Apply masks at random on the matrix
            for (int i = 0; i < _currentPreset.nMasksApplication; i++)
            {
                // Select a random mask
                tempMask = (Mask) _gameState.GetMaskById(Random.Range(0, _gameState.GetMasksList().Count));
                
                // Select a random position
                int x = Random.Range(1, _currentPreset.rows - 1);
                int y = Random.Range(1, _currentPreset.cols - 1);
                
                // Apply mask
                tempMask.ApplySum(initialMatrix, x, y, false);
            }
            
            _gameState.SetActiveMaskByIndex(0);
            
            _gameStateHistory.PushMatrix(initialMatrix);
            _gameState.SetCurrentMatrix(_gameStateHistory.PeekMatrix());
        }

        private void UpdateState()
        {
            // Update difficulty based on completitions
            if (_gameState.GetCurrentRound() < 3)
                _currentPreset =  _presetEasy;
            else if (_gameState.GetCurrentRound() < 5)
                _currentPreset = _presetMedium;
            else if (_gameState.GetCurrentRound() < 7)
                _currentPreset = _presetHard;
        }

        public void EndGame()
        {
            StartCoroutine(DelayedGoToMenu());
        }
        private IEnumerator DelayedGoToMenu()
        {
            yield return new WaitForSeconds(1f);
            _sceneManager.GoToMenu();
        }

        // -----------------
        // ---- Actions ----
        // -----------------
        public void ApplyMask(int x, int y)
        {
            Mask currentMask = (Mask) _gameState.GetActiveMask();

            if (currentMask.GetApplicationsCount() == 0)
            {
                return;
            }

            int[,] currentMatrix = _gameStateHistory.PeekMatrix();
            int[,] copy = new int[_currentPreset.rows, _currentPreset.cols];
            Array.Copy(currentMatrix, copy, currentMatrix.Length);
            
            currentMask.ApplySum(copy, x, y, true);
            
            bool isAllZero = copy.Cast<int>().All(x => x == 0);
            
            // Next Round Condition
            if (isAllZero)
            {
                Invoke(nameof(NextRound), 1f);
            }
            
            // Update State
            _gameStateHistory.PushMask(currentMask);

            int index = 0;
            if (currentMask.GetApplicationsCount() == 0)
            {
                foreach (Mask mask in _gameState.GetMasksList())
                {
                    if (mask.GetApplicationsCount() != 0)
                    {
                        _gameState.SetActiveMaskByIndex(index);
                        break;
                    }
                    index++;
                }
            }
            
            _gameStateHistory.PushMatrix(copy);
            _gameState.SetCurrentMatrix(_gameStateHistory.PeekMatrix());
        }

        public void Undo()
        {
            if (_gameStateHistory.GetMatrixCount() > 1 && _gameStateHistory.GetMasksCount() > 0)
            {
                _gameStateHistory.PopMatrix();
                Mask lastMask = _gameStateHistory.PopMask();
                lastMask.UndoApplication();
                
                _gameState.SetCurrentMatrix(_gameStateHistory.PeekMatrix());
            }
            
        }
        
        // GETTERS and SETTERS
        
        public void SetActiveMaskIndex(int index)
        {
            _gameState.SetActiveMaskByIndex(index);
        }
        
        
        public static void Print(int[,] matrix)
        {
            string s = "";
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    s += "[" + matrix[x, y] + "] ";
                }

                s += '\n';
            }
            Debug.Log(s);
        }
    }
}