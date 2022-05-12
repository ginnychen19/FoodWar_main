using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class ProjectileWeapon : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject bullet;
   
    float shootForce = 100;
    [SerializeField] float upwardForce;
    [SerializeField] float timeBetweenShooting;
    [SerializeField] float spread;
    [SerializeField] float timeBetweenShoot;
    [SerializeField] Transform firePoint;
    [SerializeField] Camera cam;

    private new void OnEnable()
    {
        if (photonView.IsMine)
        {
            CrossHairUI.instance.gameObject.SetActive(true);
        }
        
       
    }
    private new void OnDisable()
    {
        if (photonView.IsMine)
        {
            CrossHairUI.instance.gameObject.SetActive(false);
        }
        
    }

    [SerializeField] CrossHair crossHair;
    bool _isStun
    {
        get { return RoomManager.instance.isStun; }
    }
    bool _isCookUiOpen
    {
        get { return RoomManager.instance.isCookUiOpen; }
    }
    bool readyToShoot;

    private void Start()
    {
        cam = Camera.main;
        crossHair = cam.gameObject.GetComponentInChildren<CrossHair>();
        readyToShoot = true;
        
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            ShootInput();
           
        }
        
    }

    void ShootInput()
    {
        if (Input.GetMouseButtonDown(0) && readyToShoot && !_isCookUiOpen && !_isStun && !RoomManager.instance.isPause)
        {
            //Debug.LogError("shoot");
            Shoot();
        }
    }

    void Shoot()
    {
        readyToShoot = false;
        Vector3 targetPoint = crossHair.gameObject.transform.position;

        Vector3 shootDir = targetPoint - firePoint.position;

        //GameObject currentBullet = Instantiate(bullet, firePoint.position, Quaternion.identity);
        //currentBullet.transform.forward = shootDir.normalized;
        // currentBullet.GetComponent<Rigidbody>().AddForce(shootDir.normalized * shootForce, ForceMode.Impulse);
        // 0~255
        firePoint.LookAt(targetPoint);
        photonView.RPC("RPCShoot", RpcTarget.All, firePoint.position, firePoint.rotation.eulerAngles);

        HotBar.instance.WeaponUse();
        Invoke("ResetShoot", 0.1f);
    }

    [PunRPC]
    public void RPCShoot(Vector3 startPos, Vector3 startDir, PhotonMessageInfo info)
    {
        GameObject temp = Instantiate(bullet, startPos, Quaternion.Euler(startDir));
        Bullet script = temp.GetComponent<Bullet>();
        // �D�X�T���o�e�챵�����ɶ�
        script.gameObject.AddComponent<PhotonView>();
        double lag = PhotonNetwork.Time - info.SentServerTime;
        script.speed = shootForce;
        script.LagMove(lag, this.transform.root.GetInstanceID());
        
    }

    void ResetShoot()
    {
        readyToShoot = true;
    }
}
