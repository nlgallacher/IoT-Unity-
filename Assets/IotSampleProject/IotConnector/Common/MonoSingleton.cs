using UnityEngine;

namespace IndustryCSE.IoT
{
    /// <summary>
    /// MonoBehaviour singleton base class
    /// </summary>
    /// <typeparam name="TSelf">Self type</typeparam>
    public abstract class MonoSingleton<TSelf> : MonoBehaviour where TSelf : MonoSingleton<TSelf>
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        public static TSelf Instance { get; private set; }
        // ReSharper disable once StaticMemberInGenericType
        private static bool IsInitialized;
        [SerializeField]
        protected bool _dontDestroyOnLoad = true;

        /// <summary>
        /// <b>Only override Awake if you are sure you need to. <see cref="InternalInit"/> should cover most use cases.</b>
        /// </summary>
        protected virtual void Awake()
        {
            // If no other MonoBehaviour request the instance in an awake function
            // executing before this one, no need to search the object.
            if (!Instance)
            {
                Instance = (TSelf)this;
            }
            else
            {
                Debug.LogError($"Another instance of {GetType().Name} already exist! Destroying self...");
                DestroyImmediate(this);
                return;
            }

            Init();
            if (_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        /// <summary>
        /// <b>Only override OnDestroy if you are sure you need to. <see cref="OnInstanceDestroyed"/> should cover most use cases.</b>
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (Instance != this)
            {
                return;
            }

            OnInstanceDestroyed();
        }

        /// <summary>
        /// Initializes the singleton, should only be called once
        /// </summary>
        /// ReSharper disable once MemberCanBePrivate.Global
        protected void Init()
        {
            if (IsInitialized)
            {
                return;
            }

            InternalInit();
            IsInitialized = true;
        }

        /// <summary>
        /// This function is called when the instance is used the first time<br/>
        /// Put all the initializations you need here, as you would do in Awake
        /// </summary>
        protected virtual void InternalInit()
        {
            // This is exclusively for overriding classes. Any default behaviour should go in Init()
        }

        /// <summary>
        /// This function is called when the instance is destroyed or disposed of.<br/>
        /// Put any necessary cleanup in here as you would in OnDestroy
        /// </summary>
        protected virtual void OnInstanceDestroyed()
        {
        }
    }

}
