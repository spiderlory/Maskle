using System;
using System.Collections.Generic;
using Model;
using Model.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View.Abstracts;

namespace View
{
    public class MaskUI : DynamicElementBaseUI
    {
        
        private Color _defaultBackColor;
        [SerializeField] private Color _activeBackColor;
        [SerializeField] private TMP_Text _usesText;
        
        
        [SerializeField] private MaskCellUI _prefab;
        private List<MaskCellUI> _cells;

        private static UIPresenter _presenter;
        private IReadOnlyGameState _gameState;
        
        private Button _button;
        private Image _image;
        
        
        private bool _isActive = false;
        
        private event Action _updateUIEvent;

        private void Awake()
        {
            _presenter = UIPresenter.Instance;
            _button = GetComponent<Button>();
            _image = GetComponent<Image>();

            _gameState = GameState.Instance;
            
            _button.onClick.AddListener(() => _presenter.OnMaskClick(_id));
            
            int childCount = 9;
            
            _cells = new List<MaskCellUI>();

            for (int i = 0; i < childCount; i++)
            {
                _cells.Add(Instantiate(_prefab, transform));
                _updateUIEvent += _cells[i].UpdateUI;
            }
            
            _defaultBackColor = _image.color;
        }

        public override void UpdateUI()
        {
            IReadOnlyMask mask = _gameState.GetMaskById(_id);
            int maskApplications = mask.GetApplicationsCount();
            
            _usesText.text = "x" + maskApplications;

            if (maskApplications == 0)
            {
                Deactivate();
            }
            else
            {
                Activate();
            }
            
            // Check if active
            if (_gameState.GetActiveMaskIndex() == _id)
            {
                _isActive = true;
                _image.color = _activeBackColor;
            }
            else
            {
                _isActive = false;
                _image.color = _defaultBackColor;
            }
            
            // Update values
            int index = 0;
            int[] values = mask.Flatten();
            
            int i = 0;
            foreach (MaskCellUI cell in _cells)
            {
                cell.SetValue(values[i]);
                i++;
            }

            _updateUIEvent?.Invoke();
        }

        public override void Activate()
        {
            gameObject.SetActive(true);
        }

        public override void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}