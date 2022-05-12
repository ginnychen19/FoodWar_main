using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class CookController : MonoBehaviourPunCallbacks
{

    public bool inCookerArea;
   
    [SerializeField] Transform currentCookerTrans = null;
    [SerializeField] CookUI cookUI;
    [SerializeField] PlayerController playerController;
    [SerializeField] AudioSource SFXPlayer;
    [SerializeField] AudioClip pickUpSFX;
    [SerializeField] AudioClip errorSFX;
   
    public Cooker currentCooker;
    public bool canPickUpDish;
    PhotonView PV;
    public string userId;
    public FoodTeam myTeam;
    public int currentItemIndex;
   
    public bool canUsePotion;
    MeshRenderer cookerMat;
    bool _isDishSlotFull
    {
        get { return RoomManager.instance.isDishSlotFull; }
    }

    private void Start()
    {
        currentItemIndex = -1;
        PV = this.GetComponent<PhotonView>();
        userId = PV.Owner.UserId;
        cookUI = CookUI.instance;
        cookUI.gameObject.SetActive(false);
        playerController = gameObject.GetComponent<PlayerController>();
        myTeam = playerController.teamValue;
    }

    private void Update()
    {
        
        SendCookUIRequest();
        PickUpDish();
       
    }
    float lestPickUpTime;
    private void PickUpDish()
    {
        if (canPickUpDish && Input.GetKeyDown(KeyCode.E) && ! _isDishSlotFull)
        {
            if (PV.IsMine)
            {
                if (Time.time < lestPickUpTime + 0.1f)
                    return;
                lestPickUpTime = Time.time;
                currentCooker.PickUPDishRequestToServer(currentCooker.PVVeiwId, userId);
                SFXPlayer.PlayOneShot(pickUpSFX);
                
            }
        }
        else if (canPickUpDish && Input.GetKeyDown(KeyCode.E) && currentCooker.resultIconDisplay.gameObject.activeSelf && _isDishSlotFull)
        {
            SFXPlayer.PlayOneShot(errorSFX);
        }
      
    }
    /// <summary>
    /// OpenCookerUI
    /// </summary>
    private void SendCookUIRequest()
    {
        if (Input.GetKeyDown(KeyCode.E) && inCookerArea && !RoomManager.instance.isCookUiOpen && !currentCooker.cookingProgress.gameObject.activeSelf &&
            !currentCooker.resultIconDisplay.gameObject.activeSelf && currentCooker != null)
        {
            //Open
            if (PV.IsMine)
            {
                currentCooker.SendOpenRequestServerCooker(userId);
              
                
            }

              
            
        }
        else if (Input.GetKeyDown(KeyCode.E) && inCookerArea && RoomManager.instance.isCookUiOpen)
        {
            //Close
            if (PV.IsMine)
            {
                currentCooker.SendCloseRequestServerCooker(userId);
            }
        }
    }







    #region CheckPot

    private void FixedUpdate()
    {
        if (PV.IsMine && RoomManager.instance.isGameStart)
        {
            GetPot(5, 60);
        }
       
    }

   
    public void GetPot(int radius, int angle)
    {
        Collider[] cols = Physics.OverlapSphere(this.transform.position, radius, 1 << LayerMask.NameToLayer("Pot"));

        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {

                Vector3 dir = cols[i].transform.position - this.transform.position;
                if (Vector3.Angle(dir, this.transform.forward) < angle)
                {
                    
                    currentCooker = cols[i].GetComponent<Cooker>();
                    if (currentItemIndex == 2 && currentCooker != null)
                    {
                        cookerMat = currentCooker.gameObject.GetComponent<MeshRenderer>();
                        cookerMat.material.color = new Color(0.8627451f, 0.2745098f, 0.2666667f);
                        canUsePotion = true;
                    }


                    if (currentCooker.resultIconDisplay.sprite != null && currentCooker.resultIconDisplay.gameObject.activeSelf)
                    {
                       

                        
                        canPickUpDish = true;
                    }
                    
                    if (currentCooker.cookerTeam.ToString() == playerController.teamValue.ToString())
                    {
                        if (!CookUI.instance.gameObject.activeSelf && !currentCooker.cookingProgress.gameObject.activeSelf && !currentCooker.resultIconDisplay.gameObject.activeSelf)
                        {
                            currentCooker.openRemain.SetActive(true);
                        }
                        else
                        {
                            currentCooker.openRemain.SetActive(false);
                        }
                       
                        currentCookerTrans = cols[i].transform;
                        inCookerArea = true;
                    }

                }
                else if (currentCooker != null)
                {
                    if (cookerMat != null)
                    {
                        cookerMat.material.color = Color.white;
                        cookerMat = null;
                    }
                    canUsePotion = false;
                    if (RoomManager.instance.isCookUiOpen)
                    {
                        currentCooker.SendCloseRequestServerCooker(userId);
                    }
                    
                    currentCooker.openRemain.SetActive(false);
                   
                    canPickUpDish = false;
                    inCookerArea = false;
                    currentCooker = null;
                    currentCookerTrans = null;
                }



            }
        }
        else if (currentCooker != null)
        {
            if (cookerMat != null)
            {
                cookerMat.material.color = Color.white;
                cookerMat = null;
            }
            canUsePotion = false;
            if (RoomManager.instance.isCookUiOpen)
            {
                currentCooker.SendCloseRequestServerCooker(userId);
            }
            
            currentCooker.openRemain.SetActive(false);            
            canPickUpDish = false;
            inCookerArea = false;
            currentCooker = null;
            currentCookerTrans = null;
        }

    }
#endregion


}
