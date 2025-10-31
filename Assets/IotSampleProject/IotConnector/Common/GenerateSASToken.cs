using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace IndustryCSE.IoT
{
    public class GenerateSASToken
    {
        public static string GenerateSharedAccessSignature(string iotHubName, string key, string policyName, int expiryInSeconds = 36000)
        {
            TimeSpan sinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
            string expiry = Convert.ToString((int)sinceEpoch.TotalSeconds + expiryInSeconds);
            string stringToSign = Uri.EscapeDataString(iotHubName) + "\n" + expiry;

            using (var hmac = new HMACSHA256(Convert.FromBase64String(key)))
            {
                string signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
                string sasToken = $"SharedAccessSignature sr={Uri.EscapeDataString(iotHubName)}&sig={Uri.EscapeDataString(signature)}&se={expiry}&skn={policyName}";
                return sasToken;
            }
        }
    }
}
