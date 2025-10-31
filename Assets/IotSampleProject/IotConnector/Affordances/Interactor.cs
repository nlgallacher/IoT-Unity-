using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IndustryCSE.IoT
{
    /// <summary>
    /// Adds interaction capabilities to an IoT device by spawning a UI tooltip in a worldspace position.
    /// </summary>
    public class Interactor : MonoBehaviour
    {
        /// <summary>
        /// Event that is triggered when the selection changes.
        /// </summary>
        public event Action<bool> OnSelectionChange;

        [SerializeField] private Vector3 hotspotAnchorOffset = new Vector3(0,0,0);

        /// <summary>
        /// The hotspot visual gameobject
        /// </summary>
        private GameObject _hotSpot;

        /// <summary>
        /// Object selection state
        /// </summary>
        private bool _selected;

        /// <summary>
        /// Hotspot anchor
        /// </summary>
        private Vector3 _anchor;

        /// <summary>
        /// Hotspot scale factor
        /// </summary>
        private Vector3 _scaleFactor;

        private bool activate = true;

        void Start() { }

        public void SetHotspot(GameObject hotspotPrefab)
        {
            if (hotspotPrefab == null)
            {
                Debug.LogWarning($"Prefab not found");
                return;
            }

            SpawnPrefabAtLocation(hotspotPrefab, _anchor + hotspotAnchorOffset, activate);
        }

        public void SetHotspotAnchor(Vector3 anchor, bool active = false)
        {
            _anchor = anchor;
            activate = active;
        }

        /// <summary>
        /// Spawns a GameObject at the given position.
        /// </summary>
        /// <param name="prefabToSpawn">The prefab to spawn.</param>
        /// <param name="spawnPosition">The position at which to spawn the prefab.</param>
        private void SpawnPrefabAtLocation(GameObject prefabToSpawn, Vector3 spawnPosition, bool active = false)
        {
            _hotSpot = Instantiate(prefabToSpawn);
            _hotSpot.transform.SetParent(transform);
            _hotSpot.transform.position = spawnPosition;
            _hotSpot.transform.rotation = prefabToSpawn.transform.rotation;
            _hotSpot.transform.localScale = _scaleFactor;

            _hotSpot.SetActive(active);

            Selectable selectable = _hotSpot.GetComponent<Selectable>();

            if (selectable != null)
            {
                selectable.OnSelected += HandleOnPointerClick;
            }
            else
            {
                Debug.LogWarning("HotSpot prefab does not have a Selectable component.");
            }
        }

        /// <summary>
        /// Handles click events on the hotspot.
        /// </summary>
        private void HandleOnPointerClick()
        {
            _selected = !_selected;
            OnSelectionChange?.Invoke(_selected);
        }

        /// <summary>
        /// Sets the scale of the hotspot.
        /// </summary>
        /// <param name="scale">The new scale factor.</param>
        public void SetHotspotScale(Vector3 scale)
        {
            _scaleFactor = scale;

            if (_hotSpot != null)
            {
                _hotSpot.transform.localScale = scale;
            }
        }

        /// <summary>
        /// Show or hide the Hotspot
        /// </summary>
        /// <param name="isVisible"></param>
        public void SetHotspotVisibility(bool isVisible) {
            
            if (!_hotSpot)
                return;

            _hotSpot.SetActive(isVisible);
        }
    }
}

