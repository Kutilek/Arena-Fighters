﻿using UnityEngine;

public class Sword_Slash : Ability_Traxh
{
    public bool attacking;
    [SerializeField] private float knockbackForce;
    private Damage damage;

    public void Awake()
    {
        damage = GetComponent<Damage>();
    }

    public override void Cast()
    {
        if (characterCombat.ableToCast && characterPhysics.currentGravityState == GravityState.Grounded)
        {
            animator.SetTrigger(animationCondition);
            characterCombat.attacking = true;
            characterCombat.ableToCast = false;
            attacking = true;
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
