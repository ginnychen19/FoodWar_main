using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : MonoBehaviour
{
    public static Sequence instance;
    [SerializeField] public List<GameObject> sequences;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
       
    }
}
