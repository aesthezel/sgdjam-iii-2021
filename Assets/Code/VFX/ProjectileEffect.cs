using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace PixelArsenal
{
    public class ProjectileEffect : MonoBehaviour
    {
        RaycastHit hit;
        public GameObject[] projectiles;
        public Transform spawnPosition;
        [HideInInspector]
        public int currentProjectile = 0;
        public float speed = 1000;

        // void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.RightArrow))
        //     {
        //         nextEffect();
        //     }
        //
        //     if (Input.GetKeyDown(KeyCode.D))
        //     {
        //         nextEffect();
        //     }
        //
        //     if (Input.GetKeyDown(KeyCode.A))
        //     {
        //         previousEffect();
        //     }
        //     else if (Input.GetKeyDown(KeyCode.LeftArrow))
        //     {
        //         previousEffect();
        //     }
        //
        //     if (Input.GetKeyDown(KeyCode.Mouse0))
        //     {
        //
        //         if (!EventSystem.current.IsPointerOverGameObject())
        //         {
        //             if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f))
        //             {
        //                 GameObject projectile = Instantiate(projectiles[currentProjectile], spawnPosition.position, Quaternion.identity) as GameObject;
        //                 projectile.transform.LookAt(hit.point);
        //                 projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * speed);
        //                 projectile.GetComponent<Projectile>().impactNormal = hit.normal;
        //             }
        //         }
        //
        //     }
        //     Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction * 100, Color.yellow);
        // }

        public void NextEffect()
        {
            if (currentProjectile < projectiles.Length - 1)
                currentProjectile++;
            else
                currentProjectile = 0;
        }

        public void PreviousEffect()
        {
            if (currentProjectile > 0)
                currentProjectile--;
            else
                currentProjectile = projectiles.Length - 1;
        }

        public void AdjustSpeed(float newSpeed)
        {
            speed = newSpeed;
        }
    }
}