using UnityEngine;
using UnityEngine.SceneManagement;

namespace  Code.Utils
{
    public class LoadSceneOnCollide : MonoBehaviour
    {
        public string sceneName;
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("TransparentToEnemies"))
                SceneManager.LoadScene(sceneName);
        }
    }
}