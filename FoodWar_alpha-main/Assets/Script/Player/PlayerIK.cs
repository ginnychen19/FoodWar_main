using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
//[RequireComponent(typeof(PhotonView))]
public class PlayerIK : MonoBehaviourPunCallbacks
{
    [SerializeField] Animator animator;
    [SerializeField] Vector3 lookAt = Vector3.zero;


    [Range(0, 1)]
    [SerializeField] float weight = 1;
    [Range(0, 1)]
    [SerializeField] float bodyWeight = 0.5f;
    [Range(0, 1)]
    [SerializeField] float headWeight = 0.5f;
    [Range(0, 1)]
    public float rightHandWeight;
    public float leftHandWeight;
    [SerializeField] Transform rHand;
    [SerializeField] Transform rHint;
    [SerializeField] Transform lHand;
    [SerializeField] Transform lHint;


    [SerializeField] Vector3 gunPos;
    [SerializeField] Quaternion gunRot;
    [SerializeField] Vector3 gunHint;

    [SerializeField] Vector3 bombPos;
    [SerializeField] Quaternion bombRot;
    [SerializeField] Vector3 bombHint;

    [SerializeField] Vector3 dishPos;
    [SerializeField] Quaternion dishRot;
    [SerializeField] Vector3 dishHint;

    [SerializeField] PhotonView PV;

    public int currentWeaponId
    {
        get { return _currentWeaponId; }
        set
        {
            _currentWeaponId = value;
            if (PV.IsMine)
                SendWeaponIdRPC(_currentWeaponId);
        }
    }
    int _currentWeaponId = -1;

    public bool isIKActive;
 
    public bool isEquipWeapon;


    bool isISendWeaponIdRPC = false;
    int tempSendWeaponRPCID = -999;
    float lastSendWeaponIdRPCTime = 0f;
    private void SendWeaponIdRPC(int weaponId)
    {
        tempSendWeaponRPCID = weaponId;
       
        if (isISendWeaponIdRPC == false)
        {
            isISendWeaponIdRPC = true;
            StartCoroutine(ISendWeaponIdRPC());
        }
        
    }
    
    IEnumerator ISendWeaponIdRPC()
    {
        while (true)
        {
            // �p�G��ID�n�e
            if (tempSendWeaponRPCID != -999)
            {
                // �ˬd�ɶ��n���n�d
                while (Time.time - lastSendWeaponIdRPCTime < 0.5f)
                {
                    yield return null;
                }
                // �e�X
                lastSendWeaponIdRPCTime = Time.time;
                
                photonView.RPC("RPCcurrentWeaponId", RpcTarget.Others, tempSendWeaponRPCID);
              

                // ���mID
                tempSendWeaponRPCID = -999;
            }
            yield return null;
        }
    }
    private void Awake()
    {
        isIKActive = true;
        
        //PV = this.gameObject.GetPhotonView();
        animator = GetComponent<Animator>();

        currentWeaponId = -1;


    }

    private void Update()
    {

        lookAt = CrossHair.instance.transform.position;


    }

    private void OnAnimatorIK(int layerIndex)
    {


        if (isIKActive)
        {

            WeaponIK();

        }






    }

    private void lookAtIK()
    {
        animator.SetLookAtPosition(lookAt);
        animator.SetLookAtWeight(weight, bodyWeight, headWeight);
    }


    public void WeaponIK()
    {



        if (currentWeaponId == 0)
        {
            rHand.localPosition = gunPos;
            rHand.localRotation = gunRot;
            rHint.localPosition = gunHint;
            rightHandWeight = 0.8f;
            leftHandWeight = 0;

        }
        if (currentWeaponId == 1)
        {
            rHand.localPosition = bombPos;
            rHand.localRotation = bombRot;
            rHint.localPosition = bombHint;
            rightHandWeight = 1f;
            leftHandWeight = 0.8f;
        }
        if (currentWeaponId == 2)
        {
            rHand.localPosition = gunPos;
            rHand.localRotation = gunRot;
            rHint.localPosition = gunHint;
            rightHandWeight = 0.8f;
            leftHandWeight = 0;
        }
        if (currentWeaponId > 9)
        {
            rHand.localPosition = dishPos;
            rHand.localRotation = dishRot;
            rHint.localPosition = dishHint;
            rightHandWeight = 1f;
            leftHandWeight = 0f;
        }
        if (currentWeaponId == -1)
        {
            rightHandWeight = 0;
            leftHandWeight = 0;
           

        }
        #region RightHand
        animator.SetIKPosition(AvatarIKGoal.RightHand, rHand.position);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);

        animator.SetIKHintPosition(AvatarIKHint.RightElbow, rHint.position);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, rightHandWeight);

        animator.SetIKRotation(AvatarIKGoal.RightHand, rHand.rotation);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeight);
        #endregion
        #region LeftHand
        animator.SetIKPosition(AvatarIKGoal.LeftHand, lHand.position);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);

        animator.SetIKRotation(AvatarIKGoal.LeftHand, lHand.rotation);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);

        animator.SetIKHintPosition(AvatarIKHint.LeftElbow, lHint.position);
        animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, leftHandWeight);
        #endregion
    }

    [PunRPC]
    public void RPCcurrentWeaponId(int v)
    {
        currentWeaponId = v;
    }
}
