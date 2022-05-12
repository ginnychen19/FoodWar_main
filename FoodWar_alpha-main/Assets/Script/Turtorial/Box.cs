using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    int mat = 4;
    [SerializeField] TMPro.TMP_Text matCount;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && mat < 5)
        {
            mat++;
            matCount.text = "5/5";
            SquenceManager.instance.GetBox();
        }
        
    }
}
