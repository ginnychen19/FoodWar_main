using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BackLobby : MonoBehaviour
{
   public void BackToLobby()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }
}
