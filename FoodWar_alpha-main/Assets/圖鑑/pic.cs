using UnityEngine;
using UnityEngine.UI;


public class ShowObject : MonoBehaviour
{
    [Header("�Ʋz���s")]
    public Button[] btns;
    [SerializeField, Header("�Ʋz����")]
    private GameObject[] goObjects;


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

