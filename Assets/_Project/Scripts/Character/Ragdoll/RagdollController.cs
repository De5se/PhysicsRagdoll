using System;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Character
{
    public class RagdollController : MonoBehaviour
    {
        private enum RagdollState
        {
            Idle,
            Ragdoll
        }

        private RagdollState _currentState;
        
        private Animator _animator;
        private Rigidbody[] _bones;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _bones = GetComponentsInChildren<Rigidbody>();
            
            DisableRagdoll();
        }

        public void PushCharacter(Vector3 force, Vector3 point)
        {
            EnableRagdoll();

            var hitBone = _bones.OrderBy(rigidbody => Vector3.Distance(rigidbody.position, point)).First();
            hitBone.AddForceAtPosition(force, point, ForceMode.Impulse);
        }
         
        private void EnableRagdoll()
        {
            foreach (var bone in _bones)
            {
                bone.isKinematic = false;
            }

            _animator.enabled = false;
        }

        private void DisableRagdoll()
        {
            foreach (var bone in _bones)
            {
                bone.isKinematic = true;
            }
            _animator.enabled = true; 
        }
    }
    
    
    
}
