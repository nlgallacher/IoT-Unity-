using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IndustryCSE.IoT
{
    /// <summary>
    /// Manages the device locators and locates devices under a root node.
    /// </summary>
    public class DeviceLocatorManager : MonoBehaviour
    {
        [SerializeField] protected GameObject _rootNode = null!; // Null-forgiving operator to indicate that this should never be null.
        /// <summary>
        /// Locators 
        /// </summary>

        [SerializeField] protected List<DeviceLocatorBase> _deviceLocators = new List<DeviceLocatorBase>();

        /// <summary>
        /// Will be populated with located devices
        /// </summary>
        [SerializeField] protected List<GameObject> _locatedDevices = new List<GameObject>();

        /// <summary>
        /// Called before the first frame update.
        /// </summary>
        private void Awake()
        {
            LocateDevices();
        }

        /// <summary>
        /// Locates devices using the registered device locators and assigns them to _locatedDevices.
        /// </summary>
        protected virtual void LocateDevices()
        {
            if (_rootNode == null)
            {
                Debug.LogError("Root node is not assigned.");
                return;
            }

            foreach (var deviceLocator in _deviceLocators)
            {
                if (deviceLocator == null)
                {
                    Debug.LogWarning("Device locator is null, skipping.");
                    continue;
                }

                List<GameObject> devices = deviceLocator?.LocateDevices(_rootNode) ?? new List<GameObject>();
                _locatedDevices.AddRange(devices);
            }

            Debug.Log("adding iot devices total " + _locatedDevices.Count);
        }
    }

}

