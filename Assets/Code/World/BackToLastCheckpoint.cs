using Code.Hero;
using UnityEngine;

namespace Code.World
{
    [RequireComponent(typeof(Collider2D))]
    public class BackToLastCheckpoint: MonoBehaviour
    {
        // This script is thought to be used when the player falls, not when it is on the ground.
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.transform.parent.GetComponent<PlayerController2D>().BackToCheckpoint();
            }
        }
    }
}