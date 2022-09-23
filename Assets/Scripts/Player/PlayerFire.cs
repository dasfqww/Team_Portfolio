using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerFire : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject projectile;
    [SerializeField] Transform firePos;
    [SerializeField] GameObject impactVFX;

    PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            FireHitScan();
        }

        if (Input.GetMouseButtonDown(1))
        {
            /*GameObject go = Instantiate(projectile);
            go.transform.position = firePos.position;
            go.transform.forward = firePos.forward;*/
            PhotonNetwork.Instantiate("Projectile", firePos.position, firePos.rotation);
        }
    }

    void FireHitScan()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            PlayerMove pm = hit.transform.GetComponent<PlayerMove>();

            if (pm!=null)
            {
                pm.OnDamaged();
            }

            //RpcShowImpact(hit.point, hit.normal);
            photonView.RPC("RpcShowImpact", RpcTarget.All, hit.point, hit.normal);
        }
    }

    [PunRPC]
    void RpcShowImpact(Vector3 point, Vector3 normal)
    {
        GameObject vfx = Instantiate(impactVFX);
        vfx.transform.position = point;
        vfx.transform.forward = normal;
        Destroy(vfx, 2.0f);
    }
}
