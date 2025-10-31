using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IndustryCSE.IoT { 

    /// <summary>
    /// Custom context window with predefined UI elements
    /// </summary>
    public class EnhancedContextWindow : ContextWindow
    {
        /// <summary>
        /// Dictionary to track instantiated items
        /// </summary>
        private Dictionary<string, GameObject> instantiatedItems; 

        [System.Serializable]
        public class ListItemData
        {
            public Sprite icon;  // Specify the icon in the editor
            public string key; // Specify the key in the editor
            public string label; // Specify the label in the editor
        }

        [Header("List View Setup")]
        // Prefab for single list item
        public GameObject itemPrefab;              
 
        public List<ListItemData> itemsData = new List<ListItemData>(); // Specify items in the editor

        void Awake()
        {
            instantiatedItems = new Dictionary<string, GameObject>();
            PopulateList();
        }

        private void PopulateList()
        {
            // Remove existing items if necessary
            foreach (Transform child in list.transform)
            {
                Destroy(child.gameObject);
            }

            // Dynamically create items
            foreach (var itemData in itemsData)
            {
                CreateListItem(itemData, "not set");
            }
        }

        private void CreateListItem(ListItemData data, string itemValue)
        {
            // Instantiate a new item from the prefab
            GameObject newItem = Instantiate(itemPrefab, list.transform);

            // Find UI components in the prefab and assign the data
            Image icon = newItem.transform.Find("Icon").GetComponent<Image>();
            TMP_Text label = newItem.transform.Find("Label").GetComponent<TMP_Text>();
            TMP_Text value = newItem.transform.Find("Value").GetComponent<TMP_Text>();

            // Assign data to the UI elements
            if (icon != null) icon.sprite = data.icon;
            if (label != null) label.text = data.label;
            if (value != null) value.text = itemValue;

             // Add the item to the dictionary for quick lookup by label
            if (!instantiatedItems.ContainsKey(data.key))
            {
                instantiatedItems.Add(data.key, newItem);
            }
        }

        /// <summary>
        /// Updates the value of a specific list item by its label.
        /// </summary>
        /// <param name="label">The label of the item to update.</param>
        /// <param name="newValue">The new value to assign.</param>
        public void UpdateValueByLabel(string label, string newValue)
        {
            if (instantiatedItems.TryGetValue(label, out GameObject item))
            {
                TMP_Text value = item.transform.Find("Value").GetComponent<TMP_Text>();
                if (value != null)
                {
                    value.text = newValue;
                }
                else
                {
                    Debug.LogWarning($"Value Text component not found for item '{label}'!");
                }
            }
            // else
            // {
            //     Debug.LogWarning($"Item with label '{label}' was not found in the list!");
            // }
        }

        /// <summary>
        /// Add a new entry to the context view.
        /// </summary>
        /// <param name="label">Label for the entry.</param>
        /// <param name="content">Content related to the label.</param>
        public override void AddToContext(string label, string content)
        {
            if (!string.IsNullOrEmpty(label) && !string.IsNullOrEmpty(content))
            {
                UpdateValueByLabel(label, content);
            }
        }

        /// <summary>
        /// Clear all entries in the list view.
        /// </summary>
        public override void ClearContext()
        {
            
        }
    }
}

