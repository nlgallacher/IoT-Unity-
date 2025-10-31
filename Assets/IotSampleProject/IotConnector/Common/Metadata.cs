using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndustryCSE.IoT {
    
    [Serializable]
    public class Field
    {
        public string key;
        public string value;
    }

    public class Metadata : MonoBehaviour
    {
        public List<Field> fields = new List<Field>();

        // Function to set metadata
        public void SetField(string key, string value)
        {
            var index = fields.FindIndex(i => i.key == key);
            if (index != -1)
            {
                fields[index].value = value;
            }
            else
            {
                fields.Add(new Field() { key = key, value = value });
            }
        }

        // Function to retrieve metadata
        public string GetField(string key)
        {
            var field = fields.Find(i => i.key == key);
            if (field != null)
            {
                return field.value;
            }
            else
            {
                return "Key Not Found";
            }
        }
    }
}