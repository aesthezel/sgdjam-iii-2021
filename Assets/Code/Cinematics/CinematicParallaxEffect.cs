using UnityEditor.Rendering;
using UnityEngine;

namespace Code.Cinematics
{
    public class CinematicParallaxEffect : MonoBehaviour
    {
        [SerializeField] private float vel;
        [SerializeField] Transform[] tiles;

        private void Update()
        {
            foreach (var t in tiles)
                t.Translate(Vector3.up * Time.deltaTime * vel);

            for (var i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].position.y > 14)
                {
                    var next = (i + 2) % tiles.Length;
                    tiles[i].position = new Vector3(0, tiles[next].position.y - 14, 0);
                }
            }
        }
    }
}
