using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    MaterialSlot materialSlot;
   
    public Animator oilGunFX;
    public static PlayerHUD instance;
    [SerializeField] GameObject pauseUi;
    
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        materialSlot = GetComponentInChildren<MaterialSlot>();
        materialSlot.SetMaterialImage(SaveManager.instance.nowData.characterID);
    }
    public void ClosePause()
    {
        pauseUi.SetActive(false);
    }
    private void OpenPauseUI()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseUi.SetActive(!pauseUi.activeSelf);

        }
    }
    private void Update()
    {
        OpenPauseUI();
    }

    public void BackToLobby()
    {
        RoomManager.instance.MeLeftRoom();
    }


}
