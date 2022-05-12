using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTur : MonoBehaviour
{
    private void DetroySelf()
    {
        Instantiate(impactParticle, this.transform.position, this.transform.rotation);

        Destroy(this.gameObject);
    }
    [SerializeField] GameObject impactParticle = null;
    [SerializeField] ParticleSystem hitFX;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("TurTarget"))
        {
            SquenceManager.instance.GunBombHit();
        }

        Instantiate(hitFX, this.gameObject.transform.position, Quaternion.identity);
        

        DetroySelf();
    }
}
