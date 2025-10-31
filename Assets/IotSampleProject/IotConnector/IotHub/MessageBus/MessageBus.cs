using System;
using System.Collections.Generic;

namespace IndustryCSE.IoT.Messenger
{
	/// <summary>
	/// Message Bus implementation
	/// </summary>
	public class MessageBus : IMessageBus
	{
		private readonly Dictionary<Type, ListenersForMessageType> _listeners = new();

		public void Subscribe<T>(MessageListener<T> listener)
			where T : IMessage
		{
			ListenersForMessageType listenersForMessageType = GetListeners(typeof(T));
			listenersForMessageType.Add(listener);
		}


		public void Unsubscribe<T>(MessageListener<T> listener) where T : IMessage
		{
			ListenersForMessageType listenersForMessageType = GetListeners(typeof(T));
			listenersForMessageType.Remove(listener);
		}

		public void Publish(IMessagePublisher publisher, IMessage message)
		{
			Type publishToMessageType = MessengerCheck.IsMessageType(message?.GetType());
			
			// Publish this message to listeners
			while (publishToMessageType != null)
			{
				ListenersForMessageType listeners = GetListeners(publishToMessageType);
				listeners.Publish(publisher, message);
				publishToMessageType = listeners.MessageBaseType;
			}
		}

		/// <summary>
		/// Get the listeners for a specific message type
		/// </summary>
		private ListenersForMessageType GetListeners(Type messageType)
		{
			MessengerCheck.IsMessageType(messageType);

			if (!_listeners.TryGetValue(messageType, out ListenersForMessageType listenersForMessageType))
			{
				listenersForMessageType = _listeners[messageType] = new ListenersForMessageType(messageType);
			}
			return listenersForMessageType;
		}
	}
}
