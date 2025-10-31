using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace IndustryCSE.IoT
{
    public interface IMessageProvider: Messenger.IMessagePublisher
    {
        public event Action<IMessageProvider, BaseMessage> OnMessageReceived;

        public bool IsInitialized { get; }

        public bool IsPaused { get; }

        public Task InitializeAsync();

        public Task ReadMessagesAsync();
    }
}

