using System;
using UnityEngine;

namespace Code.CameraSystem
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CameraTuner : MonoBehaviour
    {
        [SerializeField] private FollowCamera camera;
        [SerializeField] private Vector2 tuning;
        
        private BoxCollider2D _myCollider;

        private void Awake()
        {
            _myCollider = GetComponent<BoxCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.CompareTag("Player"))
                camera.TuneDisplacement(tuning);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(other.gameObject.CompareTag("Player"))
                camera.TuneDisplacement(Vector2.zero); 
        }
    }
}