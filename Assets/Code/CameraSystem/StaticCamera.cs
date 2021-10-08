using UnityEngine;


namespace Code.CameraSystem
{
    public class StaticCamera: ICameraItem
    {
        [SerializeField] private float ortographicSize;

        private void Awake()
        {
            mycamera = GetComponent<Camera>();
        }

        private void Start()
        {
            mycamera.orthographicSize = ortographicSize;
        }
    }
}