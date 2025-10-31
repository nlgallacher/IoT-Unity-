
namespace IndustryCSE.IoT.Messenger
{

	public delegate void MessageListener<T>(IMessagePublisher publisher, T message)
		where T : IMessage;

	/// <summary>
	/// Interface for a class-based event message bus system
	/// </summary>
	public interface IMessageBus
	{
		/// <summary>
		/// Subscribe a callback method as a listener for events
		/// </summary>
		void Subscribe<T>(MessageListener<T> listener)
			where T : IMessage;

		/// <summary>
		/// Unsubscribe a callback method as a listener for events
		/// </summary>
		void Unsubscribe<T>(MessageListener<T> listener)
			where T : IMessage;

		/// <summary>
		/// Publish an event
		/// </summary>
		/// <param name="publisher">Object which triggered the event</param>
		/// <param name="message">Message instance</param>
		void Publish(IMessagePublisher publisher, IMessage message);
	}
}
