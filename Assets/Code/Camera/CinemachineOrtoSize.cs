using System.Collections;
using Cinemachine;
using UnityEngine;

class CinemachineOrtoSize: MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField] private float changeValue;
    [SerializeField] private float transitionTime = 0.2f;
    private float _lastValue;
    private int _runningCorDirection;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("TransparentToEnemies"))
        {
            if (_runningCorDirection == 1) return;
            
            if(_runningCorDirection == -1)
                StopAllCoroutines();
            else
                _lastValue = camera.m_Lens.OrthographicSize;
            
            StartCoroutine(DoOrthoSize(camera.m_Lens.OrthographicSize, changeValue, transitionTime));
            _runningCorDirection = 1;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("TransparentToEnemies"))
        {
            if (_runningCorDirection == -1) return;
            if(_runningCorDirection == 1) StopAllCoroutines();
            
            StartCoroutine(DoOrthoSize(camera.m_Lens.OrthographicSize, _lastValue, transitionTime));
            _runningCorDirection = -1;
        }
    }

    private IEnumerator DoOrthoSize(float start, float end, float time)
    {
        var t = 0f;
        
        while (t < time)
        {
            camera.m_Lens.OrthographicSize = start + (end - start) * (t / time);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        camera.m_Lens.OrthographicSize = end;
        _runningCorDirection = 0;
    }
}