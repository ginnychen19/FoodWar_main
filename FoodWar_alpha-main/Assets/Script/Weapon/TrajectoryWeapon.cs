using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class TrajectoryWeapon : MonoBehaviour
{
    
    [SerializeField] Transform launchPoint;
    [SerializeField] float force = 10f;
    [SerializeField] float flySpeed = 1f;

    [SerializeField] Vector3 launchToPos;
    [SerializeField] PhotonView PV;
    Vector3 randomPosOffset = Vector3.zero;
    [SerializeField] TrajectoryManager tm;
    bool launch;



    bool _isStun
    {
        get { return RoomManager.instance.isStun; }
    }
    bool _isCookUiOpen
    {
        get { return RoomManager.instance.isCookUiOpen; }
    }
    private void Start()
    {
        tm = GetComponent<TrajectoryManager>();
        PV = this.gameObject.GetPhotonView();
        randomPosOffset = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5));


    }
    private void Update()
    {
        if (PV.IsMine)
        {
            ShootInput();
        }
       
    }


    private void ShootInput()
    {

        AimState state = GetAimState();
        if (!_isCookUiOpen && !_isStun && !RoomManager.instance.isPause)
        {
            if (state == AimState.Start)
            {
                launchToPos = CrossHair.instance.transform.position;
                tm.CheckVector(launchToPos);
            }
            if (state == AimState.Move)
            {
                launchToPos = CrossHair.instance.transform.position;
                tm.CheckVector(launchToPos);
            }
            if (state == AimState.Ended)
            {
                launchToPos = CrossHair.instance.transform.position;
                tm.CheckVector(launchToPos);
                tm.line.positionCount = 0;
            }
            if (Input.GetMouseButtonDown(0) && state == AimState.Move)
            {

                tm.ShootObj(launchToPos);
                HotBar.instance.WeaponUse();



            }
            else if (Input.GetMouseButtonDown(0) && state == AimState.None)
            {
                launchToPos = CrossHair.instance.transform.position;
                tm.ShootObj(launchToPos + randomPosOffset);
                HotBar.instance.WeaponUse();

            }
        }

    }


    private AimState GetAimState()
    {

        if (Input.GetMouseButtonDown(1)) { return AimState.Start; }
        if (Input.GetMouseButton(1)) { return AimState.Move; }
        if (Input.GetMouseButtonUp(1)) { return AimState.Ended; }
        else
        {
            return AimState.None;
        }




    }

    

    private enum AimState
    {
        Start = 0,
        Move = 1,
        Stay = 2,
        Ended = 3,

        None = 9
    }
}
