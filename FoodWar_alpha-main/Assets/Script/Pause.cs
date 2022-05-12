using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject setting;
   bool _isPause
    {
        get { return RoomManager.instance.isPause; }
        set { RoomManager.instance.isPause = value; }
    }
    private void OnEnable()
    {
        _isPause = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void OnDisable()
    {
        _isPause = false;
        Cursor.lockState = CursorLockMode.Locked;
        setting.SetActive(false);
    }
   
  
}
