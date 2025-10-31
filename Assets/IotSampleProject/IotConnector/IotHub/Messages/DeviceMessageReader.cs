using System.Collections;
using System.Collections.Generic;
using IndustryCSE.IoT.Messenger;
using UnityEngine;

namespace IndustryCSE.IoT
{
    public class DeviceMessageReader : MonoSingleton<DeviceMessageReader>
    {
        public IMessageBus MessageBus = new MessageBus();

        protected IMessageProvider _messageProvider = null;

        protected override void InternalInit()
        {
        }

        public bool IsPaused()
        {
            if (_messageProvider != null)
                    return _messageProvider.IsPaused;
            
            return false;
        }

        public async void ReadDeviceMessages()
        {
            if (_messageProvider != null)
            {
                if (!_messageProvider.IsInitialized)
                {
                    _messageProvider.OnMessageReceived += OnMessageReceived;
                    await _messageProvider.InitializeAsync();
                }
                else
                {
                    await _messageProvider.ReadMessagesAsync();
                }
            }
        }

        private void OnMessageReceived(object sender, BaseMessage msg)
        {
            if (_messageProvider != null)
            {
                IMessageProvider provider = sender as IMessageProvider;
                if (provider != null && !provider.IsPaused)
                {
                    MessageBus.Publish(provider, msg);
                }
            }
        }
    }
}
