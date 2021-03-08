using System.Collections;
using UnityEngine;

public class Combat_Enemy : Combat_Character
{
    protected ParticleSystem[] attackingEffects;

    protected override void Awake()
    {
        base.Awake();
        attackingEffects = transform.Find("Armature").GetComponentsInChildren<ParticleSystem>();
    }

    protected void Update()
    {
       /* Calculate();

        if (random >= 50f)
        {
            transform.LookAt(player);
            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        }*/
/*
        if (random > 50f)
        {
            StartAttacking();
        }
        else
        {
            attacking = false;
            animator.SetBool("attacking", attacking);
            foreach (var effect in attackingEffects)
                effect.Stop();
        }*/
    }

    public void StartAttackingState()
    {
        animator.SetBool("attacking", true);
        foreach (var effect in attackingEffects)
        {
            if (effect.isStopped)
                effect.Play(false);
        }
    }

    public void LeaveAttackingState()
    {
        animator.SetBool("attacking", false);
        foreach (var effect in attackingEffects)
        {
            if (effect.isStopped)
                effect.Stop(false);
        }
    }

    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }
}
