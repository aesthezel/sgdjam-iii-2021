using Code.Interfaces;
using UnityEngine;
using Cinemachine;

namespace Code.Services
{
    public class MainCameraService : MonoBehaviour, IService
    {
        public CinemachineVirtualCamera MainCamera { get; private set; }
        public CinemachineImpulseSource ImpulseSource { get; private set; }

        private void Awake()
        {
            MainCamera = GetComponent<CinemachineVirtualCamera>();
            ImpulseSource = GetComponent<CinemachineImpulseSource>();
        }

        private void Start()
        {
            var player = ServiceLocator.Instance.ObtainService<PlayerService>();
            MainCamera.m_Follow = player.transform;
        }
    }
}