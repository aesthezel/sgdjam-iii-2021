using UnityEngine;
using Cinemachine;

public class CinemachineTransposeOffset : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField] private Vector2 changeValue;
    private CinemachineFramingTransposer _transposer;
    private Vector2 _lastValue;
    
    private void Awake()
    {
        _transposer = camera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("TransparentToEnemies"))
        {
            _lastValue = _transposer.m_TrackedObjectOffset;
            _transposer.m_TrackedObjectOffset = changeValue;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("TransparentToEnemies"))
            _transposer.m_TrackedObjectOffset = _lastValue;
    }
}
