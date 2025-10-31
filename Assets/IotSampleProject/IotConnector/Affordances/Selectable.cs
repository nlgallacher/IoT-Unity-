using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IndustryCSE.IoT
{
    public class Selectable : MonoBehaviour
    {
        public Action OnSelected;

        [SerializeField] TMP_Text _resourceLabel;

        private void OnMouseDown()
        {
          
            OnSelected?.Invoke();
        }

        public void ButtonPressed()
        {

            OnSelected?.Invoke();
        }

        public void SetLabel(string label)
        {
            _resourceLabel.text = label;   
        }

        public void ShowOrHideSelectables(bool show)
        {
            if (show)
            {
                this.gameObject.SetActive(true);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}

