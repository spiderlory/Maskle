using Model;
using Model.Interfaces;
using UnityEngine;
using View.Abstracts;
using View.Interfaces;

namespace View
{
    public class MasksScrollerUI : DynamicElementOrganizerUI
    {
        [SerializeField] private MaskUI _prefab;
        
        static UIPresenter _presenter;
        private static IReadOnlyGameState _gameState;

        private int _currentID;

        private void Start()
        {
            _presenter = UIPresenter.Instance;
            _presenter.UpdateMaskUI += UpdateUI;

            _currentID = 0;

            _gameState = GameState.Instance;
        }

        protected override IDynamicUIElement AddUIElement()
        {
            MaskUI newIstance = Instantiate(_prefab, transform);
            newIstance.SetID(_currentID);
            
            _currentID++;
            
            return newIstance;
        }

        protected override void OnUpdateUI()
        {
            int count = _gameState.GetMasksList().Count;
            if (_currentlyActive != count)
            {
                ActivateUIElements(count);
            }
        }
    }
}