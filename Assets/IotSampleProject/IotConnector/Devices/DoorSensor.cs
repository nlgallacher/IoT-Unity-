using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace IndustryCSE.IoT
{
    public class DoorSensor : BaseDevice
    {
        private ObjectAnimation _objectAnimation;

        void Awake()
        {
            //ConfigureInteractor(CalculateAnchor(), new Vector3(0.2f,0.2f, 0.2f));

            _deviceType = IoT.DeviceType.Type.Door;

            OnDeviceMessage += OnDeviceMessageHandler;
        }

        protected override void ConfigureInteractor(Vector3 anchor, Vector3 scale, bool active = false)
        {
            base.ConfigureInteractor(anchor, scale, active);

            _objectAnimation = this.gameObject.AddComponent<ObjectAnimation>();
            _objectAnimation.LocatedAnimators(this.transform);
        }
        
        private void OnDeviceMessageHandler(string message) 
        {
           //Debug.Log("Door Device Message " + message);

            try { 
                JObject bodyObj = JsonConvert.DeserializeObject<JObject>(message);
                JToken propValue = null;

                bodyObj.TryGetValue("doorStatus", out propValue);

                if (propValue.ToString().Equals("open")) {

                    _objectAnimation.Animate("T_DoorOpen");
                }
                else {
                    _objectAnimation.Animate("T_DoorClose");

                }
            }
            catch {
                
            }
            
        }

        protected override void OnSelectedChange(bool selected)
        {
            // request or release focus by camera system
            Debug.Log("Door sensor clicked");
        }
    }
}

