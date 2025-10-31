using System;
using System.Collections.Generic;
using System.Linq;
using IndustryCSE.IoT;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// Simple script to simulate iot building device messages with randomized telemetry data.
/// </summary>
public class DeviceSimulator : MonoBehaviour
{
    public delegate void DeviceMessage(string message);
    public event DeviceMessage OnDeviceMessage;

    // Configuration of the devices to simulate
    private List<DeviceConfiguration> deviceConfigurations = new List<DeviceConfiguration>
    {
        // Doors
        new DeviceConfiguration { device_id = "CanteenDoor_Sensor", type = "door_sensor" },
        new DeviceConfiguration { device_id = "ReceptionDoor_Sensor", type = "door_sensor" },
        new DeviceConfiguration { device_id = "Door3_Door_Sensor", type = "door_sensor" },

        // Air Handling
        new DeviceConfiguration { device_id = "AirHandlingUnit_001", type = "thermostat" },
        new DeviceConfiguration { device_id = "AirHandlingUnit_002", type = "thermostat" },
        new DeviceConfiguration { device_id = "AirHandlingUnit_003", type = "thermostat" },
        new DeviceConfiguration { device_id = "AirHandlingUnit_004", type = "thermostat" },
        new DeviceConfiguration { device_id = "AirHandlingUnit_005", type = "thermostat" },
        new DeviceConfiguration { device_id = "AirHandlingUnit_006", type = "thermostat" },
        new DeviceConfiguration { device_id = "AirHandlingUnit_006_1", type = "thermostat" },
        new DeviceConfiguration { device_id = "AirHandlingUnit_007", type = "thermostat" },
        new DeviceConfiguration { device_id = "AirHandlingUnit_008", type = "thermostat" },
        new DeviceConfiguration { device_id = "AirHandlingUnit_009", type = "thermostat" },
        new DeviceConfiguration { device_id = "AirHandlingUnit_010", type = "thermostat" },
        new DeviceConfiguration { device_id = "AirHandlingUnit_011", type = "thermostat" },
        new DeviceConfiguration { device_id = "AirHandlingUnit_012", type = "thermostat" },
        new DeviceConfiguration { device_id = "AirHandlingUnit_013", type = "thermostat" },
        new DeviceConfiguration { device_id = "AirHandlingUnit_014", type = "thermostat" },
        new DeviceConfiguration { device_id = "AirHandlingUnit_015", type = "thermostat" },
        new DeviceConfiguration { device_id = "AirHandlingUnit_016", type = "thermostat" },
        new DeviceConfiguration { device_id = "AirHandlingUnit_017", type = "thermostat" },
        new DeviceConfiguration { device_id = "AirHandlingUnit_018", type = "thermostat" },

        // Occupancy
        new DeviceConfiguration { device_id = "LunchTable_1", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "LunchTable_2", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "LunchTable_3", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "LunchTable_4", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "LunchTable_5", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "LunchTable_6", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "LunchTable_7", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "LunchTable_8", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "LunchTable_9", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "LunchTable_10", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "LunchTable_11", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "LunchTable_12", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "LunchTable_13", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "LunchTable_14", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "LunchTable_15", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "LunchTable_16", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "LunchTable_17", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "LunchTable_18", type = "occupancy_sensor" },

        new DeviceConfiguration { device_id = "SmallTables_1", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "SmallTables_2", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "SmallTables_3", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "SmallTables_4", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "SmallTables_5", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "SmallTables_6", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "SmallTables_7", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "SmallTables_8", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "SmallTables_9", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "SmallTables_10", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "SmallTables_11", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "SmallTables_12", type = "occupancy_sensor" },
        new DeviceConfiguration { device_id = "SmallTables_13", type = "occupancy_sensor" },
    };

    private List<IoTHubDeviceClient> clients = new List<IoTHubDeviceClient>();

    private void Start()
    {
        InitializeClients();
        StartCoroutine(SendMessages());
    }

    private void InitializeClients()
    {
        foreach (var deviceConfig in deviceConfigurations)
        {
            clients.Add(new IoTHubDeviceClient {Type = deviceConfig.type, DeviceId = deviceConfig.device_id });
        }
    }

    private System.Collections.IEnumerator SendMessages()
    {
        while (true)
        {
            clients = clients.OrderBy(a => Guid.NewGuid()).ToList(); // Shuffle the client list randomly

            foreach (var clientInfo in clients)
            {
                var deviceType = clientInfo.Type;
                var deviceId = clientInfo.DeviceId;

                string messageJson;

                if (deviceType == "thermostat")
                {
                    var message = CreateThermostatMessage(deviceId);
                    messageJson = JsonConvert.SerializeObject(message);
                }
                else if (deviceType == "occupancy_sensor")
                {
                    var message = CreateOccupancySensorMessage(deviceId);
                    messageJson = JsonConvert.SerializeObject(message);
                }
                else if (deviceType == "door_sensor")
                {
                    var message = CreateDoorSensorMessage(deviceId);
                    messageJson = JsonConvert.SerializeObject(message);
                }
                else
                {
                    continue;
                }

                OnDeviceMessage?.Invoke(messageJson);

                yield return new WaitForSeconds(1); // Sleep for 1 second before sending the next message
            }
            yield return new WaitForSeconds(1); // Sleep for 1 second before starting the next round of messages
        }
    }

    private object CreateThermostatMessage(string deviceId)
    {
        return new
        {
            deviceId,
            timestamp = DateTime.UtcNow.ToString("o"),
            currentTemperature = Math.Round(UnityEngine.Random.Range(20.0f, 25.0f), 1),
            targetTemperature = Math.Round(UnityEngine.Random.Range(22.0f, 24.0f), 1),
            co2Emission = Math.Round(UnityEngine.Random.Range(350f, 1000f), 1),
            humidity = UnityEngine.Random.Range(30, 60),
            status = "active"
        };
    }

    private object CreateOccupancySensorMessage(string deviceId)
    {
        return new
        {
            deviceId,
            timestamp = DateTime.UtcNow.ToString("o"),
            occupancyStatus = UnityEngine.Random.Range(0, 2) == 0 ? "occupied" : "vacant",
            temperature = Math.Round(UnityEngine.Random.Range(20.0f, 25.0f), 1),
            humidity = UnityEngine.Random.Range(30, 60),
            co2Emission = Math.Round(UnityEngine.Random.Range(350f, 1000f), 1),
            lightLevel = UnityEngine.Random.Range(100, 500)
        };
    }

    private object CreateDoorSensorMessage(string deviceId)
    {
        return new
        {
            deviceId,
            timestamp = DateTime.UtcNow.ToString("o"),
            doorStatus = UnityEngine.Random.Range(0, 2) == 0 ? "open" : "closed",
            batteryLevel = UnityEngine.Random.Range(0, 100),
            temperature = Math.Round(UnityEngine.Random.Range(20.0f, 25.0f), 1),
            motionDetected = UnityEngine.Random.Range(0, 2) == 0
        };
    }
}

public class DeviceConfiguration
{
    public string device_id;
    public string connection_string;
    public string type;
}

public class IoTHubDeviceClient
{
    public string Type;
    public string DeviceId;
}
