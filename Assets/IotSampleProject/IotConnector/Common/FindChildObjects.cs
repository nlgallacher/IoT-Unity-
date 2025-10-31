using System.Collections.Generic;
using UnityEngine;

namespace IndustryCSE.IoT { 
    public class FindChildObjects : MonoBehaviour
    {
        // Function to find all child objects of a specific component type
        public static List<T> FindChildrenOfType<T>(Transform parent) where T : Component
        {
            List<T> childrenOfType = new List<T>();
            FindChildrenRecursive(parent, ref childrenOfType);
            return childrenOfType;
        }

        // Helper recursive function to traverse the hierarchy
        private static void FindChildrenRecursive<T>(Transform parent, ref List<T> list) where T : Component
        {
            foreach (Transform child in parent)
            {
                T component = child.GetComponent<T>();
                if (component != null)
                {
                    list.Add(component);
                }
                
                // Recursively search the child's children
                FindChildrenRecursive(child, ref list);
            }
        }
    }
}
