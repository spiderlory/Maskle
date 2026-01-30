using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View.Interfaces;

namespace View
{
    public class MaskCellUI : MonoBehaviour, IUIElement
    {
        private TMP_Text _valueTXT;
        private TMP_Text _signTXT;
        
        private Image _image;
        
        [SerializeField] private Color _positiveColor = Color.cornflowerBlue;
        [SerializeField] private Color _neutralColor = Color.gold;
        [SerializeField] private Color _negativeColor = Color.violetRed;
        
        private int _value;
        
        private void Awake()
        {
            // Init Components
            _image = GetComponent<Image>();
            _valueTXT = transform.Find("Value").GetComponent<TMP_Text>();
            _signTXT = transform.Find("Sign").GetComponent<TMP_Text>();

            _signTXT.text = "+";
            _valueTXT.text = "0";
        }
        
        public void UpdateUI()
        {
            int absValue = Mathf.Abs(_value);
            float sign = Mathf.Sign(_value);

            Color color;
            
            if (_value > 0)
                color = _positiveColor;
            else if (_value == 0)
                color = _neutralColor;
            else
                color = _negativeColor;
            
            _image.color = color;
            
            _signTXT.text = sign >= 0? "+" : "-";
            
            _valueTXT.text = absValue.ToString(); 
        }

        public void SetValue(int value)
        {
            _value = value;
        }
    }
}