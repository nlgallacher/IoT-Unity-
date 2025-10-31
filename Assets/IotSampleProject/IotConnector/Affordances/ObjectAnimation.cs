using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndustryCSE.IoT
{
    public class ObjectAnimation : MonoBehaviour
    {
        [SerializeField] private List<Animator> _animators = new List<Animator>();

        private void Awake()
        {
            if (_animators == null)
            {
                _animators = new List<Animator>();
            }
        }

        public void LocatedAnimators(Transform parent)
        {
             if (_animators == null)
            {
                _animators = new List<Animator>();
            }

            _animators.Clear();
            CollectAllChildAnimators(parent);
        }

        private void CollectAllChildAnimators(Transform parent)
        {
            // Iterate over each child of the current parent
            foreach (Transform child in parent)
            {
                // Check if the child has an Animator component and add it to the list if it does
                Animator animator = child.GetComponent<Animator>();
                if (animator != null)
                {
                    _animators.Add(animator);
                }

                // Recursively call the method on the child to check its children
                CollectAllChildAnimators(child);
            }
        }


        public void Animate(string animationTrigger)
        {

            foreach (Animator animator in _animators)
            {
                animator.SetTrigger(animationTrigger);
            }
        }
    }
}

