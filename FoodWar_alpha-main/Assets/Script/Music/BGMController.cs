using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMController : MonoBehaviour
{
    public static BGMController instance;
    [SerializeField] AudioSource BGMPlayer;
    [SerializeField] float volumeDelta;
    [SerializeField] float targetVolume;
    [SerializeField] bool isFading;
    [SerializeField] AudioClip lobbyBGM;
    [SerializeField] AudioClip coffeeBGM;
    [SerializeField] AudioClip parkBGM;
    string sceneName;


    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(this.gameObject);
        }
        BGMPlayer = GetComponent<AudioSource>();
        volumeDelta = 0;
        isFading = false;

    }
    private void Start()
    {
        SceneManager.activeSceneChanged += SceneTransition;
        BGMPlayer.volume = 0;
        BGMPlayer.Play();
        FadeInOROut(0.3f, 10);



    }


    private void Update()
    {
        if (!isFading)
            return;
        if (Mathf.Abs(BGMPlayer.volume - targetVolume) >= Mathf.Abs(volumeDelta))
        {
            BGMPlayer.volume += (float)volumeDelta;

        }
        else
        {
            BGMPlayer.volume = targetVolume;
            isFading = false;
        }
    }

    public void FadeMusic(float _targetVolume, float durtime)
    {
        targetVolume = _targetVolume;
        if (BGMPlayer == null)
        {
            this.GetComponent<AudioSource>();
        }
        float timedelta = durtime / Time.deltaTime;
        if (timedelta > 0)
        {
            volumeDelta = (targetVolume - BGMPlayer.volume) / timedelta;
        }
        else
        {
            volumeDelta = (targetVolume - BGMPlayer.volume);
        }
        isFading = true;
    }
    //sample: fadeIn(0.5f, 1)
    //sample: fadeOut(0, 1)
    public void FadeInOROut(float targetV, float durT)
    {
        FadeMusic(targetV, durT);
    }

    private void SceneTransition(Scene current, Scene next)
    {
        
        if (next.name != "Wait")
        {
            FadeInOROut(0, 1f);
        }

        if (next.IsValid())
        {


            if (next.name == "1")
            {
                BGMPlayer.clip = coffeeBGM;
                BGMPlayer.Play();
                FadeInOROut(0.5f, 1);
            }
            if (next.name == "2")
            {
                BGMPlayer.clip = parkBGM;
                BGMPlayer.Play();
                FadeInOROut(0.5f, 1);
            }
            if (next.name == "Lobby")
            {
                if (BGMPlayer.clip != lobbyBGM)
                {
                    BGMPlayer.clip = lobbyBGM;
                    BGMPlayer.Play();
                }
                

               
               
                FadeInOROut(0.3f, 5);

            }
        }
    }


}
