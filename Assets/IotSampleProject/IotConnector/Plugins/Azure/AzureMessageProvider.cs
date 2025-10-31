using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Azure.Messaging.EventHubs.Consumer;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;
using System.Text;

namespace IndustryCSE.IoT
{
    public class AzureMessageProvider : BaseMessageProvider
    {
        // Replace with your event hub endpoint - copy the buildin connection endpoint
        [SerializeField] private string _eventHubEndpoint = "Endpoint=sb://<endpointNamespace>.servicebus.windows.net/;SharedAccessKeyName=<SharedAccessPolicyName>;SharedAccessKey=<SharedAccessKey>;EntityPath=<entry-path>";

        private EventHubConsumerClient _eventHubConsumerClient;

        private CancellationTokenSource _cancellationTokenSource;

        public override Task InitializeAsync()
        {
            if (_simulateEvents)
            {
                _deviceSimulator.OnDeviceMessage += ReadSimulatedMessage;
                return Task.CompletedTask;
            }

            Debug.Log("AzureMessageProvider - InitializeAsync");

            _eventHubConsumerClient = new EventHubConsumerClient(EventHubConsumerClient.DefaultConsumerGroupName, _eventHubEndpoint, string.Empty);
            _cancellationTokenSource = new CancellationTokenSource();

            ReadMessages();

            return Task.CompletedTask;
        }

        public override Task ReadMessagesAsync()
        {

            Debug.Log("ReadMessagesAsync Async");

            ReadMessages();

            return Task.CompletedTask;
        }

        

        private async void ReadMessages()
        {
            bool isCancelled = false;
            int maxRetries = 100;
            uint nextMessageIndex = 0;

            if (_simulateEvents)
            {
                return;
            }
            // Debug.Log("AzureMessageProvider - Read Messages");

            while (!isCancelled && maxRetries-- > 0)
            {
                try
                {

                    // Begin reading events for all partitions, starting with the first event in each partition and waiting indefinitely for
                    // events to become available. Reading can be canceled by breaking out of the loop when an event is processed or by
                    // signaling the cancellation token.
                    //
                    // The "ReadEventsAsync" method on the consumer is a good starting point for consuming events for prototypes
                    // and samples. For real-world production scenarios, it is strongly recommended that you consider using the
                    // "EventProcessorClient" from the "Azure.Messaging.EventHubs.Processor" package.

                    await foreach (PartitionEvent partitionEvent in _eventHubConsumerClient.ReadEventsAsync(
                                       _cancellationTokenSource.Token))
                    {
  
                        if (_cancellationTokenSource.Token.IsCancellationRequested)
                        {
                            isCancelled = true;
                            break;
                        }

                        if (!IsPaused)
                        {
                            Debug.Log("IsPaused " + IsPaused);
                            
                            DeviceMessage msg = CreateDeviceMessage(nextMessageIndex, partitionEvent);
                            if (msg != null)
                            {
                                TimeSpan dt = DateTimeOffset.UtcNow - partitionEvent.Data.EnqueuedTime;
                                //if (dt.Milliseconds < IgnoreMessagesOlderThanMilliseconds)
                                {
                                    MessageReceived(msg);
                                }
                                ++nextMessageIndex;
                            }
                        }
                    }


                }
                catch (TaskCanceledException)
                {
                    // This is expected when the token is signaled; it should not be considered an
                    // error in this scenario.
                    isCancelled = true;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        protected DeviceMessage CreateDeviceMessage(uint messageIndex, PartitionEvent partitionEvent)
        {
            DeviceMessage msg = null;
            string json = string.Empty;

            try
            {
                JToken deviceId = null;
                json = Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray());

                JObject bodyObj = JsonConvert.DeserializeObject<JObject>(json);
                bodyObj.TryGetValue("deviceId", out deviceId);

                msg = new DeviceMessage(json);
                msg.DeviceId = deviceId.ToString();
               
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                Debug.LogWarning($"{partitionEvent.Data.EnqueuedTime} -- JSON: '{json}'");
            }

            return msg;
        }

        private void ReadSimulatedMessage(string message)
        {
            if (!IsPaused)
            {
                Debug.Log("Simulate Message Provider - Read Messages");

                DeviceMessage msg = null;

                try
                {
                    JToken deviceId = null;

                    JObject bodyObj = JsonConvert.DeserializeObject<JObject>(message);
                    bodyObj.TryGetValue("deviceId", out deviceId);

                    msg = new DeviceMessage(message);
                    msg.DeviceId = deviceId.ToString();

                }
                catch (Exception e)
                {
                    Debug.LogWarning(e);
                    Debug.LogWarning($" -- JSON: '{message}'");
                };
                if (msg != null)
                {
                    MessageReceived(msg);
                }
            }
        }

    }
}

