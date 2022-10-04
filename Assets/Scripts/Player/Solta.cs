using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Solta : PlayerBase
{
    [SerializeField] bool isAwayTeam;

    Animator animator; // �ִϸ��̼� �߰� by ����

    public override void Start()
    {
        base.Start();
        dealAmount = 40.0f;

        if (photonView.IsMine==false)
        {
            photonView.RPC("SetupCharacter", RpcTarget.All);
        }

        animator = GetComponent<Animator>(); // �ִϸ��̼� �߰� by ����
    }

    [PunRPC]
    public override void SetupCharacter()
    {
        base.SetupCharacter();
        //TODO:ĳ���� ���� �ʱ�ȭ(������ ���̺� Ȱ��)
    }

    [PunRPC]
    public override void useESkill()
    {
        Debug.Log("use ESkill..");
        //TODO:��ų ���� ���� ����
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
        //TODO:��ų ���� ���� ����
    }

    [PunRPC]
    public override void useUltimateSkill()
    {
        base.useUltimateSkill();
        //TODO:�ñر� ���� ���� ����
    }

    [PunRPC]
    public override void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!isDead) //����������� ������
            base.TakeDamage(damage, hitPoint, hitNormal);
    }

    [PunRPC]
    public override void TakeHeal(float healAmount)
    {
        if(!isDead) //����������� ���ޱ�
            base.TakeHeal(healAmount);
    }

    [PunRPC]
    public override void persistPassive()
    {
        base.persistPassive();
        //TODO:�нú� �ɷ� �����ϱ�(�нú갡 ���ٸ� �������� ����)
    }

    //by ����

    [PunRPC]
    public void OnCollisionEnter(Collision collision)
    {
        //�Ѿ� ������ ������ or �ױ� by ����
        if (collision.gameObject.tag == "Projectile") // ���� �� �Ѿ����� �Ǵ� �ʿ� by ����
        {
            TakeDamage(5, collision.contacts[0].point, collision.contacts[0].normal); // ������ ���� ��, �Ѿ� �浹 ��ġ, �浹 ��ġ �������� by ����
            if (curTotalHp == 0) //���� ��ü���� 0�̸� �������� �ִϸ��̼� by ����
            {
                animator.SetBool("Die", true);
                //this.gameObject.SetActive(false);
                isDead = true; //������ ������ ȭ������� ��� ��ƾ��ؼ� �����̿��� true�� �ٲ���� �� �����ϴ�. by ����
            }
            Destroy(collision.gameObject);
        }
        //�Ѿ� ������ �ױ� ��
    }

    [PunRPC]
    public void OnTriggerEnter(Collider other)
    {
        // ������ �ȿ� �ִ��� üũ�ؼ� ���ӸŴ��� �÷��̾�� ���� ���� by ����
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
        //������ üũ �ڵ� ��
    }

    private void OnTriggerExit(Collider other)
    {
        // ������ �ȿ� �ִ��� üũ�ؼ� ���ӸŴ��� �÷��̾�� ���� ���� by ����
        isAwayTeam = true; 

        if (other.gameObject.tag == "OccupationZone" && isAwayTeam == true)
        {
            print("OccupationZone out"); // �׽�Ʈ�� ���� ��� ���� �����߽��ϴ�.
            GameManager.instance.playerRedCount--;
        }
        else if (other.gameObject.tag == "OccupationZone" && isAwayTeam == false)
        {
            GameManager.instance.playerBlueCount--;
        }
        //������ üũ �ڵ� ��
    }
    // end
}
