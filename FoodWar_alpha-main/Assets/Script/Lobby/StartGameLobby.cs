using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGameLobby : Windows<StartGameLobby>
{
    [SerializeField] GameObject fightImage;
    [SerializeField] Text versionText;
    float progress;
    AsyncOperation async;
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        fightImage.SetActive(false);
        versionText.text = Application.version;
    }

    public void LoadAnimeScene()
    {
        async = SceneManager.LoadSceneAsync("AnimeScene");
        async.allowSceneActivation = false;
        �UŪ.ins.isOpen = true;
        �UŪ.ins.info = "Ū����...";
        StartCoroutine(LoadingProgress());
    }

    public void LoadTurScene()
    {
        async = SceneManager.LoadSceneAsync("0-02");
        async.allowSceneActivation = false;
        �UŪ.ins.isOpen = true;
        �UŪ.ins.info = "�о������J��...";
        StartCoroutine(LoadingProgress());
    }

    public void LoadPokedexScene()
    {
        async = SceneManager.LoadSceneAsync("UI");
        async.allowSceneActivation = false;
        �UŪ.ins.isOpen = true;
        �UŪ.ins.info = "Ū����...";
        StartCoroutine(LoadingProgress());
    }

    private IEnumerator LoadingProgress()
    {
        while (progress < 0.99f)
        {
           
            progress += Mathf.Clamp(0, 0.002f, async.progress);
           
            yield return null;
        }

        progress = 1f;

        �UŪ.ins.info = "";
        �UŪ.ins.isOpen = false;
        async.allowSceneActivation = true;
        progress = 0f;
    }


   
}
