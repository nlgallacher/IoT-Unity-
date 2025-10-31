using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace IndustryCSE.IoT
{
    public class AzureDeviceProvider : BaseDeviceProvider
    {
        // Replace with your IoT Hub's HostName, SharedAccessKeyName, and SharedAccessKey
        [SerializeField] private string _iotHubHostName = "<YourIoTHub>.azure-devices.net";
        [SerializeField] private string _sharedAccessPolicyName = "<YourSharedAccessPolicyName>";
        [SerializeField] private string _sharedAccessPolicyKey = "<YourSharedAccessPolicyKey>";

        public override Task InitializeAsync()
        {
            Debug.Log("AzureDeviceProvider - InitializeAsync");
            StartCoroutine(RequestDevices());
            return Task.CompletedTask;
        }

        public override Task RequestDevicesAsync()
        {

            Debug.Log("RequestDevices Async");
            StartCoroutine(RequestDevices());
            return Task.CompletedTask;
        }

        private IEnumerator RequestDevices()
        {
            string sasToken = GenerateSASToken.GenerateSharedAccessSignature(_iotHubHostName, _sharedAccessPolicyKey, _sharedAccessPolicyName);
            Debug.Log("Generated SAS Token: " + sasToken);

            string url = $"https://{_iotHubHostName}/devices?api-version=2020-03-13";

            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("Authorization", sasToken);
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("Response: " + request.downloadHandler.text);

                // TODO: Emit message 
            }
        }
    }
}

