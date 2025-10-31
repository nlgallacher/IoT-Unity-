using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace IndustryCSE.IoT
{
    public interface IDeviceProvider
    {
        public event Action<IDeviceProvider, BaseMessage> OnMessageReceived;

        public bool IsInitialized { get; }

        public Task InitializeAsync();

        public Task RequestDevicesAsync();
    }
}

