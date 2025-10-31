using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Unity.VisualScripting;
using System;

namespace IndustryCSE.IoT
{
    public class Thermostat : BaseDevice
    {
        private ParticleColorModifier _particleColorModifier;

        private bool showThermostatState = false;

        private bool thermostatState;

        private float currentTemperature = 0f;

        public float CurrentTemperature
        {
            get { return currentTemperature; }
            set { currentTemperature = value; }
        }

        void Awake()
        {
            _contextWindowLabel = "EnhancedContextWindow - HVAC";
            
            // ConfigureInteractor(CalculateAnchor(), new Vector3(0.2f,0.2f, 0.2f));

            _deviceType = IoT.DeviceType.Type.Thermostat;

            OnDeviceMessage += OnDeviceMessageHandler;
        }

        void Start() {

            base.SubscribeMessageBus();
        }

        protected override void ConfigureInteractor(Vector3 anchor, Vector3 scale, bool active = false)
        {
            base.ConfigureInteractor(anchor, scale, active);

            contextUI = FindFilteredObject<EnhancedContextWindow>(_contextWindowLabel);

            _particleColorModifier = this.gameObject.AddComponent<ParticleColorModifier>();
            _particleColorModifier.LocatedParticleSystems(this.transform);
        }

        private void OnDeviceMessageHandler(string message)
        {
            lastMessage = message;

            try { 
                JObject bodyObj = JsonConvert.DeserializeObject<JObject>(message);
                JToken statusValue = null;
                bodyObj.TryGetValue("status", out statusValue);

                if (statusValue.ToString().Equals("active")) {
                    thermostatState = true;
                }
                else {
                    thermostatState = false;
                }

                JToken currentTemperatureVal = null;
                bodyObj.TryGetValue("currentTemperature", out currentTemperatureVal);

                string currentTemperatureValue = currentTemperatureVal.ToString();
                SetThermostatState(currentTemperatureValue, thermostatState);

                CurrentTemperature = float.Parse(currentTemperatureValue);
            }
            catch (Exception ex) {
                Debug.LogWarning(ex.ToString());
            }

            // Expose message in context ui
            if (isFocused) {
                contextUI.ClearContext();
                ExposeMessage(message);
            }
        }

        private void SetThermostatState(string currentTemperature, bool active) {
            float temperature = 0.0f;
            float.TryParse(currentTemperature, out temperature);

            // map temerature to paticle system
            _particleColorModifier.UpdateParticleColor(temperature, UnityEngine.Color.blue, UnityEngine.Color.red, 0, 80);
            
            if (showThermostatState) {
                EnableParticleSteam (active);
            }
        }

        public override void SetVisibility(bool active)
        {

            showThermostatState = active;

            if (thermostatState) {
                EnableParticleSteam (active);
            }

            if (!active && contextUI != null)
                contextUI.gameObject.SetActive(false);

             _tangibleInteractor.SetHotspotVisibility(active);
        }

        private void EnableParticleSteam (bool enable) {

            foreach (Transform child in transform)
            {
                 // Check if the child has a ParticleSystem component
                ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();

                if (particleSystem != null) // Only proceed if the ParticleSystem exists
                {
                    child.gameObject.SetActive(enable);
                }
            }
        }
    }

}
