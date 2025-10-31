using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace IndustryCSE.IoT
{
    [CreateAssetMenu(fileName = "NewDeviceLocator", menuName = "IndustryCSE/Iot/LocateDeviceByName")]
    public class LocateDeviceByName : DeviceLocatorBase
    {
        [SerializeField] private string _deviceNameTag;
        [SerializeField] private bool _checkStartsWith;
        [SerializeField] private bool _checkContains;
        [SerializeField] private bool _checkEndsWith;

        [SerializeField] private UnityEngine.Vector3 _hotspotOffset;
        [SerializeField] private UnityEngine.Vector3 _hotspotScale;

        // Optionally, you can still define an initialization method or other methods as needed.
        private void OnEnable()
        {
            // Code to execute when the ScriptableObject is instantiated or loaded
        }

        public string DeviceNameTag
        {
            get { return _deviceNameTag; }
            set { _deviceNameTag = value; }
        }

        public override List<GameObject> LocateDevices(GameObject root)
        {
            List<GameObject> devices = FindGameObjectsByName(_deviceNameTag, root);
            return devices;
        }

        /// <summary>
        /// Function to find GameObjects by that match search criteria below a specific root node
        /// </summary>
        /// <param name="searchTag"></param>
        /// <returns></returns>
        public List<GameObject> FindGameObjectsByName(string searchTag, GameObject rootNode)
        {
            List<GameObject> matchingDevices = new List<GameObject>();

            // Ensure the root node is valid
            if (rootNode != null)
            {
                FindMatchingDevicesRecursively(rootNode, searchTag, matchingDevices);
            }

            return matchingDevices;
        }

        /// <summary>
        /// Helper method to perform recursive search
        /// </summary>
        /// <param name="current"></param>
        /// <param name="searchTag"></param>
        /// <param name="matchingDevices"></param>
        private void FindMatchingDevicesRecursively(GameObject parent, string searchTag, List<GameObject> matchingDevices)
        {
            // Check if the current object's name matches the search criteria
            if (MatchesSearchCriteria(parent.name, searchTag))
            {
                BaseDevice iot_device = null;
                // Check device type
                switch (DeviceType)
                {
                    case IoT.DeviceType.Type.Generic:
                        iot_device = parent.AddComponent<GenericDevice>();
                        break;
                    case IoT.DeviceType.Type.OccupancyDeviceGroup:
                        iot_device = parent.AddComponent<DeviceGroup>();
                        iot_device.DeviceType = IoT.DeviceType.Type.OccupancyDeviceGroup;
                        break;
                    case IoT.DeviceType.Type.ThermostatDeviceGroup:
                        iot_device = parent.AddComponent<DeviceGroup>();
                        iot_device.DeviceType = IoT.DeviceType.Type.ThermostatDeviceGroup;
                        break;
                    case IoT.DeviceType.Type.Door:
                        iot_device = parent.AddComponent<DoorSensor>();
                        break;
                    case IoT.DeviceType.Type.Occupancy:
                        iot_device = parent.AddComponent<OccupancySensor>();
                        break;
                    case IoT.DeviceType.Type.Thermostat:
                        iot_device = parent.AddComponent<Thermostat>();
                        break;
                }

                if (iot_device != null)
                {
                    iot_device.hotspotPrefab = hotspotPrefab;
                    iot_device.DeviceId = parent.name;
                    iot_device.SetInteractor(_hotspotOffset, _hotspotScale);
                }

                matchingDevices.Add(parent);
            }

            // Recursively search through children
            foreach (Transform child in parent.transform)
            {
                FindMatchingDevicesRecursively(child.gameObject, searchTag, matchingDevices);
            }
        }


        /// <summary>
        /// Returns true if name comparison is successful
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="searchTag"></param>
        /// <returns></returns>
        private bool MatchesSearchCriteria(string objectName, string searchTag)
        {
            if (_checkStartsWith)
            {
                return objectName.StartsWith(searchTag);
            }
            else if (_checkContains)
            {
                return objectName.EndsWith(searchTag);
            }

            else if (_checkEndsWith)
            {
                return objectName.Contains(searchTag);
            }

            return false;
        }
    }
}

