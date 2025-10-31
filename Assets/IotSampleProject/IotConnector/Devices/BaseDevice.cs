using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace IndustryCSE.IoT
{
    public class BaseDevice : MonoBehaviour, IIotDevice
    {
        public delegate void DeviceMessageProcessor(string message);

        public event DeviceMessageProcessor OnDeviceMessage;
        [SerializeField] private Queue<DeviceMessage> _messageQueue = new();

        [SerializeField] protected string _contextWindowLabel = "ContextWindow";

        protected ContextWindow contextUI;

        protected Interactor _tangibleInteractor;

        public GameObject hotspotPrefab;

        protected string lastMessage = string.Empty;

        protected bool isFocused = false;

        [SerializeField] protected DeviceType.Type _deviceType = IoT.DeviceType.Type.Generic;

        public DeviceType.Type DeviceType
        {
            get { return _deviceType; }
            set { _deviceType = value; }
        }

        [SerializeField] protected string _deviceId = "<Device Id>";

        public string DeviceId
        {
            get { return _deviceId; }
            set { _deviceId = value; }
        }

        // Start is called before the first frame update
        void Start()
        {
            // Subscribe to Device Message
            SubscribeMessageBus();
        }

        void Update()
        {
            if (IotDeviceMessageReader.Instance != null && !IotDeviceMessageReader.Instance.IsPaused())
            {
                while (_messageQueue.TryPeek(out DeviceMessage msg))
                { 
                    OnDeviceMessage?.Invoke(msg.Value);

                    _messageQueue.Dequeue();
                }
            }

        }

        protected void SubscribeMessageBus() {
            if (IotDeviceMessageReader.Instance != null)
                IotDeviceMessageReader.Instance.MessageBus.Subscribe<DeviceMessage>(OnDeviceMessageReceived);
        }

        private void OnDeviceMessageReceived(Messenger.IMessagePublisher publisher, DeviceMessage msg)
        {
            // Debug.Log("OnDeviceMessageReceived " + msg.ValueAsString());

            if (msg.DeviceId != _deviceId)
                return;

            if (msg is DeviceMessage deviceMessage)
            {
                _messageQueue.Enqueue(deviceMessage);
            }
        }

        protected virtual Vector3 CalculateAnchor() {
            int childCount = transform.childCount;

            if (childCount == 0) {
                return Vector3.zero;
            }

            Vector3 center = Vector3.zero;

            foreach (Transform child in transform) {
                center += child.position;
            }

            center /= childCount;
            return center;
        
        }

        public virtual void SetInteractor(Vector3 offset, Vector3 scale)
        {
            ConfigureInteractor(CalculateAnchor() + offset, scale);
        }

        protected virtual void ConfigureInteractor(Vector3 anchor, Vector3 scale, bool active = false)
        {
            // Adds a TangibleInteractor component to the IoT device if it doesn't already exist
            _tangibleInteractor = gameObject.GetComponent<Interactor>();

            if (_tangibleInteractor == null)
            {
                _tangibleInteractor = gameObject.AddComponent<Interactor>();
                _tangibleInteractor.SetHotspotAnchor(anchor, active);
                _tangibleInteractor.SetHotspotScale(scale);
                _tangibleInteractor.SetHotspot(hotspotPrefab);
            }

            if (_tangibleInteractor != null)
            {
                _tangibleInteractor.OnSelectionChange += OnSelectedChange;
            }
        }
        
        public virtual void SetVisibility(bool active)
        {
            _tangibleInteractor.SetHotspotVisibility(active);
        }

        protected virtual void ExposeMessage(string message) {

            if (message == string.Empty)
            {
                return;
            }

            // Debug.Log("Message to Expose: " + message);

            try
            {
                JObject bodyObj = JsonConvert.DeserializeObject<JObject>(message);

                // Iterate through each property in the JObject
                foreach (var entry in bodyObj)
                {
                    string key = entry.Key; // The key
                    JToken value = entry.Value; // The value associated with the key

                    JToken entryVal = null;
                    bodyObj.TryGetValue(key, out entryVal);
                    string entryValue = entryVal.ToString();

                    contextUI.AddToContext(key, entryValue);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.ToString());
            }
        }

        /// <summary>
        /// Generic method to find context window matching the device item type
        /// </summary>
        protected T FindFilteredObject<T>(string filter) where T : MonoBehaviour
        {
            // Use the FindFirstObjectByType method to search for the first object of the specified type
            T[] allObjectsOfType = GameObject.FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (T obj in allObjectsOfType)
            {
                // Example filter: Match by name
                if (obj.name == filter)
                {
                    return obj;
                }
                
                if (obj.name == filter)
                {
                    return obj;
                }
            }

            // Return null if no matching object is found
            return null;
        }

        protected virtual void OnSelectedChange(bool selected)
        {
            // request or release focus by camera system
            isFocused = selected;
            // request or release focus by camera system

            if (contextUI != null)
            {
                contextUI.gameObject.SetActive(selected);
                contextUI.ClearContext();
                contextUI.SetDeviceID(DeviceId);

                ExposeMessage(lastMessage);
            }
        }

        void OnDestroy()
        {
            if (_tangibleInteractor != null)
            {
                _tangibleInteractor.OnSelectionChange -= OnSelectedChange;
            }
        }
    }
}


