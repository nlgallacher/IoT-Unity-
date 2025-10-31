using System.Reflection;
using TMPro;
using UnityEngine;

namespace IndustryCSE.IoT {
    /// <summary>
    /// Context window script to display dynamic entries in a list view with improved formatting.
    /// </summary>
    public class ContextWindow : MonoBehaviour
    {
        /// <summary>
        /// Title text for the context window.
        /// </summary>
        [Header("Header UI Elements")]
        [SerializeField] private TMP_Text deviceID;

        /// <summary>
        /// Scroll view container where entries are displayed.
        /// </summary>
        [Header("List Container")]
        [SerializeField] protected GameObject list;

        /// <summary>
        /// Set the device ID on the header text.
        /// </summary>
        /// <param name="deviceId">The identifier to display.</param>
        public void SetDeviceID(string deviceId)
        {
            if (deviceID != null)
            {
                deviceID.text = $"Device ID: {deviceId}";
            }
        }

        /// <summary>
        /// Clear all entries in the list view.
        /// </summary>
        public virtual void ClearContext()
        {
            if (list != null)
            {
                for (int i = list.transform.childCount - 1; i >= 0; i--)
                {
                    Transform child = list.transform.GetChild(i);
                    Destroy(child.gameObject);
                }
            }
        }

        /// <summary>
        /// Add a new entry to the context view.
        /// </summary>
        /// <param name="label">Label for the entry.</param>
        /// <param name="content">Content related to the label.</param>
        public virtual void AddToContext(string label, string content)
        {
            if (!string.IsNullOrEmpty(label) && !string.IsNullOrEmpty(content))
            {
                CreateTMPTextEntry(list, label, content, 28f);
            }
        }

        /// <summary>
        /// Create a new TMP_Text entry and add it to the list view.
        /// </summary>
        /// <param name="parent">Parent GameObject for the entry.</param>
        /// <param name="label">Label for the entry.</param>
        /// <param name="content">Content for the entry.</param>
        /// <param name="fontSize">Font size for the text object.</param>
        private void CreateTMPTextEntry(GameObject parent, string label, string content, float fontSize)
        {
            if (parent == null) return;

            // Create a new GameObject for the entry
            GameObject textEntry = new GameObject($"{label}_Entry");
            textEntry.transform.SetParent(parent.transform, false);

            // Add and configure TMP_Text component
            TextMeshProUGUI tmp = textEntry.AddComponent<TextMeshProUGUI>();
            tmp.text = $"<b>{label}:</b> {content}";
            tmp.fontSize = fontSize;
            tmp.color = new Color(0.9f, 0.9f, 0.9f); // Slight off-white color for better UI contrast
            tmp.alignment = TextAlignmentOptions.Left;
            tmp.margin = new Vector4(10, 5, 10, 5); // Small margin/padding for better spacing

            // Optionally, add layout components for better alignment and spacing in a vertical layout
            RectTransform rectTransform = textEntry.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(0, fontSize * 2); // Adjust height dynamically
        }
    }
}

