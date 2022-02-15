using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Code.Cinematics
{
    public class EndLight : MonoBehaviour
    {
        [SerializeField] private Light2D myLight;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            StartCoroutine(IntensityUp());
        }

        private IEnumerator IntensityUp()
        {
            var intensity = 0f;
            
            while (intensity < 40)
            {
                intensity += Time.deltaTime * 10;
                myLight.intensity = intensity;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
