using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class Movement : MonoBehaviour
{
    Animator ani;
    Rigidbody rb;
    [SerializeField] AudioSource SFXPlayer;
    [SerializeField] AudioClip sprintSFX;
    [SerializeField] AudioClip jumpSFX;
    //[SerializeField] Transform playerPos;
    [SerializeField] Transform camLookAt = null;
    [SerializeField] CinemachineVirtualCamera Vcam = null;
    [SerializeField] Vector3 playerMoveInput = Vector3.zero;
    [SerializeField] float yaw = 0;
    [SerializeField] float pitch = 0;
    [SerializeField] float velocity = 10;
    [SerializeField] Vector3 moveDir = Vector3.zero;
    [SerializeField] float sprintForce = 0;
    [SerializeField] float sprintTime;
    [SerializeField] float MOUSE_SENTIVY = 10f;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float cdTime = 10;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject bumb;
    [SerializeField] SkillCoolDown skillCoolDown;
    CapsuleCollider characterCollider;
    float angle = 0;
    bool sprint;
    public bool isSprinting = false;
    bool isSkillOk = true;
    Vector3 angles = Vector3.zero;
    bool jump;
    float fallMutiplier = 6f;
    [SerializeField] Vector3 lookAt = Vector3.zero;

    Quaternion nextRotation;
    public int currentWeaponId = -1;

    Vector3 sphere
    {
        get
        {
            Vector3 p;

            p = transform.position;
            p.y += characterCollider.radius;
            p.y -= 0.1f;
            return p;
        }
    }


    private void Start()
    {

        characterCollider = GetComponent<CapsuleCollider>();
        ani = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
      

       



        Vcam = FindObjectOfType<CinemachineVirtualCamera>();
        camLookAt = this.gameObject.transform.Find("camLookAt");



        camLookAt.transform.localPosition = new Vector3(0, 3, 0);










    }
    bool _canMove
    {
        get { return SquenceManager.instance.canMove; }
    }
    //PlayerInput;
    private void Update()
    {
        lookAt = CrossHair.instance.transform.position;
        if (_canMove)
        {
            InputMagnitude();
            if (SquenceManager.instance.sequenceIndex == 39)
            {
                Sprint();
            }
           
            Jump();
        }
        else
        {
            playerMoveInput = Vector3.zero;
            yaw = 0;
            pitch = 0;
        }
        

    }

    void InputMagnitude()
    {
        playerMoveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        yaw = -Input.GetAxis("Mouse Y");
        pitch = Input.GetAxis("Mouse X");
        sprint = Input.GetKeyDown(KeyCode.LeftShift) ? true : false;

        jump = Input.GetKeyDown(KeyCode.Space) ? true : false;
    }
    private void Sprint()
    {
        if (sprint && isSkillOk && !isSprinting)
        {
            SquenceManager.instance.NextSequence(SquenceManager.instance.sequenceIndex);
            //Debug.LogError("sprint");
            StartCoroutine(SprintCoroutine());
            
        }


    }
    private void Jump()
    {

        if (jump && isGrounded())
        {

            //SFXPlayer.PlayOneShot(jumpSFX, 2f);
            rb.AddForce(Vector3.up * 14, ForceMode.VelocityChange);
         


        }
        Fall();


    }
    public bool isGrounded()
    {
        if (Physics.CheckSphere(sphere, characterCollider.radius, layerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Fall()
    {
        if (rb.velocity.y < 10)
        {

            rb.velocity += Vector3.up * Physics.gravity.y * (fallMutiplier - 1) * Time.deltaTime;
        }

    }

    private IEnumerator SprintCoroutine()
    {
        float startTime = Time.time;
        float originVelocity = velocity;

        isSkillOk = false;
        while (Time.time < startTime + sprintTime)
        {

            isSprinting = true;
            velocity = sprintForce;


            yield return null;
            isSprinting = false;

            velocity = originVelocity;

        }
        StartCoroutine(CoolDown());
    }
    private IEnumerator CoolDown()
    {
        float startTime = Time.time;
        skillCoolDown.timeText.gameObject.SetActive(true);

        while (Time.time < startTime + cdTime)
        {


            skillCoolDown.timeText.text = ((startTime + cdTime) - Time.time).ToString("0");
            skillCoolDown.cdProgress.fillAmount = ((startTime + cdTime) - Time.time) / cdTime;


            yield return null;
        }
        skillCoolDown.timeText.gameObject.SetActive(false);
        isSkillOk = true;

    }
    private void LateUpdate()
    {
        Vcam.Follow = camLookAt;
        CameraRotation();
        AnimatePlayer();


    }

    private void Move()
    {
        moveDir = Vector3.ClampMagnitude(transform.TransformDirection(playerMoveInput), 1) * velocity;
        //moveDir = transform.TransformDirection(playerMoveInput) * velocity;
        rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);






    }


    private void FixedUpdate()
    {
        if (_canMove)
        {
            Move();
            PlayerRotate();
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        


    }

    private void CameraRotation()
    {
        camLookAt.transform.rotation *= Quaternion.AngleAxis(pitch * MOUSE_SENTIVY, Vector3.up);
        camLookAt.transform.rotation *= Quaternion.AngleAxis(yaw * MOUSE_SENTIVY, Vector3.right);
        angles = camLookAt.transform.localEulerAngles;
        angles.z = 0;
        angle = camLookAt.transform.localEulerAngles.x;


        //Clamp down and Up
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 45)
        {
            angles.x = 45;
        }


        camLookAt.transform.localEulerAngles = angles;
        //nextRotation = Quaternion.Lerp(camLookAt.transform.rotation, nextRotation, Time.deltaTime * 0.5f);


    }

    private void AnimatePlayer()
    {
        ani.SetFloat("MoveX", playerMoveInput.x);
        ani.SetFloat("MoveZ", playerMoveInput.z);
        if (isSprinting)
        {
            ani.SetBool("Sprint", true);
        }
        else
        {
            ani.SetBool("Sprint", false);
        }

    }

    private void PlayerRotate()
    {

        Quaternion camDir = Quaternion.Euler(0, camLookAt.transform.rotation.eulerAngles.y, 0);
        //playerPos.rotation = Quaternion.Lerp(playerPos.rotation, camDir, Time.fixedDeltaTime * 10f);

        //rb.MoveRotation(Quaternion.Lerp(rb.rotation, camDir, Time.fixedDeltaTime * 10f));

        rb.MoveRotation(camDir);
        //rb.MoveRotation(Quaternion.Slerp(rb.rotation, camDir, 4f * Time.deltaTime));
        camLookAt.transform.localEulerAngles = new Vector3(angles.x, 0, 0);

    }


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

    private void OnAnimatorIK(int layerIndex)
    {
        WeaponIK();
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
        ani.SetIKPosition(AvatarIKGoal.RightHand, rHand.position);
        ani.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);

        ani.SetIKHintPosition(AvatarIKHint.RightElbow, rHint.position);
        ani.SetIKHintPositionWeight(AvatarIKHint.RightElbow, rightHandWeight);

        ani.SetIKRotation(AvatarIKGoal.RightHand, rHand.rotation);
        ani.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeight);
        #endregion
        #region LeftHand
        ani.SetIKPosition(AvatarIKGoal.LeftHand, lHand.position);
        ani.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);

        ani.SetIKRotation(AvatarIKGoal.LeftHand, lHand.rotation);
        ani.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);

        ani.SetIKHintPosition(AvatarIKHint.LeftElbow, lHint.position);
        ani.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, leftHandWeight);
        #endregion
    }

}
