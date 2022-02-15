using System.Collections.Generic;
using Code.Patterns;
using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "PoolerDatabase", menuName = "Pooling/Database", order = 0)]
    public class PoolerDatabase : ScriptableObject
    {
        [SerializeField] private List<PoolCandidate> poolObjects;
        
        public List<PoolCandidate> PoolCandidates => poolObjects;
    }
}