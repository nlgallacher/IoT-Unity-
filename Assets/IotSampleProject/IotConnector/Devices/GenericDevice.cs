using UnityEngine;

namespace IndustryCSE.IoT {
    /// <summary>
    /// This class represents the most basic Iot device
    /// It can consume messages and expose the message in a generic context UI.
    /// </summary>
    public class GenericDevice : BaseDevice
    {
        [SerializeField] Vector3 _hotspotOffset = new Vector3(0, 3, 0);
        [SerializeField] Vector3 _hotspotScale = new Vector3(0.5f, 0.5f, 0.5f);
        void Awake()
        {
            OnDeviceMessage += OnDeviceMessageHandler;
        }

        void Start()
        {
            SubscribeMessageBus();
            ConfigureInteractor(_hotspotOffset, _hotspotScale, true);
        }

        private void OnDeviceMessageHandler(string message)
        {
            lastMessage = message;

            Debug.Log($"[INFO] IoT Device: {_deviceId} | Message: {lastMessage}");

            // Expose message in context ui
            if (isFocused && contextUI != null)
            {
                contextUI.ClearContext();
                ExposeMessage(message);
            }
        }
        
        protected override void ConfigureInteractor(Vector3 anchor, Vector3 scale, bool active = false)
        {
            base.ConfigureInteractor(anchor, scale, active);

            contextUI = FindFilteredObject<ContextWindow>(_contextWindowLabel);
        }
    }
}

