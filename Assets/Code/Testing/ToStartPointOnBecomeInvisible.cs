using UnityEngine;

namespace Code.Testing
{
    public class ToStartPointOnBecomeInvisible : MonoBehaviour
    {
        private Vector3 startingPosition;

        private void Awake()
        {
            startingPosition = transform.position;
        }

        private void OnBecameInvisible()
        {
            transform.position = startingPosition;
        }
    }
}