using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") &&  SquenceManager.instance.sequenceIndex == 41)
        {
            SquenceManager.instance.NextSequence(SquenceManager.instance.sequenceIndex);
        }
    }
}
