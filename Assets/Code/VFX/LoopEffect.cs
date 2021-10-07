using UnityEngine;
using System.Collections;

namespace PixelArsenal
{
public class LoopEffect : MonoBehaviour {

	public GameObject effect;
    public float maxLoopTime = 2.0f;

	void Start () => StartLooping();
	
	public void StartLooping() => StartCoroutine("Loop");
	
	IEnumerator Loop()
	{
		GameObject effectPlayer = Instantiate(effect, transform.position, transform.rotation);

		yield return new WaitForSeconds(maxLoopTime);

		Destroy (effectPlayer);
		
		// TODO: ubicarlo en un while que espere una señal que lo rompa, en vez de ejecutar otra corrutina...
		StartLooping();
	}
}
}