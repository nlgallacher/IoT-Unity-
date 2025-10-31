import json
import time
import random
from datetime import datetime
from azure.iot.device import IoTHubDeviceClient, Message

# Configuration of the devices to simulate
device_configurations = [
    
    {
        "device_id": "unity1",
        "connection_string": "HostName=REPLACE_WITH_DEVICE_STRING",
        "type": "door_sensor"
    }
]


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

                # if device_type == "thermostat":
                #     message = create_thermostat_message(device_id)
                # elif device_type == "occupancy_sensor":
                #     message = create_occupancy_sensor_message(device_id)
                if device_type == "door_sensor":
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
