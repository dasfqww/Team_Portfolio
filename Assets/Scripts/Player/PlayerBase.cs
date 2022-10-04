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

    //������ ���� probably?
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
    [SerializeField] protected float curHp; // UI ü�¹� �׽�Ʈ�� �ϴ��� ��ӹ޾ҽ��ϴ�. by ����
    [SerializeField] protected float maxHp = 50.0f; // UI ü�¹� �׽�Ʈ�� �ϴ��� ��ӹ޾ҽ��ϴ�. by ����
    [SerializeField] protected float curArmor;
    [SerializeField] protected float maxArmor = 50.0f;
    [SerializeField] protected float curShield;
    [SerializeField] protected float maxShield = 50.0f;
    [SerializeField] protected float maxTotalHp;
    [SerializeField] protected float curTotalHp;
    [SerializeField] protected float curUltimateGage;
    [SerializeField] protected float maxUltimateGage;
    [SerializeField] protected PlayerType playerType;

    

    [SerializeField] protected float attackRate;//���� ������ ��� ���� ������ ����

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

    public virtual void SetupCharacter()//���⼭ ĳ������ ������ ������ ��
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
        if (isCrouch)//���� ���¿��� ������ �������� ����
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
        //�¿�
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotY = new Vector3(0, yRot, 0) * lookSensitivity;
        rigidbody.MoveRotation(rigidbody.rotation*Quaternion.Euler(characterRotY));
    }

    void CamRoatation()
    {
        //����
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

        if (curShield > 0)//��ȣ���� ������ ��
        {

            if ((damage > curShield) && (curShield > 0))//��ȣ���� �����ϰ� �޴� �������� ���� ��ȣ�� �纸�� Ŭ��
            {
                if (curArmor > 0)// ���� �����Ҷ�
                {
                    //TODO:���� ���� ����


                    if (damage > (curArmor + curShield))//�޴� �������� ���� ��+���� ��ȣ���� ������ Ŭ ��
                    {
                        curHp -= damage - (curArmor + curShield);
                        curArmor = 0;
                        curShield = 0;

                    }
                    else//�׷��� ���� ��
                    {
                        curArmor -= (damage - curShield);
                        curShield = 0;
                        //curTotalHp -= takeDamage;
                    }
                }

                else if (curArmor <= 0)//��ȣ���� ���������� ���� �������� ���� ��
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

        else if (curArmor > 0 && curShield <= 0)//���� �����ϳ� ���尡 �������� ���� ��
        {
            //TODO:���� ���� ����


            if (damage > curArmor && curArmor > 0)//���� �����ϴµ� �޴� �������� ���� ������ Ŭ��
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

        if (curHp == maxHp && curArmor == maxArmor)//ü�°� ���� ���� �� á�� ��
            return;

        if (curHp != maxHp || curArmor != maxArmor)//ü���̳� ���� �ִ밡 �ƴ� ��
        {
            if (curHp == maxHp && curArmor != maxArmor)//ü���� ���������� ���� �ִ�ġ �ƴ� ��
            {
                curArmor += heal;
                if (curArmor >= maxArmor)//����ġ�� �Ѱ����� �ʴ´�
                {
                    curArmor = maxArmor;
                }
            }

            else if (curHp != maxHp && curArmor == 0)//���� ������ ü���� �ִ밡 �ƴ� ��(���� �����ؾ��ϴ� ������ ���)
            {
                if (heal > (maxHp - curHp))//�޴� ���� (�ִ� ü�� - ���� ü��)���� Ŭ ��
                {
                    curHp = maxHp;
                    curArmor += (maxHp - curHp);
                }
                else//�ƴ� ���
                {
                    curHp += heal;
                }
            }

            else if (curHp != maxHp && maxArmor == 0)//�ִ� ���� 0�ε� �ִ�ü���� �ƴ� ��(���� �������� �ʴ� ������ ���)
                curHp += heal;

            curTotalHp += heal;
            if (curTotalHp >= maxTotalHp)//�ִ� ü���� �����ʰ� ��
            {
                curTotalHp = maxTotalHp;
            }
        }
    }
}
