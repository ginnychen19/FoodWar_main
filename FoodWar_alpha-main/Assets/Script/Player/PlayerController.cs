using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;

public partial class PlayerController : MonoBehaviourPunCallbacks
{
    // -----------FX-----------------------
    [SerializeField] AudioSource SFXPlayer;
    [SerializeField] AudioClip sprintSFX;
    [SerializeField] AudioClip jumpSFX;
    [SerializeField] AudioClip stunSFX;
    [SerializeField] ParticleSystem stunFx;
    [SerializeField] ParticleSystem dashHitFx;
    [SerializeField] ParticleSystem dashingFx;
    [SerializeField] ParticleSystem jumpFx;



    //-----------------------------------------
    [SerializeField] Animator ani = null;
    [SerializeField] Rigidbody rb = null;
    [SerializeField] CapsuleCollider characterCollider = null;
    [SerializeField] CookUI cookUI = null;
    [SerializeField] SkillCoolDown skillCoolDown;
    [SerializeField] Transform camLookAt = null;
    [SerializeField] PlayerIK playerIK;
    [SerializeField] CinemachineVirtualCamera Vcam = null;
    [SerializeField] Vector3 playerMoveInput = Vector3.zero;
    [SerializeField] float velocity = 10;
    [SerializeField] Vector3 moveDir = Vector3.zero;
    //[SerializeField] float MOUSE_SENTIVY = 10f;
    [SerializeField] float syncRate = 10f;
    [SerializeField] float distanceToSync = 0.1f;
    [SerializeField] float jumpForce = 0;
    [SerializeField] float sprintForce = 0;
    [SerializeField] float raycastDistance = 0;
    [SerializeField] float sprintTime;
    [SerializeField] float cdTime = 10;
    [SerializeField] float stunTime = 2f;
    [SerializeField] int currentConnections;
    [SerializeField] TextMesh floatingId;
    [SerializeField] SpriteRenderer nameSlot;
    [SerializeField] float R, G, B;
    [SerializeField] CookController cookController;


    [SerializeField] LayerMask layerMask;

    float stunRemainTime = -1;
  


    public List<GameObject> characters;
    bool jump;
    bool sprint;
    public bool isSprinting;
    bool isStun;
    public int _characterId;
    float fallMutiplier = 6f;

    bool isSkillOk = true;

    



    public FoodTeam teamValue;

    Vector3 nameSlotPos;

    float angle = 0;
    Color rayColor;

    Vector3 angles = Vector3.zero;

    float rpcY = 0f;
    Vector3 lastPos = Vector3.zero;


    Vector2 lastAnim = Vector2.zero;




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

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {

            Gizmos.color = isGrounded() ? Color.red : Color.green;
            Gizmos.DrawWireSphere(sphere, characterCollider.radius);
        }
       
    }
   
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError(cause.ToString());
    }
    private void Start()
    {
        RoomManager.instance.點名();
        Cursor.lockState = CursorLockMode.Locked;
        if (photonView.IsMine)
        {

            CrossHair.instance.gameObject.transform.position = this.gameObject.transform.forward;
            cookController = this.GetComponent<CookController>();
            skillCoolDown = GameObject.FindObjectOfType<SkillCoolDown>();

            _characterId = SaveManager.instance.nowData.characterID;
            if (_characterId <= 4)
            {
                teamValue = FoodTeam.GOOD;
                R = 0.3764706f;
                G = 0.5686275f;
                B = 0.4470588f;
                nameSlotPos = new Vector3(0, 5.3f, 0);
                stunFx.gameObject.transform.localPosition = new Vector3(0, 4, 0);

            }
            else if (_characterId >= 5)
            {
                teamValue = FoodTeam.BAD;
                R = 0.9137255f;
                G = 0.282353f;
                B = 0.2745098f;
                nameSlotPos = new Vector3(0, 4.5f, 0);
                stunFx.gameObject.transform.localPosition = new Vector3(0, 4, 0);
            }

            photonView.RPC("SetCharacter", RpcTarget.All, _characterId);//_characterId
            photonView.RPC("SetPlayerName", RpcTarget.All, SaveManager.instance.nowData.playerName, R, G, B, nameSlotPos);

            _characterId = SaveManager.instance.nowData.characterID;
            Vcam = FindObjectOfType<CinemachineVirtualCamera>();
            playerIK = this.gameObject.GetComponentInChildren<PlayerIK>();
            camLookAt = this.gameObject.transform.Find("camLookAt");


            if (SaveManager.instance.nowData.characterID <= 4)// if team == green
            {
                camLookAt.transform.localPosition = new Vector3(0, 3.5f, 0);
            }
            else if (SaveManager.instance.nowData.characterID >= 5)// if team == red 
            {
                camLookAt.transform.localPosition = new Vector3(0, 3, 0);
            }


        }
        if (!photonView.IsMine)
        {
            rb.isKinematic = true;
        }

    }








    //PlayerInput;
    private void Update()
    {


      

      

        if (photonView.IsMine && _isGameStart && !RoomManager.instance.isPause)
        {
            InputMagnitude(_isStun);
            
            
            Jump();
            Sprint();

        





        }
        else if(photonView.IsMine && RoomManager.instance.isPause)
        {
            //playerMoveInput = new Vector3(0, rb.velocity.y, 0);
            rawInput = Vector2.zero;
            _yaw = 0;
            _pitch = 0;
        }
        if (photonView.IsMine)
        {
            UpdateOilBlindAnim();
        }
        else
        {
            // 鏡像接受旋轉
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0f, rpcY, 0f), Time.deltaTime * 10f);
        }

        // 無論本撙逕向都吃
        playerMoveInput.x = Mathf.Lerp(playerMoveInput.x, rawInput.x, Time.deltaTime * 10f);
        playerMoveInput.z = Mathf.Lerp(playerMoveInput.z, rawInput.y, Time.deltaTime * 10f);
    }
    Vector2 rawInput = Vector2.zero;
    void InputMagnitude(bool _stun)
    {
        

        if (!_stun)
        {
            // 操作值
            rawInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            
            //= new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (!_isCookUiOpen)
            {
                _yaw = -Input.GetAxis("Mouse Y");
                _pitch = Input.GetAxis("Mouse X");
            }
           


            jump = Input.GetKeyDown(KeyCode.Space) ? true : false;
            sprint = Input.GetKeyDown(KeyCode.LeftShift) ? true : false;

        }



    }









    private void LateUpdate()
    {
        if (photonView.IsMine)
        {

            Vcam.Follow = camLookAt;
            CameraRotation();
            SyncAnime();

        }
        AnimatePlayer();
        

    }

    float 上次送出時間 = 0f;
    Vector3 上一幀的位置 = Vector3.zero;
    float lastRotation;
    
    /// <summary>
    /// Move the Character
    /// </summary>
    private void Move()
    {
        moveDir = Vector3.ClampMagnitude(transform.TransformDirection(playerMoveInput), 1) * velocity;
        //moveDir = transform.TransformDirection(playerMoveInput) * velocity;
        rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);

        // 15 / 0.3 = 50
        // 300-50*4
        // 起始位置 速率 // m/s

        // 消極的同步座標 如果本尊移動位置就同步發送給鏡像
        /*
         float d = Vector3.Distance(rb.position, lastPos);
         if (d > distanceToSync)
         {
             lastPos = rb.position;
             photonView.RPC("GetPos", RpcTarget.Others, rb.position, rb.velocity, this.transform.rotation.eulerAngles.y);
         }
        */


        Vector3 這一幀移動多少 =  ((rb.position - 上一幀的位置) != Vector3.zero)? (rb.position - 上一幀的位置) / Time.fixedDeltaTime : Vector3.zero;
        上一幀的位置 = rb.position;
        float lastRotate = this.transform.rotation.eulerAngles.y - lastRotation;


        if (這一幀移動多少 != Vector3.zero || lastRotate != 0)
        {
            if (Time.time > 上次送出時間 + 0.1f)
            {
                上次送出時間 = Time.time;

                photonView.RPC("GetPos", RpcTarget.Others, rb.position, 這一幀移動多少, this.transform.rotation.eulerAngles.y);
                lastRotation = this.transform.rotation.eulerAngles.y;
            }
        }
        else
        {
            if (Time.time > 上次送出時間 + 0.2f)
            {
                上次送出時間 = Time.time;

                photonView.RPC("GetPos", RpcTarget.Others, rb.position, 這一幀移動多少, this.transform.rotation.eulerAngles.y);
                lastRotation = this.transform.rotation.eulerAngles.y;
            }
        }
        
    }
    /// <summary>
    ///  Jump
    /// </summary>
    
    private void Jump()
    {
       
        if (jump && isGrounded())
        {

            SFXPlayer.PlayOneShot(jumpSFX, 2f);
            rb.AddForce(Vector3.up * 14, ForceMode.VelocityChange);            
            photonView.RPC("JumpAnimAndFX", RpcTarget.All);


        }
        Fall();


    }
    private void Fall()
    {
        if (rb.velocity.y < 10)
        {

            rb.velocity += Vector3.up * Physics.gravity.y * (fallMutiplier - 1) * Time.deltaTime;
        }

    }


    /// <summary>
    /// Sprint
    /// </summary>
    private void Sprint()
    {
        if (sprint && isSkillOk && !isSprinting && moveDir != Vector3.zero)
        {

            StartCoroutine(SprintCoroutine());

        }

    }
    private IEnumerator SprintCoroutine()
    {
        float startTime = Time.time;
        float originVelocity = velocity;
        SFXPlayer.PlayOneShot(sprintSFX);
        photonView.RPC("SprintFXAndAni", RpcTarget.All, true);
        isSkillOk = false;
        while (Time.time < startTime + sprintTime)
        {

            isSprinting = true;
            playerIK.isIKActive = false;
            velocity = sprintForce;





            yield return null;
            isSprinting = false;
            playerIK.isIKActive = true;

            velocity = originVelocity;

        }
        rb.velocity = Vector3.zero;
        photonView.RPC("SprintFXAndAni", RpcTarget.All, false);
        StartCoroutine(CoolDown());
    }
    [PunRPC]
    private void JumpAnimAndFX()
    {
        ani.SetTrigger("Jump");
        Instantiate(jumpFx, this.gameObject.transform.position, Quaternion.Euler(-90, 0, 0));


    }


    [PunRPC]
    private void SprintFXAndAni(bool _isSprting)
    {
        ani.SetBool("Sprint", _isSprting);
        if (_isSprting)
        {
            dashingFx.Play();
        }
        else
        {
            dashingFx.Stop();
        }

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




    [SerializeField] float 攤平順移差次數 = 3f;
    
    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {

            Move();

            PlayerRotate();

          

            
        }
        else
        {
            Vector3 這幀順移差 = (rpc順移差 != Vector3.zero)? rpc順移差 / 攤平順移差次數 : Vector3.zero;
            //rb.MovePosition(Vector3.Lerp(rb.position, rpcPos, Time.deltaTime * syncRate));
            rb.MovePosition(rb.position + (rpcVelocity * Time.fixedDeltaTime) + 這幀順移差);
            // 用掉的順移差減小
            rpc順移差 = rpc順移差 - 這幀順移差;
        }

    }
    private void SyncAnime()
    {
        float 長度 = Vector2.Distance(lastAnim, rawInput);
        if (長度 > 0.05f)
        {
            lastAnim = rawInput;
            // 同步
            photonView.RPC("SendAnim", RpcTarget.All, rawInput.x, rawInput.y);
        }
    }
    float MOUSE_SENTIVY
    {
        get { return SaveManager.instance.nowData.mouseSensitive; }
    }
    private void CameraRotation()
    {
        camLookAt.transform.rotation *= Quaternion.AngleAxis(_pitch * MOUSE_SENTIVY, Vector3.up);
        camLookAt.transform.rotation *= Quaternion.AngleAxis(_yaw * MOUSE_SENTIVY, Vector3.right);
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
    }

   
    bool isGroundedRay()
    {


        if (Physics.Raycast(characterCollider.bounds.center, Vector3.down, characterCollider.bounds.extents.y + raycastDistance))
        {

          

            return true;
        }
        else
        {
         

            return false;
        }



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


    private void PlayerRotate()
    {

        Quaternion camDir = Quaternion.Euler(0, camLookAt.transform.rotation.eulerAngles.y, 0);
        //playerPos.rotation = Quaternion.Lerp(playerPos.rotation, camDir, Time.fixedDeltaTime * 10f);

        //rb.MoveRotation(Quaternion.Lerp(rb.rotation, camDir, Time.fixedDeltaTime * 2f));

        rb.MoveRotation(camDir);
        camLookAt.transform.localEulerAngles = new Vector3(angles.x, 0, 0);

    }

    public void TakeGunDamage()
    {
        
        // 如果受傷的是本尊
        if (photonView.IsMine)
        {
            oilBlindPower += 4f;
            oilBlindPower = (float)Mathf.CeilToInt(oilBlindPower);
            PlayerHUD.instance.oilGunFX.SetFloat("OilBlindPower", oilBlindPower);
        }
     
    }
    void UpdateOilBlindAnim()
    {
        oilBlindPower -= Time.deltaTime;
        oilBlindPower = Mathf.Clamp(oilBlindPower, 0f, 18f);
    }

    float oilBlindPower
    {
        get
        {
            return PlayerHUD.instance.oilGunFX.GetFloat("OilBlindPower");
        }
        set
        {
            PlayerHUD.instance.oilGunFX.SetFloat("OilBlindPower", value);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isGameStart && photonView.IsMine)
        {
            if (other.gameObject.CompareTag("ItemBox"))
            {
                other.GetComponent<ItemBox>().RandomWeapon();
            }
            if (other.gameObject.CompareTag("SupplyBox") && MaterialSlot.instance.materialAmount < 5)
            {
                other.GetComponent<SupplyBox>().PickUpSupplyBox();
            }
        }
       
    }


    #region ApplyStun
    private void OnCollisionEnter(Collision collision)
    {
        if (isSprinting)
        {
            photonView.RPC("HitFx", RpcTarget.All);
        }
        if (isSprinting && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PhotonView>().RPC("ApplyStun", RpcTarget.All);
        }



    }
    private IEnumerator StunCountDown()
    {
        _isStun = true;
        photonView.RPC("StunFXAndAni", RpcTarget.All, _isStun);
        SFXPlayer.loop = true;
        SFXPlayer.clip = stunSFX;
        SFXPlayer.Play();
        _yaw = 0f;
        _pitch = 0f;
        float startTime = Time.time;
        rawInput = Vector2.zero;


        while (Time.time < startTime + stunTime)
        {
            stunRemainTime = (startTime + stunTime) - Time.time;

            yield return null;


        }
        stunRemainTime = 0;
        _isStun = false;
        SFXPlayer.loop = false;
        SFXPlayer.clip = null;
        SFXPlayer.Stop();
        photonView.RPC("StunFXAndAni", RpcTarget.All, _isStun);




    }


    #endregion



    #region RPC Funtions

   

    [PunRPC]
    private void StunFXAndAni(bool r_isStun)
    {
        ani.SetBool("Stun", r_isStun);
        if (r_isStun)
        {
            stunFx.Play();
        }
        else
        {
            stunFx.Stop();
        }
    }
    [PunRPC]
    private void HitFx()
    {
        Instantiate(dashHitFx, gameObject.transform.position + new Vector3(0, 3, 0), Quaternion.identity);
    }


    [PunRPC]
    public void ApplyStun()
    {
        if (photonView.IsMine)
        {
            if (HotBar.instance.slots[2].currentItem != null)
            {
                HotBar.instance.DropItem(HotBar.instance.slots[2].currentItem, 1);
                InventoryManager.instance.RemoveCurrentItem(2, HotBar.instance.slots[2].currentItem, 1);
            }
            
            StartCoroutine(StunCountDown());

        }


    }
    Vector3 rpcVelocity;

    Vector3 rpc順移差 = Vector3.zero;

    [PunRPC]
    public void GetPos(Vector3 pos, Vector3 velocity, float y, PhotonMessageInfo info)
    {
        // 從什麼地方 向哪邊移動
        double lag = PhotonNetwork.Time - info.SentServerTime;
        rpcVelocity = velocity;
        rpc順移差 = pos - rb.position;
        //rb.position = pos;
        rpcY = y;
    }
    [PunRPC]
    public void SetCharacter(int cid)
    {
        characters[cid].SetActive(true);
        ani = characters[cid].GetComponent<Animator>();
        characterCollider = characters[cid].GetComponent<CapsuleCollider>();
        stunFx.Stop();
        dashingFx.Stop();
    }
    [PunRPC]
    public void SetPlayerName(string playerName, float r, float g, float b, Vector3 namePos)
    {
        nameSlot.transform.localPosition = namePos;
        floatingId.text = playerName;
        nameSlot.color = new Color(r, g, b, 1);


    }
    [PunRPC]
    public void SendAnim(float animX, float animZ)
    {
        rawInput.x = animX;
        rawInput.y = animZ;
        //ani.SetFloat("MoveX", animX);
        //ani.SetFloat("MoveZ", animZ);
    }
    
    
    
    #endregion
}
