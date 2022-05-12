using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public partial class PlayerController
{
    List<GameObject> playerList;
    float gameTime = 300;

    private void Awake()
    {
        RoomManager.instance.Act_StartGame += StartGameCountDown;
    }
    private void OnDestroy()
    {
        RoomManager.instance.Act_StartGame -= StartGameCountDown;
    }


    bool _isGameStart
    {
        get { return RoomManager.instance.isGameStart; }
        set { RoomManager.instance.isGameStart = value; }

    }
    bool _isStun
    {
        get { return RoomManager.instance.isStun; }
        set { RoomManager.instance.isStun = value; }
    }
    float _pitch
    {
        get { return RoomManager.instance.pitch; }
        set { RoomManager.instance.pitch = value; }
    }
    float _yaw
    {
        get { return RoomManager.instance.yaw; }
        set { RoomManager.instance.yaw = value; }
    }
    bool _isCookUiOpen
    {
        get { return RoomManager.instance.isCookUiOpen; }
    }

    private IEnumerator GameBeginCountdown(double lag)
    {

        GameBeginCountDown.instance.countDownText.gameObject.SetActive(true);
        float startTime = Time.time;
        while (Time.time < startTime + 5 - lag)
        {
            GameBeginCountDown.instance.countDownText.SetText(((startTime + 5 - lag) - Time.time).ToString("0"));
            yield return null;
        }
        GameBeginCountDown.instance.countDownText.gameObject.SetActive(false);
        _isGameStart = true;
        //photonView.RPC("GetPos", RpcTarget.Others, rb.position, Vector3.zero, this.transform.rotation.eulerAngles.y);

        StartCoroutine(GameTimer());




    }
    /// <summary>¶}©l­Ë¼Æ</summary>
    public void StartGameCountDown(double lag)
    {
       
        if (photonView.IsMine)
        {
            StartCoroutine(GameBeginCountdown(lag));
        }




    }

   

    public IEnumerator GameTimer()
    {
        float startTime = Time.time;
        while (Time.time < startTime + gameTime)
        {

            float time = ((startTime + gameTime) - Time.time);
            System.TimeSpan ts = new System.TimeSpan(0, 0, (int)time);
            ScoreBarAndTimer.instance.timerText.text = $"{ts.Minutes:00}:{ts.Seconds:00}";
            if (time <= 60)
            {
                ScoreBarAndTimer.instance.timerText.color = new Color(0.8627451f, 0.2745098f, 0.2666667f);
            }

            yield return null;
        }
        _isGameStart = false;
        
        if (PhotonNetwork.IsMasterClient)
        {
            EventManager.instance.GameOver();
        }
        


    }
}

