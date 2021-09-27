using System.Collections;
using Code.Patterns;
using Code.Services;
using UnityEngine;

namespace Code.Utils.Environment
{
    public class GargoyleBehaviour : MonoBehaviour
    {
        [SerializeField] private float cooldown = 1.5f;
        [SerializeField] private float initialWait; 
        private ObjectPooler _pooler;
        private Transform _shootPoint;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(ShootCoroutine());
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                StopAllCoroutines();
            }
        }
        

        private void Start()
        {
            _pooler = ServiceLocator.Instance.ObtainService<MainPoolerService>().Pooler;
            _shootPoint = transform.GetChild(0).transform;
        }

        private IEnumerator ShootCoroutine()
        {
            yield return new WaitForSeconds(initialWait);
            while (true)
            {
                var go = _pooler.GetByID("FireBall");
                go.transform.position = _shootPoint.position;
                var dir = transform.right;
                if (transform.localScale.x < 0) dir = transform.right * -1; 
                go.GetComponent<FireBall>().direction = dir;
                yield return new WaitForSeconds(cooldown);
            }
        }
    }
}