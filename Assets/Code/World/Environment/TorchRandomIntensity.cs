using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Code.World
{
    public class TorchRandomIntensity: MonoBehaviour
    {
        [SerializeField] private Light2D myLight;
        private bool _mustChange = true;
        
        private void Update()
        {
            if (!_mustChange) return;
            
            myLight.intensity = Random.Range(0.9f, 1.1f);
            StartCoroutine(ChangeLightIntensity());
        }

        private IEnumerator ChangeLightIntensity()
        {
            _mustChange = false;
            yield return new WaitForSeconds(0.1f);
            _mustChange = true;
        }
    }
}