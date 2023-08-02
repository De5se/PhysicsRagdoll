using Cinemachine;
using UnityEngine;
using Zenject;

namespace _Scripts.Managers.CameraManagement
{
    public class CameraManager : MonoBehaviour
    {
        #region Variables
        [Inject] private GameStateController _gameStateController;
        [SerializeField] private CinemachineVirtualCamera[] cameras;
        [SerializeField] private CameraType currentCameraType;
        [Space(10)] 
        [SerializeField] private float intensity = 1f;
        [SerializeField] private float shakeDuration = 0.1f;

        private CinemachineVirtualCamera _currentCamera;
        private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

        private float _timer;

        private CinemachineVirtualCamera CurrentCamera
        {
            get => _currentCamera;
            set
            {
                _currentCamera = value;
                _cinemachineBasicMultiChannelPerlin =
                    _currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }
        }
        #endregion

        private void Start()
        {
            _gameStateController.OnGameStartedEvent += () =>
            {
                ChangeCamera(CameraType.Camera2);
            };
        }

        private void Update()
        {
            if (_timer <= 0) return;
            
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                StopShaking();
            }
        }

        #region Change Camera
        public void ChangeCamera(CameraType cameraType)
        {
            currentCameraType = cameraType;

            foreach (var virtualCamera in cameras)
            {
                virtualCamera.m_Priority = 0;
            }
            CurrentCamera = cameras[(int)currentCameraType];
            CurrentCamera.m_Priority = 100;
        }

        public void SetCamera(CameraType cameraType, CinemachineVirtualCamera targetCamera)
        {
            targetCamera.m_Priority = cameras[(int) cameraType].m_Priority;
            cameras[(int) cameraType].m_Priority = 0;
            cameras[(int) cameraType] = targetCamera;

            if (currentCameraType == cameraType)
            {
                CurrentCamera = targetCamera;
            }
        }
        #endregion`
        
        private void ShakeCamera()
        {
            SetAmplitudeIntensity(intensity);
            _timer = shakeDuration;
        }

        private void StopShaking() => SetAmplitudeIntensity(0f);

        private void SetAmplitudeIntensity(float targetIntensity) =>
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = targetIntensity;
    }
}