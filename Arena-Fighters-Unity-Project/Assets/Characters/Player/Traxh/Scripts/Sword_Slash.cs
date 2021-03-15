using UnityEngine;

public class Sword_Slash : Ability_Traxh
{
    public bool attacking;
    [SerializeField] private float knockbackForce;
    private Damage damage;
    protected ParticleSystem[] swordSlashEffect;
    public bool swordSlashing;

    public void Awake()
    {
        damage = GetComponent<Damage>();
        swordSlashEffect = transform.Find("Skeleton").Find("Sword 1").Find("Sword_Slash_Effect").GetComponentsInChildren<ParticleSystem>();
    }

    public override void Cast()
    {
        if (characterCombat.ableToCast && characterPhysics.currentGravityState == GravityState.Grounded)
        {
            animator.SetTrigger(animationCondition);
            foreach (ParticleSystem ps in swordSlashEffect)
                ps.Play();
                
            characterCombat.attacking = true;
            characterCombat.ableToCast = false;
            swordSlashing = true;
            animator.SetFloat("walks", 0f);
        }
    }

    public void CheckHit(Collider hit)
    {
        if (swordSlashing)
        {
            float dealDamage = damage.GetAmount() * Random.Range(0.6f, 1.5f);
            if (hit.tag == "Enemy")
            {
                Physics_Character phyChar = hit.GetComponent<Physics_Character>();
                phyChar.AddOutsideForce(transform.forward, knockbackForce);
                swordSlashing = false;
                hit.transform.GetComponent<Combat_Character>().GetHit();
                hit.transform.GetComponent<Health>().DecreaseHealth(dealDamage);
            }
        }
    }

    private void SetSwordAttackingFalse()
    {
        swordSlashing = false;
        characterCombat.attacking = false;
    }

    private void SetSwordSlashingFalse()
    {
        swordSlashing = false;
    }
}
