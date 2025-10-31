using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace IndustryCSE.IoT {
    public class DeviceGroup : BaseDevice
    {
        [SerializeField] private List<BaseDevice> _locatedDevices = new List<BaseDevice>();

        void Awake()
        {
            LocateSubDevices();
        }

        protected override void ConfigureInteractor(Vector3 anchor, Vector3 scale, bool active = false)
        {
            base.ConfigureInteractor(anchor, scale, active);
        }

        protected override Vector3 CalculateAnchor() 
        {
            // Ensure the parent has a MeshFilter component
            MeshFilter parentMeshFilter = this.GetComponent<MeshFilter>();
            if (parentMeshFilter == null)
            {
                Debug.LogError("Parent GameObject does not have a MeshFilter component.");
                return Vector3.zero;
            }

            // Calculate the center of the bounding box in local space
            Mesh parentMesh = parentMeshFilter.sharedMesh;
            if (parentMesh == null)
            {
                Debug.LogError("Parent GameObject's MeshFilter does not have a valid mesh.");
                return Vector3.zero;
            }

            Bounds bounds = parentMesh.bounds;
            Vector3 localCenter = bounds.center;

            // Convert the local center to world space
            Vector3 worldCenter = this.transform.TransformPoint(localCenter);

            return worldCenter;
        }

        void LocateSubDevices()
        {
            if (this.DeviceType == IoT.DeviceType.Type.OccupancyDeviceGroup) {
                _contextWindowLabel = "EnhancedContextWindow - Rooms";
             }
             else if (this.DeviceType == IoT.DeviceType.Type.ThermostatDeviceGroup) {
                _contextWindowLabel = "EnhancedContextWindow - Other";
                
             }
             
            List<BaseDevice> devices = FindChildObjects.FindChildrenOfType<BaseDevice>(this.transform.parent);

            foreach(BaseDevice device in devices) {
                if (this.DeviceType == IoT.DeviceType.Type.OccupancyDeviceGroup && device.DeviceType == IoT.DeviceType.Type.Occupancy) {
                    _locatedDevices.Add(device);
                }
                else if (this.DeviceType == IoT.DeviceType.Type.ThermostatDeviceGroup && device.DeviceType == IoT.DeviceType.Type.Thermostat) {
                    _locatedDevices.Add(device);
                }
            }

             contextUI = FindFilteredObject<EnhancedContextWindow>(_contextWindowLabel);
        }

        public override void SetVisibility(bool active)
        {
            this.gameObject.SetActive(active);

            AggregateDevices();

            if (_tangibleInteractor == null) {
                
                Vector3 center = CalculateAnchor();
                Vector3 scale = new Vector3(0.2f,0.2f, 0.2f);

                ConfigureInteractor(center, scale, true);
            }

            _tangibleInteractor.SetHotspotVisibility(active);
            
            if (contextUI != null && !active)
                contextUI.gameObject.SetActive(active);
        }

        private float AggregateDevices() 
        {
            float aggregatedValue = 0.0f;

            float minMetric = 0.0f;
            float maxMetric = 1.0f;

            UnityEngine.Color minColor = UnityEngine.Color.green;
            UnityEngine.Color maxColor = UnityEngine.Color.red;

            switch (DeviceType) 
            {
                case IoT.DeviceType.Type.OccupancyDeviceGroup:
                    aggregatedValue = AggregateOccupiedDevices();
                    maxMetric = (float)_locatedDevices.Count;
                    break;

                case IoT.DeviceType.Type.ThermostatDeviceGroup:
                    aggregatedValue = AggregateTemperatureDevices();
                    minColor = UnityEngine.Color.blue;
                    maxMetric = 50.0f;
                    break;

                default:
                    Debug.LogWarning($"Unsupported device type: {DeviceType}");
                    break;
            }

            UpdateDeviceGroupColor(_locatedDevices.Count, aggregatedValue, minColor, maxColor, minMetric, maxMetric);
            return aggregatedValue;
        }

        private float AggregateOccupiedDevices()
        {
            int occupiedCount = 0;

            foreach (OccupancySensor device in _locatedDevices) 
            {
                if (device.IsOccupied)
                {
                    occupiedCount++;
                }
            }

            return (float)occupiedCount;
        }

        private float AggregateTemperatureDevices()
        {
            float temperatureSum = 0.0f;

            foreach (Thermostat device in _locatedDevices) 
            {
                temperatureSum += device.CurrentTemperature;
            }

            if (_locatedDevices.Count > 0) {
                temperatureSum = temperatureSum/_locatedDevices.Count;
            }

            return temperatureSum;
        }

        public void UpdateDeviceGroupColor(int totalSensors, float value, UnityEngine.Color minColor, UnityEngine.Color maxColor, float minValue = 0f, float maxValue = 1f)
        {
            // Clamp the input value to ensure we don't exceed the defined min/max thresholds
            float clampedValue = Mathf.Clamp(value, minValue, maxValue);

            // Map the clampedValue to a normalized range based on totalSensors
            float normalizedRate = Mathf.Clamp01((clampedValue - minValue) / (maxValue - minValue));

            // Interpolate between green (low occupancy) and red (high occupancy) colors
            UnityEngine.Color interpolatedColor = UnityEngine.Color.Lerp(minColor, maxColor, normalizedRate);

            // Ensure totalSensors is greater than 0 to avoid invalid calculations
            if (totalSensors <= 0)
            {
                interpolatedColor = UnityEngine.Color.gray;
            }

            // Set alpha to 0.5 (semi-transparent)
            interpolatedColor.a = 0.5f;

            // Get the Renderer component attached to the GameObject
            Renderer renderer = GetComponent<Renderer>();

            if (renderer == null || renderer.material == null)
            {
                Debug.LogWarning("No Renderer or material found on the GameObject.");
                return;
            }

            // Apply the calculated color to the material
            renderer.material.color = interpolatedColor;
        }

        protected override void OnSelectedChange(bool selected)
        {
            float aggregatedValue = AggregateDevices();
            
            contextUI.gameObject.SetActive(selected);

            contextUI.SetDeviceID(DeviceId);

            if (this.DeviceType == IoT.DeviceType.Type.OccupancyDeviceGroup) {

                contextUI.AddToContext("totalDevices", _locatedDevices.Count.ToString()); 
                contextUI.AddToContext("availableSeats", (_locatedDevices.Count - aggregatedValue).ToString("0")); 

             }
             else if (this.DeviceType == IoT.DeviceType.Type.ThermostatDeviceGroup) {
                
                contextUI.AddToContext("totalDevices", _locatedDevices.Count.ToString()); 
                contextUI.AddToContext("averageTemperature", aggregatedValue.ToString("0.0")); 
             }
            
        }
    }
}

