using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurPO : MonoBehaviour
{
    bool canUse;
    [SerializeField] GameObject pot;
    [SerializeField] ParticleSystem FX;


    void FindPot()
    {
        Collider[] cols = Physics.OverlapSphere(this.transform.position, 3);
        if (cols != null)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].gameObject.CompareTag("Pot"))
                {
                    canUse = true;
                }
                
            }
        }
        else
        {
            canUse = false;
        }
    }
    private void FixedUpdate()
    {
        FindPot();
    }

    private void Update()
    {
        if (canUse && Input.GetMouseButtonDown(0))
        {
            Instantiate(FX, pot.gameObject.transform.position, Quaternion.Euler(-90, 0, 0));
            SquenceManager.instance.PotionHit();
            HotBarForTur.instance.WeaponUse();
        }
    }
}
