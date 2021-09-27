using UnityEngine;

namespace Code.VFX
{
    public class LightFade : MonoBehaviour
    {
        [Header("Dim light")]
        public float life = 0.2f;
        public bool destroyAfter = true;
 
        private Light _light;
        private float _initialIntensity;
 

        void Start()
        {
            if (gameObject.GetComponent<Light>())
            {
                _light = gameObject.GetComponent<Light>();
                _initialIntensity = _light.intensity;
            }
            else
                print("No light object found on " + gameObject.name);
        }
        
        void Update()
        {
            if (gameObject.GetComponent<Light>())
            {
                _light.intensity -= _initialIntensity * (Time.deltaTime / life);
                if (destroyAfter && _light.intensity <= 0)
                    Destroy(gameObject);
            }
        }
    }
}