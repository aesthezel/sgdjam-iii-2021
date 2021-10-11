using System;
using System.Collections;
using Code.Services;
using UnityEngine;
using DG.Tweening;

namespace Code.CameraSystem
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CameraTuner : MonoBehaviour
    {
        [SerializeField] private FollowCamera theCamera;
        [SerializeField] private bool doWorkWithFaceDirection;
        [SerializeField] private float faceDirection;
        [SerializeField] private bool doTune;
        [SerializeField] private Vector2 tuning;
        [SerializeField] private bool doOrtho;
        [SerializeField] private float orthoGraphicSize;
        [SerializeField] private float orthoTime;
        
        private BoxCollider2D _myCollider;
        private float _lastOrtoSize;
        private Camera _cam;
        private Tween _orthoTween;
        private PlayerService player;
        
        private void Awake()
        {
            _myCollider = GetComponent<BoxCollider2D>();
            _cam = theCamera.GetComponent<Camera>();
            player = ServiceLocator.Instance.ObtainService<PlayerService>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                bool req = !(doWorkWithFaceDirection && faceDirection != player.Controller.FacingDirection);

                if(doTune && req)
                    theCamera.TuneDisplacement(tuning);
                
                if (doOrtho)
                {
                    _lastOrtoSize = _cam.orthographicSize;
                    if(_orthoTween != null)
                        _orthoTween.Kill();
                    Debug.Log(orthoGraphicSize);
                    _orthoTween = _cam.DOOrthoSize(orthoGraphicSize, orthoTime);
                    _orthoTween.onComplete += () => _orthoTween = null;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if(doTune)
                    theCamera.TuneDisplacement(Vector2.zero);
                
                if (doOrtho)
                {
                    if(_orthoTween != null)
                        _orthoTween.Kill();
                    _orthoTween = _cam.DOOrthoSize(_lastOrtoSize, orthoTime);
                    _orthoTween.onComplete += () => _orthoTween = null;
                }
            }
        }
    }
}