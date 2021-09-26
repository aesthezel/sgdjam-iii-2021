using UnityEngine;

namespace Code.Services
{
    [CreateAssetMenu(fileName = "ServiceSettings", menuName = "Settings/Service Settings", order = 0)]
    public class ServicesSettings : ScriptableObject
    {
        [SerializeField] private GameObject[] servicesPrefabs;
        public GameObject[] ServicesPrefabs
        {
            get => servicesPrefabs;
        }
    }
}