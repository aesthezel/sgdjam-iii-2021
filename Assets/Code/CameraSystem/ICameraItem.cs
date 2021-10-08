using UnityEngine;

namespace Code.CameraSystem
{
    [RequireComponent(typeof(Camera))]
    public abstract class ICameraItem : MonoBehaviour
    {
        protected Camera mycamera;
        [SerializeField] protected string id;
        
        public string ID => id;
    }
}