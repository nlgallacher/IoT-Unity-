using System;
using UnityEngine;

namespace IndustryCSE.IoT.Messenger
{
	/// <summary>
	/// Maintains a set of listeners for a specific message type, and provides the base message type (can be null) for this type
	/// </summary>
	internal class ListenersForMessageType
	{
		private Delegate _listeners;

		public Type MessageType { get; }
		
		public Type MessageBaseType { get; }

		/// <summary>
		/// Construct a set of listeners for a specific message type
		/// </summary
		public ListenersForMessageType(Type messageType)
		{
			MessageType = MessengerCheck.IsMessageType(messageType);

			if (MessageType.IsInterface)
			{
				MessageBaseType = GetBaseInterfaceType(MessageType);
			}
			else
			{

				if (typeof(IMessage).IsAssignableFrom(MessageType.BaseType))
					MessageBaseType = MessageType.BaseType;
				else
					MessageBaseType = GetBaseInterfaceType(MessageType);
			}
		}

		private Type GetBaseInterfaceType(Type type)
		{
			Type[] baseInterfaces = type.GetInterfaces();
			bool hasIMessage = false;

			foreach (Type iType in baseInterfaces)
			{
				if (iType != typeof(IMessage) && typeof(IMessage).IsAssignableFrom(iType))
				{
					return iType;
				}
				else if (iType == typeof(IMessage))
				{
					hasIMessage = true;
				}
			}
			
			if (hasIMessage)
				return typeof(IMessage);
			else
				return null;
		}

		/// <summary>
		/// Add a listener delegate to this message type
		/// </summary>
		public void Add(Delegate listener)
		{
			if (listener == null)
			{
				throw new ArgumentNullException(nameof(listener));
			}

			foreach (Delegate existingListener in _listeners?.GetInvocationList() ?? Array.Empty<Delegate>())
			{
				if (existingListener == listener)
				{
					throw new ArgumentException($"Listener is already registered.", nameof(listener));
				}
			}
			_listeners = Delegate.Combine(_listeners, listener);
		}

		/// <summary>
		/// Remove a listener delegate from this message type
		/// </summary>
		public bool Remove(Delegate listener)
		{
			int listenerCount = _listeners?.GetInvocationList().Length ?? 0;
			_listeners = Delegate.Remove(_listeners, listener);
			return (_listeners?.GetInvocationList().Length ?? 0) < listenerCount;
		}

		/// <summary>
		/// Publish a message from a given publisher to all listeners of this message type
		/// </summary>
		public void Publish(IMessagePublisher publisher, IMessage message)
		{
			foreach (Delegate listener in _listeners?.GetInvocationList() ?? Array.Empty<Delegate>())
			{
				try
				{
					listener.DynamicInvoke(publisher, message);
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
			}
		}

		public override string ToString()
		{
			int listenerCount = _listeners?.GetInvocationList().Length ?? 0;
			string listenersText = listenerCount == 1 ? "listener" : "listeners";
			return $"Listeners[{MessageType.Name}] ({listenerCount} {listenersText})";
		}
	}
}
