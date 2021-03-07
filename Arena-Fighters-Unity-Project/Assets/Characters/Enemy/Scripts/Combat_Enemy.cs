using System.Collections;
using UnityEngine;

public class Combat_Enemy : Combat_Character
{
    protected ParticleSystem[] attackingEffects;
    protected Transform player;

    protected override void Awake()
    {
        base.Awake();
        attackingEffects = transform.Find("Armature").GetComponentsInChildren<ParticleSystem>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private float random;
    private float period = 3f;
    private float lastCall = 0f;

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

    

    public void Calculate()
    {
        if (lastCall + period <= Time.time)
        {
            random = Random.Range(1f, 100f);
            lastCall = Time.time;
        }
    }

    protected virtual void StartAttacking()
    {
        attacking = true;
        animator.SetBool("attacking", attacking);
        foreach (var effect in attackingEffects)
        {
            if (effect.isStopped)
            {
                Debug.Log("ANY JE NEJLEPSI");
                effect.Play(false);
            }
        }
            
    }

    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }
}
