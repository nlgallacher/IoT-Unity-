import json
import random
from datetime import datetime
from azure.iot.device import IoTHubDeviceClient, Message

# Configuration of the device to simulate
device_id = "SimpleIotDevice"
connection_string = "HostName="

def create_sensor_message(device_id):
    message = {
        "deviceId": device_id,
        "timestamp": datetime.utcnow().isoformat() + "Z",
        "message field 1": "Simple device message",
        "message field 2": "Hello, this is a device message"
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
    try:
        # Initialize client for the device
        client = IoTHubDeviceClient.create_from_connection_string(connection_string)

        # Create a message and send it
        message = create_sensor_message(device_id)
        send_message_to_iot_hub(client, message)
         
    except Exception as e:
        print(f"Error in sending message: {e}")
    finally:
        client.shutdown()

if __name__ == "__main__":
    main()
