using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerMove : MonoBehaviourPunCallbacks
{
    public float speed = 5f;
    [SerializeField] float rotSpeed = 200.0f;
    [SerializeField] Transform camPos;

    [SerializeField]
    float gravity = -10.0f;
    
    float yVelocity = 0;

    [SerializeField]
    float jumpPower = 5.0f;

    bool isJumping = false;

    float rotDelta;
    
    float rotX;
    float rotY;

    float takeDamage = 30.0f;

    CharacterController characterController;

    PhotonView photonView;

    [SerializeField] bool isAwayTeam;

    [SerializeField] float curHp;
    [SerializeField] float maxHp=50.0f;
    [SerializeField] float curArmor;
    [SerializeField] float maxArmor = 50.0f;
    [SerializeField] float curShield;
    [SerializeField] float maxShield = 50.0f;

    [SerializeField] float maxTotalHp;
    [SerializeField] float curTotalHp;

    private void Awake()
    {
        rotDelta = rotSpeed * Time.deltaTime;
        curHp = maxHp;
        curArmor = maxArmor;
        curShield = maxShield;
        maxTotalHp = SetTotalMaxHp();
        curTotalHp = SetTotalCurHp();
    }

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine == true)
        {
            camPos.gameObject.SetActive(true);
        }
        characterController = GetComponent<CharacterController>();
        isAwayTeam = SetTeam();
        Debug.Log("isAwayTeam :" + isAwayTeam);
        if (isAwayTeam)
        {
            gameObject.tag = "RedTeam";
        }
        else
        {
            gameObject.tag = "BlueTeam";
        }
        Debug.Log("Current Player Count : " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    // Update is called once per frame
    void Update()
    {
        moving();
        Rotating();
        if (Input.GetKeyDown(KeyCode.G))
        {
            OnDamaged();
        }
    }

    float SetTotalMaxHp()
    {
        float res;

        res = maxHp + maxArmor + maxShield;

        return res;
    }

    float SetTotalCurHp()
    {
        float res;

        res = curHp + curArmor + curShield;

        return res;
    }

    //[PunRPC]
    bool SetTeam()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount==1)
        {
            return false;
        }

        return PhotonNetwork.CurrentRoom.PlayerCount % 2 == 0;
    }

    void moving()
    {
        if (photonView.IsMine == false)
            return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);

        dir = Camera.main.transform.TransformDirection(dir);

        yVelocity += gravity * Time.deltaTime;

        if (characterController.collisionFlags == CollisionFlags.Below)
        {
            yVelocity = 0;
            isJumping = false;
        }

        if (!isJumping && Input.GetButtonDown("Jump"))
        {
            yVelocity = jumpPower;
            isJumping = true;
        }

        dir.y = yVelocity;

        characterController.Move(dir * speed * Time.deltaTime);
    }

    void Rotating()
    {
        if (photonView.IsMine == false)
            return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotX += mouseX * rotDelta;
        rotY += mouseY * rotDelta;

        rotY = Mathf.Clamp(rotY, -90.0f, 90.0f);

        transform.localEulerAngles = new Vector3(0, rotX, 0);
        camPos.localEulerAngles = new Vector3(-rotY, 0, 0);
    }

    public void OnDamaged()
    {
        photonView.RPC("RpcOnDamaged", RpcTarget.All);
    }

    [PunRPC]
    void RpcOnDamaged()
    {
        if (curShield > 0)
        {

            if (takeDamage > curShield && curShield > 0 )
            {
                if(curArmor > 0)
                {
                    curArmor -= (takeDamage - curShield);
                    curShield -= takeDamage;
                    curTotalHp -= takeDamage;
                }

                else if (curArmor<=0)
                {
                    curHp -= (takeDamage - curShield);
                    curShield -= takeDamage;
                    curTotalHp -= takeDamage;
                }

                else if (curShield <= 0)
                {
                    curShield = 0;
                }
                
            }
                     
            else
            {
                curShield -= takeDamage;
                curTotalHp -= takeDamage;
            }
        }

        else if (curArmor > 0)
        {

            if (takeDamage > curArmor && curArmor > 0)
            {
                curHp -= (takeDamage - curArmor);
                curArmor -= takeDamage;
                curTotalHp -= takeDamage;
                if (curArmor <= 0) curArmor = 0;
            }

            else
            {
                curArmor -= takeDamage;
                curTotalHp -= takeDamage;
            }            
        }

        else
        {
            curHp -= takeDamage;
            curTotalHp -= takeDamage;
        }

        if (curHp <= 0)
        {
            curHp = 0;
            curTotalHp = 0;
            Debug.Log("Die...");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="OccupationZone"&&isAwayTeam==true)
        {
            GameManager.instance.playerRedCount++;
        }

        else if(other.gameObject.tag== "OccupationZone"&&isAwayTeam==false)
        {
            GameManager.instance.playerBlueCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "OccupationZone" && isAwayTeam == true)
        {
            GameManager.instance.playerRedCount--;
        }

        else if (other.gameObject.tag == "OccupationZone" && isAwayTeam == false)
        {
            GameManager.instance.playerBlueCount--;
        }
    }
}
