using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class SupplyBox : MonoBehaviourPunCallbacks
{
    [SerializeField] ParticleSystem pickUpFX;
    PhotonView PV;
    private new void OnDisable()
    {
        if (PhotonNetwork.IsMasterClient)
            Invoke("ReSpawnRPC", 10f);
    }
    private void Start()
    {
        PV = gameObject.GetPhotonView();
        floating();
    }
    void floating()
    {
        LeanTween.moveLocalY(gameObject, this.gameObject.transform.position.y + 1, 1).setLoopPingPong();

    }

    private void ReSpawnRPC()
    {
        photonView.RPC("Spawn", RpcTarget.All);
    }
    [PunRPC]
    private void Spawn()
    {
        this.gameObject.SetActive(true);
    }
   

    /*
    private void OnTriggerEnter(Collider other)
    {
        
            if (other.gameObject.CompareTag("Character") && MaterialSlot.instance.materialAmount < 5)
            {
                MaterialSlot.instance.addMaterialAmount();
                photonView.RPC("PickUp", RpcTarget.All);
            }
        
       
    }
    */

    public void PickUpSupplyBox()
    {
        MaterialSlot.instance.addMaterialAmount();
        photonView.RPC("PickUp", RpcTarget.All);
    }
    [PunRPC]

    public void PickUp()
    {
        this.gameObject.SetActive(false);
        Instantiate(pickUpFX, transform.position, Quaternion.identity);
    }
}
