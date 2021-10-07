using System;
using System.Collections;
using Code.Patterns;
using Code.Services;
using UnityEngine;


namespace Code.Utils.Environment
{
    public class FireBall : MonoBehaviour
    {
        public Vector2 Direction { get; set; } = Vector2.right;
        public float speed = 150f;
        
        private Rigidbody2D _rigidbody2D;
        private MainPoolerService _pooler;
        private Collider2D _myCollider;
        private ParticleSystem[] myEffects;
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _myCollider = GetComponent<Collider2D>();
            myEffects = GetComponentsInChildren<ParticleSystem>();
            _pooler = ServiceLocator.Instance.ObtainService<MainPoolerService>();
        }

        private void FixedUpdate()
        {
            _rigidbody2D.velocity = (Direction * speed) * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            speed = 0;
            var destroyObj = _pooler.Pooler.GetByID("FireBallDestroyEffect");
            destroyObj.transform.position = transform.position;
            StartCoroutine(DisableEffect(destroyObj, 0.5f));
            _myCollider.enabled = false;
            foreach (var ps in myEffects)
            {
                ps.Stop();
            }
            
        }

        private IEnumerator DisableEffect(GameObject go, float time)
        {
            yield return new WaitForSeconds(time);
            _myCollider.enabled = true;
            go.SetActive(false);
            gameObject.SetActive(false);
            speed = 150f;
        }
        
    }
}