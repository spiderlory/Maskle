
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View.Abstracts;

namespace View
{
    public class MatrixCellUIMenu : MonoBehaviour
    {
        private TMP_Text _value;
        private TMP_Text _sign;
        
        private Image _image;
        
        [SerializeField] private Color _positiveColor = Color.cornflowerBlue;
        [SerializeField] private Color _neutralColor = Color.gold;
        [SerializeField] private Color _negativeColor = Color.violetRed;

        
        private void Awake()
        {
            // Init Components
            _image = GetComponent<Image>();
            _value = transform.Find("Value").GetComponent<TMP_Text>();
            _sign = transform.Find("Sign").GetComponent<TMP_Text>();

            SetRandomValue();

        }
        
        public void SetRandomValue()
        {
            int value = Random.Range(-4, 4);
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
    }
}