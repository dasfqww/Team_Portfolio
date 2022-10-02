using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerBase : MonoBehaviour, IDamageAndHealable
{
    private Rigidbody rigidbody;

    protected PhotonView photonView;

    [SerializeField] Transform camPos;

    //삭제될 예정 probably?
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected Transform firePos;

    [SerializeField] protected ParticleSystem hitEffect;
    [SerializeField] protected ParticleSystem healEffect;

    protected enum PlayerType { Attack, Tank, Support }
    protected enum PlayerState { Idle, Attack, Moving, Dead}

    [Header("Character Stat")]
    [SerializeField] protected float dealAmount;
    [SerializeField] protected float healAmount;
    [SerializeField] protected int curAmmo;
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected float curHp; // UI 체력바 테스트를 하느라 상속받았습니다. by 혜원
    [SerializeField] protected float maxHp = 50.0f; // UI 체력바 테스트를 하느라 상속받았습니다. by 혜원
    [SerializeField] protected float curArmor;
    [SerializeField] protected float maxArmor = 50.0f;
    [SerializeField] protected float curShield;
    [SerializeField] protected float maxShield = 50.0f;
    [SerializeField] protected float maxTotalHp;
    [SerializeField] protected float curTotalHp;
    [SerializeField] protected float curUltimateGage;
    [SerializeField] protected float maxUltimateGage;
    [SerializeField] protected PlayerType playerType;

    

    [SerializeField] protected float attackRate;//연사 공격의 경우 공격 간격을 설정

    [Header("Movement")]
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float lookSensitivity;
    [SerializeField] float crouchSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] protected bool isDead = false;

    WaitForSeconds delayTime;

    float camRotLimit=90.0f;
    float curCamRotX;

    bool isCrouch = false;
    bool isGround = true;

    [SerializeField] float crouchPosY;
    float originPosY;
    float applyCrouchPosY;

    private CapsuleCollider capsuleCollider;

   

    // Start is called before the first frame update
    void Awake()
    {
        maxTotalHp = SetTotalHp();
        curTotalHp = maxTotalHp;
        curHp = maxHp;
        curArmor = maxHp;
        delayTime = new WaitForSeconds(1.0f);
        capsuleCollider = GetComponent<CapsuleCollider>();
        rigidbody = GetComponent<Rigidbody>();
        originPosY = camPos.localPosition.y;
        applyCrouchPosY = originPosY;
        //rigidbody.freezeRotation = true;
        //rotDelta = rotSpeed * Time.deltaTime;
    }

    public virtual void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine == true)
        {
            camPos.gameObject.SetActive(true);
        }
        persistPassive();
    }

    float SetTotalHp() { return maxHp + maxArmor + maxShield; }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine==true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                photonView.RPC("useLeftClick", RpcTarget.All);
            }

            if (Input.GetMouseButtonDown(1))
            {
                photonView.RPC("useRightClickSkill", RpcTarget.All);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                useESkill();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                useShiftSkill();
            }

            TryJump();
            TryCrouch();
            CamRoatation();
        }        
    }

     private void FixedUpdate()
     {      
         Movement();
         CharacterRotation();
     }

    public virtual void SetupCharacter()//여기서 캐릭터의 스탯을 설정할 것
    {
        if (photonView.IsMine == false)
            return;
    }

    public virtual void useShiftSkill()//shift key
    {
        if (photonView.IsMine == false)
            return;
    }

    public virtual void useESkill()//E key
    {
        if (photonView.IsMine == false)
            return;
    }

    public virtual void useLeftClick()//Left Click
    {
        if (photonView.IsMine == false)
            return;
    }

    public virtual void useRightClickSkill()//right Skill
    {
        if (photonView.IsMine == false)
            return;
    }

    public virtual void persistPassive()//passive
    {
        if (photonView.IsMine == false)
            return;
    }
  
    public virtual void useUltimateSkill()
    {
        if (photonView.IsMine == false)


        if (curUltimateGage != maxUltimateGage)
            return;        
    }

    void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    void Crouch()
    {
        isCrouch= !isCrouch;
        if (isCrouch)
        {
            moveSpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }    
        else
        {
            moveSpeed = 10.0f;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine("CrouchCoroutine");
    }

    IEnumerator CrouchCoroutine()
    {
        float posY = camPos.localPosition.y;
        int count = 0;

        while (posY!=applyCrouchPosY)
        {
            count++;
            posY = Mathf.Lerp(posY, applyCrouchPosY, 0.3f);
            camPos.localPosition = new Vector3(0, posY, 0);
            if (count > 15)
                break;            
            yield return null;
        }
        camPos.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }

    

    void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&isGround==true)
        {
            Jump();
        }
    }
    
    void Jump()
    {
        if (isCrouch)//앉은 상태에서 점프시 앉은상태 해제
            Crouch();
        rigidbody.velocity = transform.up * jumpForce;
        isGround = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Ground"||
            collision.gameObject.tag=="OccupationZone")
        {
            isGround = true;
        }
    }

    void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * h; 
        Vector3 moveVertical = transform.forward * v;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized*moveSpeed;
        rigidbody.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    void CharacterRotation()
    {
        //좌우
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotY = new Vector3(0, yRot, 0) * lookSensitivity;
        rigidbody.MoveRotation(rigidbody.rotation*Quaternion.Euler(characterRotY));
    }

    void CamRoatation()
    {
        //상하
        float xRot = Input.GetAxisRaw("Mouse Y");
        float camRotX = xRot * lookSensitivity;
        curCamRotX -= camRotX;
        curCamRotX = Mathf.Clamp(curCamRotX, -camRotLimit, camRotLimit);

        camPos.localEulerAngles = new Vector3(curCamRotX, 0, 0);
    }

    [PunRPC]
    public virtual void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        /*if (photonView.IsMine == false)
            return;*/

        if (!isDead)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            //hit Sound
        }

        if (curShield > 0)//보호막이 존재할 때
        {

            if ((damage > curShield) && (curShield > 0))//보호막이 존재하고 받는 데미지가 현재 보호막 양보다 클때
            {
                if (curArmor > 0)// 방어구가 존재할때
                {
                    //TODO:뎀감 공식 구현


                    if (damage > (curArmor + curShield))//받는 데미지가 현재 방어구+현재 보호막의 값보다 클 때
                    {
                        curHp -= damage - (curArmor + curShield);
                        curArmor = 0;
                        curShield = 0;

                    }
                    else//그렇지 않을 때
                    {
                        curArmor -= (damage - curShield);
                        curShield = 0;
                        //curTotalHp -= takeDamage;
                    }
                }

                else if (curArmor <= 0)//보호막은 존재하지만 방어구는 존재하지 않을 때
                {
                    curHp -= (damage - curShield);
                    curShield = 0;
                    //curTotalHp -= takeDamage;                   
                }
            }

            else
            {
                curShield -= damage;
                //curTotalHp -= takeDamage;
            }
            curTotalHp -= damage;
        }

        else if (curArmor > 0 && curShield <= 0)//방어구가 존재하나 쉴드가 존재하지 않을 때
        {
            //TODO:뎀감 공식 구현


            if (damage > curArmor && curArmor > 0)//방어구가 존재하는데 받는 데미지가 남은 방어구보다 클때
            {
                curHp -= (damage - curArmor);
                curArmor = 0;
            }

            else
            {
                curArmor -= damage;
                //curTotalHp -= takeDamage;
            }
            curTotalHp -= damage;
        }

        else
        {
            curHp -= damage;
            curTotalHp -= damage;
        }

        if (curHp <= 0)
        {
            curHp = 0;
            curTotalHp = 0;
            Debug.Log("Die...");
        }
    }

    [PunRPC]
    public virtual void TakeHeal(float heal)
    {
        if (photonView.IsMine)
            return;

        if (curHp == maxHp && curArmor == maxArmor)//체력과 방어구가 전부 꽉 찼을 때
            return;

        if (curHp != maxHp || curArmor != maxArmor)//체력이나 방어구가 최대가 아닐 때
        {
            if (curHp == maxHp && curArmor != maxArmor)//체력이 만땅이지만 방어구가 최대치 아닐 때
            {
                curArmor += heal;
                if (curArmor >= maxArmor)//상한치를 넘게하지 않는다
                {
                    curArmor = maxArmor;
                }
            }

            else if (curHp != maxHp && curArmor == 0)//방어구가 없으며 체력이 최대가 아닐 때(방어구가 존재해야하는 영웅일 경우)
            {
                if (heal > (maxHp - curHp))//받는 힐이 (최대 체력 - 현재 체력)보다 클 때
                {
                    curHp = maxHp;
                    curArmor += (maxHp - curHp);
                }
                else//아닌 경우
                {
                    curHp += heal;
                }
            }

            else if (curHp != maxHp && maxArmor == 0)//최대 방어구가 0인데 최대체력이 아닐 때(방어구가 존재하지 않는 영웅일 경우)
                curHp += heal;

            curTotalHp += heal;
            if (curTotalHp >= maxTotalHp)//최대 체력을 넘지않게 함
            {
                curTotalHp = maxTotalHp;
            }
        }
    }
}
