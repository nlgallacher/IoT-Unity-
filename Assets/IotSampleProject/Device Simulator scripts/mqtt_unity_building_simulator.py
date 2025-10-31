import json
import time
import random
from datetime import datetime
import paho.mqtt.client as mqtt

mqtt_configurations = [
    {
        "mqtt_broker": "[BROKER_IP]",
        "mqtt_port": 1883,
        "mqtt_username": "[USERNAME]",
        "mqtt_password": "[PASSWORD]"
    }
]

# Configuration of the devices to simulate
device_configurations = [
    {
        "device_id": "CanteenDoor_Sensor",
        "mqtt_topic": "building/sensors/updates",
        "type": "door_sensor"
    },
    {
        "device_id": "ReceptionDoor_Sensor",
        "mqtt_topic": "building/sensors/updates",
        "type": "door_sensor"
    },
    {
        "device_id": "Door3_Door_Sensor",
        "mqtt_topic": "building/sensors/updates",
        "type": "door_sensor"
    },
    {
        "device_id": "AirHandlingUnit_001",
        "mqtt_topic": "building/sensors/updates",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_002",
        "mqtt_topic": "building/sensors/updates",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_003",
        "mqtt_topic": "building/sensors/updates",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_004",
        "mqtt_topic": "building/sensors/updates",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_005",
        "mqtt_topic": "building/sensors/updates",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_006",
        "mqtt_topic": "building/sensors/updates",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_007",
        "mqtt_topic": "building/sensors/updates",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_008",
        "mqtt_topic": "building/sensors/updates",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_009",
        "mqtt_topic": "building/sensors/updates",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_010",
        "mqtt_topic": "building/sensors/updates",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_011",
        "mqtt_topic": "building/sensors/updates",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_012",
        "mqtt_topic": "building/sensors/updates",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_013",
        "mqtt_topic": "building/sensors/updates",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_014",
        "mqtt_topic": "building/sensors/updates",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_015",
        "mqtt_topic": "building/sensors/updates",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_016",
        "mqtt_topic": "building/sensors/updates",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_017",
        "mqtt_topic": "building/sensors/updates",
        "type": "thermostat"
    },
    {
        "device_id": "AirHandlingUnit_018",
        "mqtt_topic": "building/sensors/updates",
        "type": "thermostat"
    },
    {
        "device_id": "LunchTable",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable_1",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable_2",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable_3",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable_4",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable_5",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable_6",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable_7",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable_8",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable_9",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable_10",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable_11",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable_12",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable_13",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable_14",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable_15",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable_16",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "LunchTable_17",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "SmallTables_1",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "SmallTables_2",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "SmallTables_3",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "SmallTables_4",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "SmallTables_5",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "SmallTables_6",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "SmallTables_7",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "SmallTables_8",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "SmallTables_9",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "SmallTables_10",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "SmallTables_11",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "SmallTables_12",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    },
    {
        "device_id": "SmallTables_13",
        "mqtt_topic": "building/sensors/updates",
        "type": "occupancy_sensor"
    }
]

def create_thermostat_message(device_id):
    message = {
        "deviceId": device_id,
        "timestamp": datetime.utcnow().isoformat() + "Z",
        "currentTemperature": round(random.uniform(15.0, 35.0), 1),
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

def send_message_to_mqtt(client, topic, message):
    try:
        message_json = json.dumps(message)
        client.publish(topic, message_json)
        print(f"Message sent from {message['deviceId']}: {message_json}")
    except Exception as e:
        print(f"Failed to send message: {e}")

def main():
    clients = []

    # Initialize clients for each device
    for device_config in device_configurations:
        mqtt_client = mqtt.Client()
        mqtt_client.username_pw_set(mqtt_configurations[0]["mqtt_username"], mqtt_configurations[0]["mqtt_password"])
        mqtt_client.connect(mqtt_configurations[0]["mqtt_broker"], mqtt_configurations[0]["mqtt_port"], 60)
        clients.append({
            "client": mqtt_client,
            "device_id": device_config["device_id"],
            "topic": device_config["mqtt_topic"],
            "type": device_config["type"]
        })

    try:
        while True:
            random.shuffle(clients)

            for client_info in clients:
                device_type = client_info["type"]
                device_id = client_info["device_id"]
                client = client_info["client"]
                topic = client_info["topic"]

                if device_type == "thermostat":
                    message = create_thermostat_message(device_id)
                elif device_type == "occupancy_sensor":
                    message = create_occupancy_sensor_message(device_id)
                elif device_type == "door_sensor":
                    message = create_door_sensor_message(device_id)
                else:
                    continue

                send_message_to_mqtt(client, topic, message)
                time.sleep(1)  # Sleep for 1 second before sending the next set of messages
            time.sleep(1)  # Sleep for 1 second before sending the next set of messages

    except KeyboardInterrupt:
        print("Simulation stopped.")
    finally:
        for client_info in clients:
            client_info["client"].disconnect()

if __name__ == "__main__":
    main()
