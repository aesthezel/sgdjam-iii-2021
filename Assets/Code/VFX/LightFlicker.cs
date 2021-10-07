using UnityEngine;

namespace Code.VFX
{
    public class LightFlicker : MonoBehaviour
    {
        public string waveFunction = "sin"; // sin, tri, sqr, saw, inv, noise
        public float startValue = 0.0f;
        public float amplitude = 1.0f;
        public float phase = 0.0f;
        public float frequency = 0.5f;
        
        private Color originalColor;
        
        void Start (){
            originalColor = GetComponent<Light>().color;
        }
 
        void Update (){
            Light light = GetComponent<Light>();
            light.color = originalColor * (EvalWave());
        }
 
        float EvalWave (){
            float initial = (Time.time + phase)*frequency;
            float final;
 
            initial = initial - Mathf.Floor(initial);
 
            if (waveFunction=="sin") {
                final = Mathf.Sin(initial*2*Mathf.PI);
            }
            else if (waveFunction=="tri") {
                if (initial < 0.5f)
                    final = 4.0f * initial - 1.0f;
                else
                    final = -4.0f * initial + 3.0f;  
            }    
            else if (waveFunction=="sqr") {
                if (initial < 0.5f)
                    final = 1.0f;
                else
                    final = -1.0f;  
            }    
            else if (waveFunction=="saw") {
                final = initial;
            }    
            else if (waveFunction=="inv") {
                final = 1.0f - initial;
            }    
            else if (waveFunction=="noise") {
                final = 1 - (Random.value*2);
            }
            else {
                final = 1.0f;
            }        
            return (final*amplitude)+startValue;     
        }
    }
}