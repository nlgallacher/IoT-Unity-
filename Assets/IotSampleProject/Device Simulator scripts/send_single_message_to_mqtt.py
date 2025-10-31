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

def create_sensor_message(device_id):
    message = {
        "deviceId": device_id,
        "timestamp": datetime.utcnow().isoformat() + "Z",
        "message field 1": "Simple device message",
        "message field 2": "message content"
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

    mqtt_client = mqtt.Client()
    mqtt_client.username_pw_set(mqtt_configurations[0]["mqtt_username"], mqtt_configurations[0]["mqtt_password"])
    mqtt_client.connect(mqtt_configurations[0]["mqtt_broker"], mqtt_configurations[0]["mqtt_port"], 60)

    topic = "[TOPIC]"
    message = create_sensor_message("[DEVICE_ID]")

    send_message_to_mqtt(mqtt_client, topic, message)

    mqtt_client.disconnect()

if __name__ == "__main__":
    main()
