using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace IndustryCSE.IoT
{
    public class BaseDeviceProvider : MonoBehaviour, IDeviceProvider
    {
        public event Action<IDeviceProvider, BaseMessage> OnMessageReceived;

        public bool IsInitialized { get; protected set; } = false;

        public virtual Task InitializeAsync()
        {
            IsInitialized = true;
            return Task.CompletedTask;
        }

        public virtual Task RequestDevicesAsync() {
            return Task.CompletedTask;
        }

        private void OnDestroy()
        {
            IsInitialized = false;
        }
    }

}