using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGive : MonoBehaviour
{
    [SerializeField] Item item;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PickUp();
        }
    }
    int dropAmount;
    private void PickUp()
    {
        dropAmount = item.dropAmount;
        int remaining = InventoryManager.AddItemToInventory(item, dropAmount);
        if (remaining > 0 && remaining == dropAmount)
        {

            dropAmount = remaining;


        }
        else
        {


        }
    }
}
