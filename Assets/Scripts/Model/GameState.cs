using System;
using System.Collections.Generic;
using Model.Interfaces;
using UnityEngine;

namespace Model
{
    public class GameState : MonoBehaviour, IReadOnlyGameState
    {

        public static IReadOnlyGameState Instance  {get; private set;}
        private int _maxScore = 0;
        private int _currentScore;

        private int _currentRound;

        private float _timeLeft;

        private int[,] _currentMatrix;
        private List<IReadOnlyMask> _masksList;
        private IReadOnlyMask _activeMask;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            
                Instance = this;
                _masksList =  new List<IReadOnlyMask>();
            }
        }

        // ---------------
        // ---- SCORE ----
        // ---------------
        
        // READ
        public int GetMaxScore()
        {
            return _maxScore;
        }
        
        public int GetCurrentScore()
        {
            return _currentScore;
        }
        
        // WRITE
        public void IncreaseScore(int score)
        {
            if (score > 0)
            {
                _currentScore += score;
            }

            if (_currentScore > _maxScore)
            {
                _maxScore = _currentScore;
            }
        }

        public void ResetScore()
        {
            _currentScore = 0;
        }

        // ----------------
        // ---- ROUNDS ----
        // ----------------
        
        // READ
        
        public int GetCurrentRound()
        {
            return _currentRound;
        }
        
        // WRITE
        
        public void IncreaseRound()
        {
            _currentRound++;
        }

        public void ResetRound()
        {
            _currentRound = 0;
        }

        // --------------
        // ---- TIME ----
        // --------------
        
        // READ
        
        public float GetTimeLeft()
        {
            return _timeLeft;
        }
        
        // WRITE
        
        public void SetTimeLeft(float timeLeft)
        {
            _timeLeft = timeLeft;
        }

        // ----------------
        // ---- MATRIX ----
        // ----------------
        
        // READ
        
        public int[,] GetCurrentMatrix()
        {
            return _currentMatrix;
        }

        public int GetMatrixValueAt(int index)
        {
            int colsNumber = _currentMatrix.GetLength(1);
            
            int x = index / colsNumber;
            int y = index % colsNumber;
            
            return _currentMatrix[x, y];
        }
        
        
        // WRITE
        
        public void SetCurrentMatrix(int[,] matrix)
        {
            _currentMatrix = matrix;
        }
        // ---------------
        // ---- MASKS ----
        // ---------------
        
        // READ
        public List<IReadOnlyMask> GetMasksList()
        {
            return _masksList;
        }
        
        public IReadOnlyMask GetActiveMask()
        {
            return _activeMask;
        }
        
        public int GetActiveMaskIndex()
        {
            return _masksList.IndexOf(_activeMask);
        }
        
        public IReadOnlyMask GetMaskById(int id)
        {
            return _masksList[id];
        }
        
        
        // WRITE
        
        public void SetActiveMaskByIndex(int maskIndex)
        {
            _activeMask = _masksList[maskIndex];
        }
        
        public void AddMask(IReadOnlyMask mask)
        {
            _masksList.Add(mask);
        }

        public void ClearMasks()
        {
            _masksList.Clear();
        }


        public void Reset()
        {
            ResetRound();
            ResetScore();
            ClearMasks();
        }
    }
}