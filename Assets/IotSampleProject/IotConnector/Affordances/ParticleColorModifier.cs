using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndustryCSE.IoT
{
    /// <summary>
    /// Simple script to locate particle system in child objects and
    /// to modify particle color based on a temperature value
    /// </summary>
    public class ParticleColorModifier : MonoBehaviour
    {
        /// <summary>
        /// Contains the located particle systems
        /// </summary>
        private List<ParticleSystem> _particleSystems = new List<ParticleSystem>();

        /// <summary>
        /// Collect all child particle systems
        /// </summary>
        /// <param name="parent"></param>
        public void LocatedParticleSystems(Transform parent)
        {
            if (_particleSystems == null)
            {
                _particleSystems = new List<ParticleSystem>();
            }

            _particleSystems.Clear();

            CollectParticleSystems(parent);
        }
        
        // <summary>
        /// Locate all child particle systems and update materials
        /// </summary>
        /// <param name="parent"></param>
        private void CollectParticleSystems(Transform parent)
        {
            // Iterate over each child of the current parent
            foreach (Transform child in parent)
            {
                // Check if the child has an Animator component and add it to the list if it does
                ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    // Get the Renderer component from the Particle System
                    Renderer renderer = particleSystem.GetComponent<Renderer>();

                    // Create a new material instance to avoid changing the material for all Particle Systems
                    Material particleMaterial = new Material(renderer.sharedMaterial);
                    renderer.material = particleMaterial;

                    _particleSystems.Add(particleSystem);
                }

                // Recursively call the method on the child to check its children
                CollectParticleSystems(child);
            }
        }

        public void UpdateParticleColor(float value, UnityEngine.Color minColor, UnityEngine.Color maxColor, float minValue = 0f, float maxValue = 1f)
        {
            // Clamp the input value to ensure we don't exceed the defined min/max thresholds
            float clampedValue = Mathf.Clamp(value, minValue, maxValue);

            // Map the clampedValue to a normalized range based on totalSensors
            float normalizedRate = Mathf.Clamp01((clampedValue - minValue) / (maxValue - minValue));

            // Interpolate between green (low occupancy) and red (high occupancy) colors
            UnityEngine.Color interpolatedColor = UnityEngine.Color.Lerp(minColor, maxColor, normalizedRate);


            // Set alpha to semi-transparent
            interpolatedColor.a = 0.7f;

            foreach (ParticleSystem particleSystem in _particleSystems) {

                // Get the Renderer component from the Particle System
                Renderer renderer = particleSystem.GetComponent<Renderer>();
                // Apply the calculated color to the material
                renderer.material.color = interpolatedColor;
            }  
        }
    }
}

