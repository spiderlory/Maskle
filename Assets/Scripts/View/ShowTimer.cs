using System;
using Model;
using Model.Interfaces;
using TMPro;
using UnityEngine;

namespace View
{
    public class ShowTimer : MonoBehaviour
    {
        private IReadOnlyGameState _gameState;
        private TMP_Text _text;

        private void Start()
        {
            _gameState = GameState.Instance;
            _text = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            _text.text = ((int) _gameState.GetTimeLeft()).ToString();
        }
    }
}