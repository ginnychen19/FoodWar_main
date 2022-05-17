using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LoadingPanel : MonoBehaviourPunCallbacks
{
    [SerializeField] VideoClip[] videos;
    [SerializeField] RenderTexture renderImage;
    [SerializeField] Slider progressBar;
    [SerializeField] string[] sceneToLoad;// sceneName 
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] TMP_Text progressText;
    [SerializeField] Text peopleCountText;

    AsyncOperation async;

    public string sceneName;
    float progress = 0f;

    private void Start()
    {
        progress = 0f;
        RandomLoadingVideo();
        if (PhotonNetwork.IsMasterClient)
        {
            async = SceneManager.LoadSceneAsync(sceneToLoad[Random.Range(0, sceneToLoad.Length)]);//sceneToLoad[Random.Range(0, sceneToLoad.Length)]
            async.allowSceneActivation = false;
            StartCoroutine(LoadingScene());
        }
        else
        {
            StartCoroutine(ClinetLoading());
            
        }

        peopleCountText.gameObject.SetActive(PhotonNetwork.IsMasterClient);

    }

   public IEnumerator ClinetLoading()
    {
        while (progress < 0.99f)
        {
            progress += 0.5f * Time.deltaTime;
            progressBar.value = progress;
            progressText.SetText(Mathf.Floor(progress * 100f).ToString("0") + "%");
            yield return null;
        }
        photonView.RPC("CountPeople", RpcTarget.MasterClient);
    }
    int pcount
    {
        get { return _pcount; }
        set { peopleCountText.text = "連線人數 : " + value + " / " + PhotonNetwork.CurrentRoom.PlayerCount; _pcount = value; }
    }
    bool isOkToGo = false;
    int _pcount = 0;
    [PunRPC]
    public void CountPeople()
    {
        pcount++;
        if (pcount >= PhotonNetwork.CurrentRoom.PlayerCount && pcount > 0 && isOkToGo == false)
        {
            isOkToGo = true;
        }
    }
   public IEnumerator LoadingScene()
   {

       while (progress < 0.99f)
       {
           progress = Mathf.Lerp(progress, async.progress / 9 * 10, Time.deltaTime);
           //progress += Mathf.Clamp(0, 0.01f, BoltNetwork.CurrentAsyncOperation.progress);
           progressBar.value = progress;
           progressText.SetText(Mathf.Floor(progress * 100f).ToString() + "%");
           yield return null;
       }

       progress = 1f;
       progressBar.value = 1f;

        photonView.RPC("CountPeople", RpcTarget.MasterClient);
        while (isOkToGo == false)
        {
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(2f);
        async.allowSceneActivation = true;
        progress = 0f;


       // Debug.LogError(sceneToLoad);


     

}
      
    void RandomLoadingVideo()
    {
        videoPlayer.clip = videos[Random.Range(0, videos.Length)];
    }
}
