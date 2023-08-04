using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Interfaces;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Character
{
    public class RagdollController : MonoBehaviour, IResettable
    {
        [SerializeField] private float wakeUpDelay = 3f;
        [SerializeField] private float resetBonesTime = 0.5f;
        [Space(10)] 
        [SerializeField] private Transform hipsBone;

        [SerializeField] private string standUpClipName;

        private Animator _animator;
        private Rigidbody[] _rigidbodies;

        private BoneTransform[] _standingBones;
        private BoneTransform[] _ragdollBones;
        private Transform[] _bones;
        
        
        
        [Inject] private ResetSceneButton _resetSceneButton;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbodies = GetComponentsInChildren<Rigidbody>();
            
            _bones = hipsBone.GetComponentsInChildren<Transform>();
            _standingBones = new BoneTransform[_bones.Length];
            _ragdollBones = new BoneTransform[_bones.Length];

            for (var i = 0; i < _bones.Length; i++)
            {
                _standingBones[i] = new BoneTransform();
                _ragdollBones [i] = new BoneTransform();
            }

            DisableRagdoll(); 
            
            _resetSceneButton.OnClick += Reset;
        }

        public void PushCharacter(Vector3 force, Vector3 point)
        {
            EnableRagdoll();

            var hitBone = _rigidbodies.OrderBy(rb => Vector3.Distance(rb.position, point)).First();
            hitBone.AddForceAtPosition(force, point, ForceMode.Impulse);

            StopAllCoroutines();
            StartCoroutine(WakingUp());
        }
         
        private void EnableRagdoll()
        {
            foreach (var bone in _rigidbodies)
            {
                bone.isKinematic = false;
            }
            _animator.enabled = false;
        }

        private void DisableRagdoll()
        {
            foreach (var bone in _rigidbodies)
            {
                bone.isKinematic = true;
            }
            _animator.enabled = true; 
        }

        private IEnumerator WakingUp()
        {
            yield return new WaitForSeconds(wakeUpDelay);
            
            AlignPositionToHips();
            AlignRotationToHips();
            
            PopulateBoneTransforms(_ragdollBones);
            PopulateAnimationStartBoneTransforms(standUpClipName,_standingBones);

            StartCoroutine(ResetBones());
        }

        private IEnumerator ResetBones()
        {
            var elapsedResetBonesTime = 0f;
            while (elapsedResetBonesTime < resetBonesTime)
            {
                elapsedResetBonesTime += Time.deltaTime;
                var elapsedPercentage = Mathf.Min(1f, elapsedResetBonesTime / resetBonesTime);
                    
                for (var i = 0; i < _bones.Length; i++)
                {
                    _bones[i].localPosition = Vector3.Lerp(_ragdollBones[i].Position, _standingBones[i].Position,
                        elapsedPercentage);
                    _bones[i].localRotation = Quaternion.Lerp(_ragdollBones[i].Rotation, _standingBones[i].Rotation,
                        elapsedPercentage);
                }
                
                yield return new WaitForSeconds(Time.deltaTime);
            }
            
            DisableRagdoll();
            _animator.Play(standUpClipName);
        }
        
        private void AlignPositionToHips()
        {
            var transform1 = transform;
            var originalHipsPosition = hipsBone.position;
            transform1.position = originalHipsPosition;
            
            var positionOffset = _standingBones[0].Position;
            positionOffset.y = 0;
            positionOffset = transform1.rotation * positionOffset;
            transform1.position -= positionOffset;

            if (Physics.Raycast(transform.position, Vector3.down, out var raycatHit))
            {
                var position = transform1.position;
                position = new Vector3(position.x, raycatHit.point.y, position.z);
                transform1.position = position;
            }
            hipsBone.position = originalHipsPosition;
        }

        private void AlignRotationToHips()
        {
            var originalHipsPosition = hipsBone.position;
            var originalHipsRotation = hipsBone.rotation;
            
            var desiredDirection = hipsBone.up * -1;
            desiredDirection.y = 0;
            desiredDirection.Normalize();
            
            Quaternion fromToRotation = Quaternion.FromToRotation(transform.forward, desiredDirection);
            transform.rotation *= fromToRotation;

            hipsBone.position = originalHipsPosition;
            hipsBone.rotation = originalHipsRotation;
        }
        
        private class BoneTransform
        {
            public Vector3 Position;
            public Quaternion Rotation;
        }

        private void PopulateBoneTransforms(IReadOnlyList<BoneTransform> boneTransforms)
        {
            for (var i = 0; i < boneTransforms.Count; i++)
            {
                boneTransforms[i].Position = _bones[i].localPosition;
                boneTransforms[i].Rotation = _bones[i].localRotation;
            }
        }

        private void PopulateAnimationStartBoneTransforms(string clipName, BoneTransform[] boneTransforms)
        {
            foreach (var clip in _animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == clipName)
                {
                    clip.SampleAnimation(gameObject, 0);
                    PopulateBoneTransforms(boneTransforms);
                    break;
                }
            }
        }

        public void Reset()
        {
            StopAllCoroutines();
            AlignPositionToHips();
            AlignRotationToHips();
            
            DisableRagdoll();
        }
    }
}
