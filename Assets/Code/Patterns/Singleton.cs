using UnityEngine;

namespace Code.Patterns
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                CreateInstance();
                return instance;
            }
        }
    
        private static void CreateInstance() {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                
                if (instance == null)
                {
                    var go = new GameObject(typeof(T).Name);
                    instance = go.AddComponent<T>();
                }
                
                if (!instance.initialized)
                {
                    instance.Initialize();
                    instance.initialized = true;
                }
            }
        }
    
        protected bool initialized;
        protected virtual void Initialize() { Debug.Log("Base MonobehaviourSingleton");}
    
        public virtual void Awake() 
        {
            if (Application.isPlaying) {
                DontDestroyOnLoad(this);
            }
        
            if (instance != null) {
                DestroyImmediate(gameObject);
            }
        }
    }
}