using UnityEngine;

namespace IndustryCSE.IoT 
{
    public class IotDeviceMessageReader : DeviceMessageReader
    {
        [SerializeField] private BaseMessageProvider _actualProvider;

        protected override void InternalInit()
        {
            _messageProvider = _actualProvider;
        }

        // Start is called before the first frame update
        void Start()
        {
            ReadDeviceMessages();
        }
    }
}

