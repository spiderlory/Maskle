using Model;
using Model.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View.Abstracts;

namespace View
{
    public class MatrixCellUI : DynamicElementBaseUI
    {
        private TMP_Text _value;
        private TMP_Text _sign;
        private Button _button;
        
        private Image _image;
        
        [SerializeField] private Color _positiveColor = Color.cornflowerBlue;
        [SerializeField] private Color _neutralColor = Color.gold;
        [SerializeField] private Color _negativeColor = Color.violetRed;
        
        private IReadOnlyGameState _gameState;
         
        
        static UIPresenter _presenter;

        private bool _isDisabled = false;
        
        private void Awake()
        {
            // Init Components
            _image = GetComponent<Image>();
            _value = transform.Find("Value").GetComponent<TMP_Text>();
            _sign = transform.Find("Sign").GetComponent<TMP_Text>();
            _button = GetComponent<Button>();

            _gameState = GameState.Instance;
            
            _button.onClick.AddListener(() => _presenter.OnMatrixClick(_id));

            _presenter = UIPresenter.Instance;
            
            _sign.text = "+";
            _value.text = "0";
            
        }
        
        public override void Activate()
        {
            gameObject.SetActive(true);
            UpdateUI();
        }
        
        public override void UpdateUI()
        {
            SetState(_isDisabled);
            
            int value = _gameState.GetMatrixValueAt(_id);
            
            int absValue = Mathf.Abs(value);
            float sign = Mathf.Sign(value);

            Color color;
            
            if (value > 0)
                color = _positiveColor;
            else if (value == 0)
                color = _neutralColor;
            else
                color = _negativeColor;

            _image.color = color;
            _sign.text = sign >= 0? "+" : "-";
            
            _value.text = absValue.ToString(); 
        }
        

        public override void Deactivate()
        {
            gameObject.SetActive(false);
        }


        private void SetState(bool isDisabled)
        {
            _isDisabled = _presenter.isOnBorder(_id);
            
            if (isDisabled)
            {
                Disable();
            }
            else
            {
                Enable();
            }
        }
        private void Enable()
        {
            _button.interactable = true;
            
        }

        private void Disable()
        {
            _button.interactable = false;
        }
    }
}