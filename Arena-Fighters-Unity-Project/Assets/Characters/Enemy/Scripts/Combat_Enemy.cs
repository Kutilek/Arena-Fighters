﻿using UnityEngine;

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
            if (!effect.isStopped)
                effect.Stop();
        }
    }

    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }
}
