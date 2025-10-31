using System.Collections.Generic;
using UnityEngine;

namespace IndustryCSE.IoT
{
    [CreateAssetMenu(fileName = "NewDeviceLocator", menuName = "IndustryCSE/Iot/LocateDeviceByMetadata")]
    public class LocateDeviceByMetadata : DeviceLocatorBase
    {

        // Optionally, you can still define an initialization method or other methods as needed.
        private void OnEnable()
        {
            // Code to execute when the ScriptableObject is instantiated or loaded
        }

        public override List<GameObject> LocateDevices(GameObject root)
        {

            List<GameObject> devices = FindGameObjectsByType(root);
            return devices;
        }

        private List<GameObject> FindGameObjectsByType(GameObject root)
        {
            List<GameObject> iotDevices = new List<GameObject>();
            if (root == null)
                return iotDevices;

            // Find all components that implement IIotDevice in the root's hierarchy
            Metadata[] devices = root.GetComponentsInChildren<Metadata>(true);

            foreach (var device in devices)
            {
                //device.gameObject.AddComponent<NimwayIotDevice>();
                // Add them to the list
                iotDevices.Add(device.gameObject);
            }

            return iotDevices;
        }
    }
}

