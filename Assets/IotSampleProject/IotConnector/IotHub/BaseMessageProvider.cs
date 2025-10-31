using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace IndustryCSE.IoT
{
    public class BaseMessageProvider : MonoBehaviour, IMessageProvider
    {
        public event Action<IMessageProvider, BaseMessage> OnMessageReceived;

        public bool IsInitialized { get; protected set; } = false;

        [SerializeField] protected DeviceSimulator _deviceSimulator;
        [SerializeField] private bool _isPaused;

        // Simulate endpoint messages
        [SerializeField] protected bool _simulateEvents = false;

        public bool IsPaused
        {
            get { return _isPaused; }
            protected set { _isPaused = value; }
        }

        public virtual Task InitializeAsync()
        {
            IsInitialized = true;
            return Task.CompletedTask;
        }

        public virtual Task SetModeAsync(bool useSimulatedEvents)
        {
            _simulateEvents = useSimulatedEvents;
 
            return Task.CompletedTask;
        }

        public virtual Task ReadMessagesAsync()
        {
            return Task.CompletedTask;
        }

        public virtual void Pause()
        {
            IsPaused = true;
        }

        public virtual void Resume()
        {
            IsPaused = false;
        }

        protected void MessageReceived(BaseMessage msg)
        {
            OnMessageReceived?.Invoke(this, msg);
        }
    }
}

