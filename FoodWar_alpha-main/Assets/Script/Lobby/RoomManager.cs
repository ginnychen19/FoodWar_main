using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance = null;
    [SerializeField] GameObject playerUI = null;
    [SerializeField] GameObject mainCam = null;
    PhotonView PV;
    
   
    private void Awake()
    {
        instance = this;
    }
    [SerializeField] Transform[] 生成點 = new Transform[0];
    [SerializeField] Transform[] goodSpawnPoint = new Transform[0];
    [SerializeField] Transform[] badSpawnPoint = new Transform[0];
    public CookController myPlayer = null;

    //------------- PlayerStatus-------------------
    public bool isGameStart = false;
    public bool isStun = false;
    public bool isCookUiOpen = false;
    public bool isDishSlotFull = false;
    public bool isGameOver;
    public bool isPause = false;
    public float pitch;
    public float yaw;







    //----------------------------------------

    private void Start()
    {
       

        
        PV = gameObject.GetPhotonView();
      
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < goodSpawnPoint.Length; i++)
            {
                PhotonNetwork.InstantiateRoomObject("GoodPot", goodSpawnPoint[i].position, Quaternion.Euler(0, Random.Range(-360, 360), 0));
                PhotonNetwork.InstantiateRoomObject("BadPot", badSpawnPoint[i].position, Quaternion.Euler(0, Random.Range(-360, 360), 0));
            }



            PhotonNetwork.InstantiateRoomObject("ScoreManager", transform.position, Quaternion.identity);
            
            
            
        }
       
        // 每個人都生成自己的角色到場地上
        GameObject.Instantiate(mainCam, mainCam.transform.position, mainCam.transform.rotation);
        myPlayer = PhotonNetwork.Instantiate("Player", 生成點[Random.Range(0, 生成點.Length)].position, Quaternion.identity).GetComponent<CookController>();
        GameObject.Instantiate(playerUI, playerUI.transform.position, playerUI.transform.rotation);
        


    }

  

    public void MeLeftRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    int PlayerReadyNumber = 0;
    public void 點名()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PlayerReadyNumber++;
            if (PlayerReadyNumber >= PhotonNetwork.CurrentRoom.PlayerCount)
            {
                Invoke("StartGame", 1f);
            }
        }
    }
    bool isStartGame = false;
    void StartGame()
    {
        if (isStartGame)
            return;
        isStartGame = true;
        photonView.RPC("RPCStartGame", RpcTarget.AllBuffered);
    }
    /// <summary>遊戲開始事件</summary>
    public System.Action<double> Act_StartGame = null;
    [PunRPC]
    public void RPCStartGame(PhotonMessageInfo info)
    {
        
        if (Act_StartGame != null)
        {
            Act_StartGame.Invoke(PhotonNetwork.Time - info.SentServerTime);
        }
    }
}

