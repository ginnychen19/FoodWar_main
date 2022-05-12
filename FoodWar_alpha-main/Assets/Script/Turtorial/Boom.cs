using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("TurTarget"))
        {
            SquenceManager.instance.GunBombHit();
        }
      
       

        DetroySelf();
    }
    [SerializeField] GameObject impactParticle = null;
    private void DetroySelf()
    {



        Instantiate(impactParticle, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
    
}
