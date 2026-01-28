using System;
using System.Collections.Generic;
using UnityEngine;
using View.Interfaces;

namespace View.Abstracts
{
    public abstract class DynamicElementOrganizerUI : MonoBehaviour, IUIElement
    {
        private List<IDynamicUIElement> _uiElementsList = new List<IDynamicUIElement>();
        protected int _currentlyActive = 0;
        
        private event Action _updateUIEvent;

        public void UpdateUI()
        {
            OnUpdateUI();
            _updateUIEvent?.Invoke();
        }
        
        // Additional methods
        private IDynamicUIElement _addUIElement()
        {
            IDynamicUIElement element = AddUIElement();
            element.Deactivate();
            
            _uiElementsList.Add(element);
            _updateUIEvent += element.UpdateUI;
            
            return element;
        }


        protected void ActivateUIElements(int len)
        {
            int index = 0;

            for (int i = 0; i < len; i++)
            {
                IDynamicUIElement element = index < _uiElementsList.Count ? _uiElementsList[index] : _addUIElement();
                element.Activate();
                
                index++;
            }
            
            _currentlyActive = len;

            for (int i = index; i < _uiElementsList.Count; i++)
            {
                _uiElementsList[i].Deactivate();
                _updateUIEvent -= _uiElementsList[i].UpdateUI;
            }
        }
        
        protected abstract IDynamicUIElement AddUIElement();
        protected abstract void OnUpdateUI();
    }
}