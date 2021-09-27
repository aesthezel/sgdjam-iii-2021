using System.Collections;
using Code.Hero;
using UnityEngine;

namespace Code.Utils.Environment
{
    public class Push : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var rb = other.GetComponent<Rigidbody2D>();
                var controller = other.GetComponent<PlayerController>();
                var facing = controller.FacingRight ? -1 : 1;
                controller.DisableInput(0.4f);
                rb.velocity = Vector2.zero;
                rb.AddForce(other.transform.right * 3.5f * facing, ForceMode2D.Impulse);
            }
        }
    }
}