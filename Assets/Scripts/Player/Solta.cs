using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Solta : PlayerBase
{
    [SerializeField] bool isAwayTeam;

    Animator animator; // 애니메이션 추가 by 혜원

    public override void Start()
    {
        base.Start();
        dealAmount = 40.0f;

        if (photonView.IsMine==false)
        {
            photonView.RPC("SetupCharacter", RpcTarget.All);
        }

        animator = GetComponent<Animator>(); // 애니메이션 추가 by 혜원
    }

    [PunRPC]
    public override void SetupCharacter()
    {
        base.SetupCharacter();
        //TODO:캐릭터 스탯 초기화(데이터 테이블 활용)
    }

    [PunRPC]
    public override void useESkill()
    {
        Debug.Log("use ESkill..");
        //TODO:스킬 시전 로직 구현
    }

    [PunRPC]
    public override void useLeftClick()
    {
        Debug.Log("left Click..");

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            IDamageAndHealable target = hit.collider.GetComponent<IDamageAndHealable>();

            if(target!=null)
            {
                Debug.Log("Hit!");
                target.TakeDamage(dealAmount, hit.point, hit.normal);
            }
                                          
            //photonView.RPC("TakeDamage", RpcTarget.All, dealAmount, hit.point, hit.normal);
        }
    }

    [PunRPC]
    public override void useRightClickSkill()
    {
        base.useRightClickSkill();
        Debug.Log("right Click..");
        /*GameObject go = Instantiate(projectile);
            go.transform.position = firePos.position;
            go.transform.forward = firePos.forward;*/
        PhotonNetwork.Instantiate("Projectile", firePos.position, firePos.rotation);
    }

    [PunRPC]
    public override void useShiftSkill()
    {
        base.useShiftSkill();
        Debug.Log("use ShiftSkill..");
        //TODO:스킬 시전 로직 구현
    }

    [PunRPC]
    public override void useUltimateSkill()
    {
        base.useUltimateSkill();
        //TODO:궁극기 시전 로직 구현
    }

    [PunRPC]
    public override void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!isDead) //살아있을때만 데미지
            base.TakeDamage(damage, hitPoint, hitNormal);
    }

    [PunRPC]
    public override void TakeHeal(float healAmount)
    {
        if(!isDead) //살아있을때만 힐받기
            base.TakeHeal(healAmount);
    }

    [PunRPC]
    public override void persistPassive()
    {
        base.persistPassive();
        //TODO:패시브 능력 구현하기(패시브가 없다면 구현하지 않음)
    }

    //by 혜원

    [PunRPC]
    public void OnCollisionEnter(Collision collision)
    {
        //총알 맞으면 데미지 or 죽기 by 혜원
        if (collision.gameObject.tag == "Projectile") // 적이 쏜 총알인지 판단 필요 by 혜원
        {
            TakeDamage(5, collision.contacts[0].point, collision.contacts[0].normal); // 데미지 입은 값, 총알 충돌 위치, 충돌 위치 법선벡터 by 혜원
            if (curTotalHp == 0) //남은 총체력이 0이면 쓰러지기 애니메이션 by 혜원
            {
                animator.SetBool("Die", true);
                //this.gameObject.SetActive(false);
                isDead = true; //유아이 게이지 화면상으로 모두 닳아야해서 유아이에서 true로 바꿔야할 것 같습니다. by 혜원
            }
            Destroy(collision.gameObject);
        }
        //총알 맞으면 죽기 끝
    }

    [PunRPC]
    public void OnTriggerEnter(Collider other)
    {
        // 점령존 안에 있는지 체크해서 게임매니저 플레이어수 변수 조절 by 혜원
        isAwayTeam = true;

        if (other.gameObject.tag == "OccupationZone" && isAwayTeam == true)
        {
            print("OccupationZone Enter");
            GameManager.instance.playerRedCount++;
        }

        else if (other.gameObject.tag == "OccupationZone" && isAwayTeam == false)
        {
            GameManager.instance.playerBlueCount++;
        }
        //점령존 체크 코드 끝
    }

    private void OnTriggerExit(Collider other)
    {
        // 점령존 안에 있는지 체크해서 게임매니저 플레이어수 변수 조절 by 혜원
        isAwayTeam = true; 

        if (other.gameObject.tag == "OccupationZone" && isAwayTeam == true)
        {
            print("OccupationZone out"); // 테스트를 위해 잠시 팀을 지정했습니다.
            GameManager.instance.playerRedCount--;
        }
        else if (other.gameObject.tag == "OccupationZone" && isAwayTeam == false)
        {
            GameManager.instance.playerBlueCount--;
        }
        //점령존 체크 코드 끝
    }
    // end
}
