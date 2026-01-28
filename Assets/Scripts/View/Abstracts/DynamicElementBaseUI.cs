using UnityEngine;
using View.Interfaces;

namespace View.Abstracts
{
    public abstract class DynamicElementBaseUI : MonoBehaviour, IDynamicUIElement
    {
        protected int _id;


        public void SetID(int id)
        {
            _id = id;
        }

        public int GetID()
        {
            return _id;
        }

        public abstract void Activate();
        public abstract void Deactivate();

        public abstract void UpdateUI();
    }
}