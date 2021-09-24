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

        public Coroutine DoLookAhead(float value, int direction, float sustainTime, float inDuration, float outDuration)
        {
            return StartCoroutine(LookAheadCoroutine(value, direction, sustainTime, inDuration, outDuration));
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
        
        private IEnumerator LookAheadCoroutine(float value, int direction, float sustainTime, float inDuration, float outDuration)
        {
            var dir = Mathf.Sign(direction);
            var elapsedTime = 0f;
            var initLocalPos = _cCam.m_Follow.localPosition;
            
            // Move to target pos
            while (elapsedTime < inDuration)
            {
                elapsedTime += Time.deltaTime;
                
                // Compute the new position on each frame
                var newPos = _cCam.m_Follow.localPosition;
                var x = Easing.MakeEase(
                    Ease.OutQuad, 
                    elapsedTime, 
                    initLocalPos.x, 
                    value * dir, 
                    inDuration);
                newPos.x = x;
                
                // Update target object position
                _cCam.m_Follow.localPosition = newPos;
                yield return new WaitForEndOfFrame();
            }
            
            // Set the exact final value
            _cCam.m_Follow.localPosition = initLocalPos + (Vector3.right * value * dir);
            
            // Wait for dash (or anything) to finish
            yield return new WaitForSeconds(sustainTime);
            
            // Reset to normal value
            elapsedTime = 0;
            var lastPos = _cCam.m_Follow.localPosition;
            
            while (elapsedTime < outDuration)
            {
                elapsedTime += Time.deltaTime;
                
                // Compute the new position on each frame
                var newPos = _cCam.m_Follow.localPosition;
                
                var x = Easing.MakeEase(
                    Ease.OutQuad, 
                    elapsedTime,
                    lastPos.x,
                    -value * dir, 
                    outDuration);
                newPos.x = x;
                
                // Update target object position
                _cCam.m_Follow.localPosition = newPos;
                yield return new WaitForEndOfFrame();
            }
            // Set the exact final value
            _cCam.m_Follow.localPosition = initLocalPos;
        }
    }
}