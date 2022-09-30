using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Solta : PlayerBase
{
    public override void Start()
    {
        base.Start();
        dealAmount = 40.0f;

        if (photonView.IsMine==false)
        {
            photonView.RPC("SetupCharacter", RpcTarget.All);
        }
        SetupCharacter();
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
        base.TakeDamage(damage, hitPoint, hitNormal);
    }

    [PunRPC]
    public override void TakeHeal(float healAmount)
    {
        base.TakeHeal(healAmount);
    }

    [PunRPC]
    public override void persistPassive()
    {
        base.persistPassive();
        //TODO:�нú� �ɷ� �����ϱ�(�нú갡 ���ٸ� �������� ����)

    }

    
}
