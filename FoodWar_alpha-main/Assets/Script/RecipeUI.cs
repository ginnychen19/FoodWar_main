

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
{
    [Header("Buttoms")]

    [SerializeField] private Button[] btns;
    [Header("Objs")]
    [SerializeField] private GameObject[] goObjects;


    private void Start()
    {
        for (int i = 0; i < btns.Length; i++)
        {
            int index = i;
            btns[i].onClick.AddListener(() =>
            {
                ShowAndHideObject(index);
            });
        }
    }


    private void ShowAndHideObject(int index)
    {
        for (int i = 0; i < goObjects.Length; i++)
        {
            goObjects[i].SetActive(false);
        }


        goObjects[index].SetActive(true);
    }
}
