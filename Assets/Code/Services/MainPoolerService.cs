using Code.Interfaces;
using Code.Patterns;
using UnityEngine;

namespace Code.Services
{
    public class MainPoolerService : MonoBehaviour, IService
    {
        public ObjectPooler Pooler { get; private set; }

        private void Awake()
        {
            Pooler = GetComponent<ObjectPooler>();
        }
    }
}