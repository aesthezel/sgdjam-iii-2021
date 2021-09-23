using System.Collections;
using Code.Hero;
using UnityEngine;
using Cinemachine;

namespace Code.Camera
{
    public class CameraEffects : MonoBehaviour
    {
        [SerializeField] private InputMapper mapper;
        
        private CinemachineVirtualCamera _cCam;
        private CinemachineImpulseSource _impulseSource;
        
        private void Awake()
        {
            _cCam = transform.parent.GetComponent<CinemachineVirtualCamera>();
            _impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        //-----------------
        // EFFECT INVOKERS
        //-----------------
        public Coroutine DoOrtoSize(float finalValue, float animTime, Ease easeType)
        {
            return StartCoroutine(OrtoSizeCoroutine(finalValue, animTime, easeType));
        }

        public Coroutine DoShake(float strength, float duration)
        {
            return StartCoroutine(ShakeCoroutine(strength, duration));
        }
        
        //----------------------
        // COROUTINES - EFFECTS
        //----------------------
        private IEnumerator OrtoSizeCoroutine(float finalValue, float animTime, Ease easeType)
        {
            var cTime = 0f;
            var startVal = _cCam.m_Lens.OrthographicSize;
            var changeVal = finalValue - startVal;

            while (cTime < animTime)
            {
                _cCam.m_Lens.OrthographicSize = Easing.MakeEase(easeType, cTime, startVal, changeVal, animTime);
                cTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            _cCam.m_Lens.OrthographicSize = Easing.MakeEase(easeType, animTime, startVal, changeVal, animTime);
        }

        private IEnumerator ShakeCoroutine(float strength, float duration)
        {
            var elapsedTime = 0f;
            
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                _impulseSource.GenerateImpulse(strength);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}