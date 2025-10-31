using IndustryCSE.IoT;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class AppStates : MonoBehaviour
{
    // Serialized fields for UI elements and objects
    [SerializeField] private TMP_Text labelOverlay;
    [SerializeField] private TMP_Text labelApplicationMode;

    [SerializeField] private GameObject messageContainer;

    [SerializeField] private TMP_Text messageDisplay;

    // Toggles for different application states
    private bool _showOccupancy;
    private bool _showHVAC;
    private bool _showRoomOccupancy;
    private bool _showRoomTemperature;
    private bool _showLocator;

    private string _msgInfoString;

    // Cached components for performance
    private BaseDevice[] _baseDevices;
    private BaseMessageProvider[] _messageProviders;

    private void Start()
    {
        Debug.Log("AppStates checking for devices");
        // Cache devices and message providers at the start for better performance
        _baseDevices = Object.FindObjectsByType<BaseDevice>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        _messageProviders = Object.FindObjectsByType<BaseMessageProvider>(FindObjectsSortMode.None);

        SubscribeMessageBus();
    }

    private void Update()
    {
        if (messageContainer.activeSelf)
        {
            messageDisplay.text = _msgInfoString;
        }
    } 

    #region Toggle Methods
    public void ToggleOccupancy()
    {
        _showOccupancy = !_showOccupancy;
        SetState("Seating Occupancy", _showOccupancy, IndustryCSE.IoT.DeviceType.Type.Occupancy);
    }

    public void ToggleHVAC()
    {
        _showHVAC = !_showHVAC;
        SetState("HVAC", _showHVAC, IndustryCSE.IoT.DeviceType.Type.Thermostat);
    }

    public void ToggleRoomOccupancy()
    {
        _showRoomOccupancy = !_showRoomOccupancy;
        SetState("Room Occupancy", _showRoomOccupancy, IndustryCSE.IoT.DeviceType.Type.OccupancyDeviceGroup);
    }

    public void ToggleRoomTemperature()
    {
        _showRoomTemperature = !_showRoomTemperature;
        SetState("Room Temperature", _showRoomTemperature, IndustryCSE.IoT.DeviceType.Type.ThermostatDeviceGroup);
    }

    public void ToggleLocate()
    {
        _showLocator = !_showLocator;

        messageContainer.SetActive(_showLocator);
    }

    #endregion

    #region State Management
    private void SetState(string label, bool isActive, IndustryCSE.IoT.DeviceType.Type deviceType)
    {
        HideVisuals();
        SetDeviceVisibility(isActive, deviceType);
        if (isActive)
        {
            labelOverlay.text = label;
        }
        else
        {
            labelOverlay.text = string.Empty;
        }
    }

    private void SetDeviceVisibility(bool isActive, IndustryCSE.IoT.DeviceType.Type deviceType)
    {
        foreach (BaseDevice component in _baseDevices)
        {
            if (component.DeviceType == deviceType)
            {
                component.SetVisibility(isActive);
            }
        }
    }

    private void HideVisuals()
    {
        SetDeviceVisibility(false, IndustryCSE.IoT.DeviceType.Type.Occupancy);
        SetDeviceVisibility(false, IndustryCSE.IoT.DeviceType.Type.Thermostat);
        SetDeviceVisibility(false, IndustryCSE.IoT.DeviceType.Type.OccupancyDeviceGroup);
        SetDeviceVisibility(false, IndustryCSE.IoT.DeviceType.Type.ThermostatDeviceGroup);
    }
    #endregion

    #region Application Mode
    /// <summary>
    /// Switch to simulated data mode.
    /// </summary>
    public void SimulateData()
    {
        labelApplicationMode.text = "Simulated";
        SetMessageProviderMode(true);
    }

    /// <summary>
    /// Switch to IoT data stream mode.
    /// </summary>
    public void ActivateIotBackend()
    {
        labelApplicationMode.text = "Cloud";
        SetMessageProviderMode(false);
    }

    private void SetMessageProviderMode(bool simulate)
    {
        foreach (BaseMessageProvider provider in _messageProviders)
        {
            provider.SetModeAsync(simulate);
        }
    }
    #endregion
    
    private void SubscribeMessageBus()
    {
        if (IotDeviceMessageReader.Instance != null)
            IotDeviceMessageReader.Instance.MessageBus.Subscribe<DeviceMessage>(OnDeviceMessageReceived);
    }

    private void OnDeviceMessageReceived(IndustryCSE.IoT.Messenger.IMessagePublisher publisher, DeviceMessage msg)
    {
        _msgInfoString = msg.ValueAsString();
    }

}
