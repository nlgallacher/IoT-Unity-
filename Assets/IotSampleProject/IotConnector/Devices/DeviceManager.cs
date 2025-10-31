using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace IndustryCSE.IoT
{
    public class DeviceManager : MonoSingleton<DeviceManager>
    {   
        protected IDeviceProvider _deviceProvider = null;

        public async void RequestDevicesOverview()
        {
            if (_deviceProvider != null)
            {
                if (!_deviceProvider.IsInitialized)
                {
                    _deviceProvider.OnMessageReceived += OnMessageReceived;
                    await _deviceProvider.InitializeAsync();
                    //await Task.Run(async () => await _deviceProvider.InitializeAsync());
                }
                else {
                    await _deviceProvider.RequestDevicesAsync();
                    //await Task.Run(async () => await _deviceProvider.RequestDevicesAsync());
                }
            }
        }

        private void OnMessageReceived(object sender, BaseMessage msg)
        {
            if (_deviceProvider != null)
            {
                Debug.Log("OnMessageReceived " + msg);
            }
        }

    }
}

