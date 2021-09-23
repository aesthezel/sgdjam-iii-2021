using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera mainCamera;
    public Transform target;

    
    // Update is called once per frame
    void Update()
    {
        var pos = new Vector3(target.position.x, target.position.y, mainCamera.transform.position.z);
        transform.position = pos;
    }
}
