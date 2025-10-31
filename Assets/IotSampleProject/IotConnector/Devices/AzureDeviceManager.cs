using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndustryCSE.IoT
{
    public class AzureDeviceManager : DeviceManager
    {
        [SerializeField] private AzureDeviceProvider _azureDeviceProvider;

        protected override void InternalInit()
        {
            _deviceProvider = _azureDeviceProvider;
        }

        // Start is called before the first frame update
        void Start()
        {
            RequestDevicesOverview();
        }
    }
}

