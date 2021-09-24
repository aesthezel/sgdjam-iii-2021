using UnityEngine;

public class MoveAndGoback : MonoBehaviour
{
    public float x;
    public float y;
    private Vector3 initialposition;

    private void Awake()
    {
        initialposition = transform.position;
    }

    private void Update()
    {
        transform.Translate(new Vector3(x, y, 0) * Time.deltaTime * 3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        transform.position = initialposition;
    }

    private void OnCollisionEnter(Collision other)
    {
        transform.position = initialposition;
    }

}
