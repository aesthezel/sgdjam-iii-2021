using System;
using System.Collections;
using Code.Services;
using UnityEngine;
using DG.Tweening;

namespace Code.Utils.Events
{
    public class CameraTargetChange : MonoBehaviour
    {
        [SerializeField] private Transform target;
        private MainCameraService _cCam;
        private Transform _lastTarget;
        private Vector3 _transformPaso;
        private Tween runningTween;
        
        private void Start()
        {
            _cCam = ServiceLocator.Instance.ObtainService<MainCameraService>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var targetPos = target.position;
                _lastTarget = other.transform;
                target.position = other.transform.position;
                _cCam.MainCamera.m_Follow = target;
                runningTween = target.DOMove(targetPos, 2f);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                runningTween.Kill();
                runningTween = target.DOMove(_lastTarget.position, 1f);
                runningTween.onComplete += () => _cCam.MainCamera.m_Follow = _lastTarget;
            }
        }
    }
}