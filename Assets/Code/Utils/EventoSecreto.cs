using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EventoSecreto : MonoBehaviour
{
    [SerializeField] private  GameObject mapToDeactivate;
    [SerializeField] private  GameObject mapToActivate;



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            mapToActivate.SetActive(true);
            StartCoroutine(Disappear());
        }
    }

    private IEnumerator Disappear()
    {
        var maps = mapToDeactivate.GetComponentsInChildren<Tilemap>();
        var currentAlpha = 1f;
        while (currentAlpha > 0)
        {
            currentAlpha -= Time.deltaTime * 2f;
            foreach (var tilemap in maps)
            {
                var color = tilemap.color;
                tilemap.color = new Color(color.r, color.g, color.b, currentAlpha);
            }

            yield return new WaitForEndOfFrame();
        }
        
        mapToDeactivate.SetActive(false);
    }
}
