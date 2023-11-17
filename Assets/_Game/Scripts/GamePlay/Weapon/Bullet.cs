using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : GameUnit
{
    public GameObject hitVFX;
    protected Character character;
    [SerializeField] protected float moveSpeed = 6f;
    protected bool isRunning;

    public virtual void OnInit(Character character, Vector3 target, float size)
    {
        this.character = character;
        TF.forward = (target - TF.position).normalized;
        isRunning = true;
    }

    public void OnDespawn()
    {
        SimplePool.Despawn(this);
    }

    protected virtual void OnStop() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_CHARACTER))
        {
            IHit hit = Cache.GetIHit(other);
            Instantiate(hitVFX, transform.position, transform.rotation);
            if (hit != null && hit != (IHit)character)
            {
                hit.OnHit(
                    ()=> { 
                        character.AddScore(1);
                        SimplePool.Despawn(this);
                    });
            }
        }

        if (other.CompareTag(Constant.TAG_BLOCK))
        {
            OnStop();
        }

    }
}
