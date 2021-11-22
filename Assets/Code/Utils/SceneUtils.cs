using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUtils: MonoBehaviour
{
    public void LoadScene(string sceneName) => StartCoroutine(WaitLoad(sceneName));

    private IEnumerator WaitLoad(string sceneName)
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneName);
    }
}