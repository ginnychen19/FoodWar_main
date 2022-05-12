

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public static Detector instance;
  

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && SquenceManager.instance.sequenceIndex == 27)
        {
            SquenceManager.instance.NextSequence(SquenceManager.instance.sequenceIndex);
            this.gameObject.SetActive(false);
        }
    }
   

}

