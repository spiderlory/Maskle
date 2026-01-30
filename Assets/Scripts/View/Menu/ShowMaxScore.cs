using System;
using Model;
using Model.Interfaces;
using TMPro;
using UnityEngine;

namespace View
{
    public class ShowMaxScore : MonoBehaviour
    {
        private IReadOnlyGameState _gameState;
        private TMP_Text _maxScoreText;
        
        private void Start()
        {
            _gameState = GameState.Instance;
            
            _maxScoreText = GetComponent<TMP_Text>();

            _maxScoreText.text = _gameState.GetMaxScore().ToString();
        }
    }
}