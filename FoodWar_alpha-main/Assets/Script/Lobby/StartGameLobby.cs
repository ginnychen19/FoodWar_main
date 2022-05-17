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
        萬讀.ins.isOpen = true;
        萬讀.ins.info = "讀取中...";
        StartCoroutine(LoadingProgress());
    }

    public void LoadTurScene()
    {
        async = SceneManager.LoadSceneAsync("0-02");
        async.allowSceneActivation = false;
        萬讀.ins.isOpen = true;
        萬讀.ins.info = "教學關載入中...";
        StartCoroutine(LoadingProgress());
    }

    public void LoadPokedexScene()
    {
        async = SceneManager.LoadSceneAsync("UI");
        async.allowSceneActivation = false;
        萬讀.ins.isOpen = true;
        萬讀.ins.info = "讀取中...";
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

        萬讀.ins.info = "";
        萬讀.ins.isOpen = false;
        async.allowSceneActivation = true;
        progress = 0f;
    }


   
}
