using UnityEngine;

namespace Code.VFX
{
  public class Projectile : MonoBehaviour
  {
    public string tagToDestroy;
    
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject additionalParticle;

    public GameObject[] trailParticles;
    
    [HideInInspector]
    public Vector3 impactNormal;

    private bool hasCollided = false;
    
    void Awake()
    {
      projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
      projectileParticle.transform.parent = transform;
      
      if (additionalParticle)
      {
        additionalParticle = Instantiate(additionalParticle, transform.position, transform.rotation) as GameObject;
        Destroy(additionalParticle, 1.5f);
      }
    }

    void OnCollisionEnter(Collision hit)
    {
      if (!hasCollided)
      {
        hasCollided = true;
        impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;

        if (hit.gameObject.tag == tagToDestroy)
        {
          Destroy(hit.gameObject);
        }

        foreach (GameObject trail in trailParticles)
        {
          GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
          curTrail.transform.parent = null;
          Destroy(curTrail, 3f);
        }
        Destroy(projectileParticle, 3f);
        Destroy(impactParticle, 5f);
        Destroy(gameObject);

        ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
        
        for (int i = 1; i < trails.Length; i++)
        {

          ParticleSystem trail = trails[i];

          if (trail.gameObject.name.Contains("Trail"))
          {
            trail.transform.SetParent(null);
            Destroy(trail.gameObject, 2f);
          }
        }
      }
    }
    
  }
}