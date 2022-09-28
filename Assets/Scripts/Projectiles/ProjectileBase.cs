using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class ProjectileBase : MonoBehaviour
{
    protected GameObject projectile;
    protected string projectile_Index;
    protected string projectile_Name;
    protected string projectile_Type;
    protected string trajectory_Type;
    protected float damageAmount;
    protected float splashDamageAmount;
    protected float healAmount;
    protected float splashRange;
    protected float flightSpeed;
    protected float gravity;
    protected float deleteTime;
    protected int projectileCollision;
    protected Rigidbody rb;

    protected bool isDrop;
    protected bool onCritical;

    protected ParticleSystem flightEffect;
    protected ParticleSystem hitEffect;

    public abstract void SetProjectileStat();
    public abstract void flightProjectile();

    public virtual void SetAttackBox()
    {
        switch (projectileCollision)
        {
            case 1:
                break;
            
            case 2:
                break;

            case 3:

                break;
            case 4:

                break;
            case 5:
                gameObject.AddComponent<CapsuleCollider>();
                break;

            case 6:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO:충돌시 폭발 구현하기(직격, 스플래시 생기는 것도)    
        
    }
}
