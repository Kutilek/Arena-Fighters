using UnityEngine;

public class Sword_Slash : Ability_Traxh
{
    public bool attacking;
    [SerializeField] private float knockbackForce;
    private Damage damage;
    protected ParticleSystem[] swordSlashEffect;

    public void Awake()
    {
        damage = GetComponent<Damage>();
        swordSlashEffect = transform.Find("Skeleton").Find("Sword 1").Find("Sword_Slash_Effect").GetComponentsInChildren<ParticleSystem>();
    }

    public override void Cast()
    {
        if (characterCombat.ableToCast && characterPhysics.currentGravityState == GravityState.Grounded)
        {   
           // characterPhysics.SetMovementImpairingEffectStun();
            animator.SetTrigger(animationCondition);
            foreach (ParticleSystem ps in swordSlashEffect)
                ps.Play();
                
            characterCombat.attacking = true;
            characterCombat.ableToCast = false;
            attacking = true;
            animator.SetFloat("walks", 0f);
        }
    }

    public void CheckHit(Collider hit)
    {
        if (attacking)
        {
            float dealDamage = damage.GetAmount() * Random.Range(0.2f, 1.5f);
            if (hit.tag == "Enemy")
            {
                Physics_Character phyChar = hit.GetComponent<Physics_Character>();
                phyChar.AddOutsideForce(transform.forward, knockbackForce);
                attacking = false;
                hit.transform.GetComponent<Combat_Character>().GetHit();
                hit.transform.GetComponent<Health>().DecreaseHealth(dealDamage);
            }
        }
    }

    private void SetSwordAttackingFalse()
    {
        animator.SetBool("swordAttacking", false);
    }
}
