using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientSlot : InventorySlot
{
    public Button addIngredientButtom;
    public int index;
    public Action<int> ButtomOnClick;
 
    
    
    private void Awake()
    {
        includeInInventory = false;
        addIngredientButtom = this.GetComponent<Button>();
        

    }
    private void OnEnable()
    {
        addIngredientButtom.onClick.RemoveAllListeners();
        addIngredientButtom.onClick.AddListener(() => { if (ButtomOnClick != null) ButtomOnClick(index); });

       
      
    }
    private void OnDisable()
    {
        addIngredientButtom.onClick.RemoveAllListeners();
        
    }


}
