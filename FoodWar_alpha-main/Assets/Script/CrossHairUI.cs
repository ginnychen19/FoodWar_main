using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CrossHairUI : MonoBehaviour
{
    public static CrossHairUI instance;
   
    [SerializeField] CanvasGroup feedbackLines;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }

 
    private void OnEnable()
    {
       
    }


    


}
