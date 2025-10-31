import json
import time
import random
from datetime import datetime
from azure.iot.device import IoTHubDeviceClient, Message

# Configuration of the devices to simulate
device_configurations = [
    
    {
        "device_id": "CanteenDoor_Sensor",
        "connection_string": "HostName=",
        "type": "door_sensor"
    },
    {
        "device_id": "ReceptionDoor_Sensor",
        "connection_string": "HostName=",
        "type": "door_sensor"
    },
    {
        "device_id": "Door3_Door_Sensor",
        "connection_string": "HostName=",
        "type": "door_sensor"
    },
    {
        "device_id": "AirHandlingUnit_001",
        "connection_string": "HostName=",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_002",
        "connection_string": "HostName=",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_003",
        "connection_string": "HostName=",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_004",
        "connection_string": "HostName=",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_005",
        "connection_string": "HostName=",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_006",
        "connection_string": "HostName=",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_007",
        "connection_string": "HostName=",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_008",
        "connection_string": "HostName=",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_009",
        "connection_string": "HostName=",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_010",
        "connection_string": "HostName=",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_011",
        "connection_string": "HostName=",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_012",
        "connection_string": "HostName=",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_013",
        "connection_string": "HostName=",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_014",
        "connection_string": "HostName=",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_015",
        "connection_string": "HostName=",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_016",
        "connection_string": "HostName=",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_017",
        "connection_string": "HostName=",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_018",
        "connection_string": "HostName=",
        "type": "thermostat"
    },
    {
        "device_id": "LunchTable",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable1",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable2",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable3",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable4",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable5",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable6",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable7",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable8",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable9",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable10",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable11",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable12",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable13",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable14",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable15",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable16",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable17",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "Table_Round",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "Table_Round1",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "Table_Round2",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "Table_Round3",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "Table_Round4",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "Table_Round5",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "Table_Round6",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "Table_Round7",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "Table_Round8",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "Table_Round9",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "Table_Round10",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "Table_Round11",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "Table_Round12",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "Table_Round13",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "Table_Round14",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "Table_Round15",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "Table_Round16",
        "connection_string": "HostName=",
        "type": "occupancy_sensor"
    }
]

def create_thermostat_message(device_id):
    message = {
        "deviceId": device_id,
        "timestamp": datetime.utcnow().isoformat() + "Z",
        "currentTemperature": round(random.uniform(20.0, 25.0), 1),
        "targetTemperature": round(random.uniform(22.0, 24.0), 1),
        "humidity": random.randint(30, 60),
        "status": "active"
    }
    return message

def create_occupancy_sensor_message(device_id):
    message = {
        "deviceId": device_id,
        "timestamp": datetime.utcnow().isoformat() + "Z",
        "occupancyStatus": random.choice(["occupied", "vacant"]),
        "temperature": round(random.uniform(20.0, 25.0), 1),
        "humidity": random.randint(30, 60),
        "lightLevel": random.randint(100, 500)
    }
    return message

def create_door_sensor_message(device_id):
    message = {
        "deviceId": device_id,
        "timestamp": datetime.utcnow().isoformat() + "Z",
        "doorStatus": random.choice(["open", "closed"]),
        "batteryLevel": random.randint(0, 100),
        "temperature": round(random.uniform(20.0, 25.0), 1),
        "motionDetected": random.choice([True, False])
    }
    return message

def send_message_to_iot_hub(client, message):
    try:
        message_json = json.dumps(message)
        msg = Message(message_json)
        client.send_message(msg)
        print(f"Message sent from {message['deviceId']}: {message_json}")
    except Exception as e:
        print(f"Failed to send message: {e}")

def main():
    clients = []

    # Initialize clients for each device
    for device_config in device_configurations:
        client = IoTHubDeviceClient.create_from_connection_string(device_config["connection_string"])
        clients.append({"client": client, "type": device_config["type"], "device_id": device_config["device_id"]})

    try:
        while True:
            random.shuffle(clients)
            
            for client_info in clients:
                device_type = client_info["type"]
                device_id = client_info["device_id"]
                client = client_info["client"]

                if device_type == "thermostat":
                    message = create_thermostat_message(device_id)
                elif device_type == "occupancy_sensor":
                    message = create_occupancy_sensor_message(device_id)
                elif device_type == "door_sensor":
                    message = create_door_sensor_message(device_id)
                else:
                    continue

                send_message_to_iot_hub(client, message)
                time.sleep(1)  # Sleep for 5 seconds before sending the next set of messages
            time.sleep(1)  # Sleep for 5 seconds before sending the next set of messages

    except KeyboardInterrupt:
        print("Simulation stopped.")
    finally:
        for client_info in clients:
            client_info["client"].shutdown()

if __name__ == "__main__":
    main()
