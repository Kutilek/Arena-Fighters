using UnityEngine;

[RequireComponent(typeof(Physics_Character))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Damage))]
public abstract class Combat_Character : MonoBehaviour
{
    public bool ableToCast = true;
    public bool attacking;
    protected Animator animator;
    protected Physics_Character characterPhysics;
    protected ParticleSystem getHitEffect;
    
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        characterPhysics = GetComponent<Physics_Character>();
        getHitEffect = transform.Find("Hit_Effect").GetComponent<ParticleSystem>();
    }

    public virtual void GetHit()
    {
        animator.SetTrigger("gotHit");
        characterPhysics.SetMovementImpairingEffectStun();
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
