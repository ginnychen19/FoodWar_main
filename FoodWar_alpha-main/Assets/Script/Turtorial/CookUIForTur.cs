using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookUIForTur : MonoBehaviour
{
    public static CookUIForTur instance;
    Vector3 offset;
    Camera cam;
    public IngredientSlot[] ingredients;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        cam = Camera.main;
        offset = new Vector3(0, 80, 0);

    }
   

    
    public void SetCookerUIBillboard(Vector3 lookAtPos)
    {
        if (cam != null)
        {
            Vector3 pos = cam.WorldToScreenPoint(lookAtPos) + offset;
          
            if (this.gameObject.transform.position != pos) this.gameObject.transform.position = pos;
        }
        
    }

}
