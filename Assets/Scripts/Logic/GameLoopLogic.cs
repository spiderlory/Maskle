using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Logic
{
    public class GameLoopLogic : MonoBehaviour
    {
        public static GameLoopLogic Istance { get; private set; }
        
        // Params
        [SerializeField] private GamePreset _presetEasy;
        [SerializeField] private GamePreset _presetMedium;
        [SerializeField] private GamePreset _presetHard;
        
        private GamePreset _currentPreset;

        // Game State
        public int Score { get; private set; }
        public int RoundsCompleted { get; private set; }
        public int CurrentTimer { get; private set; }
        
        public int CurrentMask {get; set;}
        
        private int[,] initialMatrix;
        private Stack<int[,]> _matrixHistory;
        
        private List<Mask> _masksList;
        private Dictionary<Mask, int> _maskUses;
        private Stack<Mask> _maskHistory;
        
        
        // Other Game Objects
        private UIPresenter _presenter;
        
        // Events
        public event Action NextRoundEvent;
        public event Action EndGame;
        
        
        private void Awake()
        {
            if (Istance != null)
            {
                Destroy(gameObject);
            }

            Istance = this;

            _presenter = UIPresenter.Istance;
            
            _masksList = new List<Mask>();
            _matrixHistory = new Stack<int[,]>();
            
            _maskUses = new Dictionary<Mask, int>();
            _maskHistory = new Stack<Mask>();
            
            // Game State
            Score = 0;
        }

        private void Start()
        {
            StartCoroutine(LateStart());
        }

        private IEnumerator LateStart()
        {
            yield return null;
            ResetRound();
        }

        // -------------------------------
        // ---- Round Flow Management ----
        // -------------------------------
        
        public void NextRound()
        {
            Score += 7;
            RoundsCompleted++;
            
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
            initialMatrix = new int[_currentPreset.rows, _currentPreset.cols];
            Array.Clear(initialMatrix, 0, initialMatrix.Length);

            _masksList.Clear();
            _matrixHistory.Clear();
            _maskUses.Clear();
            
            // Generate Masks
            Mask tempMask;
            for (int i = 0; i < _currentPreset.nMasks; i++)
            {
                tempMask = MaskFactory.CreateRandomMask(_currentPreset.maskRadius, _currentPreset.min, _currentPreset.max, _currentPreset.count);
                _masksList.Add(tempMask);
                _maskUses.Add(tempMask, 0);
            }
            
            // Apply masks at random on the matrix
            for (int i = 0; i < _currentPreset.nMasksApplication; i++)
            {
                // Select a random mask
                tempMask = _masksList[Random.Range(0, _masksList.Count)];
                
                // Select a random position
                int x = Random.Range(1, _currentPreset.rows - 1);
                int y = Random.Range(1, _currentPreset.cols - 1);
                
                // Apply mask
                tempMask.ApplySum(initialMatrix, x, y, false);
                
                _maskUses[tempMask]++;
            }
            
            _matrixHistory.Push(initialMatrix);
            CurrentMask = 0;
        }

        private void UpdateState()
        {
            // Update difficulty based on completitions
            if (RoundsCompleted < 1)
                _currentPreset =  _presetEasy;
            else if (RoundsCompleted < 2)
                _currentPreset = _presetMedium;
            else if (RoundsCompleted < 3)
                _currentPreset = _presetHard;
        }

        // -----------------
        // ---- Actions ----
        // -----------------
        public void ApplyMask(int x, int y)
        {
            Mask currentMask = _masksList[CurrentMask];

            if (_maskUses[currentMask] == 0)
            {
                return;
            }

            int[,] currentMatrix = GetCurrentMatrix();
            int[,] copy = new int[_currentPreset.rows, _currentPreset.cols];
            Array.Copy(currentMatrix, copy, currentMatrix.Length);
            
            currentMask.ApplySum(copy, x, y, true);
            
            bool isAllZero = copy.Cast<int>().All(x => x == 0);

            if (isAllZero)
            {
                Invoke(nameof(NextRound), 1f);
            }
            
            // Update State
            _maskHistory.Push(currentMask);
            _maskUses[currentMask]--;

            int index = 0;
            if (_maskUses[currentMask] == 0)
            {
                foreach (Mask mask in _masksList)
                {
                    if (_maskUses[mask] != 0)
                    {
                        CurrentMask = index;
                        break;
                    }
                    index++;
                }
            }
            
            
            _matrixHistory.Push(copy);
        }

        public void Undo()
        {
            if (_matrixHistory.Count > 1 && _maskHistory.Count > 0)
            {
                _matrixHistory.Pop();
                Mask lastMask = _maskHistory.Pop();
                _maskUses[lastMask]++;
            }
            
        }
        
        // GETTERS and SETTERS
        public int[,] GetCurrentMatrix()
        {
            return _matrixHistory.Peek();
        }

        public List<Mask> GetMasks()
        {
            return _masksList;
        }

        public int GetMaskApplications(Mask mask)
        {
            return _maskUses[mask];
        }
        
        public int GetMaskApplications(int id)
        {
            return _maskUses[_masksList[id]];
        }
        
        public void Print(int[,] matrix)
        {
            string s = "";
            for (int x = 0; x < _currentPreset.rows; x++)
            {
                for (int y = 0; y < _currentPreset.cols; y++)
                {
                    s += "[" + matrix[x, y] + "] ";
                }

                s += '\n';
            }
            Debug.Log(s);
        }
    }
}

///
/// Game loop:
///     game start: generate a simple matrix
///     nextRound: if current matrix is null
///     game end: timer ends