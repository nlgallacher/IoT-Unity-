using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using M2Mqtt;
using M2Mqtt.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace IndustryCSE.IoT
{
    /// <summary>
    /// Use await messageProvider.SetModeAsync(false) to initialize in MQTT mode
    /// Use await messageProvider.SetModeAsync(true) to switch to simulation mode at runtime
    /// Use await messageProvider.SetModeAsync(false) to switch back to MQTT mode
    /// </summary>
    public class MqttMessageProvider : BaseMessageProvider
    {
        [SerializeField] private string _mqttBrokerEndpoint = "<YOUR_MQTT_BROKER_ADDRESS>";
        [SerializeField] private int _mqttBrokerPort = 1883;
        [SerializeField] private string _mqttSubTopic = "<YOUR_MQTT_Subscription_Topic>";
        [SerializeField] private string _mqttUserName = "<YOUR_MQTT_User_Name>";
        [SerializeField] private string _mqttPassword = "<YOUR_MQTT_Password>";

        [SerializeField] private bool _secured = true;

        private MqttClient _mqttClient;
        private string _clientId;
        private bool _isConnected;

        /// <summary>
        /// Switches between MQTT connection and simulated events dynamically.
        /// </summary>
        /// <param name="useSimulatedEvents">True to use simulated events; False to use MQTT.</param>
        public override async Task SetModeAsync(bool useSimulatedEvents)
        {
            _simulateEvents = useSimulatedEvents;

            if (_simulateEvents)
            {
                Debug.Log("Switching to Simulated Events mode...");

                // Disconnect MQTT if active
                DisconnectClient();

                // Register simulated device message callback
                _deviceSimulator.OnDeviceMessage += ReadSimulatedMessage;
            }
            else
            {
                Debug.Log("Switching to MQTT mode...");

                // Disable simulated events
                _deviceSimulator.OnDeviceMessage -= ReadSimulatedMessage;

                // Initialize the MQTT connection
                await InitializeAsync();
            }
        }

        /// <summary>
        /// Initializes MQTT client or sets up simulated events based on the `UseSimulatedEvents` setting.
        /// </summary>
        public override async Task InitializeAsync()
        {
            if (_simulateEvents)
            {
                Debug.Log("Initializing in Simulated Events mode...");
                _deviceSimulator.OnDeviceMessage += ReadSimulatedMessage;
                return;
            }

            Debug.Log("Initializing in MQTT mode...");
            await InitializeMqttClientAsync();
        }

        /// <summary>
        /// Initializes the MQTT client asynchronously.
        /// </summary>
        private async Task InitializeMqttClientAsync()
        {
            try
            {
                _clientId = Guid.NewGuid().ToString();
                if (!_secured)
                    _mqttClient = new MqttClient(_mqttBrokerEndpoint, _mqttBrokerPort, false, null, null, MqttSslProtocols.None);
                else 
                    _mqttClient = new MqttClient(_mqttBrokerEndpoint, _mqttBrokerPort, true, null, null, MqttSslProtocols.TLSv1_2);

                // Register event for receiving messages
                _mqttClient.MqttMsgPublishReceived += OnMqttMsgPublishReceived;

                Debug.Log("Connecting to MQTT broker...");

                Task connectTask = Task.Run(() => _mqttClient.Connect(_clientId, _mqttUserName, _mqttPassword));
                await connectTask;

                if (_mqttClient.IsConnected)
                {
                    _isConnected = true;
                    SubscribeToTopic(_mqttSubTopic);
                    Debug.Log("MQTT Client Connected");
                }
                else
                {
                    Debug.LogError("Failed to connect to the MQTT broker.");
                }
            }
            catch (SocketException se)
            {
                Debug.LogError($"[SocketException] Failed to connect to MQTT broker: {se.Message}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Exception] Failed to initialize MQTT client: {e.Message}");
            }
        }

        /// <summary>
        /// Subscribes to the specified MQTT topic.
        /// </summary>
        /// <param name="topic">The topic to subscribe to.</param>
        private void SubscribeToTopic(string topic)
        {
            if (!_isConnected || _mqttClient == null) return;

            try
            {
                _mqttClient.Subscribe(new[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                Debug.Log($"Subscribed to MQTT topic: {topic}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to subscribe to MQTT topic '{topic}': {e.Message}");
            }
        }

        /// <summary>
        /// Reads a simulated message (used in simulation mode).
        /// </summary>
        private void ReadSimulatedMessage(string message)
        {
            if (IsPaused) return;

            DeviceMessage deviceMessage = CreateDeviceMessage(message);

            if (deviceMessage != null)
            {
                MessageReceived(deviceMessage);
            }
        }

        /// <summary>
        /// Handles incoming MQTT messages.
        /// </summary>
        private void OnMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {

            if (IsPaused) return;

            try
            {
                string message = Encoding.UTF8.GetString(e.Message);
                DeviceMessage deviceMessage = CreateDeviceMessage(message);

                if (deviceMessage != null)
                {
                    MessageReceived(deviceMessage);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error processing MQTT message: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a DeviceMessage object from a raw MQTT message.
        /// </summary>
        private DeviceMessage CreateDeviceMessage(string message)
        {
            // Debug.Log("Device Message " + message);

            string deviceId = "";

            try {
                JObject body = JsonConvert.DeserializeObject<JObject>(message);
                if (body.TryGetValue("deviceId", out JToken device_id) && !string.IsNullOrEmpty(device_id?.ToString()))
                {
                    deviceId = device_id.ToString();
                }
            }
            catch (Exception)
            {
                Debug.LogWarning("Malformed MQTT message: Missing 'deviceId'.");
            }

            return new DeviceMessage(message) { DeviceId = deviceId };
        }

        /// <summary>
        /// Cleans up resources when the application quits.
        /// </summary>
        private void OnApplicationQuit()
        {
            DisconnectClient();
        }

        /// <summary>
        /// Properly disconnects the MQTT client.
        /// </summary>
        private void DisconnectClient()
        {
            if (_mqttClient != null)
            {
                try
                {
                    if (_mqttClient.IsConnected)
                    {
                        _mqttClient.Disconnect();
                        Debug.Log("MQTT client disconnected.");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error disconnecting MQTT client: {e.Message}");
                }
                finally
                {
                    _mqttClient = null;
                }
            }
        }
    }
}
