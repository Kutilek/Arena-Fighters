using UnityEngine;

[RequireComponent(typeof(Physics_Character))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Damage))]
public abstract class Combat_Character : MonoBehaviour
{
    public bool ableToCast = true;
    public bool attacking;
    private bool isHit;
    protected Animator animator;
    protected Physics_Character characterPhysics;
    protected ParticleSystem getHitEffect;
    
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        characterPhysics = GetComponent<Physics_Character>();
        getHitEffect = transform.Find("Hit_Effect").GetComponent<ParticleSystem>();
    }

    public void CheckIfAttackign()
    {
        if (attacking)
        {
            animator.SetTrigger("exitSwordAttack");
            attacking = false;
        }
    }

    public virtual void GetHit()
    {
        animator.SetTrigger("gotHit"); 
        ableToCast = false;
    }

    // Animation Event
    private void SetAbleToCastTrue()
    {
        ableToCast = true;
    }

    private void PlayGetHitEffect()
    {
        getHitEffect.Play(false);
    }
}
