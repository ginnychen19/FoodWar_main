using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveUI : MonoBehaviour
{
    [SerializeField] GameObject leaveUI;

    private void Update()
    {
        OpenUI();
    }

    private void OpenUI()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            leaveUI.SetActive(!leaveUI.activeSelf);
        }
    }
    public void CloseUI()
    {
        Invoke("Close", 0.1f);
    }
    public void LeavGame()
    {
        Invoke("Leave", 0.1f);
    }
    private void Leave()
    {
        Application.Quit();
    }

    private void Close()
    {
        leaveUI.SetActive(false);
    }
}
