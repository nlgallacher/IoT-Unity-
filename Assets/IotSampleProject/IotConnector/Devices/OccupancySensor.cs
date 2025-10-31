using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;

namespace IndustryCSE.IoT
{
    public class OccupancySensor : BaseDevice
    {
        // Names of materials to find at runtime
        [SerializeField] string freeMaterialName = "FreeMaterial";
        [SerializeField] string occupiedMaterialName = "OccupiedMaterial";

        // Materials at runtime
        private Material freeMaterial;
        private Material occupiedMaterial;

        // Optionally store the original materials for restoration
        private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();

        private bool showOccupancy = false;
        private bool occupied = true;

        public bool IsOccupied
        {
            get { return occupied; }
            set { occupied = value; }
        }

        void Awake()
        {
            _contextWindowLabel = "EnhancedContextWindow - Occupancy";

            //ConfigureInteractor(CalculateAnchor(), new Vector3(0.2f,0.2f, 0.2f));

            _deviceType = IoT.DeviceType.Type.Occupancy;

            OnDeviceMessage += OnDeviceMessageHandler;
        }

        void Start() {
            base.SubscribeMessageBus();
        }

        protected override void ConfigureInteractor(Vector3 anchor, Vector3 scale, bool active = false)
        {
            base.ConfigureInteractor(anchor, scale, active);

            contextUI = FindFilteredObject<EnhancedContextWindow>(_contextWindowLabel);

            // Load the free and occupied materials at runtime
            LoadMaterials();
            // Save the original materials of the GameObject and its children
            SaveOriginalMaterials();
        }

        /// <summary>
        /// Load occupancy materials from the Resources folder
        /// </summary>
        private void LoadMaterials()
        {
            freeMaterial = Resources.Load<Material>(freeMaterialName);
            occupiedMaterial = Resources.Load<Material>(occupiedMaterialName);

            // Safeguarding in case materials are not found
            if (freeMaterial == null)
            {
                Debug.LogError($"Free Material '{freeMaterialName}' not found. Please make sure it exists in the Resources folder.");
            }

            if (occupiedMaterial == null)
            {
                Debug.LogError($"Occupied Material '{occupiedMaterialName}' not found. Please make sure it exists in the Resources folder.");
            }
        }

        /// <summary>
        ///  Store the original materials array of each renderer
        /// </summary>
        private void SaveOriginalMaterials()
        {
            originalMaterials.Clear(); // Clear the dictionary first
            Renderer[] renderers = GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                originalMaterials[renderer] = renderer.materials;
            }
        }

        /// <summary>
        /// // Apply the new material to the GameObject and all its children
        /// </summary>
        /// <param name="newMaterial"></param>
        public void SetMaterial(Material newMaterial)
        {
            if (newMaterial == null)
            {
                Debug.LogError("The new material is null. Please assign a valid material.");
                return;
            }

            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.materials;

                // Replace all materials in the array with the new material
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = newMaterial;
                }

                renderer.materials = materials;
            }
        }
        /// <summary>
        /// Restore the original materials for all renderers
        /// </summary>
        public void RestoreOriginalMaterials()
        {
            foreach (var entry in originalMaterials)
            {
                Renderer renderer = entry.Key;
                if (renderer != null)
                {
                    renderer.materials = entry.Value;
                }
            }
        }

        private void OnDeviceMessageHandler(string message) 
        {
            lastMessage = message;

            // Update the occupancy status
            try {
                JObject bodyObj = JsonConvert.DeserializeObject<JObject>(message);
                JToken occupancyVal = null;
                bodyObj.TryGetValue("occupancyStatus", out occupancyVal);

                if (occupancyVal.ToString().Equals("occupied")) {
                    occupied = true;
                }
                else {
                    occupied = false;
                }

                SetOccupancyState(occupied);

            }
            catch (Exception ex) {
                Debug.LogWarning(ex.ToString());
            }

            // Expose message in context ui
            if (isFocused) {
                contextUI.ClearContext();
                ExposeMessage(message);
            }
        }

        /// <summary>
        /// Show or hide the Occupancy Outline
        /// </summary>
        /// <param name="active"></param>
        public override void SetVisibility(bool active)
        {
            showOccupancy = active;
            _tangibleInteractor.SetHotspotVisibility(active);

            if (showOccupancy) {
                SetOccupancyState(occupied);
            }
            else {
                RestoreOriginalMaterials();
                
                if (contextUI != null)
                    contextUI.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Change the occupancy state for this device
        /// </summary>
        /// <param name="active"></param>
        private void SetOccupancyState(bool occupied)
        {
            if (!showOccupancy)
                return;

            switch (occupied)
            {
                case false:
                    SetMaterial(freeMaterial);

                    break;
                default:
                    SetMaterial(occupiedMaterial);
                    break;
            }
        }
    }
}

