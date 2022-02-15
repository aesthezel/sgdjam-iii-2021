using UnityEngine;

namespace Code.Cinematics
{
    [RequireComponent(typeof(Animator))]
    public class NPC_Animations : MonoBehaviour
    {
        [SerializeField] private string[] clips;
        private Animator _animator;
    
        private void Awake() => _animator = GetComponent<Animator>();
        public void PlayClip(string clipName) => _animator.Play(clipName);
    }
}