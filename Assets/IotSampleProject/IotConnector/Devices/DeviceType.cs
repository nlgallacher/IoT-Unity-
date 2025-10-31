using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndustryCSE.IoT
{
    public class DeviceType
    {
        public enum Type
        {
            Generic,
            Occupancy,
            Thermostat,
            Door,
            OccupancyDeviceGroup,
            ThermostatDeviceGroup
            // Add more categories as needed
        }
    }
}
