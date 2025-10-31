using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IndustryCSE.IoT {
    public class DateTimeGenerator : MonoBehaviour
    {
        [SerializeField] Text _dateTimeText;
        private DateTime _nextLogTime;

        private void Start()
        {
            _nextLogTime = DateTime.Now;
        }

        // Update is called once per frame
        void Update()
        {
            // If the current time is equal to or past the next log time...
            if (DateTime.Now >= _nextLogTime)
            {
                // Get current date and time
                DateTime currentDateTime = DateTime.Now;

                _dateTimeText.text = currentDateTime.ToString("HH:mm ddd dd MMM yyyy");
                // Update next log time
                _nextLogTime = DateTime.Now.AddMinutes(1);
            }
        }
    }
}


