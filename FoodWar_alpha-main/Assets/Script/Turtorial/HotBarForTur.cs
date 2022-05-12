using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotBarForTur : MonoBehaviour
{
    public static HotBarForTur instance;

    public InventorySlot[] slots = null;
    [SerializeField] Transform playerPos = null;
    [SerializeField] WeaponBase[] weapons = null;
    [SerializeField] DishBase[] dishes = null;
    [SerializeField] ParticleSystem dropFX = null;
    [SerializeField] Movement player;

    GameObject currentEquip;
    Item lastItem;
    Item currentItem;
    ItemManager itemManager;

    int currentItemAmount;
    string dropItemName;
    Vector3 itemSpawnPos = Vector3.zero;

    [SerializeField] InventoryManager IM;
    public int currentSlotIndex;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }
    bool _canPick
    {
        get { return TurCooker.instance.canPick; }
    }

    private void Start()
    {
        player = GetComponent<Movement>();
        
        weapons = this.gameObject.GetComponentsInChildren<WeaponBase>();
        dishes = this.gameObject.GetComponentsInChildren<DishBase>();
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < dishes.Length; i++)
        {
            dishes[i].gameObject.SetActive(false);
        }
       
    }
    bool is25Done;
    private void Update()
    {
        if (SquenceManager.instance.sequenceIndex >= 25)
        {
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


            if (lastItem != slots[currentSlotIndex].currentItem)
            {
                lastItem = slots[currentSlotIndex].currentItem;

                //Destroy previously equipped item
                if (currentEquip)
                {
                    currentEquip.gameObject.SetActive(false);
                }
                //Instantiate equip item if currentItem type = Hand
                if (slots[currentSlotIndex].currentItem != null/*&& slots[currentSlotIndex].currentItem.type == Item.Type.Weapons*/)
                {
                    if (slots[currentSlotIndex].currentItem.type == Item.Type.weapons)
                    {
                        currentEquip = weapons[slots[currentSlotIndex].currentItem.Id].gameObject;
                        weapons[slots[currentSlotIndex].currentItem.Id].gameObject.SetActive(true);
                    }
                    if (slots[currentSlotIndex].currentItem.type == Item.Type.dish)
                    {
                        currentEquip = dishes[slots[currentSlotIndex].currentItem.Id - 10].gameObject;
                        dishes[slots[currentSlotIndex].currentItem.Id - 10].gameObject.SetActive(true);
                    }

                    player.currentWeaponId = slots[currentSlotIndex].currentItem.Id;

                    if (currentSlotIndex == 2 && !is25Done)
                    {
                        SquenceManager.instance.NextSequence(SquenceManager.instance.sequenceIndex);
                        is25Done = true;
                    }

                }
                else
                {
                    player.currentWeaponId = -1;
                }

               
            }
        

        }

        if (Input.GetKeyDown(KeyCode.Q) && slots[currentSlotIndex].currentItem != null)
        {

        }

        PickUpDish();


    }

    public void WeaponUse()
    {
        
       
        IM.RemoveCurrentItem(currentSlotIndex, slots[currentSlotIndex].currentItem, 1);
    }
    private void PickUpDish()
    {
        if (_canPick && Input.GetKeyDown(KeyCode.E) && SquenceManager.instance.sequenceIndex == 17)
        {
            TurCooker.instance.resultDish.SetActive(false);
            InventoryManager.AddItemToInventory(ItemManager.instance.GetMaterialById(36), 1);
            SquenceManager.instance.NextSequence(SquenceManager.instance.sequenceIndex);
        }
    }
}

