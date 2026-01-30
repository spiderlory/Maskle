using Model;
using Model.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using View.Abstracts;
using View.Interfaces;

namespace View
{
    
    public class MatrixUI : DynamicElementOrganizerUI
    {
        [SerializeField] private MatrixCellUI _prefab;
        public IReadOnlyGameState _gameState;
        
        private GridLayoutGroup _gridLayoutGroup;
        static UIPresenter _presenter;

        private int _currentID;

        private void Start()
        {
            _presenter = UIPresenter.Instance;
            _presenter.UpdateMatrixUI += UpdateUI;

            _gameState = GameState.Instance;
            
            _gridLayoutGroup = transform.GetComponentInChildren<GridLayoutGroup>();

            _currentID = 0;
        }

        protected override IDynamicUIElement AddUIElement()
        {
            MatrixCellUI newIstance = Instantiate(_prefab, transform);
            newIstance.SetID(_currentID);
            
            _currentID++;
            
            return newIstance;
        }

        protected override void OnUpdateUI()
        {
            int[,] currentMatrix = _gameState.GetCurrentMatrix();
            
            if (_currentlyActive != currentMatrix.Length)
            {
                int colsNum = currentMatrix.GetLength(1);
                
                _gridLayoutGroup.constraintCount = colsNum;
                ActivateUIElements(currentMatrix.Length);
            }
        }
    }
}