using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class HotBar : MonoBehaviourPunCallbacks
{
    public InventorySlot[] slots = null;
    [SerializeField] GameObject gameUI = null;
    [SerializeField] Transform playerPos = null;
    [SerializeField] WeaponBase[] weapons = null;
    [SerializeField] DishBase[] dishes = null;
    [SerializeField] PlayerIK playerIK = null;
    [SerializeField] ParticleSystem dropFX;

    public static HotBar instance;


    GameObject currentEquip;
    Item lastItem;
    Item currentItem;
    PhotonView PV;
    ItemManager itemManager;
    CookController cookController;
    ScoreController scoreController;

    int currentItemAmount;
    string dropItemName;
    Vector3 itemSpawnPos = Vector3.zero;

    InventoryManager IM;



    int currentSlotIndex;

    bool _isDishSlotFull
    {
        get { return RoomManager.instance.isDishSlotFull; }
        set { RoomManager.instance.isDishSlotFull = value; }
    }

    private void Start()

    {
        scoreController = this.GetComponent<ScoreController>();
        playerPos = this.gameObject.transform;
        playerIK = GetComponentInChildren<PlayerIK>();
        gameUI = GameObject.FindWithTag("GameUI");
        slots = gameUI.GetComponentsInChildren<InventorySlot>();
        IM = GameObject.FindObjectOfType<InventoryManager>();
        weapons = this.gameObject.GetComponentsInChildren<WeaponBase>();
        dishes = this.gameObject.GetComponentsInChildren<DishBase>();
        PV = this.gameObject.GetPhotonView();
        cookController = this.GetComponent<CookController>();

        //weapons[0].gameObject.AddComponent<ProjectileWeapon>();
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < dishes.Length; i++)
        {
            dishes[i].gameObject.SetActive(false);
        }
       


    }
    private void Awake()
    {
        playerIK = GetComponentInChildren<PlayerIK>();
        if (instance == null) // Maybe???
        {
            instance = this;
        }
        itemManager = ItemManager.instance;



    }


    private void Update()
    {
        if (PV.IsMine)
        {

            if (slots[2].currentItem != null)
            {
                _isDishSlotFull = true;
            }
            else
            {
                _isDishSlotFull = false;
            }

            //Scale up currently selected slot
            for (int i = 0; i < slots.Length; i++)
            {
                if (i == currentSlotIndex)//&& slots[i].currentItem != null
                {
                    slots[i].transform.localScale = Vector3.one * 1.1f;
                    slots[currentSlotIndex].transform.GetChild(1).GetComponent<Image>().enabled = true;

                }

                else
                {
                    slots[i].transform.localScale = Vector3.one;
                    slots[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
                }

            }



            SwitchItem();



        }
        if (lastItem != slots[currentSlotIndex].currentItem)
        {
            lastItem = slots[currentSlotIndex].currentItem;
            if (PV.IsMine)
            {
                // 搞定手
                //Instantiate equip item if currentItem type = Hand
                if (slots[currentSlotIndex].currentItem != null/*&& slots[currentSlotIndex].currentItem.type == Item.Type.Weapons*/)
                {
                    playerIK.isEquipWeapon = true;


                    // 搞定手上的東西

                    if (slots[currentSlotIndex].currentItem.type == Item.Type.weapons || slots[currentSlotIndex].currentItem.type == Item.Type.dish)
                    {
                        SendEquipItemRPC(slots[currentSlotIndex].currentItem.Id);
                    }
                   
                    
                    //Debug.LogError(slots[currentSlotIndex].currentItem.Id);


                    scoreController.currentDishId = slots[currentSlotIndex].currentItem.Id;
                    playerIK.currentWeaponId = slots[currentSlotIndex].currentItem.Id;
                    cookController.currentItemIndex = slots[currentSlotIndex].currentItem.Id;

                }
                else if (slots[currentSlotIndex].currentItem == null)
                {
                    SendEquipItemRPC(-1);
                    scoreController.currentDishId = -1;
                    playerIK.currentWeaponId = -1;
                    cookController.currentItemIndex = -1;
                    playerIK.isEquipWeapon = false;
                }
            }



        }
        if (Input.GetKeyDown(KeyCode.Q) && slots[currentSlotIndex].currentItem != null)
        {
            if (PV.IsMine)
            {
                DropItem(slots[currentSlotIndex].currentItem, slots[currentSlotIndex].currentItemAmount);
                photonView.RPC("DropFX", RpcTarget.All, itemSpawnPos);
                IM.RemoveCurrentItem(currentSlotIndex, slots[currentSlotIndex].currentItem, slots[currentSlotIndex].currentItemAmount);


            }






        }




    }

    bool isISendEquipItemRPC = false;
    int tempSendEquipItemRPCID = -999;
    float lastSendEquipItemRPCTime = 0f;
    void SendEquipItemRPC(int id)
    {
        tempSendEquipItemRPCID = id;
        EquipItem(id);
        if (isISendEquipItemRPC == false)
        {
            isISendEquipItemRPC = true;
            StartCoroutine(ISendEquipItemRPC());
        }
    }
  
    IEnumerator ISendEquipItemRPC()
    {
        while(true)
        {
            // 如果有ID要送
            if (tempSendEquipItemRPCID != -999)
            {
                // 檢查時間要不要卡
                while (Time.time - lastSendEquipItemRPCTime < 0.5f)
                {
                    yield return null;
                }
                // 送出
                lastSendEquipItemRPCTime = Time.time;
                photonView.RPC("EquipItem", RpcTarget.Others, tempSendEquipItemRPCID);
               
                // 重置ID
                tempSendEquipItemRPCID = -999;
            }
            yield return null;
        }
    }

   

   



    

    float lastChangeCurrentSlotIndexTime = 0f;
    [SerializeField] float currentSlotIndexCD = 0.1f;
    private void SwitchItem()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            if (currentSlotIndex >= slots.Length - 1)
                currentSlotIndex = 0;
            else
                currentSlotIndex++;
        }
        else if (Input.mouseScrollDelta.y > 0)
        {
            if (currentSlotIndex <= 0)
                currentSlotIndex = slots.Length - 1;
            else
                currentSlotIndex--;
        }

    }
    public void WeaponUse()
    {
        IM.RemoveCurrentItem(currentSlotIndex, slots[currentSlotIndex].currentItem, 1);
    }
    [PunRPC]
    private void DropFX(Vector3 spawnPos)
    {
        Instantiate(dropFX, spawnPos, Quaternion.identity);
    }
    [PunRPC]
    private void EquipItem(int itemId)
    {
        // 無腦關手上的東西
        
        unEquipItem();
       
        if (itemId != -1)
        {
            if (itemId < 9)
            {
                // 如果這欄位有東西就拿出來
                weapons[itemId].gameObject.SetActive(true);
                currentEquip = weapons[itemId].gameObject;
            }
            else
            {
                dishes[itemId - 10].gameObject.SetActive(true);
                currentEquip = dishes[itemId - 10].gameObject;
            }
        }

    }
  
    private void unEquipItem()
    {
        if (currentEquip)
            currentEquip.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.tag == "Weapon" || collision.gameObject.tag == "Dish")
            {
                ItemPickUp itemPick = collision.transform.GetComponent<ItemPickUp>();
                int remaining = InventoryManager.AddItemToInventory(itemPick._item, itemPick.itemAmount);

                if (remaining > 0)
                {
                    itemPick.itemAmount = remaining;
                }
                else
                {
                    itemPick.KillMe();
                }
            }
        }

    }


    public void DropItem(Item item, int amount, bool removeCurrentItem = true)
    {

        if (item == null)
            return;
        Vector3 random = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(0f, 0.2f), Random.Range(-0.2f, 0.2f));
        Vector3 direction = playerPos.forward + random;
        itemSpawnPos = playerPos.position + direction * 5;



        GameObject dropTemp = PhotonNetwork.Instantiate(item.name, itemSpawnPos, Quaternion.identity);
        ItemPickUp tempItemPickUp = dropTemp.GetComponent<ItemPickUp>();
        // 要求該物件改變(該物件自己同步)
        tempItemPickUp.SetUpPickupable(item.name, amount);
        tempItemPickUp.StartCoroutine(tempItemPickUp.countDownToDetroy());


        if (removeCurrentItem)
        {
            currentItem = null;
            currentItemAmount = 0;

        }


    }

}

