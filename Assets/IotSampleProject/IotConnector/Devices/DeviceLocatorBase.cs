using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndustryCSE.IoT
{
    public class DeviceLocatorBase : ScriptableObject
    {
        [SerializeField] DeviceType.Type _deviceType;

        [SerializeField] protected GameObject hotspotPrefab;

        public DeviceType.Type DeviceType
        {
            get { return _deviceType; }
            set { _deviceType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual List<GameObject> LocateDevices(GameObject root) {
            throw new NotImplementedException();
        }

    }
}

