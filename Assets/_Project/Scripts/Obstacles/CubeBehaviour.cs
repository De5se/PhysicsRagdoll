using System;
using _Project.Scripts.Interfaces;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Obstacles
{
    public class CubeBehaviour : MonoBehaviour, IResettable
    {
        [SerializeField] private Rigidbody rb;

        private Vector3 _startPosition;
        private Quaternion _startRotation;
        
        [Inject] private ResetSceneButton _resetSceneButton;

        private void Start()
        {
            _startPosition = transform.position;
            _startRotation = transform.rotation;
            
            _resetSceneButton.OnClick += Reset;
        }

        public void Reset()
        {
            rb.isKinematic = true;
            transform.DOMove(_startPosition, _resetSceneButton.ResetDuration).SetEase(Ease.Linear);
            transform.DORotateQuaternion(_startRotation, _resetSceneButton.ResetDuration).SetEase(Ease.Linear).OnComplete(
                () =>
                {
                    rb.isKinematic = false;
                });
        }
    }
}