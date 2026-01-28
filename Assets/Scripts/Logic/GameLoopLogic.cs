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
        public int rows = 2;
        public int cols = 2;
        
        public int nMasks = 3;
        public int nMasksApplication = 3;
        
        public int maskRadius = 1;
        public int min = 0;
        public int max = 2;
        public int count = 3;

        // Game State
        private int score;
        private int currentTimer;
        
        public int CurrentMask {get; set;}
        
        private int[,] initialMatrix;
        private Stack<int[,]> matrixStates;
        private List<Mask> _masksList;
        
        // Other Game Objects
        private UIPresenter _presenter;
        
        // Events
        public event Action NextRound;
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
            matrixStates = new Stack<int[,]>();
        }

        private void Start()
        {
            StartCoroutine(LateStart());
        }

        private IEnumerator LateStart()
        {
            yield return null;
            InitRound();
        }

        // Initialize the round, create a new matrix and filters based on the game params
        public void InitRound()
        {
            // Clear previous data
            initialMatrix = new int[rows, cols];
            Array.Clear(initialMatrix, 0, initialMatrix.Length);

            _masksList.Clear();
            matrixStates.Clear();
            
            // Generate Round Masks
            Mask tempMask;
            for (int i = 0; i < nMasks; i++)
            {
                tempMask = MaskFactory.CreateRandomMask(maskRadius, min, max, count);
                _masksList.Add(tempMask);
            }
            
            // Apply masks at random on the matrix
            for (int i = 0; i < nMasksApplication; i++)
            {
                // Select a random mask
                tempMask = _masksList[Random.Range(0, _masksList.Count)];
                
                // Select a random position
                int x = Random.Range(0, rows);
                int y = Random.Range(0, cols);
                
                // Apply mask
                initialMatrix = tempMask.ApplySum(initialMatrix, x, y, false);
            }
            
            matrixStates.Push(initialMatrix);
            CurrentMask = 0;
            
            NextRound?.Invoke();
        }

        public void ApplyMask(int x, int y)
        {
            Mask currentMask = _masksList[CurrentMask];

            int[,] currentMatrix = GetCurrentMatrix();
            int[,] copy = new int[rows, cols];
            Array.Copy(currentMatrix, copy, currentMatrix.Length);
            
            currentMask.ApplySum(copy, x, y, true);
            matrixStates.Push(copy);
        }

        public void Undo()
        {
            if (matrixStates.Count > 1)
            {
                matrixStates.Pop();
            }
        }
        
        // GETTERS and SETTERS
        public int[,] GetCurrentMatrix()
        {
            return matrixStates.Peek();
        }

        public List<Mask> GetMasks()
        {
            return _masksList;
        }
        
        public void Print(int[,] matrix)
        {
            string s = "";
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
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