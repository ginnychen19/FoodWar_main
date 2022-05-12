using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurFood : MonoBehaviour
{
    bool canSend;
    void CheckTarget()
    {
        Collider[] cols = Physics.OverlapSphere(this.transform.position, 3);
        if (cols != null)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].CompareTag("Obstacle"))
                {
                    canSend = true;
                }
            }
        }
        else
        {
            canSend = false;
        }
    }

    private void FixedUpdate()
    {
        CheckTarget();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canSend && HotBarForTur.instance.currentSlotIndex == 2 && SquenceManager.instance.sequenceIndex == 43)
        {
          
            SquenceManager.instance.NextSequence(SquenceManager.instance.sequenceIndex);
            
        }
    }
}
