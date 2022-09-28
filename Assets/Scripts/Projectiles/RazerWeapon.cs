using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazerWeapon : ProjectileBase
{

    private void OnEnable()
    {
        SetAttackBox();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void SetProjectileStat()
    {
        
    }

    public override void flightProjectile()
    {
        
    }

    public override void SetAttackBox() => base.SetAttackBox(); 
}
